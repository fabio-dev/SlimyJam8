using Assets.Scripts.Domain;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Instance { get; private set; }

    private List<IZone> _zones = new List<IZone>();

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
        _zones.Add(zone);

        if (zone is CircleZone circle)
        {
            SplashGO splash = CreateVisual(circle.GetCenter(), circle._radius);
            circle.OnRemoved += (circle) => CircleZoneRemoved(circle, splash);
            circle.OnReduced += (newRadius) => CircleZoneReduced(newRadius, splash);
        }
    }

    private void CircleZoneRemoved(CircleZone circleZone, SplashGO splash)
    {
        splash.transform.DOScale(0f, .2f).OnComplete(() =>
        {
            DamagePlayerIfNotInAnyZone();
            _zones.Remove(circleZone);
        });
    }

    private void CircleZoneReduced(float newRadius, SplashGO splash)
    {
        splash.transform.DOScale(newRadius * 2f, .2f).OnComplete(() =>
        {
            DamagePlayerIfNotInAnyZone();
        });
    }

    private void DamagePlayerIfNotInAnyZone()
    {
        PlayerGO player = GameManager.Instance.PlayerGO;

        if (player != null && !IsInsideAnyZone(player.transform.position) && player.Player.State != PlayerState.Jumping)
        {
            player.Player.Health.TakeDamage(1f);
            AddZone(new CircleZone(player.transform.position, .8f));
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

    public bool IsInsideAnyZone(Vector2 position)
    {
        foreach (IZone zone in _zones)
        {
            if (zone.Contains(position))
            {
                return true;
            }
        }
        return false;
    }

    public void ReduceZoneAtPosition(Vector2 position, float radiusAbsorption)
    {
        List<IZone> zonesToReduce = _zones.Where(z => z.Contains(position)).ToList();

        if (zonesToReduce.Count > 0)
        {
            SFXPlayer.Instance.PlayFlaskAbsorb();

            foreach (var zone in zonesToReduce)
            {
                zone.Reduce(radiusAbsorption);
            }
        }
    }

    private SplashGO CreateVisual(Vector2 center, float radius)
    {
        if (circleZoneVisualPrefab == null)
        {
            return null;
        }

        SplashGO splash = Instantiate(circleZoneVisualPrefab, center, Quaternion.identity, transform);
        splash.transform.localScale = Vector2.zero;
        splash.transform.DOScale(radius * 2f, .15f)
            .OnComplete(() => splash.DisableDamages());

        GameObject particles = Instantiate(_splashParticles, splash.transform.position, Quaternion.identity);
        Destroy(particles, 1f);

        return splash;
    }
}
