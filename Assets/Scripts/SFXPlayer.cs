using Sirenix.OdinInspector;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public static SFXPlayer Instance { get; private set; }

    private Camera _camera;

    [BoxGroup("Enemy")]
    [SerializeField] private AudioClip _enemyDie;
    [BoxGroup("Enemy")]
    [SerializeField] private AudioClip _enemyHurt;
    [BoxGroup("Enemy")]
    [SerializeField] private AudioClip _enemyShoot;

    [BoxGroup("Player")]
    [SerializeField] private AudioClip _feshHurt;
    [BoxGroup("Player")]
    [SerializeField] private AudioClip _feshFallground;
    [BoxGroup("Player")]
    [SerializeField] private AudioClip _feshLevelUp;
    [BoxGroup("Player")]
    [SerializeField] private AudioClip[] _feshShoots;
    [BoxGroup("Player")]
    [SerializeField] private AudioClip[] _feshJumps;
    [BoxGroup("Player")]
    [SerializeField] private AudioClip[] _feshFallsplashes;

    [BoxGroup("Misc")]
    [SerializeField] private AudioClip _gameover;
    [BoxGroup("Misc")]
    [SerializeField] private AudioClip[] _loots;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayEnemyDie() => Play(_enemyDie);
    public void PlayEnemyHurt() => Play(_enemyHurt);
    public void PlayEnemyShoot() => Play(_enemyShoot);
    public void PlayPlayerHurt() => Play(_feshHurt);
    public void PlayPlayerFallground() => Play(_feshFallground);
    public void PlayPlayerLevelUp() => Play(_feshLevelUp);
    public void PlayPlayerShoot() => PlayAny(_feshShoots);
    public void PlayPlayerJump() => PlayAny(_feshJumps);
    public void PlayPlayerSplash() => PlayAny(_feshFallsplashes);
    public void PlayGameover() => Play(_gameover);
    public void PlayLoot() => PlayAny(_loots);

    public void PlayAny(AudioClip[] clip)
    {
        int rng = Random.Range(0, clip.Length);
        Play(clip[rng]);
    }

    public void Play(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, CameraPosition);
    }

    private Vector3 CameraPosition
    {
        get
        {
            if (_camera == null)
            {
                _camera = FindFirstObjectByType<Camera>();
            }

            return _camera?.transform?.position ?? Vector3.zero;
        }
    }
}
