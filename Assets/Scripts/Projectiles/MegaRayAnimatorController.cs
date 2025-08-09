using UnityEngine;

public class MegaRayAnimatorController : MonoBehaviour
{
    [SerializeField] private SpriteAnimations _headAnimations;
    [SerializeField] private SpriteAnimations _bodyAnimations;
    [SerializeField] private SpriteAnimations _footAnimations;
    [SerializeField] private SpriteRenderer _headSpriteRenderer;
    [SerializeField] private SpriteRenderer _bodySpriteRenderer;
    [SerializeField] private SpriteRenderer _footSpriteRenderer;

    private LoopSpriteAnimator _headAnimator = new();
    private LoopSpriteAnimator _bodyAnimator = new();
    private LoopSpriteAnimator _footAnimator = new();

    void Start()
    {
        _headAnimator.SetSpritesAnimations(_headAnimations);
        _headAnimator.SetSpriteRenderer(_headSpriteRenderer);
        _headAnimator.Play();
        _bodyAnimator.SetSpritesAnimations(_bodyAnimations);
        _bodyAnimator.SetSpriteRenderer(_bodySpriteRenderer);
        _bodyAnimator.Play();
        _footAnimator.SetSpritesAnimations(_footAnimations);
        _footAnimator.SetSpriteRenderer(_footSpriteRenderer);
        _footAnimator.Play();
    }

    private void OnDestroy()
    {
        _headAnimator.Kill();
        _bodyAnimator.Kill();
        _footAnimator.Kill();
    }
}