using UnityEngine;

namespace DunkShot
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        public BallConfig BallConfig;

        public Color CurrentBackgroundColor;
        public Color LightThemeBackgroundColor;
        public Color DarkThemeBackgroundColor;
    }
}