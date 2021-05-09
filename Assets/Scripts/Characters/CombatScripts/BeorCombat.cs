using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeorCombat : CharacterCombat
{
    [SerializeField] private float _rotationSpeed = 10f;

    [Header("Shield bash")]
    [SerializeField] private float _shieldBashDamage = 10f;
    [SerializeField] private float _shieldBashRadius;
    [SerializeField] private float _shieldBashSlowingFactor;
    [SerializeField] private float _shieldBashSlowingDuaration;
    [SerializeField] private float _shieldBashCooldown;

    private Vector3 _firstSkillDirection;

    private float _timeToNextShieldBash = 0; 
    public override void AutoAttack()
    {
        if (_timeToNextAttack >= _autoAttackRate && Target != null)
        {
            if (Vector3.Distance(transform.position, Target.position) <= _autoAttackRange)
            {
                transform.LookAt(_target);
                OnAutoAttack();
                _timeToNextAttack = 0;
            } 
        }

    }
    public override void UseFirstSkill()
    {
        if(_timeToNextShieldBash >= _shieldBashCooldown)
        {
            OnFirsSkillUse();
            _timeToNextShieldBash = 0;
        }
    }

    protected override void AutoAttackHit()
    {
        if (Target.TryGetComponent(out CharacterHealth enemy))
        {
            enemy.ModifyHealth(_autoAttackDamage);
            _timeToNextAttack = 0;
        }
    }

    protected override void FirstSkillHit()
    {
        throw new NotImplementedException();
    }

    public override void UseSecondSkill()
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        EventAggregator.Subscribe<AutoattackEvent>(AutoAttackHandler);
        EventAggregator.Subscribe<FirstSkillEvent>(FirstSkillHandler);
    }

    private void FirstSkillHandler(object arg1, FirstSkillEvent skil)
    {
        if (skil.Direction != Vector3.zero)
            _firstSkillDirection = skil.Direction;
        else
            _firstSkillDirection = Vector3.forward;

        UseFirstSkill();
    }

    private void AutoAttackHandler(object arg1, AutoattackEvent arg2)
    {
        AutoAttack();
    }

    private void Update()
    {
        _timeToNextAttack += Time.deltaTime;
        _timeToNextShieldBash += Time.deltaTime;
    }

  
}
