using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class FirstSkillButton : SkillButton, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CancleButton.IsCancle)
        {
            EventAggregator.Post(this, new FirstSkillEvent() { Direction = _direction });
        }
        Debug.Log(_direction);
        _inputManager.IsPlayerHoldingFirstSkillButton = false;
        _direction = Vector3.zero;

        EventAggregator.Post(this, new OnActivateCancleButton { IsOn = false });
        SetBackgroundAndThumbleVisibility(false);
    }


   public void OnPointerDown(PointerEventData eventData)
   {
     _inputManager.IsPlayerHoldingFirstSkillButton = true;
   }
}
