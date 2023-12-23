using Assets.Objects.Player.Input;
using MEC;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Objects.Health
{


    public class ConsumeInputOnHit : SerializedMonoBehaviour
    {
        [SerializeField] private IInputConsumable _inputConsumable;
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
            _inputConsumable.Consume();
            while (timer < _time)
            {
                timer += Time.deltaTime;
                yield return Timing.WaitForOneFrame;
            }
            _inputConsumable.Release();
        }
    }
}
