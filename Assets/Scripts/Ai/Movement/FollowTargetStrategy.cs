using UnityEngine;

public class FollowTargetStrategy : IMovementStrategy
{
    [SerializeField] private float _moveSpeedSettings = 1.0f;
    [SerializeField] private float _stopRangeSettings = 5f;
    [SerializeField] private float _obstacleCheckDistanceSettings = 0.5f;
    [SerializeField] private LayerMask _obstacleMaskSettings;

    private EnemyGO _owner = null;
    private Transform _target = null;
    private float _moveSpeed;
    private float _stopRange;
    private Vector2 _knockback;
    private const float _knockbackRecoverySpeed = 1f;
    private float _obstacleCheckDistance = 0.5f;
    private LayerMask _obstacleMask;

    public Transform Target => _target;

    public void ApplyKnockback(Vector2 force)
    {
        _knockback += force;
    }

    public IMovementStrategy Init(EnemyGO owner, Transform target)
    {
        FollowTargetStrategy strategy = new FollowTargetStrategy();
        strategy._owner = owner;
        strategy._target = target;
        strategy._moveSpeed = _moveSpeedSettings;
        strategy._stopRange = _stopRangeSettings;
        strategy._obstacleMask = _obstacleMaskSettings;
        strategy._obstacleCheckDistance = _obstacleCheckDistanceSettings;
        return strategy;
    }

    public void Update()
    {
        if (_owner == null || _target == null) return;

        Vector2 currentPos = _owner.transform.position;
        Vector2 targetPos = _target.position;
        Vector2 dirToTarget = (targetPos - currentPos);
        float distanceToTarget = dirToTarget.magnitude;

        Vector2 move = Vector2.zero;

        if (distanceToTarget > _stopRange)
        {
            Vector2 desiredMove = dirToTarget.normalized * _moveSpeed * Time.fixedDeltaTime;

            // Raycast dans la direction de déplacement
            RaycastHit2D hit = Physics2D.Raycast(currentPos, dirToTarget.normalized, _obstacleCheckDistance, _obstacleMask);
            Debug.DrawRay(_owner.transform.position, dirToTarget, Color.red);
            Debug.DrawRay(_owner.transform.position, dirToTarget.normalized, Color.cyan);

            if (hit.collider != null)
            {
                // Obstacle détecté, on tente un déplacement latéral
                Vector2 perp = Vector2.Perpendicular(dirToTarget.normalized); // perpendiculaire (gauche/droite)
                Vector2 left = currentPos + perp * _obstacleCheckDistance;
                Vector2 right = currentPos - perp * _obstacleCheckDistance;

                // Check à gauche
                bool canMoveLeft = !Physics2D.Raycast(currentPos, perp, _obstacleCheckDistance, _obstacleMask);
                bool canMoveRight = !Physics2D.Raycast(currentPos, -perp, _obstacleCheckDistance, _obstacleMask);

                if (canMoveLeft)
                    desiredMove = perp.normalized * _moveSpeed * Time.fixedDeltaTime;
                else if (canMoveRight)
                    desiredMove = -perp.normalized * _moveSpeed * Time.fixedDeltaTime;
                else
                    desiredMove = Vector2.zero; // bloqué
            }

            move = desiredMove + _knockback;
        }
        else
        {
            move = _knockback.normalized * _moveSpeed * Time.fixedDeltaTime;
        }

        _owner.Rigidbody.MovePosition(currentPos + move);
        _knockback = Vector2.Lerp(_knockback, Vector2.zero, Time.fixedDeltaTime * _knockbackRecoverySpeed);
    }
}
