using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private AudioClip _music;
    [SerializeField] private SceneTransition _sceneTransition;

    private void Start()
    {
        MusicManager.Instance.ChangeClip(_music);
    }

    public void Play()
    {
        MusicManager.Instance.StopMusic();
        _sceneTransition.HideScreen();
        _sceneTransition.OnHidden += GoToDungeonScene;
    }

    private void GoToDungeonScene()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
