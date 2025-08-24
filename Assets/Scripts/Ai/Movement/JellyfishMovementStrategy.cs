using UnityEngine;

public class JellyfishMovementStrategy : IMovementStrategy
{
    private EnemyGO _owner;
    private Transform _target;
    private Rigidbody2D _rigidbody;
    private bool _isMovementPaused;

    // --- PARAMÈTRES À AJUSTER POUR LE COMPORTEMENT ---
    // La force initiale de l'impulsion. Une valeur plus élevée donne une accélération plus forte au départ.
    private readonly float _pulseForce = 10f;

    // La rapidité avec laquelle le mouvement ralentit. Une valeur plus élevée signifie que l'ennemi s'arrêtera plus vite.
    private readonly float _decelerationFactor = 6f;

    // La durée de l'impulsion de mouvement.
    private readonly float _pulseDuration = 0.4f;

    // La durée de la pause entre les impulsions (réduite comme demandé).
    private readonly float _pauseDuration = 0.4f;
    // ---------------------------------------------------

    private float _timer;
    private bool _isPulsing;
    private Vector2 _currentVelocity;
    private Vector2 _knockback;
    private const float KnockbackRecoverySpeed = 2f;

    public Transform Target => _target;

    public IMovementStrategy Init(EnemyGO owner, Transform target)
    {
        _owner = owner;
        _target = target;
        _rigidbody = owner.GetComponent<Rigidbody2D>();
        _timer = _pauseDuration; // On commence par une pause
        _isPulsing = false;
        _isMovementPaused = false;
        return this;
    }

    public void Update()
    {
        if (_owner == null || _target == null || _isMovementPaused)
        {
            if (_rigidbody != null) _rigidbody.linearVelocity = Vector2.zero;
            return;
        }

        _timer -= Time.fixedDeltaTime;

        if (_timer <= 0)
        {
            _isPulsing = !_isPulsing;
            _timer = _isPulsing ? _pulseDuration : _pauseDuration;

            // Si on commence une nouvelle impulsion, on calcule la direction et on applique la force initiale.
            if (_isPulsing)
            {
                Vector2 direction = ((Vector2)_target.position - (Vector2)_owner.transform.position).normalized;
                _currentVelocity = direction * _pulseForce;
            }
        }

        // Si on n'est pas en train de pulser, la vélocité est nulle (l'ennemi dérive).
        if (!_isPulsing)
        {
            _currentVelocity = Vector2.zero;
        }

        // Appliquer la décélération pour que la vitesse diminue rapidement pendant l'impulsion.
        // Cela crée l'effet de "burst" qui s'estompe.
        _currentVelocity = Vector2.Lerp(_currentVelocity, Vector2.zero, _decelerationFactor * Time.fixedDeltaTime);
        
        // Calculer le mouvement final en combinant la vélocité de l'impulsion et le knockback.
        Vector2 finalMove = (_currentVelocity * Time.fixedDeltaTime) + _knockback;

        if (_rigidbody != null)
        {
            _rigidbody.MovePosition((Vector2)_owner.transform.position + finalMove);
        }

        // Récupération progressive du knockback
        _knockback = Vector2.Lerp(_knockback, Vector2.zero, KnockbackRecoverySpeed * Time.fixedDeltaTime);
    }


    public void ApplyKnockback(Vector2 force)
    {
        _knockback += force;
    }

    public void Pause()
    {
        _isMovementPaused = true;
        _currentVelocity = Vector2.zero; // Arrêter tout mouvement en cas de pause
    }

    public void Resume()
    {
        _isMovementPaused = false;
        _timer = _pauseDuration; // Réinitialiser le cycle en commençant par une pause
        _isPulsing = false;
    }
}