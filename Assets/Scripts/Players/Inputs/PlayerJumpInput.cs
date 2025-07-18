using DG.Tweening;
using UnityEngine.InputSystem;

public class PlayerJumpInput : BasePlayerInput
{
	private Tween _jumpAnimation;
	void Start()
	{
		Init();
		InputManager.Instance.Player.Value.Jump.performed += JumpPerformed;
	}

	private Tween GetAnimation()
	{
		if (_jumpAnimation == null)
		{
			_jumpAnimation = _playerGO
				.SpriteRenderer
				.transform
				.DOLocalMoveY(1f, Player.JumpDuration / 2)
				.SetEase(Ease.OutQuad)
				.SetLoops(2, LoopType.Yoyo)
				.SetAutoKill(false)
				.OnComplete(() => Player.EndJump());
			_jumpAnimation.Pause();
		}

		return _jumpAnimation;
	}

	private void JumpPerformed(InputAction.CallbackContext context)
	{
		if (Player.IsJumping || Player.IsDashing)
		{
			return;
		}

		if (Player.JumpAbility?.CanCast == false)
		{
			return;
		}

		Player.Jump();
		GetAnimation()?.Restart();
	}

	private void OnDestroy()
    {
		if (InputManager.Instance?.Player != null)
		{
			InputManager.Instance.Player.Value.Jump.performed -= JumpPerformed;
		}
	}

	void OnEnable()
	{
		if (InputManager.Instance?.Player == null)
		{
			InputManager.Instance.Player.Value.Jump.Enable();
		}
	}
	void OnDisable()
	{
		if (InputManager.Instance?.Player != null)
		{
			InputManager.Instance.Player.Value.Jump.Disable();
		}
	}
}
