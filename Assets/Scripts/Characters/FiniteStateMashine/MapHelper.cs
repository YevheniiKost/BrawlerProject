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
    public CharacterIdentifier GetNearestEnemy(CharacterIdentifier me)
    {
        if(me.Team == 0)
        {
            var nearest = BlueTeamCharactes.OrderBy(t => Vector2.Distance(me.transform.position, t.transform.position)).FirstOrDefault();
            return nearest;
        } else if(me.Team == 1)
        {
            var nearest = RedTeamCharactes.OrderBy(t => Vector2.Distance(me.transform.position, t.transform.position)).FirstOrDefault();
            return nearest;
        }
        else
        {
            Debug.LogError($"Character {me.name} identifier error");
            return null;
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
    #endregion

    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void Start()
    {
        FillContainers();
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister(this);
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


}
