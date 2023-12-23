using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Objects.Health
{

    public class ColorFlashOnHit : MonoBehaviour
    {
        [SerializeField] private HP _health;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _targetColor;
        [SerializeField] private float _timeToHoldIt;
        private Color _ogColor;

        private CoroutineHandle _handle;

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
            _ogColor = _spriteRenderer.color;
            _spriteRenderer.color = _targetColor;
            yield return Timing.WaitForSeconds(_timeToHoldIt);
            _spriteRenderer.color = _ogColor;
        }
    }
}
