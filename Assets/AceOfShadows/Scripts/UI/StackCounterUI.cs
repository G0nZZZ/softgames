using UnityEngine;
using UnityEngine.UI;
using AceOfShadows;
using TMPro;

/// <summary>
/// UI component that displays the count of cards in a StackModel.
/// </summary>
public class StackCounterUI : MonoBehaviour
{
    [Tooltip("Text component used to display the stack count.")]
    [SerializeField] private TextMeshPro counterText;

    private StackModel model;

    /// <summary>
    /// Binds this UI component to a StackModel, initializing the display and subscribing to updates.
    /// </summary>
    /// <param name="stackModel">The StackModel to observe.</param>
    public void Bind(StackModel stackModel)
    {
        if (model != null)
        {
            // Unsubscribe from previous model events
            model.CardPushed -= OnCardChanged;
            model.CardPopped -= OnCardChanged;
        }

        model = stackModel;
        if (model == null || counterText == null)
            return;

        // Subscribe to model changes
        model.CardPushed += OnCardChanged;
        model.CardPopped += OnCardChanged;

        // Initialize display
        UpdateCounter();
    }

    private void OnDestroy()
    {
        if (model != null)
        {
            model.CardPushed -= OnCardChanged;
            model.CardPopped -= OnCardChanged;
        }
    }

    /// <summary>
    /// Handler for push/pop events to refresh the UI.
    /// </summary>
    /// <param name="card">The card that was pushed or popped (unused).</param>
    private void OnCardChanged(CardModel card)
    {
        UpdateCounter();
    }

    /// <summary>
    /// Updates the text to reflect the current count.
    /// </summary>
    private void UpdateCounter()
    {
        Debug.Log("Updated counter");
        counterText.text = model != null ? model.Count.ToString() : "0";
    }
}