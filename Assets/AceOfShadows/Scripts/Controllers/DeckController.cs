using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AceOfShadows.Controllers
{
    /// <summary>
    /// Moves cards from a central deck to random stacks at a fixed interval with animation.
    /// Pops and pushes only after animation completes to prevent premature view destruction.
    /// </summary>
    public class DeckController : MonoBehaviour
    {
        [SerializeField] private int targetStackCount = 4;
        [SerializeField] private float moveInterval = 1f;

        private List<StackModel> stacks;
        private StackModel deck;
        private IAnimationService animationService;
        private GameManager gameManager;
        private Dictionary<int, GameObject> cardMap;
        private List<StackController> stackControllers;

        private int moveCount;
        private int totalMoves;

        /// <summary>
        /// Event fired when all animations have completed.
        /// </summary>
        public event Action OnAllAnimationsComplete;

        private void Awake()
        {
            ServiceLocator.Register<DeckController>(this);
        }

        void Start()
        {
            // Resolve services and game manager
            var cardService = ServiceLocator.Resolve<ICardService>();
            animationService = ServiceLocator.Resolve<IAnimationService>();
            gameManager = ServiceLocator.Resolve<GameManager>();

            // Initialize stacks and deck
            stacks = cardService.Initialize(targetStackCount).ToList();
            deck = stacks[0];

            // Record total moves to perform
            totalMoves = deck.Count;
            moveCount = 0;

            // Cache controllers (populate views in their Start)
            stackControllers = FindObjectsOfType<StackController>()
                                .OrderBy(c => c.StackIndex)
                                .ToList();

            // Initialize cardMap after one frame to ensure StackControllers have populated their CardInstances
            StartCoroutine(InitializeAndBeginMoves());
        }

        private IEnumerator InitializeAndBeginMoves()
        {
            // Wait a frame for StackController.Start to finish instantiating cards
            yield return null;

            // Build map from card ID to its GameObject view
            cardMap = new Dictionary<int, GameObject>();
            foreach (var controller in stackControllers)
            {
                foreach (var (model, go) in controller.CardInstances)
                    cardMap[model.Id] = go;
            }

            // Start the move loop
            InvokeRepeating(nameof(MoveOneCard), moveInterval, moveInterval);
        }

        private void MoveOneCard()
        {
            // Increment move counter and check for end of game
            moveCount++;
            if (moveCount >= totalMoves)
            {
                CancelInvoke(nameof(MoveOneCard));
                OnAllAnimationsComplete?.Invoke();
                return;
            }

            // If no cards left in deck, bail early
            if (deck.Count == 0)
            {
                CancelInvoke(nameof(MoveOneCard));
                return;
            }

            // Peek top card without removing
            var card = deck.Peek();

            // Choose a random target stack (skip deck index 0)
            int targetIndex = Random.Range(1, stacks.Count);
            var target = stacks[targetIndex];

            // Animate then update model and view
            if (cardMap.TryGetValue(card.Id, out var go))
            {
                var destAnchor = stackControllers[targetIndex].CardAnchor;
                animationService.Move(go, destAnchor, () =>
                {
                    // Update model
                    deck.Pop();
                    target.Push(card);
                    card.CurrentStack = target;

                    // Update visuals
                    var sourceController = stackControllers[0];
                    var destController = stackControllers[targetIndex];
                    sourceController.MoveCardView(card, destController);
                });
            }
        }
    }
}
