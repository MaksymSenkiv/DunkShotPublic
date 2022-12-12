using UnityEngine;

namespace DunkShot
{
    [CreateAssetMenu(fileName = "BasketConfig", menuName = "Configs/Basket/BasketConfig")]
    public class BasketConfig : ScriptableObject
    {
        public Basket Prefab;
        public float BasketWidth;
        public BasketSpawnAnimationConfig BasketSpawnAnimationConfig;
    }
}