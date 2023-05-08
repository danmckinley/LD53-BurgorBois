using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace GUI
{
    public class FinishMenu : MonoBehaviour
    {
        [SerializeField] public UnityEvent restartButtonClicked;
        [SerializeField] public UnityEvent mainMenuButtonClicked;

        // Only for initialisation
        [SerializeField] private bool visible = true;

        private UIDocument _uiDocument;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            Visible(false);
        }

        private void OnEnable()
        {
            var restartButton = _uiDocument.rootVisualElement.Q("RestartButton") as Button;
            if (restartButton != null)
            {
                restartButton.clicked += () => restartButtonClicked?.Invoke();
            }
            else
            {
                Debug.LogWarning("`RestartButton` cannot be found in `PauseMenu`");
            }

            var mainMenuButton = _uiDocument.rootVisualElement.Q("MainMenuButton") as Button;
            if (mainMenuButton != null)
            {
                mainMenuButton.clicked += () => mainMenuButtonClicked?.Invoke();
            }
            else
            {
                Debug.LogWarning("`MainMenuButton` cannot be found in `PauseMenu`");
            }

            Visible(true);
        }

        public void Visible(bool value)
        {
            visible = value;
            _uiDocument.rootVisualElement.visible = visible;
        }
    }
}