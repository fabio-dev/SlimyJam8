using Assets.Scripts.Domain;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private PlayerGO _playerGO;
	[SerializeField] private EnemySpawner _enemySpawner;
	[SerializeField] private AbilityUI _dashAbility;
	[SerializeField] private AbilityUI _splashAbility;
	[SerializeField] private PowerUpManager _powerUpManager;
	[SerializeField] private CameraFollow _camera;
	[SerializeField] private HealthUI _healthUI;

	private bool _firstUpdate = false;
	private Player _player;

	public PlayerGO PlayerGO { get { return _playerGO; } }
	public static GameManager Instance { get; private set; }

    private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	private void Update()
	{
		if (!_firstUpdate)
		{
			FirstUpdate();
			_firstUpdate = true;
		}
	}

	private void FirstUpdate()
	{
		_player = new Player();
		_player.SetHealth(5f);
		_player.BasicAttackCooldown = .5f;
		_player.SetMoveSpeed(3f);
		_playerGO.Setup(_player);

        _enemySpawner.Setup(_playerGO);

		_healthUI.Setup(_player);

        _dashAbility.SetAbility(_player.DashAbility);
		_splashAbility.SetAbility(_player.SplashAbility);

		_powerUpManager.Setup(_player);
		_powerUpManager.OnSelecting += () => Pause();
		_powerUpManager.OnSelected += () => Resume();
	}

	private void Pause()
	{
		Time.timeScale = 0f;
		_playerGO.Pause();
		_camera.Pause();
	}

	private void Resume()
	{
		Time.timeScale = 1f;
		_playerGO.Resume();
		_camera.Resume();
	}
}
