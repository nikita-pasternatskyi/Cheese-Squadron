using UnityEngine;
using UnityEngine.Events;

namespace Assets.Misc
{
    public class TriggerEnterEvents2D : MonoBehaviour
    {
        public UnityEvent Entered;
        public UnityEvent<GameObject> Entered_Object;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Entered_Object?.Invoke(collision.gameObject);
            Entered?.Invoke();
        }
    }
}
