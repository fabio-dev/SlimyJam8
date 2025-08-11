namespace Assets.Scripts.Domain.Collectibles
{
    public abstract class ACollectible
    {
        public abstract void Collect(Player player, LevelManager levelManager);

        public abstract string[] GetSounds();

        public virtual string GetSprite()
        {
            return null;
        }

        public virtual string GetSpriteAnimations()
        {
            return null;
        }
    }
}