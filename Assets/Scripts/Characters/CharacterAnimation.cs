using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(Character))]
public class CharacterAnimation : MonoBehaviour, IStunComponent
{
    [SerializeField] private Animator _animator;

    private Character _character;

    private int _characterLocomotionParamID;
    private int _characterAutoattackParamID;
    private int _characterFirstSkillParamID;
    private int _characterSecondSkillParamID;
    private int _characterDeathParamID;
    private int _characterWakeUpParamID;
    private int _characterStunParamID;

    private bool _isStunned;

    #region stun
    public bool IsStunned => _isStunned;
    public void SetIsStunned(bool value) => SetStunnedAnimation(value);
    #endregion

    private void Awake()
    {
        _character = GetComponent<Character>();

        _characterLocomotionParamID = Animator.StringToHash("MovementSpeed");
        _characterAutoattackParamID = Animator.StringToHash("Autoattack");
        _characterFirstSkillParamID = Animator.StringToHash("FirstSkill");
        _characterSecondSkillParamID = Animator.StringToHash("SecondSkill");
        _characterDeathParamID = Animator.StringToHash("Death");
        _characterWakeUpParamID = Animator.StringToHash("WakeUp");
        _characterStunParamID = Animator.StringToHash("IsStunned");

        _character.CharCombat.AutoAttackWasUsed += UseAutoattack;
        _character.CharCombat.FirstSkillWasUsed += UseFirstSkill;
        _character.CharCombat.SecondSkillWasUsed += UseSecondSkill;

        EventAggregator.Subscribe<CharacterDeath>(OnCharacterDeathHandler);
        EventAggregator.Subscribe<CharacterWakeUp>(OnCharacterWakeUpHandler);
    }


    private void OnDestroy()
    {
        _character.CharCombat.AutoAttackWasUsed -= UseAutoattack;
        _character.CharCombat.FirstSkillWasUsed -= UseFirstSkill;
        _character.CharCombat.SecondSkillWasUsed -= UseSecondSkill;

        EventAggregator.Unsubscribe<CharacterDeath>(OnCharacterDeathHandler);
        EventAggregator.Unsubscribe<CharacterWakeUp>(OnCharacterWakeUpHandler);
    }

    private void Update()
    {
        if (_character.NavMeshAgent.enabled)
        {
            if (!_character.NavMeshAgent.isStopped)
                _animator.SetFloat(_characterLocomotionParamID, _character.CharMovement.NormilizedSpeed);
            else
                _animator.SetFloat(_characterLocomotionParamID, 0);
        }
    }

    private void OnCharacterDeathHandler(object character, CharacterDeath death)
    {
        if (death.Character == _character.CharHealth)
        {
            CharacterDeath();
        }
    }

    private void OnCharacterWakeUpHandler(object arg1, CharacterWakeUp data)
    {
        if(data.Character == _character.CharHealth)
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
