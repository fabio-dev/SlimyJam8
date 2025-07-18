using UnityEngine;

public class ProjectileGO : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifeTime = 2f;

    private Vector3 _direction;

    public void Launch(Vector3 direction)
    {
        _direction = direction.normalized;
        Destroy(gameObject, _lifeTime);
    }

    private void FixedUpdate()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }

    private void OnDestroy()
    {
        SpriteAnimator animator = GetComponent<SpriteAnimator>();
        if (animator != null)
        {
            animator.Kill();
        }
    }
}