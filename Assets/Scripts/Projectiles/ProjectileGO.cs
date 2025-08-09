using System;
using System.Collections;
using UnityEngine;

public class ProjectileGO : MonoBehaviour
{
    [SerializeField] protected float _speed = 10f;
    [SerializeField] protected float _lifeTime = 2f;
    [SerializeField] protected bool _killOnCollide = true;
    [SerializeField] protected AudioClip[] _appearSounds;
    [SerializeField] protected float _knockBackStrength = 1f;

    protected Vector3 _direction;
    protected float _damageAmount = 0.0f;
    protected Assets.Scripts.Domain.Cooldown _launchDelay = new Assets.Scripts.Domain.Cooldown(1f);

    public event Action<ProjectileGO> OnCollide;

    public virtual void Launch(Vector3 directionToTaget, float damage, float delay)
    {
        if (_appearSounds != null && _appearSounds.Length > 0)
        {
            SFXPlayer.Instance.PlayAny(_appearSounds);
        }
        else
        {
            SFXPlayer.Instance.PlayPlayerShoot();
        }

        _direction = directionToTaget.normalized;
        _damageAmount = damage;
        _launchDelay.SetDuration(delay);
        _launchDelay.Start();

        StartCoroutine(KillIn(_lifeTime));
    }

    private IEnumerator KillIn(float timeBeforeKill)
    {
        yield return new WaitForSeconds(timeBeforeKill);
        Kill();
    }

    private void FixedUpdate()
    {
        if (_launchDelay.IsRunning())
        {
            return;
        }

        MoveProjectile();
    }

    protected virtual void MoveProjectile()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        CheckTrigger(collider);
    }

    protected bool CheckTrigger(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Wall"))
        {
            Collide();
            return false;
        }

        if (collider.gameObject.TryGetComponent(out PotGO pot))
        {
            pot.Damage();
            Collide();
            return false;
        }

        if (collider.gameObject.TryGetComponent(out ChestGO chest))
        {
            chest.Damage();
            Collide();
            return false;
        }

        if (collider.gameObject.TryGetComponent(out ACharacterGO characterGO))
        {
            characterGO.Character.Health.TakeDamage(_damageAmount);

            if (characterGO.gameObject.TryGetComponent(out AiBrain brain))
            {
                Vector2 knockbackDir = _direction.normalized;
                brain.ApplyKnockback(knockbackDir * _knockBackStrength);
            }

            Collide();
        }

        return true;
    }

    private void Collide()
    {
        OnCollide?.Invoke(this);
        if (_killOnCollide)
        {
            Kill();
        }
    }

    protected virtual void Kill()
    {
        Destroy(gameObject);
    }

    internal void SplashEvery(float secondsBetweenWaves, float splashRadius)
    {
        StartCoroutine(Splash(secondsBetweenWaves, splashRadius));
    }

    private IEnumerator Splash(float secondsBetweenSplashes, float splashRadius)
    {
        while (gameObject != null)
        {
            ZoneManager.Instance.AddZone(new CircleZone(transform.position, splashRadius));
            SFXPlayer.Instance.PlayPlayerSplash();
            yield return new WaitForSeconds(secondsBetweenSplashes);
        }
    }
}