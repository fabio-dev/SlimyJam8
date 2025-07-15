using System;

namespace Assets.Scripts.Domain
{
    public class Ability
    {
        public event Action<float> OnCast;

        public Ability(float cooldown)
        {
            Cooldown = cooldown;
        }

        public float Cooldown { get; private set; }

        public void Cast()
        {
            OnCast?.Invoke(Cooldown);
        }
    }
}
