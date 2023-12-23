using Assets._Game.Controls;
using MykroFramework.Runtime.Controls;
using MykroFramework.Runtime.Objects.Player2D;
using MykroFramework.Runtime.Objects.Player2D.Input;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Objects.Player.Input
{


    public class PlayerInput : MonoBehaviour, IMovementInputProvider2D, IInputConsumable
    {
        [SerializeField] private InputRouterReference _inputRouter;
        [SerializeField] private ObjectFacing2D _objectFacing;
        [SerializeField] private WeaponSocket _weaponSocket;
        [SerializeField] private UI _uiInputMap;
        [SerializeField] private Gameplay _gameplayInputMap;
        [SerializeField] private PlayerBase _player;
        [Header("Weapon Collection")]
        [SerializeField] private Bounds _collectibleBounds;
        [SerializeField] private LayerMask _collectibles;

        public UnityEvent Paused;
        public UnityEvent UnPaused;

        private Collider2D[] _colliders = new Collider2D[2];

        public Vector2 AbsoluteInput { get; private set; }

        public bool IsConsumed { get; private set; }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + _collectibleBounds.center, _collectibleBounds.size);
        }

        private void Start()
        {
            _inputRouter.Router.ChangeButtonMap(_gameplayInputMap);
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < Physics2D.OverlapBoxNonAlloc(transform.position + _collectibleBounds.center,
                _collectibleBounds.size, 0, _colliders); i++)
            {
                var c = _colliders[i];
                if (c.TryGetComponent(out CustomWeapon weapon))
                {
                    _weaponSocket.PickUpWeaponObject(weapon);
                }
            }
        }

        public void UnPause()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (IsConsumed)
                return;

            if (_gameplayInputMap.Values[_gameplayInputMap.PAUSE].WasJustPressed)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Paused?.Invoke();
            }

            var x = _gameplayInputMap.CreateAxis(_gameplayInputMap.RIGHT, _gameplayInputMap.LEFT);
            var y = _gameplayInputMap.CreateAxis(_gameplayInputMap.UP, _gameplayInputMap.DOWN);

            AbsoluteInput = new Vector2(x, y);
            _objectFacing.FaceInput(AbsoluteInput);
            _player.Move(AbsoluteInput);
            _player.Jump(_gameplayInputMap.Values[_gameplayInputMap.JUMP]);
            _player.Special(_gameplayInputMap.Values[_gameplayInputMap.SPECIAL]);

            if (_gameplayInputMap.Values[_gameplayInputMap.SWITCH_WEAPON].WasJustPressed)
            {
                _weaponSocket.SwitchWeapon();
                if(_gameplayInputMap.Values[_gameplayInputMap.SHOOT].IsPressed) 
                    _weaponSocket.Weapon.Fire();
            }
            if (_gameplayInputMap.Values[_gameplayInputMap.SHOOT].IsPressed)
                _weaponSocket.Weapon.HoldingFire(); 
            if (_gameplayInputMap.Values[_gameplayInputMap.SHOOT].WasJustReleased)
                _weaponSocket.Weapon.EndFire(); 
            if (_gameplayInputMap.Values[_gameplayInputMap.SHOOT].WasJustPressed)
                _weaponSocket.Weapon.Fire();
        }

        public void Consume()
        {
            _weaponSocket.Weapon.EndFire();
            IsConsumed = true;
        }

        public void Release()
        {
            IsConsumed = false;
        }
    }
}
