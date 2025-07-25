using Assets.Scripts.Domain;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackInput : BasePlayerInput
{
	[SerializeField] private ProjectileGO _projectilePrefab;
	private Vector2 _pointerScreenPosition;
	private CancellationTokenSource _attackCancellationTokenSource;
	private Camera _mainCamera;
	private float _lastAttackTime;

	void Start()
	{
		Init();
		_mainCamera = Camera.main;
		InputManager.Instance.Player.Value.Look.performed += LookPerformed;
		InputManager.Instance.Player.Value.Attack.performed += AttackPerformed;
		InputManager.Instance.Player.Value.Attack.canceled += AttackCanceled;
	}

	private void LookPerformed(InputAction.CallbackContext context)
	{
		_pointerScreenPosition = context.ReadValue<Vector2>();
	}

	private void AttackPerformed(InputAction.CallbackContext obj)
	{
		bool isAttacking = _attackCancellationTokenSource != null;
		if (isAttacking)
		{
			return;
		}

		GameManager.Instance.PlayerGO.ShowGun();

		CancellationTokenSource cancellationTokenSource = new();
		_attackCancellationTokenSource = cancellationTokenSource;
		_ = AttackLoop(cancellationTokenSource.Token);
	}

	private void AttackCanceled(InputAction.CallbackContext obj)
	{
        GameManager.Instance.PlayerGO.HideGun();

        _attackCancellationTokenSource?.Cancel();
		_attackCancellationTokenSource = null;
	}

	private async Task AttackLoop(CancellationToken token)
	{
		float timeSinceLastAttack = Time.time - _lastAttackTime;
		if (timeSinceLastAttack < Player.AttackCooldown)
		{
			await Task.Delay(Mathf.RoundToInt((Player.AttackCooldown - timeSinceLastAttack) * 1000), token);
		}

		try
		{
			while (!token.IsCancellationRequested)
			{
				Shoot(_pointerScreenPosition);
				_lastAttackTime = Time.time;
				await Task.Delay(Mathf.RoundToInt(Player.AttackCooldown * 1000));
			}
		}
		catch (TaskCanceledException)
		{ }
		catch (System.Exception ex)
		{
			Debug.LogError($"Unexpected error in AttackLoop : {ex}");
		}
	}

	private void Shoot(Vector2 pointerScreenPosition)
	{
		Vector3 pointerWorldPosition = _mainCamera.ScreenToWorldPoint(pointerScreenPosition);
		pointerWorldPosition.z = 0;

		Vector2 shootDirection = (pointerWorldPosition - transform.position).normalized;

        ProjectileGO projectile = Instantiate(_projectilePrefab, GameManager.Instance.PlayerGO.GunShotPosition, Quaternion.identity);
		projectile.transform.right = shootDirection;
		projectile.Launch(shootDirection, GameManager.Instance.PlayerGO.Player.AttackDamages, 0f);
	}

	private void OnDestroy()
	{
		if (InputManager.Instance?.Player != null)
		{
			InputManager.Instance.Player.Value.Look.performed -= LookPerformed;
			InputManager.Instance.Player.Value.Attack.performed -= AttackPerformed;
			InputManager.Instance.Player.Value.Attack.canceled -= AttackCanceled;
		}
	}

	void OnEnable()
	{
		if (InputManager.Instance?.Player != null)
		{
			InputManager.Instance.Player.Value.Enable();
		}
	}
	void OnDisable()
	{
		if (InputManager.Instance?.Player != null)
		{
			InputManager.Instance.Player.Value.Attack.Disable();
		}
		_attackCancellationTokenSource?.Cancel();
		_attackCancellationTokenSource = null;
	}
}
