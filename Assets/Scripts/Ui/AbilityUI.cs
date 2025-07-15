using Assets.Scripts.Domain;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] private Image _cooldownImage;

    private Ability _ability;

    public void SetAbility(Ability ability)
    {
        if (_ability != null)
        {
            _ability.OnCast -= OnCast;
        }

        _ability = ability;
        _ability.OnCast += OnCast;
    }

    private void OnCast(float cooldown)
    {
        _cooldownImage.fillAmount = 1f;
        _cooldownImage.DOFillAmount(0f, cooldown).SetEase(Ease.Linear);
    }
}
