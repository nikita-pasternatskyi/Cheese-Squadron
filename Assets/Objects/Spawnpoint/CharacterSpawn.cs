using Assets.Objects.Player;
using MykroFramework.Runtime.Cameras.Camera2D;
using System;
using UnityEngine;

namespace Assets.Objects.Spawnpoint
{
    public class CharacterSpawn : MonoBehaviour
    {
        [SerializeField] private CharacterSpawnSO _characterToSpawn;
        [SerializeField] private FollowCamera2D _camera2D;
        public event Action<PlayerBase> PlayerSpawned;

        private void Start()
        {
            _characterToSpawn.RegisterEntryPoint(this);
        }

        public PlayerBase Spawn()
        {
            var go = Instantiate(_characterToSpawn.CharacterPrefab, transform.position, transform.rotation);
            var player = go.GetComponent<PlayerBase>();
            PlayerSpawned?.Invoke(player);
            _camera2D.Target = player.transform;
            return player;
        }
    }
}
