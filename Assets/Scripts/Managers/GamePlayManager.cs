using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _playerCamera;

    [Header("Game stats")]
    [SerializeField] private float _characterDeathTimer;
    [SerializeField] private float _timerBeforeStartGame;
    [SerializeField] private float _additionalTime;

    public TeamMatchSetup MatchData;
    public List<Transform> RedSpawnPoints = new List<Transform>();
    public List<Transform> BlueSpawnPoints = new List<Transform>();

    private int _redTeamPoints;
    private int _blueTeamPoints;
    private float _matchTimer;
    private IGameTimer _gameTimer;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = ServiceLocator.Resolve<GameManager>();
        MatchData = _gameManager.CurrentTeamMatchData;
        _gameTimer = FindObjectOfType<UpperPanelInGame>();
        EventAggregator.Subscribe<OnGetPoint>(OnGetPointHandler);
        EventAggregator.Subscribe<CharacterDeath>(OnCharacterDeathHadler);
        EventAggregator.Subscribe<OnStartGame>(StartAcluallyGame);
    }

    private void Start()
    {
        StartGameScene();
        EventAggregator.Post(this, new TransferCurrentCharacterData { Data = _gameManager.ListOfHeroes[_gameManager.SelectedHero] });
    }

    private void Update()
    {
        if (_matchTimer > 0)
            _matchTimer -= Time.deltaTime;
        else if(_matchTimer <= 0)
        {
            EndGameHandler();
        }
        _gameTimer.UpdateTimer(_matchTimer);
    }

  

    private void OnDestroy()
    {
        EventAggregator.Unsubscribe<OnGetPoint>(OnGetPointHandler);
        EventAggregator.Unsubscribe<CharacterDeath>(OnCharacterDeathHadler);
        EventAggregator.Unsubscribe<OnStartGame>(StartAcluallyGame);
    }

    private void StartGameScene()
    {
        FillRedTeam();
        FillBlueTeam();
        _redTeamPoints = 0;
        _blueTeamPoints = 0;
        _matchTimer = 0;
        UpdateCrystalCount();
        EventAggregator.Post(this, new OnStartGameScene { GameStartTime = _timerBeforeStartGame }) ;
    }

    private void EndGameHandler()
    {
        Team winner;
        if(_redTeamPoints > _blueTeamPoints)
        {
            winner = Team.Red;
            EventAggregator.Post(this, new OnEndGame { Winner = winner });
        } else if( _blueTeamPoints > _redTeamPoints)
        {
            winner = Team.Blue;
            EventAggregator.Post(this, new OnEndGame { Winner = winner });
        }
        else
        {
            _matchTimer += _additionalTime;
        }
        
    }

    private void StartAcluallyGame(object arg1, OnStartGame arg2)
    {
        _matchTimer = MatchData.BattleDuration;
    }


    private void OnCharacterDeathHadler(object arg1, CharacterDeath data)
    {
        StartCoroutine(HandleCharacterDeath(_characterDeathTimer, data.Character));
    }

    private IEnumerator HandleCharacterDeath(float characterDeathTimer, CharacterHealth character)
    {
        yield return new WaitForSeconds(characterDeathTimer);
        if (character.GetComponent<CharacterIdentifier>().Team == 0)
        {
            character.transform.position = RedSpawnPoints[0].position;
        } else if (character.GetComponent<CharacterIdentifier>().Team == 1)
        {
            character.transform.position = BlueSpawnPoints[0].position;
        }
        character.RenewCharacter();
    }

    private void OnGetPointHandler(object arg1, OnGetPoint data)
    {
        if(data.CharacterTeam == Team.Blue)
        {
            _blueTeamPoints++;
            UpdateCrystalCount();
        } else if(data.CharacterTeam == Team.Red)
        {
            _redTeamPoints++;
            UpdateCrystalCount();
        }
    }
    private void UpdateCrystalCount()
    {
        EventAggregator.Post(this, new UpdateCrystalCounter { CurrentTeam = Team.Blue, CurrentCrystalCount = _blueTeamPoints });
        EventAggregator.Post(this, new UpdateCrystalCounter { CurrentTeam = Team.Red, CurrentCrystalCount = _redTeamPoints });
    }

    private void FillBlueTeam()
    {
        for (int i = 0; i < MatchData.BlueTeam.Count; i++)
        {
            GameObject character = null;
            foreach (var hero in _gameManager.ListOfHeroes)
            {
                if (MatchData.BlueTeam[i] == hero.Name)
                {
                    character = Instantiate(hero.Prefab, BlueSpawnPoints[i].position, Quaternion.identity, BlueSpawnPoints[i]);
                    if (i == 0)
                    {
                        character.GetComponent<CharacterIdentifier>().IsControlledByThePlayer = true;
                        _playerCamera.Follow = character.transform;
                    }
                    else
                    {
                        character.GetComponent<CharacterIdentifier>().IsControlledByThePlayer = false;
                    }
                    character.GetComponent<CharacterIdentifier>().Team = 1;
                    break;
                }
                else { continue; }
            }
        }
    }

    private void FillRedTeam()
    {
        for (int i = 0; i < MatchData.RedTeam.Count; i++)
        {
            GameObject character = null;
            foreach (var hero in _gameManager.ListOfHeroes)
            {
                if (MatchData.RedTeam[i] == hero.Name)
                {
                    character = hero.Prefab;
                    character.GetComponent<CharacterIdentifier>().IsControlledByThePlayer = false;
                    character.GetComponent<CharacterIdentifier>().Team = 0;
                    break;
                }
                else { continue; }
            }
            Instantiate(character, RedSpawnPoints[i].position, Quaternion.identity, RedSpawnPoints[i]);
        }
    }
}
