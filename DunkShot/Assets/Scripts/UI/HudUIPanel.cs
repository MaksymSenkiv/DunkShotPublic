using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DunkShot
{
    public class HudUIPanel : UIPanel
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _starsAmountText;

        public event Action PauseButtonPressed;

        protected void OnEnable()
        {
            _pauseButton.onClick.AddListener(OnPauseButtonPressed);
        }

        protected void OnDisable()
        {
            _pauseButton.onClick.RemoveListener(OnPauseButtonPressed);
        }

        private void OnPauseButtonPressed()
        {
            PauseButtonPressed?.Invoke();
        }

        public void SetScoreText(int score)
        {
            _scoreText.text = score.ToString();
        }

        public void SetStarsAmountText(int starsAmount)
        {
            _starsAmountText.text = starsAmount.ToString();
        }
    }
}