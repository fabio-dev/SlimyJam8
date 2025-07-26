using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    public void SetScore(int score)
    {
        _scoreText.text = $"Score : {score}";
    }
}
