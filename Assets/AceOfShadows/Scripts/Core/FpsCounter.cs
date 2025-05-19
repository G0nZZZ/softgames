using TMPro;
using UnityEngine;

namespace AceOfShadows.Core
{
    /// <summary>
    /// Calculates and displays the current frames-per-second in the top-left corner.
    /// Attach this to a UI Text under a Canvas and assign the Text component.
    /// </summary>
    public class FpsCounter : MonoBehaviour
    {
        [Tooltip("UI Text component to display the FPS value.")]
        [SerializeField] private TextMeshProUGUI fpsText;

        [Tooltip("Interval in seconds at which FPS value is updated.")]
        [SerializeField] private float updateInterval = 0.5f;

        private int framesCount;
        private float elapsedTime;

        void Update()
        {
            framesCount++;
            elapsedTime += Time.unscaledDeltaTime;

            if (elapsedTime >= updateInterval)
            {
                float fps = framesCount / elapsedTime;
                if (fpsText != null)
                {
                    fpsText.text = Mathf.RoundToInt(fps) + " FPS";
                }
                // Reset counters
                framesCount = 0;
                elapsedTime = 0f;
            }
        }
    }
}