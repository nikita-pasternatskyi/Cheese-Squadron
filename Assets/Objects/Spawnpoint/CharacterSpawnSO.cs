using System;
using UnityEngine;

namespace Assets.Objects.Spawnpoint
{
    [CreateAssetMenu(menuName = "Game/Character Spawn")]
    public class CharacterSpawnSO : ScriptableObject
    {
        public GameObject CharacterPrefab;
        public event Action<CharacterSpawn> EntryPointRegistered;

        public void ChangeCharacter(GameObject c)
        {
            CharacterPrefab = c;
        }

        public void RegisterEntryPoint(CharacterSpawn spawn)
        {
            EntryPointRegistered?.Invoke(spawn);
        }
    }
}
