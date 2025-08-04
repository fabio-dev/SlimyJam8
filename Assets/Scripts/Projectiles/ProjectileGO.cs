using Assets.Scripts.Domain;
using System;
using UnityEngine;

public class ProjectileGO : MonoBehaviour
{
	[SerializeField] private float _speed = 10f;
	[SerializeField] private float _lifeTime = 2f;

	private Vector3 _direction;
	private float _damageAmount = 0.0f;
    private Cooldown _launchDelay = new Cooldown(1f);

    public event Action<ProjectileGO> OnCollide;

    internal void Launch(Vector3 directionToTaget, float damage, float delay)
    {
        _direction = directionToTaget.normalized;
        _damageAmount = damage;
        Destroy(gameObject, _lifeTime);
        _launchDelay.SetDuration(delay);
        _launchDelay.Start();
    }

    private void FixedUpdate()
	{
        if (_launchDelay.IsRunning())
        {
            return;
        }
		transform.position += _direction * _speed * Time.deltaTime;
	}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Wall"))
        {
            Kill();
            return;
        }

        if (collider.gameObject.TryGetComponent(out PotGO pot))
        {
            pot.Damage();
            Kill();
            return;
        }

        if (collider.gameObject.TryGetComponent(out ChestGO chest))
        {
            chest.Damage();
            Kill();
            return;
        }

        if (!collider.gameObject.TryGetComponent(out ACharacterGO characterGO))
        {
            return;
        }

        characterGO.Character.Health.TakeDamage(_damageAmount);
        if (characterGO.gameObject.TryGetComponent(out AiBrain brain))
        {
            Kill();
            Vector2 knockbackDir = _direction.normalized;
            brain.ApplyKnockback(knockbackDir);
        }
    }

    private void Kill()
    {
        OnCollide?.Invoke(this);
        Destroy(gameObject);
    }
}