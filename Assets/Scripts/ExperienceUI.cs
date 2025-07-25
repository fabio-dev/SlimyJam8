using Assets.Scripts.Domain;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceUI : MonoBehaviour
{
    private LevelManager _levelManager;
    [SerializeField] private Image _image;

    public void Setup(LevelManager levelManager)
    {
        _levelManager = levelManager;
        _levelManager.OnXPGained += OnXPGained;
    }

    private void OnXPGained()
    {
        float fillAmount = (float)_levelManager.CurrentLevelXP / _levelManager.CurrentLevelMaxXP;
        float targetFillAmount = fillAmount;

        if (targetFillAmount == 0f)
        {
            targetFillAmount = 1f;
        }

        _image.DOFillAmount(targetFillAmount, .3f).OnComplete(() => _image.fillAmount = fillAmount);
    }

    private void OnDestroy()
    {
        _levelManager.OnXPGained -= OnXPGained;
    }
}
