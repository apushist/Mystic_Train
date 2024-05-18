using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterFinder : MonoBehaviour
{
    [SerializeField] Transform _playerTarget;
    [SerializeField] bool _seePlayerAllTime = false;
    [SerializeField] bool _seePlayerNow = false;
    [SerializeField] bool _usePatrolPaths = true;
    [SerializeField] bool _canLoosePlayer = false;
    [SerializeField] float _timeToLoosePlayer = 2f;
    [SerializeField] float _baseSpeed;
    [SerializeField] float _rageSpeed;
    [SerializeField] Transform[] _patrolPoints;
    [SerializeField] AudioSource _roarAudioSource;
    [SerializeField] AudioClip _roarStartClip;
    [SerializeField] AudioClip _roarEndClip;
    [SerializeField] GameObject _deathEffect;
    [Header("FoundSettings")]
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _angleFound = 60;
    [SerializeField] float _rangeFound = 2;
    [SerializeField] float _absoluteRangeFound = 0.5f;
    [SerializeField] float _absoluteRangeFoundInRage = 2f;
    [SerializeField] int _rayCountFound = 5;

    private BGMController bgmc;
    private Animator animator;
    private Transform _currentPatrolPoint;
    private int _currentPatrolIndex = 0;

    private bool _isStanding = false;
    private float _globalTimeLooseAt = 0;

    private NavMeshAgent _agent;

    private AudioSource _audioSource;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = _seePlayerAllTime ? _rageSpeed : _baseSpeed;
        _audioSource = GetComponent<AudioSource>();
        bgmc = FindObjectOfType<BGMController>();

        if (_patrolPoints.Length == 0)
        {
            _usePatrolPaths = false;
        }
        else
        {
            _currentPatrolPoint = _patrolPoints[_currentPatrolIndex];
        }
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (ReachedDestination() && _audioSource.isPlaying)
            PlayFootSteps(false);
        else if (!ReachedDestination() && !_audioSource.isPlaying)
            PlayFootSteps(true);

        if (_seePlayerAllTime)
        {
            _agent.SetDestination(_playerTarget.position);
            if (ReachedDestination())
            {
                OnMonsterDeath();
            }
        }
        else if (_seePlayerNow)
        {
            _agent.SetDestination(_playerTarget.position);
            if (ReachedDestination())
            {
                OnMonsterDeath();
            }
            if (_canLoosePlayer)
            {
                if (TrySeePlayerInRage())
                {
                    _globalTimeLooseAt = Time.time;
                }
                if(Time.time - _globalTimeLooseAt > _timeToLoosePlayer)
                {
                    LoosePlayer();
                }
            }
        }
        else
        {
            if (TrySeePlayer())
            {
                StartMonsterRage();
            }
            else if (_usePatrolPaths)
            {
                if (!_isStanding)
                {
                    _agent.SetDestination(_currentPatrolPoint.position);
                    if (ReachedDestination())
                    {
                        _currentPatrolIndex++;
                        _currentPatrolPoint = _patrolPoints[_currentPatrolIndex % _patrolPoints.Length];
                        StartCoroutine(StayAtPoint(1));
                    }
                }
            }
        }
        UpdateAnimation();
    }

    bool ReachedDestination()
    {
        if (!_agent.pathPending)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
    bool TrySeePlayer()
    {
        RaycastHit2D preHit = Physics2D.Raycast(transform.position, _playerTarget.position - transform.position, _absoluteRangeFound, _layerMask);
        Debug.DrawLine(transform.position, transform.position + (_playerTarget.position - transform.position).normalized * _absoluteRangeFound, Color.red);/////////////
        if (preHit)
        {
            if (preHit.collider.CompareTag("Player"))
            {
                Debug.Log("found length");
                return true;
            }
        }
        float baseAngle = Mathf.Atan2(_agent.desiredVelocity.y, _agent.desiredVelocity.x) * Mathf.Rad2Deg - (_angleFound / 2);
        for (int i = 0; i < _rayCountFound; i++)
        {
            float curAngle = (baseAngle + _angleFound / (_rayCountFound - 1) * i) * Mathf.Deg2Rad;
            
            Vector2 curDir = new Vector2(Mathf.Cos(curAngle), Mathf.Sin(curAngle));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, curDir, _rangeFound, _layerMask);
            Debug.DrawLine(transform.position, transform.position + (new Vector3(curDir.x, curDir.y, 0)).normalized * _rangeFound);////////////////
            if (hit)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("found view");
                    return true;                   
                }
            }
        }

        return false;
    }
    bool TrySeePlayerInRage()
    {
        RaycastHit2D preHit = Physics2D.Raycast(transform.position, _playerTarget.position - transform.position, _absoluteRangeFoundInRage, _layerMask);
        Debug.DrawLine(transform.position, transform.position + (_playerTarget.position - transform.position).normalized * _absoluteRangeFoundInRage, Color.red);/////////////
        if (preHit)
        {
            if (preHit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }


    IEnumerator StayAtPoint(float seconds)
    {
        _isStanding = true;
        yield return new WaitForSeconds(seconds);
        _isStanding = false;
    }

    private void LoosePlayer()
    {
        _seePlayerNow = false;
        StopMonsterRage();
        _agent.SetDestination(_agent.transform.position);
        StartCoroutine(StayAtPoint(2));

        bgmc.SetVolume(0, 1f);
        bgmc.SetVolume(2, 0f);
        bgmc.RestartClip(0);
    }
    public void OnMonsterAlive()
    {
        gameObject.SetActive(true);
        if (_seePlayerAllTime || _seePlayerNow)
            StartMonsterRage();
        
    }
    public void OnMonsterDeath()
    {
        if (_seePlayerAllTime || _seePlayerNow)
        {
            StopMonsterRage();
            var deathVFX = Instantiate(_deathEffect, transform.position, Quaternion.identity);
            Destroy(deathVFX, 4);
        }
        bgmc.SetVolume(0, 1f);
        bgmc.SetVolume(2, 0f);
        bgmc.RestartClip(0);
        gameObject.SetActive(false);
    }
    
    public void StartMonsterRage()
    {
        StartRoarAudio();
        _seePlayerNow = true;
        _agent.speed = _rageSpeed;

        bgmc.SetVolume(2, 1f);
        bgmc.SetVolume(0, 0f);
        bgmc.RestartClip(2);
    }
    public void StopMonsterRage()
    {
        EndRoarAudio();
        _seePlayerNow = false;
        _agent.speed = _baseSpeed;
    }
    private void StartRoarAudio()
    {
        if (_roarAudioSource != null && _roarStartClip != null)
            _roarAudioSource.PlayOneShot(_roarStartClip);
    }
    private void EndRoarAudio()
    {
        if (_roarAudioSource != null && _roarEndClip != null)
            _roarAudioSource.PlayOneShot(_roarEndClip);
    }
    void PlayFootSteps(bool p)
    {
        if (p)
            _audioSource.Stop();
        else
            _audioSource.Play();
    }
    private void UpdateAnimation()
    {
        // Установка параметров анимации в зависимости от направления движения монстра
        Vector2 direction = _agent.desiredVelocity;

        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            animator.SetBool("up", false);
            animator.SetBool("down", false);
            animator.SetBool("right", direction.x > 0);
            animator.SetBool("left", direction.x < 0);
        }
        else
        {
            animator.SetBool("right", false);
            animator.SetBool("left", false);
            animator.SetBool("up", direction.y > 0);
            animator.SetBool("down", direction.y < 0);
        }
    }
}
