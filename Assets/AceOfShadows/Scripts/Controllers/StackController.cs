using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AceOfShadows.Controllers
{
    /// <summary>
    /// View/controller for a single StackModel. Instantiates/destroys Card.prefab views on push/pop.
    /// Positions initial cards from bottom to top so the top card remains visible.
    /// </summary>
    public class StackController : MonoBehaviour
    {
        [SerializeField] private Transform cardAnchor;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private int stackIndex;
        [SerializeField] private int stackCount = 4;

        private StackModel model;
        private readonly List<GameObject> cardInstances = new List<GameObject>();

        /// <summary>Index of this stack in the CardService.Initialize list.</summary>
        public int StackIndex => stackIndex;

        /// <summary>Anchor transform for this stack's cards.</summary>
        public Transform CardAnchor => cardAnchor;

        /// <summary>Enumerates CardModel and corresponding GameObject pairs.</summary>
        public IEnumerable<(CardModel Model, GameObject GameObject)> CardInstances
            => cardInstances.Select(go => (go.GetComponent<CardModelReference>().Model, go));

        private PoolService poolService;
        void Start()
        {
            var counterUI = GetComponent<StackCounterUI>();
            poolService = ServiceLocator.Resolve<PoolService>();
            // Resolve and bind model
            var cardService = ServiceLocator.Resolve<ICardService>();
            var stacks = cardService.Initialize(stackCount).ToList();
            model = stacks[stackIndex];
            counterUI.Bind(model);
            // Buffer all cards by popping them
            int initialCount = model.Count;
            var buffer = new List<CardModel>(initialCount);
            for (int i = 0; i < initialCount; i++)
            {
                buffer.Add(model.Pop());
            }

            // Reverse buffer so bottom card is first
            buffer.Reverse();

            // Instantiate from bottom to top
            for (int i = 0; i < buffer.Count; i++)
            {
                var card = buffer[i];
                var go = poolService.Get();
                go.transform.SetParent(cardAnchor, false);
                go.transform.localPosition = new Vector3(0, i * 0.02f, 0);

                var sr = go.GetComponent<SpriteRenderer>();
                sr.sprite = card.Sprite;
                sr.sortingOrder = i;

                var modelRef = go.GetComponent<CardModelReference>();
                modelRef.Model = card;

                cardInstances.Add(go);

                // Push back into model so order remains unchanged
                model.Push(card);
            }

            // Listen for live updates
            model.CardPushed += OnCardPushed;
            model.CardPopped += OnCardPopped;
        }

        // In StackController.cs, add:

        /// <summary>
        /// Moves an existing card view from this stack to a destination stack controller.
        /// </summary>
        public void MoveCardView(CardModel card, StackController destination)
        {
            // Find the GameObject for the card
            var tuple = cardInstances
                .Select(go => (go, go.GetComponent<CardModelReference>().Model))
                .FirstOrDefault(t => t.Model == card);
            if (tuple.go == null) return;

            // Remove from this stack
            cardInstances.Remove(tuple.go);

            // Reparent
            tuple.go.transform.SetParent(destination.cardAnchor);

            // Position in destination
            int idx = destination.model.Count; // after push
            tuple.go.transform.localPosition = new Vector3(0, idx * 0.02f, 0);
            tuple.go.GetComponent<SpriteRenderer>().sortingOrder = idx;

            // Add to destination list
            destination.cardInstances.Add(tuple.go);
        }

        
        private void OnCardPushed(CardModel card)
        {
            int idx = model.Count - 1;
            var go = Instantiate(cardPrefab, cardAnchor);
            go.transform.localPosition = new Vector3(0, idx * 0.02f, 0);

            var sr = go.GetComponent<SpriteRenderer>();
            sr.sprite = card.Sprite;
            sr.sortingOrder = idx;

            go.GetComponent<CardModelReference>().Model = card;
            cardInstances.Add(go);
        }

        private void OnCardPopped(CardModel card)
        {
            if (cardInstances.Count == 0)
                return;

            var go = cardInstances.Last();
            cardInstances.RemoveAt(cardInstances.Count - 1);
            poolService.Release(go);
            
        }
    }
}