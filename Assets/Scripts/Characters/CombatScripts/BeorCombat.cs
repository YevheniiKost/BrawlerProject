using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeorCombat : CharacterCombat
{
    [SerializeField] private float _rotationSpeed = 10f;

    [Header("Shield bash")]
    [SerializeField] private int _shieldBashDamage = 10;
    [SerializeField] private float _shieldBashRadius;
    [SerializeField] private float _shieldBashSlowingFactor;
    [SerializeField] private float _shieldBashSlowingDuaration;
    [SerializeField] private float _shieldBashCooldown;
    [SerializeField] private ShieldBashCollider _shieldBashCollider;

    [Header("Shield throw")]
    [SerializeField] private BeorShieldScript _shieldPrefab;
    [SerializeField] private Transform _shieldStartPoint;
    [SerializeField] private float _shieldThrowSpeed;
    [SerializeField] private float _shieldThrowDistance;
    [SerializeField] private int _shieldThrowDamage;
    [SerializeField] private float _shieldThrowStunDuration;
    [SerializeField] private float _shieldThrowCooldown = 10f;

    private Vector3 _firstSkillDirection = Vector3.zero;
    private Vector3 _secondSkillDirection = Vector3.zero;

    private float _timeToNextShieldBash = 0;
    private float _timeToNextShieldThrow = 0;

    #region Use of skills
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
        if (_timeToNextShieldBash >= _shieldBashCooldown)
        {
            if (_firstSkillDirection == Vector3.zero && _target != null)
            {
                transform.LookAt(_target);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, Mathf.Atan2(_firstSkillDirection.x, _firstSkillDirection.y) * Mathf.Rad2Deg, 0);
            }
            OnFirsSkillUse();
            _shieldBashCollider.gameObject.SetActive(true);
            _timeToNextShieldBash = 0;
        }
    }

    public override void UseSecondSkill()
    {
        if(_timeToNextShieldThrow >= _shieldThrowCooldown)
        {
            if (_firstSkillDirection == Vector3.zero && _target != null)
            {
                transform.LookAt(_target);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, Mathf.Atan2(_secondSkillDirection.x, _secondSkillDirection.y) * Mathf.Rad2Deg, 0);
            }
            OnSecondSkillUse();
            _timeToNextShieldThrow = 0;
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
    }

    private void Update()
    {
        _timeToNextAttack += Time.deltaTime;
        _timeToNextShieldBash += Time.deltaTime;
        _timeToNextShieldThrow += Time.deltaTime;
    }


    private void AutoAttackHit()
    {
        if (Target.TryGetComponent(out CharacterHealth enemy))
        {
            enemy.ModifyHealth(_autoAttackDamage);
            _timeToNextAttack = 0;
        }
    }

    private void FirstSkillHit()
    {
        var contacts = new List<CharacterIdentifier>(_shieldBashCollider.GetContacts());

        if (contacts.Count == 0)
        {
            return;
        }

        foreach (var contact in contacts)
        {
            if(contact.Team == this.GetComponent<CharacterIdentifier>().Team)
            {
                continue;
            }
            else
            {
                contact.GetComponent<CharacterHealth>().ModifyHealth(-_shieldBashDamage);
                _effectsManager.SnareEffect(contact, _shieldBashSlowingFactor, _shieldBashSlowingDuaration);
                Debug.Log($"{contact.gameObject.name} get damageded");
            }
        }
        _shieldBashCollider.ClearContacts();
        _shieldBashCollider.gameObject.SetActive(false);
    }

   private void SecondSkillHit()
    {
        var shield = Instantiate(_shieldPrefab, _shieldStartPoint.position + Vector3.down * .5f, Quaternion.identity);
        shield.GetData(_charID.Team, _shieldThrowDamage, _shieldThrowSpeed, _shieldThrowDistance, new Vector3(_secondSkillDirection.x, 0, _secondSkillDirection.y)); ;
    }

    private void AutoAttackHandler(object arg1, AutoattackEvent arg2)
    {
        AutoAttack();
    }

    private void FirstSkillInputHandler(object arg1, FirstSkillEvent skill)
    {
        if (_charID.IsControlledByThePlayer)
        {
            _firstSkillDirection = skill.Direction;
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
}
