using DG.Tweening;
using UnityEngine;
 
public abstract class ASpriteAnimator : ScriptableObject
{
    [SerializeField] protected SpriteAnimationDuration[] _sprites;
    protected SpriteRenderer _spriteRenderer;
    protected Sequence _spriteAnimation;
    protected int _currentSpriteIndex = -1;

    protected virtual Sequence CreateSpriteSequence()
    {
        Sequence animation = DOTween.Sequence();

        foreach (SpriteAnimationDuration sprite in _sprites)
        {
            animation.AppendInterval(sprite.Duration);
            animation.AppendCallback(SetNextSprite);
        }

        animation.SetAutoKill(false);
        return animation;
    }

    public void SetSpriteRenderer(SpriteRenderer spriteRenderer)
    {
        _spriteRenderer = spriteRenderer;  
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

        if (_currentSpriteIndex >= _sprites.Length)
        {
            _currentSpriteIndex = 0;
        }

        _spriteRenderer.sprite = _sprites[_currentSpriteIndex].Sprite;
    }
}
