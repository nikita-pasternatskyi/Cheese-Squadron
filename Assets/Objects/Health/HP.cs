using MykroFramework.Runtime.Objects.Weaponry;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Objects.Health
{
    public class HP : MonoBehaviour, IDamageable, IHealable
    {
        [SerializeField] private int _maxHealth;
        public bool Invincible;

        private bool _destroyed;
        public event Action Died;
        public event Action<int, Vector2, int> GotHit;
        public event Action<float> HealthChangedPercentage;
        public event Action<int> Healed;

        public UnityEvent<int> UnityEvent_Healed;
        public UnityEvent<int, Vector2, int> UnityEvent_GotHit;
        public UnityEvent<float> UnityEvent_HealthChangedPercentage;
        public UnityEvent UnityEvent_Died;

        public int MaxHealth => _maxHealth;
        public int CurrentHealth { get; private set; }

        private void Start()
        {
            CurrentHealth = _maxHealth;
        }

        private void OnEnable()
        {
            Died += UnityEvent_Died.Invoke;
            HealthChangedPercentage += UnityEvent_HealthChangedPercentage.Invoke;
            GotHit += UnityEvent_GotHit.Invoke;
            Healed += UnityEvent_Healed.Invoke;
        }

        private void OnDisable()
        {
            Died -= UnityEvent_Died.Invoke;
            HealthChangedPercentage -= UnityEvent_HealthChangedPercentage.Invoke;
            GotHit -= UnityEvent_GotHit.Invoke;
            Healed -= UnityEvent_Healed.Invoke;
        }

        public void Heal(int heal)
        {
            _destroyed = false;
            CurrentHealth = Mathf.Clamp(CurrentHealth + heal, 0, _maxHealth);
            HealthChangedPercentage?.Invoke(CurrentHealth);
            Healed?.Invoke(CurrentHealth);
        }

        public void TakeDamage(int damage, Vector2 direction)
        {
            if (_destroyed)
                return;
            if (Invincible)
                return;
            if (CurrentHealth - damage <= 0)
            {
                HealthChangedPercentage?.Invoke(0);
                GotHit?.Invoke(0, direction, damage);
                Died?.Invoke();
                _destroyed = true;
            }
            else
            {
                CurrentHealth -= damage;
                HealthChangedPercentage?.Invoke((float)CurrentHealth / _maxHealth);
                GotHit?.Invoke(CurrentHealth, direction, damage);
            }
        }

        public void InstantKill()
        {
            if (_destroyed)
                return;
            CurrentHealth = 0;
            HealthChangedPercentage?.Invoke(0);
            GotHit?.Invoke(0, Vector2.zero, 100);
            Died?.Invoke();
            _destroyed = true;
        }
    }
}
