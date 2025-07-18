using DG.Tweening;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private float _delayBetweenSprites = .1f;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Sequence _spriteAnimation;

    private int _currentSpriteIndex = -1;

    public void SetSprites(Sprite[] sprites)
    {
        _sprites = sprites;
        _currentSpriteIndex = -1;
    }

    void Start()
    {
        _spriteAnimation = DOTween.Sequence();

        foreach (Sprite sprite in _sprites)
        {
            _spriteAnimation.AppendInterval(_delayBetweenSprites);
            _spriteAnimation.AppendCallback(() => SetNextSprite(_sprites));
        }

        _spriteAnimation.SetLoops(-1);

        SetNextSprite(_sprites);
    }

    public void Kill()
    {
        _spriteAnimation.Kill();
    }

    private void SetNextSprite(Sprite[] sprites)
    {
        _currentSpriteIndex++;

        if (_currentSpriteIndex >= sprites.Length)
        {
            _currentSpriteIndex = 0;
        }

        _spriteRenderer.sprite = sprites[_currentSpriteIndex];
    }
}
