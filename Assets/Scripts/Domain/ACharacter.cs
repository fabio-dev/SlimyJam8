using System;
using UnityEngine;

namespace Assets.Scripts.Domain
{
	public abstract class ACharacter
	{
		public ACharacter()
		{
		}

        public event Action<float> OnDamaged;
		public event Action OnDie;

		public void SetHealth(float health)
		{
			Health = new HealthComponent(health);
            Health.OnDamaged += TriggerOnDamaged;
            Health.OnDie += TriggerOnDie;
        }

		public float MoveSpeed { get; private set; }
		public float BasicAttackCooldown { get; set; }
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

        private void TriggerOnDie()
        {
            OnDie?.Invoke();
        }

        private void TriggerOnDamaged(float damage)
        {
            OnDamaged?.Invoke(damage);
        }
    }
}
