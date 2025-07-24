using Assets.Scripts.Domain;
using UnityEngine;

public class MeleeAttackStrategy : IAttackStrategy
{
	[SerializeField] private float _attackRangeSettings = 1f;
	[SerializeField] private float _attackDamageSettings = 1f;
	[SerializeField] private float _attackRateSettings = 1f;

	private EnemyGO _owner = null;
	private HealthComponent _targetHealthComponent = null;
	private Transform _target = null;
	private float _attackRange;
	private float _attackDamage;
	private Cooldown _cooldown = null;

	public IAttackStrategy Init(EnemyGO owner, PlayerGO target)
	{
		MeleeAttackStrategy strategy = new MeleeAttackStrategy();
		strategy._owner = owner;
		strategy._targetHealthComponent = target.Player.Health;
		strategy._target = target.Center;
		strategy._attackRange = _attackRangeSettings;
		strategy._attackDamage = _attackDamageSettings;
		strategy._cooldown = new Cooldown(_attackRateSettings);
		return strategy;
	}

	public void Update()
	{
		if (_target != null || _targetHealthComponent != null)
		{
			Vector3 currentPos = _owner.transform.position;
			Vector3 targetPos = _target.transform.position;
			float distanceToTarget = (targetPos - currentPos).magnitude;

			if (distanceToTarget <= _attackRange && !_cooldown.IsRunning())
			{
				_targetHealthComponent.TakeDamage(_attackDamage);
				_cooldown.Start();
				_owner.TriggerAttack();
            }
        }
	}
}
