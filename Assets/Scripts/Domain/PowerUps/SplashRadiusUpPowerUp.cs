using UnityEngine;

namespace Assets.Scripts.Domain
{
    [CreateAssetMenu(fileName = "SplashRadiusUpPowerUp", menuName = "ScriptableObjects/PowerUps/Abilities/SplashRadiusUp", order = 1)]
    public class SplashRadiusUpPowerUp : APowerUp
    {
        [Range(0f, 1f)]
        public float BaseRadiusUpPercentage = .2f;

        public override void Use(Player player)
        {
            player.IncreaseSplashRadius(BaseRadiusUpPercentage);
        }
    }
}
