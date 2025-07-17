using System;

namespace Assets.Scripts.Domain
{
	public class Ability
	{
		private Cooldown _cooldown = null;

		public event Action<float> OnCast;

		public Ability(float cooldown)
		{
            BaseCooldown = cooldown;
            _cooldown = new Cooldown(cooldown);
		}

        public float BaseCooldown { get; private set; }

        public float Cooldown { get { return _cooldown.Duration; } }

		public bool CanCast { get { return _cooldown.IsRunning() == false; } }

		public void Cast()
		{
			OnCast?.Invoke(Cooldown);
			_cooldown.Start();
		}

        internal void DecreaseCooldown(float reduction)
        {
            Cooldown = Math.Max(Cooldown - reduction, 0f);
        }
    }
}
