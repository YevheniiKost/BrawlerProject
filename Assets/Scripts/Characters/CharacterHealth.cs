using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LifeStatus
{
    FullHealth,
    Alright,
    NeedHealth,
    Dead
}

[RequireComponent(typeof(Character))]
public class CharacterHealth : MonoBehaviour
{
    public event Action<float> OnHealthPctChange = delegate { };

    [SerializeField] private float _initialHealh;

    public float PercentOfHealthToStartRetreat = 30f;

    private float _currentHealh;
    private Character _character;
    private AudioManager _audioManager;

    public LifeStatus GetLifeStatus()
    {
        if (PercentOfHealth() >= 100)
        {
            return LifeStatus.FullHealth;
        }
        else if (PercentOfHealth() > PercentOfHealthToStartRetreat)
        {
            return LifeStatus.Alright;
        }
        else if (PercentOfHealth() <= PercentOfHealthToStartRetreat && _currentHealh > 0)
        {
            return LifeStatus.NeedHealth;
        }
        else if (_currentHealh <= 0)
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

    public void RenewCharacter()
    {
        transform.localPosition = Vector3.zero;
        _currentHealh += _initialHealh;
        EventAggregator.Post(this, new AddHealthBar { Character = this });
        EventAggregator.Post(this, new CharacterWakeUp { Character = this });
    }

    private void Awake()
    {
        _character = GetComponent<Character>();
        _currentHealh = _initialHealh;
    }

    private void Start()
    {
        _audioManager = ServiceLocator.Resolve<AudioManager>();
        EventAggregator.Post(this, new AddHealthBar { Character = this });
    }

    private void ProcessCharacterDead(CharacterIdentifier killer)
    {
        EventAggregator.Post(this, new CharacterDeath { Killer = killer, Character = this });

        if (_character.CharID.Name == CharacterName.Beor)
            _audioManager.PlaySFX(SoundsFx.BeorDeath);
        else if (_character.CharID.Name == CharacterName.Jisele)
            _audioManager.PlaySFX(SoundsFx.JiseleDeath);
        else if (_character.CharID.Name == CharacterName.Mork)
            _audioManager.PlaySFX(SoundsFx.MorkDeath);

        _character.CharMovement.StopMovement();
    }

    private float PercentOfHealth()
    {
        return _currentHealh / _initialHealh * 100;
    }
}
