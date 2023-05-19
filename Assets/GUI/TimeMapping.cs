using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace GUI
{
    public class TimeMapping : MonoBehaviour
    {
        [SerializeField] private string labelName;
        [SerializeField] private string format = "mm\\:ss\\.fff";

        public double GetTime()
        {
            Label label;
            try
            {
                label = GetLabel();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return 0;
            }

            if (TimeSpan.TryParseExact(label.text, format, null, out var timeSpan))
            {
                return timeSpan.TotalSeconds;
            }

            return 0;
        }

        public void SetTime(double value)
        {
            Label label;
            try
            {
                label = GetLabel();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return;
            }

            var timeSpan = TimeSpan.FromSeconds(value);
            label.text = timeSpan.ToString(format);
        }

        private Label GetLabel()
        {
            var uiDocument = GetComponent<UIDocument>();
            if (uiDocument == null)
            {
                throw new Exception("`UIDocument` cannot be found");
            }

            var label = uiDocument.rootVisualElement?.Q(labelName) as Label;
            if (label == null)
            {
                throw new Exception($"`{labelName}` cannot be found");
            }

            return label;
        }
    }
}