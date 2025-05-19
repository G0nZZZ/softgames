using UnityEngine;
using System;
using MagicWords.Models;

namespace MagicWords.State
{
    /// <summary>
    /// Holds the current MagicWords data & loading/error state.
    /// Singleton MonoBehaviour; events fire when data loads or errors.
    /// </summary>
    public class MagicWordsState : MonoBehaviour
    {
        public static MagicWordsState Instance { get; private set; }

        public MagicWordsData Data { get; private set; }
        public bool IsLoading { get; private set; }
        public bool HasError  { get; private set; }

        public event Action OnDataLoaded;
        public event Action OnError;
        public event Action OnFinish;
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Toggle loading flag.
        /// </summary>
        public void SetLoading(bool loading)
        {
            IsLoading = loading;
        }

        /// <summary>
        /// Populate data, clear error/loading, and notify listeners.
        /// </summary>
        public void SetData(MagicWordsData data)
        {
            Data      = data;
            IsLoading = false;
            HasError  = false;
            Debug.Log("🟢 OnDataLoaded fired");
            OnDataLoaded?.Invoke();
        }

        /// <summary>
        /// Mark error, clear loading, and notify listeners.
        /// </summary>
        public void SetError()
        {
            HasError  = true;
            IsLoading = false;
            Debug.Log("🔴 OnError fired");
            OnError?.Invoke();
        }

        public void SetFinish()
        {
            HasError  = false;
            Debug.Log("🟢 SetFinish fired");
            OnFinish?.Invoke();
        }
    }
}