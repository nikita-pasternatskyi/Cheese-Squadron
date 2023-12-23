using UnityEngine;

namespace Assets.Objects.Player
{
    public class CrouchAbility : MonoBehaviour
    {
        public Collider2D ColliderToDisable;

        public bool IsCrouching => ColliderToDisable.enabled == false;
        public void Crouch() => ColliderToDisable.enabled = false;
        public void StopCrouching() => ColliderToDisable.enabled = true;
    }
}
