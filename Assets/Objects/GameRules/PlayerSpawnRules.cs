using Assets.Objects.Player;
using Assets.Objects.Spawnpoint;
using MykroFramework.Runtime.Objects.Player2D;
using MykroFramework.Runtime.SceneLoading;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Objects.GameRules
{
    public class PlayerSpawnRules : MonoBehaviour
    {
        [SerializeField] private CharacterSpawnSO _spawnSO;
        private PlayerBase _player;

        [Button]
        private void Reload()
        {
            SceneLoader.Instance.ReloadScene();
        }

        private void OnEnable()
        {
            _spawnSO.EntryPointRegistered += OnNewEntryPoint;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnDisable()
        {
            _spawnSO.EntryPointRegistered -= OnNewEntryPoint;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnNewEntryPoint(CharacterSpawn obj)
        {
            if (_player == null)
            {
                _player = obj.Spawn();
            }
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if(arg1 == LoadSceneMode.Single)
                _player = null;
        }
    }
}
