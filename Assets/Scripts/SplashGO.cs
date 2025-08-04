using UnityEngine;

public class SplashGO : MonoBehaviour
{
    [SerializeField] private CircleCollider2D _circleCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out EnemyGO enemy))
        {
            enemy.Enemy.Health.TakeDamage(GameManager.Instance.PlayerGO.Player.AttackDamages);
        }
    }

    public void DisableDamages()
    {
        _circleCollider.enabled = false;
    }
}
