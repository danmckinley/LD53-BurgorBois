using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace GUI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] public UnityEvent resumeButtonClicked;
        [SerializeField] public UnityEvent mainMenuButtonClicked;

        private UIDocument _uiDocument;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            Visible(false);
        }

        private void OnEnable()
        {
            var resumeButton = _uiDocument.rootVisualElement.Q("ResumeButton") as Button;
            if (resumeButton != null)
            {
                resumeButton.clicked += () => resumeButtonClicked?.Invoke();
            }
            else
            {
                Debug.LogWarning("`ResumeButton` cannot be found in `PauseMenu`");
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
        }

        public void Visible(bool visible)
        {
            _uiDocument.rootVisualElement.visible = visible;
        }
    }
}
