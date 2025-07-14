using Assets.Scripts.Domain;
using UnityEngine;

public class MeleeAttackStrategy : IAttackStrategy
{
	[SerializeField] private float _attackRange = 1.0f;
	[SerializeField] private float _attackDamage = 1.0f;
	[SerializeField] private float _attackRate = 1.0f;

	private EnemyGO _owner = null;
	private HealthComponent _targetHealthComponent = null;
	private Transform _target = null;

	public IAttackStrategy Init(EnemyGO owner, HealthComponent healthComponent, Transform targetTransform)
	{
		MeleeAttackStrategy strategy = new MeleeAttackStrategy();
		strategy._owner = owner;
		strategy._targetHealthComponent = healthComponent;
		strategy._target = targetTransform;
		return strategy;
	}

	public void Update()
	{
		if (_target != null || _targetHealthComponent != null)
		{
			Vector3 currentPos = _owner.transform.position;
			Vector3 targetPos = _target.transform.position;
			float distanceToTarget = (targetPos - currentPos).magnitude;

			if (distanceToTarget <= _attackRange)
			{
				UnityEngine.Debug.Log("ATTACK");
				// TODO set the correct attack rate to avoid attacking the target multiple times at once.
				_targetHealthComponent.TakeDamage(_attackDamage);
			}
		}
	}
}
