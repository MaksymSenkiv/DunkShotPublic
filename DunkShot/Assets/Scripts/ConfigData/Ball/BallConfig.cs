using UnityEngine;

namespace DunkShot
{
    [CreateAssetMenu(fileName = "BallConfig", menuName = "Configs/Ball/BallConfig")]
    public class BallConfig : ScriptableObject
    {
        public BallShooterConfig BallShooterConfig;
        
        public Ball Prefab;
        public float MoveToBasketCenterDuration;
        
        public float FlyingRotationSpeed;
    }
}