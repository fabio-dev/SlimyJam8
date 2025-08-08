using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private Image _weaponImage;

    private PlayerGO _player;
    private AWeaponGO _weapon;

    public void Setup(PlayerGO player)
    {
        _player = player;
        _player.OnWeaponChanged += WeaponChanged;

        WeaponChanged(_player.Weapon);
    }

    private void WeaponChanged(AWeaponGO weapon)
    {
        if (_weapon != null)
        {
            UnregisterWeaponEvents();
        }

        _weapon = weapon;

        _weaponImage.sprite = _weapon.WeaponSprite;
        _fillImage.DOFillAmount(_weapon.AmmoPercentage, .3f);

        RegisterWeaponEvents();
    }

    private void UnregisterWeaponEvents()
    {
        _weapon.OnShot -= OnShot;
    }

    private void RegisterWeaponEvents()
    {
        _weapon.OnShot += OnShot;
    }

    private void OnShot(AWeaponGO weapon)
    {
        if (weapon.MaxAmmo > 0)
        {
            _fillImage.DOFillAmount(weapon.AmmoPercentage, .3f);
        }
    }
}
