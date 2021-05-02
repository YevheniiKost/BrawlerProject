using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] private float _initialHealh;

    private float _currentHealh;

    public void ModifyHealth(float amount)
    {
        _currentHealh -= amount;
        if(_currentHealh <= 0)
        {
            ProcessCharacterDead();
        }
    }

    private void Awake()
    {
        _currentHealh = _initialHealh;
    }

    private void ProcessCharacterDead()
    {
        throw new NotImplementedException();
    }
}
