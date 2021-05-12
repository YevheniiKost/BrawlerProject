using System;
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
    private int _characterAutoattackParamID;
    private int _characterFirstSkillParamID;
    private int _characterSecondSkillParamID;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _movement = GetComponent<CharacterMovement>();

        _characterLocomotionParamID = Animator.StringToHash("MovementSpeed");
        _characterAutoattackParamID = Animator.StringToHash("Autoattack");
        _characterFirstSkillParamID = Animator.StringToHash("FirstSkill");
        _characterSecondSkillParamID = Animator.StringToHash("SecondSkill");

        GetComponent<CharacterCombat>().AutoAttackWasUsed += UseAutoattack;
        GetComponent<CharacterCombat>().FirstSkillWasUsed += UseFirstSkill;
        GetComponent<CharacterCombat>().SecondSkillWasUsed += UseSecondSkill;

    }

    private void Update()
    {
        if (!_navMeshAgent.isStopped)
            _animator.SetFloat(_characterLocomotionParamID, _movement.NormilizedSpeed);
        else
            _animator.SetFloat(_characterLocomotionParamID, 0);
    }

    private void UseAutoattack()
    {
        _animator.SetTrigger(_characterAutoattackParamID);
    }

    private void UseFirstSkill()
    {
        _animator.SetTrigger(_characterFirstSkillParamID);
    }

    private void UseSecondSkill()
    {
        _animator.SetTrigger(_characterSecondSkillParamID);
    }
}
