using Assets.Objects.Player.CornCharacter.CornBomb;
using Assets.Objects.Player.Input;
using MykroFramework.Runtime.Objects.Player2D;
using MykroFramework.Runtime.Objects.SOStateMachine;
using UnityEngine;

namespace Assets.Objects.Player
{
    public class PopsState : PlayerBaseState
    {
        [SerializeField] private ObjectFacing2D _facing;
        [SerializeField] private PopcornBomb _popCornBombPrefab;
        [SerializeField] private WeaponSocket _weaponSocket;
        private CrouchAbility _crouchAbility;

        private Vector2 _input;

        public override void Init(SOStateMachine stateMachine)
        {
            _facing = stateMachine.GetComponent<ObjectFacing2D>();
        }

        protected override void OnUpdate()
        {
            if (!PlayerCharacter.Grounded)
                _crouchAbility.Crouch();
            if (PlayerCharacter.Grounded)
            {
                if (Input.Values[Input.JUMP].WasJustPressed)
                {
                    PlayerCharacter.Jump(new Vector2(0, JumpHeight));
                }
                if (PlayerInput.AbsoluteInput.y < 0)
                {
                    _crouchAbility.Crouch();
                    return;
                }
                else if (PlayerInput.AbsoluteInput.y >= 0 && _crouchAbility.IsCrouching)
                    _crouchAbility.StopCrouching();
            }

            _weaponSocket.transform.localRotation = Quaternion.Euler(0, 0, PlayerInput.AbsoluteInput.y * 45);

            _input.x = _facing.Facing;

            if (PlayerCharacter.Grounded)
            {
                _input *= Mathf.Abs(PlayerInput.AbsoluteInput.x);
            }

            PlayerCharacter.Move(MoveSpeed, 0, _input);
        }

        protected override void OnJumped()
        {
        }

        protected override void OnLanded()
        {
            _crouchAbility.StopCrouching();
        }

        private int SnapFloat01(float f)
        {
            if (f > 0)
                return 1;
            else if (f < 0)
                return -1;
            else
                return 0;
        }
    }
}
