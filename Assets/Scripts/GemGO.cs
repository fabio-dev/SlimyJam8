using DG.Tweening;
using System;
using UnityEngine;

public class GemGO : MonoBehaviour
{
    private Sequence _idleAnimation;
    [SerializeField] private int _value;

    public event Action<int> OnCollected;

    void Start()
    {
        _idleAnimation = DOTween.Sequence();
        _idleAnimation.Append(transform.DOScale(1.1f, 1f));
        _idleAnimation.Append(transform.DOScale(.9f, 1f));
        _idleAnimation.Append(transform.DOScale(1f, 1f));
        _idleAnimation.SetLoops(-1);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent(out PlayerGO playerGO))
        {
            CollectGem();
        }
    }

    private void CollectGem()
    {
        SFXPlayer.Instance.PlayLoot();
        _idleAnimation.Kill();
        OnCollected?.Invoke(_value);
        Destroy(gameObject);
    }
}
