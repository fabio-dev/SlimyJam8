using UnityEngine;

public class ExplodeOnDeath : MonoBehaviour
{
    [SerializeField] private ProjectileGO _projectilePrefab;
    [SerializeField] private int _numberOfProjectiles = 10;
    [SerializeField] private float _damage = 1f;

    private EnemyGO _enemyGO;

    public void Setup(EnemyGO enemyGO)
    {
        _enemyGO = enemyGO;
        enemyGO.OnDieAnimationEnded += Explode;
    }

    private void Explode()
    {
        float angleStep = 360f / _numberOfProjectiles;

        for (int i = 0; i < _numberOfProjectiles; i++)
        {
            float currentAngle = angleStep * i;

            Vector2 projectileDirection = new Vector2(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad)
            ).normalized;


            ProjectileGO projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
            projectile.transform.right = projectileDirection;
            projectile.Launch(projectileDirection, _damage, 0f);
        }

        if (_enemyGO != null && _enemyGO.Enemy != null)
        {
            _enemyGO.OnDieAnimationEnded -= Explode;
        }
    }

    private void Start()
    {
    }
}
