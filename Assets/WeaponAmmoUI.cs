using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponAmmoUI : MonoBehaviour
{
    [SerializeField] private Transform _ammoBar;
    [SerializeField] private Image _frontBar;

    private PlayerGO _playerGO;
    private AWeaponGO _currentWeapon;

    public void Setup(PlayerGO player)
    {
        _playerGO = player;
        _playerGO.OnWeaponChanged += WeaponChanged;
    }

    private void WeaponChanged(AWeaponGO weapon)
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.OnShot -= Shot;
            _currentWeapon.OnEmptyAmmo -= Shot;
        }

        _currentWeapon = weapon;

        if (weapon is BasicWeapon)
        {
            StartCoroutine(DisableAmmoBar());
            return;
        }

        _ammoBar.gameObject.SetActive(true);
        _frontBar.fillAmount = weapon.AmmoPercentage;
        weapon.OnShot += Shot;
        weapon.OnEmptyAmmo += Shot;
    }

    private IEnumerator DisableAmmoBar()
    {
        yield return new WaitForSeconds(.3f);
        _ammoBar.gameObject.SetActive(false);
    }

    private void Shot(AWeaponGO weapon)
    {
        _frontBar.DOFillAmount(weapon.AmmoPercentage, .3f);
    }
}
