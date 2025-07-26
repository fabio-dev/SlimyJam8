using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPhase", menuName = "ScriptableObjects/Spawn/Enemy")]
public class SpawnEnemy : ScriptableObject
{
    public EnemyGO Enemy;
    public int Weight = 1;
}
