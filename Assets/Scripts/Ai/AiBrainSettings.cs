using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "AiBrainSettings", menuName = "ScriptableObjects/AiBrainSettings", order = 1)]
public class AiBrainSettings : SerializedScriptableObject
{
	[SerializeField] private IAttackStrategy _attackStrategy;
	[SerializeField] private IMovementStrategy _movementStrategy;

	public IMovementStrategy MovementStrategy { get { return _movementStrategy; } }
	public IAttackStrategy AttackStrategy { get { return _attackStrategy; } }
}
