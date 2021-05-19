using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private Team _team;
    [SerializeField] private int _healthRegenAmount;
    [SerializeField] private float _healthRegenRate;

    public Team SpawnerTeam => _team;

    private List<CharacterHealth> _characterInside = new List<CharacterHealth>();
    private int _intTeam;

    private void Start()
    {
        _intTeam = _team == Team.Red ? 0 : 1;
        StartCoroutine(EndlessHealthRegeneration());
    }

    private IEnumerator EndlessHealthRegeneration()
    {
        while (true)
        {
            if (_characterInside.Count != 0)
            {
                for (int i = 0; i < _characterInside.Count; i++)
                {
                    if (_characterInside[i].GetComponent<CharacterIdentifier>().Team == _intTeam && _characterInside[i].GetLifeStatus() != LifeStatus.FullHealth)
                    {
                        _characterInside[i].ModifyHealth(_healthRegenAmount, _characterInside[i].GetComponent<CharacterIdentifier>());
                    }
                    else if (_characterInside[i].GetComponent<CharacterIdentifier>().Team != _intTeam)
                    {
                        _characterInside[i].ModifyHealth(-_healthRegenAmount, _characterInside[i].GetComponent<CharacterIdentifier>());
                    }
                }
            }
            yield return new WaitForSeconds(_healthRegenRate);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterHealth character))
        {
            _characterInside.Add(character);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CharacterHealth character))
        {
            _characterInside.Remove(character);
        }
    }
}

public enum Team
{
    Red,
    Blue
}
