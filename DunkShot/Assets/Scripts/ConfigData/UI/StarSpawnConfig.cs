using DunkShot;
using UnityEngine;

namespace ConfigData.UI
{
    [CreateAssetMenu(fileName = "StarSpawnConfig", menuName = "Configs/UI/StarSpawnConfig", order = 0)]
    public class StarSpawnConfig : ScriptableObject
    {
        public Star StarPrefab;
        public int StartPoolCount;
        
        public float DistanceFromBasketToStar;
        [Range(0, 100)]
        public int SpawnChance;
    }
}