using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace GUI
{
    public class ButtonMapping : MonoBehaviour
    {
        [SerializeField] private string buttonName;
        [SerializeField] private UnityEvent callback;

        private Button _button;
        private bool _connected;

        private void OnEnable()
        {
            Connect();
        }

        private void OnDisable()
        {
            Disconnect();
        }

        private void Connect()
        {
            if (_connected)
            {
                Debug.LogWarning($"`{buttonName}` already connected");
                return;
            }

            Button button;
            try
            {
                button = GetButton();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return;
            }

            button.clicked += InvokeCallback;
            _connected = true;
        }

        private void Disconnect()
        {
            if (!_connected)
            {
                return;
            }

            Button button;
            try
            {
                button = GetButton();
            }
            catch
            {
                // Assume the connection is disconnected
                _connected = false;
                return;
            }

            button.clicked -= InvokeCallback;
            _connected = false;
        }

        private Button GetButton()
        {
            var uiDocument = GetComponent<UIDocument>();
            if (uiDocument == null)
            {
                throw new Exception("`UIDocument` cannot be found");
            }

            var button = uiDocument.rootVisualElement?.Q(buttonName) as Button;
            if (button == null)
            {
                throw new Exception($"`{buttonName}` cannot be found");
            }

            return button;
        }

        private void InvokeCallback()
        {
            callback?.Invoke();
        }
    }
}