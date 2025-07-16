using Assets.Scripts.Domain;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private PlayerGO _playerGO;
	[SerializeField] private EnemyGO _enemyGO;
	[SerializeField] private AbilityUI _dashAbility;
	[SerializeField] private AbilityUI _splashAbility;
	[SerializeField] private AbilityUI _jumpAbility;
	[SerializeField] private PowerUpSpawner _powerUpSpawner;
	[SerializeField] private PowerUpUI _powerUpUiPrefab;
	[SerializeField] private Transform _powerUps;

	private bool _firstUpdate = false;
	public PlayerGO PlayerGO { get { return _playerGO; } }
	public static GameManager Instance { get; private set; }

	private Action _onInitialized;
	public Action OnInitialized
	{
		get { return _onInitialized; }
		set { _onInitialized -= value; _onInitialized += value; }
	}

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
		Player player = new Player();
		_playerGO.SetPlayer(player);

		Enemy enemy = new Enemy();
		_enemyGO.SetEnemy(enemy);

		_dashAbility.SetAbility(player.DashAbility);
		_splashAbility.SetAbility(player.SplashAbility);
		_jumpAbility.SetAbility(player.JumpAbility);

        SelectPowerUp(player);

        _onInitialized?.Invoke();
	}

	private void SelectPowerUp(Player player)
	{
		List<APowerUp> powerUps = _powerUpSpawner.GetRandomPowerUps(3);

		foreach(APowerUp powerUp in powerUps)
		{
			PowerUpUI powerUpUi = Instantiate(_powerUpUiPrefab, _powerUps);
			powerUpUi.SetPowerUp(powerUp);

			powerUpUi.OnSelect += (powerUp) =>
			{
				powerUp.Use(player);
			};
		}
	}
}
