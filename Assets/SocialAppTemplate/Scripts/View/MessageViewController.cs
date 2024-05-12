using UnityEngine;
using UnityEngine.UI;
//using TMPro;
using UniTools;

namespace SocialApp
{
    public class MessageViewController : MonoBehaviour
    {
        [SerializeField]
        private Image BubleImage = default;
        [SerializeField]
        private Image ContentImage = default;
        //[SerializeField]
        //private TextMeshProUGUI BodyText = default;
        [SerializeField]
        private Text UserNameText = default;
        [SerializeField]
        private Text DateText = default;

        [SerializeField]
        private RectTransform TextRect = default;
        [SerializeField]
        private RectTransform BubleRect = default;
        [SerializeField]
        private RectTransform ContentRect = default;
        [SerializeField]
        private RectTransform MainRect = default;
        [SerializeField]
        private RectTransform ProfileRect = default;
        [SerializeField]
        private float StartBubbleWidth = default;

        [SerializeField]
        private Color UserBubbleColor = default;
        [SerializeField]
        private Color MyBubbleColor = default;
        [SerializeField]
        private AvatarViewController AvatarView = default;
        [SerializeField]
        private OpenHyperlinks LinksChecker = default;
        [SerializeField]
        private bool CacheAvatar = default;

        private Message CurrentMessage;

        private Vector2 TextOffsetMin;
        private Vector2 BubbleOffsetMin;
        private Vector2 BubbleOffsetMax;
        private float MaxContentWidth = 600f;

        private void Awake()
        {
            SaveResetValue();
        }

        private void SaveResetValue()
        {
            TextOffsetMin = TextRect.offsetMin;
            BubbleOffsetMin = BubleRect.offsetMin;
            BubbleOffsetMax = BubleRect.offsetMax;
        }

        public void LoadMedia(Message _msg)
        {
            AvatarView.SetCacheTexture(CacheAvatar);
            ResetRects();
            CurrentMessage = _msg;
            if (_msg.Type == ContentMessageType.TEXT)
            {
                LoadText();
            }
            else if (_msg.Type == ContentMessageType.IMAGE)
            {
                LoadContent();
            }
            LoadGraphics();
            UpdateUIRect();
            GetProfileImage();
        }

        public void LoadGraphics()
        {
            if (isMine())
            {
                BubleImage.color = MyBubbleColor;
            }
            else
            {
                BubleImage.color = UserBubbleColor;
            }
        }

        public void LoadText()
        {
            //BodyText.text = CurrentMessage.BodyTXT;
            UserNameText.text = CurrentMessage.FullName;
            DateText.text = CurrentMessage.DateCreated;
            LinksChecker.CheckLinks();
            ContentImage.gameObject.SetActive(false);
        }

        public void LoadContent()
        {
            UserNameText.text = CurrentMessage.FullName;
            DateText.text = CurrentMessage.DateCreated;
            ContentImage.gameObject.SetActive(true);
            ContentImage.color = Color.grey;
            float width = CurrentMessage.MediaInfo.ContentWidth;
            float height = CurrentMessage.MediaInfo.ContentHeight;
            if (width > MaxContentWidth)
            {
                height = MaxContentWidth * height / width;
                width = MaxContentWidth;
            }

            ContentRect.sizeDelta = new Vector2(width, height);
            ContentRect.anchoredPosition = new Vector2(ContentRect.anchoredPosition.x, -height/2f);

            if (!string.IsNullOrEmpty(CurrentMessage.MediaInfo.ContentURL))
            {
                CoroutineExecuter _ce = new CoroutineExecuter();
                ImageService _is = new ImageService(_ce);
                _is.DownloadOrLoadTexture(CurrentMessage.MediaInfo.ContentURL, _texture =>
                {
                    if (_texture != null)
                    {
                        ContentImage.color = Color.white;
                        ContentImage.sprite = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                    }
                });
            }
        }

        private void ResetRects()
        {
            TextRect.offsetMin = TextOffsetMin;
            BubleRect.offsetMin = BubbleOffsetMin;
            BubleRect.offsetMax = BubbleOffsetMax;
        }

        private bool isMine()
        {
            return CurrentMessage.UserID == AppManager.USER_PROFILE.FIREBASE_USER.UserId;
        }

        private void UpdateUIRect()
        {
            if (CurrentMessage.Type == ContentMessageType.TEXT)
            {
                // update buble text rect
				float txtPreferredWidth = 320;//BodyText.preferredWidth;
                if (txtPreferredWidth > TextRect.rect.width)
                    txtPreferredWidth = TextRect.rect.width;
                //TextRect.offsetMin = new Vector2(TextRect.offsetMin.x, TextRect.offsetMin.y - BodyText.preferredHeight + (float)BodyText.fontSize);
                //BubleRect.offsetMin = new Vector2(BubleRect.offsetMin.x, BubleRect.offsetMin.y - TextRect.rect.height + StartBubbleWidth);
                //BubleRect.offsetMax = new Vector2(BubleRect.offsetMax.x + txtPreferredWidth - StartBubbleWidth, BubleRect.offsetMax.y);
            }
            else if (CurrentMessage.Type == ContentMessageType.IMAGE)
            {
                // update buble content rect
                BubleRect.offsetMin = new Vector2(BubleRect.offsetMin.x, BubleRect.offsetMin.y - ContentRect.rect.height + StartBubbleWidth);
                BubleRect.offsetMax = new Vector2(BubleRect.offsetMax.x + ContentRect.rect.width - StartBubbleWidth, BubleRect.offsetMax.y );
            }
            // update message rect
            float _height = BubleRect.rect.height + ProfileRect.rect.height;
            MainRect.sizeDelta = new Vector2(MainRect.rect.width, _height);
        }

        private void GetProfileImage()
        {
            AvatarView.LoadAvatar(CurrentMessage.UserID);
        }
    }
}
