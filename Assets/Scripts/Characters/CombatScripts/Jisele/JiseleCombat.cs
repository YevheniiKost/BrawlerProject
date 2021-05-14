using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiseleCombat : CharacterCombat
{
    [SerializeField] private float _autoattackFlyDistance;
    [SerializeField] private float _autoattackFlySpeed;
    [SerializeField] private JiseleAutoattackSphere _autoattackSphere;
    [SerializeField] private Transform _autoattackStartPosition;

    private Vector3 _autoattackDirection = Vector3.zero;
    private Vector3 _firstSkillDirection = Vector3.zero;
    private Vector3 _secondSkillDirection = Vector3.zero;

    public override void AttackBehavior()
    {
        throw new System.NotImplementedException();
    }

    #region Using of skills
    public override void AutoAttack()
    {
        if (_timeToNextAttack >= _autoAttackRate && Target != null && !_isStunned)
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
        throw new System.NotImplementedException();
    }

    public override void UseSecondSkill()
    {
        throw new System.NotImplementedException();
    }

    #endregion

    private void Awake()
    {
        _animEventHandler.OnAutoattackHit += AutoAttackHit;
        _animEventHandler.OnFirstSkillHit += FirstSkillHit;
        _animEventHandler.OnSecondSkillHit += SecondSkillHit;

        EventAggregator.Subscribe<AutoattackEvent>(AutoAttackHandler);
        EventAggregator.Subscribe<FirstSkillEvent>(FirstSkillInputHandler);
        EventAggregator.Subscribe<SecondSkillEvent>(SecondSkillInputHandler);
    }

    private void OnDestroy()
    {
        _animEventHandler.OnAutoattackHit -= AutoAttackHit;
        _animEventHandler.OnFirstSkillHit -= FirstSkillHit;
        _animEventHandler.OnSecondSkillHit -= SecondSkillHit;
    }

    private void Update()
    {
        _timeToNextAttack += Time.deltaTime;

        if (_firstAbilityCooldownTimer > 0)
        {
            _firstAbilityCooldownTimer -= Time.deltaTime;
        }
        else
        {
            _firstAbilityCooldownTimer = 0;
        }

        if (_secondAbilityCooldownTimer > 0)
        {
            _secondAbilityCooldownTimer -= Time.deltaTime;
        }
        else
        {
            _secondAbilityCooldownTimer = 0;
        }
    }


    private void AutoAttackHit()
    {
      if(_target != null)
        {
            var ball = Instantiate(_autoattackSphere, _autoattackStartPosition.position, Quaternion.identity);

            _autoattackDirection = _target.transform.position - ball.transform.position;
            _autoattackDirection.y = 0;
            ball.SetData(_autoattackDirection, _autoAttackDamage, _charID, _autoattackFlySpeed, _autoattackFlyDistance);
        }
    }

    private void FirstSkillHit()
    {
        throw new NotImplementedException();
    }

    private void SecondSkillHit()
    {
        throw new NotImplementedException();
    }

    private void AutoAttackHandler(object arg1, AutoattackEvent arg2)
    {
        if (_charID.IsControlledByThePlayer)
            AutoAttack();
    }

    private void FirstSkillInputHandler(object arg1, FirstSkillEvent arg2)
    {
        throw new NotImplementedException();
    }

    private void SecondSkillInputHandler(object arg1, SecondSkillEvent arg2)
    {
        throw new NotImplementedException();
    }
}
