using UnityEngine;

namespace Assets.Scripts.Domain
{
	public abstract class ACharacter
	{
		public ACharacter(float moveSpeed, float basicAttackCooldown, float healtAmount)
		{
			MoveSpeed = moveSpeed;
			BasicAttackCooldown = basicAttackCooldown;
			Health = new HealthComponent(healtAmount);
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
