using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using System;
using SocialApp;

namespace SocialApp
{
    public class ProfileController : MonoBehaviour
    {

        // firebase user
        private FirebaseUser FireUser;
        private string UserFullName;

        public FirebaseUser FIREBASE_USER
        {
            get
            {
                return FireUser;
            }
            set
            {
                FireUser = value;
            }
        }

        // social user
        private User SocialUser;
        public User SOCIAL_USER
        {
            get
            {
                return SocialUser;
            }
            set
            {
                SocialUser = value;
            }
        }

        // profile image loaded
        private bool IsProfileImageLoaded;
        public bool PROFILE_IMAGE_LOADED
        {
            get
            {
                return IsProfileImageLoaded;
            }
            set
            {
                IsProfileImageLoaded = value;
            }
        }

        public void GetUserFullName(Action<string> _callback)
        {
            if (string.IsNullOrEmpty(UserFullName))
            {
                AppManager.FIREBASE_CONTROLLER.GetUserFullName(FireUser.UserId, (_userName =>
                {
                    UserFullName = _userName;
                    _callback.Invoke(UserFullName);
                }));
            }
            else
            {
                _callback.Invoke(UserFullName);
            }
        }

        public bool IsMine(string _userId)
        {
            return _userId == FireUser.UserId;
        }

        public bool IsLogined()
        {
            return FireUser != null;
        }

        public void ClearUser()
        {
            FireUser = null;
            PROFILE_IMAGE_LOADED = false;
            UserFullName = string.Empty;
            AppManager.FIREBASE_CONTROLLER.ClearDeviceToken();
        }
    }

   
    public class User
    {
        public string UserID;
        public string FullName;
        public string Phone;
        public string DataRegistration;
        public string LastActivity;
        public string Email;
        public string ReferralCode;
        public string MyReferralCode;
        public string ImageUrl;
    }
}