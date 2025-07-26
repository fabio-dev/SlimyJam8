namespace Assets.Scripts.Domain.Collectibles
{
    public class Heart : ACollectible
    {
        public override void Collect(Player player, LevelManager levelManager)
        {
            player.Health.Increase(1);
        }

        public override string[] GetSounds()
        {
            return new string[] { "loot_heart" };
        }

        public override string GetSprite() => "World/heart";
    }
}
