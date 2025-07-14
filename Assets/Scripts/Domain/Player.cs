namespace Assets.Scripts.Domain
{
    public class Player
    {
        public Player()
        {
            MoveSpeed = 3f;
            SplashRadius = 1.7f;
            DashRadius = .5f;
            DashDuration = .15f;
            DashSpeedMultiplier = 8f;
            DashZoneInterval = .02f;
            BasicAttackCooldown = 1f;
        }

        public float MoveSpeed { get; private set; }
        public float SplashRadius { get; private set; }
        public float DashRadius { get; private set; }
        public float DashDuration { get; private set; }
        public float DashSpeedMultiplier { get; private set; }
        public float DashMoveSpeed => MoveSpeed * DashSpeedMultiplier;
        public float DashZoneInterval { get; private set; }
        public float BasicAttackCooldown { get; private set; }
        public bool IsDashing { get; private set; }
        public bool IsMoving { get; private set; }


        public void Move() => IsMoving = true;
        public void StopMove() => IsMoving = false;
        public void Dash() => IsDashing = true;
        public void StopDash() => IsDashing = false;
    }
}
