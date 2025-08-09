using DG.Tweening;
using UnityEngine;

public class MegaRayProjectileGO : ProjectileGO
{
    [SerializeField] private Transform _headSprite;
    [SerializeField] private SpriteRenderer _bodySprite;
    [SerializeField] private Transform _footSprite;

    private float _bodyBaseLength;

    private void Awake()
    {
        _bodyBaseLength = _bodySprite.sprite.rect.width / _bodySprite.sprite.pixelsPerUnit;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckTrigger(collision);
    }

    protected override void MoveProjectile()
    {
        _headSprite.transform.position += _direction * _speed * Time.deltaTime;

        float distance = Vector2.Distance(_footSprite.position, _headSprite.transform.position);

        Vector2 dir = (_headSprite.transform.position - _footSprite.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _bodySprite.transform.rotation = Quaternion.Euler(0, 0, angle);

        _bodySprite.transform.localScale = new Vector3(distance / _bodyBaseLength,
                                                       _bodySprite.transform.localScale.y,
                                                       1f);
    }

    protected override void Kill()
    {
        Sequence killAnimation = DOTween.Sequence();
        killAnimation.Append(_headSprite.transform.DOScaleY(0f, .1f));
        killAnimation.Join(_bodySprite.transform.DOScaleY(0f, .1f));
        killAnimation.Join(_footSprite.transform.DOScaleY(0f, .1f));

        killAnimation.OnComplete(() => base.Kill());
    }
}
