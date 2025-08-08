namespace Assets.Scripts.Domain.Collectibles
{
    public class Weapon : ACollectible
    {
        public WeaponType WeaponType { get; private set; }
        
        public Weapon(WeaponType type)
        {
            WeaponType = type;
        }

        public override void Collect(Player player, LevelManager levelManager)
        {
            player.ChangeWeapon(WeaponType);
        }

        public override string[] GetSounds()
        {
            return new string[] { "loot_gem", "loot_gem2", "loot_gem3" };
        }

        public override string GetSprite()
        {
            return $"World/weapon_collectible_{WeaponType.ToString().ToLowerInvariant()}";
        }
    }

    public enum WeaponType
    {
        Basic,
        Splash,
        Wave,
    }
}
