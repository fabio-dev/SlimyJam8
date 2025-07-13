using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _splashRadius = 2f;

    private InputSystem_Actions _inputActions;
    private Vector2 _moveInput;

    void Awake()
    {
        _inputActions = new InputSystem_Actions();

        _inputActions.Player.Move.performed += MovePerformed;
        _inputActions.Player.Move.canceled += MoveCanceled;
        _inputActions.Player.Splash.performed += SplashPerformed;
    }

    private void SplashPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("Creating zone");
        Vector2 position = transform.position;

        var zone = new CircleZone(position, _splashRadius);
        ZoneManager.Instance.AddZone(zone);
    }

    private void OnDestroy()
    {
        _inputActions.Player.Move.performed -= MovePerformed;
        _inputActions.Player.Move.canceled -= MoveCanceled;
        _inputActions.Player.Splash.performed -= SplashPerformed;
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
        bool isMoving = _moveInput != Vector2.zero;

        if (isMoving)
        {
            HandleMovement(_moveInput);
        }
    }

    private void HandleMovement(Vector2 moveInput)
    {
        Vector3 direction = new Vector3(moveInput.x, moveInput.y, 0f).normalized;
        Vector3 targetPosition = transform.position + direction * _moveSpeed * Time.fixedDeltaTime;

        if (CanMoveTo(targetPosition))
        {
            transform.position = targetPosition;
            SetOrientation(moveInput.x);
        }
    }

    private static bool CanMoveTo(Vector3 targetPosition)
    {
        return ZoneManager.Instance != null && ZoneManager.Instance.IsInsideAnyZone(targetPosition);
    }

    private void SetOrientation(float xPosition)
    {
        if (xPosition > 0)
        {
            transform.rotation = Quaternion.identity;
        }
        else if (xPosition < 0)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
    }
}
