using Assets.Scripts.Domain;
using System;
using UnityEngine;

public class EnemyGO : ACharacterGO
{
    [SerializeField] private IAnimatorController _animatorController;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _health;
    [SerializeField] private float _basicAttackCooldown;
    [SerializeField] private int _score = 10;
    [SerializeField] private int _numberOfDrops = 1;
    [SerializeField] private AiBrain _aiBrain;

    private DropManager _dropManager;
    public Enemy Enemy => Character as Enemy;

    public IMovementStrategy MovementStrategy => _aiBrain.MovementStrategy;

    public event Action OnAttack;

    public override void Setup(ACharacter character)
    {
        base.Setup(character);
        _aiBrain.Setup();
        character.SetHealth(_health);
        character.SetMoveSpeed(_moveSpeed);
        character.SetAttackCooldown(_basicAttackCooldown);
        character.OnDamaged += PlayDamagedSound;
        ((Enemy)character).Score = _score;
        _animatorController.Setup(this);
        TriggerOnSetup();
    }

    private void PlayDamagedSound(float damaged)
    {
        SFXPlayer.Instance.PlayEnemyHurt();
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
        Enemy.OnDamaged -= PlayDamagedSound;
    }
}
