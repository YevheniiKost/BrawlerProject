using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LifeStatus
{
    Alright,
    NeedHealth,
    Dead
}


public class CharacterHealth : MonoBehaviour
{
    public event Action<float> OnHealthPctChange = delegate { };

    [SerializeField] private float _initialHealh;

    public float PercentOfHealthToStartRetreat = 30f;

    private float _currentHealh;

    public LifeStatus GetLifeStatus()
    {
        if(_currentHealh > PercentOfHealthToStartRetreat)
        {
            return LifeStatus.Alright;
        } else if(_currentHealh <= PercentOfHealthToStartRetreat && _currentHealh > 0)
        {
            return LifeStatus.NeedHealth;
        }
        else if(_currentHealh <= 0)
        {
            return LifeStatus.Dead;
        }
        else { return LifeStatus.Dead; }
    }


    public void ModifyHealth(int amount, CharacterIdentifier hiter)
    {
        if (GetLifeStatus() != LifeStatus.Dead)
        {
            _currentHealh += amount;

            float currentHealthPct = (float)_currentHealh / (float)_initialHealh;
            OnHealthPctChange(currentHealthPct);

            EventAggregator.Post(this, new CharacterHit { Hiter = hiter, Amount = amount, Character = this });

            if (_currentHealh <= 0)
            {
                ProcessCharacterDead(hiter);
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

    private void ProcessCharacterDead(CharacterIdentifier killer)
    {
        EventAggregator.Post(this, new CharacterDeath { Killer = killer, Character = this });
    }
}
