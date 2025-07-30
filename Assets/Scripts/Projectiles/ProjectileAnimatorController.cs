using UnityEngine;

public class ProjectileAnimatorController : MonoBehaviour
{
    [SerializeField] private SpriteAnimations _idleAnimations;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private LoopSpriteAnimator _idleAnimator = new();

    void Start()
    {
        _idleAnimator.SetSpritesAnimations(_idleAnimations);
        _idleAnimator.SetSpriteRenderer(_spriteRenderer);
        _idleAnimator.Play();
    }

    private void OnDestroy()
    {
        _idleAnimator.Kill();
    }
}