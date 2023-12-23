using MEC;
using MykroFramework.Runtime.Objects.Player2D;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Objects.Health
{
    public class Lives : MonoBehaviour
    {
        [SerializeField] private int _startCount;
        [SerializeField] private float _time;
        [SerializeField] private HP _health;
        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private float _rayLength;
        [SerializeField] private PlayerCharacter2D _character;
        [SerializeField] private MakeInvincibleOnHit _invincbility;
        [SerializeField] private float _yOffset; 
        private Vector3 _lastPosition;
        private int _currentCount;
        public UnityEvent<int> LivesChanged;
        public UnityEvent<string> LivesChangedString;
        public UnityEvent NoLivesLeft;
        public UnityEvent Respawned;

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(transform.position, Vector3.down * _rayLength);
        }

        private void Awake()
        {
            _currentCount = _startCount;
            LivesChanged?.Invoke(_currentCount);
            LivesChangedString?.Invoke(_currentCount.ToString());
        }

        private void OnEnable()
        {
            _health.Died += OnDied;
        }

        private void LateUpdate()
        {
            if (Physics2D.Raycast(transform.position, Vector2.down, _rayLength, _whatIsGround))
            {
                _lastPosition = _character.transform.position;
                _lastPosition.y += _yOffset;
            }
        }

        private void OnDied()
        {
            if (_currentCount - 1 < 0)
            {
                NoLivesLeft?.Invoke();
                return;
            }
            _currentCount -= 1;
            LivesChanged?.Invoke(_currentCount);
            LivesChangedString?.Invoke(_currentCount.ToString());
            Timing.RunCoroutine(Respawn());
        }

        private IEnumerator<float> Respawn()
        {
            yield return Timing.WaitForSeconds(_time);
            transform.position = _lastPosition;
            _invincbility.Effect();
            _health.Heal(200);
            Respawned?.Invoke();
        }
    }
}
