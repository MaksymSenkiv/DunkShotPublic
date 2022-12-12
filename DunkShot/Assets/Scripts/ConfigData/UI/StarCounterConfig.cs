using UnityEngine;

namespace DunkShot
{
    [CreateAssetMenu(fileName = "StarCounterConfig", menuName = "Configs/UI/StarCounterConfig")]
    public class StarCounterConfig : ScriptableObject
    {
        public int StarsAmount;
    }
}