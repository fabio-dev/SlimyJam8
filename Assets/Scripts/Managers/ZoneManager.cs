using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
	public static ZoneManager Instance { get; private set; }

	private List<IZone> zones = new List<IZone>();

	[SerializeField] private SplashGO circleZoneVisualPrefab;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
	}

	public void AddZone(IZone zone)
	{
		zones.Add(zone);

		if (zone is CircleZone circle)
		{
			CreateVisual(circle.GetCenter(), circle.radius);
		}
	}

	public bool IsInsideAnyZone(Vector2 point)
	{
		foreach (IZone zone in zones)
		{
			if (zone.Contains(point))
			{
				return true;
			}
		}
		return false;
	}

	private void CreateVisual(Vector2 center, float radius)
	{
		if (circleZoneVisualPrefab == null)
		{
			return;
		}

		SplashGO splash = Instantiate(circleZoneVisualPrefab, center, Quaternion.identity);
		splash.transform.localScale = Vector2.zero;
		splash.transform.DOScale(radius * 2f, .15f)
			.OnComplete(() => splash.DisableDamages());
	}
}
