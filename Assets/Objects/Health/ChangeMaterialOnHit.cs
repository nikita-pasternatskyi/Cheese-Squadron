using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Objects.Health
{

    public class ChangeMaterialOnHit : MonoBehaviour
    {
        [SerializeField] private HP _health;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Material _targetMaterial;
        [SerializeField] private float _timeToHoldIt;
        private CoroutineHandle _handle;
        private Material _ogMaterial;

        private void OnEnable()
        {
            _health.GotHit += OnGotHit;
        }

        private void OnDisable()
        {
            _health.GotHit -= OnGotHit;
        }

        private void OnGotHit(int health, Vector2 direction, int damage)
        {
            if (_handle.IsRunning)
                return;
            _handle = Timing.RunCoroutine(Flash());
        }

        private IEnumerator<float> Flash()
        {
            _ogMaterial = _spriteRenderer.sharedMaterial;
            _spriteRenderer.sharedMaterial = _targetMaterial;
            yield return Timing.WaitForSeconds(_timeToHoldIt);
            _spriteRenderer.sharedMaterial = _ogMaterial;
        }
    }
}
