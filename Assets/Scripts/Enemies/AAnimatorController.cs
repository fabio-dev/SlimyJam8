using UnityEngine;

public abstract class AAnimatorController : MonoBehaviour
{
    public abstract void Setup(EnemyGO enemyGO);

    public abstract void Kill();
}
