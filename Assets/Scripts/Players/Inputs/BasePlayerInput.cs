using Assets.Scripts.Domain;
using UnityEngine;

public class BasePlayerInput : MonoBehaviour
{
    protected PlayerGO _playerGO;

    protected void Init()
    {
        _playerGO = GetComponent<PlayerGO>();
    }

    protected Player Player => _playerGO.Player;

    protected void SetOrientation(float xPosition)
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
