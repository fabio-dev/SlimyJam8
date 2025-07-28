using UnityEngine;

namespace Assets.Scripts.Domain
{
    [CreateAssetMenu(fileName = "SplashRadiusUpPowerUp", menuName = "ScriptableObjects/PowerUps/Skills/Shield", order = 1)]
    public class ShieldPowerUp : APowerUp
    {
        public float ShieldDuration = 10f;

        public override void Use(Player player)
        {
            player.Shield(ShieldDuration);
        }
    }
}
