using System;

namespace Assets.Scripts.Domain
{
    public class Ability
    {
        public event Action<float> OnCast;

        public Ability(float cooldown)
        {
            BaseCooldown = cooldown;
            Cooldown = cooldown;
        }

        public float BaseCooldown { get; private set; }
        public float Cooldown { get; private set; }

        public void Cast()
        {
            OnCast?.Invoke(Cooldown);
        }

        internal void DecreaseCooldown(float reduction)
        {
            Cooldown = Math.Max(Cooldown - reduction, 0f);
        }
    }
}
