using Assets.Scripts.Domain;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private SpriteAnimations _idleAnimations;
    [SerializeField] private SpriteAnimations _moveAnimations;
    [SerializeField] private SpriteAnimations _jumpAnimations;
    [SerializeField] private SpriteAnimations _splashAnimations;
    [SerializeField] private SpriteAnimations _damagedAnimations;
    [SerializeField] private SpriteAnimations _dyingAnimations;

    private LoopSpriteAnimator _idleAnimator = new();
    private LoopSpriteAnimator _moveAnimator = new();
    private OneTimeSpriteAnimator _jumpAnimator = new();
    private OneTimeSpriteAnimator _splashAnimator = new();
    private OneTimeSpriteAnimator _damagedAnimator = new();
    private OneTimeSpriteAnimator _dyingAnimator = new();

    private PlayerGO _playerGO;
    private ASpriteAnimator _currentAnimator;

    public void Setup(PlayerGO playerGO)
    {
        UnregisterEvents();
        _playerGO = playerGO;
        _idleAnimator.SetSpriteRenderer(_playerGO.SpriteRenderer);
        _idleAnimator.SetSpritesAnimations(_idleAnimations);
        _moveAnimator.SetSpriteRenderer(_playerGO.SpriteRenderer);
        _moveAnimator.SetSpritesAnimations(_moveAnimations);
        _jumpAnimator.SetSpriteRenderer(_playerGO.SpriteRenderer);
        _jumpAnimator.SetSpritesAnimations(_jumpAnimations);
        _splashAnimator.SetSpriteRenderer(_playerGO.SpriteRenderer);
        _splashAnimator.SetSpritesAnimations(_splashAnimations);
        _damagedAnimator.SetSpriteRenderer(_playerGO.SpriteRenderer);
        _damagedAnimator.SetSpritesAnimations(_damagedAnimations);
        _dyingAnimator.SetSpriteRenderer(_playerGO.SpriteRenderer);
        _dyingAnimator.SetSpritesAnimations(_dyingAnimations);
        RegisterEvents();

        PlayerStateChanged(_playerGO.State);
    }

    private void RegisterEvents()
    {
        if (_playerGO == null)
        {
            return;
        }

        _playerGO.Player.OnStateChanged += PlayerStateChanged;
        _playerGO.Player.Health.OnDamaged += PlayerDamaged;
        _playerGO.Player.OnDie += PlayerDying;
        _damagedAnimator.OnComplete += DamagedAnimatorComplete;
    }

    private void UnregisterEvents()
    {
        if (_playerGO == null)
        {
            return;
        }

        _playerGO.Player.OnStateChanged -= PlayerStateChanged;
        _playerGO.Player.Health.OnDamaged -= PlayerDamaged;
        _playerGO.Player.OnDie -= PlayerDying;
        _damagedAnimator.OnComplete -= DamagedAnimatorComplete;
    }

    private void PlayerDying()
    {
        if (_currentAnimator != null)
        {
            _currentAnimator.Stop();
        }

        _currentAnimator = _dyingAnimator;
        _currentAnimator.Play();
    }

    private void DamagedAnimatorComplete()
    {
        PlayerStateChanged(_playerGO.Player.State);
    }

    private void PlayerDamaged(float obj)
    {
        if (_currentAnimator != null)
        {
            _currentAnimator.Stop();
        }

        _currentAnimator = _damagedAnimator;
        _currentAnimator.Play();
    }

    private void PlayerStateChanged(PlayerState state)
    {
        if (_dyingAnimator.IsPlaying())
        {
            return;
        }

        if (_damagedAnimator.IsPlaying())
        {
            return;
        }

        if (_currentAnimator != null)
        {
            _currentAnimator.Stop();
        }

        if (state == PlayerState.Idle)
        {
            _currentAnimator = _idleAnimator;
        }
        if (state == PlayerState.Moving)
        {
            _currentAnimator = _moveAnimator;
        }
        if (state == PlayerState.Jumping)
        {
            _currentAnimator = _jumpAnimator;
        }
        if (state == PlayerState.Splashing)
        {
            _currentAnimator = _splashAnimator;
        }

        _currentAnimator.Play();
    }
}
