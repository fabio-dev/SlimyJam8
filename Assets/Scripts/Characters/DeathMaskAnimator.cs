using Assets.Scripts.Domain;
using UnityEngine;

public class DeathMaskAnimatorController : MonoBehaviour
{
    [SerializeField] private SpriteAnimations _sprites;
    private OneTimeSpriteAnimator _animator = new OneTimeSpriteAnimator();
    private Cooldown _startDelay = new Cooldown(0f);
    private bool _started;

    private void Start()
    {
        Setup(GetComponent<SpriteRenderer>());
    }

    private void Update()
    {
        if (_startDelay.IsRunning() || _started)
        {
            return;
        }

        _started = true;
        _animator.Play();
    }

    public void Setup(SpriteRenderer spriteRenderer)
    {
        _animator.SetSpriteRenderer(spriteRenderer);
        _animator.SetSpritesAnimations(_sprites);
        _animator.OnComplete += OnCompleted;
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

    internal void StartIn(float delay)
    {
        _startDelay.SetDuration(delay);
        _startDelay.Start();
    }
}