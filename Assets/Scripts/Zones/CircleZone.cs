using UnityEngine;

public class CircleZone : IZone
{
	public Transform centerTransform;
	public float radius;

	private Vector2 _center;

	// Needed for the Odin serialization.
	public CircleZone()
	{
	}

	public CircleZone(Vector2 center, float radius)
	{
		this._center = center;
		this.radius = radius;
	}

	public CircleZone(Transform center, float radius)
	{
		this.centerTransform = center;
		this.radius = radius;
	}

	public Vector2 GetCenter()
	{
		if (centerTransform == null)
		{
			return _center;
		}

		_center = centerTransform.position;
		return _center;
	}

	public bool Contains(Vector2 point)
	{
		return Vector2.Distance(point, GetCenter()) <= radius + .1f;
	}
}
