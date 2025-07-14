using UnityEngine;

public class FollowTargetStrategy : IMovementStrategy
{
	[SerializeField] private float _moveSpeed = 1.0f;
	[SerializeField] private float _stopRange = 0.5f;

	private EnemyGO _owner = null;
	private Transform _target = null;

	public IMovementStrategy Init(EnemyGO owner, Transform target)
	{
		FollowTargetStrategy strategy = new FollowTargetStrategy();
		strategy._owner = owner;
		strategy._target = target;
		return strategy;
	}

	public void Update()
	{
		if (_owner == null || _target == null)
		{
			return;
		}

		float moveSpeed = _owner.Enemy.MoveSpeed;
		Vector3 currentPos = _owner.transform.position;
		Vector3 targetPos = _target.position;

		Vector3 dirToTarget = (targetPos - currentPos);
		float distanceToTarget = dirToTarget.magnitude;
		dirToTarget.z = 0;

		if (distanceToTarget >= _stopRange)
		{
			_owner.transform.position = currentPos + dirToTarget.normalized * moveSpeed * Time.deltaTime;
		}
	}
}
