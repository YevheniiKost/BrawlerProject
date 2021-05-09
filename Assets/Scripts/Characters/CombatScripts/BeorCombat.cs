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

    private float _timeToNextShieldBash = 0; 
    public override void AutoAttack()
    {
        if (_timeToNextAttack >= _autoAttackRate && Target != null)
        {
            if (Vector3.Distance(transform.position, Target.position) <= _autoAttackRange)
            {
                transform.LookAt(_target);
                OnAutoAttack();
            } 
        }

    }
    public override void UseFirstSkill()
    {
        if(_timeToNextShieldBash >= _shieldBashCooldown)
        {

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

    public override void UseSecondSkill()
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Target != null)
        {
            AutoAttack();
        }
        _timeToNextAttack += Time.deltaTime;
        _timeToNextShieldBash += Time.deltaTime;
    }
}
