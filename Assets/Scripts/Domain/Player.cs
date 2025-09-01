using Assets.Scripts.Domain.Collectibles;
using System;
using UnityEngine;

namespace Assets.Scripts.Domain
{
    public class Player : ACharacter
	{
		private PlayerState _state;
        private bool _isJumping;

        public event Action OnJumpStart;
		public event Action OnJumpEnd;
		public event Action<PlayerState> OnStateChanged;
		public event Action<float> OnShielded;
		public event Action<WeaponType> OnWeaponChanged;

		public Ability DashAbility { get; private set; }
		public Ability SplashAbility { get; private set; }
		public Ability JumpAbility { get; private set; }

        private bool _isSplashing;

        public PlayerState State
		{
			get => _state;
			set
			{
				if (_state == value)
				{
					return;
				}

				if (_isJumping && value != PlayerState.Jumping && value != PlayerState.Splashing)
				{
					return;
				}

				if (_isSplashing && value != PlayerState.Splashing)
				{
					return;
				}

				if (value == PlayerState.Moving && _state != PlayerState.Idle)
				{
					return;
				}

				_state = value;
				OnStateChanged?.Invoke(_state);
			}
		}

		public Player()
		{
			BaseSplashRadius = 1.7f;
			SplashRadius = 1.7f;
            BaseDashRadius = .5f;
			DashRadius = .5f;
            DashDuration = .2f;
			DashSpeedMultiplier = 6f;
			DashZoneInterval = .015f;
			JumpDuration = 1f;
            InvulnerabilityDuration = 1f;
			BaseAttackDamages = 1f;
			AttackDamages = 1f;

            DashAbility = new Ability(2f);
			SplashAbility = new Ability(8f);
			JumpAbility = new Ability(1f);

			State = PlayerState.Idle;
		}

		public float BaseSplashRadius { get; private set; }
        public float SplashRadius { get; private set; }

        public float BaseDashRadius { get; private set; }
		public float DashRadius { get; private set; }

        public float DashDuration { get; private set; }
		public float DashSpeedMultiplier { get; private set; }

		public float JumpDuration { get; private set; }
		public float DashMoveSpeed => MoveSpeed * DashSpeedMultiplier;

		public float DashZoneInterval { get; private set; }
		public float InvulnerabilityDuration { get; private set; }
        public float BaseAttackDamages { get; private set; }
        public float AttackDamages { get; private set; }

        public bool CanMakeAction => State == PlayerState.Idle || State == PlayerState.Moving;
		public bool CanSplash => CanMakeAction || State == PlayerState.Jumping;

        public override void Move(Vector2 lastMove)
		{
			State = PlayerState.Moving;
			base.Move(lastMove);
		}

		public override void StopMove()
		{
			base.StopMove();
			State = PlayerState.Idle;
		}

		public void Dash()
		{
			State = PlayerState.Dashing;
			DashAbility.Cast();
		}

		public void StopDash()
		{
			State = PlayerState.Idle;
		}

		public void Jump()
		{
			_isJumping = true;
            State = PlayerState.Jumping;
			JumpAbility?.Cast();
			OnJumpStart?.Invoke();
		}

		public void EndJump()
		{
			OnJumpEnd?.Invoke();
			_isJumping = false;
            State = PlayerState.Idle;
		}

		public void Splash()
		{
			_isSplashing = true;
            State = PlayerState.Splashing;
			SplashAbility?.Cast();
		}

		public void EndSplash()
		{
            _isSplashing = false;
            State = PlayerState.Idle;
		}

		public void Invulnerable() => Health.Invulnerable();
		public void Vulnerable() => Health.Vulnerable();

        internal void DecreaseDashCooldownInPercentage(float downPercentage)
        {
            float amountToReduce = DashAbility.BaseCooldown * downPercentage;
            DashAbility.DecreaseCooldown(amountToReduce);
        }

        internal void DecreashAttackCooldown(float downPercentage)
        {
            float amountToReduce = BasicAttackCooldown * downPercentage;
			AttackCooldown = Math.Max(.1f, AttackCooldown - amountToReduce);
        }

        internal void IncreaseDamage(float upPercentage)
        {
            float amount = BaseAttackDamages * upPercentage;
            AttackDamages += amount;
        }

        internal void IncreaseDashRadius(float upPercentage)
        {
            float amount = BaseDashRadius * upPercentage;
            DashRadius += amount;
        }

        internal void IncreaseSplashRadius(float upPercentage)
        {
            float amount = BaseSplashRadius * upPercentage;
			SplashRadius += amount;
        }

        internal void Shield(float shieldDuration)
        {
			OnShielded?.Invoke(shieldDuration);
        }

        internal void ChangeWeapon(WeaponType weapon)
        {
			OnWeaponChanged?.Invoke(weapon);
        }
    }
}
