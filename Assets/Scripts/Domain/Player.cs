﻿using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Domain
{
    public class Player : ACharacter
	{
		private PlayerState _state;

		public event Action OnJumpStart;
		public event Action OnJumpEnd;
		public event Action<PlayerState> OnStateChanged;

		public Ability DashAbility { get; private set; }
		public Ability SplashAbility { get; private set; }
		public Ability JumpAbility { get; private set; }
		public PlayerState State
		{
			get => _state;
			set
			{
				if (_state == value)
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

		public Player() : base()
		{
			SplashRadius = 1.7f;
			DashRadius = .5f;
			DashDuration = .15f;
			DashSpeedMultiplier = 8f;
			DashZoneInterval = .02f;
			JumpDuration = 1f;

			DashAbility = new Ability(2f);
			SplashAbility = new Ability(1f);
			JumpAbility = new Ability(2f);
			State = PlayerState.Idle;
		}

		public float SplashRadius { get; private set; }
		public float DashRadius { get; private set; }
		public float DashDuration { get; private set; }
		public float DashSpeedMultiplier { get; private set; }
		public float JumpDuration { get; private set; }
		public float DashMoveSpeed => MoveSpeed * DashSpeedMultiplier;
		public float DashZoneInterval { get; private set; }

		public bool CanMakeAction => State == PlayerState.Idle || State == PlayerState.Moving;

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
            State = PlayerState.Jumping;
			JumpAbility?.Cast();
			OnJumpStart?.Invoke();
		}

		public void EndJump()
		{
			OnJumpEnd?.Invoke();
			State = PlayerState.Idle;
		}

		public void Splash()
		{
			State = PlayerState.Splashing;
			SplashAbility?.Cast();
		}

		public void EndSplash()
		{
			State= PlayerState.Idle;
		}

        internal void DecreaseDashCooldownInPercentage(float baseCooldownReductionInPercentage)
        {
			float amountToReduce = DashAbility.BaseCooldown * baseCooldownReductionInPercentage;
            DashAbility.DecreaseCooldown(amountToReduce);
        }
    }
}
