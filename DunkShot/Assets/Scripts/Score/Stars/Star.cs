using UnityEngine;

namespace DunkShot
{
    public class Star : MonoBehaviour
    {
        private StarSpawner _starSpawner;

        public void Init(StarSpawner starSpawner)
        {
            _starSpawner = starSpawner;
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider.TryGetComponent(out Ball _))
            {
                _starSpawner.ReturnToPool(this);
            }
        }
    }
}