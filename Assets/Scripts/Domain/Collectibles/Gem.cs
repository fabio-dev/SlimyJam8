namespace Assets.Scripts.Domain.Collectibles
{
    public class Gem : ACollectible
    {
        public Gem(int value)
        {
            Value = value;
        }

        public int Value { get; private set; }

        public override void Collect(Player player, LevelManager levelManager)
        {
            levelManager.AddXP(Value);
        }

        public override string[] GetSounds()
        {
            return new string[] { "loot_gem", "loot_gem2", "loot_gem3" };
        }

        public override string GetSprite()
        {
            if (Value == 1)
            {
                return "World/gem-s";
            }
            if (Value == 5)
            {
                return "World/gem-m";
            }
            return "World/gem-l";
        }
    }
}
