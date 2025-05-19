using UnityEngine;
using System;
using System.Collections.Generic;

namespace AceOfShadows
{
    /// <summary>
    /// Concrete implementation of IAnimationService, also implements IUpdatableService to drive animations.
    /// Uses linear interpolation over a fixed duration.
    /// </summary>
    public class AnimationService : IAnimationService, IUpdatableService
    {
        private class Tween
        {
            public GameObject Card;
            public Transform Target;
            public Action OnComplete;
            public Vector3 StartPos;
            public float Duration;
            public float Elapsed;
        }

        private readonly List<Tween> tweens = new List<Tween>();
        private const float DefaultDuration = 0.25f;

        /// <summary>
        /// Starts moving the card GameObject to the target transform over DefaultDuration seconds.
        /// </summary>
        public void Move(GameObject card, Transform target, Action onComplete)
        {
            if (card == null || target == null)
            {
                Debug.Log("Card or Target is null");
                onComplete?.Invoke();
                return;
            }

            Debug.Log("Move Card");
            var tween = new Tween
            {
                Card = card,
                Target = target,
                OnComplete = onComplete,
                StartPos = card.transform.position,
                Duration = DefaultDuration,
                Elapsed = 0f
            };

            tweens.Add(tween);
            Debug.Log($"Tweens Count {tweens.Count}");
        }

        /// <summary>
        /// Advances all active tweens by deltaTime. Should be called each frame.
        /// </summary>
        public void Tick(float deltaTime)
        {
            Debug.Log("Tick");
            for (int i = tweens.Count - 1; i >= 0; i--)
            {
                Debug.Log($"Tween {i}");
                var tween = tweens[i];
                tween.Elapsed += deltaTime;
                float t = Mathf.Clamp01(tween.Elapsed / tween.Duration);
                tween.Card.transform.position = Vector3.Lerp(tween.StartPos, tween.Target.position, t);

                if (t >= 1f)
                {
                    Debug.Log("Card Moved");
                    // Ensure final position is exact
                    tween.Card.transform.position = tween.Target.position;
                    tween.OnComplete?.Invoke();
                    tweens.RemoveAt(i);
                }
            }
        }
    }

}

