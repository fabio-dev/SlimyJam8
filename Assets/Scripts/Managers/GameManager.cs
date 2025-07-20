using Assets.Scripts.Domain;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private PlayerGO _playerGO;
	[SerializeField] private EnemyGO _enemyGO;
	[SerializeField] private AbilityUI _dashAbility;
	[SerializeField] private AbilityUI _splashAbility;
	[SerializeField] private AbilityUI _jumpAbility;
	[SerializeField] private PowerUpManager _powerUpManager;
	[SerializeField] private CameraFollow _camera;

	private bool _firstUpdate = false;
	private Player _player;

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
		_player = new Player(3.0f, 1.0f, 20.0f);
		_playerGO.Setup(_player);

		Enemy enemy = new Enemy(3.0f, 1.0f, 5.0f);
		_enemyGO.Setup(enemy);

		_dashAbility.SetAbility(_player.DashAbility);
		_splashAbility.SetAbility(_player.SplashAbility);
		_jumpAbility.SetAbility(_player.JumpAbility);

		_powerUpManager.Setup(_player);
		_powerUpManager.OnSelecting += () => Pause();
		_powerUpManager.OnSelected += () => Resume();

		_onInitialized?.Invoke();
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
