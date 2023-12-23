using MykroFramework.Runtime.Objects.InteractingWithEnvironment.Scan;
using MykroFramework.Runtime.Objects.Weaponry;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Assets.Objects.Enemy
{
    public class HitArea : MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private bool _instaKill;
        [SerializeReference] private EnvironmentScanner2D _scanner;

        private void OnDrawGizmos()
        {
            _scanner.DrawDebug();
        }

        private void FixedUpdate()
        {
            var hits = _scanner.GetHits(transform.position);
            foreach (var hit in hits)
            {
                if (hit.isTrigger == false)
                    continue;
                if (hit.gameObject.TryGetComponent(out IDamageable damageable))
                {
                    if (_instaKill)
                    {
                        damageable.InstantKill();
                        return;
                    }
                    damageable.TakeDamage(_damage, transform.right);
                }
            }
        }
    }
}
