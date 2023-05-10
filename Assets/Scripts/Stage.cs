using UnityEngine;
using UnityEngine.Events;

public class Stage : MonoBehaviour
{
    [SerializeField] public UnityEvent finished;
    [SerializeField] public UnityEvent<bool> pausedChanged;

    private bool _finished;

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