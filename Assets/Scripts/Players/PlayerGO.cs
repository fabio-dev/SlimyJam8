using Assets.Scripts.Domain;
using System.Collections;
using UnityEngine;

public class PlayerGO : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _shadowSpriteRenderer;
    [SerializeField] private Sprite[] _idleSprites;
    [SerializeField] private Sprite[] _movingSprites;
    [SerializeField] private Sprite[] _jumpSprites;
    [SerializeField] private Sprite[] _damagedSprites;
    [SerializeField] private Sprite[] _splashSprites;

    private SpriteAnimator _animator;
    private BasePlayerInput[] _inputs;
    public Player Player { get; private set; }

    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    private void Start()
    {
        _inputs = GetComponents<BasePlayerInput>();
        _animator = GetComponent<SpriteAnimator>();
        _animator.SetSprites(_idleSprites);
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
        Player.OnStateChanged += StateChanged;
    }

    private void StateChanged(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                _animator.SetSprites(_idleSprites);
                break;
            case PlayerState.Moving:
                _animator.SetSprites(_movingSprites);
                break;
            case PlayerState.Jumping:
                _animator.SetSprites(_jumpSprites);
                break;
            case PlayerState.Splashing:
                _animator.SetSprites(_splashSprites);
                break;
        }
    }

    private void UnregisterEvents()
    {
        Player.OnJumpStart -= JumpStart;
        Player.OnJumpEnd -= JumpEnd;
    }

    private void JumpStart()
    {
        _animator.SetSprites(_jumpSprites);
        _shadowSpriteRenderer.gameObject.SetActive(true);
    }

    private void JumpEnd()
    {
        _animator.SetSprites(_idleSprites);
        _shadowSpriteRenderer.gameObject.SetActive(false);
    }
}
