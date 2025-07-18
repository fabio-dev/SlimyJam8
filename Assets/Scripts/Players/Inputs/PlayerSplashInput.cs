using Assets.Scripts.Domain;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSplashInput : BasePlayerInput
{
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

		if (Player.IsDashing || Player.IsJumping)
		{
			return;
		}

		Player?.Splash();

		Vector2 position = transform.position;

		var zone = new CircleZone(position, Player.SplashRadius);
		ZoneManager.Instance.AddZone(zone);
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
