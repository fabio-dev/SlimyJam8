using Assets.Scripts.Domain;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField] private PlayerGO _playerGO;
	[SerializeField] private EnemySpawner _enemySpawner;
	[SerializeField] private AbilityUI _dashAbility;
	[SerializeField] private AbilityUI _splashAbility;
	[SerializeField] private PowerUpManager _powerUpManager;
	[SerializeField] private CameraFollow _camera;
	[SerializeField] private HealthUI _healthUI;
	[SerializeField] private DropManager _dropManager;
	[SerializeField] private ExperienceUI _experienceUI;
	[SerializeField] private ScoreManager _scoreManager;
	[SerializeField] private AudioClip _music;
	[SerializeField] private AudioClip _gameOverMusic;
    [SerializeField] private SceneTransition _sceneTransition;
	[SerializeField] private PlayerSpawning _playerSpawning;
	[SerializeField] private GameOverUI _gameOver;
	[SerializeField] private Image _redImage;

	private bool _firstUpdate = false;
	private Player _player;
	private LevelManager _levelManager;
	public PlayerGO PlayerGO { get { return _playerGO; } }
	public static GameManager Instance { get; private set; }

	public void Quit()
	{
		MusicManager.Instance.StopMusic();
		_sceneTransition.HideScreen();
		_sceneTransition.OnHidden += () => SceneManager.LoadScene(0);
	}

	public void Replay()
    {
		MusicManager.Instance.StopMusic();
        _sceneTransition.HideScreen();
        _sceneTransition.OnHidden += () => SceneManager.LoadScene(1);
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
		MusicManager.Instance.ChangeClip(_music);
		_sceneTransition.ShowScreen();
        _sceneTransition.OnShowed += PlayerSpawning;
    }

	private void PlayerSpawning()
	{
		_playerSpawning.OnGroundTouched += () =>
        {
            ZoneManager.Instance.AddZone(new CircleZone(_playerSpawning.transform.position, 2f));
			SFXPlayer.Instance.PlayPlayerSplash();
        };
		_playerSpawning.OnReady += InitAndStart;
		_playerSpawning.RunAnimation();
    }

    private void InitAndStart()
    {
		_playerGO.gameObject.SetActive(true);
        _playerSpawning.gameObject.SetActive(false);

        _player = new Player();
        _player.SetHealth(5f);
        _player.SetAttackCooldown(.5f);
        _player.SetMoveSpeed(3f);
        _player.OnDie += LostGame;

        _levelManager = new LevelManager();
        _dropManager.Setup(_playerGO, _levelManager);
        _experienceUI.Setup(_levelManager);

        _playerGO.Setup(_player);

        _enemySpawner.Setup(_playerGO, _dropManager, _scoreManager);

        _healthUI.Setup(_player);

        _dashAbility.SetAbility(_player.DashAbility);
        _splashAbility.SetAbility(_player.SplashAbility);

        _powerUpManager.Setup(_player, _levelManager);
        _powerUpManager.OnSelecting += () => Pause();
        _powerUpManager.OnSelected += () => Resume();

        _scoreManager.Setup(_player);
        _scoreManager.Run();
    }

    private void LostGame(ACharacter player)
    {
		SFXPlayer.Instance.PlayGameOver();
		MusicManager.Instance.StopInstant();
		StartCoroutine(EasterEgg());
		_gameOver.gameObject.SetActive(true);
		_gameOver.SetScore(_scoreManager.Score);
		_powerUpManager.gameObject.SetActive(false);
    }

	private IEnumerator EasterEgg()
	{
		yield return new WaitForSeconds(8f);
		MusicManager.Instance.ChangeClip(_gameOverMusic);
		_redImage.DOFade(1f, 66f);
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
