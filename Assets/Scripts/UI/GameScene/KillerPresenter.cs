using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillerPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _textExistTime;

    private Coroutine _currentCoroutine;
    internal void CreateTextPopup(string character, string killer)
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(CreatePopup(character, killer));
    }

    private IEnumerator CreatePopup(string character, string killer)
    {
        _text.text = $"{killer} brutally killed {character}!";

        yield return new WaitForSeconds(_textExistTime);

        _text.text = " ";
    }

}

