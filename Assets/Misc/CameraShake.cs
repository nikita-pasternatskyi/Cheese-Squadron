using Assets.Objects.Player;
using EZCameraShake;
using MEC;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Misc
{

    [CreateAssetMenu(menuName ="Camera/Shake")]
    public class CameraShake : ScriptableObject
    {
        public float Magnitude, Roughness, FadeInTime, FadeOutTime;

        public void Shake()
        {
            CameraShaker.Instance.ShakeOnce(Magnitude, Roughness, FadeInTime, FadeOutTime);
        }
    }
}
