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

    [Header("Fireball")]
    [SerializeField] private int _fireaballDamage;
    [SerializeField] private float _fireballFlyDistance;
    [SerializeField] private float _fireballSpeed;
    [SerializeField] private float _fireballCooldown;
    [SerializeField] private JiseleFireball _fireballPrefab;
    [SerializeField] private Transform _fireballStartPoint;

    [Header("Meteor")]
    [SerializeField] private float _secondAbilityRadius;
    [SerializeField] private float _secondAbilityCooldown;
    [SerializeField] private int _meteorExplosionDamage;
    [SerializeField] private float _meteorExplosionRadius;
    [SerializeField] private int _fireAreaDamage;
    [SerializeField] private float _fireAreaDamageRate;
    [SerializeField] private float _fireAreaRadius;
    [SerializeField] private float _fireAreaLifeTime;
    [SerializeField] private JiseleMeteor _meteorPrefab;

    private Vector3 _autoattackDirection = Vector3.zero;
    private Vector3 _firstSkillDirection = Vector3.zero;
    private Vector3 _secondSkillDirection = Vector3.zero;

    public override void AttackBehavior()
    {
        if (_secondAbilityCooldownTimer == 0)
        {
            UseSecondSkill();
        }
        else if (_firstAbilityCooldownTimer == 0)
        {
            UseFirstSkill();
        }
        else
        {
            AutoAttack();
        }
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
        if (_firstAbilityCooldownTimer == 0 && !_isStunned)
        {
            if (_firstSkillDirection == Vector3.zero)
            {
                if (_target != null)
                {
                    _firstSkillDirection = (_target.transform.position - transform.position).normalized;
                    transform.LookAt(_target);
                }
                else
                {
                    _firstSkillDirection = Vector3.forward;
                }
            }
            transform.localRotation = Quaternion.Euler(0, Mathf.Atan2(_firstSkillDirection.x, _firstSkillDirection.z) * Mathf.Rad2Deg, 0);
            OnFirstSkillUse();
            GetComponent<CharacterMovement>()?.ProcessForcedStop();
           
        }
    }

    public override void UseSecondSkill()
    {
        if (_secondAbilityCooldownTimer == 0 && !_isStunned)
        {
            if(_secondSkillDirection == Vector3.zero)
            {
                if(_target != null && Vector3.Distance(transform.position, _target.transform.position) <= _secondAbilityRadius)
                {
                    _secondSkillDirection = _target.transform.position - transform.position;
                }
            }
            else
            {
                var direction = new Vector3(_secondSkillDirection.x, 0, _secondSkillDirection.y);
                _secondSkillDirection = direction * _secondAbilityRadius;
            }
            OnSecondSkillUse();
            GetComponent<CharacterMovement>()?.ProcessForcedStop();
        }
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

        EventAggregator.Unsubscribe<AutoattackEvent>(AutoAttackHandler);
        EventAggregator.Unsubscribe<FirstSkillEvent>(FirstSkillInputHandler);
        EventAggregator.Unsubscribe<SecondSkillEvent>(SecondSkillInputHandler);
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
            ball.SetData(_autoattackDirection.normalized, _autoAttackDamage, _charID, _autoattackFlySpeed, _autoattackFlyDistance);
        }
    }

    private void FirstSkillHit()
    {
        var fireBall = Instantiate(_fireballPrefab, _fireballStartPoint.position, Quaternion.identity);
        fireBall.SetData(_fireaballDamage, _fireballSpeed, _fireballFlyDistance, _firstSkillDirection, _charID);
        FirstAbilityUsedEvent(_fireballCooldown);
        _firstAbilityCooldownTimer = _fireballCooldown;
        GetComponent<CharacterMovement>()?.UndoForcedStop();
    }

    private void SecondSkillHit()
    {
        var meteor = Instantiate(_meteorPrefab, transform.position + _secondSkillDirection, Quaternion.identity);
        meteor.GetData(_meteorExplosionDamage, _fireAreaDamage, _meteorExplosionRadius, _fireAreaDamageRate, _fireAreaDamageRate, _fireAreaLifeTime, _charID);
        SecondAvilityUsedEvent(_secondAbilityCooldown);
        _secondAbilityCooldownTimer = _secondAbilityCooldown;
        GetComponent<CharacterMovement>()?.UndoForcedStop();
    }

    #region Skills player input
    private void AutoAttackHandler(object arg1, AutoattackEvent arg2)
    {
        if (_charID.IsControlledByThePlayer)
            AutoAttack();
    }

    private void FirstSkillInputHandler(object arg1, FirstSkillEvent skill)
    {
        if (_charID.IsControlledByThePlayer)
        {
            _firstSkillDirection = new Vector3(skill.Direction.x, 0, skill.Direction.y);
            UseFirstSkill();
        }
    }

    private void SecondSkillInputHandler(object arg1, SecondSkillEvent skill)
    {
        if (_charID.IsControlledByThePlayer)
        {
            _secondSkillDirection = skill.Direction;
            UseSecondSkill();
        }
    }

    #endregion
}
