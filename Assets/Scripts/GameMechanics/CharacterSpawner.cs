using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public Team SpawnerTeam => _team;

    [SerializeField] private Team _team;
   
}

public enum Team
{
    Red,
    Blue
}
