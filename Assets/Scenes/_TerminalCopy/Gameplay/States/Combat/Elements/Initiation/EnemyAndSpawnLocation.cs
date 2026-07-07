using UnityEngine;

public partial class EnemyAndSpawnLocation : MonoBehaviour {
    public Enemy Enemy => enemy;
    public Vector2 SpawnLocation => spawnLocation;

    [SerializeField] Enemy enemy;
    [SerializeField] Vector2 spawnLocation;
}
