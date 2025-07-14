using UnityEngine;

public interface IMovementStrategy
{
	public IMovementStrategy Init(EnemyGO owner, Transform target);

	public void Update();
}
