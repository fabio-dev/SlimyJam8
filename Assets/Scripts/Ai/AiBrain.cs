using Assets.Scripts.Domain;
using UnityEngine;

public class AiBrain : MonoBehaviour
{
    [SerializeField] private AiBrainSettings _settings = null;
    [SerializeField] private EnemyGO _owner = null;

    private IMovementStrategy _movementStrategy = null;
    private IAttackStrategy _attackStrategy = null;
    private bool _isDead = false;
    private bool _isPaused = false;

    public void Pause() => _isPaused = true;
    public void Resume() => _isPaused = false;

    // Modifiez cette ligne pour permettre la modification (setter public)
    public IMovementStrategy MovementStrategy
    {
        get { return _movementStrategy; }
        set { _movementStrategy = value; }
    }

    public void ApplyKnockback(Vector2 force)
    {
        if (_movementStrategy != null)
        {
            _movementStrategy.ApplyKnockback(force / _owner.TotalWeight);
        }
    }

    private void OnDestroy()
    {
        if (_owner != null)
        {
            _owner.OnSetup -= Setup;
        }
    }

    private void FixedUpdate()
    {
        if (_isPaused || _isDead)
        {
            return;
        }
        if (_movementStrategy != null)
        {
            _movementStrategy.Update();
        }

        if (_attackStrategy != null)
        {
            _attackStrategy.Update();
        }
    }

    public void Setup()
    {
        if (_settings == null)
        {
            return;
        }

        PlayerGO player = GameManager.Instance.PlayerGO;
        _attackStrategy = _settings.AttackStrategy.Init(_owner, player);
        _movementStrategy = _settings.MovementStrategy.Init(_owner, player.Center);
        _owner.Enemy.OnDie += OnDie;
    }

    private void OnDie(ACharacter character)
    {
        _isDead = true;
    }
}