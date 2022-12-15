using System.Collections.Generic;
using UnityEngine;

namespace DunkShot
{
    public class BallTrajectory
    {
        private readonly List<TrajectoryPoint> _points;
        private readonly float _limitBallPositionX;
        private readonly float _ballGravityScale;
        private readonly float _timeStep;
        private readonly float _simulationTime;

        public BallTrajectory(List<TrajectoryPoint> points, float limitBallPositionX, float ballGravityScale, TrajectoryConfig config)
        {
            _points = points;
            _limitBallPositionX = limitBallPositionX;
            _ballGravityScale = ballGravityScale;
            _simulationTime = config.TrajectorySimulationTime;

            for (var i = 0; i < _points.Count; i++)
            {
                _points[i].transform.localScale = 
                    Vector3.one * (config.MaxPointScale - (config.MaxPointScale - config.MinPointScale) / (_points.Count - 1) * i );
            }
            
            _timeStep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        }

        public void Calculate(Vector3 startPoint, Vector3 startVelocity)
        {
            _points[0].transform.position = startPoint;
            Vector2 gravityAccel = Physics2D.gravity * (_ballGravityScale * _timeStep * _timeStep);
            Vector2 moveStep = startVelocity * _timeStep;
            float stepsAmount = (_simulationTime / (_points.Count - 1)) / _timeStep;
            for (var i = 1; i < _points.Count; i++)
            {
                moveStep += gravityAccel * stepsAmount;
                _points[i].transform.position = _points[i - 1].transform.position + (Vector3) moveStep * stepsAmount;

                _points[i].transform.position = _points[i].transform.position
                    .WithX(CheckBallBounce(_points[i].transform.position.x, ref moveStep));
            }
        }

        public void RemoveTrajectoryPoints()
        {
            foreach (TrajectoryPoint point in _points)
            {
                Color color = point.SpriteRenderer.color;
                point.SpriteRenderer.color = new Color(color.r, color.g, color.b, 0);
            }
        }

        public void SetTrajectoryColorAlpha(float colorAlpha)
        {
            foreach (TrajectoryPoint point in _points)
            {
                Color color = point.SpriteRenderer.color;
                point.SpriteRenderer.color = new Color(color.r, color.g, color.b, colorAlpha);
            }
        }

        private float CheckBallBounce(float pointX, ref Vector2 moveStep)
        {
            if (Mathf.Abs(pointX) > _limitBallPositionX)
            {
                pointX = pointX > 0 ? _limitBallPositionX * 2 - pointX : - pointX - 2 * _limitBallPositionX;
                moveStep.x *= -1;
            }

            return pointX;
        }
    }
}