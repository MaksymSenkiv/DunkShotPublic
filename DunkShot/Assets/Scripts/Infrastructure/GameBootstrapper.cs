using UnityEngine;

namespace DunkShot
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private Level _level;
        
        private void Start()
        {
            _level.Begin();
        }
    }
}