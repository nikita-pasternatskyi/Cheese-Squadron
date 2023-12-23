using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Objects.Player.Input
{
    public class WeaponSocketUI : MonoBehaviour
    {
        [SerializeField] private Image _weaponIcon;

        public UnityEvent PickedAlternativeWeapon;
        public UnityEvent PickedMainWeapon;

        private WeaponSocket _weaponSocket;

        private void Awake()
        {
            TryGetComponent(out _weaponSocket);
        }

        private void OnEnable()
        {
            _weaponSocket.WeaponChanged += OnWeaponChanged;
            _weaponSocket.WeaponObtained += OnWeaponObtained;
        }

        private void OnDisable()
        {
            _weaponSocket.WeaponChanged -= OnWeaponChanged;
            _weaponSocket.WeaponObtained -= OnWeaponObtained;
        }

        private void OnWeaponObtained(CustomWeapon obj)
        {
            _weaponIcon.sprite = obj.UIDisplay;
        }

        private void OnWeaponChanged(WeaponType obj)
        {
            switch (obj)
            {
                case WeaponType.Main:
                    PickedMainWeapon?.Invoke();
                    break;
                case WeaponType.Additional:
                    PickedAlternativeWeapon?.Invoke();
                    break;
            }
        }
    }
}
