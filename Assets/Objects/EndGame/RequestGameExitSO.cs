using System;
using UnityEngine;

namespace Assets.Objects.EndGame
{
    [CreateAssetMenu(menuName ="Game/Request Game Exit SO")]
    public class RequestGameExitSO : ScriptableObject
    {
        public event Action RequestGameExit;

        public void RequestExit()
        {
            RequestGameExit?.Invoke();
        }
    }
}
