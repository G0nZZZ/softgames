using UnityEngine;

namespace AceOfShadows
{
    using System.Collections.Generic;

    /// <summary>
    /// Service for loading and distributing cards into stacks.
    /// </summary>
    public interface ICardService
    {
        /// <summary>
        /// Initializes the card decks by creating the specified number of stacks,
        /// loading all card sprites, and distributing cards evenly.
        /// </summary>
        /// <param name="stackCount">The number of stacks to create.</param>
        /// <returns>An enumerable of initialized StackModel instances.</returns>
        IEnumerable<StackModel> Initialize(int stackCount);
    }

}

