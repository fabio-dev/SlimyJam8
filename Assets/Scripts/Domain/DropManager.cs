using Assets.Scripts.Domain.Collectibles;
using UnityEngine;

namespace Assets.Scripts.Domain
{
    public class DropManager : MonoBehaviour
    {
        [SerializeField] private CollectibleGO _collectiblePrefab;

        private int _gemSDropWeight = 88;
        private int _gemMDropWeight = 8;
        private int _gemLDropWeight = 3;
        private int _heartDropWeight = 1;

        private LevelManager _levelManager;
        private PlayerGO _player;

        private Gem _gemS = new Gem(1);
        private Gem _gemM = new Gem(5);
        private Gem _gemL = new Gem(10);
        private Heart _heart = new Heart();

        public void Setup(PlayerGO player, LevelManager levelManager)
        {
            _player = player;
            _levelManager = levelManager;
        }

        public void Drop(Vector2 position)
        {
            CollectibleGO collectibleGO = Instantiate(_collectiblePrefab, position, Quaternion.identity);
            ACollectible collectible = RandomCollectible();
            collectibleGO.Setup(collectible, _player, _levelManager);
        }

        private ACollectible RandomCollectible()
        {
            int rng = Random.Range(0, _gemLDropWeight + _gemMDropWeight + _gemSDropWeight + _heartDropWeight);

            if (rng < _gemSDropWeight)
            {
                return _gemS;
            }

            if (rng < _gemSDropWeight + _gemMDropWeight)
            {
                return _gemM;
            }

            if (rng < _gemSDropWeight + _gemMDropWeight + _gemLDropWeight)
            {
                return _gemL;
            }

            return _heart;
        }
    }
}
