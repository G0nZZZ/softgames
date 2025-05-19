using UnityEngine;
using System;

namespace AceOfShadows
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a stack of CardModel instances in LIFO order.
    /// Raises events when cards are pushed or popped.
    /// </summary>
    public class StackModel
    {
        private readonly Stack<CardModel> cards = new Stack<CardModel>();

        /// <summary>
        /// Number of cards currently in the stack.
        /// </summary>
        public int Count => cards.Count;

        /// <summary>
        /// Event fired when a card is pushed onto the stack.
        /// </summary>
        public event Action<CardModel> CardPushed;

        /// <summary>
        /// Event fired when a card is popped from the stack.
        /// </summary>
        public event Action<CardModel> CardPopped;

        /// <summary>
        /// Pushes a card onto the top of the stack.
        /// </summary>
        public void Push(CardModel card)
        {
            Debug.Log("Push");
            cards.Push(card);
            CardPushed?.Invoke(card);
        }

        /// <summary>
        /// Pops the top card from the stack.
        /// </summary>
        public CardModel Pop()
        {
            var card = cards.Pop();
            CardPopped?.Invoke(card);
            return card;
        }
        
        /// <summary>
        /// Returns the top card without removing it.
        /// </summary>
        public CardModel Peek()
        {
            return cards.Peek();
        }
    }
}