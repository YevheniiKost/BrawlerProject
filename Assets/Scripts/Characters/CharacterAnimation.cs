using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(CharacterMovement)), RequireComponent(typeof(CharacterHealth))]
public class CharacterAnimation : MonoBehaviour, IStunComponent
{
    [SerializeField] private Animator _animator;

    private CharacterMovement _movement;
    private CharacterHealth _health;
    private NavMeshAgent _navMeshAgent;

    private int _characterLocomotionParamID;
    private int _characterAutoattackParamID;
    private int _characterFirstSkillParamID;
    private int _characterSecondSkillParamID;
    private int _characterDeathParamID;
    private int _characterWakeUpParamID;
    private int _characterStunParamID;

    private bool _isStunned;

    #region stun
    public bool GetIsStunned()
    {
        return _isStunned;
    }

    public void SetIsStunned(bool value)
    {
        SetStunnedAnimation(value);
    }

    #endregion

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _movement = GetComponent<CharacterMovement>();
        _health = GetComponent<CharacterHealth>();

        _characterLocomotionParamID = Animator.StringToHash("MovementSpeed");
        _characterAutoattackParamID = Animator.StringToHash("Autoattack");
        _characterFirstSkillParamID = Animator.StringToHash("FirstSkill");
        _characterSecondSkillParamID = Animator.StringToHash("SecondSkill");
        _characterDeathParamID = Animator.StringToHash("Death");
        _characterWakeUpParamID = Animator.StringToHash("WakeUp");
        _characterStunParamID = Animator.StringToHash("IsStunned");

        GetComponent<CharacterCombat>().AutoAttackWasUsed += UseAutoattack;
        GetComponent<CharacterCombat>().FirstSkillWasUsed += UseFirstSkill;
        GetComponent<CharacterCombat>().SecondSkillWasUsed += UseSecondSkill;

        EventAggregator.Subscribe<CharacterDeath>(OnCharacterDeathHandler);
        EventAggregator.Subscribe<CharacterWakeUp>(OnCharacterWakeUpHandler);

    }


    private void OnDestroy()
    {
        GetComponent<CharacterCombat>().AutoAttackWasUsed -= UseAutoattack;
        GetComponent<CharacterCombat>().FirstSkillWasUsed -= UseFirstSkill;
        GetComponent<CharacterCombat>().SecondSkillWasUsed -= UseSecondSkill;
    }

    private void Update()
    {
        if (!_navMeshAgent.isStopped)
            _animator.SetFloat(_characterLocomotionParamID, _movement.NormilizedSpeed);
        else
            _animator.SetFloat(_characterLocomotionParamID, 0);
    }

    private void OnCharacterDeathHandler(object character, CharacterDeath death)
    {
        if (death.Character == _health)
        {
            CharacterDeath();
        }
    }

    private void OnCharacterWakeUpHandler(object arg1, CharacterWakeUp data)
    {
        if(data.Character == _health)
        {
            CharacterWakeUp();
        }
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

    private void CharacterDeath()
    {
        _animator.SetTrigger(_characterDeathParamID);
    }

    private void CharacterWakeUp()
    {
        _animator.SetTrigger(_characterWakeUpParamID);
    }

    private void SetStunnedAnimation(bool value)
    {
        _animator.SetBool(_characterStunParamID, value);
    }
}
