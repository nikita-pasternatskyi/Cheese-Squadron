using UnityEngine;

namespace Assets.Misc
{
    public class Test : MonoBehaviour
    {
        private void OnMouseEnter()
        {
            Debug.Log("enter", gameObject);
        }

        private void OnMouseDown()
        {
            Debug.Log("mouse down");   
        }
    }
}
