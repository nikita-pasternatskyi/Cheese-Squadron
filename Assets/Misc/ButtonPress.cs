using MykroFramework.Runtime.Controls;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Misc
{

    public class ButtonPress : SerializedMonoBehaviour
    {
        [SerializeField] private bool _anyButton;
        [SerializeField, HideIf("@!_anyButton")] private bool _ignoreMouseClick;
        [SerializeField] private bool _uiAccept;
        private string[] _buttonsToPress;
        [SerializeField, Required] private ButtonMap _buttonMap;
        [SerializeField, Required] private InputRouterReference _inputRouterReference;
        [SerializeField, ValueDropdown("@_buttonMap.ButtonsArray")] private string[] _buttons;
        public UnityEvent ButtonPressed;

        private void Update()
        {
            if (_uiAccept)
            {
                if(_inputRouterReference.Router.UI_Apply_State.WasJustPressed)
                    ButtonPressed?.Invoke();
                return;
            }

            if (_anyButton)
            {
                if (_inputRouterReference.Router.TryGetCurrentlyPressedKey(out MykroKeyCode code))
                {
                    if (code.IsAxisInput() || code.IsMouseMovement())
                        return;
                    else if (code.IsMouse() && _ignoreMouseClick)
                        return;
                    ButtonPressed?.Invoke();
                    return;
                }
                return;
            }
            foreach (var item in _buttonsToPress)
            {
                if (_buttonMap.Values[item].WasJustPressed == true)
                    ButtonPressed?.Invoke();
            }
        }
    }
}
