using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SecondSkillButton : SkillButton, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CancleButton.IsCancle)
        {
            EventAggregator.Post(this, new SecondSkillEvent() { Direction = _direction });
        }
        _inputManager.IsPlayerHoldingSecondSkillButton = false;
        _direction = Vector3.zero;

        EventAggregator.Post(this, new OnActivateCancleButton { IsOn = false });
        SetBackgroundAndThumbleVisibility(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _inputManager.IsPlayerHoldingSecondSkillButton = true;
    }
}
