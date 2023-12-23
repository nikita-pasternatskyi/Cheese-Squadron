using Assets.Objects.Spawnpoint;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Misc
{
    public class ChooseSpriteBasedOnCharacter : SerializedMonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private CharacterSpawnSO _characterSpawnSO;

        [SerializeField] private Dictionary<GameObject, Sprite> _sprites;

        public void ChangeSprite()
        {
            if (_sprites.TryGetValue(_characterSpawnSO.CharacterPrefab, out Sprite newSprite))
            {
                _spriteRenderer.sprite = newSprite;
            }
        }
    }
}
