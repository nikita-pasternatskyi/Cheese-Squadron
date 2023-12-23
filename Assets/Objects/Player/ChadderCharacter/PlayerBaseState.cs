using Assets._Game.Controls;
using Assets.Objects.Player.Input;
using MykroFramework.Runtime.Objects.Player2D;
using MykroFramework.Runtime.Objects.SOStateMachine;
using UnityEngine;

namespace Assets.Objects.Player
{
    public class PlayerBaseState : StateAction
    {
        [SerializeField] protected float MoveSpeed;
        [SerializeField] protected float JumpHeight;
        [SerializeField] private float _jumpBufferTime;
        private float _lastJumpTime;

        [field: SerializeField] protected Gameplay Input;
        protected PlayerCharacter2D PlayerCharacter;
        protected PlayerInput PlayerInput;

        private bool _jumpRequested;

        public override void Init(SOStateMachine stateMachine)
        {
            PlayerInput = stateMachine.GetComponent<PlayerInput>();
            PlayerCharacter = stateMachine.GetComponent<PlayerCharacter2D>();
            OnInit(stateMachine);
        }

        public override void Act()
        {
            if (Input.Values[Input.JUMP].WasJustPressed && !PlayerCharacter.Grounded)
            {
                _lastJumpTime = Time.time;
                _jumpRequested = true;
            }
            OnUpdate();
        }

        public override void Enter()
        {
            PlayerCharacter.Landed.AddListener(Landed);
            PlayerCharacter.Fell.AddListener(OnFell);
            PlayerCharacter.Jumped.AddListener(OnJumped);
            OnEnter();
        }

        public override void Exit()
        {
            PlayerCharacter.Landed.RemoveListener(Landed);
            PlayerCharacter.Fell.RemoveListener(OnFell);
            PlayerCharacter.Jumped.RemoveListener(OnJumped);
            OnExit();
        }

        private void Landed()
        {
            if (!_jumpRequested)
                return;
            if (Time.time - _lastJumpTime <= _jumpBufferTime)
                PlayerCharacter.Jump(new Vector2(0, JumpHeight));
            _jumpRequested = false;
            OnLanded();
        }
        protected virtual void OnUpdate() { }
        protected virtual void OnEnter() { }
        protected virtual void OnExit() { }
        protected virtual void OnLanded() { }
        protected virtual void OnFell() { }
        protected virtual void OnJumped() { }

        protected virtual void OnInit(SOStateMachine stateMachine) { }
    }
}
