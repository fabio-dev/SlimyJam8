using Assets.Scripts.Domain;
using UnityEngine;

public class EnemyGO : ACharacterGO
{
    [SerializeField] private EnemyAnimatorController _animatorController;

    public override void Setup(ACharacter character)
    {
        Debug.Log("Setup enemy");
        base.Setup(character);
        _animatorController.Setup(this);
    }

    public Enemy Enemy => Character as Enemy;

    private void OnDestroy()
    {
        _animatorController.Kill();
    }
}
