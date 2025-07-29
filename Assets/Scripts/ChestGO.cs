using Assets.Scripts.Domain;
using System;
using UnityEngine;

public class ChestGO : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private GameObject _breakMask;
    [SerializeField] private int _health;
    [SerializeField] private Sprite[] _damageSprites;
    [SerializeField] private int _minLoot = 3;
    [SerializeField] private int _maxLoot = 6;

    private DropManager _dropManager;
    private int _damages;

    public void Damage()
    {
        SetNextSprite();
        _damages++;

        if (_damages == _health)
        {
            Break();
        }
        else
        {
            SFXPlayer.Instance.PlayChestDamage();
        }
    }

    internal void SetDropManager(DropManager potDropManager)
    {
        _dropManager = potDropManager;
    }

    private void Break()
    {
        SFXPlayer.Instance.PlayChestBreak();
        GetComponent<Collider2D>().enabled = false;
        Drop();
    }

    private void Drop()
    {
        if (_dropManager == null)
        {
            return;
        }

        int rngDrop = UnityEngine.Random.Range(_minLoot, _maxLoot + 1);

        for (int i = 0; i < rngDrop; i++)
        {
            float rngX = transform.position.x + UnityEngine.Random.Range(-1f, 1f);
            Vector2 dropPosition = new Vector2(rngX, transform.position.y);
            Instantiate(_breakMask, dropPosition, Quaternion.identity);
            _dropManager.Drop(dropPosition);
        }
    }

    private void SetNextSprite()
    {
        int index = Math.Min(_damages, _damageSprites.Length - 1);
        _sprite.sprite = _damageSprites[index];
    }
}
