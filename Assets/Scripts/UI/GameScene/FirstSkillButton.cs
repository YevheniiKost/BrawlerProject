using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class FirstSkillButton : SkillButton, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        EventAggregator.Post(this, new FirstSkillEvent() { Direction = _direction });
        Debug.Log($"skill 1 used with {_direction} angle");
        _direction = Vector3.zero;

        SetBackgroundAndThumbleVisibility(false);
    }
}
