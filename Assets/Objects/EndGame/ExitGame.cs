using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Objects.EndGame
{

    public class ExitGame : MonoBehaviour
    {
        [SerializeField] private RequestGameExitSO _gameExitChannelToListen;

        private void OnEnable()
        {
            _gameExitChannelToListen.RequestGameExit += Exit;
        }

        private void OnDisable()
        {
            _gameExitChannelToListen.RequestGameExit -= Exit;
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
