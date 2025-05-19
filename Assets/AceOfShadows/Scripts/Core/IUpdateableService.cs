using UnityEngine;

namespace AceOfShadows
{
    /// <summary>
    /// Contract for services that require periodic updates each frame or tick.
    /// </summary>
    public interface IUpdatableService
    {
        /// <summary>
        /// Called with the delta time since the last update.
        /// </summary>
        void Tick(float deltaTime);
    }

}

