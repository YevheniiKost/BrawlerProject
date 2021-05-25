using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalSpot : MonoBehaviour
{
    [Header("Base stats")]
    [SerializeField] private float _crystalSpawnCooldown = 10f;
    [SerializeField] private float _timeToTakeFriendlyCrystal = 3f;
    [SerializeField] private float _friendlyCrystalGrabDistance = 1f;
    [SerializeField] private Team _crystalType;
    [SerializeField] private Transform _chestCover;
    [SerializeField] private Image _visualTimer;
    [SerializeField] private ParticleSystem _redParticles;
    [SerializeField] private ParticleSystem _blueParticles;

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
        HandleCrystalSpot();
    }

    private void HandleCrystalSpot()
    {
        if (IsCrystalOn)
        {
            _visualTimer.enabled = true;
            _chestCover.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            if (_isFriendlyCharacterHere)
            {
                if (Vector3.Distance(transform.position, _frienlyChar.transform.position) < _friendlyCrystalGrabDistance)
                {
                    _friendlyCrystalGrabCooldown += Time.deltaTime;
                    _visualTimer.fillAmount -= 1 / _timeToTakeFriendlyCrystal * Time.deltaTime;
                    if (_friendlyCrystalGrabCooldown >= _timeToTakeFriendlyCrystal)
                    {
                        ImmediatlyGetCrystal();
                        EventAggregator.Post(this, new OnGetPoint { CharacterTeam = SpotTeam });
                        _friendlyCrystalGrabCooldown = 0;
                    }
                }
                else
                {
                    _visualTimer.fillAmount = 1;
                }
            }
            else
            {
                _visualTimer.fillAmount = 1;
            }
        }
        else
        {
            _chestCover.transform.localRotation = Quaternion.Euler(Vector3.zero);
            _visualTimer.enabled = false;
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
                {
                    ImmediatlyGetCrystal();
                    EventAggregator.Post(this, new OnGetPoint { CharacterTeam = Team.Red });
                }
            }
            else if (player.Team == 1)
            {
                if (_crystalType == Team.Blue)
                    StartCrystalGrabbingCountdown(player);
                else if (_crystalType == Team.Red)
                {
                    ImmediatlyGetCrystal();
                    EventAggregator.Post(this, new OnGetPoint { CharacterTeam = Team.Blue });
                }
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
        ServiceLocator.Resolve<AudioManager>().PlaySFX(SoundsFx.CrystalGet);
        PlayPaticles();
        IsCrystalOn = false;
        Destroy(_currentCrystal.gameObject);
        StartCoroutine(WaitAndSpawnCrystal(_crystalSpawnCooldown));
    }

    private void PlayPaticles()
    {
        if (_crystalType == Team.Red)
            _redParticles.Play();
        else
            _blueParticles.Play();
    }

    private IEnumerator WaitAndSpawnCrystal(float coolDown)
    {
        yield return new WaitForSeconds(coolDown);
        SpawnCrystal();
    }

    private void SpawnCrystal()
    {
        IsCrystalOn = true;
        if(_currentCrystal != null)
        {
            Destroy(_currentCrystal.gameObject);
        }
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


