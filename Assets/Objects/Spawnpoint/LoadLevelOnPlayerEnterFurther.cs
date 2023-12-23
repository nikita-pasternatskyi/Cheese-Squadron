using Assets.Objects.Player;
using MykroFramework.Runtime.SceneLoading;
using Udar.SceneManager;
using UnityEngine;

namespace Assets.Objects.Spawnpoint
{
    public class LoadLevelOnPlayerEnterFurther : MonoBehaviour
    {   
        [SerializeField] private SceneField _scene;
        private bool _loaded; 
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_loaded)
                return;
            if (collision.TryGetComponent<PlayerBase>(out PlayerBase playerBase))
            {
                SceneLoader.Instance.LoadSceneAdditively(_scene, true);
                _loaded = true;
            }
        }
    }
}
