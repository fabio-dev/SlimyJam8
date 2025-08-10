using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementManager : MonoBehaviour
{
    private static EnemyMovementManager _instance;
    public static EnemyMovementManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<EnemyMovementManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("EnemyMovementManager");
                    _instance = go.AddComponent<EnemyMovementManager>();
                }
            }
            return _instance;
        }
    }

    [SerializeField] private int _maxPathfindingPerFrame = 5; // Max d'ennemis qui calculent par frame

    private Queue<FollowTargetStrategy> _pathfindingQueue = new Queue<FollowTargetStrategy>();
    private List<FollowTargetStrategy> _registeredStrategies = new List<FollowTargetStrategy>();
    private int _currentFrameCalculations = 0;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        _currentFrameCalculations = 0;
    }

    public void RegisterStrategy(FollowTargetStrategy strategy)
    {
        if (!_registeredStrategies.Contains(strategy))
        {
            _registeredStrategies.Add(strategy);
        }
    }

    public void UnregisterStrategy(FollowTargetStrategy strategy)
    {
        _registeredStrategies.Remove(strategy);

        // Retirer de la queue si présent
        var tempQueue = new Queue<FollowTargetStrategy>();
        while (_pathfindingQueue.Count > 0)
        {
            var item = _pathfindingQueue.Dequeue();
            if (item != strategy)
                tempQueue.Enqueue(item);
        }
        _pathfindingQueue = tempQueue;
    }

    public bool CanCalculatePathfinding(FollowTargetStrategy strategy)
    {
        if (_currentFrameCalculations < _maxPathfindingPerFrame)
        {
            _currentFrameCalculations++;
            return true;
        }

        // Ajouter à la queue si pas déjà présent
        if (!_pathfindingQueue.Contains(strategy))
        {
            _pathfindingQueue.Enqueue(strategy);
        }

        return false;
    }

    public void ProcessQueuedPathfinding()
    {
        while (_pathfindingQueue.Count > 0 && _currentFrameCalculations < _maxPathfindingPerFrame)
        {
            var strategy = _pathfindingQueue.Dequeue();
            if (strategy != null)
            {
                strategy.ForcePathfindingCalculation();
                _currentFrameCalculations++;
            }
        }
    }

    private void Update()
    {
        ProcessQueuedPathfinding();
    }

    // Méthode pour obtenir des infos de debug
    public void GetDebugInfo(out int totalEnemies, out int queuedCalculations, out int currentFrameCalcs)
    {
        totalEnemies = _registeredStrategies.Count;
        queuedCalculations = _pathfindingQueue.Count;
        currentFrameCalcs = _currentFrameCalculations;
    }
}

// Extension de la stratégie pour intégrer le manager
public partial class FollowTargetStrategy : IMovementStrategy
{
    private bool _isRegistered = false;
    private bool _movementPaused = false;

    // Modification de la méthode Init
    public IMovementStrategy Init(EnemyGO owner, Transform target)
    {
        FollowTargetStrategy strategy = new FollowTargetStrategy();
        strategy._owner = owner;
        strategy._target = target;
        strategy._moveSpeed = _moveSpeedSettings;
        strategy._stopRange = _stopRangeSettings;
        strategy._obstacleMask = _obstacleMaskSettings;
        strategy._raycastDistance = _raycastDistanceSettings;
        strategy._avoidanceForce = _avoidanceForceSettings;
        strategy._raycastCount = _raycastCountSettings;
        strategy._enemySizeMultiplier = _enemySizeMultiplierSettings;
        strategy._lastPosition = owner.transform.position;

        // S'enregistrer auprès du manager
        EnemyMovementManager.Instance.RegisterStrategy(strategy);
        strategy._isRegistered = true;

        return strategy;
    }

    // Méthode pour forcer le calcul (appelée par le manager)
    public void ForcePathfindingCalculation()
    {
        if (_owner == null || _target == null) return;

        Vector2 currentPos = _owner.transform.position;
        Vector2 targetPos = _target.position;

        // Forcer le recalcul même si le cooldown n'est pas écoulé
        _lastPathfindingTime = Time.time;
        Vector2 directDirection = (targetPos - currentPos).normalized;
        CalculateAvoidanceDirection(currentPos, targetPos, directDirection);
    }

    // Modification de CalculateAvoidanceDirection pour utiliser le manager
    private Vector2 CalculateAvoidanceDirection(Vector2 currentPos, Vector2 targetPos, Vector2 desiredDirection)
    {
        Vector2 bestDirection = desiredDirection;
        float bestScore = -1f;

        // Vérifier si on peut calculer cette frame
        bool canCalculate = EnemyMovementManager.Instance.CanCalculatePathfinding(this);

        if (canCalculate || Time.time > _lastPathfindingTime + _pathfindingCooldown * 2) // Force si trop ancien
        {
            float angleStep = 360f / _raycastCount;
            float baseAngle = Mathf.Atan2(desiredDirection.y, desiredDirection.x) * Mathf.Rad2Deg;

            for (int i = 0; i < _raycastCount; i++)
            {
                float angle = baseAngle + (i - _raycastCount / 2) * angleStep / 2;
                Vector2 testDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

                float score = EvaluateDirection(currentPos, targetPos, testDirection);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestDirection = testDirection;
                }
            }

            if (bestScore > 0f)
            {
                _currentWaypoint = FindWaypoint(currentPos, targetPos, bestDirection);
                _waypointTimeout = _maxWaypointTime;
                _lastPathfindingTime = Time.time;
            }
        }

        return bestDirection;
    }

    // Méthode de nettoyage
    public void Cleanup()
    {
        if (_isRegistered)
        {
            EnemyMovementManager.Instance.UnregisterStrategy(this);
            _isRegistered = false;
        }
    }

    public void Pause()
    {
        _movementPaused = true;
    }

    public void Resume()
    {
        _movementPaused = false;
    }
}
