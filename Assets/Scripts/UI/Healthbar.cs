using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private CharacterHealth _health;
    [SerializeField] private Image _foregroundImage;
    [SerializeField] private float _updateSpeedSeconds = .5f;

    private void Awake()
    {
        _health.OnHealthPctChange += HandleHealthChange;
    }

    private void OnDestroy()
    {
        _health.OnHealthPctChange -= HandleHealthChange;
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }

    private void HandleHealthChange(float pct)
    {
        StartCoroutine(ChangeToPct(pct));
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
}
