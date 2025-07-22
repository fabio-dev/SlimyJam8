using Assets.Scripts.Domain;
using UnityEngine;

public class PlayerGO : ACharacterGO
{
	[SerializeField] private SpriteRenderer _shadowSpriteRenderer;

	private PlayerAnimatorController _animatorController;
	private CapsuleCollider2D _collider;
	private BasePlayerInput[] _inputs;

	public Player Player => Character as Player;

    public PlayerState State => Player.State;

	private void Start()
	{
		_inputs = GetComponents<BasePlayerInput>();
		_animatorController = GetComponent<PlayerAnimatorController>();
		_collider = GetComponent<CapsuleCollider2D>();
	}

	public override void Setup(ACharacter character)
	{
		if (Character != null)
		{
			UnregisterEvents();
		}

		base.Setup(character);

		RegisterEvents();
		_animatorController.Setup(this);
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
