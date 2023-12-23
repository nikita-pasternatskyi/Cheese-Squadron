using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Objects.Health
{
    public class ParticleSystemDeathEffect : DeathEffect
    {
        [SerializeField] private ParticleSystem _particleSystemToTrigger;

        public override IEnumerator<float> Effect()
        {
            _particleSystemToTrigger.Play();
            if (_particleSystemToTrigger.main.loop == false)
            {
                while (_particleSystemToTrigger.isPlaying)
                    yield return Timing.WaitForOneFrame;
            }
            else
            {
                while (_particleSystemToTrigger.time <= _particleSystemToTrigger.main.duration)
                    yield return Timing.WaitForOneFrame;
            }

        }
    }
}
