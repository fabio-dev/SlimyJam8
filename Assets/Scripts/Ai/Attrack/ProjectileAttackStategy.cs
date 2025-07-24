using Assets.Scripts.Domain;
using UnityEngine;

public class ProjectileAttackStategy : IAttackStrategy
{
	[SerializeField] private float _attackRangeSettings = 5.0f;
	[SerializeField] private float _attackRateSettings = 1f;
	[SerializeField] private float _damageAmount = 1.0f;
	[SerializeField] private ProjectileGO _projectilePrefab = null;

	private EnemyGO _owner = null;
	private Transform _target = null;
	private ProjectileGO _projectile = null;
	private Cooldown _cooldown = null;
	private float _attackRange = 0.0f;
	private float _damage = 0.0f;

	public IAttackStrategy Init(EnemyGO owner, PlayerGO target)
	{
		ProjectileAttackStategy strategy = new ProjectileAttackStategy();
		strategy._owner = owner;
		strategy._target = target.transform;
		strategy._projectile = _projectilePrefab;
		strategy._attackRange = _attackRangeSettings;
		strategy._cooldown = new Cooldown(_attackRateSettings);
		strategy._damage = _damageAmount;
		return strategy;
	}

	public void Update()
	{
		if (_target == null)
		{
			return;
		}

		Vector3 currentPos = _owner.transform.position;
		Vector3 targetPos = _target.transform.position;
		Vector3 directionToTaget = targetPos - currentPos;
		directionToTaget.z = 0f;
		float distanceToTarget = directionToTaget.magnitude;

		if (distanceToTarget < _attackRange && !_cooldown.IsRunning())
		{
			_owner.TriggerAttack();

			ProjectileGO projectileGO = Object.Instantiate(_projectile, currentPos, Quaternion.identity);
			projectileGO.transform.right = directionToTaget;
			projectileGO.Launch(directionToTaget, _damage, .2f);
			Object.Destroy(projectileGO, 2f);
			_cooldown.Start();
        }
    }
}
