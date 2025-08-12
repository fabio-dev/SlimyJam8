using System;
using UnityEngine;

public abstract class AWeaponGO : MonoBehaviour
{
    [SerializeField] private int _ammo;
    [SerializeField] public Sprite UiSprite;
    [SerializeField] public Sprite WeaponSprite;

    public event Action<AWeaponGO> OnEmptyAmmo;
    public event Action<AWeaponGO> OnShot;

    public int CurrentAmmo { get; protected set; }
    public int MaxAmmo => _ammo;
    public float AmmoPercentage => MaxAmmo > 0 ? (float)CurrentAmmo / MaxAmmo : 1f;

    public virtual void Shoot(Vector2 shootDirection)
    {
        ShootInner(shootDirection);

        if (_ammo > 0)
        {
            CurrentAmmo--;

            if (CurrentAmmo <= 0)
            {
                OnEmptyAmmo?.Invoke(this);
            }
        }

        OnShot?.Invoke(this);
    }

    protected abstract void ShootInner(Vector2 shootDirection);

    public void ResetAmmo()
    {
        CurrentAmmo = MaxAmmo;
    }
}