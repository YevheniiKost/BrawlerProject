using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(CharacterMovement))]
public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private CharacterMovement _movement;
    private NavMeshAgent _navMeshAgent;

    private int _characterLocomotionParamID;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _movement = GetComponent<CharacterMovement>();

        _characterLocomotionParamID = Animator.StringToHash("MovementSpeed");
    }

    private void Update()
    {
        if (!_navMeshAgent.isStopped)
            _animator.SetFloat(_characterLocomotionParamID, _movement.NormilizedSpeed);
        else
            _animator.SetFloat(_characterLocomotionParamID, 0);
    }
}
