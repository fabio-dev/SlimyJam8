using UnityEngine;

public class DeathMaskAnimatorController : MonoBehaviour
{
    [SerializeField] private SpriteAnimations _sprites;
    private OneTimeSpriteAnimator _animator = new OneTimeSpriteAnimator();

    private void Start()
    {
        Setup(GetComponent<SpriteRenderer>());
    }

    public void Setup(SpriteRenderer spriteRenderer)
    {
        _animator.SetSpriteRenderer(spriteRenderer);
        _animator.SetSpritesAnimations(_sprites);
        _animator.OnComplete += OnCompleted;
        _animator.Play();
    }

    private void OnCompleted()
    {
        Kill();
        Destroy(gameObject);
    }

    public void Kill()
    {
        _animator.Kill();
    }
}