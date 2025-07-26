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
        public event Action<float> OnHealed;
        public event Action<ACharacter> OnDie;

		public void SetHealth(float health)
		{
			Health = new HealthComponent(health);
            Health.OnDamaged += TriggerOnDamaged;
            Health.OnDie += TriggerOnDie;
            Health.OnHealed += TriggerOnHealed;
        }

        public float MoveSpeed { get; private set; }

        public float AttackCooldown { get; protected set; }
		public float BasicAttackCooldown { get; protected set; }
		public bool IsMoving { get; private set; }
		public Vector2 LastMove { get; private set; }

		public HealthComponent Health { get; private set; }

        internal void SetAttackCooldown(float amount)
        {
			AttackCooldown = amount;
			BasicAttackCooldown = amount;
        }

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
            OnDie?.Invoke(this);
        }

        private void TriggerOnDamaged(float damage)
        {
            OnDamaged?.Invoke(damage);
        }

        private void TriggerOnHealed(float heal)
        {
			OnHealed?.Invoke(heal);
        }
    }
}
