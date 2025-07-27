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

    private static readonly RaycastHit2D[] _hitsBuffer = new RaycastHit2D[2]; // petite alloc réutilisable

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

    private Vector2? _temporaryTarget = null;

    public void Update()
    {
        if (_owner == null || _target == null) return;

        Vector2 currentPos = _owner.transform.position;
        Vector2 realTarget = _target.position;
        Vector2 targetPos = _temporaryTarget ?? realTarget;
        Vector2 dirToTarget = targetPos - currentPos;
        float distanceToTarget = dirToTarget.magnitude;
        Vector2 move = Vector2.zero;

        if (distanceToTarget > _stopRange)
        {
            Vector2 desiredMove = dirToTarget.normalized * _moveSpeed * Time.fixedDeltaTime;

            // Création du filtre collision
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(_obstacleMask);
            filter.useTriggers = false;

            // Test collision dans la direction désirée
            int hitCount = _owner.Rigidbody.Cast(desiredMove.normalized, filter, _hitsBuffer, desiredMove.magnitude);

            if (hitCount == 0)
            {
                // Pas d'obstacle, on avance normalement vers la cible
                move = desiredMove + _knockback;

                // Si on était en train de contourner et qu'on peut maintenant aller vers la cible réelle
                if (_temporaryTarget != null && CanReachTarget(currentPos, realTarget, filter))
                {
                    _temporaryTarget = null;
                }
            }
            else
            {
                // Obstacle détecté : système de contournement amélioré
                move = HandleObstacleAvoidance(currentPos, realTarget, desiredMove, filter) + _knockback;
            }
        }
        else
        {
            // Proche de la cible (temporaire ou réelle)
            if (_temporaryTarget != null)
            {
                // On a atteint la cible temporaire
                _temporaryTarget = null;
            }
            move = _knockback.normalized * _moveSpeed * Time.fixedDeltaTime;
        }

        _owner.Rigidbody.MovePosition(currentPos + move);
        _knockback = Vector2.Lerp(_knockback, Vector2.zero, Time.fixedDeltaTime * _knockbackRecoverySpeed);
    }

    private Vector2 HandleObstacleAvoidance(Vector2 currentPos, Vector2 realTarget, Vector2 desiredMove, ContactFilter2D filter)
    {
        Vector2 obstacleNormal = _hitsBuffer[0].normal;

        // Calculer les deux directions possibles de contournement
        Vector2 rightDirection = Vector2.Perpendicular(obstacleNormal);
        Vector2 leftDirection = -rightDirection;

        // Déterminer quelle direction nous rapproche le plus de la cible réelle
        Vector2 toRealTarget = (realTarget - currentPos).normalized;
        float rightDot = Vector2.Dot(rightDirection, toRealTarget);
        float leftDot = Vector2.Dot(leftDirection, toRealTarget);

        // Choisir la meilleure direction
        Vector2 preferredDirection = rightDot > leftDot ? rightDirection : leftDirection;
        Vector2 alternateDirection = rightDot > leftDot ? leftDirection : rightDirection;

        // Tester d'abord la direction préférée
        Vector2 moveAttempt = TryMoveInDirection(currentPos, preferredDirection, filter);
        if (moveAttempt != Vector2.zero)
        {
            // Définir une cible temporaire dans cette direction
            SetTemporaryTarget(currentPos, preferredDirection, realTarget, filter);
            return moveAttempt;
        }

        // Si la direction préférée est bloquée, essayer l'autre
        moveAttempt = TryMoveInDirection(currentPos, alternateDirection, filter);
        if (moveAttempt != Vector2.zero)
        {
            SetTemporaryTarget(currentPos, alternateDirection, realTarget, filter);
            return moveAttempt;
        }

        // Si les deux directions sont bloquées, essayer de glisser le long de l'obstacle
        Vector2 slideDirection = Vector2.Perpendicular(obstacleNormal);
        if (Vector2.Dot(slideDirection, desiredMove) < 0)
            slideDirection = -slideDirection;

        return TryMoveInDirection(currentPos, slideDirection, filter);
    }

    private Vector2 TryMoveInDirection(Vector2 currentPos, Vector2 direction, ContactFilter2D filter)
    {
        Vector2 moveVector = direction.normalized * _moveSpeed * Time.fixedDeltaTime;
        int hitCount = _owner.Rigidbody.Cast(moveVector.normalized, filter, _hitsBuffer, moveVector.magnitude);

        if (hitCount == 0)
        {
            return moveVector;
        }

        return Vector2.zero;
    }

    private void SetTemporaryTarget(Vector2 currentPos, Vector2 direction, Vector2 realTarget, ContactFilter2D filter)
    {
        // Projeter un rayon dans la direction choisie pour trouver un point de contournement
        float maxDistance = 10f; // Distance maximale pour chercher un point de contournement
        float stepDistance = 1f; // Distance entre chaque test

        for (float distance = stepDistance; distance <= maxDistance; distance += stepDistance)
        {
            Vector2 testPoint = currentPos + direction.normalized * distance;

            // Vérifier si depuis ce point on peut voir la cible réelle
            if (CanReachTarget(testPoint, realTarget, filter))
            {
                _temporaryTarget = testPoint;
                return;
            }
        }

        // Si aucun point optimal n'est trouvé, utiliser une distance fixe
        _temporaryTarget = currentPos + direction.normalized * 3f;
    }

    private bool CanReachTarget(Vector2 from, Vector2 to, ContactFilter2D filter)
    {
        Vector2 directionToTarget = to - from;
        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget < 0.1f) return true;

        // Utiliser un raycast pour vérifier s'il y a des obstacles
        int hitCount = _owner.Rigidbody.Cast(directionToTarget.normalized, filter, _hitsBuffer, distanceToTarget);
        return hitCount == 0;
    }
}
