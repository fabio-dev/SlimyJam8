using DG.Tweening;
using UnityEngine.InputSystem;

public class PlayerJumpInput : BasePlayerInput
{
	private Tween _jumpAnimation;

	public void Stop()
    {
        Player.EndJump();
		_jumpAnimation.Rewind();
    }

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
				.Body
				.transform
				.DOLocalMoveY(1f, Player.JumpDuration / 2)
				.SetEase(Ease.OutQuad)
				.SetLoops(2, LoopType.Yoyo)
				.SetAutoKill(false)
				.OnComplete(() =>
				{
					Player.EndJump();

					if (ZoneManager.Instance.IsInsideAnyZone(transform.position))
					{
                        SFXPlayer.Instance.PlayPlayerSplash();
                    }
                    else
					{
                        SFXPlayer.Instance.PlayPlayerFallground();
                    }
                });
			_jumpAnimation.Pause();
		}

		return _jumpAnimation;
	}

	private void JumpPerformed(InputAction.CallbackContext context)
	{
		if (!Player.CanMakeAction)
		{
			return;
		}

		if (Player.JumpAbility?.CanCast == false)
		{
			return;
		}

		Player.Jump();
		SFXPlayer.Instance.PlayPlayerJump();
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
