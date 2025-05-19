using System;
using UnityEngine;

namespace AceOfShadows
{
    /// <summary>
    /// Bootstraps services and tracks global game state.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Service for card management.
        /// </summary>
        private ICardService cardService;

        /// <summary>
        /// Service for handling animations.
        /// </summary>
        private IAnimationService animationService;
        private IUpdatableService animationTick;



        void Awake()
        {
            // Register core services
            ServiceLocator.Register<ICardService>(new CardService());
            var animSvc = new AnimationService();
            ServiceLocator.Register<IAnimationService>(animSvc);
            ServiceLocator.Register<IUpdatableService>(animSvc);
            ServiceLocator.Register<GameManager>(this);
        }

        void Start()
        {
            // Resolve services to verify registration
            cardService = ServiceLocator.Resolve<ICardService>();
            animationService = ServiceLocator.Resolve<IAnimationService>();
            animationTick = ServiceLocator.Resolve<IUpdatableService>();
            
            Debug.Assert(cardService != null, "ICardService not registered");
            Debug.Assert(animationService != null, "IAnimationService not registered");
        }

        private void Update()
        {
            animationTick.Tick(Time.deltaTime);
        }


    }

}
