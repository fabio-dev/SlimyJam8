using System;
using UnityEngine;

public class CircleZone : IZone
{
    public Transform _centerTransform;
    public float _radius;
    public event Action<CircleZone> OnRemoved;
    public event Action<float> OnReduced;

    private Vector2 _center;

    public CircleZone()
    {
    }

    public CircleZone(Vector2 center, float radius)
    {
        this._center = center;
        this._radius = radius;
    }

    public CircleZone(Transform center, float radius)
    {
        this._centerTransform = center;
        this._radius = radius;
    }

    public Vector2 GetCenter()
    {
        if (_centerTransform == null)
        {
            return _center;
        }

        _center = _centerTransform.position;
        return _center;
    }

    public bool Contains(Vector2 point)
    {
        return Vector2.Distance(point, GetCenter()) <= _radius + .1f;
    }

    public void Remove()
    {
        OnRemoved?.Invoke(this);
    }

    public void Reduce(float radiusAbsorption)
    {
        _radius -= radiusAbsorption;

        if (_radius <= 0)
        {
            Remove();
        }
        else
        {
            OnReduced?.Invoke(_radius);
        }
    }
}
