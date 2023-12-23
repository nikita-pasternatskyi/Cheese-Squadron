using System;
using UnityEngine;

namespace Assets.Objects.Player.Input
{

    public class WeaponSocket : MonoBehaviour
    {
        [SerializeField] private CustomWeapon _mainWeapon;
        [SerializeField] private CustomWeapon _additionalWeapon;
        private bool _useAdditionalWeapon;
        public CustomWeapon Weapon { get; private set; }

        public event Action<WeaponType> WeaponChanged;
        public event Action<CustomWeapon> WeaponObtained;
        public event Action WeaponLost;

        private void Awake()
        {
            if (_mainWeapon == null)
                Debug.LogError("No weapon found");
            Weapon = _mainWeapon;
        }

        public void LoseWeapon()
        {
            if (_additionalWeapon != null)
            {
                WeaponLost?.Invoke();
                Destroy(_additionalWeapon.gameObject);
            }   
        }

        public void PickUpWeaponObject(CustomWeapon newWeapon)
        {
            bool switchWeapon = Weapon != _additionalWeapon;

            if (newWeapon == _additionalWeapon)
                return;
            LoseWeapon();
            _additionalWeapon = newWeapon;
            _additionalWeapon.transform.parent = transform;
            _additionalWeapon.transform.localPosition = Vector3.zero;
            _additionalWeapon.transform.localRotation = Quaternion.identity;
            WeaponObtained?.Invoke(newWeapon);
            if(switchWeapon)
                SwitchWeapon();
            else
            {
                Weapon = _additionalWeapon;
                WeaponChanged?.Invoke(WeaponType.Additional);
            }
        }

        public void SwitchWeapon()
        {
            if (_additionalWeapon == null)
                return;
            _useAdditionalWeapon = !_useAdditionalWeapon;
            Weapon.EndFire();
            Weapon = _useAdditionalWeapon ? _additionalWeapon : _mainWeapon;
            WeaponChanged?.Invoke(_useAdditionalWeapon ? WeaponType.Additional : WeaponType.Main);
        }
    }
}
