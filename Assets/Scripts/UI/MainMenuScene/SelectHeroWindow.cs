using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectHeroWindow : MonoBehaviour, IWindow
{
    [SerializeField] private TextMeshProUGUI _heroNameText;
    [SerializeField] private Button _rightArrowButton;
    [SerializeField] private Button _leftArrowButton;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Slider _heroRotateSlider;
    [SerializeField] private Image _firstSkillImage;
    [SerializeField] private Image _secondSkillImage;
    [SerializeField] private TextMeshProUGUI _firstSkillDescriptionText;
    [SerializeField] private TextMeshProUGUI _secondSkillDescriptionText;

    [SerializeField] private Transform _heroNamePanel;
    [SerializeField] private Transform _firstSkillPanel;
    [SerializeField] private Transform _secondSkillPanel;
    [SerializeField] private Transform _firstDescPanel;
    [SerializeField] private Transform _secondDescPanel;

    [SerializeField] private float _verticalOffcet;
    [SerializeField] private float _horizontalOffcet;
    [SerializeField] private float _openCloseTime;

    private bool _isOpened;

    public void CloseWindow()
    {
        if (_isOpened)
        {
            MyUtilities.UI.MoveUp(_heroNamePanel, _verticalOffcet, _openCloseTime);
            MyUtilities.UI.MoveUp(_heroRotateSlider.transform, _horizontalOffcet, _openCloseTime);
            MyUtilities.UI.MoveDown(_selectButton.transform, _verticalOffcet, _openCloseTime);
            MyUtilities.UI.MoveRight(_secondDescPanel, _horizontalOffcet, _openCloseTime);
            MyUtilities.UI.MoveRight(_secondSkillPanel, _horizontalOffcet, _openCloseTime);
            MyUtilities.UI.MoveRight(_rightArrowButton.transform, _horizontalOffcet, _openCloseTime);
            MyUtilities.UI.MoveLeft(_firstDescPanel, _horizontalOffcet, _openCloseTime);
            MyUtilities.UI.MoveLeft(_firstSkillPanel, _horizontalOffcet, _openCloseTime);
            MyUtilities.UI.MoveLeft(_leftArrowButton.transform, _horizontalOffcet, _openCloseTime);

            _isOpened = false;
        }
    }

    public void OpenWindow()
    {
        if (!_isOpened)
        {
            MyUtilities.UI.MoveDown(_heroNamePanel, _verticalOffcet, _openCloseTime);
            MyUtilities.UI.MoveDown(_heroRotateSlider.transform, _horizontalOffcet, _openCloseTime);
            MyUtilities.UI.MoveUp(_selectButton.transform, _verticalOffcet, _openCloseTime);
            MyUtilities.UI.MoveLeft(_secondDescPanel, _horizontalOffcet, _openCloseTime);
            MyUtilities.UI.MoveLeft(_secondSkillPanel, _horizontalOffcet, _openCloseTime);
            MyUtilities.UI.MoveLeft(_rightArrowButton.transform, _horizontalOffcet, _openCloseTime);
            MyUtilities.UI.MoveRight(_firstDescPanel, _horizontalOffcet, _openCloseTime);
            MyUtilities.UI.MoveRight(_firstSkillPanel, _horizontalOffcet, _openCloseTime);
            MyUtilities.UI.MoveRight(_leftArrowButton.transform, _horizontalOffcet, _openCloseTime);

            _isOpened = true;
        }
    }

    private void Awake()
    {
        _leftArrowButton.onClick.AddListener(OnLeftArrowClickHandler);
        _rightArrowButton.onClick.AddListener(OnRightArrowClickHandler);
        _selectButton.onClick.AddListener(OnSelectButtonClickHandler);
        _isOpened = false;

        EventAggregator.Subscribe<TransferCurrentCharacterData>(ChangeCharacterData);
    }

    private void Start()
    {
        EventAggregator.Post(this, new SetWindow { Window = this });
    }
    private void OnDestroy()
    {
        EventAggregator.Post(this, new RemoveWindow { Window = this });
        EventAggregator.Unsubscribe<TransferCurrentCharacterData>(ChangeCharacterData);
    }


    private void ChangeCharacterData(object arg1, TransferCurrentCharacterData data)
    {
        _firstSkillDescriptionText.text = data.Data.FirstSkillDescription.text;
        _secondSkillDescriptionText.text = data.Data.SecondSkillDescription.text;
        _firstSkillImage.sprite = data.Data.FirstSkillImage;
        _secondSkillImage.sprite = data.Data.SecondSkillImage;
        _heroNameText.text = data.Data.Name.ToString();
    }

    private void OnSelectButtonClickHandler()
    {
        EventAggregator.Post(this, new OnSelectHeroButtonClick { });
        ServiceLocator.Resolve<UIManager>().OpenWindow(typeof(MainWindow));
        CloseWindow();

    }

    private void OnLeftArrowClickHandler()
    {
        EventAggregator.Post(this, new OnLeftArrowHeroSelectClick { });
    }

    private void OnRightArrowClickHandler()
    {
        EventAggregator.Post(this, new OnRightArrowHeroSelectClick { });
    }


}
