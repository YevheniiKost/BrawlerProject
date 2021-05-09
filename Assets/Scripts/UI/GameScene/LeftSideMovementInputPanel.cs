using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LeftSideMovementInputPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Image _background;
    [SerializeField] private Image _thumble;
    [SerializeField] private float _sizeAjust;

    private Vector3 _direction = Vector3.zero;
    private Vector3 _startThumblePosition;

    private float _distance;
    private Vector3 _newPosition;
    private float _backgroundRadius;

    private PlayerInputManager _inputManager;

    public void OnPointerDown(PointerEventData eventData)
    {
        _background.transform.position = eventData.position;
        SetBackgroundAndThumbleVisibility(true);

        _inputManager.IsPlayerHoldingMovementButton = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetBackgroundAndThumbleVisibility(false);
        _direction = Vector3.zero;
        _inputManager.MovenemtInputDirection = _direction;

        _inputManager.IsPlayerHoldingMovementButton = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startThumblePosition = _thumble.transform.position;
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
        _inputManager.MovenemtInputDirection = _direction;
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

    private void CalculateRadius()
    {
        var rectTransform = _background.GetComponent<RectTransform>();
        _backgroundRadius = rectTransform.sizeDelta.x * _sizeAjust * .5f;
    }

    private void SetBackgroundAndThumbleVisibility(bool isVisible)
    {
        _background.gameObject.SetActive(isVisible);
        _thumble.gameObject.SetActive(isVisible);
    }

    
}
