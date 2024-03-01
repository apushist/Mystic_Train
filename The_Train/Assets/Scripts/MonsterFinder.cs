using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterFinder : MonoBehaviour
{
    [SerializeField] Transform _playerTarget;
    [SerializeField] bool _seePlayerAllTime = false;
    [SerializeField] bool _seePlayerNow = false;
    [SerializeField] Transform[] _patrolPoints;


    private Transform _currentPatrolPoint;
    private int _currentPatrolIndex = 0;

    private NavMeshAgent _agent;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _currentPatrolPoint = _patrolPoints[_currentPatrolIndex];
    }

    void Update()
    {
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
            if (!_agent.pathPending)
            {
                if (_agent.remainingDistance <= _agent.stoppingDistance)
                {
                    if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                    {
                        _currentPatrolIndex++;
                        _currentPatrolPoint = _patrolPoints[_currentPatrolIndex % _patrolPoints.Length];
                    }
                }
            }
            _agent.SetDestination(_currentPatrolPoint.position);
        }
    }

}
