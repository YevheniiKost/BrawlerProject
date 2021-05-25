using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;

public delegate void CombatEvent();

[RequireComponent(typeof(Character))]
public abstract class CharacterCombat : MonoBehaviour, IStunComponent
{
    public event CombatEvent AutoAttackWasUsed;
    public event CombatEvent FirstSkillWasUsed;
    public event CombatEvent SecondSkillWasUsed;

    [HideInInspector]
    public const float DetectEnemyUpdateTime = 0.2f; 

    [SerializeField] protected AnimationEventHandler _animEventHandler;
    

    [Header("Autoattacks")]
    [SerializeField] protected int _autoAttackDamage;
    [SerializeField] protected float _autoAttackRange;
    [SerializeField] protected float _autoAttackRate;

    public float AutoattackRange => _autoAttackRange;
    public Transform Target => _target;

    #region stun
    public bool IsStunned => _isStunned;
    public void SetIsStunned(bool value) => _isStunned = value;
    #endregion

    public float EnemyDetectRadius = 10f;
    [HideInInspector]
    public bool IsEnemyDetected;

    protected bool _isStunned;
    protected float _timeToNextAttack = 0;
    protected Transform _target;
    protected CharacterEffectsManager _effectsManager;
    protected AudioManager _audioManager;
    protected Character _character;

    protected float _firstAbilityCooldownTimer;
    protected float _secondAbilityCooldownTimer;

    private MapHelper _mapHelper;

    public abstract void AutoAttack();
    public abstract void UseFirstSkill();
    public abstract void UseSecondSkill();
    public abstract void AttackBehavior();
    public virtual void OnAutoAttack() => AutoAttackWasUsed?.Invoke();
    public virtual void OnFirstSkillUse() => FirstSkillWasUsed?.Invoke();
    public virtual void OnSecondSkillUse() => SecondSkillWasUsed?.Invoke();


    private void Start()
    {
        _character = GetComponent<Character>();
        _mapHelper = ServiceLocator.Resolve<MapHelper>();
        _effectsManager = ServiceLocator.Resolve<CharacterEffectsManager>();
        _audioManager = ServiceLocator.Resolve<AudioManager>();

        StartCoroutine(StartEnemySearchCycle());
    }
    protected IEnumerator StartEnemySearchCycle()
    {
        while (true) 
        {
            yield return new WaitForSeconds(DetectEnemyUpdateTime);
            DetectEnemy();
        }
    }
    private void DetectEnemy()
    {
        if (_mapHelper.GetNearestEnemy(_character.CharID) == null)
        {
            IsEnemyDetected = false;
            _target = null;
        }
        else
        {
            if (_mapHelper.DistanceToNearestEnemy(_character.CharID) <= EnemyDetectRadius)
            {
                IsEnemyDetected = true;

                if (Target == _mapHelper.GetNearestEnemy(_character.CharID).transform)
                {
                    return;
                }
                else
                {
                    _target = _mapHelper.GetNearestEnemy(_character.CharID).transform;
                }
            }
            else
            {
                IsEnemyDetected = false;
                _target = null;
            }
        }
    }
    protected void FirstAbilityUsedEvent(float CD)
    {
        if (_character.CharID.IsControlledByThePlayer)
            EventAggregator.Post(this, new FirstAbilityWasUsed { Cooldown = CD });
    }
    protected void SecondAbilityUsedEvent(float CD)
    {
        if(_character.CharID.IsControlledByThePlayer)
            EventAggregator.Post(this, new SecondAbilityWasUsed { Cooldown = CD });
    }
}

public interface IStunComponent 
{
    bool IsStunned { get; }

    void SetIsStunned(bool value);
}
