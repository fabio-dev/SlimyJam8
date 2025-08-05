using UnityEngine;

public class SplashWeapon : AWeaponGO
{
    [SerializeField] private ProjectileGO _projectilePrefab;
    [SerializeField] private float _splashRadius = 1f;

    protected override void ShootInner(Vector2 shootDirection)
    {
        ProjectileGO projectile = Instantiate(_projectilePrefab, GameManager.Instance.PlayerGO.GunShotPosition, Quaternion.identity);
        projectile.transform.right = shootDirection;
        projectile.Launch(shootDirection, GameManager.Instance.PlayerGO.Player.AttackDamages, 0f);
        projectile.OnCollide += CreateArea;
    }

    private void CreateArea(ProjectileGO projectile)
    {
        ZoneManager.Instance.AddZone(new CircleZone(projectile.transform.position, _splashRadius));
        projectile.OnCollide -= CreateArea;
    }
}