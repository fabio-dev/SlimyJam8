using Assets.Scripts.Domain;
using System;
using UnityEngine;

public class EnemyGO : ACharacterGO
{
    [SerializeField] private AAnimatorController _animatorController;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _health;
    [SerializeField] private float _basicAttackCooldown;
    [SerializeField] private int _score = 10;
    [SerializeField] private int _numberOfDrops = 1;
    [SerializeField] private AiBrain _aiBrain;
    [SerializeField] private float _weight;

    private float _baseScale;
    private float _scale;
    private int _facing = 1;
    private Cooldown _invulnerableCooldown = new Cooldown(.25f);
    private DropManager _dropManager;
    public Enemy Enemy => Character as Enemy;

    public IMovementStrategy MovementStrategy => _aiBrain.MovementStrategy;

    public float Scale => _scale;
    public float BaseScale => _baseScale;
    public float Weight => _weight;
    public float WeightMultiplier { get; set; } = 1f;

    public event Action OnAttack;

    private void Start()
    {
        _baseScale = transform.localScale.x;
        _scale = _baseScale;
    }

    public void SetFacing(int facing)
    {
        transform.localScale = new Vector3(facing * _scale, _scale, 1f);
    }

    public void SetScale(float scale)
    {
        _scale = scale;
        transform.localScale = new Vector3(_facing * _scale, _scale, 1f);
    }

    public float TotalWeight => Weight * WeightMultiplier;

    public override void Setup(ACharacter character)
    {
        base.Setup(character);
        _aiBrain.Setup();
        character.SetHealth(_health);
        character.SetMoveSpeed(_moveSpeed);
        character.SetAttackCooldown(_basicAttackCooldown);
        character.OnDamaged += Damaged;
        ((Enemy)character).Score = _score;
        _animatorController.Setup(this);
        TriggerOnSetup();
    }

    private void Update()
    {
        if (Enemy.Health.IsInvulnerable && !_invulnerableCooldown.IsRunning())
        {
            Enemy.Health.Vulnerable();
            _invulnerableCooldown.Stop();
        }
    }

    private void Damaged(float damaged)
    {
        SFXPlayer.Instance.PlayEnemyHurt();

        Enemy.Health.Invulnerable();
        _invulnerableCooldown.Start();
    }

    protected override void OnDie(ACharacter character)
    {
        if (_dropManager != null)
        {
            if (_numberOfDrops == 1)
            {
                _dropManager.Drop(transform.position);
            }
            else
            {
                for (int i = 0; i < _numberOfDrops; i++)
                {
                    float rngX = UnityEngine.Random.Range(transform.position.x - 1f, transform.position.x + 1f);
                    float rngY = UnityEngine.Random.Range(transform.position.y - 1f, transform.position.y + 1f);
                    Vector2 dropPosition = new Vector2(rngX, rngY);

                    _dropManager.Drop(dropPosition);
                }
            }
        }

        SFXPlayer.Instance.PlayEnemyDie();
        base.OnDie(character);
    }

    public void StopMove()
    {
        MovementStrategy.Pause();
    }

    public void StartMove()
    {
        MovementStrategy.Resume();
    }

    public void SetDropManager(DropManager dropManager)
    {
        _dropManager = dropManager;
    }

    public void TriggerAttack(bool isMelee)
    {
        if (!isMelee)
        {
            SFXPlayer.Instance.PlayEnemyShoot();
        }
        OnAttack?.Invoke();
    }

    private void OnDestroy()
    {
        _animatorController.Kill();
        Enemy.OnDamaged -= Damaged;
    }
}
