using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RightSideInputPanelUI : MonoBehaviour
{
   [SerializeField] private Button _autoAttackButton;
    [SerializeField] private Image _firstSkillImage;
    [SerializeField] private Image _SecondSkillImage;
    [SerializeField] private AbilityCooldownTimer _firstSkillCD;
    [SerializeField] private AbilityCooldownTimer _secondSkillCD;

    private void Awake()
    {
        _autoAttackButton.onClick.AddListener(AutoattackButtonClickHandler);

        EventAggregator.Subscribe<TransferCurrentCharacterData>(InsertAbilityImages);
        EventAggregator.Subscribe<FirstAbilityWasUsed>(CreateCDTimerFirstSkill);
        EventAggregator.Subscribe<SecondAbilityWasUsed>(CreateCDTimerSecondSkill);
    }
    private void OnDestroy()
    {
        EventAggregator.Unsubscribe<TransferCurrentCharacterData>(InsertAbilityImages);
        EventAggregator.Unsubscribe<FirstAbilityWasUsed>(CreateCDTimerFirstSkill);
        EventAggregator.Unsubscribe<SecondAbilityWasUsed>(CreateCDTimerSecondSkill);
    }

    private void CreateCDTimerSecondSkill(object arg1, SecondAbilityWasUsed data) => _secondSkillCD.CreateColldown(data.Cooldown);

    private void CreateCDTimerFirstSkill(object arg1, FirstAbilityWasUsed data) => _firstSkillCD.CreateColldown(data.Cooldown);

    private void InsertAbilityImages(object arg1, TransferCurrentCharacterData data)
    {
        _firstSkillImage.sprite = data.Data.FirstSkillImage;
        _SecondSkillImage.sprite = data.Data.SecondSkillImage;
    }

    private void AutoattackButtonClickHandler()
    {
        EventAggregator.Post(this, new AutoattackEvent() { });
    }
}
