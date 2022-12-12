using System;
using System.Collections;
using UnityEngine;

namespace DunkShot
{
    public class BasketsSpawner : MonoBehaviour
    {
        private static readonly System.Random _random = new System.Random();

        [SerializeField] private Transform _firstBasketPoint;
        [SerializeField] private Transform _secondBasketPoint;
        
        [SerializeField] private BasketConfig _basketConfig;
        [SerializeField] private BasketSpawnerConfig _basketSpawnerConfig;

        private BasketFactory _basketFactory;
        private bool _firstBasketHit;

        private Basket _firstBasket;
        private Basket _secondBasket;

        private CameraFollower _cameraFollower;
        private Borders _borders;

        public event Action<Transform> BasketSpawned; 

        public Basket FirstBasket => _firstBasket;

        public void Init(Borders borders, CameraFollower cameraFollower)
        {
            _borders = borders;
            _cameraFollower = cameraFollower;
            
            _basketFactory = new BasketFactory();
        }

        public Coroutine Spawn()
        {
            return StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            _firstBasket = _basketFactory.Create(_basketConfig, _firstBasketPoint.position, transform);
            yield return new WaitForSeconds(_basketSpawnerConfig.SecondBasketSpawnDelay);
            _secondBasket = _basketFactory.Create(_basketConfig, _secondBasketPoint.position, transform);
            BasketSpawned?.Invoke(_secondBasket.transform);
            
            _firstBasket.BallIsScored += TeleportBasket;
            _firstBasket.BallIsScored += _cameraFollower.UpdateLimits;
            _secondBasket.BallIsScored += TeleportBasket;
            _secondBasket.BallIsScored += _cameraFollower.UpdateLimits;

            yield return new WaitForSeconds(_basketConfig.BasketSpawnAnimationConfig.ScaleDuration);
        }

        private void OnDestroy()
        {
            _firstBasket.BallIsScored -= TeleportBasket;
            _firstBasket.BallIsScored -= _cameraFollower.UpdateLimits;
            _secondBasket.BallIsScored -= TeleportBasket;
            _secondBasket.BallIsScored -= _cameraFollower.UpdateLimits;
        }

        private void TeleportBasket(Basket hitBasket)
        {
            if (!_firstBasketHit)
            {
                _firstBasketHit = true;
                return;
            }
            
            Basket teleportedBasket = hitBasket == _firstBasket ? _secondBasket : _firstBasket;
            CalculateTransform(hitBasket.transform, out Vector3 position, out float angleZ);
            
            teleportedBasket.Teleport(position, angleZ);

            BasketSpawned?.Invoke(teleportedBasket.transform);
        }

        private void CalculateTransform(Transform hitBasket, out Vector3 position, out float angleZ)
        {
            float minPositionX, maxPositionX, minRotationZ, maxRotationZ;
            if (hitBasket.position.x >= 0)
            {
                minPositionX = -(_borders.ScreenLengths.x / 2 - _basketConfig.BasketWidth / 2);
                maxPositionX = hitBasket.position.x - _basketConfig.BasketWidth;

                minRotationZ = -_basketSpawnerConfig.BasketRandomAngleRange;
                maxRotationZ = 0;
            }
            else
            {
                minPositionX = hitBasket.position.x + _basketConfig.BasketWidth;
                maxPositionX = _borders.ScreenLengths.x / 2 - _basketConfig.BasketWidth / 2;

                minRotationZ = 0;
                maxRotationZ = _basketSpawnerConfig.BasketRandomAngleRange;
            }

            position.x = _random.Next((int)(minPositionX * 100), (int)(maxPositionX * 100)) / 100f;
            if (Mathf.Abs(position.x) > _borders.ScreenLengths.x / 2 - _basketConfig.BasketWidth)
            {
                position.x = position.x > 0
                    ? _borders.ScreenLengths.x / 2 - _basketConfig.BasketWidth / 2
                    : _basketConfig.BasketWidth / 2 - _borders.ScreenLengths.x / 2;
            }
            position.y = CalculatePositionY(hitBasket);
            position.z = 0;
                
            angleZ = _random.Next((int)(minRotationZ * 100), (int)(maxRotationZ * 100)) / 100f;
        }

        private float CalculatePositionY(Transform hitBasket)
        {
            return Mathf.Clamp(hitBasket.position.y + _random.Next(
                    (int)(_basketSpawnerConfig.MinBasketsDistanceY * 100),
                    (int)(hitBasket.position.y + (_basketSpawnerConfig.MaxBasketsDistanceY * 100))) / 100f,
                hitBasket.position.y + _basketSpawnerConfig.MinBasketsDistanceY, 
                hitBasket.position.y + _basketSpawnerConfig.MaxBasketsDistanceY);
        }
    }
}