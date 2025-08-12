using UnityEngine;

public class ShotgunWeapon : AWeaponGO
{
    [SerializeField] private ProjectileGO _projectilePrefab;
    [SerializeField] private int _numberOfProjectiles = 3;
    [SerializeField] private float _spreadAngle = 30f;

    protected override void ShootInner(Vector2 shootDirection)
    {
        float baseAngle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

        float angleStep = _numberOfProjectiles > 1 ? _spreadAngle / (_numberOfProjectiles - 1) : 0f;
        float startAngle = baseAngle - (_spreadAngle / 2f);

        for (int i = 0; i < _numberOfProjectiles; i++)
        {
            float currentAngle = startAngle + (angleStep * i);

            Vector2 projectileDirection = new Vector2(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad)
            ).normalized;

            ProjectileGO projectile = Instantiate(_projectilePrefab, GameManager.Instance.PlayerGO.GunShotPosition, Quaternion.identity);
            projectile.transform.right = projectileDirection;
            projectile.Launch(projectileDirection, GameManager.Instance.PlayerGO.Player.AttackDamages, 0f);
        }
    }
}