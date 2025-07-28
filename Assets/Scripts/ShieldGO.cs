using Assets.Scripts.Domain;
using DG.Tweening;
using System;
using UnityEngine;

public class ShieldGO : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;

    private Sequence _breakingAnimation;
    private Sequence _appearAnimation;
    private Cooldown _shieldDuration = new Cooldown(1f);
    private bool _shieldEnabled = false;
    private bool _shieldBreaking = false;
    private Color _spriteColor;

    public event Action OnBreak;

    private void Start()
    {
        _spriteColor = _sprite.color;
    }

    private Sequence BreakingAnimation
    {
        get
        {
            if (_breakingAnimation == null)
            {
                _breakingAnimation = DOTween.Sequence();
                _breakingAnimation.Append(_sprite.DOFade(0f, .2f));
                _breakingAnimation.Append(_sprite.DOFade(1f, .2f));
                _breakingAnimation.SetLoops(-1);
                _breakingAnimation.Pause();
                _breakingAnimation.SetAutoKill(false);
            }

            return _breakingAnimation;
        }
    }

    private Sequence AppearAnimation
    {
        get
        {
            if (_appearAnimation == null)
            {
                _appearAnimation = DOTween.Sequence();
                _appearAnimation.Append(_sprite.transform.DOScale(1.2f, .3f));
                _appearAnimation.Append(_sprite.transform.DOScale(1f, .2f));
                _appearAnimation.Pause();
                _appearAnimation.SetAutoKill(false);
            }

            return _appearAnimation;
        }
    }

    private void Update()
    {
        if (_shieldEnabled)
        {
            if (!_shieldDuration.IsRunning())
            {
                Break();
            }

            if (!_shieldBreaking && _shieldDuration.Duration - _shieldDuration.ElapsedSeconds() < 2f)
            {
                Breaking();
            }
        }
    }

    private void Breaking()
    {
        _shieldBreaking = true;
        _sprite.DOFade(0f, 1f);

        BreakingAnimation.Play();
    }

    private void Break()
    {
        _shieldDuration.Stop();

        _sprite.gameObject.SetActive(false);

        _shieldEnabled = false;
        _shieldBreaking = true;

        BreakingAnimation.Rewind();

        OnBreak?.Invoke();
    }

    public void Begin(float duration)
    {
        _shieldDuration.SetDuration(duration);
        _shieldDuration.Start();

        _sprite.gameObject.SetActive(true);
        _sprite.transform.localScale = Vector2.zero;
        _sprite.color = _spriteColor;

        AppearAnimation.Restart();
        AppearAnimation.Play();

        _shieldEnabled = true;
    }
}
