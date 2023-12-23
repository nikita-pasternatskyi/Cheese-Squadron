using UnityEngine;

namespace Assets.UserInterface
{

    public class ChangeTMProColor : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _target;

        public void ChangeColor(Color newColor)
        {
            _target.color = newColor;
        }
    }
}
