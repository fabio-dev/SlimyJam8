using UnityEngine;

namespace Assets.Scripts.Domain
{
    [CreateAssetMenu(fileName = "SpeedUp", menuName = "ScriptableObjects/PowerUps/Stats/SpeedUp", order = 1)]
    public class SpeedUpPowerUp : APowerUp
    {
        [Range(0f, 1f)]
        public float BaseSpeedBonusInPercentage = .15f;

        public override void Use(Player player)
        {
            player.IncreaseMoveSpeed(BaseSpeedBonusInPercentage);
        }
    }
}
