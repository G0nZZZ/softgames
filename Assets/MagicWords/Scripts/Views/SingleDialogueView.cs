using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MagicWords.Views
{
    public class SingleDialogueView : MonoBehaviour
    {
        [SerializeField] private Image avatarImage;
        [SerializeField] private RectTransform avatarParent;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI bodyText;

        [SerializeField] private float avatarXLeft = 45f;
        [SerializeField] private float avatarXRight = 450f;

        public void SetText(string name, string body)
        {
            nameText.text = name;
            bodyText.text = body;
        }

        public void SetAvatar(Sprite avatar)
        {
            avatarImage.sprite = avatar;
        }

        public void SetAvatar(Sprite avatar, bool left)
        {
            avatarImage.sprite = avatar;
            avatarParent.anchoredPosition = new Vector2(left ? avatarXLeft : avatarXRight, avatarParent.anchoredPosition.y);
        }
        public void SetSpriteAsset(TMP_SpriteAsset spriteAsset)
        {
            bodyText.spriteAsset = spriteAsset;
        }
    }
}