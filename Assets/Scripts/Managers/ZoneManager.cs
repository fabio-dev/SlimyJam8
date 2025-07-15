using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Instance { get; private set; }

    private List<IZone> zones = new List<IZone>();

    [SerializeField] private GameObject circleZoneVisualPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        AddZone(new CircleZone(Vector2.zero, 1.5f));
    }

    public void AddZone(IZone zone)
    {
        zones.Add(zone);

        if (zone is CircleZone circle)
        {
            CreateVisual(circle.center, circle.radius);
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

        GameObject visual = Instantiate(circleZoneVisualPrefab, center, Quaternion.identity);
        visual.transform.localScale = Vector2.zero;
        visual.transform.DOScale(radius * 2f, .15f);
    }
}
