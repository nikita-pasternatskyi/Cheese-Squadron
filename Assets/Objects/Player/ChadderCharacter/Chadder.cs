using Assets.Objects.Player.Input;
using MEC;
using MykroFramework.Runtime.Objects.InteractingWithEnvironment.Scan;
using MykroFramework.Runtime.Objects.Player2D;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Objects.Player
{

    public class Chadder : PlayerBase
    {
        [SerializeField] private ObjectFacing2D _facing;
        [SerializeField] private float _ceilingCheckLength;
        [SerializeReference] private EnvironmentScanner2D _scanner;
        [SerializeField] private float _wallCheckLength;
        [SerializeField] private float _wallCheckHeight;
        [SerializeField] private Vector3 _wallCheckSize;
        [SerializeField] private Vector3 _ceilingCheckSize;
        [SerializeField] private LayerMask _whatIsWall;
        [SerializeField] private float _slideSpeed;
        [SerializeField] private float _slideTime;
        [SerializeField] private Collider2D[] _collidersToDisableForSlide;
        private CoroutineHandle _slideRoutine;
        private float _moveSpeed;
        private bool _hasWall;
        private bool _hasCeiling;

        public UnityEvent StartedSlide;
        public UnityEvent EndedSlide;

        private void OnDrawGizmos()
        {
            var wallCheck = transform.up * _wallCheckHeight + _wallCheckLength * new Vector3(_facing.Facing, 0, 0);
            Gizmos.DrawWireCube(transform.position + wallCheck, _wallCheckSize);
            Gizmos.DrawWireCube(transform.position + transform.up * _ceilingCheckLength, _ceilingCheckSize);
            _scanner.DrawDebug();
        }

        protected override void OnLanded()
        {
            _moveSpeed = MoveSpeed;
        }

        private void Update()
        {
            var wallCheck = transform.up * _wallCheckHeight + _wallCheckLength * new Vector3(_facing.Facing,0,0);
            _hasWall = Physics2D.Raycast(transform.position, new Vector2(_facing.Facing, 0), _wallCheckLength, _whatIsWall);
            _hasCeiling = Physics2D.BoxCast(transform.position, _ceilingCheckSize, 0, Vector3.up, _ceilingCheckLength, _whatIsWall);
        }

        protected override void OnMove(Vector2 input)
        {
            if (_hasWall && !_slideRoutine.IsRunning)
                return;
            _moveSpeed = _slideRoutine.IsRunning ? _slideSpeed : MoveSpeed;
            input.x = _slideRoutine.IsRunning ? _facing.Facing : input.x;
            PlayerCharacter.Move(_moveSpeed, 0, input);
        }

        protected override void OnJump(ButtonState state)
        {
            if (state.WasJustPressed && PlayerCharacter.Grounded)
            {
                if (_slideRoutine.IsRunning)
                {
                    Timing.KillCoroutines(_slideRoutine);
                    EndedSlide?.Invoke();
                }
                PlayerCharacter.Jump(new Vector2(0, JumpHeight));
            }
            else if (state.WasJustReleased && PlayerCharacter.Velocity.y > 0)
            {
                PlayerCharacter.Velocity.y = 0;
            }
        }

        protected override void OnSpecial(ButtonState state)
        {
            if (_hasWall)
                return;
            if (state.WasJustPressed && !_slideRoutine.IsRunning && PlayerCharacter.Grounded)
                _slideRoutine = Timing.RunCoroutine(Slide(_facing.Facing));
        }

        private IEnumerator<float> Slide(float startingInput)
        {
            float timer = 0;
            StartedSlide?.Invoke();

            foreach (var c in _collidersToDisableForSlide)
            {
                c.enabled = false;
            }
            while (timer <= _slideTime)
            {
                timer += Time.deltaTime;
                if (Vector2.Dot(new Vector2(startingInput, 0), PlayerInput.AbsoluteInput) < 0)
                    break; 
                yield return Timing.WaitForOneFrame;
            }

            yield return Timing.WaitUntilTrue(CanExitSlide);

            foreach (var c in _collidersToDisableForSlide)
            {
                c.enabled = true;
            }
            EndedSlide?.Invoke();
        }

        private bool CanExitSlide() => !_hasCeiling;
    }
}
