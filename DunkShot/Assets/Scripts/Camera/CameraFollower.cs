using UnityEngine;

namespace DunkShot
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _delayUp;
        [SerializeField] private float _delayDown;
        
        [SerializeField, Range(0.1f, 0.5f)] private float _targetPositionY;
        [SerializeField, Range(0.1f, 0.5f)] private float _maxCurrentBasketPositionY;
        [SerializeField, Range(0.05f, 0.3f)] private float _minCurrentBasketPositionY;

        private float _minPositionY;
        private float _maxPositionY;

        private Transform _target;
        private Level _level;
        private Borders _borders;

        public void Init(Borders borders, Level level)
        {
            _level = level;
            _borders = borders;

            _level.BackgroundColorChanged += ChangeBackgroundColor;
        }

        private void Start()
        {
            _minPositionY = transform.position.y;
            _maxPositionY = transform.position.y;
        }

        private void OnDestroy()
        {
            _level.BackgroundColorChanged -= ChangeBackgroundColor;
        }

        private void FixedUpdate()
        {
            if (ReferenceEquals(_target, null)) {
                return;
            }
            
            FollowTarget();
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void FollowTarget()
        {
            float positionY = 
                Mathf.Clamp(_target.position.y + _borders.ScreenLengths.y * (0.5f - _targetPositionY), 
                _minPositionY, _maxPositionY);
            
            float delay = positionY > transform.position.y ? _delayUp : _delayDown;
            positionY = Mathf.Lerp(transform.position.y, positionY, 1 / delay);

            transform.position = transform.position.WithY(positionY);
        }

        public void UpdateLimits(Basket basket)
        {
            Vector3 basketPosition = basket.transform.position;
            _minPositionY = basketPosition.y + _borders.ScreenLengths.y * (0.5f - _maxCurrentBasketPositionY);
            _maxPositionY = basketPosition.y + _borders.ScreenLengths.y * 0.5f - 
                            _minCurrentBasketPositionY * _borders.ScreenLengths.y;
        }

        private void ChangeBackgroundColor(Color color)
        {
            _camera.backgroundColor = color;
        }
    }
}