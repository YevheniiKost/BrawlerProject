using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;

public delegate void CombatEvent();

[RequireComponent(typeof(CharacterIdentifier))]
public abstract class CharacterCombat : MonoBehaviour
{
    public event CombatEvent AutoAttackWasUsed;
    public event CombatEvent FirstSkillWasUsed;
    public event CombatEvent SecondSkillWasUsed;

    [HideInInspector]
    public const float DetectEnemyUpdateTime = 0.2f; 

    [SerializeField] protected AnimationEventHandler _animEventHandler;
    [SerializeField] protected CharacterIdentifier _charID;

    [Header("Autoattacks")]
    [SerializeField] protected float _autoAttackDamage;
    [SerializeField] protected float _autoAttackRange;
    [SerializeField] protected float _autoAttackRate;

    public float EnemyDetectRadius = 10f;
    public float AutoattackRange => _autoAttackRange;
    public bool IsEnemyDetected;
    public Transform Target => _target;

    protected float _timeToNextAttack = 0;
    protected Transform _target;
    private MapHelper _mapHelper;

    public abstract void AutoAttack();
    public abstract void UseFirstSkill();
    public abstract void UseSecondSkill();
    public virtual void OnAutoAttack() => AutoAttackWasUsed?.Invoke();
    public virtual void OnFirsSkillUse() => FirstSkillWasUsed?.Invoke();
    public virtual void OnSecondSkillUse() => SecondSkillWasUsed?.Invoke();

    private void Awake()
    {
        _animEventHandler.OnAutoattackHit += AutoAttackHit;
        _animEventHandler.OnFirstSkillHit += FirstSkillHit;
    }

    protected abstract void AutoAttackHit();
    protected abstract void FirstSkillHit();

    private void Start()
    {
        _mapHelper = ServiceLocator.Resolve<MapHelper>();
        StartCoroutine(StartEnemySearchCycle());
    }

    private void Update()
    {
        
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
        if (_mapHelper.DistanceToNearestEnemy(_charID) <= EnemyDetectRadius)
        {
            IsEnemyDetected = true;

            if (Target == _mapHelper.GetNearestEnemy(_charID).transform)
            {
                return;
            }
            else
            {
                _target = _mapHelper.GetNearestEnemy(_charID).transform;
            }

        }
        else
        {
            IsEnemyDetected = false;
            _target = null;
        }
    }
}
