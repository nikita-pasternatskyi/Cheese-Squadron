using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Objects.Health
{
    public class FadeSpriteRenderer : DeathEffect
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _fadeTime;

        public override IEnumerator<float> Effect()
        {
            float timer = 0;
            var color = _spriteRenderer.color;
            while (timer <= _fadeTime)
            {
                timer += Time.deltaTime;
                color.a = Mathf.Lerp(1, 0, timer / _fadeTime);
                _spriteRenderer.color = color;
                yield return Timing.WaitForOneFrame;
            }
        }
    }
}
