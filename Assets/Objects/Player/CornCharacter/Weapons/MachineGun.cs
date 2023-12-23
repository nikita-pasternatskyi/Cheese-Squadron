using Assets.Objects.Player.Input;
using MykroFramework.Runtime.Objects.Weaponry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Objects.Player.CornCharacter
{

    public class MachineGun : CustomWeapon
    {
        [SerializeField] private ClassicGun2DProjectile _projectile;
        [SerializeField] private float _fireRate;
        private float _fireTimer;

        public override void HoldingFire()
        {
            _fireTimer += Time.deltaTime;
            if (_fireTimer >= 1/(1+_fireRate))
            {
                FireBullet();
            }
        }

        public override void Fire()
        {
            FireBullet();
        }

        private void FireBullet()
        {
            var p = Instantiate(_projectile, transform.position, transform.rotation);
            p.Destroyed += projectile => Destroy(projectile.gameObject);
            _fireTimer = 0;
        }
    }
}
