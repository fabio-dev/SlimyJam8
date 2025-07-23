using UnityEngine;

public class SnekAnimatorController : MonoBehaviour
{
    [SerializeField] private SpriteAnimations _idleAnimationsFace;
    [SerializeField] private SpriteAnimations _idleAnimationsSide;
    [SerializeField] private SpriteAnimations _idleAnimationsBack;
    [SerializeField] private SpriteAnimations _damagedAnimationsFace;
    [SerializeField] private SpriteAnimations _damagedAnimationsSide;
    [SerializeField] private SpriteAnimations _damagedAnimationsBack;
    [SerializeField] private SpriteAnimations _dieAnimationsFace;
    [SerializeField] private SpriteAnimations _dieAnimationsSide;
    [SerializeField] private SpriteAnimations _dieAnimationsBack;
    [SerializeField] private SpriteAnimations _attackAnimationsFace;
    [SerializeField] private SpriteAnimations _attackAnimationsSide;
    [SerializeField] private SpriteAnimations _attackAnimationsBack;

    private EnemyGO _enemyGO;
    private Vector2 _lastPosition;

    public void Setup(EnemyGO enemyGO)
    {
        UnregisterEvents();
        _enemyGO = enemyGO;

        //_idleAnimatorFace.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        //_idleAnimatorSide.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        //_idleAnimatorBack.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        //_damagedAnimatorFace.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        //_damagedAnimatorSide.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        //_damagedAnimatorBack.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        //_dieAnimatorFace.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        //_dieAnimatorSide.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        //_dieAnimatorBack.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        //_attackAnimatorFace.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        //_attackAnimatorSide.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        //_attackAnimatorBack.SetSpriteRenderer(_enemyGO.SpriteRenderer);

        //_damagedAnimatorFace.OnComplete += PlayIdleAnimation;
        //_damagedAnimatorSide.OnComplete += PlayIdleAnimation;
        //_damagedAnimatorBack.OnComplete += PlayIdleAnimation;

        RegisterEvents();
        PlayIdleAnimation();
    }

    private void UnregisterEvents()
    {
        if (_enemyGO == null)
        {
            return;
        }

        _enemyGO.Enemy.OnDamaged -= PlayDamagedAnimation;
        _enemyGO.Enemy.OnDie -= PLayDieAnimation;
    }

    private void RegisterEvents()
    {
        if (_enemyGO == null)
        {
            return;
        }

        _enemyGO.Enemy.OnDamaged += PlayDamagedAnimation;
        _enemyGO.Enemy.OnDie += PLayDieAnimation;
    }

    private void PlayIdleAnimation()
    {

    }

    private void PlayDamagedAnimation(float damaged)
    {
    }

    private void PLayDieAnimation()
    {
    }

    internal void Kill()
    {
    }
}
