using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace GUI
{
    public class Credits : MonoBehaviour
    {
        [SerializeField] public UnityEvent backButtonClicked;

        // Only for initialisation
        [SerializeField] private bool visible = true;

        private UIDocument _uiDocument;

        private void OnEnable()
        {
            _uiDocument = GetComponent<UIDocument>();

            var backButton = _uiDocument.rootVisualElement.Q("BackButton") as Button;
            if (backButton != null)
            {
                backButton.clicked += () => backButtonClicked?.Invoke();
            }
            else
            {
                Debug.LogWarning("`BackButton` cannot be found in `MainMenu`");
            }

            Visible(visible);
        }

        public void Visible(bool value)
        {
            visible = value;
            _uiDocument.rootVisualElement.visible = visible;
        }
    }
}