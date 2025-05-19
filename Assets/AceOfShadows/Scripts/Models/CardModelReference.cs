using UnityEngine;
using AceOfShadows;

namespace AceOfShadows.Controllers
{
    /// <summary>
    /// Helper component to hold a reference to the CardModel on a card prefab.
    /// </summary>
    public class CardModelReference : MonoBehaviour
    {
        /// <summary>
        /// The model backing this view instance.
        /// </summary>
        public CardModel Model;
    }
}