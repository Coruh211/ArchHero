using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class BulletController : MonoBehaviour
    {
        public float Damage { get; private set; }

        [SerializeField] private float moveTime;

        private Vector3 _targetPos;
        
        public void Initialize(Vector3 targetPos, float damage)
        {
            Damage = damage;
            _targetPos = targetPos;
            
            Move();
        }

        private void Move()
        {
            transform.DOMove(_targetPos, moveTime).SetEase(Ease.Linear).OnComplete(() => SimplePool.Takeobj(gameObject));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Environment"))
            {
                SimplePool.Takeobj(gameObject);
            }
        }
    }
}