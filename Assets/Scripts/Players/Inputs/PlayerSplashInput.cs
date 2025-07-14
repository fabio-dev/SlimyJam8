using Assets.Scripts.Domain;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSplashInput : BasePlayerInput
{
    void Start()
    {
        Init();
        InputManager.Instance.Player.Splash.performed += SplashPerformed;
    }

    private void SplashPerformed(InputAction.CallbackContext obj)
    {
        Vector2 position = transform.position;

        var zone = new CircleZone(position, Player.SplashRadius);
        ZoneManager.Instance.AddZone(zone);
    }

    private void OnDestroy()
    {
        InputManager.Instance.Player.Splash.performed -= SplashPerformed;
    }

    void OnEnable() => InputManager.Instance.Player.Splash.Enable();
    void OnDisable()
    {
        if (InputManager.Instance?.Player != null)
        {
            InputManager.Instance.Player.Splash.Disable();
        }
    }
}
