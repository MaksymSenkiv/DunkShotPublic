using System;
using UnityEngine;
using UnityEngine.UI;

namespace DunkShot
{
    public class GameOverUIPanel : UIPanel
    {
        [SerializeField] private Button _playAgainButton;
        [SerializeField] private Button _settingsButton;
        
        public event Action PlayAgainButtonPressed;
        public event Action SettingsButtonPressed;

        private void OnEnable()
        {
            _playAgainButton.onClick.AddListener(OnPlayAgainButtonPressed);
            _settingsButton.onClick.AddListener(OnSettingsButtonPressed);
        }

        private void OnDisable()
        {
            _playAgainButton.onClick.RemoveListener(OnPlayAgainButtonPressed);
            _settingsButton.onClick.RemoveListener(OnSettingsButtonPressed);
        }

        private void OnSettingsButtonPressed()
        {
            SettingsButtonPressed?.Invoke();
        }

        private void OnPlayAgainButtonPressed()
        {
            Hide();
            PlayAgainButtonPressed?.Invoke();
        }
    }
}