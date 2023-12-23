using Assets.Objects.Player.CornCharacter.CornBomb;
using Assets.Objects.Player.Input;
using MykroFramework.Runtime.Objects.Player2D;
using UnityEngine;

namespace Assets.Objects.Player.CornCharacter
{
    public class Corn : PlayerBase
    {
        [SerializeField] private ObjectFacing2D _facing;
        [SerializeField] private PopcornBomb _popCornBombPrefab;
        [SerializeField] private Collider2D _colliderToDisableForCrouchAndRollJump;
        [SerializeField] private WeaponSocket _weaponSocket;
        [SerializeField] private int _cornBombCount;

        private Vector2 _input;

        protected override void OnMove(Vector2 input)
        {
            if (!PlayerCharacter.Grounded)
                _colliderToDisableForCrouchAndRollJump.enabled = false;

            if (PlayerCharacter.Grounded)
            {
                if (PlayerInput.AbsoluteInput.y < 0)
                {
                    _colliderToDisableForCrouchAndRollJump.enabled = false;
                    return;
                }
                else if (PlayerInput.AbsoluteInput.y >= 0 && _colliderToDisableForCrouchAndRollJump.enabled == false)
                    _colliderToDisableForCrouchAndRollJump.enabled = true;
            }

            _weaponSocket.transform.localRotation = Quaternion.Euler(0, 0, PlayerInput.AbsoluteInput.y * 45); 
            
            _input.x = _facing.Facing;

            if (PlayerCharacter.Grounded)
            {
                _input *= Mathf.Abs(PlayerInput.AbsoluteInput.x);
            }

            PlayerCharacter.Move(MoveSpeed, 0, _input);
        }

        protected override void OnJump(ButtonState state)
        {
            if (state.WasJustPressed && PlayerCharacter.Grounded)
            {
                PlayerCharacter.Jump(new Vector2(0, JumpHeight));
            }
        }

        protected override void OnLanded()
        {
            _colliderToDisableForCrouchAndRollJump.enabled = true;
        }

        protected override void OnSpecial(ButtonState state)
        {
            if (state.WasJustPressed && _cornBombCount > 0)
            {
                _cornBombCount--;
                Instantiate(_popCornBombPrefab, transform.position, transform.rotation);
            }
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
