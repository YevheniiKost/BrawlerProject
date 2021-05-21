using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image _foregroundImage;
    [SerializeField] private float _positionOffcet = 1f;
    [SerializeField] private float _updateSpeedSeconds = .5f;
    [SerializeField] private Color _playerColor;
    [SerializeField] private Color _redTeamColor;
    [SerializeField] private Color _blueTeamColor;

    private CharacterHealth _health;
    private Image _mainImage;
    private Camera _mainCamera;

    private Coroutine _currentcoroutine;
    private void Start()
    {
        _mainCamera = ServiceLocator.Resolve<MainCamera>().GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (_health != null)
            transform.position = _mainCamera.WorldToScreenPoint(_health.transform.position + Vector3.up * _positionOffcet);
    }

    private void OnDestroy()
    {
        if (_currentcoroutine != null)
            StopCoroutine(_currentcoroutine);
        _health.OnHealthPctChange -= HandleHealthChange;
    }

    private void HandleHealthChange(float pct)
    {
        if (_currentcoroutine != null)
            StopCoroutine(_currentcoroutine);
        _currentcoroutine = StartCoroutine(ChangeToPct(pct));
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangedPct = _foregroundImage.fillAmount;
        float elapsed = 0;

        while(elapsed < _updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            _foregroundImage.fillAmount = Mathf.Lerp(preChangedPct, pct, elapsed / _updateSpeedSeconds);
            yield return null;
        }
        _foregroundImage.fillAmount = pct;
    }

    public void SetHealth(CharacterHealth character)
    {
        _health = character;

        if (character.GetComponent<CharacterIdentifier>().IsControlledByThePlayer)
            _foregroundImage.color = new Color(_playerColor.r, _playerColor.g, _playerColor.b);
        else if (character.GetComponent<CharacterIdentifier>().Team == 1)
            _foregroundImage.color = new Color(_blueTeamColor.r, _blueTeamColor.g, _blueTeamColor.b);
        else
            _foregroundImage.color = new Color(_redTeamColor.r, _redTeamColor.g, _redTeamColor.b);

        _health.OnHealthPctChange += HandleHealthChange;
    }
}
