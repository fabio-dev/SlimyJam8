using UnityEngine;

public interface IMovementStrategy
{
	public void ApplyKnockback(Vector2 force);

	public IMovementStrategy Init(EnemyGO owner, Transform target);

	public void Update();

	public Transform Target { get; }
}
