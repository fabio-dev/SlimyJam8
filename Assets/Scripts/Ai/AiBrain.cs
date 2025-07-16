using UnityEngine;

public class AiBrain : MonoBehaviour
{
	[SerializeField] private AiBrainSettings _settings = null;
	[SerializeField] private EnemyGO _owner = null;

	private IMovementStrategy _movementStrategy = null;
	private IAttackStrategy _attackStrategy = null;

	private void Start()
	{
		GameManager.Instance.OnInitialized += OnGameInitialized;
	}

	private void OnDisable()
	{
		GameManager.Instance.OnInitialized -= OnGameInitialized;
	}

	private void Update()
	{
		if (_movementStrategy != null)
		{
			_movementStrategy.Update();
		}

		if (_attackStrategy != null)
		{
			_attackStrategy.Update();
		}
	}

	private void OnGameInitialized()
	{

		if (_settings == null)
		{
			return;
		}

		PlayerGO player = GameManager.Instance.PlayerGO;
		Transform targetTransform = player.transform;
		_attackStrategy = _settings.AttackStrategy.Init(_owner, player);
		_movementStrategy = _settings.MovementStrategy.Init(_owner, targetTransform);
	}
}
