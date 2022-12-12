using UnityEngine;

namespace DunkShot
{
    public class TrajectoryPoint : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer { get; private set; }

        private void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}