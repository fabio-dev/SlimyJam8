using Sirenix.OdinInspector;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    public static SFXPlayer Instance { get; private set; }

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
    [SerializeField] private AudioClip _feshFalling;
    [BoxGroup("Player")]
    [SerializeField] private AudioClip _feshDashing;
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
    [BoxGroup("Misc")]
    [SerializeField] private AudioClip[] _potDamages;
    [BoxGroup("Misc")]
    [SerializeField] private AudioClip _potBreak;
    [BoxGroup("Misc")]
    [SerializeField] private AudioClip[] _chestDamages;
    [BoxGroup("Misc")]
    [SerializeField] private AudioClip _chestBreak;

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
    public void PlayPlayerFalling() => Play(_feshFalling);
    public void PlayPlayerDashing() => Play(_feshDashing);
    public void PlayPlayerShoot() => PlayAny(_feshShoots);
    public void PlayPlayerJump() => PlayAny(_feshJumps);
    public void PlayPlayerSplash() => PlayAny(_feshFallsplashes);
    public void PlayGameOver() => Play(_gameover);
    public void PlayLoot() => PlayAny(_loots);
    public void PlayPotDamage() => PlayAny(_potDamages);
    public void PlayPotBreak() => Play(_potBreak);
    public void PlayChestDamage() => PlayAny(_chestDamages);
    public void PlayChestBreak() => Play(_chestBreak);

    public void PlayAny(AudioClip[] clip)
    {
        int rng = Random.Range(0, clip.Length);
        Play(clip[rng]);
    }

    public void Play(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
