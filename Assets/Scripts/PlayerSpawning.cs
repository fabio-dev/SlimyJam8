using DG.Tweening;
using System;
using UnityEngine;

public class PlayerSpawning : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _shadowSprite;
    [SerializeField] private SpriteRenderer _feshSprite;
    [SerializeField] private Sprite[] _spawnedSprites;

    private int _animationSpriteIndex = -1;

    public event Action OnGroundTouched;
    public event Action OnReady;

    public void RunAnimation()
    {
        SFXPlayer.Instance.PlayPlayerFalling();
        _shadowSprite.transform.DOScale(1f, 1f).SetEase(Ease.OutCubic);

        Sequence animation = DOTween.Sequence();
        animation.Append(_feshSprite.transform.DOLocalMoveY(_feshSprite.transform.localPosition.y - 10f, 1f).SetEase(Ease.Linear));
        animation.AppendCallback(() =>
        {
            OnGroundTouched?.Invoke();
            _shadowSprite.enabled = false;
        });

        foreach (Sprite sprite in _spawnedSprites)
        {
            animation.AppendCallback(SetNextSprite);
            animation.AppendInterval(.1f);
        }

        animation.SetAutoKill(false);
        animation.OnComplete(() => OnReady?.Invoke());
        animation.Play();
    }

    private void SetNextSprite()
    {
        _animationSpriteIndex++;

        if (_animationSpriteIndex >= _spawnedSprites.Length) 
        {
            _animationSpriteIndex--;
        }

        _feshSprite.sprite = _spawnedSprites[_animationSpriteIndex];
    }
}
