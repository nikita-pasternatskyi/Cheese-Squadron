using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Misc
{
    public class FlickerGameObject : MonoBehaviour
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private float _rate;

        private void Start()
        {
            Timing.RunCoroutine(Flicker());
        }

        private IEnumerator<float> Flicker()
        {
            float waitTime = 1 /(1 + _rate);
            while (true)
            {
                yield return Timing.WaitForSeconds(waitTime);
                if (_gameObject == null)
                    yield break;
                _gameObject.SetActive(!_gameObject.activeSelf);
            }
        }
    }
}
