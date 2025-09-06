using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private AudioClip _music;
    [SerializeField] private SceneTransition _sceneTransition;

    private void Start()
    {
        MusicManager.Instance.ChangeClip(_music);
        _sceneTransition.ShowScreen();
    }

    public void Play()
    {
        _sceneTransition.HideScreen();
        MusicManager.Instance.StopMusic();
        _sceneTransition.OnHidden += GoToDungeonScene;
    }

    public void Tutorial()
    {
        _sceneTransition.HideScreen();
        _sceneTransition.OnHidden += GoToTutorialScene;
    }

    private void GoToTutorialScene()
    {
        _sceneTransition.OnHidden -= GoToTutorialScene;
        SceneManager.LoadScene(2);
    }

    private void GoToDungeonScene()
    {
        _sceneTransition.OnHidden -= GoToDungeonScene;
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
