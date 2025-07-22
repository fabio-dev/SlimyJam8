using UnityEngine;

public class ProjectileGO : MonoBehaviour
{
	[SerializeField] private float _speed = 10f;
	[SerializeField] private float _lifeTime = 2f;

	private Vector3 _direction;
	private float _damageAmount = 0.0f;

	public void Launch(Vector3 direction, float damageAmount)
	{
		_direction = direction.normalized;
		_damageAmount = damageAmount;
		Destroy(gameObject, _lifeTime);
	}

	private void FixedUpdate()
	{
		transform.position += _direction * _speed * Time.deltaTime;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ACharacterGO characterGO) == false)
        {
            return;
        }

        characterGO.Character.Health.TakeDamage(_damageAmount);
        Destroy(gameObject, 0f);
    }
}