﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        ServiceLocator.Resolve<AudioManager>().PlaySFX(SoundsFx.ButtonSound);
    }

   
}
