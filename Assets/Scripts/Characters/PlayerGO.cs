using Assets.Scripts.Domain;
using UnityEngine;

public class PlayerGO : ACharacterGO
{
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private SpriteRenderer _shadowSpriteRenderer;

	private PlayerAnimatorController _animatorController;
	private BasePlayerInput[] _inputs;

	public Player Player { get { return Character as Player; } }
	public SpriteRenderer SpriteRenderer => _spriteRenderer;

	public PlayerState State => Player.State;

	private void Start()
	{
		_inputs = GetComponents<BasePlayerInput>();
		_animatorController = GetComponent<PlayerAnimatorController>();
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
	}

	private void JumpEnd()
	{
		_shadowSpriteRenderer.gameObject.SetActive(false);
	}
}
