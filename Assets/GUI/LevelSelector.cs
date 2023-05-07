using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] public string level;
    
    public void LoadLevel()
    {
        SceneManager.LoadScene(level);
    }
}
