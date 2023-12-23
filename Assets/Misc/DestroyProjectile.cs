using MykroFramework.Runtime.Objects.Weaponry;
using UnityEngine;

namespace Assets.Misc
{
    public class DestroyProjectile : MonoBehaviour
    {
        [SerializeField] private ClassicGun2DProjectile _projectile;
        private void OnEnable()
        {
            _projectile.Destroyed += OnDestroyed;   
        }

        private void OnDestroyed(Projectile obj)
        {
            _projectile.Destroyed -= OnDestroyed;
            Destroy(_projectile.gameObject);
        }
    }
}
