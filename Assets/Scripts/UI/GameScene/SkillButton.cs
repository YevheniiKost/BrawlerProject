using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] protected Image _background;
    [SerializeField] protected Image _thumble;
    [SerializeField] protected float _sizeAjust;

    protected Vector3 _direction = Vector3.zero;
    protected Vector3 _startThumblePosition;

    protected float _distance;
    protected Vector3 _newPosition;
    protected float _backgroundRadius;

    protected PlayerInputManager _inputManager;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startThumblePosition = _thumble.transform.position;

        SetBackgroundAndThumbleVisibility(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _distance = Vector3.Distance(_startThumblePosition, eventData.position);
        _direction = new Vector3(eventData.position.x, eventData.position.y) - _startThumblePosition;
        _direction = _direction.normalized;

        if (_distance > _backgroundRadius)
        {
            _newPosition = _startThumblePosition + _direction * _backgroundRadius;
        }
        else
        {
            _direction *= _distance / _backgroundRadius;
            _newPosition = eventData.position;
        }

        _thumble.transform.position = _newPosition;

        _inputManager.FirstSkillDirection = _direction;
        _inputManager.SecondSkillDirection = _direction;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _thumble.transform.position = _startThumblePosition;
    }

    private void Start()
    {
        CalculateRadius();
        _inputManager = ServiceLocator.Resolve<PlayerInputManager>();
    }

    protected void CalculateRadius()
    {
        var rectTransform = _background.GetComponent<RectTransform>();
        _backgroundRadius = rectTransform.sizeDelta.x * _sizeAjust * .5f;
    }

    protected void SetBackgroundAndThumbleVisibility(bool isVisible)
    {
        _background.gameObject.SetActive(isVisible);
        _thumble.gameObject.SetActive(isVisible);
    }

 
}
