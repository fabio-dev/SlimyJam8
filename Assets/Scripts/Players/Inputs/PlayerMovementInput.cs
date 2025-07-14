using Assets.Scripts.Domain;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementInput : BasePlayerInput
{
    private Vector2 _moveInput;

    void Start()
    {
        Init();
        InputManager.Instance.Player.Move.performed += MovePerformed;
        InputManager.Instance.Player.Move.canceled += MoveCanceled;
    }

    private void OnDestroy()
    {
        InputManager.Instance.Player.Move.performed -= MovePerformed;
        InputManager.Instance.Player.Move.canceled -= MoveCanceled;
    }

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

    private bool CanMoveTo(Vector3 targetPosition)
    {
        return Player.IsJumping
            || (ZoneManager.Instance != null
            && ZoneManager.Instance.IsInsideAnyZone(targetPosition));
    }

    void OnEnable() => InputManager.Instance.Player.Move.Enable();
    void OnDisable()
    {
        if (InputManager.Instance?.Player != null)
        {
            InputManager.Instance.Player.Move.Disable();
        }
    }
}
