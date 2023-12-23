using MykroFramework.Runtime.Cameras;
using MykroFramework.Runtime.Objects.Player2D;
using UnityEngine;

namespace Assets.Objects.Health
{

    public class DamageKnockbackOnHit : MonoBehaviour
    {
        [SerializeField] private int _knockBackThreshold;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private HP _health;
        [SerializeField] private FloatLimits _forceLimits;

        private void OnEnable()
        {
            _health.GotHit += OnGotHit;
        }

        private void OnDisable()
        {
            _health.GotHit -= OnGotHit;
        }

        private void OnGotHit(int health, Vector2 direction, int damage)
        {
            if (damage > _knockBackThreshold)
            {
                var forceLimits = Mathf.Clamp(damage, _forceLimits.Min, _forceLimits.Max);
                _rigidbody.velocity = forceLimits * direction;
            }
        }
    }
}
