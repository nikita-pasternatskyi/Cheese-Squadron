using MykroFramework.Runtime.Objects.InteractingWithEnvironment.Scan;
using MykroFramework.Runtime.Objects.Weaponry;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Objects.Health
{
    public class HealHealth : MonoBehaviour
    {
        [SerializeReference] private EnvironmentScanner2D _scanner;
        [SerializeField] private int _amount;

        public UnityEvent Healed;

        private void OnDrawGizmos()
        {
            _scanner.DrawDebug();
        }

        private void FixedUpdate()
        {
            foreach (var item in _scanner.GetHits(transform.position))
            {
                if (item.gameObject.TryGetComponent(out IHealable healable))
                {
                    healable.Heal(_amount);
                    Healed?.Invoke();
                }
            }
        }
    }
}
