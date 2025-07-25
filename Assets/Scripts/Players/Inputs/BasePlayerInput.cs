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
}
