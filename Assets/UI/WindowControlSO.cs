using System;
using UnityEngine;

namespace Assets.UserInterface
{
    [CreateAssetMenu(menuName ="Game/UI/Window Control SO")]
    public class WindowControlSO : ScriptableObject
    {
        public event Action<Window> WindowChangeRequested;
        public event Action<bool> BlockChangeRequested;

        public void BlockNavigation(bool block)
        {
            BlockChangeRequested?.Invoke(block);
        }

        public void ChangeWindow(Window newWindow)
        {
            WindowChangeRequested?.Invoke(newWindow);
        }
    }

}

