using System;
using UnityEngine;

[Serializable]
public class SpriteAnimationDuration 
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private float _duration = .1f;

    public Sprite Sprite => _sprite;
    public float Duration => _duration;
}