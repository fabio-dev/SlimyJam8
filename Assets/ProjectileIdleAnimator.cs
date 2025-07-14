using DG.Tweening;
using UnityEngine;

public class ProjectileIdleAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private float _delayBetweenSprites = .1f;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Sequence _spriteAnimation;

    private int _currentSpriteIndex = -1;

    void Start()
    {
        _spriteAnimation = DOTween.Sequence();

        foreach (Sprite sprite in _sprites)
        {
            _spriteAnimation.AppendInterval(_delayBetweenSprites);
            _spriteAnimation.AppendCallback(SetNextSprite);
        }

        _spriteAnimation.SetLoops(-1);
        SetNextSprite();
    }

    public void Kill()
    {
        _spriteAnimation.Kill();
    }

    private void SetNextSprite()
    {
        _currentSpriteIndex++;

        if (_currentSpriteIndex >= _sprites.Length)
        {
            _currentSpriteIndex = 0;
        }

        _spriteRenderer.sprite = _sprites[_currentSpriteIndex];
    }
}
