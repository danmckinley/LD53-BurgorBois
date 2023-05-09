using UnityEngine;
using UnityEngine.UIElements;

namespace GUI
{
    public class Visibility : MonoBehaviour
    {
        [SerializeField] private bool visible = true;

        private UIDocument _uiDocument;

        private void Start()
        {
            _uiDocument = GetComponent<UIDocument>();
            Visible(visible);
        }

        public void Visible(bool value)
        {
            visible = value;
            _uiDocument.rootVisualElement.visible = visible;
        }
    }
}