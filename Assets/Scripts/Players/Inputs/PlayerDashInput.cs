using Assets.Scripts.Domain;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashInput : BasePlayerInput
{
	private InputSystem_Actions _inputActions;
	private Vector2 _pointerScreenPosition;

	void Awake()
	{
		_inputActions = new InputSystem_Actions();
		_inputActions.Player.Dash.performed += DashPerformed;
		_inputActions.Player.Look.performed += LookPerformed;
	}

	private void DashPerformed(InputAction.CallbackContext obj)
	{
		_ = TryDash();
	}

	private void LookPerformed(InputAction.CallbackContext context)
	{
		_pointerScreenPosition = context.ReadValue<Vector2>();
	}

	private async Task TryDash()
	{
		if (Player.IsDashing)
		{
			return;
		}

		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(_pointerScreenPosition);
		mouseWorldPos.z = 0f;

		Vector3 direction = (mouseWorldPos - transform.position).normalized;

		if (direction == Vector3.zero)
		{
			return;
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
		_inputActions.Player.Dash.performed -= DashPerformed;
		_inputActions.Player.Look.performed -= LookPerformed;
	}

	void OnEnable() => _inputActions.Enable();
	void OnDisable() => _inputActions.Disable();
}
