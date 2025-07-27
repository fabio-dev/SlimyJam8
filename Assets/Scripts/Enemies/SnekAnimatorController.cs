using Assets.Scripts.Domain;
using System;
using UnityEngine;

public class SnekAnimatorController : MonoBehaviour, IAnimatorController
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

    private LoopSpriteAnimator _idleAnimatorFace = new LoopSpriteAnimator();
    private LoopSpriteAnimator _idleAnimatorSide = new LoopSpriteAnimator();
    private LoopSpriteAnimator _idleAnimatorBack = new LoopSpriteAnimator();
    private OneTimeSpriteAnimator _damagedAnimatorFace = new OneTimeSpriteAnimator();
    private OneTimeSpriteAnimator _damagedAnimatorSide = new OneTimeSpriteAnimator();
    private OneTimeSpriteAnimator _damagedAnimatorBack = new OneTimeSpriteAnimator();
    private OneTimeSpriteAnimator _dieAnimatorFace = new OneTimeSpriteAnimator();
    private OneTimeSpriteAnimator _dieAnimatorSide = new OneTimeSpriteAnimator();
    private OneTimeSpriteAnimator _dieAnimatorBack = new OneTimeSpriteAnimator();
    private OneTimeSpriteAnimator _attackAnimatorFace = new OneTimeSpriteAnimator();
    private OneTimeSpriteAnimator _attackAnimatorSide = new OneTimeSpriteAnimator();
    private OneTimeSpriteAnimator _attackAnimatorBack = new OneTimeSpriteAnimator();
    private ASpriteAnimator _currentAnimator;

    private EnemyGO _enemyGO;
    private IMovementStrategy _movementStrategy;

    private SpriteOrientation _orientation;
    private float _scale;

    private event Action<SpriteOrientation> _onOrientationChanged;

    public void Setup(EnemyGO enemyGO)
    {
        UnregisterEvents();
        _enemyGO = enemyGO;
        _movementStrategy = _enemyGO.MovementStrategy;
        _idleAnimatorFace.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        _idleAnimatorSide.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        _idleAnimatorBack.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        _damagedAnimatorFace.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        _damagedAnimatorSide.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        _damagedAnimatorBack.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        _dieAnimatorFace.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        _dieAnimatorSide.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        _dieAnimatorBack.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        _attackAnimatorFace.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        _attackAnimatorSide.SetSpriteRenderer(_enemyGO.SpriteRenderer);
        _attackAnimatorBack.SetSpriteRenderer(_enemyGO.SpriteRenderer);

        _idleAnimatorFace.SetSpritesAnimations(_idleAnimationsFace);
        _idleAnimatorSide.SetSpritesAnimations(_idleAnimationsSide);
        _idleAnimatorBack.SetSpritesAnimations(_idleAnimationsBack);
        _damagedAnimatorFace.SetSpritesAnimations(_damagedAnimationsFace);
        _damagedAnimatorSide.SetSpritesAnimations(_damagedAnimationsSide);
        _damagedAnimatorBack.SetSpritesAnimations(_damagedAnimationsBack);
        _dieAnimatorFace.SetSpritesAnimations(_dieAnimationsFace);
        _dieAnimatorSide.SetSpritesAnimations(_dieAnimationsSide);
        _dieAnimatorBack.SetSpritesAnimations(_dieAnimationsBack);
        _attackAnimatorFace.SetSpritesAnimations(_attackAnimationsFace);
        _attackAnimatorSide.SetSpritesAnimations(_attackAnimationsSide);
        _attackAnimatorBack.SetSpritesAnimations(_attackAnimationsBack);

        RegisterEvents();
        PlayIdleAnimation();
    }

    private void Start()
    {
        _scale = transform.localScale.x;
    }

    private void FixedUpdate()
    {
        Vector2 currentPosition = transform.position;

        if (_movementStrategy?.Target == null)
        {
            return;
        }

        double xDiff = _movementStrategy.Target.position.x - currentPosition.x;
        double yDiff = _movementStrategy.Target.position.y - currentPosition.y;

        if (xDiff >= 0)
        {
            LookRight();
        }
        else
        {
            LookLeft();
        }

        if (Math.Abs(xDiff) > Math.Abs(yDiff))
        {
            SetOrientation(SpriteOrientation.Side);
        }
        else
        {
            SetOrientation(yDiff >= 0 ? SpriteOrientation.Back : SpriteOrientation.Face);
        }
    }

    private void SetOrientation(SpriteOrientation orientation)
    {
        if (_orientation == orientation)
        {
            return;
        }
        _orientation = orientation;
        _onOrientationChanged?.Invoke(orientation);
    }

    private void LookRight() => transform.localScale = new Vector2(_scale, _scale);
    private void LookLeft() => transform.localScale = new Vector2(-_scale, _scale);

    private void UnregisterEvents()
    {
        if (_enemyGO == null)
        {
            return;
        }

        _enemyGO.Enemy.OnDamaged -= PlayDamagedAnimation;
        _enemyGO.Enemy.OnDie -= PlayDieAnimation;
        _onOrientationChanged -= OrientationChanged;
        _damagedAnimatorFace.OnComplete -= PlayIdleAnimation;
        _damagedAnimatorSide.OnComplete -= PlayIdleAnimation;
        _damagedAnimatorBack.OnComplete -= PlayIdleAnimation;
        _attackAnimatorFace.OnComplete -= PlayIdleAnimation;
        _attackAnimatorSide.OnComplete -= PlayIdleAnimation;
        _attackAnimatorBack.OnComplete -= PlayIdleAnimation;
    }

    private void RegisterEvents()
    {
        if (_enemyGO == null)
        {
            return;
        }

        _enemyGO.Enemy.OnDamaged += PlayDamagedAnimation;
        _enemyGO.Enemy.OnDie += PlayDieAnimation;
        _onOrientationChanged += OrientationChanged;
        _enemyGO.OnAttack += PlayAttackAnimation;
        _damagedAnimatorFace.OnComplete += PlayIdleAnimation;
        _damagedAnimatorSide.OnComplete += PlayIdleAnimation;
        _damagedAnimatorBack.OnComplete += PlayIdleAnimation;
        _attackAnimatorFace.OnComplete += PlayIdleAnimation;
        _attackAnimatorSide.OnComplete += PlayIdleAnimation;
        _attackAnimatorBack.OnComplete += PlayIdleAnimation;
    }

    private void OrientationChanged(SpriteOrientation obj)
    {
        ReplayAnimator();
    }

    private void ReplayAnimator()
    {
        if (_currentAnimator == null)
        {
            return;
        }

        if (_currentAnimator == _idleAnimatorBack
            || _currentAnimator == _idleAnimatorFace
            || _currentAnimator == _idleAnimatorSide)
        {
            PlayIdleAnimation();
        }
    }

    private void PlayIdleAnimation()
    {
        switch (_orientation)
        {
            case SpriteOrientation.Side:
                Play(_idleAnimatorSide);
                break;
            case SpriteOrientation.Back:
                Play(_idleAnimatorBack);
                break;
            case SpriteOrientation.Face:
            default:
                Play(_idleAnimatorFace);
                break;
        }
    }

    private void PlayDamagedAnimation(float damaged)
    {
        PlayDamagedAnimation();
    }

    private void PlayDamagedAnimation()
    {
        switch (_orientation)
        {
            case SpriteOrientation.Side:
                Play(_damagedAnimatorSide);
                break;
            case SpriteOrientation.Back:
                Play(_damagedAnimatorBack);
                break;
            case SpriteOrientation.Face:
            default:
                Play(_damagedAnimatorFace);
                break;
        }
    }

    private void PlayDieAnimation(ACharacter character)
    {
        switch (_orientation)
        {
            case SpriteOrientation.Side:
                Play(_dieAnimatorSide);
                break;
            case SpriteOrientation.Back:
                Play(_dieAnimatorBack);
                break;
            case SpriteOrientation.Face:
            default:
                Play(_dieAnimatorFace);
                break;
        }
    }

    private void PlayAttackAnimation()
    {
        switch (_orientation)
        {
            case SpriteOrientation.Side:
                Play(_attackAnimatorSide);
                break;
            case SpriteOrientation.Back:
                Play(_attackAnimatorBack);
                break;
            case SpriteOrientation.Face:
            default:
                Play(_attackAnimatorFace);
                break;
        }
    }

    private void Play(ASpriteAnimator animator)
    {
        if (_currentAnimator == animator)
        {
            return;
        }

        if (_currentAnimator != null)
        {
            _currentAnimator.Stop();
        }

        _currentAnimator = animator;
        _currentAnimator.Play();
    }

    public void Kill()
    {
        _idleAnimatorFace.Kill();
        _idleAnimatorSide.Kill();
        _idleAnimatorBack.Kill();
        _damagedAnimatorFace.Kill();
        _damagedAnimatorSide.Kill();
        _damagedAnimatorBack.Kill();
        _dieAnimatorFace.Kill();
        _dieAnimatorSide.Kill();
        _dieAnimatorBack.Kill();
        _attackAnimatorFace.Kill();
        _attackAnimatorSide.Kill();
        _attackAnimatorBack.Kill();
    }

    private enum SpriteOrientation
    {
        Face,
        Side,
        Back,
    }
}
