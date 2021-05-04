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

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _movement = GetComponent<CharacterMovement>();

    }

    private void Update()
    {
        Debug.Log(_navMeshAgent.isStopped);
    }
}
