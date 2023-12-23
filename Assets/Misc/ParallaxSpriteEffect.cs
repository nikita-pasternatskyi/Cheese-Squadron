using UnityEngine;

namespace Assets.Misc
{
    //thanks to https://www.youtube.com/watch?v=wBol2xzxCOU
    public class ParallaxSpriteEffect : MonoBehaviour
    {
        [SerializeField] private Vector2 _multiplier;
        [SerializeField] private Transform _camera;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private bool _infinite = true;

        private Vector3 _lastCameraPosition;
        private float _textureUnitSizeX;

        private void Start()
        {
            _lastCameraPosition = _camera.transform.position;
            Texture2D texture = _sprite.texture;
            _textureUnitSizeX = texture.width / _sprite.pixelsPerUnit;
        }

        private void LateUpdate()
        {
            Vector3 delta = _camera.transform.position - _lastCameraPosition;
            transform.position += new Vector3(delta.x * _multiplier.x, delta.y * _multiplier.y, 0);
            _lastCameraPosition = _camera.transform.position;

            if (!_infinite)
                return;
            if (Mathf.Abs(_camera.transform.position.x - transform.position.x) >= _textureUnitSizeX)
            {
                float offsetPositionX = (_camera.transform.position.x - transform.position.x) % _textureUnitSizeX;
                transform.position = new Vector3(_camera.transform.position.x + offsetPositionX, transform.position.y);
            }
        }

    }
}
