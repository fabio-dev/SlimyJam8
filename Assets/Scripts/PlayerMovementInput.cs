using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementInput : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;

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
        _moveInput = Vector2.zero;
    }

    private void MovePerformed(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        bool isMoving = _moveInput == Vector2.zero;

        if (isMoving)
        {
            Vector3 newPos = transform.position + (Vector3)(_moveInput.normalized * _moveSpeed * Time.deltaTime);
            transform.position = newPos;
        }
    }
}
