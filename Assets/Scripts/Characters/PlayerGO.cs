using Assets.Scripts.Domain;
using Assets.Scripts.Domain.Collectibles;
using DG.Tweening;
using System;
using UnityEngine;

public class PlayerGO : ACharacterGO
{
    [SerializeField] private SpriteRenderer _shadowSpriteRenderer;
    [SerializeField] private SpriteRenderer _weaponSpriteRenderer;
    [SerializeField] private Transform _center;
    [SerializeField] private Transform _gun;
    [SerializeField] private Transform _gunSprite;
    [SerializeField] private Transform _body;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _gunShotPosition;
    [SerializeField] private GameObject _splashParticles;
    [SerializeField] private ShieldGO _shield;

    [Header("Weapons")]
    [SerializeField] private AWeaponGO _basicWeapon;
    [SerializeField] private AWeaponGO _splashWeapon;
    [SerializeField] private AWeaponGO _waveWeapon;
    [SerializeField] private AWeaponGO _megaRayWeapon;
    [SerializeField] private AWeaponGO _shotgunWeapon;
    [SerializeField] private WeaponAmmoUI _weaponAmmoUI;

    private AWeaponGO _currentWeapon;
    private PlayerAnimatorController _animatorController;
    private BasePlayerInput[] _inputs;
    private Cooldown _invulnerableCooldown;
    private bool _invulnerableCooldownStarted;

    public event Action<AWeaponGO> OnWeaponChanged;

    public Player Player => Character as Player;
    public AWeaponGO Weapon => _currentWeapon;

    public Transform Center => _center;

    public Transform Body => _body;

    public PlayerState State => Player.State;

    public Vector3 GunShotPosition => _gunShotPosition.position;

    public float AttackCooldown => _currentWeapon.AttackCooldownModifier * Player.AttackCooldown;

    public void ShowGun()
    {
        _gunSprite.gameObject.SetActive(true);
    }

