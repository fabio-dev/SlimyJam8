using Assets.Scripts.Domain;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public abstract class ACharacterGO : SerializedMonoBehaviour
{
	[SerializeField] private IZone _deathZone;
    [SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private Rigidbody2D _rigidbody;

    public ACharacter Character { get; private set; }
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public Rigidbody2D Rigidbody => _rigidbody;
	public event Action OnSetup;
	public bool IsSetup { get; private set; }

    public virtual void Setup(ACharacter character)
	{
		Character = character;
		Character.Health.OnDie += OnDie;
		IsSetup = true;
    }

	protected virtual void OnDie()
	{
		Character.Health.OnDie -= OnDie;

		ZoneManager.Instance.AddZone(_deathZone);

        // Maybe play a death animation or something like that.
        GameObject.Destroy(gameObject, .3f);
	}

	protected void TriggerOnSetup() => OnSetup?.Invoke();
}
