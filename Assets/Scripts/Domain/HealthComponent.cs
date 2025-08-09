using System;

namespace Assets.Scripts.Domain
{
    public class HealthComponent
	{
		private bool _invulnerable;

		public HealthComponent(float baseValue)
		{
			BaseValue = baseValue;
			DamageTaken = 0;
		}

		public event Action<float> OnDamaged;
		public event Action<float> OnHealed;
        public event Action OnDie;

		public float BaseValue { get; private set; }
		public float DamageTaken { get; private set; }

        public void TakeDamage(float damage)
		{
			if (_invulnerable)
			{
				return;
			}

			if (IsDead())
			{
				return;
			}

			DamageTaken += damage;
			OnDamaged?.Invoke(damage);

			if (IsDead())
			{
				OnDie?.Invoke();
			}
		}

		public bool IsDead()
		{
			return BaseValue - DamageTaken <= 0;
		}

		internal bool IsInvulnerable => _invulnerable;
        internal void Invulnerable() => _invulnerable = true;
		internal void Vulnerable() => _invulnerable = false;

        internal void Increase(float value)
        {
			if (DamageTaken > 0)
			{
				DamageTaken -= value;
				OnHealed?.Invoke(value);
			}
        }
    }
}
