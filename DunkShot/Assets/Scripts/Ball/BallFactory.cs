using System.Collections.Generic;
using UnityEngine;

namespace DunkShot
{
    public class BallFactory
    {
        private const float BasketOffsetY = 2f;
        
        public Ball Create(BallConfig ballConfig, Basket basket, List<TrajectoryPoint> trajectoryPoints, Borders borders)
        {
            Vector3 basketPosition = basket.transform.position;
            Vector3 instancePosition = basketPosition.WithY(basketPosition.y + BasketOffsetY);
            
            Ball ballInstance = Object.Instantiate(ballConfig.Prefab, instancePosition, Quaternion.identity);
            ballInstance.Init(ballConfig);
            
            BallShooter ballShooter = ballInstance.gameObject.AddComponent<BallShooter>();
            ballShooter.Init(ballConfig.BallShooterConfig, trajectoryPoints, borders);
            
            return ballInstance;
        }
    }
}