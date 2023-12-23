using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Misc
{

    class FrameRateLimiter : MonoBehaviour
    {
        public int FrameRateLimit;

        private void Awake()
        {
            Application.targetFrameRate = FrameRateLimit;
        }
    }
}
