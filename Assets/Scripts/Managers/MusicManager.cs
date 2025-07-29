using UnityEngine;
using DG.Tweening;
using System;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private AudioSource _audioSource;

    [Header("Musique"), SerializeField] private AudioClip _musicClip;
    [Range(0f, 1f), SerializeField] private float _volume = 0.5f;
    [SerializeField] private bool _loop = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = _musicClip;
        _audioSource.volume = _volume;
        _audioSource.loop = _loop;
        _audioSource.playOnAwake = false;
    }

    public void ChangeClip(AudioClip clip)
    {
        if (_audioSource.clip == clip)
        {
            return;
        }

        StopMusic(() =>
        {
            _audioSource.clip = clip;
            _audioSource.volume = _volume;
            PlayMusic();
        });
    }

    public void PlayMusic()
    {
        if (_audioSource != null && !_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

    public void StopInstant()
    {
        _audioSource.Stop();
    }

    public void StopMusic(Action onComplete = null)
    {
        if (_audioSource != null && _audioSource.isPlaying)
        {
            _audioSource.DOFade(0f, 1f).OnComplete(() =>
            {
                _audioSource.Stop();
                onComplete?.Invoke();
            });
        }
        else
        {
            onComplete?.Invoke();
        }
    }

    public void SetVolume(float newVolume)
    {
        if (_audioSource != null)
        {
            _audioSource.volume = Mathf.Clamp01(newVolume);
        }
    }
}