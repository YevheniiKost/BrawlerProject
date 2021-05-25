using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(Character))]
public class CharacterMovement : MonoBehaviour, IStunComponent
{
    [Header("Movenet stats")]
    [SerializeField] private float _initialMovementSpeed = 10f;

    [Header("Development")]
    [SerializeField] private Transform _targetSphere;

    [SerializeField] private ParticleSystem _slowDownParticles;

    public bool ForcedStop = false;
    public float NormilizedSpeed;
    public bool IsCharacterMoving => _isMoving;

    private float _currentMovementSpeed;

    private bool _isMoving;
    private Character _character;

    #region Stun
    public bool IsStunned => ForcedStop;

    public void SetIsStunned(bool value)
    {
        ForcedStop = value;
        StopMovement();
    }
    #endregion

    public void ModifyMovementSpeed(float percent)
    {
        if (_character.NavMeshAgent.enabled)
        {
            if (percent > 0)
                _slowDownParticles.gameObject.SetActive(true);

            _currentMovementSpeed = _initialMovementSpeed - percent * _initialMovementSpeed / 100;
            _character.NavMeshAgent.speed = _currentMovementSpeed;
            RecalculateNormalizedSpeed();
        }
    }

    public void ReturnNormalSpeed()
    {
        if (_character.NavMeshAgent.enabled)
        {
            _slowDownParticles.gameObject.SetActive(false);
            _character.NavMeshAgent.speed = _initialMovementSpeed;
            _currentMovementSpeed = _initialMovementSpeed;
            RecalculateNormalizedSpeed();
        }
    }

    #region AI movement
    public void SetTarget(Transform target)
    {
        if (_character.NavMeshAgent.enabled)
        {
            if (!ForcedStop && target != null)
            {
                _character.NavMeshAgent.isStopped = false;
                _character.NavMeshAgent.destination = target.position;

                _currentMovementSpeed = _character.NavMeshAgent.speed;
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
        if (_character.NavMeshAgent.enabled)
        {
            _character.NavMeshAgent.destination = transform.position;
            _character.NavMeshAgent.isStopped = true;

            _currentMovementSpeed = _character.NavMeshAgent.speed;
            RecalculateNormalizedSpeed();
        }
    }
    #endregion

    public void ProcessForcedStop() => ForcedStop = true;

    public void UndoForcedStop() => ForcedStop = false;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }
   
    private void Start()
    {
        _character.NavMeshAgent.speed = _initialMovementSpeed;
        RecalculateNormalizedSpeed();
    }

    private void Update()
    {
        if (_character.CharID.IsControlledByThePlayer && !ForcedStop && GetComponent<CharacterHealth>()?.GetLifeStatus() != LifeStatus.Dead)
            ProcessPlayerMovement();


        if (_character.NavMeshAgent.enabled)
            _isMoving = !_character.NavMeshAgent.isStopped;
    }

    private void ProcessPlayerMovement()
    {
        if (_character.NavMeshAgent.enabled)
        {
            if (_character.CharInput.MovementDirection != Vector3.zero)
            {
                _character.NavMeshAgent.isStopped = false;

                var newSpherePosition = transform.position + _character.CharInput.MovementDirection;
                _targetSphere.position = new Vector3(newSpherePosition.x, 0, newSpherePosition.z);

                if (!_targetSphere.GetComponent<TargetSphere>().IsInObstacle)
                    _character.NavMeshAgent.destination = _targetSphere.position;
            }
            else
            {
                _character.NavMeshAgent.destination = transform.position;
                _character.NavMeshAgent.isStopped = true;
                _targetSphere.position = new Vector3(transform.position.x, 0, transform.position.z);
            }

            _currentMovementSpeed = _character.NavMeshAgent.speed;
            RecalculateNormalizedSpeed();
        } 
    }

    private void RecalculateNormalizedSpeed()
    {
        NormilizedSpeed = _currentMovementSpeed / _initialMovementSpeed;
    }

    
}

