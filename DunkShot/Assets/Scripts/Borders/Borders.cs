using UnityEngine;

namespace DunkShot
{
    public class Borders : MonoBehaviour
    {
        [SerializeField] private Transform _leftBorder;
        [SerializeField] private Transform _rightBorder;
        [SerializeField] private Transform _bottomBorder;
        
        private Camera _camera;

        public Vector2 ScreenLengths { private set; get; }

        private void Awake()
        {
            Vector2 resolution = UnityEditor.Handles.GetMainGameViewSize();
            _camera = Camera.main;
            
            ScreenLengths = _camera.ScreenToWorldPoint(new Vector3(resolution.x, resolution.y, 0)) * 2;
        }

        private void Start()
        {
            SetBordersPosition(ScreenLengths);

            SetBordersScale(ScreenLengths);
        }

        private void SetBordersPosition(Vector2 screenLengths)
        {
            _leftBorder.position = new Vector3(- screenLengths.x / 2 - _leftBorder.localScale.x / 2, 0, 0);
            _rightBorder.position = new Vector3(screenLengths.x / 2 + _rightBorder.localScale.x / 2, 0, 0);
            _bottomBorder.position = new Vector3(0, - screenLengths.y / 2 - _bottomBorder.localScale.y / 2, 0);
        }

        private void SetBordersScale(Vector2 screenLengths)
        {
            float lateralBordersScaleY = screenLengths.y;

            _leftBorder.localScale = _leftBorder.localScale.WithY(lateralBordersScaleY);
            _rightBorder.localScale = _rightBorder.localScale.WithY(lateralBordersScaleY);
            _bottomBorder.localScale = _bottomBorder.localScale.WithX(screenLengths.x);
        }
    }
}