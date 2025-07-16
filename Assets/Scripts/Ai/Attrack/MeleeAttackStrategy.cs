using Assets.Scripts.Domain;
using UnityEngine;

public class MeleeAttackStrategy : IAttackStrategy
{
	[SerializeField] private float _attackRangeSettings = 1.0f;
	[SerializeField] private float _attackDamageSettings = 1.0f;
	[SerializeField] private float _attackRateSettings = 1.0f;

	private EnemyGO _owner = null;
	private HealthComponent _targetHealthComponent = null;
	private Transform _target = null;
	private float _attackRange = 0.0f;
	private float _attackDamage = 0.0f;
	private float _attackRate = 0.0f;

	public IAttackStrategy Init(EnemyGO owner, PlayerGO target)
	{
		MeleeAttackStrategy strategy = new MeleeAttackStrategy();
		strategy._owner = owner;
		strategy._targetHealthComponent = target.Player.Health;
		strategy._target = target.transform;
		strategy._attackRange = _attackRangeSettings;
		strategy._attackDamage = _attackDamageSettings;
		strategy._attackRate = _attackRateSettings;
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
				// TODO set the correct attack rate to avoid attacking the target multiple times at once.
				_targetHealthComponent.TakeDamage(_attackDamage);
			}
		}
	}
}
