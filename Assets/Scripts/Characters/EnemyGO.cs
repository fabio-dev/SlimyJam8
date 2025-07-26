using Assets.Scripts.Domain;
using System;
using UnityEngine;

public class EnemyGO : ACharacterGO
{
    [SerializeField] private IAnimatorController _animatorController;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _health;
    [SerializeField] private float _basicAttackCooldown;
    [SerializeField] private int _score = 10;

    private DropManager _dropManager;
    public Enemy Enemy => Character as Enemy;

    public event Action OnAttack;

    public override void Setup(ACharacter character)
    {
        base.Setup(character);
        character.SetHealth(_health);
        character.SetMoveSpeed(_moveSpeed);
        character.SetAttackCooldown(_basicAttackCooldown);
        character.OnDamaged += PlayDamagedSound;
        ((Enemy)character).Score = _score;
        _animatorController.Setup(this);
        TriggerOnSetup();
    }

    private void PlayDamagedSound(float damaged)
    {
        SFXPlayer.Instance.PlayEnemyHurt();
    }

    protected override void OnDie(ACharacter character)
    {
        if (_dropManager != null)
        {
            _dropManager.Drop(transform.position);
        }

        SFXPlayer.Instance.PlayEnemyDie();
        base.OnDie(character);
    }

    public void SetDropManager(DropManager dropManager)
    {
        _dropManager = dropManager;
    }

    public void TriggerAttack(bool isMelee)
    {
        if (!isMelee)
        {
            SFXPlayer.Instance.PlayEnemyShoot();
        }
        OnAttack?.Invoke();
    }

    private void OnDestroy()
    {
        _animatorController.Kill();
        Enemy.OnDamaged -= PlayDamagedSound;
    }
}
