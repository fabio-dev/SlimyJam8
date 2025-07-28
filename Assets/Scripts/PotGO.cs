using Assets.Scripts.Domain;
using System;
using UnityEngine;

public class PotGO : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private GameObject _breakMask;
    [SerializeField] private int _health;
    [SerializeField] private Sprite[] _damageSprites;

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
            SFXPlayer.Instance.PlayPotDamage();
        }
    }

    internal void SetDropManager(DropManager potDropManager)
    {
        _dropManager = potDropManager;
    }

    private void Break()
    {
        SFXPlayer.Instance.PlayPotDamage();
        Instantiate(_breakMask, transform.position, Quaternion.identity);
        GetComponent<Collider2D>().enabled = false;
        Drop();
    }

    private void Drop()
    {
        if (_dropManager != null)
        {
            _dropManager.Drop(transform.position);
        }
    }

    private void SetNextSprite()
    {
        int index = Math.Min(_damages, _damageSprites.Length - 1);
        _sprite.sprite = _damageSprites[index];
    }
}
