using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class MonsterFinder : MonoBehaviour
{
    [SerializeField] Transform _playerTarget;
    [SerializeField] bool _seePlayerAllTime = false;
    [SerializeField] bool _seePlayerNow = false;
    [SerializeField] bool _usePatrolPaths = true;
    [SerializeField] float _baseSpeed;
    [SerializeField] Transform[] _patrolPoints;

    [Header("FoundSettings")]
    [SerializeField] LayerMask _layerMask;
    [SerializeField] float _angleFound = 60;
    [SerializeField] float _rangeFound = 2;
    [SerializeField] float _absoluteRangeFound = 0.5f;
    [SerializeField] int _rayCountFound = 5;
    [SerializeField] float _rageSpeed;


    private Transform _currentPatrolPoint;
    private int _currentPatrolIndex = 0;

    private bool _isStanding = false;

    private NavMeshAgent _agent;

    private AudioSource _audioSource;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = _seePlayerAllTime ? _rageSpeed : _baseSpeed;
        _audioSource = GetComponent<AudioSource>();     

        if (_patrolPoints.Length == 0)
        {
            _usePatrolPaths = false;
        }
        else
        {
            _currentPatrolPoint = _patrolPoints[_currentPatrolIndex];
        }
    }

    void Update()
    {
        if (ReachedDestination() && _audioSource.isPlaying)
            PlayFootSteps(false);
        else if(!ReachedDestination() && !_audioSource.isPlaying)
            PlayFootSteps(true);


        if (_seePlayerAllTime)
        {
            _agent.SetDestination(_playerTarget.position);
        }
        else if(_seePlayerNow)
        {
            _agent.SetDestination(_playerTarget.position);
        }
        else
        {
            if (TrySeePlayer())
            {
                _seePlayerNow = true;
                _agent.speed = _rageSpeed;
            }
            else if (_usePatrolPaths)
            {
                if(!_isStanding)
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

    IEnumerator StayAtPoint(float t)
    {
        _isStanding = true;
        yield return new WaitForSeconds(t);
        _isStanding = false;
    }

    void PlayFootSteps(bool p)
    {
        if (p)
            _audioSource.UnPause();
        else
            _audioSource.Pause();
    }

}
