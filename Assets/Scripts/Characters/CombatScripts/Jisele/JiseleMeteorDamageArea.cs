using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiseleMeteorDamageArea : MonoBehaviour
{

    private List<CharacterIdentifier> _enemies = new List<CharacterIdentifier>();

    private CharacterIdentifier _caster;
    private bool _isActive = false;

    public void StartBurning(CharacterIdentifier caster, float lifeTime, int damage, float rate, float radius)
    {
        _caster = caster;
        _isActive = true;
        GetComponent<CapsuleCollider>().radius = radius;
        StartCoroutine(BurningCoroutine(lifeTime, damage, rate));
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out CharacterIdentifier enemy) && _isActive)
        {
            if(enemy.Team != _caster.Team && !_enemies.Contains(enemy))
            {
                _enemies.Add(enemy);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CharacterIdentifier enemy))
        {
            if (_enemies.Contains(enemy))
            {
                _enemies.Remove(enemy);
            }
        }
    }

    private IEnumerator BurningCoroutine(float lifeTime, int damage, float rate)
    {
        while(lifeTime > 0)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].GetComponent<CharacterHealth>().ModifyHealth(-damage, _caster);
            }
            lifeTime -= rate;
            yield return new WaitForSeconds(rate);
        }
    }
}
