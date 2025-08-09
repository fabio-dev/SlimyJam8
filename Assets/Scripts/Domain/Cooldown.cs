namespace Assets.Scripts.Domain
{
	public class Cooldown
	{
		private float _lastStartTime = float.MinValue;

		public float Duration { get; private set; }

		public Cooldown(float duration)
		{
			Duration = duration;
		}

		public void Start()
		{
			_lastStartTime = UnityEngine.Time.time;
		}

		public bool IsRunning()
		{
			return ElapsedSeconds() <= Duration;
		}

		public void Stop()
		{
			_lastStartTime = 0.0f;
		}

        public void SetDuration(float duration)
        {
			Duration = duration;
        }

        internal float ElapsedSeconds()
        {
			return UnityEngine.Time.time - _lastStartTime;
        }
    }
}

