using Assets.Scripts.Domain;

public class EnemyGO : ACharacterGO
{
	public Enemy Enemy { get { return Character as Enemy; } }
}
