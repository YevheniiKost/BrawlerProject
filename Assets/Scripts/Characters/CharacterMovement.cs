using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent)), RequireComponent (typeof(CharacterPlayerInput)), RequireComponent (typeof(CharacterIdentifier))]
public class CharacterMovement : MonoBehaviour, IStunComponent
{
    [Header("Movenet stats")]
    [SerializeField] private float _initialMovementSpeed = 10f;

    [Header("Development")]
    [SerializeField] private Transform _targetSphere;

    public bool ForcedStop = false;
    public float NormilizedSpeed;
    public bool IsCharacterMoving => _isMoving;

    private float _currentMovementSpeed;

    private bool _isMoving;
    private NavMeshAgent _navMeshAgent;
    private CharacterPlayerInput _input;
    private CharacterIdentifier _iD;

    #region Stun
    public bool GetIsStunned() => ForcedStop;

    public void SetIsStunned(bool value)
    {
        ForcedStop = value;
        StopMovement();
    }
    #endregion

    public void ModifyMovementSpeed(float percent)
    {
        if (_navMeshAgent.enabled)
        {
            _currentMovementSpeed = _initialMovementSpeed - percent * _initialMovementSpeed / 100;
            _navMeshAgent.speed = _currentMovementSpeed;
            RecalculateNormalizedSpeed();
        }
    }

    public void ReturnNormalSpeed()
    {
        if (_navMeshAgent.enabled)
        {
            _navMeshAgent.speed = _initialMovementSpeed;
            _currentMovementSpeed = _initialMovementSpeed;
            RecalculateNormalizedSpeed();
        }
    }

    #region AI movement
    public void SetTarget(Transform target)
    {
        if (_navMeshAgent.enabled)
        {
            if (!ForcedStop && target != null)
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.destination = target.position;

                _currentMovementSpeed = _navMeshAgent.speed;
                RecalculateNormalizedSpeed();
            }
            else
            {
                StopMovement();
            }
        }
    }

    public void StopMovement()
    {
        if (_navMeshAgent.enabled)
        {
            _navMeshAgent.destination = transform.position;
            _navMeshAgent.isStopped = true;

            _currentMovementSpeed = _navMeshAgent.speed;
            RecalculateNormalizedSpeed();
        }
    }
    #endregion

    public void ProcessForcedStop() => ForcedStop = true;

    public void UndoForcedStop() => ForcedStop = false;

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
        if (_iD.IsControlledByThePlayer && !ForcedStop && GetComponent<CharacterHealth>()?.GetLifeStatus() != LifeStatus.Dead)
            ProcessPlayerMovement();
        else
            ProcessAIMovemet();

        if (_navMeshAgent.enabled)
            _isMoving = !_navMeshAgent.isStopped;
    }

    private void SetDependencies()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _iD = GetComponent<CharacterIdentifier>();
        _input = GetComponent<CharacterPlayerInput>();
    }

    private void ProcessPlayerMovement()
    {
        if (_navMeshAgent.enabled)
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
    }

    private void ProcessAIMovemet()
    {

    }

    private void RecalculateNormalizedSpeed()
    {
        NormilizedSpeed = _currentMovementSpeed / _initialMovementSpeed;
    }

    
}

