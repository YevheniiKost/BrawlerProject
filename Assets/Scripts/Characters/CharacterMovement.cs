using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent)), RequireComponent (typeof(CharacterPlayerInput))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movenet stats")]
    [SerializeField] private float _initialMovementSpeed = 10f;

    [Header("Development")]
    [SerializeField] private Transform _targetSphere;

    public bool IsControlledByThePlayer;

    private float _currentMovementSpeed;

    private NavMeshAgent _navMeshAgent;
    private CharacterPlayerInput _input;
    private Vector3 _movemetDirection = Vector3.zero;

    public void ModifyMovementSpeed(float percent)
    {
        var newSpeed = _initialMovementSpeed - percent * _initialMovementSpeed / 100;
        _navMeshAgent.speed = newSpeed;
    }

    public void ReturnNormalSpeed()
    {
        _navMeshAgent.speed = _initialMovementSpeed;
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        if (IsControlledByThePlayer)
            _input = GetComponent<CharacterPlayerInput>();

        if (IsControlledByThePlayer)
            _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        else
            _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
    }

    private void Start()
    {
        _navMeshAgent.speed = _initialMovementSpeed;
    }

    private void Update()
    {
        if (IsControlledByThePlayer)
            ProcessPlayerMovement();
        else
            ProcessAIMovemet();

    }

    private void ProcessPlayerMovement()
    {
        if (_input.VerticalInput != 0 || _input.HorizontalInput != 0)
        {
            _navMeshAgent.isStopped = false;
            _movemetDirection = new Vector3(_input.HorizontalInput, 0, _input.VerticalInput);
            var newSpherePosition = transform.position + _movemetDirection.normalized;
            _targetSphere.position = new Vector3(newSpherePosition.x, 0, newSpherePosition.z);
            if (!_targetSphere.GetComponent<TargetSphere>().IsInObstacle)
                _navMeshAgent.destination = _targetSphere.position;
        }
        else
        {
            _navMeshAgent.destination = transform.position;
            _movemetDirection = Vector3.zero;
            _navMeshAgent.isStopped = true;
            _targetSphere.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }


    private void ProcessAIMovemet()
    {

    }
}

