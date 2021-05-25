using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private float _teamMatchDuration;
    [SerializeField, Range(1, 3)] private int _numberOfHeroesInTeam;

    public int SelectedHero;
    private readonly string SelectedHeroKey = "SelecterHero";

    public List<HeroSelecterData> ListOfHeroes = new List<HeroSelecterData>();

    public int HumberOfHeroesInTeam;

    public TeamMatchSetup CurrentTeamMatchData;

    private UIManager _uiManager;
    private AudioManager _audioManger;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;

        ServiceLocator.Register(this);

        SubscribeOnEvents();

        SelectedHero = PlayerPrefs.GetInt(SelectedHeroKey);

        LoadHeroes();
    }

    private void OnDestroy()
    {
        UnsubscribeOnEvents();
    }

    private void Start()
    {
        GenerateMatchData();
        _uiManager = ServiceLocator.Resolve<UIManager>();
        _audioManger = ServiceLocator.Resolve<AudioManager>();
        _audioManger.PlayMusic(MusicType.MainMenu);
    }

    #region Scene management
    private void ProcessPlayClick(object arg1, OnPlayClick arg2)
    {
        SceneManager.LoadScene(GameConstants.Scenes.TeamMatch);
        _audioManger.PlayMusic(MusicType.Game);
    }

    private void LoadMainMenuScene(object arg1, OnStartScenePlayClick arg2)
    {
        SceneManager.LoadScene(GameConstants.Scenes.MainMenu);
    }

    private void ReturnToMainMenu(object arg1, OnReturnToMainMenuClick arg2)
    {
        if (_uiManager.Windows.ContainsKey(typeof(ConfirmationWindow)))
        {
            UnpauseGame();
            _uiManager.CreateConfirmationWindow(Return, " ");
        }
        else
        {
            UnpauseGame();
            Return();
        }
    }
    private void Return()
    {
        _audioManger.PlayMusic(MusicType.MainMenu);
        SceneManager.LoadScene(GameConstants.Scenes.StartMenu);
    }
    private void ExitGameHandler(object arg1, OnExitGameClickes arg2) => _uiManager.CreateConfirmationWindow(ExitGame, "Exit game?");
    private void ExitGame() => Application.Quit();
    private void PauseGameHandler(object arg1, OnGamePaused arg2) => PauseGame();
    private void UnpauseGameHandler(object arg1, OnGameUnpased arg2) => UnpauseGame();
    private void RestartTeamFightHandler(object arg1, OnRestartTeamFirght arg2) => _uiManager.CreateConfirmationWindow(RestartTeamFigth, " ");
    private void RestartTeamFigth()
    {
        UnpauseGame();
        _audioManger.PlayMusic(MusicType.Game);
        SceneManager.LoadScene(GameConstants.Scenes.TeamMatch);
    }
    private void EndGameHandler(object arg1, OnEndGame arg2) => PauseGame();
    private void PauseGame() => Time.timeScale = 0;
    private void UnpauseGame() => Time.timeScale = 1;

    #endregion

    #region Work with Match data
    private void GiveTeamMatchData(object arg1, RequestForTeamMatchData arg2)
    {
        EventAggregator.Post(this, new TakeTeamFightData { MatchData = CurrentTeamMatchData });
    }
    private void SelectAndSaveCurrentCharacter(object arg1, SelectAndSaveCurrentHero data)
    {
        SelectedHero = data.HeroIndex;
        GenerateMatchData();
        PlayerPrefs.SetInt(SelectedHeroKey, SelectedHero);
    }
    private void GenerateMatchData()
    {
        int numerInTeam = _numberOfHeroesInTeam;
        var list = new List<CharacterName>(ListOfRandomCharacters(numerInTeam * 2));
        var blueList = new List<CharacterName>(list);
        blueList.RemoveRange(numerInTeam, numerInTeam);
        var redList = new List<CharacterName>(list);
        redList.RemoveRange(0, numerInTeam);
        blueList[0] = ListOfHeroes[SelectedHero].Name;
        var data = new TeamMatchSetup(redList, blueList, blueList[0], _teamMatchDuration, numerInTeam);
        CurrentTeamMatchData = data;
    }
    private List<CharacterName> ListOfRandomCharacters(int lenght)
    {
        List<CharacterName> list = new List<CharacterName>();
        var numberOfCharacters = Enum.GetValues(typeof(CharacterName)).Length;
        for (int i = 0; i < lenght; i++)
        {
            CharacterName name;
            if (i < numberOfCharacters)
            {
                name = (CharacterName)Enum.GetValues(typeof(CharacterName)).GetValue(i);
            }
            else
            {
                name = (CharacterName)UnityEngine.Random.Range(0, numberOfCharacters);
            }
            if (list.Contains(name))
            {
                for (int y = 0; y < numberOfCharacters; y++)
                {
                    name = (CharacterName)Enum.GetValues(name.GetType()).GetValue(y);
                    if (!list.Contains(name))
                    {
                        list.Add(name);
                        break;
                    }
                    else
                        continue;
                }
                name = (CharacterName)UnityEngine.Random.Range(0, numberOfCharacters);
                list.Add(name);
                continue;
            }
            else
            {
                list.Add(name);
                continue;
            }
        }

        return list;
    }

    #endregion

    #region Heroes data loading

    public const string HeroesFilePath = "HeroesData/Prefabs";
    public const string IconsFilePath = "HeroesData/SkillsIcons";
    public const string DescriptionFilesPath = "HeroesData/SkillDescription";

    private void LoadHeroes()
    {
        ListOfHeroes.Add(LoadHero(CharacterName.Beor));
        ListOfHeroes.Add(LoadHero(CharacterName.Jisele));
        ListOfHeroes.Add(LoadHero(CharacterName.Mork));
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

    private void SubscribeOnEvents()
    {
        EventAggregator.Subscribe<RequestForTeamMatchData>(GiveTeamMatchData);
        EventAggregator.Subscribe<SelectAndSaveCurrentHero>(SelectAndSaveCurrentCharacter);
        EventAggregator.Subscribe<OnPlayClick>(ProcessPlayClick);
        EventAggregator.Subscribe<OnStartScenePlayClick>(LoadMainMenuScene);
        EventAggregator.Subscribe<OnExitGameClickes>(ExitGameHandler);
        EventAggregator.Subscribe<OnReturnToMainMenuClick>(ReturnToMainMenu);
        EventAggregator.Subscribe<OnRestartTeamFirght>(RestartTeamFightHandler);
        EventAggregator.Subscribe<OnGamePaused>(PauseGameHandler);
        EventAggregator.Subscribe<OnGameUnpased>(UnpauseGameHandler);
        EventAggregator.Subscribe<OnEndGame>(EndGameHandler);
    }


    private void UnsubscribeOnEvents()
    {
        EventAggregator.Unsubscribe<RequestForTeamMatchData>(GiveTeamMatchData);
        EventAggregator.Unsubscribe<SelectAndSaveCurrentHero>(SelectAndSaveCurrentCharacter);
        EventAggregator.Unsubscribe<OnPlayClick>(ProcessPlayClick);
        EventAggregator.Unsubscribe<OnStartScenePlayClick>(LoadMainMenuScene);
        EventAggregator.Unsubscribe<OnExitGameClickes>(ExitGameHandler);
        EventAggregator.Unsubscribe<OnReturnToMainMenuClick>(ReturnToMainMenu);
        EventAggregator.Unsubscribe<OnRestartTeamFirght>(RestartTeamFightHandler);
        EventAggregator.Unsubscribe<OnGamePaused>(PauseGameHandler);
        EventAggregator.Unsubscribe<OnGameUnpased>(UnpauseGameHandler);
        EventAggregator.Unsubscribe<OnEndGame>(EndGameHandler);
    }

    
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

[Serializable]
public class TeamMatchSetup
{
    public List<CharacterName> RedTeam = new List<CharacterName>();
    public List<CharacterName> BlueTeam = new List<CharacterName>();

    public CharacterName PlayerCharacter;

    public float BattleDuration;

    public int NumberOfCharactesInTeam;

    public TeamMatchSetup(List<CharacterName> redTeam, List<CharacterName> blueTeam, CharacterName playerCharacter, float duration, int numberInTeam)
    {
        RedTeam = redTeam;
        BlueTeam = blueTeam;
        PlayerCharacter = playerCharacter;
        BattleDuration = duration;
        NumberOfCharactesInTeam = numberInTeam;
    }

}
