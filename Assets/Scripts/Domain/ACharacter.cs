namespace Assets.Scripts.Domain
{
	public abstract class ACharacter
	{
		// TODO set the health from this constructor.
		public ACharacter()
		{
			MoveSpeed = 3f;
			BasicAttackCooldown = 1f;
			Health = new HealthComponent(5.0f);
		}

		public float MoveSpeed { get; private set; }
		public float BasicAttackCooldown { get; private set; }
		public bool IsMoving { get; private set; }

		public HealthComponent Health { get; private set; }

		public void SetMoveSpeed(float speed)
		{
			MoveSpeed = speed;
		}

		public void Move() => IsMoving = true;
		public void StopMove() => IsMoving = false;
	}
}
