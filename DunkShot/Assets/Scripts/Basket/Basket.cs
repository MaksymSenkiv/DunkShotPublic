using System;
using DG.Tweening;
using UnityEngine;

namespace DunkShot
{
    public class Basket : MonoBehaviour
    {
        [SerializeField] private Transform _ballPosition;
        
        private BasketSpawnAnimationConfig _spawnAnimationConfig;
        
        private bool _isHit;

        public Transform BallPosition => _ballPosition;

        public event Action<Basket> BallIsScored;

        public void Init(BasketSpawnAnimationConfig spawnAnimationConfig)
        {
            _spawnAnimationConfig = spawnAnimationConfig;
        }
        
        private void Start()
        {
            SpawnEffect();
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (!_isHit && otherCollider.TryGetComponent(out Ball _))
            {
                BallIsScored?.Invoke(this);
                _isHit = true;
            }
        }
        
        public void Teleport(Vector3 position, float angleZ)
        {
            _isHit = false;
            transform.position = position;
            transform.rotation = Quaternion.Euler(0, 0, angleZ);
            
            SpawnEffect();
        }

        private void SpawnEffect()
        {
            float currentScale = transform.localScale.x;
            transform.localScale = Vector3.zero;
            transform.DOScale(currentScale, _spawnAnimationConfig.ScaleDuration)
                .SetEase(_spawnAnimationConfig.ScaleEase);
        }
    }
}