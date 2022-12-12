using System;
using UnityEngine;
using UnityEngine.UI;

namespace DunkShot
{
    public class PauseMenuUIPanel : UIPanel
    {
        [SerializeField] private Image _background;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _resumeButton;

        public event Action<UIPanel> SettingsButtonPressed;
        public event Action RestartButtonPressed;

        protected void OnEnable()
        {
            _settingsButton.onClick.AddListener(OnPauseButtonPressed);
            _restartButton.onClick.AddListener(OnRestartButtonPressed);
            _resumeButton.onClick.AddListener(OnResumeButtonPressed);
        }

        protected void OnDisable()
        {
            _settingsButton.onClick.RemoveListener(OnPauseButtonPressed);
            _restartButton.onClick.RemoveListener(OnRestartButtonPressed);
            _resumeButton.onClick.RemoveListener(OnResumeButtonPressed);
        }

        private void OnPauseButtonPressed()
        {
            SettingsButtonPressed?.Invoke(this);
        }

        private void OnRestartButtonPressed()
        {
            RestartButtonPressed?.Invoke();
        }

        private void OnResumeButtonPressed()
        {
            Hide();
        }

        public void ChangeBackgroundColor(Color color)
        {
            _background.color = color;
        }
    }
}