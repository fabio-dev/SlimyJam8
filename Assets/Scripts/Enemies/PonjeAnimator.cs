using Assets.Scripts.Domain;
using UnityEngine;

public class PonjeAnimator : AAnimatorController
{
    [SerializeField] private SpriteAnimations _idleAnimations;
    [SerializeField] private SpriteAnimations _damagedAnimations;
    [SerializeField] private SpriteAnimations _dieAnimations;

    private LoopSpriteAnimator _idleAnimator = new();
    private OneTimeSpriteAnimator _damagedAnimator = new();
    private OneTimeSpriteAnimator _dieAnimator = new();
    private EnemyGO _enemyGO;
    private IMovementStrategy _movementStrategy;
    private int _facing = 1;

    private void FixedUpdate()
    {
        Vector2 currentPosition = transform.position;

        if (_movementStrategy?.Target == null)
        {
            return;
        }

        float xDiff = _movementStrategy.Target.position.x - transform.position.x;

        _facing = (xDiff >= 0) ? 1 : -1;
        _enemyGO.SetFacing(_facing);
    }

    public override void Setup(EnemyGO enemyGO)
    {
        UnregisterEvents();
        _enemyGO = enemyGO;
        _movementStrategy = _enemyGO.MovementStrategy;

        _idleAnimator.SetSpriteRenderer(enemyGO.SpriteRenderer);
        _damagedAnimator.SetSpriteRenderer(enemyGO.SpriteRenderer);
        _dieAnimator.SetSpriteRenderer(enemyGO.SpriteRenderer);

        _idleAnimator.SetSpritesAnimations(_idleAnimations);
        _damagedAnimator.SetSpritesAnimations(_damagedAnimations);
        _dieAnimator.SetSpritesAnimations(_dieAnimations);

        _damagedAnimator.OnComplete += PlayIdleAnimation;

        RegisterEvents();

        _idleAnimator.Play();
    }

    private void UnregisterEvents()
    {
        if (_enemyGO == null)
        {
            return;
        }

        _enemyGO.Enemy.OnDamaged -= PlayDamagedAnimation;
        _enemyGO.Enemy.OnDie -= PlayDieAnimation;
    }

    private void RegisterEvents()
    {
        if (_enemyGO == null)
        {
            return;
        }

        _enemyGO.Enemy.OnDamaged += PlayDamagedAnimation;
        _enemyGO.Enemy.OnDie += PlayDieAnimation;
    }

    private void PlayIdleAnimation()
    {
        _idleAnimator.Play();
    }

    private void PlayDamagedAnimation(float damaged)
    {
        _idleAnimator.Stop();
        _damagedAnimator.Replay();
    }

    private void PlayDieAnimation(ACharacter character)
    {
        _dieAnimator.Play();
    }

    public override void Kill()
    {
        _idleAnimator.Kill();
        _damagedAnimator.Kill();
        _dieAnimator.Kill();
    }
}
