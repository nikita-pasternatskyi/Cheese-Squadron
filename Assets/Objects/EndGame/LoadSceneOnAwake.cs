using MykroFramework.Runtime.SceneLoading;
using Udar.SceneManager;
using UnityEngine;

namespace Assets.Objects.EndGame
{
    public class LoadSceneOnAwake : MonoBehaviour
    {
        [SerializeField ]private SceneField _sceneToLoad;

        private void Awake()
        {
            SceneLoader.Instance.LoadScene(_sceneToLoad);
        }
    }
}
