using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Assets.Objects.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy _enemyToSpawn;
        [SerializeField] private Camera _camera;
        [SerializeField] private float _distanceOutsideOfCamera;
        [SerializeField] private bool _infinite;
        [SerializeField, ShowIf("_infinite")] private float _spawnInterval;
        private float _timer;

        private LinkedList<Transform> _spawnPositions = new LinkedList<Transform>();

        private void Awake()
        {
            if (_infinite == true)
                return;
            foreach (Transform item in transform)
            {
                _spawnPositions.AddLast(item);
            }
        }

        private void Update()
        {
            if (_infinite)
            {
                var screenPosition = _camera.WorldToViewportPoint(transform.position); 
                if (screenPosition.x <= _distanceOutsideOfCamera && screenPosition.x > 0)
                {
                    _timer += Time.deltaTime;
                    if (_timer >= _spawnInterval)
                    {
                        Spawn(transform.position, transform.rotation);
                        _timer = 0;
                    }
                }
                return;
            }
            foreach (Transform point in _spawnPositions)
            {
                var screenPosition = _camera.WorldToViewportPoint(point.position);
                if (screenPosition.x <= _distanceOutsideOfCamera)
                {
                    Spawn(point.position, point.rotation);
                    _spawnPositions.Remove(point);
                    return;
                }
            }
        }

        private void Spawn(Vector3 position, Quaternion rotation)
        {
            Instantiate(_enemyToSpawn, position, rotation);
        }

    }
}
