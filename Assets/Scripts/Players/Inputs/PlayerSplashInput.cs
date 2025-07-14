using Assets.Scripts.Domain;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSplashInput : BasePlayerInput
{
    private InputSystem_Actions _inputActions;

    void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.Splash.performed += SplashPerformed;
    }

    private void SplashPerformed(InputAction.CallbackContext obj)
    {
        Vector2 position = transform.position;

        var zone = new CircleZone(position, Player.SplashRadius);
        ZoneManager.Instance.AddZone(zone);
    }

    private void OnDestroy()
    {
        _inputActions.Player.Splash.performed -= SplashPerformed;
    }

    void OnEnable() => _inputActions.Enable();
    void OnDisable() => _inputActions.Disable();
}
