using UnityEngine;

namespace Assets.Scripts.Domain
{
    [CreateAssetMenu(fileName = "DashCooldown", menuName = "ScriptableObjects/PowerUps/Abilities/DashCooldownReduction", order = 1)]
    public class DashCooldownReductionPowerUp : APowerUp
    {
        [Range(0f, 1f)]
        public float BaseCooldownReductionInPercentage = .1f;

        public override void Use(Player player)
        {
            player.DecreaseDashCooldownInPercentage(BaseCooldownReductionInPercentage);
        }
    }
}
