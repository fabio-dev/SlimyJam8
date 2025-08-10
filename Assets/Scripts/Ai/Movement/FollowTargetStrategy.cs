using UnityEngine;

public partial class FollowTargetStrategy : IMovementStrategy
{
    [SerializeField] private float _moveSpeedSettings = 1.0f;
    [SerializeField] private float _stopRangeSettings = 5f;
    [SerializeField] private float _raycastDistanceSettings = 3f; // Augmenté pour ennemis plus grands
    [SerializeField] private LayerMask _obstacleMaskSettings;
    [SerializeField] private float _avoidanceForceSettings = 2f;
    [SerializeField] private int _raycastCountSettings = 7; // Plus de rayons pour 50 ennemis
    [SerializeField] private float _enemySizeMultiplierSettings = 1.5f; // Facteur de taille pour collision

    private EnemyGO _owner = null;
    private Transform _target = null;
    private float _moveSpeed;
    private float _stopRange;
    private float _raycastDistance;
    private float _avoidanceForce;
    private int _raycastCount;
    private float _enemySizeMultiplier;
    private Vector2 _knockback;
    private LayerMask _obstacleMask;

    // Système de waypoints pour éviter les boucles
    private Vector2? _currentWaypoint = null;
    private float _waypointTimeout = 0f;
    private const float _maxWaypointTime = 3f;

    // Cache pour éviter les recalculs
    private float _lastPathfindingTime = 0f;
    private const float _pathfindingCooldown = 0.2f; // Recalcul toutes les 0.2s seulement

    private const float _knockbackRecoverySpeed = 2f;
    private const float _stuckThreshold = 0.1f; // Seuil pour détecter si on est bloqué
    private Vector2 _lastPosition;
    private float _stuckTimer = 0f;

    public Transform Target => _target;

    public void ApplyKnockback(Vector2 force)
    {
        _knockback += force;
    }

    public void Update()
    {
        if (_owner == null || _target == null) return;

        Vector2 currentPos = _owner.transform.position;

        if (_movementPaused)
        {
            _owner.Rigidbody.MovePosition(currentPos + _knockback);
        }
        else
        {
            Vector2 targetPos = _target.position;
            float distanceToTarget = Vector2.Distance(currentPos, targetPos);

            // Gestion du timeout des waypoints
            if (_waypointTimeout > 0f)
            {
                _waypointTimeout -= Time.fixedDeltaTime;
                if (_waypointTimeout <= 0f)
                {
                    _currentWaypoint = null;
                }
            }

            // Détection si on est bloqué
            CheckIfStuck(currentPos);

            Vector2 moveDirection = Vector2.zero;

            if (distanceToTarget > _stopRange)
            {
                moveDirection = CalculateMovementDirection(currentPos, targetPos);
            }

            // Application du mouvement avec knockback
            Vector2 finalMove = moveDirection * _moveSpeed * Time.fixedDeltaTime + _knockback;
            _owner.Rigidbody.MovePosition(currentPos + finalMove);
        }

        // Récupération du knockback
        _knockback = Vector2.Lerp(_knockback, Vector2.zero, Time.fixedDeltaTime * _knockbackRecoverySpeed);
        _lastPosition = currentPos;
    }

    private Vector2 CalculateMovementDirection(Vector2 currentPos, Vector2 targetPos)
    {
        Vector2 directDirection = (targetPos - currentPos).normalized;

        // Vérifier si le chemin direct est libre
        if (IsPathClear(currentPos, targetPos))
        {
            _currentWaypoint = null; // Plus besoin de waypoint
            return directDirection;
        }

        // Si on a un waypoint valide, aller vers lui
        if (_currentWaypoint.HasValue)
        {
            float waypointDistance = Vector2.Distance(currentPos, _currentWaypoint.Value);
            if (waypointDistance < 0.5f || IsPathClear(currentPos, targetPos))
            {
                _currentWaypoint = null; // Waypoint atteint ou chemin direct libre
                return directDirection;
            }
            else
            {
                return (_currentWaypoint.Value - currentPos).normalized;
            }
        }

        // Calculer une nouvelle direction d'évitement
        return CalculateAvoidanceDirection(currentPos, targetPos, directDirection);
    }

    private float EvaluateDirection(Vector2 currentPos, Vector2 targetPos, Vector2 direction)
    {
        // CircleCast pour tenir compte de la taille de l'ennemi
        float checkRadius = _enemySizeMultiplier * 0.5f;
        RaycastHit2D hit = Physics2D.CircleCast(currentPos, checkRadius, direction, _raycastDistance, _obstacleMask);

        float clearDistance = hit.collider == null ? _raycastDistance : hit.distance;
        if (clearDistance < checkRadius * 2) return -1f; // Direction bloquée

        // Score basé sur : distance libre + alignement avec la cible
        Vector2 directionToTarget = (targetPos - currentPos).normalized;
        float alignment = Vector2.Dot(direction, directionToTarget);

        return clearDistance * 0.5f + alignment * 0.5f;
    }

    private Vector2 FindWaypoint(Vector2 currentPos, Vector2 targetPos, Vector2 direction)
    {
        // Projeter un point dans la direction choisie
        RaycastHit2D hit = Physics2D.Raycast(currentPos, direction, _raycastDistance, _obstacleMask);
        float safeDistance = hit.collider == null ? _raycastDistance * 0.8f : hit.distance * 0.8f;

        return currentPos + direction * safeDistance;
    }

    private bool IsPathClear(Vector2 from, Vector2 to)
    {
        Vector2 direction = to - from;
        float distance = direction.magnitude;

        if (distance < 0.1f) return true;

        // Utiliser la taille de l'ennemi pour la détection
        float checkRadius = _enemySizeMultiplier * 0.5f;
        RaycastHit2D hit = Physics2D.CircleCast(from, checkRadius, direction.normalized, distance, _obstacleMask);
        return hit.collider == null;
    }

    private void CheckIfStuck(Vector2 currentPos)
    {
        float movementThisFrame = Vector2.Distance(currentPos, _lastPosition);

        if (movementThisFrame < _stuckThreshold)
        {
            _stuckTimer += Time.fixedDeltaTime;
            if (_stuckTimer > 1f) // Bloqué depuis 1 seconde
            {
                // Forcer la recherche d'un nouveau chemin
                _currentWaypoint = null;
                _waypointTimeout = 0f;
                _stuckTimer = 0f;
            }
        }
        else
        {
            _stuckTimer = 0f;
        }
    }
}
