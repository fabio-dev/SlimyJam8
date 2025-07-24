using Assets.Scripts.Domain;
using DG.Tweening;
using UnityEngine;

public class PlayerGO : ACharacterGO
{
	[SerializeField] private SpriteRenderer _shadowSpriteRenderer;
	[SerializeField] private Transform _center;

	private Cooldown _invulnerableCooldown;
	private bool _invulnerableCooldownStarted;
	private PlayerAnimatorController _animatorController;
	private BasePlayerInput[] _inputs;

	public Player Player => Character as Player;

	public Transform Center => _center;

    public PlayerState State => Player.State;

    private void Start()
	{
		_inputs = GetComponents<BasePlayerInput>();
		_animatorController = GetComponent<PlayerAnimatorController>();
	}

    private void Update()
    {
        if (_invulnerableCooldownStarted && !_invulnerableCooldown.IsRunning())
		{
			_invulnerableCooldownStarted = false;
			_invulnerableCooldown.Stop();
			Player.Health.Vulnerable();
		}
	}

	public override void Setup(ACharacter character)
	{
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

	protected override void OnDie()
	{
		UnregisterEvents();

		base.OnDie();
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

    private void Dying()
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
		gameObject.layer = LayerMask.NameToLayer("PlayerJumping");
    }

    private void JumpEnd()
	{
		_shadowSpriteRenderer.gameObject.SetActive(false);
		gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
