using UnityEngine;

public class BasicWeapon : AWeapon
{
    [SerializeField] private ProjectileGO _projectilePrefab;

    public override void Shoot(Vector2 shootDirection)
    {
        ProjectileGO projectile = Instantiate(_projectilePrefab, GameManager.Instance.PlayerGO.GunShotPosition, Quaternion.identity);
        projectile.transform.right = shootDirection;
        projectile.Launch(shootDirection, GameManager.Instance.PlayerGO.Player.AttackDamages, 0f);
    }
}