namespace DunkShot
{
    public class StarCounter
    {
        private readonly Ball _ball;
        private readonly HudUIPanel _hudUIPanel;
        private readonly StarCounterConfig _config;
        private Basket _previousBasket;

        public StarCounter(Ball ball, UIProvider uiProvider) 
        {
            _ball = ball;
            _hudUIPanel = uiProvider.GetUIPanel<HudUIPanel>();
            _config = uiProvider._starCounterConfig;
            
            _hudUIPanel.SetStarsAmountText(_config.StarsAmount);

            _ball.StarCollided += IncreaseStarCounter;
            _ball.Fell += Unsubscribe;
        }

        private void Unsubscribe()
        {
            _ball.StarCollided -= IncreaseStarCounter;
            _ball.Fell -= Unsubscribe;
        }

        private void IncreaseStarCounter()
        {
            _config.StarsAmount++;
            _hudUIPanel.SetStarsAmountText(_config.StarsAmount);
        }
    }
}