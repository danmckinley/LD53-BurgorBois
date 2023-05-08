using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GUI
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] public string level;

        public void LoadScene()
        {
            SceneManager.LoadScene(level);
        }

        private IEnumerator LoadSceneAsync()
        {
            var sceneCurrent = SceneManager.GetActiveScene().buildIndex;

            var loadOperation = SceneManager.LoadSceneAsync(level);
            while (!loadOperation.isDone)
            {
                yield return null;
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(level));
            SceneManager.UnloadSceneAsync(sceneCurrent);
        }
    }
}