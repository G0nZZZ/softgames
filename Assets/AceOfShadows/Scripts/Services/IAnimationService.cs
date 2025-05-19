using System;
using UnityEngine;

namespace AceOfShadows
{
    /// <summary>
    /// Contract for services that handle smooth movement animations of card GameObjects.
    /// </summary>
    public interface IAnimationService
    {
        /// <summary>
        /// Moves the given card GameObject to the target transform smoothly, invoking onComplete when done.
        /// </summary>
        /// <param name="card">The card GameObject to animate.</param>
        /// <param name="target">The destination transform for the card.</param>
        /// <param name="onComplete">Callback invoked once the animation finishes.</param>
        void Move(GameObject card, Transform target, Action onComplete);
    }

}
