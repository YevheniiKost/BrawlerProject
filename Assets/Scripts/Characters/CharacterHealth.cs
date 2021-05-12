using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public event Action<float> OnHealthPctChange = delegate { };

    [SerializeField] private float _initialHealh;

    public float PercentOfHealthToStartRetreat = 30f;
    public bool IsCharacterDead => _isCharacteDead;

    private bool _isCharacteDead;
    private float _currentHealh;
    

    public void ModifyHealth(float amount)
    {
        if (!_isCharacteDead)
        {
            _currentHealh += amount;

            float currentHealthPct = (float)_currentHealh / (float)_initialHealh;
            OnHealthPctChange(currentHealthPct);

            if (_currentHealh <= 0)
            {
                ProcessCharacterDead();
            }
        }
        else
        {
            return;
        }
    }

    public float PercentOfHealth()
    {
        return _currentHealh / _initialHealh * 100;
    }

    private void Awake()
    {
        _currentHealh = _initialHealh;
    }

    private void ProcessCharacterDead()
    {
        return;
    }
}
