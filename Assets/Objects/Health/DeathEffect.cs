using System;
using System.Collections.Generic;

namespace Assets.Objects.Health
{
    [Serializable]
    public abstract class DeathEffect
    {
        public abstract IEnumerator<float> Effect();
    }
}
