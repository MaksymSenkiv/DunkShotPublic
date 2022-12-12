using UnityEngine;

namespace DunkShot
{
    [CreateAssetMenu(fileName = "BallShooterConfig", menuName = "Configs/Ball/BallShooterConfig")]
    public class BallShooterConfig : ScriptableObject
    {
        public float VelocityInputMultiplier;
        
        public float MinShotVelocity;
        public float MaxShotVelocity;

        public TrajectoryConfig TrajectoryConfig;
    }
}