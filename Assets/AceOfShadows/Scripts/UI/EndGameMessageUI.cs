using AceOfShadows.Controllers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AceOfShadows.UI
{
    /// <summary>
    /// Loads and displays an end-game message banner via Addressables when all card animations are finished.
    /// </summary>
    public class EndGameMessageUI : MonoBehaviour
    {
        [Tooltip("Addressable key or label for the banner prefab.")]
        [SerializeField] private string bannerAddress;

        private GameObject bannerInstance;
        private AsyncOperationHandle<GameObject>? loadHandle;
        private DeckController deckController;

        void Start()
        {
            // Resolve GameManager for event subscription
            deckController = ServiceLocator.Resolve<DeckController>();
            if (deckController != null)
                deckController.OnAllAnimationsComplete += ShowBanner;

            // Begin loading the banner prefab
            if (!string.IsNullOrEmpty(bannerAddress))
            {
                loadHandle = Addressables.LoadAssetAsync<GameObject>(bannerAddress);
                loadHandle.Value.Completed += OnBannerLoaded;
            }
        }

        void OnDestroy()
        {
            // Unsubscribe event
            if (deckController != null)
                deckController.OnAllAnimationsComplete -= ShowBanner;

            // Release Addressables handle and instance
            if (loadHandle.HasValue)
            {
                Addressables.Release(loadHandle.Value);
                loadHandle = null;
            }
            if (bannerInstance != null)
            {
                Addressables.ReleaseInstance(bannerInstance);
                bannerInstance = null;
            }
        }

        /// <summary>
        /// Called when the banner prefab is loaded. Instantiates and hides the banner.
        /// </summary>
        /// <param name="handle">Async load handle.</param>
        private void OnBannerLoaded(AsyncOperationHandle<GameObject> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                // Instantiate as child of this UI root
                bannerInstance = Instantiate(handle.Result, transform);
                bannerInstance.SetActive(false);
            }
            else
            {
                Debug.LogError($"Failed to load banner at address '{bannerAddress}'.");
            }
        }

        /// <summary>
        /// Activates the end-game banner when all animations are complete.
        /// </summary>
        private void ShowBanner()
        {
            Debug.Log("Showing banner");
            if (bannerInstance != null)
                bannerInstance.SetActive(true);
        }
    }
}
