using Assets.Scripts.Domain;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementInput : BasePlayerInput
{
    private Vector2 _moveInput;
    private Rigidbody2D _rigidbody;

    void Start()
    {
        Init();
        InputManager.Instance.Player.Value.Move.performed += MovePerformed;
        InputManager.Instance.Player.Value.Move.canceled += MoveCanceled;
        _rigidbody = GetComponent<Rigidbody2D>();
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
        if (Player.State == PlayerState.Dashing
            || Player.State == PlayerState.Splashing)
        {
            return;
        }
        Vector3 direction = new Vector3(moveInput.x, moveInput.y, 0f).normalized;
        Vector2 movement = direction * Player.MoveSpeed * Time.fixedDeltaTime;
        Vector2 targetPosition = _rigidbody.position + movement;

        if (CanMoveTo(targetPosition))
        {
            _rigidbody.MovePosition(targetPosition);
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
        return Player.State == PlayerState.Jumping
            || (ZoneManager.Instance != null
            && ZoneManager.Instance.IsInsideAnyZone(targetPosition));
    }

    private void OnDestroy()
    {
        if (InputManager.Instance?.Player != null)
        {
            InputManager.Instance.Player.Value.Move.performed -= MovePerformed;
            InputManager.Instance.Player.Value.Move.canceled -= MoveCanceled;
        }
    }

    void OnEnable()
    {
        if (InputManager.Instance?.Player != null)
        {
            InputManager.Instance.Player.Value.Move.Enable();
        }
    }

    void OnDisable()
    {
        if (InputManager.Instance?.Player != null)
        {
            InputManager.Instance.Player.Value.Move.Disable();
        }
    }
}
