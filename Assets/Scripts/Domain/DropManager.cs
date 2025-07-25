using UnityEngine;

namespace Assets.Scripts.Domain
{
    public class DropManager : MonoBehaviour
    {
        [SerializeField] private GemGO _gemSPrefab;
        [SerializeField] private GemGO _gemMPrefab;
        [SerializeField] private GemGO _gemLPrefab;

        private int _gemSDropWeight = 44;
        private int _gemMDropWeight = 5;
        private int _gemLDropWeight = 1;
        private LevelManager _levelManager;

        public void Setup(LevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        public void Drop(Vector2 position)
        {
            GemGO prefab = RandomGemPrefab();
            GemGO gem = Instantiate(prefab, position, Quaternion.identity);
            gem.OnCollected += GainXP;
        }

        private void GainXP(int xp)
        {
            _levelManager.AddXP(xp);
        }

        private GemGO RandomGemPrefab()
        {
            int rng = Random.Range(0, _gemLDropWeight + _gemMDropWeight + _gemSDropWeight);

            if (rng < _gemSDropWeight)
            {
                return _gemSPrefab;
            }

            if (rng < _gemSDropWeight + _gemMDropWeight)
            {
                return _gemMPrefab;
            }

            return _gemLPrefab;
        }
    }
}
