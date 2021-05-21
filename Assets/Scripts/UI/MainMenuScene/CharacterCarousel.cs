using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCarousel : MonoBehaviour, IHeroRotater
{
    [SerializeField] private List<CharacterLite> _characters = new List<CharacterLite>();
    [SerializeField] private float _heroRotationSpeed;

    public int IndexOfCurrentCharacter;
    private GameManager _gameManager;

    private void Awake()
    {
        EventAggregator.Subscribe<OnLeftArrowHeroSelectClick>(LeftArrowClickHandler);
        EventAggregator.Subscribe<OnRightArrowHeroSelectClick>(RightArrowClickHandler);
        EventAggregator.Subscribe<OnSelectHeroButtonClick>(SelectCurrenHero);
    }

    private void OnDestroy()
    {
        EventAggregator.Unsubscribe<OnLeftArrowHeroSelectClick>(LeftArrowClickHandler);
        EventAggregator.Unsubscribe<OnRightArrowHeroSelectClick>(RightArrowClickHandler);
        EventAggregator.Unsubscribe<OnSelectHeroButtonClick>(SelectCurrenHero);
    }

    private void Start()
    {
        _gameManager = ServiceLocator.Resolve<GameManager>();
        IndexOfCurrentCharacter = _gameManager.SelectedHero;
        ActivateCurrendHero();
    }

    private void SelectCurrenHero(object arg1, OnSelectHeroButtonClick arg2)
    {
        EventAggregator.Post(this, new SelectAndSaveCurrentHero { HeroIndex = IndexOfCurrentCharacter });
    }

    private void RightArrowClickHandler(object arg1, OnRightArrowHeroSelectClick arg2)
    {
        IncreaseIndex();
        ActivateCurrendHero();
    }

    private void LeftArrowClickHandler(object arg1, OnLeftArrowHeroSelectClick arg2)
    {
        ReduseIndex();
        ActivateCurrendHero();
    }

    private void ActivateCurrendHero()
    {
        for (int i = 0; i < _characters.Count; i++)
        {
            if(i == IndexOfCurrentCharacter)
            {
                _characters[i].gameObject.SetActive(true);
            }
            else
            {
                _characters[i].gameObject.SetActive(false);
            }
        }

        EventAggregator.Post(this, new TransferCurrentCharacterData { Data = _gameManager.ListOfHeroes[IndexOfCurrentCharacter] });
    }

    private void IncreaseIndex()
    {
        IndexOfCurrentCharacter++;
        if(IndexOfCurrentCharacter >= _characters.Count)
        {
            IndexOfCurrentCharacter = 0;
        }
    }
    private void ReduseIndex()
    {
        IndexOfCurrentCharacter--;
        if(IndexOfCurrentCharacter < 0)
        {
            IndexOfCurrentCharacter = _characters.Count - 1;
        }
    }

    public void RotateHero(float amount)
    {
        transform.rotation = Quaternion.Euler(Vector3.up * -amount);
    }
}

public interface IHeroRotater
{
    void RotateHero(float amount);
}
