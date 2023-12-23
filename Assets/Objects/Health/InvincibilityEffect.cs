using System;
using System.Collections.Generic;

namespace Assets.Objects.Health
{
    [Serializable]
    public abstract class InvincibilityEffect
    {
        public abstract IEnumerator<float> Effect();
        public abstract void Stop();
    }
}
