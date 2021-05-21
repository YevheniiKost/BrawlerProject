using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapHelper : MonoBehaviour
{
    public readonly List<CharacterIdentifier> RedTeamCharactes = new List<CharacterIdentifier>();

    public readonly List<CharacterIdentifier> BlueTeamCharactes = new List<CharacterIdentifier>();

    public readonly List<CrystalSpot> RedCrystalsList = new List<CrystalSpot>();

    public readonly List<CrystalSpot> BlueCrystalsList = new List<CrystalSpot>();
   
    public Transform RedCharacterSpawner;

    public Transform BlueCharacterSpawner;

    #region Helpers

    public bool IsAllEnemiesDead(CharacterIdentifier me)
    {
        if (me.Team == 1)
        {
            foreach (var enemy in RedTeamCharactes)
            {
                if (enemy.GetComponent<CharacterHealth>().GetLifeStatus() != LifeStatus.Dead)
                {
                    return false;
                }
            }
            return true;
        } else if(me.Team == 0)
        {
            foreach (var enemy in BlueTeamCharactes)
            {
                if (enemy.GetComponent<CharacterHealth>().GetLifeStatus() != LifeStatus.Dead)
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            Debug.LogError($"Character {me.name} identifier error");
            return false;
        }
    }

    public bool IsAllFriendAreDead(CharacterIdentifier me)
    {
        if (me.Team == 0)
        {
            foreach (var enemy in RedTeamCharactes)
            {
                if (enemy.GetComponent<CharacterHealth>().GetLifeStatus() != LifeStatus.Dead)
                {
                    return false;
                }
            }
            return true;
        }
        else if (me.Team == 1)
        {
            foreach (var enemy in BlueTeamCharactes)
            {
                if (enemy.GetComponent<CharacterHealth>().GetLifeStatus() != LifeStatus.Dead)
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            Debug.LogError($"Character {me.name} identifier error");
            return false;
        }
    }

    public CharacterIdentifier GetNearestEnemy(CharacterIdentifier me)
    {
        if (IsAllEnemiesDead(me))
        {
            return null;
        }
        else
        {
            if (me.Team == 0)
            {
                var nearest = BlueTeamCharactes.OrderBy(t => Vector3.Distance(me.transform.position, t.transform.position));
                foreach (var near in nearest)
                {
                    if (near.GetComponent<CharacterHealth>().GetLifeStatus() == LifeStatus.Dead)
                        continue;
                    else
                        return near;
                }
                return null;
            }
            else if (me.Team == 1)
            {
                var nearest = RedTeamCharactes.OrderBy(t => Vector3.Distance(me.transform.position, t.transform.position));
                foreach (var near in nearest)
                {
                    if (near.GetComponent<CharacterHealth>().GetLifeStatus() == LifeStatus.Dead)
                        continue;
                    else
                        return near;
                }
                return null;
            }
            else
            {
                Debug.LogError($"Character {me.name} identifier error");
                return null;
            }
        }
    }

    public CharacterIdentifier GetNearestFriends(CharacterIdentifier me)
    {
        if (IsAllFriendAreDead(me))
        {
            return null;
        }
        else
        {
            if (me.Team == 1)
            {
                var nearest = BlueTeamCharactes.OrderBy(t => Vector2.Distance(me.transform.position, t.transform.position)).ToList();
                return nearest[1];
            }
            else if (me.Team == 0)
            {
                var nearest = RedTeamCharactes.OrderBy(t => Vector2.Distance(me.transform.position, t.transform.position)).ToList();
                return nearest[1];
            }
            else
            {
                Debug.LogError($"Character {me.name} identifier error");
                return null;
            }
        }
    }

    public float DistanceToNearestEnemy(CharacterIdentifier me)
    {
        return Vector3.Distance(me.transform.position, GetNearestEnemy(me).transform.position);
    }

    public Transform MyTeamSpawner(CharacterIdentifier me)
    {
        if (me.Team == 0)
            return RedCharacterSpawner;
        else if (me.Team == 1)
            return BlueCharacterSpawner;
        else
        {
            Debug.LogError($"Character {me.name} identifier error");
            return null;
        }
    }

    public Transform LocateNearestActiveEnemyCrystal(CharacterIdentifier me)
    {
        int searchIndex = 0;
        if (me.Team == 0)
        {
            var nearestCrystals = BlueCrystalsList.OrderBy(t => Vector3.Distance(me.transform.position, t.transform.position)).ToArray();
            while (searchIndex < nearestCrystals.Length)
            {
                if (nearestCrystals[searchIndex].IsCrystalOn)
                {
                    return nearestCrystals[searchIndex].transform;
                }
                else
                {
                    searchIndex++;
                }
            } return null;

        } else if(me.Team == 1)
        {
            var nearestCrystals = RedCrystalsList.OrderBy(t => Vector3.Distance(me.transform.position, t.transform.position)).ToArray();
            while (searchIndex < nearestCrystals.Length)
            {
                if (nearestCrystals[searchIndex].IsCrystalOn)
                {
                    return nearestCrystals[searchIndex].transform;
                }
                else
                {
                    searchIndex++;
                }
            }
            return null;
        }
        else
        {
            Debug.LogError($"Character {me.name} identifier error");
            return null;
        }
    }

    public Transform LocateNearestActiveFriendlyCrystal(CharacterIdentifier me)
    {
        int searchIndex = 0;
        if (me.Team == 1)
        {
            var nearestCrystals = BlueCrystalsList.OrderBy(t => Vector3.Distance(me.transform.position, t.transform.position)).ToArray();
            while (searchIndex < nearestCrystals.Length)
            {
                if (nearestCrystals[searchIndex].IsCrystalOn)
                {
                    return nearestCrystals[searchIndex].transform;
                }
                else
                {
                    searchIndex++;
                }
            }
            return null;
        }
        else if (me.Team == 0)
        {
            var nearestCrystals = RedCrystalsList.OrderBy(t => Vector3.Distance(me.transform.position, t.transform.position)).ToArray();
            while (searchIndex < nearestCrystals.Length)
            {
                if (nearestCrystals[searchIndex].IsCrystalOn)
                {
                    return nearestCrystals[searchIndex].transform;
                }
                else
                {
                    searchIndex++;
                }
            }
            return null;
        }
        else
        {
            Debug.LogError($"Character {me.name} identifier error");
            return null;
        }
    }

    public bool IsSomeOfEnemiesCrystalsActive(CharacterIdentifier me)
    {
        if (me.Team == 0)
        {
            foreach (var crystalSpot in BlueCrystalsList)
            {
                if (crystalSpot.IsCrystalOn)
                {
                    return true;
                }
                else
                    continue;
            }
            return false;
        } else if( me.Team == 1)
        {
            foreach (var crystalSpot in RedCrystalsList)
            {
                if (crystalSpot.IsCrystalOn)
                {
                    return true;
                }
                else
                    continue;
            }
            return false;
        }
        else
        {
            Debug.LogError($"Character {me.name} identifier error");
            return false;
        }
    }

    public bool IsSomeFriendlyCrystalsActive(CharacterIdentifier me)
    {
        if (me.Team == 1)
        {
            foreach (var crystalSpot in BlueCrystalsList)
            {
                if (crystalSpot.IsCrystalOn)
                {
                    return true;
                }
                else
                    continue;
            }
            return false;
        }
        else if (me.Team == 0)
        {
            foreach (var crystalSpot in RedCrystalsList)
            {
                if (crystalSpot.IsCrystalOn)
                {
                    return true;
                }
                else
                    continue;
            }
            return false;
        }
        else
        {
            Debug.LogError($"Character {me.name} identifier error");
            return false;
        }
    }
    #endregion

    private void Awake()
    {
        ServiceLocator.Register(this);
        EventAggregator.Subscribe<OnStartGameScene>(OnStartGameHandler);
        EventAggregator.Subscribe<OnStartGame>(OnStartActuallyGameHandler);
    }

    
    private void OnStartGameHandler(object arg1, OnStartGameScene arg2)
    {
        FillContainers();
        StopAllCharacters();
    }
    private void OnStartActuallyGameHandler(object arg1, OnStartGame arg2)
    {
        ReleaseAllCharacters();
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister(this);
        EventAggregator.Unsubscribe<OnStartGameScene>(OnStartGameHandler);
        EventAggregator.Unsubscribe<OnStartGame>(OnStartActuallyGameHandler);
    }

    private void FillContainers()
    {
        foreach(var character in FindObjectsOfType<CharacterIdentifier>())
        {
            if(character.Team == 0)
            {
                RedTeamCharactes.Add(character);
                continue;
            } else if(character.Team == 1)
            {
                BlueTeamCharactes.Add(character);
                continue;
            }
            else
            {
                Debug.Log($"Character`s {character.name} team in this game must be 0 or 1");
                continue;
            }
        }

        foreach(var crystalSpot in FindObjectsOfType<CrystalSpot>())
        {
            if(crystalSpot.SpotTeam == Team.Red)
            {
                RedCrystalsList.Add(crystalSpot);
                continue;
            } else if(crystalSpot.SpotTeam == Team.Blue)
            {
                BlueCrystalsList.Add(crystalSpot);
                continue;
            }
            else
            {
                Debug.Log($"Crystal spot {crystalSpot.name} has wrong identifier");
            }
        }
    }

    private void StopAllCharacters()
    {
        foreach (var hero in RedTeamCharactes)
        {
            hero.GetComponent<CharacterMovement>().ProcessForcedStop();
        }
        foreach (var hero in BlueTeamCharactes)
        {
            hero.GetComponent<CharacterMovement>().ProcessForcedStop();
        }
    }

    private void ReleaseAllCharacters()
    {
        foreach (var hero in RedTeamCharactes)
        {
            hero.GetComponent<CharacterMovement>().UndoForcedStop();
        }
        foreach (var hero in BlueTeamCharactes)
        {
            hero.GetComponent<CharacterMovement>().UndoForcedStop();
        }
    }

}
