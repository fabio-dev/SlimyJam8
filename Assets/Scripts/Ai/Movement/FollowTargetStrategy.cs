using UnityEngine;

public class FollowTargetStrategy : IMovementStrategy
{
    [SerializeField] private float _moveSpeedSettings = 1.0f;
    [SerializeField] private float _stopRangeSettings = 5f;

    private EnemyGO _owner = null;
    private Transform _target = null;
    private float _moveSpeed;
    private float _stopRange;

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

        Vector2 currentPos = _owner.transform.position;
        Vector2 targetPos = _target.position;

        Vector2 dirToTarget = (targetPos - currentPos);
        float distanceToTarget = dirToTarget.magnitude;

        if (distanceToTarget > _stopRange)
        {
            Vector2 move = dirToTarget.normalized * _moveSpeed * Time.fixedDeltaTime;
            _owner.Rigidbody.MovePosition(currentPos + move);
        }
    }
}
