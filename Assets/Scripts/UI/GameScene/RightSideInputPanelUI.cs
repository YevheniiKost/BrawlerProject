using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RightSideInputPanelUI : MonoBehaviour
{
   [SerializeField] private Button _autoAttackButton;

    private void Awake()
    {
        _autoAttackButton.onClick.AddListener(AutoattackButtonClickHandler);
    }

    private void AutoattackButtonClickHandler()
    {
        EventAggregator.Post(this, new AutoattackEvent() { });
    }
}
