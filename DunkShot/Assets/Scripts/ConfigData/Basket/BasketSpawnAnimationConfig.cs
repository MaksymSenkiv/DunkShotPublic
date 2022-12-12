using DG.Tweening;
using UnityEngine;

namespace DunkShot
{
    [CreateAssetMenu(fileName = "BasketSpawnAnimationConfig", menuName = "Configs/Basket/BasketSpawnAnimationConfig")]
    public class BasketSpawnAnimationConfig : ScriptableObject
    {
        public float ScaleDuration;
        public Ease ScaleEase;
    }
}