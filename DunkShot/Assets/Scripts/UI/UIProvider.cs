using System.Collections.Generic;
using UnityEngine;

namespace DunkShot
{
    public class UIProvider : MonoBehaviour
    {
        [SerializeField] private List<UIPanel> _uiPanels;
        public StarCounterConfig _starCounterConfig;
        

        private Level _level;
        private UIManager _uiManager;

        public void Init(Level level)
        {
            _level = level;

            _uiManager = new UIManager(_level, this);
        }

        public TPanel GetUIPanel<TPanel>() where TPanel : UIPanel
        {
            return _uiPanels.Find(panel => panel is TPanel) as TPanel;
        }
    }
}