using UnityEngine;

namespace DunkShot
{
    [CreateAssetMenu(fileName = "BasketSpawnerConfig", menuName = "Configs/Basket/BasketSpawnerConfig")]
    public class BasketSpawnerConfig : ScriptableObject
    {
        public float MinBasketsDistanceY;
        public float MaxBasketsDistanceY;
        public float MinBasketsDistanceX;
        public int BasketRandomAngleRange;
        public float SecondBasketSpawnDelay;
    }
}