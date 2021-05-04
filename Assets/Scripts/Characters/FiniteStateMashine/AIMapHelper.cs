using System.Collections;
using UnityEngine;

public class AIMapHelper : MonoBehaviour
{
    [SerializeField] private Transform[] _enemyGemsSpawnPoints;
    public Transform[] EnemyGemsSpawnPoints => _enemyGemsSpawnPoints;

    [SerializeField] private Transform[] _allyGemsSpawnPoints;
    public Transform[] AllyGemsSpawnPoints => _allyGemsSpawnPoints;

    [SerializeField] private Transform _allyBase;
    public Transform AllyBase => _allyBase;

}
