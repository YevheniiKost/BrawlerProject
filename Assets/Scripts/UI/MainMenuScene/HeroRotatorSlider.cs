using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class HeroRotatorSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CharacterCarousel _carousel;
    [SerializeField] private float _rotationBackSpeed;
    private Slider _slider;
    private bool _isPlayerHoldingSlider;

    private float _startValue = .5f;
    private float _currentValue;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.onValueChanged.AddListener(RotateHero);
    }
    private void Start()
    {
        _slider.value = _startValue;
    }
    private void Update()
    {
        if (!_isPlayerHoldingSlider)
        {
            _slider.value = Mathf.Lerp(_slider.value, _startValue, Time.deltaTime * _rotationBackSpeed);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPlayerHoldingSlider = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPlayerHoldingSlider = false;
    }

    private void RotateHero(float amount)
    {
        float rot = Mathf.Lerp(-150f, 150f, amount);
        _carousel.RotateHero(rot);
        _currentValue = _slider.value;
    }
}
