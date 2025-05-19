using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AceOfShadows
{
    /// <summary>
    /// Concrete implementation of ICardService.
    /// Loads card sprites once and manages a central deck plus target stacks.
    /// </summary>
    public class CardService : ICardService
    {
        private List<CardModel> cardModels;
        private List<StackModel> stacks;

        /// <summary>
        /// Initializes a single deck of all cards plus the specified number of empty target stacks.
        /// Subsequent calls return the same stacks.
        /// </summary>
        /// <param name="targetStackCount">Number of target stacks (excluding the deck).</param>
        public IEnumerable<StackModel> Initialize(int targetStackCount)
        {
            // If already initialized, return existing list
            if (stacks != null)
                return stacks;

            // Load all sprites from Resources/Sprites/Cards
            var sprites = Resources.LoadAll<Sprite>("Sprites/Cards");
            Debug.Log($"Loaded {sprites.Length} card sprites.");

            // Create CardModel instances
            cardModels = sprites.Select((sprite, index) => new CardModel
            {
                Id = index,
                Sprite = sprite
            }).ToList();

            // Create a central deck and push all cards onto it
            var deck = new StackModel();
            foreach (var card in cardModels)
            {
                deck.Push(card);
                card.CurrentStack = deck;
            }

            // Create empty target stacks
            var targets = Enumerable.Range(0, targetStackCount)
                                    .Select(_ => new StackModel())
                                    .ToList();

            // Cache: first element is deck, then targets
            stacks = new List<StackModel> { deck };
            stacks.AddRange(targets);

            Debug.Log($"CardService initialized with 1 deck and {targetStackCount} target stacks.");
            return stacks;
        }
    }
}
