using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPhase", menuName = "ScriptableObjects/Spawn/Phase")]
public class SpawnPhase : ScriptableObject
{
    public SpawnEnemy[] Enemies;
    public float PhaseDurationInSeconds = 20f;
    public float DelayToNextSpawnInSeconds = 5f;
    public int NumberOfEnemiesToSpawn = 1;
}
