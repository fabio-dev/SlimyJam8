using Assets.Scripts.Domain;
using System.Collections;
using UnityEngine;

public class PlayerGO : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _shadowSpriteRenderer;
    private BasePlayerInput[] _inputs;
    public Player Player { get; private set; }

    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    private void Start()
    {
        _inputs = GetComponents<BasePlayerInput>();
    }

    public void SetPlayer(Player player)
    {
        if (Player != null)
        {
            UnregisterEvents();
        }

        Player = player;
        RegisterEvents();
    }

    public void Pause()
    {
        foreach (BasePlayerInput input in _inputs)
        {
            input.enabled = false;
        }
    }

    public void Resume()
    {
        foreach (BasePlayerInput input in _inputs)
        {
            input.enabled = true;
        }
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
