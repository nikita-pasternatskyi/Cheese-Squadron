using MykroFramework.Runtime.Extensions;
using MykroFramework.Runtime.Objects.Weaponry;
using UnityEngine;

namespace Assets.Objects.Enemy
{
    public class HitAreaCollisionTrigger : MonoBehaviour
    {
        [SerializeField] private int _damage;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage, transform.position.GetDirection(gameObject.transform.position));
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage, transform.position.GetDirection(other.gameObject.transform.position));
            }
        }
    }
}
