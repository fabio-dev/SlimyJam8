using Assets.Scripts.Domain;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private ASpriteAnimator _idleAnimator;
    [SerializeField] private ASpriteAnimator _moveAnimator;
    [SerializeField] private ASpriteAnimator _jumpAnimator;
    [SerializeField] private ASpriteAnimator _splashAnimator;
    [SerializeField] private ASpriteAnimator _damagedAnimator;

    private PlayerGO _playerGO;
    private ASpriteAnimator _currentAnimator;

    public void Setup(PlayerGO playerGO)
    {
        UnregisterEvents();
        _playerGO = playerGO;
        _idleAnimator.SetSpriteRenderer(_playerGO.SpriteRenderer);
        _moveAnimator.SetSpriteRenderer(_playerGO.SpriteRenderer);
        _jumpAnimator.SetSpriteRenderer(_playerGO.SpriteRenderer);
        _splashAnimator.SetSpriteRenderer(_playerGO.SpriteRenderer);
        _damagedAnimator.SetSpriteRenderer(_playerGO.SpriteRenderer);
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
        _damagedAnimator.OnComplete -= DamagedAnimatorComplete;
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
