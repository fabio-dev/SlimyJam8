using Assets.Scripts.Domain;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class ACharacterGO : SerializedMonoBehaviour
{
	[SerializeField] private IZone _deathZone;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public ACharacter Character { get; private set; }
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    public virtual void Setup(ACharacter character)
	{
		Character = character;
		Character.Health.OnDie += OnDie;
	}

	protected virtual void OnDie()
	{
		Character.Health.OnDie -= OnDie;

		ZoneManager.Instance.AddZone(_deathZone);

		// Maybe play a death animation or something like that.
		GameObject.Destroy(gameObject, .3f);
	}
}
