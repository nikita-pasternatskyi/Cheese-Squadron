using MEC;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Objects.Health
{
    public class FlickerSpriteRendererInvincibility : InvincibilityEffect
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _rate;
        [SerializeField] private bool _customOriginalColor;
        [SerializeField, ShowIf("_customOriginalColor")] private Color _originalColor;
        [SerializeField] private Color _flickerColor;
        private bool _run = true;

        public override IEnumerator<float> Effect()
        {
            _run = true;
            Color color = _customOriginalColor ? _originalColor : _spriteRenderer.color;
            bool flickered = false;
            float timer = 0;
            float tick = 1 / (1 + _rate);
            while (_run)
            {
                if (_spriteRenderer == null)
                    yield break;
                timer += Time.deltaTime;
                if (timer >= tick)
                {
                    flickered = !flickered;
                    _spriteRenderer.color = flickered ? _flickerColor : color;
                    timer = 0;
                }
                yield return Timing.WaitForOneFrame;
            }
            if (flickered == true)
            {
                while (timer <= tick)
                {
                    timer += Time.deltaTime;
                    yield return Timing.WaitForOneFrame;
                }
                if (_spriteRenderer == null)
                    yield break;
                _spriteRenderer.color = color;
            }
        }

        public override void Stop()
        {
            _run = false;
        }
    }
}
