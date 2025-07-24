using Assets.Scripts.Domain;
using System;
using UnityEngine;

public class EnemyGO : ACharacterGO
{
    [SerializeField] private IAnimatorController _animatorController;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _health;
    [SerializeField] private float _basicAttackCooldown;

    public override void Setup(ACharacter character)
    {
        base.Setup(character);
        character.SetHealth(_health);
        character.SetMoveSpeed(_moveSpeed);
        character.BasicAttackCooldown = _basicAttackCooldown;
        _animatorController.Setup(this);
        TriggerOnSetup();
    }

    public Enemy Enemy => Character as Enemy;

    public event Action OnAttack;

    public void TriggerAttack()
    {
        OnAttack?.Invoke();
    }

    private void OnDestroy()
    {
        _animatorController.Kill();
    }
}
