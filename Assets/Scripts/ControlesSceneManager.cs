using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlesSceneManager : MonoBehaviour
{
    [SerializeField] private SceneTransition _sceneTransition;

    private void Start()
    {
        _sceneTransition.ShowScreen();
    }

    public void Back()
    {
        _sceneTransition.HideScreen();
        _sceneTransition.OnHidden += GoToMainMenu;
    }

    private void GoToMainMenu()
    {
        _sceneTransition.OnHidden -= GoToMainMenu;
        SceneManager.LoadScene(0);
    }
}
