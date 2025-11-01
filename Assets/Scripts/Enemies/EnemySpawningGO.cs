using DG.Tweening;
using System;
using UnityEngine;

public class EnemySpawningGO : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private const float ANIMATION_DURATION = 2f;
    private const float FADE_DURATION = .3f;

    public event Action OnEnding;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        StartAnimation();
    }

    private void StartAnimation()
    {
        _spriteRenderer.transform.localScale = new Vector2(.2f, .2f);
        _spriteRenderer.color = new Color(1f, 1f, 1f, 0f);

        Sequence rotationAnimation = DOTween.Sequence();
        rotationAnimation.Append(_spriteRenderer.transform.DORotate(new Vector3(0, 0, -360), .5f, RotateMode.FastBeyond360).SetEase(Ease.Linear));
        rotationAnimation.SetLoops(-1);

        Sequence animation = DOTween.Sequence();
        animation.Append(_spriteRenderer.DOFade(1f, FADE_DURATION));
        animation.Join(_spriteRenderer.transform.DOScale(1f, ANIMATION_DURATION - FADE_DURATION));
        animation.Append(_spriteRenderer.DOFade(0f, FADE_DURATION).OnStart(() =>
        {
            OnEnding?.Invoke();
        }));

        animation.OnComplete(() =>
        {
            rotationAnimation.Kill();
        });
    }
}
