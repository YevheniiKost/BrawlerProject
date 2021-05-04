using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class CharacterCombat : MonoBehaviour
{
    [Header("Autoattacks")]
    [SerializeField] protected float _autoAttackDamage;
    [SerializeField] protected float _autoAttackRange;
    [SerializeField] protected float _autoAttackRate;

    public abstract void AutoAttack(Transform target);
    public abstract void UseFirstSkill();
    public abstract void UseSecondSkill();

    protected List<CharacterIdentifier> _enemyList = new List<CharacterIdentifier>();
    protected CharacterIdentifier _nearestTarget;

    public void GetTarget()
    {
        _nearestTarget = _enemyList.OrderBy(t => Vector3.Distance(transform.position, t.transform.position)).FirstOrDefault();
    }

    private void Start()
    {
        GetEnemies();
    }

    protected void GetEnemies()
    {
        foreach (var character in FindObjectsOfType<CharacterIdentifier>())
        {
            if (character.Team != GetComponent<CharacterIdentifier>().Team)
            {
                _enemyList.Add(character);
            }
        }
    }
}
