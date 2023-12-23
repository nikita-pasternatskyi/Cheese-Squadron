using Assets.Objects.Player.Input;
using MykroFramework.Runtime.Extensions;
using MykroFramework.Runtime.Objects.Weaponry;
using UnityEngine;

namespace Assets.Objects.Player.CornCharacter
{

    public class SpreadGun : CustomWeapon
    {
        [SerializeField] private ClassicGun2DProjectile _projectile;
        [SerializeField] private float _fireRate;
        [SerializeField] private float _angleDifference;
        [SerializeField] private int _bulletCount;
        private float _lastTimeSinceFire;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            float rayLength = 2;
            Gizmos.color = Color.green;
            var starting = new Vector2(transform.right.x, transform.right.y);
            for (int i = 1; i <= _bulletCount; i++)
            {
                var angle = i * _angleDifference;
                var rotated = starting.Rotate(angle);
                Gizmos.DrawRay(transform.position, rotated.normalized * rayLength);
            }
            for (int i = 1; i <= _bulletCount; i++)
            {
                var angle = i * _angleDifference;
                var rotated = starting.Rotate(-angle);
                Gizmos.DrawRay(transform.position, rotated.normalized * rayLength);
            }
            Gizmos.DrawRay(transform.position, transform.right * rayLength);
        }
#endif

        public override void Fire()
        {
            if (Time.time - _lastTimeSinceFire >= 1 / (1 + _fireRate))
                FireBullet();
        }

        private void FireBullet()
        {
            for (int i = 1; i <= _bulletCount; i++)
            {
                var angle = i * _angleDifference;
                var p = Instantiate(_projectile, transform.position, transform.rotation);
                p.Destroyed += projectile => Destroy(projectile.gameObject);
                p.transform.Rotate(0, 0, angle);
            }
            for (int i = 1; i <= _bulletCount; i++)
            {
                var angle = i * _angleDifference;
                var p = Instantiate(_projectile, transform.position, transform.rotation);
                p.Destroyed += projectile => Destroy(projectile.gameObject);
                p.transform.Rotate(0, 0, -angle);
            }
            var projectile = Instantiate(_projectile, transform.position, transform.rotation);
            projectile.Destroyed += projectile => Destroy(projectile.gameObject);
            _lastTimeSinceFire = Time.time;
        }
    }
}
