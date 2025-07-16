public interface IAttackStrategy
{
	public IAttackStrategy Init(EnemyGO owner, PlayerGO target);

	public void Update();
}
