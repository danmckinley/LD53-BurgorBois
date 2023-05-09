using System;
using UnityEngine;
using UnityEngine.Events;

namespace GUI
{
    [Serializable] public class PausedEvent : UnityEvent<bool>{}

    public class Pauser : MonoBehaviour
    {
        [SerializeField] public PausedEvent paused;

        private PlayerInput _input;

        private void Awake()
        {
            _input = new PlayerInput();
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.GUI.Pause.performed += _ => TogglePause();
        }

        private void OnDestroy()
        {
            _input.Dispose();
        }

        public void TogglePause()
        {
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0;
                paused.Invoke(true);
            }
            else
            {
                Time.timeScale = 1;
                paused.Invoke(false);
            }
        }
    }
}