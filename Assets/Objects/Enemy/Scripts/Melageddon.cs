using Assets.Objects.Health;
using Assets.Objects.Player;
using Assets.Objects.Spawnpoint;
using MEC;
using MykroFramework.Runtime.Cameras;
using MykroFramework.Runtime.Extensions;
using MykroFramework.Runtime.Objects.InteractingWithEnvironment.Scan;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Objects.Enemy
{
    public class Melageddon : MonoBehaviour
    {
        [System.Serializable]
        public struct HealthState
        {
            public int HealthLimit;
            public FloatLimits WaitTimes;
            public float ShootCooldown;
            public float ShootRotationTime;
            public int MaxPlayerPositions;
        }

        [SerializeField] private HealthState _startPhase;
        [SerializeField] private HealthState _midPhase;
        [SerializeField] private HealthState _endPhase;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _shootPosition;
        [SerializeReference] private EnvironmentScanner2D _playerScanner;
        [SerializeField] private Transform _head;
        [SerializeField] private CharacterSpawn _spawnPoint;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _beginBeamRotation;
        [SerializeField] private float _endBeamRotation;
        [SerializeField] private HP _health;
        private HealthState _currentHealthState;
        private bool _previousState;
        public UnityEvent BeamEnded;
        public UnityEvent ChargedBeam;
        private GameObject _player;
        [SerializeField] private float _anticipationTime;
        private LinkedList<Vector3> _playerPositions = new LinkedList<Vector3>();

        private void OnDrawGizmos()
        {
            _playerScanner.DrawDebug();
        }

        private void OnEnable()
        {
            _spawnPoint.PlayerSpawned += OnPlayerSpawned;
            _health.GotHit += OnGotHit;
        }
        private void OnDisable()
        {
            _spawnPoint.PlayerSpawned -= OnPlayerSpawned;
            _health.GotHit -= OnGotHit;
        }

        private void Awake()
        {
            _currentHealthState = _startPhase;
        }

        private void OnGotHit(int arg1, Vector2 arg2, int arg3)
        {
            if (arg1 < _midPhase.HealthLimit && _currentHealthState.HealthLimit != _midPhase.HealthLimit)
                _currentHealthState = _midPhase;
            if(arg1 < _endPhase.HealthLimit)
                _currentHealthState = _endPhase;
        }

        private void OnPlayerSpawned(PlayerBase obj)
        {
            _player = obj.gameObject;
        }

        public void BeginStates()
        {
            if (_player == null)
            {
                foreach (var item in _playerScanner.GetHits(transform.position))
                {
                    if (item.TryGetComponent<PlayerBase>(out PlayerBase pb))
                    {
                        _player = pb.gameObject;
                        break;
                    }
                }
            }
            Timing.RunCoroutine(Wait());
        }

        int _repeats;
        private IEnumerator<float> Wait()
        {
            _animator.Play("Wait");
            var randomWaitTime = Random.Range(_currentHealthState.WaitTimes.Min, _currentHealthState.WaitTimes.Max);
            yield return Timing.WaitForSeconds(randomWaitTime);
            var randomState = Random.Range(0, 7);
            _playerPositions.Clear();
            bool beam = randomState % 2 == 0;
            if (beam != _previousState)
                _repeats = 0;
            else if (beam == _previousState)
            {
                _repeats++;
            }
            else if (_repeats > 1)
            {
                beam = !beam;
            }
            
            _previousState = beam;
            if (!beam)
            {
                yield return Timing.WaitUntilDone(Seeds().CancelWith(gameObject));
            }
            else
            {
                yield return Timing.WaitUntilDone(Beam().CancelWith(gameObject));
            }

        }

        //collect player's location 
        private IEnumerator<float> Beam()
        {
            _animator.Play("Beam");
            yield return Timing.WaitForSeconds(_anticipationTime);

            //shoot
            float time = 1;
            float timer = 0;
            var startRot = _head.rotation;
            while (timer < time)
            {
                timer += Time.deltaTime;
                _head.rotation = Quaternion.Slerp(startRot, Quaternion.Euler(0, 0, _beginBeamRotation), timer / time);
                yield return Timing.WaitForOneFrame;
            }
            ChargedBeam?.Invoke();
            yield return Timing.WaitForSeconds(2);
            timer = 0;
            time = 3;
            startRot = _head.rotation;
            while (timer < time)
            {
                timer += Time.deltaTime;
                _head.rotation = Quaternion.Slerp(startRot, Quaternion.Euler(0, 0, _endBeamRotation), timer / time);
                yield return Timing.WaitForOneFrame;
            }
            BeamEnded?.Invoke();
            yield return Timing.WaitUntilDone(Wait().CancelWith(gameObject));

        }

        public void Shoot()
        {
            Instantiate(_bulletPrefab, _shootPosition.position, _shootPosition.rotation);
        }

        private IEnumerator<float> Seeds()
        {
            _animator.Play("Seeds");
            yield return Timing.WaitForSeconds(_anticipationTime);
            for (int i = 0; i < _currentHealthState.MaxPlayerPositions; i++)
            {
                var pos = _player.transform.position;
                var targetRotation = Quaternion.LookRotation(-_head.transform.position.GetDirection(pos)) * Quaternion.Euler(0, 0, -5);
                var startRotation = _head.transform.rotation;
                float timer = 0;
                while (timer < _currentHealthState.ShootRotationTime)
                {
                    timer += Time.deltaTime;
                    var newRotation = Quaternion.Slerp(startRotation, targetRotation, timer / _currentHealthState.ShootRotationTime);
                    newRotation.y = Quaternion.identity.y;
                    newRotation.x = Quaternion.identity.x;
                    _head.transform.rotation = newRotation;
                    yield return Timing.WaitForOneFrame;
                }
                _animator.Play("Fire", 0, 0);
                yield return Timing.WaitForSeconds(_currentHealthState.ShootCooldown);
            }
            yield return Timing.WaitUntilDone(Wait().CancelWith(gameObject));
        }
    }
}
