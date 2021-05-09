using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSpot : MonoBehaviour
{
    [Header("Base stats")]
    [SerializeField] private float _crystalSpawnCooldown = 10f;
    [SerializeField] private float _timeToTakeFriendlyCrystal = 3f;
    [SerializeField] private float _friendlyCrystalGrabDistance = 1f;
    [SerializeField] private Team _crystalType;

    [Header("Tecnical parameters")]
    [SerializeField] private float _spawnHeigth = 1f;

    [Tooltip("Red - 0 team")]
    [SerializeField] private Crystal _redCrystalPrefab;
    [Tooltip("Blue - 1 team")]
    [SerializeField] private Crystal _blueCrystalPrefab;

    public Team SpotTeam => _crystalType;
    public bool IsCrystalOn;
    private Crystal _currentCrystal;
    private float _friendlyCrystalGrabCooldown = 0;
    private bool _isFriendlyCharacterHere;
    private CharacterIdentifier _frienlyChar;

    private void Start()
    {
        SpawnCrystal();
        StartCoroutine(WaitAndSpawnCrystal(_crystalSpawnCooldown));
    }

    private void Update()
    {
        if (_isFriendlyCharacterHere && IsCrystalOn)
        {
            if(Vector3.Distance(transform.position, _frienlyChar.transform.position) < _friendlyCrystalGrabDistance)
            {
                _friendlyCrystalGrabCooldown += Time.deltaTime;
                if(_friendlyCrystalGrabCooldown >= _timeToTakeFriendlyCrystal)
                {
                    ImmediatlyGetCrystal();
                    _friendlyCrystalGrabCooldown = 0;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_frienlyChar != null && other.TryGetComponent(out CharacterIdentifier character))
        {
            if (character.Team == 0 && SpotTeam == Team.Red)
            {
                _isFriendlyCharacterHere = false;
                _frienlyChar = null;
            }
            else if (other.GetComponent<CharacterIdentifier>().Team == 1 && SpotTeam == Team.Blue)
            {
                _isFriendlyCharacterHere = false;
                _frienlyChar = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out CharacterIdentifier player) && _currentCrystal != null)
        {
            if (player.Team == 0)
            {
                if (_crystalType == Team.Red)
                    StartCrystalGrabbingCountdown(player);
                else if (_crystalType == Team.Blue)
                    ImmediatlyGetCrystal();
            }
            else if (player.Team == 1)
            {
                if (_crystalType == Team.Blue)
                    StartCrystalGrabbingCountdown(player);
                else if (_crystalType == Team.Red)
                    ImmediatlyGetCrystal();
            }
            else
                throw new Exception($"Player {player.name} team value issue");
        }
    }

    private void StartCrystalGrabbingCountdown(CharacterIdentifier character)
    {
        _isFriendlyCharacterHere = true;
        _frienlyChar = character;
    }

    private void ImmediatlyGetCrystal()
    {
        IsCrystalOn = false;
        Destroy(_currentCrystal.gameObject);
        StartCoroutine(WaitAndSpawnCrystal(_crystalSpawnCooldown));
    }

    private IEnumerator WaitAndSpawnCrystal(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        SpawnCrystal();
    }

    private void SpawnCrystal()
    {
        IsCrystalOn = true;
        _currentCrystal = Instantiate(GetCorrectCrystal(), transform.position + Vector3.up * _spawnHeigth, Quaternion.identity);
        _currentCrystal.transform.parent = transform;
    }

    private Crystal GetCorrectCrystal()
    {
        if (_crystalType == Team.Blue)
            return _blueCrystalPrefab;
        else if (_crystalType == Team.Red)
            return _redCrystalPrefab;
        else
            return null;
    }
}


