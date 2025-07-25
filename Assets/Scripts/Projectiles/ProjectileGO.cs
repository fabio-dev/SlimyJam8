using Assets.Scripts.Domain;
using UnityEngine;

public class ProjectileGO : MonoBehaviour
{
	[SerializeField] private float _speed = 10f;
	[SerializeField] private float _lifeTime = 2f;

	private Vector3 _direction;
	private float _damageAmount = 0.0f;
    private Cooldown _launchDelay = new Cooldown(1f);

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
        if (collider.gameObject.TryGetComponent(out ACharacterGO characterGO) == false)
        {
            return;
        }

        characterGO.Character.Health.TakeDamage(_damageAmount);

        if (characterGO is EnemyGO)
        {
            SFXPlayer.Instance.PlayEnemyHurt();
        }
        else
        {
            SFXPlayer.Instance.PlayPlayerHurt();
        }

        Destroy(gameObject, 0f);
    }
}