using Assets.Scripts.Domain;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashInput : BasePlayerInput
{
	void Start()
	{
		Init();
		InputManager.Instance.Player.Value.Dash.performed += DashPerformed;
	}

	private void DashPerformed(InputAction.CallbackContext context)
	{
		_ = TryDash();
	}

	private async Task TryDash()
	{
		if (!Player.CanMakeAction)
		{
			return;
		}

		if (Player.DashAbility?.CanCast == false)
		{
			return;
		}

		Vector3 direction = Player.LastMove;
		if (direction == Vector3.zero)
		{
			direction = Vector3.right;
		}

		await DashAsync(direction);
	}

	private async Task DashAsync(Vector3 direction)
	{
		Player.Dash();

		float dashTimer = 0f;
		float zoneTimer = 0f;

		SetOrientation(direction.x);

		while (dashTimer < Player.DashDuration)
		{
			float elapsedDeltaTime = Time.deltaTime;
			dashTimer += elapsedDeltaTime;
			zoneTimer += elapsedDeltaTime;

			transform.position += direction * Player.DashMoveSpeed * elapsedDeltaTime;

			if (zoneTimer >= Player.DashZoneInterval)
			{
				CreateZone();
				zoneTimer = 0f;
			}

			await Task.Yield();
		}

		CreateZone();
		Player.StopDash();
	}

	private void CreateZone()
	{
		var zone = new CircleZone(transform.position, Player.DashRadius);
		ZoneManager.Instance.AddZone(zone);
	}

	private void OnDestroy()
	{
		if (InputManager.Instance?.Player != null)
		{
			InputManager.Instance.Player.Value.Dash.performed -= DashPerformed;
		}
	}

	void OnEnable()
	{
		if (InputManager.Instance?.Player != null)
		{
			InputManager.Instance.Player.Value.Dash.Enable();
        }
    }

	void OnDisable()
	{
		if (InputManager.Instance?.Player != null)
		{
			InputManager.Instance.Player.Value.Dash.Disable();
		}
	}
}
