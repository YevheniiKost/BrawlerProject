using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CancleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool IsCancle => _isCancle;

    private static bool _isCancle;

    private void Awake()
    {
        EventAggregator.Subscribe<OnActivateCancleButton>(DisableImage);
    }

    private void DisableImage(object arg1, OnActivateCancleButton data)
    {
        GetComponent<Image>().enabled = data.IsOn;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isCancle = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isCancle = false;
    }

}
