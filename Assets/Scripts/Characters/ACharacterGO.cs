using Assets.Scripts.Domain;
using UnityEngine;

public abstract class ACharacterGO : MonoBehaviour
{
	public ACharacter Character { get; private set; }

	public virtual void Setup(ACharacter character)
	{
		Character = character;
		Character.Health.OnDie += OnDie;
	}

	protected virtual void OnDie()
	{
		Character.Health.OnDie -= OnDie;

		// Maybe play a death animation or something like that.
		GameObject.Destroy(gameObject);
	}
}
