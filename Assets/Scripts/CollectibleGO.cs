using Assets.Scripts.Domain;
using Assets.Scripts.Domain.Collectibles;
using DG.Tweening;
using System;
using UnityEngine;

public class CollectibleGO : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    protected Sequence _idleAnimation;
    private LoopSpriteAnimator _idleAnimator;

    public event Action<ACollectible> OnCollected;
    private ACollectible _collectible;
    private PlayerGO _player;
    private LevelManager _levelManager;

    public void Setup(ACollectible collectible, PlayerGO player, LevelManager levelManager)
    {
        _collectible = collectible;
        _player = player;
        _levelManager = levelManager;
        _idleAnimation = CreateIdleAnimation();

        string sprite = collectible.GetSprite();
        if (sprite == null)
        {
            string animations = collectible.GetSpriteAnimations();
            _idleAnimator = new();
            _idleAnimator.SetSpriteRenderer(_spriteRenderer);
            _idleAnimator.SetSpritesAnimations(Resources.Load<SpriteAnimations>(animations));
            _idleAnimator.Play();
            Debug.Log("animations sprites configured");
        }
        else
        {
            _spriteRenderer.sprite = Resources.Load<Sprite>($"Sprites/{sprite}");
        }
    }

    protected virtual Sequence CreateIdleAnimation()
    {
        Sequence animation = DOTween.Sequence();
        animation.Append(transform.DOScale(1.1f, 1f));
        animation.Append(transform.DOScale(.9f, 1f));
        animation.Append(transform.DOScale(1f, 1f));
        animation.SetLoops(-1);

        return animation;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent(out PlayerGO playerGO))
        {
            Collect();
        }
    }

    private void Collect()
    {
        string[] audioNames = _collectible.GetSounds();
        AudioClip[] audioClips = new AudioClip[audioNames.Length];
        for (int i = 0; i < audioNames.Length; i++)
        {
            audioClips[i] = Resources.Load<AudioClip>($"Audio/{audioNames[i]}");
        }

        SFXPlayer.Instance.PlayAny(audioClips);
        _collectible.Collect(_player.Player, _levelManager);

        _idleAnimation.Kill();
        _idleAnimator?.Kill();
        OnCollected?.Invoke(_collectible);
        Destroy(gameObject);
    }
}
