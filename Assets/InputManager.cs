using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<InputManager>();
            }
            return _instance;
        }
    }

    private InputSystem_Actions _playerControls;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        _playerControls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    public InputSystem_Actions.PlayerActions Player => _playerControls.Player;
    public InputSystem_Actions.UIActions UI => _playerControls.UI;

}