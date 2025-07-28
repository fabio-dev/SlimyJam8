using Assets.Scripts.Domain.PowerUps;
using UnityEngine;

namespace Assets.Scripts.Domain
{
    public abstract class APowerUp : ScriptableObject
    {
        [Min(1)]
        public int DropWeight = 1;
        public string Title;
        public PowerUpType PowerUpType;

        public abstract void Use(Player player);
    }
}
