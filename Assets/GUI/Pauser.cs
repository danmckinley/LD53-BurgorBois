using UnityEngine;

namespace GUI
{
    public class Pauser : MonoBehaviour
    {
        public void TogglePause()
        {
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
}