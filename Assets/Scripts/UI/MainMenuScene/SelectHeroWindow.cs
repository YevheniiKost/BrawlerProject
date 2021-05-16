using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectHeroWindow : MonoBehaviour,IWindow
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

    public void CloseWindow()
    {
        throw new System.NotImplementedException();
    }

    public void OpenWindow()
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        
    }

    private void Start()
    {
        EventAggregator.Post(this, new SetWindow { Window = this });
    }
}
