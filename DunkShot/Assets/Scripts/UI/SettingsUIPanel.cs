using System;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle;

namespace DunkShot
{
    public class SettingsUIPanel : UIPanel
    {
        [SerializeField] private Image _background;
        [SerializeField] private Button _backButton;
        [SerializeField] private Toggle _nightModeToggle;
        [SerializeField] private Image _toggleImage;
        [SerializeField] private Sprite _lightModeToggleImage;
        [SerializeField] private Sprite _nightModeToggleImage;
        
        private Level _level;

        public event Action<bool> NightModeToggleChanged;

        public void Init(Level level)
        {
            _level = level;
        }
        
        private void OnEnable()
        {
            _nightModeToggle.onValueChanged.AddListener(ToggleNightMode);
            _backButton.onClick.AddListener(OnBackButtonPressed);
            _level.BackgroundColorChanged += ChangeBackgroundColorAndIcon;
        }

        private void OnDisable()
        {
            _nightModeToggle.onValueChanged.RemoveListener(ToggleNightMode);
            _backButton.onClick.RemoveListener(OnBackButtonPressed);
            _level.BackgroundColorChanged -= ChangeBackgroundColorAndIcon;
        }

        private void ToggleNightMode(bool toggleValue)
        {
            NightModeToggleChanged?.Invoke(toggleValue);
        }

        private void OnBackButtonPressed()
        {
            Hide();
        }
        
        public void ChangeBackgroundColorAndIcon(Color color)
        {
            _background.color = color;
        }

        public void SetNightMode(bool isNightModeOn)
        {
            _toggleImage.sprite = isNightModeOn ? _nightModeToggleImage : _lightModeToggleImage;
            _nightModeToggle.SetIsOnWithoutNotify(isNightModeOn);
        }
    }
}