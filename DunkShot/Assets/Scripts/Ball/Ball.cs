using System;
using DG.Tweening;
using UnityEngine;

namespace DunkShot
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ball : MonoBehaviour
    {
        public class ShotData
        {
            public bool IsPerfect;
            public bool WasReturnedInBasket;
            public bool IsBounced;
            public Basket CurrentBasket;
        }

        private const string LosingBorderTag = "LosingBorder";
        private const string LateralBorderTag = "LateralBorder";
        private const string BasketBorderTag = "BasketBorder";
        
        private readonly ShotData _shotData = new ShotData();

        [SerializeField] private Transform _body;
        [SerializeField] private BallConfig _config;

        private BallStates _state = BallStates.Initial;
        private Rigidbody2D _rigidbody2D;

        public event Action Fell;
        public event Action StarCollided;
        public event Action<ShotData> HitInBasket;

        public BallStates State => _state;

        public void Init(BallConfig config)
        {
            _config = config;
        }

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_state == BallStates.Flying)
            {
                _body.Rotate(new Vector3(0, 0, _config.FlyingRotationSpeed * Time.deltaTime));
            }
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider.TryGetComponent(out Star _))
            {
                AudioPlayer.PlaySound(SoundType.StarCollected);
                StarCollided?.Invoke();
            }

            if (otherCollider.TryGetComponent(out Basket basket))
            {
                Basket previousBasket = _shotData.CurrentBasket;
                _shotData.CurrentBasket = basket;

                StopBall();
                MoveIntoBasket(_shotData.CurrentBasket.BallPosition);
                HitInBasket?.Invoke(_shotData);
                _state = BallStates.InBasket;
                
                _shotData.WasReturnedInBasket = previousBasket == _shotData.CurrentBasket;
                if (!_shotData.WasReturnedInBasket)
                {
                    AudioPlayer.PlaySound(SoundType.HitInNextBasket);
                }
            }
            else if (otherCollider.CompareTag(LosingBorderTag))
            {
                Fell?.Invoke();
                gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (collision2D.gameObject.CompareTag(LateralBorderTag))
            {
                AudioPlayer.PlaySound(SoundType.BallHit);
                _shotData.IsBounced = true;
            }
            if (_state == BallStates.Flying && collision2D.gameObject.CompareTag(BasketBorderTag))
            {
                AudioPlayer.PlaySound(SoundType.BallHit);
                _shotData.IsPerfect = false;
            }
        }

        public void Shoot(Vector2 shotVelocity)
        {
            _shotData.IsPerfect = true;
            _shotData.IsBounced = false;
            
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody2D.AddForce(shotVelocity * _rigidbody2D.mass, ForceMode2D.Impulse);
            _state = BallStates.Flying;
        }

        private void MoveIntoBasket(Transform positionInBasket)
        {
            _rigidbody2D.DOMove(positionInBasket.position, _config.MoveToBasketCenterDuration);
            transform.SetParent(positionInBasket);
        }

        private void StopBall()
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            _rigidbody2D.velocity = Vector2.zero;
        }
    }
}