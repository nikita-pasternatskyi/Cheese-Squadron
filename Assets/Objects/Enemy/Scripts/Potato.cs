using Assets.Objects.Health;
using Assets.Objects.Player;
using Assets.Objects.Player.Input;
using MEC;
using MykroFramework.Runtime.Extensions;
using MykroFramework.Runtime.Objects.Player2D;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Objects.Enemy
{

    public class Potato : Enemy
    {
        [SerializeField] private float _anticipation;
        [SerializeField] private float _vulnerabilityDuration;
        [SerializeField] private float _attackCoolDown;
        [SerializeField] private Vector2 _rectCheckSize;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _idleAnimation;
        [SerializeField] private string _fireAnimation;
        [SerializeField] private string _anticipationAnimation;
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private ObjectFacing2D _objectFacing;
        [SerializeField] private HP _hp;
        private bool _died;

        private CoroutineHandle _attackRoutine;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, _rectCheckSize);
        }

        private void OnEnable() => _hp.Died += OnDied;

        private void OnDisable() => _hp.Died -= OnDied;

        private void OnDied() => _died = true;

        private bool _hasPlayer;

        private void Update()
        {
            if (_died)
            {
                Timing.KillCoroutines(_attackRoutine);
            }
            
            if (_attackRoutine.IsRunning == true)
                return;
            var collider = Physics2D.OverlapBox(transform.position, _rectCheckSize, 0, _playerLayer);
            if (collider)
            {
                if(collider.TryGetComponent<PlayerBase>(out PlayerBase player))
                {
                    _objectFacing.FaceInput(transform.position.GetDirection(player.transform.position));
                    _attackRoutine = Timing.RunCoroutine(Attack().CancelWith(gameObject));
                }
            }
        }

        private IEnumerator<float> Attack()
        {
            _animator.Play(_anticipationAnimation, 0, 0);
            yield return Timing.WaitForSeconds(_anticipation);
            _animator.Play(_fireAnimation, 0, 0);
            yield return Timing.WaitForSeconds(_vulnerabilityDuration);
            _animator.Play(_idleAnimation);
            yield return Timing.WaitForSeconds(_attackCoolDown);
        }
    }
}
