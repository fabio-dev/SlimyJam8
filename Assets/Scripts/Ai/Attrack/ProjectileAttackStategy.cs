using UnityEngine;

public class ProjectileAttackStategy : IAttackStrategy
{
	[SerializeField] private float _attackRangeSettings = 5.0f;
	[SerializeField] private ProjectileGO _projectilePrefab = null;

	private EnemyGO _owner = null;
	private Transform _target = null;
	private ProjectileGO _projectile = null;
	private float _attackRange = 0.0f;

	public IAttackStrategy Init(EnemyGO owner, PlayerGO target)
	{
		ProjectileAttackStategy strategy = new ProjectileAttackStategy();
		strategy._owner = owner;
		strategy._target = target.transform;
		strategy._projectile = _projectilePrefab;
		strategy._attackRange = _attackRangeSettings;
		return strategy;
	}

	public void Update()
	{
		Vector3 currentPos = _owner.transform.position;
		Vector3 targetPos = _target.transform.position;
		Vector3 directionToTaget = targetPos - currentPos;
		float distanceToTarget = directionToTaget.magnitude;

		if (distanceToTarget < _attackRange)
		{
			ProjectileGO projectileGO = GameObject.Instantiate(_projectile, currentPos, Quaternion.identity);
			projectileGO.Launch(directionToTaget);
		}
	}
}
