using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeorCombat : CharacterCombat
{
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

    #region Use of skills
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
            if (_firstSkillDirection == Vector3.zero && _target != null)
            {
                transform.LookAt(_target);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, Mathf.Atan2(_firstSkillDirection.x, _firstSkillDirection.y) * Mathf.Rad2Deg, 0);
            }
            OnFirstSkillUse();
            FirstAbilityUsedEvent(_shieldBashCooldown);
            _shieldBashCollider.gameObject.SetActive(true);
            _firstAbilityCooldownTimer = _shieldBashCooldown;
        }
    }

    public override void UseSecondSkill()
    {
        if(_secondAbilityCooldownTimer == 0 && !_isStunned)
        {
            if (_secondSkillDirection == Vector3.zero)
            {
                if (_target != null)
                {
                    var dirToTarget = _target.transform.position - transform.position;
                    _secondSkillDirection = new Vector3(dirToTarget.x, dirToTarget.z, 0).normalized;
                    transform.LookAt(_target);
                }
                else
                {
                    _secondSkillDirection = Vector3.up;
                }
            }
          
             transform.localRotation = Quaternion.Euler(0, Mathf.Atan2(_secondSkillDirection.x, _secondSkillDirection.y) * Mathf.Rad2Deg, 0);
             OnSecondSkillUse();
             SecondAvilityUsedEvent(_shieldThrowCooldown);
             GetComponent<CharacterMovement>()?.ProcessForcedStop();
            _secondAbilityCooldownTimer = _shieldThrowCooldown;
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

        if(_firstAbilityCooldownTimer > 0)
        {
            _firstAbilityCooldownTimer -= Time.deltaTime;
        }
        else
        {
            _firstAbilityCooldownTimer = 0;
        }

        if(_secondAbilityCooldownTimer > 0)
        {
            _secondAbilityCooldownTimer -= Time.deltaTime;
        }
        else
        {
            _secondAbilityCooldownTimer = 0;
        }
    }

    #region Skills hits
    private void AutoAttackHit()
    {
        if (Target.TryGetComponent(out CharacterHealth enemy))
        {
            enemy.ModifyHealth(-_autoAttackDamage, _charID);
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
                contact.GetComponent<CharacterHealth>().ModifyHealth(-_shieldBashDamage, _charID);
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
        shield.GetData(_shieldThrowDamage, _shieldThrowSpeed, _shieldThrowDistance, new Vector3(_secondSkillDirection.x, 0, _secondSkillDirection.y).normalized, _charID, _shieldThrowStunDuration);
        GetComponent<CharacterMovement>()?.UndoForcedStop();
    }

    #endregion

    #region Player input handler
    private void AutoAttackHandler(object arg1, AutoattackEvent arg2)
    {
        if (_charID.IsControlledByThePlayer)
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

    #endregion
}
