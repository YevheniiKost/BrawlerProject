using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldownTimer : MonoBehaviour
{
    [SerializeField] private Image _darkImage;
    [SerializeField] private TextMeshProUGUI _nubmers;
    
    public void CreateColldown(float timer)
    {
        StartCoroutine(StartCooldownTimer(timer));
    }

    private IEnumerator StartCooldownTimer(float timer)
    {
        _darkImage.enabled = true;
        _nubmers.enabled = true;
        int integers;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            integers = (int)timer;
            _nubmers.text = integers.ToString();
            yield return null;
        }
        _darkImage.enabled = false;
        _nubmers.enabled = false;
    }
}
