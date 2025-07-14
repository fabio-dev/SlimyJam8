namespace Assets.Scripts.Domain
{
	public class HealthComponent
	{
		public HealthComponent(float baseValue)
		{
			BaseValue = baseValue;
			DamageTaken = 0;
		}

		public float BaseValue { get; private set; }
		public float DamageTaken { get; private set; }

		public void TakeDamage(float damage)
		{
			DamageTaken += damage;
		}

		public bool IsDead()
		{
			return BaseValue - DamageTaken <= 0;
		}
	}
}
