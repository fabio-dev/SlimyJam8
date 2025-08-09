using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class ParentColliderFitter : MonoBehaviour
{
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _foot;

    private CapsuleCollider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider2D>();
    }

    private void LateUpdate()
    {
        float distance = Vector2.Distance(_foot.position, _head.position) + 1f;
        float height = _collider.size.y;
        _collider.size = new Vector2(distance, height);
        _collider.offset = new Vector2(distance / 2f - .5f, _collider.offset.y);
    }
}