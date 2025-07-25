using UnityEngine;

namespace Assets.Scripts.Domain
{
    [CreateAssetMenu(fileName = "DamageUp", menuName = "ScriptableObjects/PowerUps/Stats/DamageUp", order = 1)]
    public class DamageUpPowerUp : APowerUp
    {
        [Range(0f, 1f)]
        public float BaseDamageUpPercentage = .25f;

        public override void Use(Player player)
        {
            player.IncreaseDamage(BaseDamageUpPercentage);
        }
    }
}
