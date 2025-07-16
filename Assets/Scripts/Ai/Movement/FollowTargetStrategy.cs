using UnityEngine;

public class FollowTargetStrategy : IMovementStrategy
{
	[SerializeField] private float _moveSpeedSettings = 1.0f;
	[SerializeField] private float _stopRangeSettings = 0.5f;

	private EnemyGO _owner = null;
	private Transform _target = null;
	private float _moveSpeed = 1.0f;
	private float _stopRange = 1.0f;

	public IMovementStrategy Init(EnemyGO owner, Transform target)
	{
		FollowTargetStrategy strategy = new FollowTargetStrategy();
		strategy._owner = owner;
		strategy._target = target;
		strategy._moveSpeed = _moveSpeedSettings;
		strategy._stopRange = _stopRangeSettings;
		return strategy;
	}

	public void Update()
	{
		if (_owner == null || _target == null)
		{
			return;
		}

		Vector3 currentPos = _owner.transform.position;
		Vector3 targetPos = _target.position;

		Vector3 dirToTarget = (targetPos - currentPos);
		float distanceToTarget = dirToTarget.magnitude;
		dirToTarget.z = 0;

		if (distanceToTarget >= _stopRange)
		{
			_owner.transform.position = currentPos + dirToTarget.normalized * _moveSpeed * Time.deltaTime;
		}
	}
}
