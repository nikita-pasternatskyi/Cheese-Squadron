using UnityEngine;

namespace Assets.Objects.Player.CornCharacter
{
    public class FireWaveEffect : MonoBehaviour
    {
        [SerializeField] private FireWave _fireWave;
        [SerializeField] private ParticleSystem _particleSystem;

        private void OnEnable()
        {
            _fireWave.FireStarted += OnFireStarted;
            _fireWave.FireEnded += OnFireEnded;
        }

        private void OnDisable()
        {
            _fireWave.FireStarted -= OnFireStarted;
            _fireWave.FireEnded -= OnFireEnded;
        }

        private void OnFireEnded()
        {
            _particleSystem.Stop();
        }

        private void OnFireStarted()
        {
            _particleSystem.Play();
        }
    }
}
