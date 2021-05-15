using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AutoattackButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private PlayerInputManager _input;
    private void Start()
    {
        _input = ServiceLocator.Resolve<PlayerInputManager>();     
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _input.IsPlayerHoldingAutoattackButton = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _input.IsPlayerHoldingAutoattackButton = false;
    }
}
