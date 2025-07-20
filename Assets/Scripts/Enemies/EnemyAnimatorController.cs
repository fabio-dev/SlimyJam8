using System;
using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{
    [SerializeField] private ASpriteAnimator _idleAnimator;
    [SerializeField] private ASpriteAnimator _damagedAnimator;
    [SerializeField] private ASpriteAnimator _dieAnimator;

    private EnemyGO _enemyGO;

    public void Setup(EnemyGO enemyGO)
    {
        UnregisterEvents();
        _enemyGO = enemyGO;
        _idleAnimator.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        _damagedAnimator.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        _damagedAnimator.OnComplete += PlayIdleAnimation;
        _dieAnimator.SetSpriteRenderer(_enemyGO.SpriteRenderer);
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
        _idleAnimator.Play();
    }

    private void PlayDamagedAnimation(float damaged)
    {
        _idleAnimator.Stop();
        _damagedAnimator.Replay();
    }

    private void PLayDieAnimation()
    {
        _dieAnimator.Play();
    }

    internal void Kill()
    {
        _idleAnimator.Kill();
        _damagedAnimator.Kill();
        _dieAnimator.Kill();
    }
}
