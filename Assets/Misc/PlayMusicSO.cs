using System;
using UnityEngine;

namespace Assets.Misc
{
    [CreateAssetMenu(menuName ="Play Music")]
    public class PlayMusicSO : ScriptableObject
    {
        public event Action<AudioClip> MusicRequested;
        public event Action FadeRequested;

        public void Play(AudioClip clip)
        {
            MusicRequested?.Invoke(clip);
        }

        public void Fade() => FadeRequested?.Invoke();
    }
}
