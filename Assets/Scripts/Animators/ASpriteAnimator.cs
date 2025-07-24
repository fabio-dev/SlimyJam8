using DG.Tweening;
using System;
using UnityEngine;

public class ASpriteAnimator
{
    private SpriteAnimations _sprites;
    protected SpriteRenderer _spriteRenderer;
    protected Sequence _spriteAnimation;
    protected int _currentSpriteIndex = -1;

    public bool IsPlaying() => _spriteAnimation?.IsPlaying() ?? false;

    public event Action OnComplete;

    public void SetSpritesAnimations(SpriteAnimations sprites)
    {
        _sprites = sprites;
    }

    protected virtual Sequence CreateSpriteSequence()
    {
        Sequence animation = DOTween.Sequence();

        foreach (SpriteAnimationDuration sprite in _sprites.Sprites)
        {
            animation.AppendCallback(SetNextSprite);
            animation.AppendInterval(sprite.Duration);
        }

        animation.SetAutoKill(false);
        animation.OnComplete(Complete);
        return animation;
    }

    private void Complete()
    {
        OnComplete?.Invoke();
    }

    public void SetSpriteRenderer(SpriteRenderer spriteRenderer)
    {
        _spriteRenderer = spriteRenderer;
    }

    public void Replay()
    {
        if (_spriteAnimation == null)
        {
            _spriteAnimation = CreateSpriteSequence();
        }
        _spriteAnimation.Restart();
    }

    public void Play()
    {
        if (_spriteAnimation == null)
        {
            _spriteAnimation = CreateSpriteSequence();
        }
        _spriteAnimation.Play();
    }

    public void Kill()
    {
        _spriteAnimation.Kill();
    }

    public void Stop()
    {
        _currentSpriteIndex = -1;
        _spriteAnimation.Rewind();
    }

    protected void SetNextSprite()
    {
        _currentSpriteIndex++;

        if (_currentSpriteIndex >= _sprites.Sprites.Length)
        {
            _currentSpriteIndex = 0;
        }

        _spriteRenderer.sprite = _sprites.Sprites[_currentSpriteIndex].Sprite;
    }
}
