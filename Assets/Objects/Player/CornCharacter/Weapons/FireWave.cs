using Assets.Objects.Player.Input;
using MykroFramework.Runtime.Objects.Weaponry;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Objects.Player.CornCharacter
{

    public class FireWave : CustomWeapon
    {
        [SerializeField] private Bounds _hitArea;
        [SerializeField] private LayerMask _hittable;
        [SerializeField] private float _fireRate;
        [SerializeField] private int _damage;
        private float _fireTimer;

        private Collider2D[] _objects = new Collider2D[8];

        public UnityEvent UnityEvent_FireStarted;
        public UnityEvent UnityEvent_FireEnded;

        public event Action FireStarted;
        public event Action FireEnded;

        private bool _holding;

        private void OnEnable()
        {
            FireStarted += UnityEvent_FireStarted.Invoke;
            FireEnded += UnityEvent_FireEnded.Invoke;
        }

        private void OnDisable()
        {
            FireStarted -= UnityEvent_FireStarted.Invoke;
            FireEnded -= UnityEvent_FireEnded.Invoke;
        }

        private void OnDrawGizmos()
        {
            var size = _hitArea.size.x * transform.right + _hitArea.size.y * transform.up;
            var center = _hitArea.center.x * transform.right + _hitArea.center.y * transform.up;
            Gizmos.DrawWireCube(transform.position + center, size);
        }

        public override void Fire()
        {
            _fireTimer = 0;
            FireStarted?.Invoke();
        }

        public override void EndFire()
        {
            FireEnded?.Invoke();
            _holding = false;
        }

        public override void HoldingFire()
        {
            _holding = true;
            _fireTimer += Time.deltaTime; 
            var size = _hitArea.size.x * transform.right + _hitArea.size.y * transform.up;
            var center = _hitArea.center.x * transform.right + _hitArea.center.y * transform.up;
            if (_fireTimer >= 1 / (1 + _fireRate))
            {
                for (int i = 0; i < Physics2D.OverlapBoxNonAlloc(transform.position + center,
                size, 0, _objects, _hittable); i++)
                {
                    if (_objects[i].TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamage(_damage, transform.right);
                    }
                }
                _fireTimer = 0;
            }
            
        }
    }
}
