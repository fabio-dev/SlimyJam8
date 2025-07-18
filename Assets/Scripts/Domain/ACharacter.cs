using UnityEngine;

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
		public Vector2 LastMove { get; private set; }

		public HealthComponent Health { get; private set; }

		public virtual void Move(Vector2 lastMove)
        {
			IsMoving = true;
            LastMove = lastMove;
        }

        public virtual void StopMove() => IsMoving = false;

		public void SetMoveSpeed(float speed)
		{
			MoveSpeed = speed;
		}

        public void IncreaseMoveSpeed(float bonusSpeed)
        {
			MoveSpeed += bonusSpeed;
        }
	}
}