    public void HideGun()
    {
        _gunSprite.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_invulnerableCooldownStarted && !_invulnerableCooldown.IsRunning())
        {
            _invulnerableCooldownStarted = false;
            _invulnerableCooldown.Stop();

            if (!_shield.IsShielded())
            {
                Player.Vulnerable();
            }
        }
    }

    private void HandlePlayerOrientation(Vector2 direction)
    {
        SpriteRenderer.transform.localScale = direction.x < 0f ? new Vector3(-1f, 1f, 1f) : Vector3.one;
    }

    private void HandleGunOrientation(Vector2 direction)
    {
        if (direction.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _gun.rotation = Quaternion.Euler(0, 0, angle);

            bool shouldFlipGun;
            if (UnifiedLookInput.IsUsingMouse)
            {
                shouldFlipGun = UnifiedLookInput.CurrentLookInput.x < Screen.width / 2f;
            }
            else
            {
                shouldFlipGun = direction.x < 0;
            }

            _gunSprite.localScale = shouldFlipGun ? new Vector3(1f, -1f, 1f) : Vector3.one;
        }
    }

    private void Start()
    {
        UnifiedLookInput.OnLookInput += OnLookAround;
    }

    public override void Setup(ACharacter character)
    {
        _inputs = GetComponents<BasePlayerInput>();
        _animatorController = GetComponent<PlayerAnimatorController>();
        _shield.OnBreak += BreakShield;

        if (Character != null)
        {
            UnregisterEvents();
        }

        base.Setup(character);

        if (character is Player player)
        {
            _invulnerableCooldown = new Cooldown(player.InvulnerabilityDuration);
        }

        RegisterEvents();
        _animatorController.Setup(this);

        _weaponAmmoUI.Setup(this);
        ChangeWeapon(WeaponType.Basic);

        TriggerOnSetup();
    }

    private void OnLookAround(Vector2 position, bool isMouse)
    {
        Vector2 test = UnifiedLookInput.GetWorldDirection(transform.position);

        Debug.Log("Looking around: " + test);
        HandlePlayerOrientation(test);
        HandleGunOrientation(test);
    }

    protected override void OnDie(ACharacter character)
    {
        HideGun();
        UnregisterEvents();

        base.OnDie(character);
    }

    public void Pause()
    {
        foreach (BasePlayerInput input in _inputs)
        {
            input.enabled = false;
        }
    }

    public void Resume()
    {
        foreach (BasePlayerInput input in _inputs)
        {
            input.enabled = true;
        }
    }

    private void RegisterEvents()
    {
        Player.OnJumpStart += JumpStart;
        Player.OnJumpEnd += JumpEnd;
        Player.OnDamaged += OnDamaged;
        Player.OnDie += Dying;
        Player.OnShielded += OnShielded;
        Player.OnWeaponChanged += ChangeWeapon;

        _splashWeapon.OnEmptyAmmo += OnEmptyAmmo;
        _waveWeapon.OnEmptyAmmo += OnEmptyAmmo;
        _megaRayWeapon.OnEmptyAmmo += OnEmptyAmmo;
        _shotgunWeapon.OnEmptyAmmo += OnEmptyAmmo;
    }

    private void OnEmptyAmmo(AWeaponGO weapon)
    {
        ChangeWeapon(WeaponType.Basic);
    }

    private void ChangeWeapon(WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponType.Splash:
                _currentWeapon = _splashWeapon;
                break;
            case WeaponType.Wave:
                _currentWeapon = _waveWeapon;
                break;
            case WeaponType.MegaRay:
                _currentWeapon = _megaRayWeapon;
                break;
            case WeaponType.Shotgun:
                _currentWeapon = _shotgunWeapon;
                break;
            default:
                _currentWeapon = _basicWeapon;
                break;
        }

        _currentWeapon.ResetAmmo();
        _weaponSpriteRenderer.sprite = _currentWeapon.WeaponSprite;
        OnWeaponChanged?.Invoke(_currentWeapon);
    }

    private void OnShielded(float duration)
    {
        _shield.Begin(duration);
        Player.Invulnerable();
    }

    private void BreakShield()
    {
        Player.Vulnerable();
    }

    private void Dying(ACharacter character)
    {
        Pause();
    }

    private void OnDamaged(float obj)
    {
        Player.Invulnerable();
        _invulnerableCooldownStarted = true;
        _invulnerableCooldown.Start();

        SpriteRenderer
          .DOFade(0f, .1f)
          .SetLoops(5, LoopType.Yoyo)
          .OnComplete(() => SpriteRenderer.DOFade(1f, 0f));

        SFXPlayer.Instance.PlayPlayerHurt();
    }

    private void UnregisterEvents()
    {
        Player.OnJumpStart -= JumpStart;
        Player.OnJumpEnd -= JumpEnd;
        Player.OnDamaged -= OnDamaged;
        Player.OnDie -= Dying;
        Player.OnShielded -= OnShielded;
        Player.OnWeaponChanged -= ChangeWeapon;

        _splashWeapon.OnEmptyAmmo -= OnEmptyAmmo;
        _waveWeapon.OnEmptyAmmo -= OnEmptyAmmo;
        _megaRayWeapon.OnEmptyAmmo -= OnEmptyAmmo;
        _shotgunWeapon.OnEmptyAmmo -= OnEmptyAmmo;
    }

    private void JumpStart()
    {
        _shadowSpriteRenderer.gameObject.SetActive(true);
    }

    private void JumpEnd()
    {
        _shadowSpriteRenderer.gameObject.SetActive(false);

        if (!ZoneManager.Instance.IsInsideAnyZone(transform.position) && Player.State != PlayerState.Splashing)
        {
            Player.Health.TakeDamage(1f);
            ZoneManager.Instance.AddZone(new CircleZone(transform.position, 1f));
        }

        GameObject particles = Instantiate(_splashParticles, transform.position, Quaternion.identity);
        Destroy(particles, 1f);
    }

    internal void Shoot(Vector2 shootDirection)
    {
        if (_currentWeapon != null && !Player.Health.IsDead())
        {
            _currentWeapon.Shoot(shootDirection);
        }
    }
}
