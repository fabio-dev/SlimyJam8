using UnityEngine;

public class CircleZone : IZone
{
    public Vector2 center;
    public float radius;

    public CircleZone(Vector2 center, float radius)
    {
        this.center = center;
        this.radius = radius;
    }

    public bool Contains(Vector2 point)
    {
        return Vector2.Distance(point, center) <= radius;
    }
}
