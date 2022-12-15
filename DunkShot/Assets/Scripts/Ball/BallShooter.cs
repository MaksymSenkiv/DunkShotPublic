using System.Collections.Generic;
using UnityEngine;

namespace DunkShot
{
    [RequireComponent(typeof(Ball))]
    public class BallShooter : MonoBehaviour
    {
        private BallShooterConfig _ballShooterConfig;
        private List<TrajectoryPoint> _trajectoryPoints;
        
        private Ball _ball;
        private BallTrajectory _ballTrajectory;
        private Transform _currentBasketTransform;
        private Borders _borders;
        private Camera _camera;
        private Vector2 _startMousePosition;

        private Vector2 CurrentMousePosition => _camera.ScreenToWorldPoint(Input.mousePosition);

        public void Init(BallShooterConfig ballShooterConfig, List<TrajectoryPoint> trajectoryPoints, Borders borders)
        {
            _ballShooterConfig = ballShooterConfig;
            _trajectoryPoints = trajectoryPoints;
            _borders = borders;
        }
        
        private void Awake()
        {
            _camera = Camera.main;
            _ball = GetComponent<Ball>();
        }

        private void OnEnable()
        {
            _ball.HitInBasket += SetCurrentBasket;
        }

        private void OnDestroy()
        {
            _ball.HitInBasket -= SetCurrentBasket;
        }

        private void SetCurrentBasket(Ball.ShotData shotData)
        {
            _currentBasketTransform = shotData.CurrentBasket.transform;
        }

        private void Start()
        {
            _ballTrajectory = new BallTrajectory(_trajectoryPoints,
                _borders.ScreenLengths.x / 2 - _ball.transform.localScale.x / 2, 
                _ball.GetComponent<Rigidbody2D>().gravityScale,
                _ballShooterConfig.TrajectoryConfig);
        }

        private void Update()
        {
            if (_ball.State != BallStates.InBasket)
            {
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                _startMousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                Aim();
            }
            else if (Input.GetMouseButtonUp(0) && CheckMinVelocity(out Vector2 shotVelocity))
            {
                Shoot(shotVelocity);
            }
        }

        private void Aim()
        {
            if (CheckMinVelocity(out Vector2 shotVelocity))
            {
                _ballTrajectory.Calculate(_currentBasketTransform.position, shotVelocity);

                _ballTrajectory.SetTrajectoryColorAlpha(shotVelocity.magnitude / _ballShooterConfig.MaxShotVelocity);
                
                BasketLookAtTrajectory();
            }
            else
            {
                _ballTrajectory.RemoveTrajectoryPoints();
            }
        }

        private void BasketLookAtTrajectory()
        {
            Vector3 direction = _trajectoryPoints[1].transform.position - _currentBasketTransform.position;
            float angleZ = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;
            _currentBasketTransform.rotation = Quaternion.Euler(0f, 0f, angleZ - 90);;
        }

        private bool CheckMinVelocity(out Vector2 shotVelocity)
        {
            shotVelocity = CalculateShotVelocity();
            return shotVelocity.magnitude > _ballShooterConfig.MinShotVelocity;
        }

        private void Shoot(Vector2 shotVelocity)
        {
            AudioPlayer.PlaySound(SoundType.Shoot);
            _ball.Shoot(shotVelocity);
            _ballTrajectory.RemoveTrajectoryPoints();
        }

        private Vector2 CalculateShotVelocity()
        {
            Vector2 shotVelocity = (_startMousePosition - CurrentMousePosition) * _ballShooterConfig.VelocityInputMultiplier;
            shotVelocity = Vector2.ClampMagnitude(shotVelocity, _ballShooterConfig.MaxShotVelocity);
            return shotVelocity;
        }
    }
}