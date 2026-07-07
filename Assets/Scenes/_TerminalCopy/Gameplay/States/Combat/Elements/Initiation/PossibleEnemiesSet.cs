using System.Collections.Generic;
using UnityEngine;

public partial class PossibleEnemiesSet : MonoBehaviour {
    public float Weight => weight;
    public List<EnemyAndSpawnLocation> EnemiesAndSpawnLocation => enemiesAndSpawnLocation;

    [SerializeField] float weight;
    [SerializeField] List<EnemyAndSpawnLocation> enemiesAndSpawnLocation;
}
