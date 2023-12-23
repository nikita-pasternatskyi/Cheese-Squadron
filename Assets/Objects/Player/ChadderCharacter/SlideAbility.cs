using UnityEngine;

namespace Assets.Objects.Player
{
    public class SlideAbility : MonoBehaviour
    {
        public Collider2D ColliderToDisable;

        public void Slide() => ColliderToDisable.enabled = false;
        public void StopSlide() => ColliderToDisable.enabled = true;
    }
}
