using System;

namespace Assets.Scripts.Domain
{
    public class Player : ACharacter
	{
		public event Action OnJumpStart;
		public event Action OnJumpEnd;

		public Ability DashAbility { get; private set; }
		public Ability SplashAbility { get; private set; }	
		public Ability JumpAbility { get; private set; }

		public Player() : base()
		{
			SplashRadius = 1.7f;
			DashRadius = .5f;
			DashDuration = .15f;
			DashSpeedMultiplier = 8f;
			DashZoneInterval = .02f;
			JumpDuration = 1f;

			DashAbility = new Ability(2f);
			SplashAbility = new Ability(8f);
			JumpAbility = new Ability(2f);
		}

		public float SplashRadius { get; private set; }
		public float DashRadius { get; private set; }
		public float DashDuration { get; private set; }
		public float DashSpeedMultiplier { get; private set; }
		public float JumpDuration { get; private set; }
		public float DashMoveSpeed => MoveSpeed * DashSpeedMultiplier;
		public float DashZoneInterval { get; private set; }
		public bool IsDashing { get; private set; }
        public bool IsJumping { get; private set; }

		public void Dash()
		{
			IsDashing = true;
            DashAbility.Cast();
		}

		public void StopDash() => IsDashing = false;

		public void Jump()
		{
			IsJumping = true;
			JumpAbility?.Cast();
			OnJumpStart?.Invoke();
		}

		public void EndJump()
		{
			IsJumping = false;
			OnJumpEnd?.Invoke();
		}

		public void Splash()
		{
			SplashAbility?.Cast();
		}

        internal void DecreaseDashCooldownInPercentage(float baseCooldownReductionInPercentage)
        {
			float amountToReduce = DashAbility.BaseCooldown * baseCooldownReductionInPercentage;
            DashAbility.DecreaseCooldown(amountToReduce);
        }
    }
}
