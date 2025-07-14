using Assets.Scripts.Domain;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementInput : BasePlayerInput
{
    private InputSystem_Actions _inputActions;
    private Vector2 _moveInput;

    void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.Move.performed += MovePerformed;
        _inputActions.Player.Move.canceled += MoveCanceled;
    }

    private void OnDestroy()
    {
        _inputActions.Player.Move.performed -= MovePerformed;
        _inputActions.Player.Move.canceled -= MoveCanceled;
    }

    void OnEnable() => _inputActions.Enable();
    void OnDisable() => _inputActions.Disable();

    private void MoveCanceled(InputAction.CallbackContext context)
    {
        Player.StopMove();
        _moveInput = Vector2.zero;
    }

    private void MovePerformed(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        bool isMoving = _moveInput != Vector2.zero;

        if (isMoving)
        {
            HandleMovement(_moveInput);
        }
    }

    private void HandleMovement(Vector2 moveInput)
    {
        Vector3 direction = new Vector3(moveInput.x, moveInput.y, 0f).normalized;
        Vector3 targetPosition = transform.position + direction * Player.MoveSpeed * Time.fixedDeltaTime;

        if (CanMoveTo(targetPosition))
        {
            transform.position = targetPosition;
            SetOrientation(moveInput.x);
            Player.Move(_moveInput);
        }
        else
        {
            Player.StopMove();
        }
    }

    private static bool CanMoveTo(Vector3 targetPosition)
    {
        return ZoneManager.Instance != null && ZoneManager.Instance.IsInsideAnyZone(targetPosition);
    }
}
