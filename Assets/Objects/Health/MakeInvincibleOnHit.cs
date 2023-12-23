using MEC;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Objects.Health
{
    public class MakeInvincibleOnHit : SerializedMonoBehaviour
    {
        [SerializeReference] private InvincibilityEffect _effect;
        [SerializeField] private HP _health;
        [SerializeField] private float _time; 
        
        private void OnEnable()
        {
            _health.GotHit += OnGotHit;
        }

        private void OnDisable()
        {
            _health.GotHit -= OnGotHit;
        }

        private void OnGotHit(int arg1, Vector2 arg2, int arg3)
        {
            Effect();
        }

        public void Effect()
        {
            Timing.RunCoroutine(Invincible().CancelWith(gameObject));
        }

        private IEnumerator<float> Invincible()
        {
            float timer = 0;
            _health.Invincible = true;
            Timing.RunCoroutine(_effect.Effect());
            while (timer < _time)
            {
                timer += Time.deltaTime;
                yield return Timing.WaitForOneFrame;
            }
            _health.Invincible = false;
            _effect.Stop();
        }
    }
}
