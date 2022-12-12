using UnityEngine;

namespace DunkShot
{
    public class BasketFactory
    {
        public Basket Create(BasketConfig basketConfig, Vector3 position, Transform parent)
        {
            Basket basketInstance = Object.Instantiate(basketConfig.Prefab, position, Quaternion.identity, parent);
            basketInstance.Init(basketConfig.BasketSpawnAnimationConfig);

            return basketInstance;
        }
    }
}