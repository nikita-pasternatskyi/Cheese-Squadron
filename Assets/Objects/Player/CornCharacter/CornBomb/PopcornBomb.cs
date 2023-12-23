using MykroFramework.Runtime.Cameras;
using MykroFramework.Runtime.Objects.Weaponry;
using UnityEngine;

namespace Assets.Objects.Player.CornCharacter.CornBomb
{
    public class PopcornBomb : MonoBehaviour
    {
        [SerializeField] private FloatLimits _size;
        [SerializeField] private float _growthTime;
        [SerializeField] private LayerMask _collidable;
        [SerializeField] private int _damagePerFrame;
        private float _currentSize;
        private Collider2D[] _allObjects = new Collider2D[16];
        private float _lifeTime;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _size.Min);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _size.Max);
        }

        private void FixedUpdate()
        {
            if (_lifeTime >= _growthTime)
                Destroy(gameObject);
            _lifeTime += Time.deltaTime;
            _currentSize = Mathf.Lerp(_size.Min, _size.Max, _lifeTime / _growthTime);
            transform.localScale = new Vector3(_currentSize, _currentSize, _currentSize);
            int objectNum = Physics2D.OverlapCircleNonAlloc(transform.position, _currentSize, _allObjects, _collidable);
            if (objectNum > 0)
            {
                for (int i = 0; i < objectNum; i++)
                {
                    if (_allObjects[i].TryGetComponent<IDamageable>(out IDamageable damageable))
                    {
                        damageable.TakeDamage(_damagePerFrame, -(transform.position - _allObjects[i].transform.position).normalized);
                    }
                }
            }
        }
    }
}
