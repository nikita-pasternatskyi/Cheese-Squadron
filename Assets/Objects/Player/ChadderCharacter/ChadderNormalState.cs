using Assets.Objects.Player.Input;
using MEC;
using MykroFramework.Runtime.Objects.Player2D;
using MykroFramework.Runtime.Objects.SOStateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Objects.Player
{
    public class ChadderNormalState : PlayerBaseState
    {
        [SerializeField] private float _slideSpeed;
        [SerializeField] private float _slideTime;
        private SlideAbility _slideAbility;
        private ObjectFacing2D _facing;
        private CoroutineHandle _slideRoutine;
        private float _moveSpeed;

        protected override void OnInit(SOStateMachine stateMachine)
        {
            _slideAbility = stateMachine.GetComponent<SlideAbility>();
            _facing = stateMachine.GetComponent<ObjectFacing2D>();
        }

        protected override void OnLanded()
        {
            _moveSpeed = MoveSpeed;
        }

        protected override void OnUpdate()
        {
            if (PlayerCharacter.Grounded)
            {
                _moveSpeed = _slideRoutine.IsRunning ? _slideSpeed : MoveSpeed;
                if (Input.Values[Input.JUMP].WasJustPressed)
                {
                    PlayerCharacter.Jump(new Vector2(0, JumpHeight));
                }
            }

            if (PlayerInput.AbsoluteInput.x != 0)
                PlayerCharacter.Move(_moveSpeed, 0, PlayerInput.AbsoluteInput);

            if (Input.Values[Input.JUMP].WasJustReleased && PlayerCharacter.Velocity.y > 0)
                PlayerCharacter.Velocity.y = 0;

            if (Input.Values[Input.SPECIAL].WasJustPressed && !_slideRoutine.IsRunning)
                _slideRoutine = Timing.RunCoroutine(Slide(_facing.Facing));
        }

        private IEnumerator<float> Slide(float startingInput)
        {
            float timer = 0;
            _slideAbility.Slide();
            while (timer <= _slideTime)
            {
                timer += Time.deltaTime;
                if (Vector2.Dot(new Vector2(startingInput, 0), PlayerInput.AbsoluteInput) < 0)
                    break;
                yield return Timing.WaitForOneFrame;
            }
            _slideAbility.StopSlide();
        }
    }
}
