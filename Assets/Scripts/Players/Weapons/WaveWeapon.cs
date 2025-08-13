using UnityEngine;

public class WaveWeapon : AWeaponGO
{
    [SerializeField] private ProjectileGO _projectilePrefab;
    [SerializeField] private float _knockbackRadius = 1f;
    [SerializeField] private LayerMask _enemyLayerMask = -1;
    [SerializeField] private float _knockbackForce = 5f;
    [SerializeField] private bool _dealSplashDamage = true;
    [SerializeField] private float _splashDamageMultiplier = 1f;

    private Vector2 _shotPosition;

    protected override void ShootInner(Vector2 shootDirection)
    {
        _shotPosition = (Vector2)GameManager.Instance.PlayerGO.GunShotPosition;
        ProjectileGO projectile = Instantiate(_projectilePrefab, GameManager.Instance.PlayerGO.GunShotPosition, Quaternion.identity);
        projectile.transform.right = shootDirection;
        projectile.Launch(shootDirection, GameManager.Instance.PlayerGO.Player.AttackDamages, 0f);
        projectile.OnCollide += KnockbackNearbyEnemies;
    }

    private void KnockbackNearbyEnemies(ProjectileGO projectile)
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(
            projectile.transform.position,
            _knockbackRadius,
            _enemyLayerMask
        );

        Vector2 explosionCenter = projectile.transform.position;

        foreach (Collider2D collider in nearbyColliders)
        {
            EnemyGO enemyGO = collider.GetComponent<EnemyGO>();

            if (enemyGO == null)
            {
                continue;
            }

            Vector2 knockbackDirection = ((Vector2)collider.transform.position - _shotPosition).normalized;

            float distance = Vector2.Distance(explosionCenter, collider.transform.position);
            float distanceRatio = 1f - (distance / _knockbackRadius);

            Rigidbody2D enemyRb = collider.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 knockbackForce = knockbackDirection * _knockbackForce * distanceRatio;
                enemyGO.MovementStrategy.ApplyKnockback(knockbackForce);
            }

            if (_dealSplashDamage)
            {
                float splashDamage = GameManager.Instance.PlayerGO.Player.AttackDamages * _splashDamageMultiplier;
                enemyGO.Enemy.Health.TakeDamage(Mathf.RoundToInt(splashDamage));
            }
        }

        projectile.OnCollide -= KnockbackNearbyEnemies;
    }
}