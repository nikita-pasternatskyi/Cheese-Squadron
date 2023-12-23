using UnityEngine;
using UnityEngine.Events;

namespace Assets.UserInterface
{
    public class Window : MonoBehaviour
    {
        public Window PreviousWindow;
        public bool RememberThisWindow = true;
        public UnityEvent Opened;
        public UnityEvent Closed;
        public void Open() { Opened?.Invoke(); }
        public void Close() { Closed?.Invoke(); }
    }

}

