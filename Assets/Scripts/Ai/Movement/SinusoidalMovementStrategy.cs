using UnityEngine;

public class SinusoidalMovementStrategy : IMovementStrategy
{
    private EnemyGO _owner;
    private Transform _target;
    private Rigidbody2D _rigidbody;
    private bool _isMovementPaused;

    // --- PARAMÈTRES POUR LE MOUVEMENT DU SERPENT ---
    // La vitesse de base à laquelle le serpent avance vers le joueur.
    private readonly float _forwardSpeed = 1.5f;

    // La largeur des oscillations. Une valeur plus grande donne des "S" plus larges.
    private readonly float _waveAmplitude = 1.5f;

    // La vitesse des oscillations. Une valeur plus grande donne des "S" plus fréquents.
    private readonly float _waveFrequency = 12f;
    // ------------------------------------------------

    private Vector2 _knockback;
    private const float KnockbackRecoverySpeed = 2f;

    public Transform Target => _target;

    public IMovementStrategy Init(EnemyGO owner, Transform target)
    {
        _owner = owner;
        _target = target;
        _rigidbody = owner.GetComponent<Rigidbody2D>();
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

        // 1. Direction principale vers la cible
        Vector2 directionToTarget = ((Vector2)_target.position - (Vector2)_owner.transform.position).normalized;

        // 2. Direction perpendiculaire pour l'oscillation
        // On obtient un vecteur à 90 degrés pour créer le mouvement de côté.
        Vector2 perpendicularDirection = new Vector2(-directionToTarget.y, directionToTarget.x);

        // 3. Calcul de la force de l'oscillation avec une onde sinusoïdale
        // Time.time assure que l'onde progresse continuellement.
        float waveFactor = Mathf.Sin(Time.time * _waveFrequency) * _waveAmplitude;

        // 4. Combinaison des mouvements
        // Le mouvement final est la somme de l'avancée vers le joueur et de l'oscillation latérale.
        Vector2 finalVelocity = (directionToTarget * _forwardSpeed) + (perpendicularDirection * waveFactor);

        // Appliquer le knockback en plus de la vélocité normale
        finalVelocity += _knockback / Time.fixedDeltaTime;

        if (_rigidbody != null)
        {
            _rigidbody.linearVelocity = finalVelocity;
        }

        // Récupération progressive du knockback
        _knockback = Vector2.Lerp(_knockback, Vector2.zero, KnockbackRecoverySpeed * Time.fixedDeltaTime);
    }

    public void ApplyKnockback(Vector2 force)
    {
        _knockback += force;
        if (_rigidbody != null) _rigidbody.linearVelocity = Vector2.zero;
    }

    public void Pause()
    {
        _isMovementPaused = true;
        if (_rigidbody != null) _rigidbody.linearVelocity = Vector2.zero;
    }

    public void Resume()
    {
        _isMovementPaused = false;
    }
}