using MEC;
using MykroFramework.Runtime.Objects.Player2D;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Objects.Health
{
    public class EnableDisableCharacterController : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter2D _characterToDisableMove;
        [SerializeField] private HP _health;
        [SerializeField] private float _time;

        private CoroutineHandle _timerHandle;

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
            Timing.RunCoroutine(Consume());
        }

        private IEnumerator<float> Consume()
        {
            float timer = 0;
            _characterToDisableMove.Velocity = Vector2.zero;
            _characterToDisableMove.enabled = false;
            while (timer < _time)
            {
                timer += Time.deltaTime;
                yield return Timing.WaitForOneFrame;
            }
            _characterToDisableMove.enabled = true;
        }
    }
}
