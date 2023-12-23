using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Misc
{
    public class BackgroundMusicPlayer : MonoBehaviour
    {
        [SerializeField] private PlayMusicSO _musicChannel;
        [SerializeField] private float _fadeDuration = 1f;
        private CoroutineHandle _fadeHandle;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            _musicChannel.FadeRequested += Fade;
            _musicChannel.MusicRequested += Play;
        }

        private void OnDisable()
        {
            _musicChannel.FadeRequested -= Fade;
            _musicChannel.MusicRequested -= Play;
        }

        private void Play(AudioClip clip)
        {
            if (_fadeHandle.IsRunning)
                Timing.KillCoroutines(_fadeHandle);
            _audioSource.volume = 1;
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        private void Fade()
        {
            if (_fadeHandle.IsRunning)
                Timing.KillCoroutines(_fadeHandle);
            _fadeHandle = Timing.RunCoroutine(FadeRoutine());
        }

        private IEnumerator<float> FadeRoutine()
        {
            float time = 0;
            while (time <= _fadeDuration)
            {
                time += Time.unscaledDeltaTime;
                _audioSource.volume = Mathf.Lerp(1, 0, time / _fadeDuration);
                yield return Timing.WaitForOneFrame;
            }
        }
    }
}
