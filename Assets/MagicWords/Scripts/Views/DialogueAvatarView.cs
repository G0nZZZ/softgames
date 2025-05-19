using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using AceOfShadows;
using UnityEngine.Networking;
using TMP = TMPro.TextMeshProUGUI;
using MagicWords.State;
using MagicWords.Models;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace MagicWords.Views
{
    /// <summary>
    /// Listens for MagicWordsState.OnDataLoaded and, for each DialogueEntry,
    /// instantiates a DialogueLine prefab under contentParent, linking to its AvatarEntry,
    /// and parses inline emoji codes like {intrigued} into Unicode.
    /// </summary>
    public class DialogueAvatarView : MonoBehaviour
    {
        [Tooltip("Parent under which to instantiate dialogue-avatar items")]
        [SerializeField] private RectTransform contentParent;

        [Tooltip("ScrollRect controlling the scrollable conversation view")]
        [SerializeField] private ScrollRect scrollRect;
        
        [Tooltip("Default avatar sprite if URL loading fails or while loading")]
        [SerializeField] private Sprite defaultAvatar;

        [Tooltip("Sprite Asset containing emoji sprites, with names matching keys in EmojiMap.")]
        [SerializeField] private TMPro.TMP_SpriteAsset emojiSpriteAsset;
        private MagicWordsState _state;

        [Tooltip("Delay in seconds between each dialogue line display")]
        [SerializeField] private float delayPerLine = 1f;
        
        [Tooltip("Horizontal inset in pixels for left/right alignment")]
        [SerializeField] private float horizontalInset = 50f;
        // Map semantic keys to sprite indices in the sprite sheet
        private static readonly Dictionary<string, int> EmojiIndexMap = new Dictionary<string, int>
        {
            { "intrigued",   0 },  // thinking face at index 0
            { "satisfied",   1 },  // relieved face at index 1
            { "neutral",     2 },  // neutral face at index 2
            { "affirmative", 3 },  // thumbs up at index 3
            { "laughing",    4 }   // face with tears of joy at index 4
        };

        private PoolService _pooler;
        private void Start()
        {
            _state = MagicWordsState.Instance;
            if (_state == null)
                Debug.LogError("MagicWordsState instance not found. Please add MagicWordsState to the scene before using DialogueAvatarView.");
            
            if (_state != null)
                _state.OnDataLoaded += HandleDataLoaded;

            _pooler = GetComponent<PoolService>();
        }

        private void OnDestroy()
        {
            if (_state != null)
                _state.OnDataLoaded -= HandleDataLoaded;
        }

        private void HandleDataLoaded()
        {
            if (contentParent == null)
            {
                Debug.LogWarning("ContentParent is not assigned on DialogueAvatarView.");
                return;
            }

            foreach (Transform child in contentParent)
                Destroy(child.gameObject);

            // Start conversation display
            StartCoroutine(PlayConversation(_state.Data));
        }

        private IEnumerator PlayConversation(MagicWordsData data)
        {
            float currentY = 200f; // track vertical offset
            float verticalSpacing = 200f;
            for (int i = 0; i < data.dialogue.Count; i++)
            {
                var dialog = data.dialogue[i];
                var avatarEntry = data.avatars.Find(a => a.name == dialog.name);
                var go = _pooler.Get();
                go.transform.SetParent(contentParent, false);
                go.gameObject.SetActive(true);
                // Ensure root stretches horizontally for vertical stacking
                var rt = go.GetComponent<RectTransform>();

                // Alternate sides by setting insets
                if (i % 2 == 0)
                {
                    rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, horizontalInset, rt.rect.width);
                }
                else
                {
                    rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, horizontalInset, rt.rect.width);
                }

                rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, currentY, rt.rect.height);
                currentY += rt.rect.height + verticalSpacing;

                contentParent.sizeDelta = new Vector2(contentParent.sizeDelta.x, currentY);
                var component = go.GetComponent<SingleDialogueView>();
                component.SetSpriteAsset(emojiSpriteAsset);
                component.SetText(dialog.name, ParseEmojis(dialog.text));
                component.SetAvatar(defaultAvatar, i % 2 == 0);
                if (avatarEntry != null && !string.IsNullOrEmpty(avatarEntry.url))
                {
                    StartCoroutine(LoadAvatarImage(avatarEntry.url, component));
                }

                // Force layout rebuild and scroll to bottom
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0f;
                // Wait before showing next line
                yield return new WaitForSeconds(delayPerLine);
            }

            yield return new WaitForSeconds(delayPerLine);
            _state.SetFinish();
        }
        
        private static string ParseEmojis(string text)
        {
            foreach (var kv in EmojiIndexMap)
            {
                text = text.Replace("{" + kv.Key + "}", $"<sprite index={kv.Value}>");
            }
            return text;
        }

        private IEnumerator LoadAvatarImage(string url, SingleDialogueView targetImage)
        {
            using (var uwr = UnityWebRequestTexture.GetTexture(url))
            {
                yield return uwr.SendWebRequest();
#if UNITY_2020_1_OR_NEWER
                if (uwr.result == UnityWebRequest.Result.Success)
#else
                if (!uwr.isNetworkError && !uwr.isHttpError)
#endif
                {
                    var tex = DownloadHandlerTexture.GetContent(uwr);
                    var sprite = Sprite.Create(
                        tex,
                        new Rect(0, 0, tex.width, tex.height),
                        new Vector2(0.5f, 0.5f));
                    targetImage.SetAvatar(sprite);
                }
                else
                {
                    Debug.LogWarning($"Failed to load avatar image from {url}: {uwr.error}");
                }
            }
        }
    }
}
