using Assets.Objects.Player;
using MEC;
using MykroFramework.Runtime.Extensions;
using MykroFramework.Runtime.Objects.Player2D;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Objects.Enemy.Scripts
{
    public class Tomato : Enemy
    {
        [SerializeField] private float _desiredLengthTillPlayer;
        [SerializeField] private Vector2 _bigRectCheckSize;
        [SerializeField] private ObjectFacing2D _objectFacing;
        [SerializeField] private Animator _animator;
        [SerializeField] private LayerMask _player;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _attackTime;
        [SerializeField] private string _walkAnimation;
        [SerializeField] private string _attackAnimation;
        [SerializeField] private string _fallAnimation;
        private PlayerCharacter2D _character2D;
        private PlayerBase _playerBase;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, _bigRectCheckSize);
        }

        private void Awake()
        {
            _character2D = GetComponent<PlayerCharacter2D>();
            Timing.RunCoroutine(Tick().CancelWith(gameObject));
        }

        private IEnumerator<float> Tick()
        {
            while (_character2D.Grounded == false)
            {
                _animator.Play(_fallAnimation);
                yield return Timing.WaitForOneFrame;
            }
            _animator.Play(_walkAnimation);
            while (_playerBase == null)
            {
                var c = Physics2D.OverlapBox(transform.position, _bigRectCheckSize, 0, _player);
                if (c != null)
                {
                    if (c.TryGetComponent<PlayerBase>(out PlayerBase playerBase))
                        _playerBase = playerBase;
                    yield return Timing.WaitForOneFrame;
                }

                yield return Timing.WaitForOneFrame;
            }
            var distance = Vector2.Distance(transform.position, _playerBase.transform.position);
            while (distance > _desiredLengthTillPlayer)
            {
                var facing = transform.position.GetDirection(_playerBase.transform.position);
                _objectFacing.FaceInput(facing.x);
                _character2D.Move(_moveSpeed, 0, transform.right); 
                distance = Vector2.Distance(transform.position, _playerBase.transform.position);
                yield return Timing.WaitForOneFrame;
            }
            yield return Timing.WaitUntilDone(Attack().CancelWith(gameObject));
            yield return Timing.WaitUntilDone(Tick().CancelWith(gameObject));
        }

        private IEnumerator<float> Attack()
        {
            _animator.Play(_attackAnimation,0,0);
            yield return Timing.WaitForSeconds(_attackTime);
        }
    }
}
