namespace DunkShot
{
    public class UIManager
    {
        private readonly Level _level;
        private readonly HudUIPanel _hudUIPanel;
        private readonly PauseMenuUIPanel _pauseMenuUIPanel;
        private readonly SettingsUIPanel _settingsUIPanel;
        private readonly GameOverUIPanel _gameOverUIPanel;

        public UIManager(Level level, UIProvider uiProvider)
        {
            _level = level;
            
            _hudUIPanel = uiProvider.GetUIPanel<HudUIPanel>();
            _pauseMenuUIPanel = uiProvider.GetUIPanel<PauseMenuUIPanel>();
            _settingsUIPanel = uiProvider.GetUIPanel<SettingsUIPanel>();
            _gameOverUIPanel = uiProvider.GetUIPanel<GameOverUIPanel>();

            _settingsUIPanel.Init(level);

            Subscribe();
        }

        private void Subscribe()
        {
            _hudUIPanel.PauseButtonPressed += ShowPauseUIPanel;
            _pauseMenuUIPanel.RestartButtonPressed += _level.RestartLevel;
            _pauseMenuUIPanel.SettingsButtonPressed += ShowSettingsUIPanelFromPauseMenu;
            _gameOverUIPanel.PlayAgainButtonPressed += _level.RestartLevel;
            _gameOverUIPanel.SettingsButtonPressed += ShowSettingsUIPanelFromGameOverMenu;

            _level.LevelStarted += ShowHudUIPanel;
            _level.LevelLost += ShowGameOverUIPanel;
            _level.LevelRestarted += Unsubscribe;
            _level.BackgroundColorChanged += _pauseMenuUIPanel.ChangeBackgroundColor;
            _level.BackgroundColorChanged += _settingsUIPanel.ChangeBackgroundColorAndIcon;
        }

        private void Unsubscribe()
        {
            _hudUIPanel.PauseButtonPressed -= ShowPauseUIPanel;
            _pauseMenuUIPanel.SettingsButtonPressed -= ShowSettingsUIPanelFromPauseMenu;
            _pauseMenuUIPanel.RestartButtonPressed -= _level.RestartLevel;
            _gameOverUIPanel.PlayAgainButtonPressed -= _level.RestartLevel;
            _gameOverUIPanel.SettingsButtonPressed -= ShowSettingsUIPanelFromGameOverMenu;

            _level.LevelStarted -= ShowHudUIPanel;
            _level.LevelLost -= ShowGameOverUIPanel;
            _level.LevelRestarted -= Unsubscribe;
            _level.BackgroundColorChanged -= _pauseMenuUIPanel.ChangeBackgroundColor;
            _level.BackgroundColorChanged -= _settingsUIPanel.ChangeBackgroundColorAndIcon;
        }

        private void ShowSettingsUIPanelFromPauseMenu(UIPanel actualPanel)
        {
            _settingsUIPanel.Show(actualPanel);
        }
        
        private void ShowSettingsUIPanelFromGameOverMenu()
        {
            _settingsUIPanel.Show();
        }

        private void ShowHudUIPanel()
        {
            _hudUIPanel.Show();
        }

        private void ShowPauseUIPanel()
        {
            _pauseMenuUIPanel.Show(_hudUIPanel);
        }

        private void ShowGameOverUIPanel()
        {
            _gameOverUIPanel.Show(_hudUIPanel);
        }
    }
}