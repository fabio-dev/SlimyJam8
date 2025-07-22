using Assets.Scripts.Domain;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashInput : BasePlayerInput
{
	private Rigidbody2D _rigidBody;

	void Start()
	{
		Init();
		InputManager.Instance.Player.Value.Dash.performed += DashPerformed;
        _rigidBody = GetComponent<Rigidbody2D>();

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

        StartCoroutine(DashCoroutine(direction));
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

            Vector2 move = direction.normalized * Player.DashMoveSpeed * elapsedDeltaTime;
            _rigidBody.MovePosition(_rigidBody.position + move);

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

    private IEnumerator DashCoroutine(Vector2 direction)
    {
        Player.Dash();

        float dashTimer = 0f;
        float zoneTimer = 0f;
        float duration = Player.DashDuration;
        float interval = Player.DashZoneInterval;
        float speed = Player.DashMoveSpeed;

        SetOrientation(direction.x);

        while (dashTimer < duration)
        {
            float deltaTime = Time.fixedDeltaTime;
            dashTimer += deltaTime;
            zoneTimer += deltaTime;

            Vector2 move = direction.normalized * speed * deltaTime;
            _rigidBody.MovePosition(_rigidBody.position + move);

            if (zoneTimer >= interval)
            {
                CreateZone();
                zoneTimer = 0f;
            }

            yield return new WaitForFixedUpdate();
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
