using Assets.Objects.Player.Input;
using UnityEngine;

namespace Assets.Misc
{
    public class ConsumePlayerInput : MonoBehaviour
    {
        private PlayerInput _playerInput;

        public void GetPlayerInput(GameObject go)
        {
            _playerInput = go.GetComponent<PlayerInput>();
        }

        public void Consume()
        {
            _playerInput.Consume();
        }

        public void Release()
        {
            _playerInput.Release();
        }
    }
}
