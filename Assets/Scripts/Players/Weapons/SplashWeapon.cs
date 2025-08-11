using UnityEngine;

public class SplashWeapon : AWeaponGO
{
    [SerializeField] private ProjectileGO _projectilePrefab;
    [SerializeField] private float _splashRadius = 1f;
    [SerializeField] private float _secondsBetweenSplashes = .1f;

    protected override void ShootInner(Vector2 shootDirection)
    {
        ProjectileGO projectile = Instantiate(_projectilePrefab, GameManager.Instance.PlayerGO.GunShotPosition, Quaternion.identity);
        projectile.transform.right = shootDirection;

        Vector3 scale = projectile.transform.localScale;
        if (shootDirection.x < 0)
        {
            scale.y = -Mathf.Abs(scale.y);
        }
        else
        {
            scale.y = Mathf.Abs(scale.y);
        }

        projectile.transform.localScale = scale;
        projectile.Launch(shootDirection, GameManager.Instance.PlayerGO.Player.AttackDamages, 0f);
        projectile.SplashEvery(_secondsBetweenSplashes, _splashRadius);
    }
}
