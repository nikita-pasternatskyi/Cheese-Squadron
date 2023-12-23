using Assets._Game.Controls;
using Assets.Objects.Player.Input;
using MykroFramework.Runtime.Objects.Player2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Objects.Player
{
    public class PlayerBase : MonoBehaviour
    {
        [SerializeField] protected float MoveSpeed;
        [SerializeField] protected float JumpHeight;
        [SerializeField] private float _jumpBufferTime;
        private float _lastJumpTime;

        protected PlayerCharacter2D PlayerCharacter;
        protected PlayerInput PlayerInput;

        private bool _jumpRequested;

        private void Awake()
        {
            TryGetComponent(out PlayerInput);
            TryGetComponent(out PlayerCharacter);
            OnAwake();
        }

        private void OnEnable()
        {
            PlayerCharacter.Landed.AddListener(Landed);
            PlayerCharacter.Fell.AddListener(OnFell);
            PlayerCharacter.Jumped.AddListener(OnJumped);
            AfterEnable();
        }

        private void OnDisable()
        {
            PlayerCharacter.Landed.RemoveListener(Landed);
            PlayerCharacter.Fell.RemoveListener(OnFell);
            PlayerCharacter.Jumped.RemoveListener(OnJumped);
            AfterDisable();
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

        public void Special(ButtonState state) => OnSpecial(state);

        public void Move(Vector2 input) => OnMove(input);

        public void Jump(ButtonState state)
        {
            if (state.WasJustPressed && !PlayerCharacter.Grounded)
            {
                _lastJumpTime = Time.time;
                _jumpRequested = true;
            }
            OnJump(state);
        }

        protected virtual void OnSpecial(ButtonState state)
        {
        }
        protected virtual void OnMove(Vector2 input)
        {
        }
        protected virtual void OnJump(ButtonState state) { }
        protected virtual void AfterEnable() { }
        protected virtual void AfterDisable() { }
        protected virtual void OnLanded() { }
        protected virtual void OnFell() { }
        protected virtual void OnJumped() { }

        protected virtual void OnAwake() { }
    }
}
