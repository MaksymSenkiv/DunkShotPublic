using System.Collections.Generic;
using UnityEngine;

namespace DunkShot
{
    public class BallTrajectory
    {
        private readonly List<TrajectoryPoint> _points;
        private readonly float _limitBallPositionX;
        private readonly float _ballGravityScale;

        public BallTrajectory(List<TrajectoryPoint> points, float limitBallPositionX, float ballGravityScale, TrajectoryConfig config)
        {
            _points = points;
            _limitBallPositionX = limitBallPositionX;
            _ballGravityScale = ballGravityScale;

            for (var i = 0; i < _points.Count; i++)
            {
                _points[i].transform.localScale = 
                    Vector3.one * (config.MaxPointScale - (config.MaxPointScale - config.MinPointScale) / (_points.Count - 1) * i );
            }
        }

        public void Calculate(Vector3 startPoint, Vector3 startVelocity, float flyingSimulationTime)
        {
            float timeStep = flyingSimulationTime / _points.Count;
            _points[0].transform.position = startPoint;
            for (var i = 1; i < _points.Count; i++)
            {
                _points[i].transform.position = _points[i].transform.position
                    .WithX(_points[0].transform.position.x + startVelocity.x * timeStep * i);
                
                _points[i].transform.position  = _points[i].transform.position
                    .WithX(CheckBallBounce(_points[i].transform.position.x));
                
                _points[i].transform.position  = _points[i].transform.position
                    .WithY(_points[0].transform.position.y + 
                           (startVelocity.y + Physics2D.gravity.y * _ballGravityScale * timeStep * i * 0.5f) * timeStep * i);
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

        private float CheckBallBounce(float pointX)
        {
            if (Mathf.Abs(pointX) > _limitBallPositionX)
            {
                pointX = pointX > 0 ? _limitBallPositionX * 2 - pointX : - pointX - 2 * _limitBallPositionX;
            }

            return pointX;
        }
    }
}