using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using System;
using SocialApp;

namespace SocialApp
{
    public class NavigationController : MonoBehaviour
    {

        [SerializeField]
        private Image[] Icons = default;
        [SerializeField]
        private Image[] Backgounds = default;
        [SerializeField]
        private Color ActiveColor = default;
        [SerializeField]
        private Color DefaultColor = default;
        [SerializeField]
        private Color ActiveBackColor = default;
        [SerializeField]
        private GameObject UnreadCountObject = default;
        [SerializeField]
        private Text UnreadCountLabel = default;
        [SerializeField]
        private GameObject FriendFeedsCountObject = default;
        [SerializeField]
        private Text FriendFeedCountLabel = default;
        [SerializeField]
        private GameObject FriendCountObject = default;
        [SerializeField]
        private Text FriendCountLabel = default;

        private DatabaseReference DRRequestFriendsCount;
        private DatabaseReference DRFeedFriendsCount;
        private DatabaseReference DRUnreadAllCount;

        private NavigationTab CurrentTab;
        public delegate void DeActivateHomeScreen();
        public event DeActivateHomeScreen OnDeActivateHomeScreen;

        public delegate void ActivateHomeScreen();
        public event ActivateHomeScreen OnActivateHomeScreen;

        public delegate void ActivateSettingsScreen();
        public event ActivateSettingsScreen OnActivateSettingsScreen;
        public void ShowUserProfile()
        {
            CurrentTab = NavigationTab.Profile;
            AppManager.DEVICE_CONTROLLER.UnloadAssets();
            AppManager.VIEW_CONTROLLER.HideNavigationGroup();
            AppManager.VIEW_CONTROLLER.ShowUserProfile();

            ToDefault();
            Icons[0].color = ActiveColor;
            Backgounds[0].color = ActiveBackColor;

            if (OnDeActivateHomeScreen != null)
                OnDeActivateHomeScreen();
        }

        public void ShowSetting()
        {
            CurrentTab = NavigationTab.Setting;
            AppManager.DEVICE_CONTROLLER.UnloadAssets();
            AppManager.VIEW_CONTROLLER.HideNavigationGroup();
            AppManager.VIEW_CONTROLLER.ShowSettings();

            ToDefault();
            Icons[6].color = ActiveColor;
            Backgounds[6].color = ActiveBackColor;

            if (OnActivateSettingsScreen != null)
                OnActivateSettingsScreen();
        }

        public void ShowFriends()
        {
            CurrentTab = NavigationTab.Friends;
            AppManager.DEVICE_CONTROLLER.UnloadAssets();
            AppManager.VIEW_CONTROLLER.HideNavigationGroup();
            AppManager.VIEW_CONTROLLER.ShowFriendsList();
           // AppManager.FRIEND_UI_CONTROLLER.OnFriends();

            ToDefault();
            Icons[4].color = ActiveColor;
            Backgounds[4].color = ActiveBackColor;

            if (OnDeActivateHomeScreen != null)
                OnDeActivateHomeScreen();
        }

        public void ShowFriendsNews()
        {
            CurrentTab = NavigationTab.FriendsNews;
            AppManager.DEVICE_CONTROLLER.UnloadAssets();
            AppManager.VIEW_CONTROLLER.HideNavigationGroup();
            AppManager.VIEW_CONTROLLER.ShowFriendsNews();
            ToDefault();
            Icons[2].color = ActiveColor;
            Backgounds[2].color = ActiveBackColor;
            AppManager.FIREBASE_CONTROLLER.ClearUnreadFriendsFeed();

            if (OnDeActivateHomeScreen != null)
                OnDeActivateHomeScreen();
        }

        public void ShowAllNews()
        {
            CurrentTab = NavigationTab.WorldNews;
            AppManager.DEVICE_CONTROLLER.UnloadAssets();
            AppManager.VIEW_CONTROLLER.HideNavigationGroup();
            AppManager.VIEW_CONTROLLER.ShowWorldNews();
            ToDefault();
            Icons[1].color = ActiveColor;
            Backgounds[1].color = ActiveBackColor;

            if (OnDeActivateHomeScreen != null)
                OnDeActivateHomeScreen();
        }

