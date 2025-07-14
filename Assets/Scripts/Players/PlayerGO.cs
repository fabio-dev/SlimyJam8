using Assets.Scripts.Domain;
using UnityEngine;

public class PlayerGO : MonoBehaviour
{
    public Player Player { get; private set; }

    public void SetPlayer(Player player)
    {
        Player = player;
    }
}
