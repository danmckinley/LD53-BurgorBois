using UnityEngine;
using UnityEngine.SceneManagement;

namespace GUI
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }
    }
}