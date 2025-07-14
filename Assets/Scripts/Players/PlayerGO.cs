using Assets.Scripts.Domain;
using UnityEngine;

public class PlayerGO : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _shadowSpriteRenderer;

    public Player Player { get; private set; }

    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    public void SetPlayer(Player player)
    {
        if (Player != null)
        {
            UnregisterEvents();
        }

        Player = player;
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        Player.OnJumpStart += JumpStart;
        Player.OnJumpEnd += JumpEnd;
    }

    private void UnregisterEvents()
    {
        Player.OnJumpStart -= JumpStart;
        Player.OnJumpEnd -= JumpEnd;
    }

    private void JumpStart()
    {
        _shadowSpriteRenderer.gameObject.SetActive(true);
    }

    private void JumpEnd()
    {
        _shadowSpriteRenderer.gameObject.SetActive(false);
    }
}
