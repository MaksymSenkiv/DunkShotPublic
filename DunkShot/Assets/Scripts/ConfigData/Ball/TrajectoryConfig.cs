using UnityEngine;

namespace DunkShot
{
    [CreateAssetMenu(fileName = "TrajectoryConfig", menuName = "Configs/Ball/TrajectoryConfig")]
    public class TrajectoryConfig : ScriptableObject
    {
        public float TrajectorySimulationTime;

        public float MinPointScale;
        public float MaxPointScale;
    }
}