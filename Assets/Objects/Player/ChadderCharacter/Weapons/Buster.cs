using Assets.Objects.Player.Input;
using MykroFramework.Runtime.Objects.Weaponry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Objects.Player.ChadderCharacter.Weapons
{
    public class Buster : CustomWeapon
    {
        [SerializeField] private ClassicGun2DProjectile _normalShot;
        [SerializeField] private ClassicGun2DProjectile _middleChargeShot;
        [SerializeField] private ClassicGun2DProjectile _maxChargeShot;
        [SerializeField] private int _maxProjectileCount;
        [SerializeField] private float _chargeAwakeTime;
        [SerializeField] private float _timeUntilMaxCharge;
        public UnityEvent ChargeStart;
        public UnityEvent FullCharge;
        public UnityEvent ChargeEnd;
        public UnityEvent NormalShotFired;
        public UnityEvent MiddleShotFired;
        public UnityEvent ChargeShotFired;
        public UnityEvent ShotFired;

        private bool _chargeStartEmitted;
        private bool _fullChargeEmitted;

        private List<GameObject> _spawnedProjectiles;
        private float _timer;

        private void Awake()
        {
            if (_maxProjectileCount > 0)
                _spawnedProjectiles = new List<GameObject>(_maxProjectileCount);
        }

        public override void HoldingFire()
        {
            _timer = Mathf.Clamp(_timer + Time.deltaTime, 0, _timeUntilMaxCharge + _chargeAwakeTime);
            if (!_chargeStartEmitted && _timer >= _chargeAwakeTime)
            {
                _chargeStartEmitted = true;
                ChargeStart.Invoke();
            }
            if (!_fullChargeEmitted && _timer >= _timeUntilMaxCharge + _chargeAwakeTime)
            {
                _fullChargeEmitted = true;
                FullCharge.Invoke();
            }
        }

        public override void EndFire()
        {
            _chargeStartEmitted = _fullChargeEmitted = false;
            if (_timer > _chargeAwakeTime)
                ChargeEnd?.Invoke();

            if (_timer >= _chargeAwakeTime + _timeUntilMaxCharge)
            {
                ShootAProjectile(_maxChargeShot);
                ChargeShotFired?.Invoke();
            }
            else if (_timer >= _chargeAwakeTime)
            {
                ShootAProjectile(_middleChargeShot); 
                MiddleShotFired?.Invoke();
            }
        }

        private void ShootAProjectile(Projectile projectile)
        {
            if (_maxProjectileCount > 0)
                if (_spawnedProjectiles.Count >= _maxProjectileCount)
                    return;
            _timer = 0;
            var obj = Instantiate(projectile, transform.position, transform.rotation, null);
            ShotFired?.Invoke();
            obj.Destroyed += OnProjectileDestroyed;

            if (_maxProjectileCount < 0)
                return;
            _spawnedProjectiles.Add(obj.gameObject);
        }

        public override void Fire()
        {
            ShootAProjectile(_normalShot);
            NormalShotFired?.Invoke();
        }

        private void OnProjectileDestroyed(Projectile obj)
        {
            obj.Destroyed -= OnProjectileDestroyed;
            Destroy(obj.gameObject);
            if (_maxProjectileCount > 0)
                _spawnedProjectiles.Remove(obj.gameObject);
        }
    }
}
