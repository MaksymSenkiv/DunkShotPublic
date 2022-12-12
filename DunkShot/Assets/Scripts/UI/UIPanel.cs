using DG.Tweening;
using UnityEngine;

namespace DunkShot
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : MonoBehaviour
    {
        [SerializeField] protected UIPanelAppearanceConfig _appearanceConfig;

        private UIPanel _previousUIPanel;
        private CanvasGroup _mainCanvasGroup;
        private Sequence _appearanceSequence;

        protected virtual void Awake()
        {
            _mainCanvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Show(UIPanel previousPanel = null)
        {
            _previousUIPanel = previousPanel;
            previousPanel?.Hide();
            gameObject.SetActive(true);
            
            _appearanceSequence?.Kill();
            _appearanceSequence = DOTween.Sequence();

            _appearanceSequence
                .Append(_mainCanvasGroup.DOFade(1f, _appearanceConfig.ShowDuration));
        }

        public virtual void Hide()
        {
            _appearanceSequence?.Kill();
            _appearanceSequence = DOTween.Sequence();

            _appearanceSequence
                .Append(_mainCanvasGroup.DOFade(0, _appearanceConfig.HideDuration))
                .OnComplete(() => gameObject.SetActive(false));

            _previousUIPanel?.Show();
        }
    }
}