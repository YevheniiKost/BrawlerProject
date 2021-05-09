using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SecondSkillButton : SkillButton, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        EventAggregator.Post(this, new FirstSkillEvent() { Direction = _direction });
        Debug.Log($"skill 2 used with {_direction} angle");
        _direction = Vector3.zero;

        SetBackgroundAndThumbleVisibility(false);
    }
}
