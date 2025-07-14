using Assets.Scripts.Domain;
using UnityEngine;

public interface IAttackStrategy
{
	public IAttackStrategy Init(EnemyGO owner, HealthComponent healthComponent, Transform targetTransform);

	public void Update();
}
