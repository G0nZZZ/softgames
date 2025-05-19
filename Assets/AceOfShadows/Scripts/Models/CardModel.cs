using UnityEngine;

namespace AceOfShadows
{
    /// <summary>
    /// Data model representing a single card in the deck.
    /// </summary>
    public class CardModel
    {
        /// <summary>
        /// Unique identifier for the card.
        /// </summary>
        public int Id { get; set; }
    
        /// <summary>
        /// Sprite to be rendered for this card.
        /// </summary>
        public Sprite Sprite { get; set; }
    
        /// <summary>
        /// Reference to the stack this card currently belongs to.
        /// </summary>
        public StackModel CurrentStack { get; set; }
    }

}
