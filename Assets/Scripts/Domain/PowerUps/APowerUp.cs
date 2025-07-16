using UnityEngine;

namespace Assets.Scripts.Domain
{
    public abstract class APowerUp : ScriptableObject
    {
        [Min(1)]
        public int DropWeight = 1;
        public string Title;
        public string Description;
        public Sprite Sprite;

        public abstract void Use(Player player);
    }
}
