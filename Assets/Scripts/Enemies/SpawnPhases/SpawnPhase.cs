using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPhase", menuName = "ScriptableObjects/Spawn/Phase")]
public class SpawnPhase : ScriptableObject
{
    public SpawnEnemy[] Enemies;
    public float PhaseDurationInSeconds;
    public float MinDelayToSpawnInSeconds;
    public float MaxDelayToSpawnInSeconds;
}
