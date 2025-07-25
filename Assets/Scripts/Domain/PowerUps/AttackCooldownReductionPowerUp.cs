using UnityEngine;

namespace Assets.Scripts.Domain
{
    [CreateAssetMenu(fileName = "AttackCooldown", menuName = "ScriptableObjects/PowerUps/Stats/AttackCooldownReduction", order = 1)]
    public class AttackCooldownReductionPowerUp : APowerUp
    {
        [Range(0f, 1f)]
        public float BaseCooldownReductionInPercentage = .1f;

        public override void Use(Player player)
        {
            player.DecreashAttackCooldown(BaseCooldownReductionInPercentage);
        }
    }
}
