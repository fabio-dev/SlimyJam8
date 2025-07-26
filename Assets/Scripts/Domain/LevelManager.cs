using System;

namespace Assets.Scripts.Domain
{
    public class LevelManager
    {
        public int CurrentLevel { get; private set; }
        public int CurrentLevelXP { get; private set; }
        public int CurrentLevelMaxXP { get; private set; }
        public int TotalXP { get; private set; }
        public int XPStepByLevel { get; private set; }

        public event Action OnXPGained;
        public event Action OnLevelUp;

        public LevelManager()
        {
            CurrentLevel = 1;
            XPStepByLevel = 2;
            CurrentLevelMaxXP = XPStepByLevel;
        }

        public void AddXP(int xp)
        {
            TotalXP += xp;
            CurrentLevelXP += xp;

            if (CurrentLevelXP >= CurrentLevelMaxXP)
            {
                LevelUp();
            }

            OnXPGained?.Invoke();
        }

        private void LevelUp()
        {
            CurrentLevel++;
            CurrentLevelMaxXP += XPStepByLevel;
            CurrentLevelXP = 0;
            OnLevelUp?.Invoke();
            SFXPlayer.Instance.PlayPlayerLevelUp();
        }
    }
}
