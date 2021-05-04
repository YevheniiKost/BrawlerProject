using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSpot : MonoBehaviour
{
    [Header("Base stats")]
    [SerializeField] private float _crystalSpawnCooldown = 10f;
    public CrystalType _crystalType;

    [Header("Tecnical parameters")]
    [SerializeField] private float _spawnHeigth = 1f;

    [Tooltip("Red - 0 team")]
    [SerializeField] private Crystal _redCrystalPrefab;
    [Tooltip("Blue - 1 team")]
    [SerializeField] private Crystal _blueCrystalPrefab;

    private bool _isCrystalOn;
    private Crystal _currentCrystal;

    private void Start()
    {
        StartCoroutine(WaitAndSpawnCrystal(_crystalSpawnCooldown));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out CharacterIdentifier player) && _currentCrystal != null)
        {
            if (player.Team == 0)
            {
                if (_crystalType == CrystalType.Red)
                    StartCrystalGrabbingCountdown();
                else if (_crystalType == CrystalType.Blue)
                    ImmediatlyGetCrystal();
            }
            else if (player.Team == 1)
            {
                if (_crystalType == CrystalType.Blue)
                    StartCrystalGrabbingCountdown();
                else if (_crystalType == CrystalType.Red)
                    ImmediatlyGetCrystal();
            }
            else
                throw new Exception("Player team value issue");
        }
    }

    private void ImmediatlyGetCrystal()
    {
        Destroy(_currentCrystal.gameObject);
        StartCoroutine(WaitAndSpawnCrystal(_crystalSpawnCooldown));
    }

    private void StartCrystalGrabbingCountdown()
    {
        
    }

    private IEnumerator WaitAndSpawnCrystal(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        SpawnCrystal();
    }

    private void SpawnCrystal()
    {
        _currentCrystal = Instantiate(GetCorrectCrystal(), transform.position + Vector3.up * _spawnHeigth, Quaternion.identity);
        _currentCrystal.transform.parent = transform;
    }

    private Crystal GetCorrectCrystal()
    {
        if (_crystalType == CrystalType.Blue)
            return _blueCrystalPrefab;
        else if (_crystalType == CrystalType.Red)
            return _redCrystalPrefab;
        else
            return null;
    }
}

public enum CrystalType
{
    Red,
    Blue
}
