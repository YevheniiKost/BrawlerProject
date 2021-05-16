using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static int SelectedHero;

    public List<HeroSelecterData> ListOfHeroes = new List<HeroSelecterData>();

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if(Instance != null)
            Destroy(this);
        else
            Instance = this;

        ServiceLocator.Register(this);
    }
    private void Start()
    {
        LoadHeroes();
    }

    #region Heroes data loading

    public const string HeroesFilePath = "HeroesData/Prefabs";
    public const string IconsFilePath = "HeroesData/SkillsIcons";
    public const string DescriptionFilesPath = "HeroesData/SkillDescription";

    private void LoadHeroes()
    {
        ListOfHeroes.Add(LoadHero(CharacterName.Beor));
        ListOfHeroes.Add(LoadHero(CharacterName.Jisele));
    } 

    private HeroSelecterData LoadHero(CharacterName name)
    {
        HeroSelecterData hero = new HeroSelecterData();
        string heroName = name.ToString();
        hero.Name = name;
        hero.Prefab = Resources.Load<GameObject>($"{HeroesFilePath}/{heroName}");
        hero.FirstSkillImage = Resources.Load<Sprite>($"{IconsFilePath}/{heroName}_FirstSkill");
        hero.SecondSkillImage = Resources.Load<Sprite>($"{IconsFilePath}/{heroName}_SecondSkill");
        hero.FirstSkillDescription = Resources.Load<TextAsset>($"{DescriptionFilesPath}/{heroName}_FirstSkill");
        hero.SecondSkillDescription = Resources.Load<TextAsset>($"{DescriptionFilesPath}/{heroName}_SecondSkill");
        return hero;
    }

    #endregion
}

public enum GameMode
{
    TeamFight,
    DeathMatch
}

[Serializable]
public class HeroSelecterData
{
    public CharacterName Name;
    public GameObject Prefab;
    public Sprite FirstSkillImage;
    public Sprite SecondSkillImage;
    public TextAsset FirstSkillDescription;
    public TextAsset SecondSkillDescription;
}
