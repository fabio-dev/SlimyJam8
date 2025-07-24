using Assets.Scripts.Domain;
using UnityEngine;

public class EnemyGO : ACharacterGO
{
    [SerializeField] private IAnimatorController _animatorController;

    public override void Setup(ACharacter character)
    {
        base.Setup(character);
        _animatorController.Setup(this);
    }

    public Enemy Enemy => Character as Enemy;

    private void OnDestroy()
    {
        _animatorController.Kill();
    }
}
