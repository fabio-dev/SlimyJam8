using Assets.Scripts.Domain.Collectibles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Domain
{
    public class DropManager : MonoBehaviour
    {
        [SerializeField] private CollectibleGO _collectiblePrefab;
        [SerializeField] private int _gemSDropWeight = 88;
        [SerializeField] private int _gemMDropWeight = 8;
        [SerializeField] private int _gemLDropWeight = 3;
        [SerializeField] private int _heartDropWeight = 1;
        [SerializeField] private int _splashWeaponDropWeight = 0;
        [SerializeField] private int _waveWeaponDropWeight = 0;

        private LevelManager _levelManager;
        private PlayerGO _player;

        private List<DropItem> _drops = new();

        public void Setup(PlayerGO player, LevelManager levelManager)
        {
            _player = player;
            _levelManager = levelManager;
            _drops = new List<DropItem>
            {
                new DropItem(_gemSDropWeight, new Gem(1)),
                new DropItem(_gemMDropWeight, new Gem(5)),
                new DropItem(_gemLDropWeight, new Gem(10)),
                new DropItem(_heartDropWeight, new Heart()),
                new DropItem(_splashWeaponDropWeight, new Weapon(WeaponType.Splash)),
                new DropItem(_waveWeaponDropWeight, new Weapon(WeaponType.Wave)),
            };
        }

        public void Drop(Vector2 position)
        {
            CollectibleGO collectibleGO = Instantiate(_collectiblePrefab, position, Quaternion.identity);
            ACollectible collectible = RandomCollectible();
            collectibleGO.Setup(collectible, _player, _levelManager);
        }

        private ACollectible RandomCollectible()
        {
            int totalWeight = _drops.Sum(d => d.Weight);
            int rng = Random.Range(0, totalWeight);
            int cumulative = 0;

            foreach (DropItem drop in _drops)
            {
                cumulative += drop.Weight;
                if (rng < cumulative)
                {
                    return drop.Collectible;
                }
            }

            return _drops.Last().Collectible;
        }

        private class DropItem
        {
            public int Weight { get; private set; }
            public ACollectible Collectible { get; private set; }

            public DropItem(int weight, ACollectible collectible)
            {
                Weight = weight;
                Collectible = collectible;
            }
        }
    }
}
