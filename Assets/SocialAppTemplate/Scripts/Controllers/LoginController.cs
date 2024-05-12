using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocialApp;
using System;
using UnityEngine.SceneManagement;
using Firebase.Auth;


namespace SocialApp
{
    public class LoginController : MonoBehaviour
    {

        [SerializeField]
        private InputField EmailInput = default;
        [SerializeField]
        private InputField PasswordInput = default;

        public event Action OnLoginEvent;
        public event Action OnLogoutEvent;

        public GoogleCallbackLoginMessage googlMessage=null;
        public PhoneCallbackLoginMessage phoneMessage=null;
        public FacebookCallbackLoginMessage facebookMessage=null;
        private void OnEnable()
        {
            ClearFields();
           
        }
        
        public void SendLogIn()
        {
            if (CheckError())
                return;
            string _login = EmailInput.text.Trim();
            string _password = PasswordInput.text.Trim();
            OnLogin(_login, _password);
        }

        public void AutoLogin(string _mail, string _password)
        {
            OnLogin(_mail, _password);
        }

        private void OnLogin(string _mail, string _password)
        {
            AppManager.VIEW_CONTROLLER.ShowLoading();
            
        }
       
        public void GoogleLogInButton()
        {
            StaticValues.BlockCreatingDoubleProcess = true;
            StaticValues.phoneNumber = "";
            StaticValues.phoneNumberWithoutPrefix = "";
            StaticValues.displayName = "";
            StaticValues.displayNameinUC = "";
            StaticValues.MyReferralCode = "";
            StaticValues.ReferralCode = "";
            AppManager.VIEW_CONTROLLER.ClearText();
           
        }
        public void OnRegistration()
        {
            AppManager.VIEW_CONTROLLER.HideLogin();
            AppManager.VIEW_CONTROLLER.ShowRegistration();
        }

        
        
        private void ClearFields()
        {
            EmailInput.text = string.Empty;
            PasswordInput.text = string.Empty;
        }

        private bool CheckError()
        {
            bool IsError = false;
            if (string.IsNullOrEmpty(EmailInput.text))
            {
                AppManager.VIEW_CONTROLLER.ShowPopupMSG(MessageCode.EmptyEmail);
                IsError = true;
            }
            else if (string.IsNullOrEmpty(PasswordInput.text))
            {
                AppManager.VIEW_CONTROLLER.ShowPopupMSG(MessageCode.EmptyPassword);
                IsError = true;
            }
            else if (!EmailInput.text.Contains(AppManager.APP_SETTINGS.EmailValidationCharacter))
            {
                AppManager.VIEW_CONTROLLER.ShowPopupMSG(MessageCode.EmailNotValid);
                IsError = true;
            }
            else if (PasswordInput.text.Length < AppManager.APP_SETTINGS.MinAllowPasswordCharacters)
            {
                AppManager.VIEW_CONTROLLER.ShowPopupMSG(MessageCode.SmallPassword);
                IsError = true;
            }
            return IsError;
        }
        public void OnSignIn()
        {
            //OnLoginEvent?.Invoke();
        }
        public void OnSignOut()
        {
            OnLogoutEvent?.Invoke();
        }
        
       
        
    }
}
