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
        visual.transform.localScale = new Vector3(radius * 2f, radius * 2f, 1f);
    }
}
