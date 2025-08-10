using Assets.Scripts.Domain;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class FlaskAbsorber : MonoBehaviour
{
    [SerializeField] private float _radiusAbsorption = 1f;
    [SerializeField, Range(.5f, 10f)] private float _fatDuration = 3f;
    [SerializeField, Range(.1f, 2f)] private float _fatSize = 1.3f;
    [SerializeField] private float _fatWeightMultiplier = 5f;
    [SerializeField] private float _fatFlaskRadius = 1.5f;
    [SerializeField] private float _absorptionStopDurationInSeconds = .5f;
    
    private EnemyGO _enemyGO;
    private Cooldown _absorbCooldown = new Cooldown(3f);
    private Sequence _absorbAnimation;

    private void Start()
    {
        _enemyGO = GetComponent<EnemyGO>();
        _absorbCooldown.Start();

        float scaleDuration = .2f;
        _absorbAnimation = DOTween.Sequence();
        _absorbAnimation.Append(DOTween.To(() => _enemyGO.Scale, x => _enemyGO.SetScale(x), _enemyGO.Scale * _fatSize, scaleDuration));
        _absorbAnimation.AppendInterval(_fatDuration - scaleDuration * 2);
        _absorbAnimation.Append(DOTween.To(() => _enemyGO.Scale, x => _enemyGO.SetScale(x), _enemyGO.BaseScale, scaleDuration));
        _absorbAnimation.OnComplete(() =>
        {
            _enemyGO.WeightMultiplier = 1f;
            _enemyGO.ResetDeathRadius();
        });
        _absorbAnimation.Pause();
        _absorbAnimation.SetAutoKill(false);
    }

    private void Update()
    {
        if (_enemyGO == null || _enemyGO.Enemy.Health.IsDead() || _absorbCooldown.IsRunning())
        {
            return;
        }

        if (ZoneManager.Instance.IsInsideAnyZone(transform.position))
        {
            _absorbAnimation.Restart();
            _absorbAnimation.Play();
            _enemyGO.WeightMultiplier = _fatWeightMultiplier;
            _enemyGO.SetDeathRadius(_fatFlaskRadius);
            _enemyGO.StopMove();
            StartCoroutine(RestartMove(_absorptionStopDurationInSeconds));
            ZoneManager.Instance.ReduceZoneAtPosition(transform.position, _radiusAbsorption);
            _absorbCooldown.Start();
        }
    }

    private IEnumerator RestartMove(float afterSeconds)
    {
        yield return new WaitForSeconds(afterSeconds);
        if (_enemyGO != null)
        {
            _enemyGO.StartMove();
        }
    }
}
