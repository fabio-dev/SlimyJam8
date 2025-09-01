using Assets.Scripts.Domain;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackInput : BasePlayerInput
{
    private CancellationTokenSource _attackCancellationTokenSource;
    private Camera _mainCamera;
    private float _lastAttackTime;

    void Start()
    {
        Init();
        _mainCamera = Camera.main;

        UnifiedLookInput.OnLookInput += OnUnifiedLookInput;

        InputManager.Instance.Player.Value.Attack.performed += AttackPerformed;
        InputManager.Instance.Player.Value.Attack.canceled += AttackCanceled;
    }

    private void OnUnifiedLookInput(Vector2 input, bool usingMouse)
    {
        Debug.Log($"Attack System received unified input: {input} (Mouse: {usingMouse})");
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
                Vector2 shootDirection = GetShootDirection();

                if (shootDirection.magnitude > 0.1f)
                {
                    Shoot(shootDirection);
                    _lastAttackTime = Time.time;
                }

                await Task.Delay(Mathf.RoundToInt(Player.AttackCooldown * 1000), token);
            }
        }
        catch (TaskCanceledException)
        {
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Unexpected error in AttackLoop: {ex}");
        }
    }

    private Vector2 GetShootDirection()
    {
        return UnifiedLookInput.GetWorldDirection(transform.position, _mainCamera);
    }

    private void Shoot(Vector2 shootDirection)
    {
        GameManager.Instance.PlayerGO.Shoot(shootDirection);
    }

    private void Update()
    {
        DebugVisualShotDirection();
    }

    private void DebugVisualShotDirection()
    {
        if (Application.isEditor)
        {
            Vector2 direction = GetShootDirection();
            if (direction.magnitude > 0.1f)
            {
                Debug.DrawRay(transform.position, direction * 3f, UnifiedLookInput.IsUsingMouse ? Color.green : Color.blue);
            }
        }
    }

    private void OnDestroy()
    {
        UnifiedLookInput.OnLookInput -= OnUnifiedLookInput;

        if (InputManager.Instance?.Player != null)
        {
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
            InputManager.Instance.Player.Value.Disable();
        }

        _attackCancellationTokenSource?.Cancel();
        _attackCancellationTokenSource = null;
    }
}