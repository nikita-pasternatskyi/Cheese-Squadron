using Assets.Objects.Health;
using Assets.Objects.Player.ChadderCharacter.Weapons;
using Assets.Objects.Player.Input;
using MEC;
using MykroFramework.Runtime.Objects.Player2D;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Objects.Player
{
    public class ChadderAnimations : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private Animator _bodyAnimator;
        [SerializeField] private Animator _busterAnimator;
        [SerializeField] private PlayerCharacter2D _character;
        [SerializeField] private HP _health;
        [SerializeField] private Chadder _chadder;

        [Header("Arms")]
        [SerializeField] private Buster _buster;
        [SerializeField] private SpriteRenderer _busterArm;
        [SerializeField] private SpriteRenderer _normalArm;
        [SerializeField] private float _fireCooldownTime;

        [Header("Animations")]
        [SerializeField] private string _jumpAnimation;
        [SerializeField] private string _idleAnimation;
        [SerializeField] private string _runAnimation;
        [SerializeField] private string _slideAnimation;
        [SerializeField] private string _idleFireAnimation;
        [SerializeField] private string _hurtAnimation;
        private string _currentAnimation;
        private bool _sliding;

        private CoroutineHandle _handle;
        private CoroutineHandle _idleFireHandle;
        private float _fireCooldownTimer;

        private void OnEnable()
        {
            _chadder.StartedSlide.AddListener(OnStartedSlide);
            _chadder.EndedSlide.AddListener(OnSlideEnded);
            _buster.ShotFired.AddListener(OnShotFired);
        }

        private void OnDisable()
        {
            _chadder.StartedSlide.RemoveListener(OnStartedSlide);
            _chadder.EndedSlide.RemoveListener(OnSlideEnded);
            _buster.ShotFired.RemoveListener(OnShotFired);
        }

        private IEnumerator<float> Fire()
        {
            _normalArm.enabled = false;
            _busterArm.enabled = true;
            while (_fireCooldownTimer < _fireCooldownTime)
            {
                _fireCooldownTimer += Time.deltaTime;
                yield return Timing.WaitForOneFrame;
            }
            _busterArm.enabled = false;
            _normalArm.enabled = true;
            var time = _bodyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            _busterAnimator.Play(_currentAnimation, 0, time);
        }

        private void Update()
        {
            if (_health.Invincible && _playerInput.IsConsumed && _health.CurrentHealth < _health.MaxHealth)
            {
                Play(_hurtAnimation);
                return;
            }

            if (_character.Grounded == false)
            {
                Play(_jumpAnimation);
                return;
            }

            if (_sliding)
            {
                Play(_slideAnimation);
                return;
            }

            if (_rigidbody.velocity.x != 0)
                Play(_runAnimation);
            else
                Play(_idleAnimation);
        }

        private void Play(string name, float time = -1)
        {
            _currentAnimation = name;
            if (_idleFireHandle.IsRunning)
                return;
            if (time == -1)
            {
                _bodyAnimator.Play(_currentAnimation);
                _busterAnimator.Play(_currentAnimation);
                return;
            }
            _bodyAnimator.Play(_currentAnimation, 0, time);
            _busterAnimator.Play(_currentAnimation, 0, time);
        }

        private void OnShotFired()
        {
            _fireCooldownTimer = 0;
            _busterAnimator.Play(_currentAnimation, 0, 0);
            if (!_handle.IsRunning)
                _handle = Timing.RunCoroutine(Fire());
        }

        private void OnStartedSlide()
        {
            _sliding = true;
        }

        private void OnSlideEnded()
        {
            _sliding = false;
        }
    }
}
