using Assets.Scripts.Domain;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class ACharacterGO : SerializedMonoBehaviour
{
	[SerializeField] private IZone _deathZone;

	public ACharacter Character { get; private set; }

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
		GameObject.Destroy(gameObject);
	}
}
