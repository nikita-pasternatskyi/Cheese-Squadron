using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UserInterface
{
    public class WindowTest : MonoBehaviour
    {
        public WindowControl WindowControl;
        public Window newWindow;

        [Button]
        private void Test()
        {
            WindowControl.SwitchWindow(newWindow);
        }
    }

}

