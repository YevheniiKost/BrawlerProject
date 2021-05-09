using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent)), RequireComponent (typeof(CharacterPlayerInput)), RequireComponent (typeof(CharacterIdentifier))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movenet stats")]
    [SerializeField] private float _initialMovementSpeed = 10f;

    [Header("Development")]
    [SerializeField] private Transform _targetSphere;

    
    public float NormilizedSpeed;
    public bool IsCharacterMoving => _isMoving;

    private float _currentMovementSpeed;

    private bool _isMoving;
    private NavMeshAgent _navMeshAgent;
    private CharacterPlayerInput _input;
    private CharacterIdentifier _iD;

    public void ModifyMovementSpeed(float percent)
    {
        _currentMovementSpeed = _initialMovementSpeed - percent * _initialMovementSpeed / 100;
        _navMeshAgent.speed = _currentMovementSpeed;
        RecalculateNormalizedSpeed();
    }

    public void ReturnNormalSpeed()
    {
        _navMeshAgent.speed = _initialMovementSpeed;
        _currentMovementSpeed = _initialMovementSpeed;
        RecalculateNormalizedSpeed();
    }

    public void SetTarget(Transform target)
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.destination = target.position;

        _currentMovementSpeed = _navMeshAgent.speed;
        RecalculateNormalizedSpeed();
    }

    public void StopMovement()
    {
        _navMeshAgent.destination = transform.position;
        _navMeshAgent.isStopped = true;

        _currentMovementSpeed = _navMeshAgent.speed;
        RecalculateNormalizedSpeed();
    }

    private void Awake()
    {
        SetDependencies();
    }
   
    private void Start()
    {
        _navMeshAgent.speed = _initialMovementSpeed;
        RecalculateNormalizedSpeed();
    }

    private void Update()
    {
        if (_iD.IsControlledByThePlayer)
            ProcessPlayerMovement();
        else
            ProcessAIMovemet();

        _isMoving = !_navMeshAgent.isStopped; 
    }

    private void SetDependencies()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _iD = GetComponent<CharacterIdentifier>();
        if (_iD.IsControlledByThePlayer)
            _input = GetComponent<CharacterPlayerInput>();
    }

    private void ProcessPlayerMovement()
    {
        if (_input.MovementDirection != Vector3.zero)
        {
            _navMeshAgent.isStopped = false;

            var newSpherePosition = transform.position + _input.MovementDirection;
            _targetSphere.position = new Vector3(newSpherePosition.x, 0, newSpherePosition.z);

            if (!_targetSphere.GetComponent<TargetSphere>().IsInObstacle)
                _navMeshAgent.destination = _targetSphere.position;
        }
        else
        {
            _navMeshAgent.destination = transform.position;
            _navMeshAgent.isStopped = true;
            _targetSphere.position = new Vector3(transform.position.x, 0, transform.position.z);
        }

        _currentMovementSpeed = _navMeshAgent.speed;
        RecalculateNormalizedSpeed();
    }

    private void ProcessAIMovemet()
    {

    }

    private void RecalculateNormalizedSpeed()
    {
        NormilizedSpeed = _currentMovementSpeed / _initialMovementSpeed;
    }
}

