using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DunkShot
{
    public class SceneLoader : MonoBehaviour
    {
        public void Load(string sceneName, Action onLoaded = null)
        {
            StartCoroutine(LoadScene(sceneName, onLoaded));
        }

        public void Restart()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            Load(currentSceneName);
        }

        private IEnumerator LoadScene(string nextScene, Action onLoaded = null)
        {
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
                yield return null;
      
            onLoaded?.Invoke();
        }
    }
}