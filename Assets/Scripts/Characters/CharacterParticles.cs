using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParticles : MonoBehaviour, IStunComponent
{
    [SerializeField] private ParticleSystem _stunParticles;

    private bool _isStunned;
    public bool IsStunned => _isStunned;

    public void SetIsStunned(bool value)
    {
        _isStunned = value;
    }

    private void Update()
    {
        if (_isStunned)
        {
            _stunParticles.gameObject.SetActive(true);
        } else
        {
            _stunParticles.gameObject.SetActive(false);
        }
    }
}
