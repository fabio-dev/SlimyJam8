using UnityEngine;

namespace Assets.Scripts.Domain
{
    [CreateAssetMenu(fileName = "SplashRadiusUpPowerUp", menuName = "ScriptableObjects/PowerUps/Skills/FlaskRain", order = 1)]
    public class FaskRainPowerUp : APowerUp
    {
        public float MinFlaskSize = .5f;
        public float MaxFlaskSize = 2f;
        public float NumberOfFlasks = 10;
        public float FlaskPositionVariance = 5f;
        public float DelayBetweenFlask = .3f;

        public override void Use(Player player)
        {
            for (int i = 0; i < NumberOfFlasks; i++)
            {
                float size = Random.Range(MinFlaskSize, MaxFlaskSize);
                Vector2 playerPosition = GameManager.Instance.PlayerGO.transform.position;

                float rngX = Random.Range(playerPosition.x - FlaskPositionVariance, playerPosition.x + FlaskPositionVariance);
                float rngY = Random.Range(playerPosition.y - FlaskPositionVariance, playerPosition.y + FlaskPositionVariance);

                ZoneManager.Instance.AddZoneDelayed(new CircleZone(new Vector2(rngX, rngY), size), i * DelayBetweenFlask, () =>
                {
                    SFXPlayer.Instance.PlayPlayerSplash();
                });
            }
        }
    }
}
