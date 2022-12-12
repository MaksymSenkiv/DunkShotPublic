using ConfigData.UI;
using Redcode.Pools;
using UnityEngine;
using Random = System.Random;

namespace DunkShot
{
    public class StarSpawner : MonoBehaviour
    {
        private static readonly Random _random = new Random();
        
        [SerializeField] private StarSpawnConfig _starSpawnConfig;

        private BasketsSpawner _basketsSpawner;
        private Pool<Star> _pool;

        public void Init(BasketsSpawner basketsSpawner)
        {
            _basketsSpawner = basketsSpawner;
            _basketsSpawner.BasketSpawned += SpawnStarWithChance;
        }

        private void Awake()
        {
            CreatePool();
        }

        private void OnDestroy()
        {
            _basketsSpawner.BasketSpawned -= SpawnStarWithChance;
        }

        public void ReturnToPool(Star starClone)
        {
            _pool.Take(starClone);
        }
        
        private void CreatePool()
        {
            _pool = Pool.Create(_starSpawnConfig.StarPrefab, OnCreate, _starSpawnConfig.StartPoolCount, transform);
        }

        private Star OnCreate()
        {
            Star starClone = Instantiate(_starSpawnConfig.StarPrefab, transform);
            starClone.Init(this);

            return starClone;
        }
        
        private void SpawnStarWithChance(Transform basketTransform)
        {
            if (_starSpawnConfig.SpawnChance > _random.Next(0, 100))
            {
                Star startInstance = _pool.Get();
                startInstance.transform.position = 
                    basketTransform.position + _starSpawnConfig.DistanceFromBasketToStar * basketTransform.up;
            }
        }
    }
}