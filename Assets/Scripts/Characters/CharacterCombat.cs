using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterCombat : MonoBehaviour
{
    [Header("Autoattacks")]
    [SerializeField] protected float _autoAttackDamage;
    [SerializeField] protected float _autoAttackRange;
    [SerializeField] protected float _autoAttackRate;

    

    public abstract void AutoAttack(Transform target);
    public abstract void UseFirstSkill();
    public abstract void UseSecondSkill();

    public void GetTarget()
    {

    }
}
