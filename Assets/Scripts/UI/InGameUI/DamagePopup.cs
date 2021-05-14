using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private float _offcet;
    [SerializeField] private float _lifeTime;
    [SerializeField] private Color _damageColor;
    [SerializeField] private Color _healingColor;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _scalerReduser;
    [SerializeField] private float _moveAmount;

    private Camera _mainCamera;
    private Transform _target;

    private void Start()
    {
        _mainCamera = ServiceLocator.Resolve<MainCamera>().GetComponent<Camera>();
    }
    private void LateUpdate()
    {
        if (_target != null)
            transform.position = _mainCamera.WorldToScreenPoint(_target.position + Vector3.up * _offcet);
    }

    public void Setup(int amount, CharacterHealth character, CharacterIdentifier hiter)
    {
        _target = character.transform;

        if (amount > 0)
            _text.color = Color.green;
        else
            _text.color = Color.red;

        _text.text = Mathf.Abs(amount).ToString();

        transform.DOBlendableScaleBy(Vector3.one * _scalerReduser, _lifeTime);
        _text.transform.DOBlendableMoveBy(Vector3.up * _moveAmount, _lifeTime);

        Destroy(this.gameObject, _lifeTime);
    }
}
