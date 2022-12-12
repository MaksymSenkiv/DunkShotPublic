using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DunkShot
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private SceneLoader _sceneLoader;
        
        [SerializeField] private CameraFollower _cameraFollower;
        [SerializeField] private List<TrajectoryPoint> _trajectoryPoints;
        [SerializeField] private Borders _borders;
        [SerializeField] private UIProvider _uiProvider;
        [SerializeField] private BasketsSpawner _basketsSpawner;
        [SerializeField] private StarSpawner _starSpawner;

        private SettingsUIPanel _settingsUIPanel;
        private ScoreCounter _scoreCounter;
        private StarCounter _starCounter;

        private BallFactory _ballFactory;
        private Ball _ball;

        public event Action LevelStarted;
        public event Action LevelLost;
        public event Action LevelRestarted;
        public event Action<Color> BackgroundColorChanged;

        public void Begin()
        {
            StartCoroutine(InitRoutine());
            LevelStarted?.Invoke();
        }

        private IEnumerator InitRoutine()
        {
            _cameraFollower.Init(_borders, this);
            _basketsSpawner.Init(_borders, _cameraFollower);
            InitUI();
            _starSpawner.Init(_basketsSpawner);

            yield return _basketsSpawner.Spawn();

            InitBall();

            _cameraFollower.SetTarget(_ball.transform);
            _scoreCounter = new ScoreCounter(_ball, _uiProvider);
            _starCounter = new StarCounter(_ball, _uiProvider);
        }

        private void GameOver()
        {
            AudioPlayer.PlaySound(SoundType.GameOver);
            LevelLost?.Invoke();
        }

        public void RestartLevel()
        {
            _settingsUIPanel.NightModeToggleChanged -= ChangeBackgroundColor;
            LevelRestarted?.Invoke();
            _sceneLoader.Restart();
        }

        private void InitUI()
        {
            _settingsUIPanel = _uiProvider.GetUIPanel<SettingsUIPanel>();
            _settingsUIPanel.SetNightMode(_levelConfig.DarkThemeBackgroundColor == _levelConfig.CurrentBackgroundColor);
            _settingsUIPanel.NightModeToggleChanged += ChangeBackgroundColor;
            _uiProvider.Init(this);
            
            BackgroundColorChanged?.Invoke(_levelConfig.CurrentBackgroundColor);
        }

        private void InitBall()
        {
            _ballFactory = new BallFactory();
            _ball = _ballFactory.Create(_levelConfig.BallConfig, _basketsSpawner.FirstBasket, _trajectoryPoints, _borders);
            _ball.Fell += GameOver;
        }

        private void ChangeBackgroundColor(bool nightMode)
        {
            _levelConfig.CurrentBackgroundColor = 
                nightMode ? _levelConfig.DarkThemeBackgroundColor : _levelConfig.LightThemeBackgroundColor;
            BackgroundColorChanged?.Invoke(_levelConfig.CurrentBackgroundColor);
            _settingsUIPanel.SetNightMode(_levelConfig.DarkThemeBackgroundColor == _levelConfig.CurrentBackgroundColor);
        }
    }
}