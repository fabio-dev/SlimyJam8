using Assets.Scripts.Domain;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerGO : ACharacterGO
{
	[SerializeField] private SpriteRenderer _shadowSpriteRenderer;
	[SerializeField] private Transform _center;
    [SerializeField] private Transform _gun;
    [SerializeField] private Transform _gunSprite;
	[SerializeField] private Camera _camera;
	[SerializeField] private Transform _gunShotPosition;
	[SerializeField] private PlayerAnimatorController _animatorController;
	[SerializeField] private BasePlayerInput[] _inputs;

    private Cooldown _invulnerableCooldown;
	private bool _invulnerableCooldownStarted;

	public Player Player => Character as Player;

	public Transform Center => _center;

    public PlayerState State => Player.State;

	public Vector3 GunShotPosition => _gunShotPosition.position;

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
            Player.Health.Vulnerable();
        }

        Vector2 mouseScreenPos = InputManager.Instance.Player.Value.Look.ReadValue<Vector2>();

        HandlePlayerOrientation(mouseScreenPos);
        HandleGunOrientation(mouseScreenPos);
    }

    private void HandlePlayerOrientation(Vector2 mouseScreenPos)
    {
        SpriteRenderer.transform.localScale = mouseScreenPos.x < Screen.width / 2f ? new Vector3(-1f, 1f, 1f) : Vector3.one;
    }

    private void HandleGunOrientation(Vector2 mouseScreenPos)
    {
        Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0;

        Vector2 direction = mouseWorldPos - _gun.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _gun.rotation = Quaternion.Euler(0, 0, angle);

        _gunSprite.localScale = mouseScreenPos.x < Screen.width / 2f ? new Vector3(1f, -1f, 1f) : Vector3.one;
    }

    public override void Setup(ACharacter character)
    {
        _inputs = GetComponents<BasePlayerInput>();
        _animatorController = GetComponent<PlayerAnimatorController>();

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

		TriggerOnSetup();
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
    }

    private void UnregisterEvents()
	{
		Player.OnJumpStart -= JumpStart;
		Player.OnJumpEnd -= JumpEnd;
	}

	private void JumpStart()
	{
        _shadowSpriteRenderer.gameObject.SetActive(true);
    }

    private void JumpEnd()
	{
		_shadowSpriteRenderer.gameObject.SetActive(false);

		if (!ZoneManager.Instance.IsInsideAnyZone(transform.position))
		{
			Player.Health.TakeDamage(1f);
		}
    }
}
