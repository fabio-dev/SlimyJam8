using UnityEngine;

public class FollowTargetStrategy : IMovementStrategy
{
    [SerializeField] private float _moveSpeedSettings = 1.0f;
    [SerializeField] private float _stopRangeSettings = 0.5f;

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

        Vector2 currentPos = _owner.transform.position;
        Vector2 targetPos = _target.position;

        Vector2 dirToTarget = (targetPos - currentPos);
        float distanceToTarget = dirToTarget.magnitude;

        if (distanceToTarget > _stopRangeSettings)
        {
            Vector2 move = dirToTarget.normalized * _moveSpeedSettings * Time.fixedDeltaTime;
            _owner.Rigidbody.MovePosition(currentPos + move);
        }
    }
}
