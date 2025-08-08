using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class ZoneManager : MonoBehaviour
{
	public static ZoneManager Instance { get; private set; }

	private List<IZone> zones = new List<IZone>();

	[SerializeField] private SplashGO circleZoneVisualPrefab;
	[SerializeField] private GameObject _splashParticles;
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

	public void AddZoneDelayed(IZone zone, float delay, Action onAdding)
	{
		StartCoroutine(AddZoneDelayedInternal(zone, delay, onAdding));
	}

    private IEnumerator AddZoneDelayedInternal(IZone zone, float delay, Action onAdding)
    {
		yield return new WaitForSeconds(delay);
		AddZone(zone);
		onAdding?.Invoke();
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

		SplashGO splash = Instantiate(circleZoneVisualPrefab, center, Quaternion.identity, transform);
		splash.transform.localScale = Vector2.zero;
		splash.transform.DOScale(radius * 2f, .15f)
			.OnComplete(() => splash.DisableDamages());

        GameObject particles = Instantiate(_splashParticles, splash.transform.position, Quaternion.identity);
        Destroy(particles, 1f);
    }
}
