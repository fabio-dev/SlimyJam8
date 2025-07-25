using UnityEngine;

namespace Assets.Scripts.Domain
{
    [CreateAssetMenu(fileName = "DashRadiusUpPowerUp", menuName = "ScriptableObjects/PowerUps/Abilities/DashRadiusUp", order = 1)]
    public class DashRadiusUpPowerUp : APowerUp
    {
        [Range(0f, 1f)]
        public float BaseRadiusUpPercentage = .2f;

        public override void Use(Player player)
        {
            player.IncreaseDashRadius(BaseRadiusUpPercentage);
        }
    }
}
