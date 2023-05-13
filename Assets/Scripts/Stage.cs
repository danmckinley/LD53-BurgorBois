using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{
    [SerializeField] public UnityEvent finished;
    [SerializeField] public UnityEvent<bool> pausedChanged;

    private bool _finished;

    public void LoadScene(string scene)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(scene);
    }

    public void Delivered()
    {
        var landingPads = GameObject.FindGameObjectsWithTag("LandingPad");
        var anyActiveLandingPads = landingPads.Any(landingPad => landingPad.activeSelf);

        if (!anyActiveLandingPads)
        {
            Finish();
        }
    }

    public void Finish()
    {
        _finished = true;
        Time.timeScale = 0;
        finished?.Invoke();
    }

    public bool Paused()
    {
        return Time.timeScale == 0;
    }

    public void Paused(bool paused)
    {
        if (_finished) return;

        Time.timeScale = paused ? 0 : 1;
        pausedChanged?.Invoke(paused);
    }

    public void TogglePause()
    {
        Paused(!Paused());
    }

    /**
     * Input event handler
     */
    public void OnPause()
    {
        TogglePause();
    }
}