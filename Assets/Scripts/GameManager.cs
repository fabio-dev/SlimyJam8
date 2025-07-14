using Assets.Scripts.Domain;
using Mono.Cecil.Cil;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerGO _playerGO;

    private bool _firstUpdate = false;

    void Update()
    {
        if (!_firstUpdate)
        {
            FirstUpdate();
            _firstUpdate = true;
        }
    }

    private void FirstUpdate()
    {
        Player player = new Player();
        _playerGO.SetPlayer(player);
    }
}
