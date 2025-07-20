using Assets.Scripts.Domain;

public class EnemyGO : ACharacterGO
{
    private EnemyAnimatorController _animatorController;

    private void Start()
    {
        _animatorController = GetComponent<EnemyAnimatorController>();
    }

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
