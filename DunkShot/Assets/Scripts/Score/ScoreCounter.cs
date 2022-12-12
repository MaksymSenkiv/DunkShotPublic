using UnityEngine;

namespace DunkShot
{
    public class ScoreCounter
    {
        private const int _maxPerfectScore = 10;
        
        private readonly Ball _ball;
        private readonly HudUIPanel _hudUIPanel;

        private Basket _previousBasket;
        private int _score;
        private int _perfectShots;

        public ScoreCounter(Ball ball, UIProvider uiProvider) 
        {
            _ball = ball;
            _hudUIPanel = uiProvider.GetUIPanel<HudUIPanel>();

            _ball.HitInBasket += IncreaseScore;
            _ball.Fell += Unsubscribe;
        }

        private void Unsubscribe()
        {
            _ball.HitInBasket -= IncreaseScore;
            _ball.Fell -= Unsubscribe;
        }

        private void IncreaseScore(Ball.ShotData shotData)
        {
            if (_ball.State == BallStates.Initial)
            {
                _previousBasket = shotData.CurrentBasket;
                return;
            }
            
            if (shotData.CurrentBasket != _previousBasket)
            {
                CalculateScore(shotData);
                _hudUIPanel.SetScoreText(_score);
                
                _previousBasket = shotData.CurrentBasket;
            }
        }

        private void CalculateScore(Ball.ShotData shotData)
        {
            _perfectShots = shotData.WasReturnedInBasket ? 0 : _perfectShots;
            _perfectShots = shotData.IsPerfect ? _perfectShots + 1 : 0;

            int addScore = shotData.IsPerfect ? Mathf.Clamp(_perfectShots + 1, 2, _maxPerfectScore) : 1;
            addScore = shotData.IsBounced ? addScore * 2 : addScore;
            _score += addScore;
        }
    }
}