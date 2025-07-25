using Assets.Scripts.Domain;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public abstract class ACharacterGO : SerializedMonoBehaviour
{
	[SerializeField] private IZone _deathZone;
    [SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private DeathMaskAnimatorController _deathMaskPrefab;

    public ACharacter Character { get; private set; }
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public Rigidbody2D Rigidbody => _rigidbody;
	public event Action OnSetup;
	public bool IsSetup { get; private set; }

    public virtual void Setup(ACharacter character)
	{
		Character = character;
		Character.OnDie += OnDie;
		IsSetup = true;
    }

	protected virtual void OnDie(ACharacter character)
	{
		Character.OnDie -= OnDie;
		ZoneManager.Instance.AddZone(_deathZone);
        DeathMaskAnimatorController deathMask = Instantiate(_deathMaskPrefab, transform.position, Quaternion.identity);
		deathMask.StartIn(.3f);
        Destroy(gameObject, .3f);
	}

	protected void TriggerOnSetup() => OnSetup?.Invoke();
}
