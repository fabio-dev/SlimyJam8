using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Syst�me unifi� qui combine automatiquement les inputs Look (souris) et LookGamepad (manette)
/// en un seul �v�nement uniforme pour tous les autres scripts
/// </summary>
public class UnifiedLookInput : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float joystickDeadZone = 0.2f;
    [SerializeField] private float mouseSensitivityThreshold = 10f;

    private static Camera _mainCamera;
    public static event System.Action<Vector2, bool> OnLookInput;
    public static event System.Action<Vector2> OnLookPerformed;

    private bool isUsingMouse = true;
    private Vector2 lastMousePosition;
    private Vector2 currentUnifiedInput;

    public static bool IsUsingMouse { get; private set; } = true;
    public static Vector2 CurrentLookInput { get; private set; }

    private void Start()
    {
        InputManager.Instance.Player.Value.Look.performed += OnMouseLook;
        InputManager.Instance.Player.Value.LookGamepad.performed += OnJoystickLook;
        InputManager.Instance.Player.Value.LookGamepad.canceled += OnJoystickCanceled;

        if (Mouse.current != null)
        {
            lastMousePosition = Mouse.current.position.ReadValue();
        }

        _mainCamera = Camera.main;
    }

    private void OnMouseLook(InputAction.CallbackContext context)
    {
        Vector2 mouseInput = context.ReadValue<Vector2>();
        ProcessInput(mouseInput, true);
    }

    private void OnJoystickLook(InputAction.CallbackContext context)
    {
        Vector2 joystickInput = context.ReadValue<Vector2>();

        if (joystickInput.magnitude > joystickDeadZone)
        {
            ProcessInput(joystickInput, false);
        }
    }

    private void OnJoystickCanceled(InputAction.CallbackContext context)
    {
    }

    private void ProcessInput(Vector2 input, bool fromMouse)
    {
        isUsingMouse = fromMouse;
        currentUnifiedInput = input;

        IsUsingMouse = isUsingMouse;
        CurrentLookInput = currentUnifiedInput;

        OnLookInput?.Invoke(currentUnifiedInput, isUsingMouse);
        OnLookPerformed?.Invoke(currentUnifiedInput);

        Debug.Log($"Unified Look Input: {input} (Source: {(fromMouse ? "Mouse" : "Joystick")})");
    }

    private void Update()
    {
        if (!isUsingMouse && Mouse.current != null)
        {
            Vector2 currentMousePosition = Mouse.current.position.ReadValue();
            Vector2 mouseDelta = currentMousePosition - lastMousePosition;

            if (mouseDelta.magnitude > mouseSensitivityThreshold)
            {
                ProcessInput(currentMousePosition, true);
                lastMousePosition = currentMousePosition;
            }
        }
        else if (isUsingMouse && Mouse.current != null)
        {
            lastMousePosition = Mouse.current.position.ReadValue();
        }
    }

    private void OnDestroy()
    {
        if (InputManager.Instance?.Player != null)
        {
            InputManager.Instance.Player.Value.Look.performed -= OnMouseLook;
            InputManager.Instance.Player.Value.LookGamepad.performed -= OnJoystickLook;
            InputManager.Instance.Player.Value.LookGamepad.canceled -= OnJoystickCanceled;
        }
    }

    public static Vector2 GetWorldDirection(Vector3 fromPosition)
    {
        return GetWorldDirection(fromPosition, _mainCamera);
    }

    public static Vector2 GetWorldDirection(Vector3 fromPosition, Camera camera)
    {
        if (IsUsingMouse)
        {
            Vector3 worldPosition = camera.ScreenToWorldPoint(CurrentLookInput);
            worldPosition.z = 0;
            return (worldPosition - fromPosition).normalized;
        }
        else
        {
            return CurrentLookInput.normalized;
        }
    }
}