        public void ShowMessanging()
        {
            CurrentTab = NavigationTab.Messanging;
            AppManager.DEVICE_CONTROLLER.UnloadAssets();
            AppManager.VIEW_CONTROLLER.HideNavigationGroup();
            AppManager.VIEW_CONTROLLER.ShowMessageList();

            ToDefault();
            Icons[5].color = ActiveColor;
            Backgounds[5].color = ActiveBackColor;

            if (OnDeActivateHomeScreen != null)
                OnDeActivateHomeScreen();
        }
        public void ShowHomePage()
        {
            CurrentTab = NavigationTab.Home;
            AppManager.DEVICE_CONTROLLER.UnloadAssets();
            AppManager.VIEW_CONTROLLER.HideNavigationGroup();
            

            ToDefault();
            Icons[3].color = ActiveColor;
            Backgounds[3].color = ActiveBackColor;

            if (OnActivateHomeScreen != null)
                OnActivateHomeScreen();
        }
        private bool enableListener = false;
        public void AddListeners()
        {
            if (AppManager.FIREBASE_CONTROLLER.IsFirebaseInited())
            {
                DRRequestFriendsCount = AppManager.FIREBASE_CONTROLLER.GetRequestFriendCountReferece(AppManager.USER_PROFILE.FIREBASE_USER.UserId);
                DRFeedFriendsCount = AppManager.FIREBASE_CONTROLLER.GetFriendFeedCountReferece(AppManager.USER_PROFILE.FIREBASE_USER.UserId);
                DRUnreadAllCount = AppManager.FIREBASE_CONTROLLER.GetAllUnreadCountReferece(AppManager.USER_PROFILE.FIREBASE_USER.UserId);
                if (!enableListener)
                {
                    DRRequestFriendsCount.ValueChanged += OnRequestCountUpdated;
                    DRFeedFriendsCount.ValueChanged += OnFriendsCountUpdated;
                    DRUnreadAllCount.ValueChanged += OnUnreadCountUpdated;
                    enableListener = true;
                }
            }
            UnreadCountObject.SetActive(false);
            FriendFeedsCountObject.SetActive(false);
            FriendCountObject.SetActive(false);
        }

        public void RemoveListeners()
        {
            if (AppManager.FIREBASE_CONTROLLER.IsFirebaseInited())
            {
                if (enableListener)
                {
                    enableListener = false;
                    DRRequestFriendsCount.ValueChanged -= OnRequestCountUpdated;
                    DRFeedFriendsCount.ValueChanged -= OnFriendsCountUpdated;
                    DRUnreadAllCount.ValueChanged -= OnUnreadCountUpdated;
                }
            }
        }

        private void ToDefault()
        {
            foreach (Image _background in Backgounds)
            {
                _background.color = ActiveColor;
            }
            foreach (Image _icon in Icons)
            {
                _icon.color = DefaultColor;
            }
        }

        private void OnRequestCountUpdated(object sender, ValueChangedEventArgs args)
        {
            if (args.DatabaseError != null)
            {
                FriendCountLabel.text = "0";
                FriendCountObject.SetActive(false);
                return;
            }
            try
            {
                if (args.Snapshot.Value.ToString() == "0")
                {
                    FriendCountLabel.text = "0";
                    FriendCountObject.SetActive(false);
                }
                else
                {
                    FriendCountLabel.text = args.Snapshot.Value.ToString();
                    FriendCountObject.SetActive(true);
                }
            }
            catch (Exception)
            {
                FriendCountLabel.text = "0";
                FriendCountObject.SetActive(false);
            }
        }

        private void OnFriendsCountUpdated(object sender, ValueChangedEventArgs args)
        {
            if (args.DatabaseError != null)
            {
                FriendFeedCountLabel.text = "0";
                FriendFeedsCountObject.SetActive(false);
                return;
            }
            try
            {
                if (args.Snapshot.Value.ToString() == "0")
                {
                    FriendFeedCountLabel.text = "0";
                    FriendFeedsCountObject.SetActive(false);
                }
                else
                {
                    FriendFeedCountLabel.text = args.Snapshot.Value.ToString();
                    FriendFeedsCountObject.SetActive(true);
                }
            }
            catch (Exception)
            {
                FriendFeedCountLabel.text = "0";
                FriendFeedsCountObject.SetActive(false);
            }
        }

        private void OnUnreadCountUpdated(object sender, ValueChangedEventArgs args)
        {
            if (args.DatabaseError != null)
            {
                UnreadCountLabel.text = "0";
                UnreadCountObject.SetActive(false);
                return;
            }
            try
            {
                if (args.Snapshot.Value.ToString() == "0")
                {
                    UnreadCountLabel.text = "0";
                    UnreadCountObject.SetActive(false);
                }
                else
                {
                    UnreadCountLabel.text = args.Snapshot.Value.ToString();
                    UnreadCountObject.SetActive(true);
                }
            }
            catch (Exception)
            {
                UnreadCountLabel.text = "0";
                UnreadCountObject.SetActive(false);
            }
        }

        public void ShowLastTab()
        {
            switch (CurrentTab)
            {
                case NavigationTab.Profile:
                    ShowUserProfile();
                    break;
                case NavigationTab.WorldNews:
                    ShowAllNews();
                    break;
                case NavigationTab.FriendsNews:
                    ShowFriendsNews();
                    break;
                case NavigationTab.Friends:
                    ShowFriends();
                    break;
                case NavigationTab.Messanging:
                    ShowMessanging();
                    break;
                case NavigationTab.Setting:
                    ShowSetting();
                    break;
                case NavigationTab.Home:
                    ShowHomePage();
                    break;
                default:
                    Debug.Log("NOTHING");
                    break;
            }


        }

        public enum NavigationTab
        {
            Profile,
            WorldNews,
            FriendsNews,
            Friends,
            Messanging,
            Setting,
            Home
        }
    }
}
