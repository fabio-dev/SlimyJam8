using Assets.Scripts.Domain;
using System;
using UnityEngine;

public class EnemyGO : ACharacterGO
{
    [SerializeField] private IAnimatorController _animatorController;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _health;
    [SerializeField] private float _basicAttackCooldown;
    private DropManager _dropManager;
    public Enemy Enemy => Character as Enemy;

    public event Action OnAttack;

    public override void Setup(ACharacter character)
    {
        base.Setup(character);
        character.SetHealth(_health);
        character.SetMoveSpeed(_moveSpeed);
        character.BasicAttackCooldown = _basicAttackCooldown;
        _animatorController.Setup(this);
        TriggerOnSetup();
    }

    protected override void OnDie()
    {
        if (_dropManager != null)
        {
            _dropManager.Drop(transform.position);
        }
        base.OnDie();
    }

    public void SetDropManager(DropManager dropManager)
    {
        _dropManager = dropManager;
    }

    public void TriggerAttack()
    {
        OnAttack?.Invoke();
    }

    private void OnDestroy()
    {
        _animatorController.Kill();
    }
}
