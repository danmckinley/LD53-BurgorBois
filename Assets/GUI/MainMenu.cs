using System;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public UnityEvent startButtonClicked;
    [SerializeField] public UnityEvent quitButtonClicked;
    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        var startButton = uiDocument.rootVisualElement.Q("StartButton") as Button;
        if (startButton != null)
        {
            startButton.clicked += () => startButtonClicked?.Invoke();
        }
        else
        {
            Debug.LogWarning("`StartButton` cannot be found in `MainMenu`");
        }
        
        var quitButton = uiDocument.rootVisualElement.Q("QuitButton") as Button;
        if (quitButton != null)
        {
            quitButton.clicked += () => quitButtonClicked?.Invoke();
        }
        else
        {
            Debug.LogWarning("`QuitButton` cannot be found in `MainMenu`");
        }
    }
}
