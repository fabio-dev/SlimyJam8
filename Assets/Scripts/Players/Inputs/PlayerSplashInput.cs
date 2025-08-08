using Assets.Scripts.Domain;
using DG.Tweening;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSplashInput : BasePlayerInput
{
	[SerializeField] private PlayerJumpInput _jumpInput;

	void Start()
	{
		Init();
		InputManager.Instance.Player.Value.Splash.performed += SplashPerformed;
	}

	private void SplashPerformed(InputAction.CallbackContext obj)
	{
		if (Player.SplashAbility?.CanCast == false)
		{
			return;
		}

		if (!Player.CanSplash)
		{
			return;
		}

		_ = MakeSplashAsync();
	}

	private async Task MakeSplashAsync()
	{
        Player?.Splash();

        if (Player.State == PlayerState.Jumping)
        {
            float currentY = _playerGO.Body.position.y;
            _jumpInput.Stop();
            _playerGO.Body.position = new Vector3(_playerGO.Body.position.x, currentY, 0f);
            _playerGO.Body.DOLocalMoveY(0f, .15f).SetEase(Ease.Linear);
        }
        else
        {
            SFXPlayer.Instance.PlayPlayerJump();
        }

        await Task.Delay(400);

        CircleZone zone = new(transform.position, Player.SplashRadius);
		ZoneManager.Instance.AddZone(zone);

		SFXPlayer.Instance.PlayPlayerSplash();
		Player?.EndSplash();
	}

	private void OnDestroy()
	{
		if (InputManager.Instance?.Player != null)
		{
			InputManager.Instance.Player.Value.Splash.performed -= SplashPerformed;
		}
	}

	void OnEnable()
	{
		if (InputManager.Instance?.Player != null)
		{
			InputManager.Instance.Player.Value.Splash.Enable();
		}
	}
	void OnDisable()
	{
		if (InputManager.Instance?.Player != null)
		{
			InputManager.Instance.Player.Value.Splash.Disable();
		}
	}
}
