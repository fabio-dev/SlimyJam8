using Assets.Scripts.Domain;
using System;
using UnityEngine;

public class EnemyGO : ACharacterGO
{
    [SerializeField] private IAnimatorController _animatorController;

    public override void Setup(ACharacter character)
    {
        base.Setup(character);
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
