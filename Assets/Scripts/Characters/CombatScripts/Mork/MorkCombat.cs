using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MorkCombat : CharacterCombat
{
    private const float _smallValue = .02f;

    [Header("EarthQuake")]
    [SerializeField] private int _firstSkillDamage;
    [SerializeField] private float _firstSkillRadius;
    [SerializeField] private float _firstSkillSlowdownEffect;
    [SerializeField] private float _firstSkillSlowdownDuration;
    [SerializeField] private float _firstSkillCooldown;
    [SerializeField] private Transform _firstSkillParticles;

    [Header("Rock jump")]
    [SerializeField] private float _secondSkillCooldown;
    [SerializeField] private float _secondSkillJumpDistance;
    [SerializeField] private int _secondSkillDamage;
    [SerializeField] private float _secondSkillDamageRadius;
    [SerializeField] private float _secondSkillStunDuration;
    [SerializeField] private float _secondSkillJumpTime;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private Transform _secondSkillParticles;

    private Vector3 _secondSkillDirection = Vector3.zero;

 

    public override void AttackBehavior()
    {
        if (!_isStunned)
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
                _audioManager.PlaySFX(SoundsFx.Mork01Hit);
                _timeToNextAttack = 0;
            }
        }
    }

    public override void UseFirstSkill()
    {
        if(_firstAbilityCooldownTimer == 0)
        {
            OnFirstSkillUse();
            FirstAbilityUsedEvent(_firstSkillCooldown);
            _firstAbilityCooldownTimer = _firstSkillCooldown;
        }
    }

    public override void UseSecondSkill()
    {
        if (_secondAbilityCooldownTimer == 0 && !_isStunned)
        {
            if (_secondSkillDirection == Vector3.zero)
            {
                if (_target != null && Vector3.Distance(transform.position, _target.transform.position) <= _secondSkillJumpDistance)
                {
                    _secondSkillDirection = _target.transform.position;
                }
                else
                {
                    _secondSkillDirection = transform.position + Vector3.forward * _secondSkillJumpDistance;
                }
            }
            else
            {
                var direction = new Vector3(_secondSkillDirection.x, 0, _secondSkillDirection.y);
                _secondSkillDirection = transform.position + (direction * _secondSkillJumpDistance);
            }
            transform.LookAt(_secondSkillDirection);
            StartCoroutine(MorkJump(SecondSkillDirection(_secondSkillDirection), _secondSkillJumpTime));
            OnSecondSkillUse();
            SecondAbilityUsedEvent(_secondSkillCooldown);
            _secondAbilityCooldownTimer = _secondSkillCooldown;
            _character.CharMovement.ProcessForcedStop();
        }
    }

    private IEnumerator MorkJump(Vector3 secondSkillDirection, float secondSkillJumpTime)
    {
        Vector3 p1 = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 p2 = new Vector3(transform.position.x, _jumpHeight, transform.position.z);
        Vector3 p3 = new Vector3(secondSkillDirection.x, _jumpHeight, secondSkillDirection.z);
        Vector3 p4 = new Vector3(secondSkillDirection.x, 0, secondSkillDirection.z);

        float tParam = 0;

        _character.NavMeshAgent.enabled = false;
        while (tParam <= 1)
        {
            tParam += Time.deltaTime * _secondSkillJumpTime;

            Vector3 newPos = Mathf.Pow(1 - tParam, 3) * p1 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p2 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p3 +
                Mathf.Pow(tParam, 3) * p4;

            transform.position = newPos;
            yield return new WaitForEndOfFrame();
        }
        if (_character.CharID.IsControlledByThePlayer)
            EventAggregator.Post(this, new ShakeCamera { Intencity = 5, Time = .5f });
        transform.position = secondSkillDirection;
        Instantiate(_secondSkillParticles, transform.position + Vector3.up * .02f, Quaternion.identity);
        _audioManager.PlaySFX(SoundsFx.Mork03Hit);
        _character.NavMeshAgent.enabled = true;
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

    private void OnDestroy()
    {
        _animEventHandler.OnAutoattackHit -= AutoAttackHit;
        _animEventHandler.OnFirstSkillHit -= FirstSkillHit;
        _animEventHandler.OnSecondSkillHit -= SecondSkillHit;

        EventAggregator.Unsubscribe<AutoattackEvent>(AutoAttackHandler);
        EventAggregator.Unsubscribe<FirstSkillEvent>(FirstSkillInputHandler);
        EventAggregator.Unsubscribe<SecondSkillEvent>(SecondSkillInputHandler);
    }

    #region Skills hits
    private void AutoAttackHit()
    {
        if (Target.TryGetComponent(out CharacterHealth enemy))
        {
            enemy.ModifyHealth(-_autoAttackDamage, _character.CharID);
            _timeToNextAttack = 0;
        }
    }

    private void FirstSkillHit()
    {
        Instantiate(_firstSkillParticles, transform.position + Vector3.up * _smallValue, Quaternion.identity);
        _audioManager.PlaySFX(SoundsFx.Mork02Hit);

        var contacts = Physics.OverlapSphere(transform.position, _firstSkillRadius);
        for (int i = 0; i < contacts.Length; i++)
        {
            if(contacts[i].TryGetComponent(out CharacterIdentifier character))
            {
                if(character.Team != _character.CharID.Team)
                {
                    character.GetComponent<CharacterHealth>().ModifyHealth(-_firstSkillDamage, _character.CharID);
                    _effectsManager.SnareEffect(character, _firstSkillSlowdownEffect, _firstSkillSlowdownDuration);
                }
            }
        }
    }

    private void SecondSkillHit()
    {
        var contacts = Physics.OverlapSphere(transform.position, _secondSkillDamageRadius);
        for (int i = 0; i < contacts.Length; i++)
        {
            if (contacts[i].TryGetComponent(out CharacterIdentifier character))
            {
                if (character.Team != _character.CharID.Team)
                {
                    character.GetComponent<CharacterHealth>().ModifyHealth(-_secondSkillDamage, _character.CharID);
                    _effectsManager.StunEffect(character, _secondSkillStunDuration);
                }
            }
        }
        _secondSkillDirection = Vector3.zero;
        _character.CharMovement.UndoForcedStop();
    }

    #endregion

    #region Player input handler
    private void AutoAttackHandler(object arg1, AutoattackEvent arg2)
    {
        if (_character.CharID.IsControlledByThePlayer)
            AutoAttack();
    }

    private void FirstSkillInputHandler(object arg1, FirstSkillEvent skill)
    {
        if (_character.CharID.IsControlledByThePlayer)
        {
            UseFirstSkill();
        }
    }

    private void SecondSkillInputHandler(object arg1, SecondSkillEvent skill)
    {
        if (_character.CharID.IsControlledByThePlayer)
        {
            _secondSkillDirection = skill.Direction;
            UseSecondSkill();
        }

    }

    #endregion

    private Vector3 SecondSkillDirection(Vector3 secondSkillDirection)
    {
        if (secondSkillDirection.x > 19.5f)
            secondSkillDirection.x = 19.5f;

        if (secondSkillDirection.x < -19.5f)
            secondSkillDirection.x = -19.5f;

        if (secondSkillDirection.z > 29.5f)
            secondSkillDirection.z = 29.5f;

        if (secondSkillDirection.z < -29.5f)
            secondSkillDirection.z = -29.5f;

        return secondSkillDirection;
    }
}
