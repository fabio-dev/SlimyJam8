using System;
using UnityEngine;

public abstract class AWeaponGO : MonoBehaviour
{
    [SerializeField] private int _ammo;
    [SerializeField] public Sprite WeaponSprite;

    public event Action OnEmptyAmmo;
    public event Action OnShot;

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
                OnEmptyAmmo?.Invoke();
            }
        }

        OnShot?.Invoke();
    }

    protected abstract void ShootInner(Vector2 shootDirection);

    public void ResetAmmo()
    {
        CurrentAmmo = MaxAmmo;
    }
}