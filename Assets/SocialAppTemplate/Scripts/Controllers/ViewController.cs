using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Android;

namespace SocialApp
{
    public class ViewController : MonoBehaviour
    {

        [SerializeField]
        private Camera MainCamera = default;
        [SerializeField]
        private GameObject LoadingScreen = default;
        [SerializeField]
        private GameObject PopupObject = default;
        [SerializeField]
        private GameObject RegistrationObject = default;
       
      
       
        [SerializeField]
        private GameObject LoginObject = default;
       
       
       
       
      
       
       
       
        
      
       
        [SerializeField]
        public GameObject Rating = default;
        [SerializeField]
        public GameObject InternetObject = default;
        [SerializeField]
        public GameObject VersionObject = default;
        [SerializeField]
        public GameObject MaintenanceObject = default;
        [SerializeField]
        private GameObject QuitObject = default;
        [SerializeField]
        private GameObject LogoutObject = default;
        [SerializeField]
        private GameObject RewardedObject = default;

        [SerializeField]
        private GameObject CanvasLogin_EnterMobile = default;
        [SerializeField]
        private GameObject CanvasLogin_EnterReferralCode = default;
        [SerializeField]
        private GameObject CanvasLogin_EnterOTP = default;
        [SerializeField]
        private GameObject CanvasLogin_EnterName = default;

        [SerializeField]
        private InputField _EnterMobile = default;
        [SerializeField]
        private InputField _EnterReferralCode = default;
        [SerializeField]
        private InputField _EnterOnlyMobile = default;
        [SerializeField]
        private InputField _EnterName = default;
        [SerializeField]
        private Text RewardedText = default;
        [SerializeField]
        private GameObject HaveReferralCode = default;
        bool needrestart;

        public GameObject Resend;
        public Text timer;
        float timeLeft = 60f;
        bool startcountdown = false;

        public InputField VerifyCode;
        protected string receivedCode = "";
        public Text text1;
        public Text text2;
        public Text text3;
        public Text text4;
        public Text text5;
        public Text text6;
        public Text OTPSendText;
        

        private void Start()
        {
            needrestart = true;
            HaveReferralCode.GetComponent<Toggle>().isOn = false;
            HaveReferralCode.GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
            HaveReferralCode.GetComponent<Toggle>().onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    StaticValues.HaveReferralCode = true;
                }
                else
                {
                    StaticValues.HaveReferralCode = false;
                }
            });

            VerifyCode.onValueChanged.RemoveAllListeners();
            VerifyCode.onValueChanged.AddListener(OnValueChangeInCode);

#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#else
  Debug.unityLogger.logEnabled = true;
#endif
        }
        private void OnEnable()
        {
            UnitySMSReceiver.fetchOtpNumber += UnitySMSReceiver_fetchOtpNumber;
        }
        private void OnDisable()
        {
            UnitySMSReceiver.fetchOtpNumber -= UnitySMSReceiver_fetchOtpNumber;
        }
        private void UnitySMSReceiver_fetchOtpNumber(string otp_no)
        {
            //Debug.Log("recieved otp_no..." + otp_no);
            
            if (!string.IsNullOrEmpty(otp_no))
            {
                VerifyCode.text = otp_no;
                OnValueChangeInCode(otp_no);
            }
        }

        // camera
        public Camera GetMainCamera()
        {
            if (MainCamera)
            {
                return MainCamera;
            }
            return Camera.main;
        }
        public void SetOnlyPhoneNumber()
        {
            _EnterOnlyMobile.text = StaticValues.phoneNumberWithoutPrefix;
        }
        public void SetName()
        {
            _EnterName.text = StaticValues.displayName;
        }
        private void Update()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable && needrestart == true)
            {
                needrestart = false;
                ShowInternetMessage();
            }
            else if (Application.internetReachability != NetworkReachability.NotReachable && needrestart == false)
            {
                
                HideInternetObject();
            }

            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Menu) || Input.GetKeyUp(KeyCode.Home) || Input.GetKeyUp(KeyCode.Menu))
                {
                    if (PlayerSave.singleton != null)
                    {
                        PlayerSave.singleton.ShowErrorMessage("App doesn't support split-screen mode");
                    }
                }
            }

            if (startcountdown)
            {
                timeLeft -= Time.deltaTime;

                timer.gameObject.SetActive(true);
                Resend.gameObject.SetActive(false);
                timer.text = "Resend OTP in <color=White>" + (timeLeft).ToString("00")+ " sec</color>";

                if (timeLeft < 0)
                {
                    Resend.gameObject.SetActive(true);
                    timer.gameObject.SetActive(false);
                    startcountdown = false;
                }
            }
        }

        public void ResendPhone()
        {
            StaticValues.HaveReferralCode = false;
            StaticValues.mobileVerificationId = string.Empty;
            if (PlayerSave.singleton != null)
            {
                PlayerSave.singleton.KhelTamashaGetUserDetails(StaticValues.phoneNumberWithoutPrefix, StaticValues.mobileVerificationId, OnKTLoginResponse, 0);
            }
        }
        private void OnKTLoginResponse(ServerUserDetailsResponseAddPot _serverUserDetailsResponse)
        {

            if (_serverUserDetailsResponse != null)
            {
                if (_serverUserDetailsResponse.status.Equals("200"))
                {
                    //PlayerSave.singleton.ShowErrorMessage(_serverUserDetailsResponse.message);

                    if (StaticValues.CurrentVersion == StaticValues.version)
                    {
                        if (StaticValues.ismaintenance == 0)
                        {
                            PlayerPrefs.SetInt(Utils.LOGGED, Utils.PH);
                            PlayerPrefs.Save();
                            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
                        }
                        else
                        {
                            //PlayerSave.singleton.ShowErrorMessage("Game is on maintenance!!!");

                            AppManager.VIEW_CONTROLLER.HideAllScreen();
                            AppManager.VIEW_CONTROLLER.ShowMaintenance();
                        }

                    }
                    else
                    {
                        //PlayerSave.singleton.ShowErrorMessage("Please update apk");

                        AppManager.VIEW_CONTROLLER.HideAllScreen();
                        AppManager.VIEW_CONTROLLER.ShowVersion();

                    }

                }
                else if (_serverUserDetailsResponse.status.Equals("201"))//User already exists but verification failed
                {

                    AppManager.VIEW_CONTROLLER.HideAllScreen();
                    AppManager.VIEW_CONTROLLER.OTPSend();
                    AppManager.VIEW_CONTROLLER.ShowEnterOTP();

                }
                else if (_serverUserDetailsResponse.status.Equals("420"))//User is blocked!
                {
                    PlayerSave.singleton.ShowErrorMessage(_serverUserDetailsResponse.message);

                    AppManager.VIEW_CONTROLLER.HideAllScreen();
                    AppManager.VIEW_CONTROLLER.ShowLogin();
                    AppManager.USER_SETTINGS.Logout();


                }
                else if (_serverUserDetailsResponse.status.Equals("404"))//User is not blocked!
                {

                    if (!_serverUserDetailsResponse.message.Equals("User is blocked!"))
                    {

                        PlayerSave.singleton.KhelTamashaAddNewUserAPICall(OnRegistrationResponse);
                    }
                    else
                    {
                        PlayerSave.singleton.ShowErrorMessage(_serverUserDetailsResponse.message);
                        AppManager.VIEW_CONTROLLER.HideAllScreen();
                        AppManager.VIEW_CONTROLLER.ShowLogin();
                        AppManager.USER_SETTINGS.Logout();

                    }
                }
                else
                {
                    AppManager.VIEW_CONTROLLER.HideAllScreen();
                    AppManager.VIEW_CONTROLLER.ShowLogin();
                    AppManager.USER_SETTINGS.Logout();
                    PlayerSave.singleton.ShowErrorMessage(_serverUserDetailsResponse.message);

                }
            }
        }
        private void OnRegistrationResponse(ServerUserDetailsResponse _serverUserDetailsResponse)
        {

            if (_serverUserDetailsResponse != null)
            {
                if (_serverUserDetailsResponse.status.Equals("200"))
                {
                    //PlayerSave.singleton.ShowErrorMessage(_serverUserDetailsResponse.message);
                    //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
                    if (StaticValues.HaveReferralCode)
                    {
                        AppManager.VIEW_CONTROLLER.HideAllScreen();
                        AppManager.VIEW_CONTROLLER.ShowEnterReferralCode();

                    }
                    else
                    {
                        AppManager.VIEW_CONTROLLER.HideAllScreen();
                        AppManager.VIEW_CONTROLLER.OTPSend();
                        AppManager.VIEW_CONTROLLER.ShowEnterOTP();
                    }
                }
                else if (_serverUserDetailsResponse.status.Equals("404"))
                {
                    if (_serverUserDetailsResponse.message.Contains("already") || _serverUserDetailsResponse.message.Contains("Already"))
                    {
                        //PlayerSave.singleton.ShowErrorMessage(_serverUserDetailsResponse.message);
                        //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
                        StaticValues.HaveReferralCode = false;
                        AppManager.VIEW_CONTROLLER.HideAllScreen();
                        AppManager.VIEW_CONTROLLER.OTPSend();
                        AppManager.VIEW_CONTROLLER.ShowEnterOTP();
                    }
                    else if (_serverUserDetailsResponse.message.Contains("Oops. Something went wrong. Please try again later"))
                    {
                        AppManager.VIEW_CONTROLLER.HideAllScreen();
                        AppManager.VIEW_CONTROLLER.ShowLogin();
                        AppManager.USER_SETTINGS.Logout();
                        PlayerSave.singleton.ShowErrorMessage(_serverUserDetailsResponse.message);
                        //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
                    }
                    else
                    {
                        AppManager.VIEW_CONTROLLER.HideAllScreen();
                        AppManager.VIEW_CONTROLLER.ShowLogin();
                        AppManager.USER_SETTINGS.Logout();
                        PlayerSave.singleton.ShowErrorMessage(_serverUserDetailsResponse.message);
                    }
                }
                else
                {
                    AppManager.VIEW_CONTROLLER.HideAllScreen();
                    AppManager.VIEW_CONTROLLER.ShowLogin();
                    AppManager.USER_SETTINGS.Logout();
                    PlayerSave.singleton.ShowErrorMessage(_serverUserDetailsResponse.message);
                }
            }
        }
        public void VerifyPhone()
        {
            StaticValues.BlockCreatingDoubleProcess = true;
            if (!string.IsNullOrWhiteSpace(_EnterMobile.text))
            {
                if (_EnterMobile.text.Length >= 10)
                {
                    StaticValues.phoneNumber = _EnterMobile.text;
                    if (StaticValues.phoneNumber.StartsWith("+91"))
                    {
                        StaticValues.phoneNumberWithoutPrefix = StaticValues.phoneNumber.Remove(0, 3);
                        AppManager.FIREBASE_CONTROLLER.VerifyPhonenumber(StaticValues.phoneNumberWithoutPrefix, null);
                    }
                    else if (StaticValues.phoneNumber.StartsWith("091"))
                    {
                        StaticValues.phoneNumberWithoutPrefix = StaticValues.phoneNumber.Remove(0, 3);
                        AppManager.FIREBASE_CONTROLLER.VerifyPhonenumber( StaticValues.phoneNumberWithoutPrefix, null);
                    }
                    else if (StaticValues.phoneNumber.StartsWith("0"))
                    {
                        StaticValues.phoneNumberWithoutPrefix = StaticValues.phoneNumber.Remove(0, 1);
                        AppManager.FIREBASE_CONTROLLER.VerifyPhonenumber(StaticValues.phoneNumberWithoutPrefix, null);
                    }
                    else if (_EnterMobile.text.Length == 10)
                    {
                        StaticValues.phoneNumberWithoutPrefix = StaticValues.phoneNumber;
                        AppManager.FIREBASE_CONTROLLER.VerifyPhonenumber( StaticValues.phoneNumberWithoutPrefix, null);
                    }
                    else if (_EnterMobile.text.Length == 11)
                    {
                        StaticValues.phoneNumberWithoutPrefix = StaticValues.phoneNumber.Remove(0, 1);
                        AppManager.FIREBASE_CONTROLLER.VerifyPhonenumber(StaticValues.phoneNumberWithoutPrefix, null);
                    }
                    else if (_EnterMobile.text.Length == 12)
                    {
                        StaticValues.phoneNumberWithoutPrefix = StaticValues.phoneNumber.Remove(0, 2);
                        AppManager.FIREBASE_CONTROLLER.VerifyPhonenumber( StaticValues.phoneNumberWithoutPrefix, null);
                    }
                    else if (_EnterMobile.text.Length == 13)
                    {
                        StaticValues.phoneNumberWithoutPrefix = StaticValues.phoneNumber.Remove(0, 3);
                        AppManager.FIREBASE_CONTROLLER.VerifyPhonenumber( StaticValues.phoneNumberWithoutPrefix, null);
                    }
                    else if (_EnterMobile.text.Length == 14)
                    {
                        StaticValues.phoneNumberWithoutPrefix = StaticValues.phoneNumber.Remove(0, 4);
                        AppManager.FIREBASE_CONTROLLER.VerifyPhonenumber( StaticValues.phoneNumberWithoutPrefix, null);
                    }
                    else
                    {
                        AppManager.FIREBASE_CONTROLLER.VerifyPhonenumber( StaticValues.phoneNumber, null);
                    }
                }
                else
                {
                    AppManager.FIREBASE_CONTROLLER.ShowErrorMessage("Please enter your valid mobile number");
                }
               
            }
            else
            {

                AppManager.FIREBASE_CONTROLLER.ShowErrorMessage("Please enter your mobile number");
            }
        }
        
        internal virtual void PermissionCallbacks_PermissionDeniedAndDontAskAgain(string permissionName)
        {

            //Debug.Log($"{permissionName} PermissionDeniedAndDontAskAgain");

        }

        internal virtual void PermissionCallbacks_PermissionGranted(string permissionName)
        {

            //Debug.Log($"{permissionName} PermissionCallbacks_PermissionGranted");
        }

        internal virtual void PermissionCallbacks_PermissionDenied(string permissionName)
        {

            //Debug.Log($"{permissionName} PermissionCallbacks_PermissionDenied");
        }
        public void startCount()
        {
            timeLeft = 60f;
            startcountdown = true;
            timer.gameObject.SetActive(true);
            Resend.SetActive(false);
        }
        public void OTPSend()
        {
            if (!string.IsNullOrEmpty(StaticValues.phoneNumberWithoutPrefix))
            {
                OTPSendText.text = "OTP sent to " + StaticValues.phoneNumberWithoutPrefix;
            }
            else
            {
                OTPSendText.text = "OTP sent to " + StaticValues.FirebaseUserId;
            }
            startCount();
        }
        public void VerifyReceivedPhoneCode()
        {
            if (!string.IsNullOrWhiteSpace(receivedCode))
            {
                if (receivedCode.Length >= 6)
                {
                    AppManager.VIEW_CONTROLLER.ShowLoading();
                    AppManager.FIREBASE_CONTROLLER.VerifyReceivedPhoneCode(receivedCode, null);
                }
                else
                {
                    AppManager.FIREBASE_CONTROLLER.ShowErrorMessage("Please enter valid Otp!!!");
                }

            }
            else
            {
                AppManager.FIREBASE_CONTROLLER.ShowErrorMessage("Enter Otp before proceed!!!");
            }
        }
        public void ClearText()
        {
            _EnterMobile.text = "";
            _EnterOnlyMobile.text = "";
            _EnterName.text = "";
            _EnterReferralCode.text = "";
            VerifyCode.text = "";
            receivedCode = "";
            text1.text = "";
            text2.text = "";
            text3.text = "";
            text4.text = "";
            text5.text = "";
            text6.text = "";
        }
        
        
        public void OnReceivedReferralCode()
        {
            StaticValues.BlockCreatingDoubleProcess = true;
            if (!string.IsNullOrEmpty(_EnterReferralCode.text))
            {
                if (_EnterReferralCode.text.Length >= 4)
                {
                    StaticValues.ReferralCode = _EnterReferralCode.text;
                    if(PlayerSave.singleton!=null)
                    {
                        AppManager.VIEW_CONTROLLER.ShowLoading();
                        PlayerSave.singleton.CheckReferralCode(StaticValues.ReferralCode, OnCheckReferralCodeResponse);
                    }

                }
                else
                {
                    AppManager.FIREBASE_CONTROLLER.ShowErrorMessage("Please enter atleast 4 characters!!!");
                }
            }
            else
            {
                AppManager.FIREBASE_CONTROLLER.ShowErrorMessage("Please enter referral code!!!");
            }
        }
        public void OnSkipReceivedReferralCode()
        {
            AppManager.VIEW_CONTROLLER.HideAllScreen();
            StaticValues.HaveReferralCode = false;
            AppManager.VIEW_CONTROLLER.OTPSend();
            AppManager.VIEW_CONTROLLER.ShowEnterOTP();
            StaticValues.mobileVerificationId = string.Empty;
           
            if (PlayerSave.singleton != null)
            {
                PlayerSave.singleton.KhelTamashaGetUserDetails(StaticValues.FirebaseUserId, StaticValues.mobileVerificationId, OnKTLoginResponse, 0);
            }
            HaveReferralCode.GetComponent<Toggle>().isOn = false;
        }
        private void OnValueChangeInCode(string _text)
        {
            if (VerifyCode.gameObject.activeInHierarchy)
            {
                if (string.IsNullOrEmpty(VerifyCode.text))
                {
                    if (text1)
                    {
                        text1.text = "";
                        text2.text = "";
                        text3.text = "";
                        text4.text = "";
                        text5.text = "";
                        text6.text = "";
                    }
                }

                if (receivedCode != VerifyCode.text)
                {

                    receivedCode = VerifyCode.text;

                    for (int i = 0; i < receivedCode.Length; i++)
                    {

                        if (i == 0)
                        {
                            if (text1)
                            {
                                text1.text = receivedCode[0].ToString();
                            }
                        }
                        else if (i == 1)
                        {
                            if (text1)
                            {
                                text1.text = receivedCode[0].ToString();
                            }
                            if (text2)
                            {
                                text2.text = receivedCode[1].ToString();
                            }

                        }
                        else if (i == 2)
                        {
                            if (text1)
                            {
                                text1.text = receivedCode[0].ToString();
                            }
                            if (text2)
                            {
                                text2.text = receivedCode[1].ToString();
                            }
                            if (text3)
                            {
                                text3.text = receivedCode[2].ToString();
                            }

                        }
                        else if (i == 3)
                        {
                            if (text1)
                            {
                                text1.text = receivedCode[0].ToString();
                            }
                            if (text2)
                            {
                                text2.text = receivedCode[1].ToString();
                            }
                            if (text3)
                            {
                                text3.text = receivedCode[2].ToString();
                            }
                            if (text4)
                            {
                                text4.text = receivedCode[3].ToString();
                            }

                        }
                        else if (i == 4)
                        {
                            if (text1)
                            {
                                text1.text = receivedCode[0].ToString();
                            }
                            if (text2)
                            {
                                text2.text = receivedCode[1].ToString();
                            }
                            if (text3)
                            {
                                text3.text = receivedCode[2].ToString();
                            }
                            if (text4)
                            {
                                text4.text = receivedCode[3].ToString();
                            }
                            if (text5)
                            {
                                text5.text = receivedCode[4].ToString();
                            }

                        }
                        else if (i == 5)
                        {
                            if (text1)
                            {
                                text1.text = receivedCode[0].ToString();
                            }
                            if (text2)
                            {
                                text2.text = receivedCode[1].ToString();
                            }
                            if (text3)
                            {
                                text3.text = receivedCode[2].ToString();
                            }
                            if (text4)
                            {
                                text4.text = receivedCode[3].ToString();
                            }
                            if (text5)
                            {
                                text5.text = receivedCode[4].ToString();
                            }
                            if (text6)
                            {
                                text6.text = receivedCode[5].ToString();
                            }
                        }
                    }
                }
            }
        }
        // popup
        public void ShowPopupMessage(PopupMessage _msg)
        {
            PopupObject.SetActive(true);
            PopupObject.GetComponent<PopupController>().ShowMessage(_msg);
        }
        public void ShowPopupMessage(PopupMessage _msg,int ind)
        {
            PopupObject.SetActive(true);
            PopupObject.GetComponent<PopupController>().ShowMessage(_msg,ind);
        }
		public void ShowBankPopupMessage(PopupMessage _msg,int ind)
		{
			PopupObject.SetActive(true);
			PopupObject.GetComponent<PopupController>().ShowBankMessage(_msg,ind);
		}
		public void ShowWithdrawPopupMessage(PopupMessage _msg,int ind)
		{
			PopupObject.SetActive(true);
			PopupObject.GetComponent<PopupController>().ShowWithdrawMessage(_msg,ind);
		}
        public void HidePopupMessage()
        {
            PopupObject.SetActive(false);
        }
        public void ShowInternetMessage()
        {
            InternetObject.SetActive(true);
        }
        public void HideInternetObject()
        {
            needrestart = true;
            InternetObject.SetActive(false);
        }
        public void ShowRewardedAdsMessage(double _coins)
        {
            RewardedObject.SetActive(true);
            RewardedText.text = "You got "+_coins.ToString()+" reward Coins.";
        }
        public void HideRewardedAdsMessage()
        {
            RewardedObject.SetActive(false);
            
        }
        public void ShowQuitMessage()
        {
            QuitObject.SetActive(true);
        }
        public void HideQuitMessage()
        {
            QuitObject.SetActive(false);
        }
        public void ShowLogoutMessage()
        {
            LogoutObject.SetActive(true);
        }
        public void HideLogoutMessage()
        {
            LogoutObject.SetActive(false);
        }
        public void OnlyQuitMessage()
        {
            Application.Quit();
            QuitObject.SetActive(false);
        }
        // loading
        public void ShowLoading()
        {
            //Debug.Log("Showloading");
            LoadingScreen.SetActive(true);
        }
        public void ShowVersion()
        {
            VersionObject.SetActive(true);
        }
        public void HideVersion()
        {
            VersionObject.SetActive(false);
        }
        public void ShowMaintenance()
        {
            MaintenanceObject.SetActive(true);
        }
        public void HideMaintenance()
        {
            MaintenanceObject.SetActive(false);
        }
        public void ShowError(PopupMessage _msg)
        {
            PopupObject.SetActive(true);
            PopupObject.GetComponent<PopupController>().ShowError(_msg);
        }
        public void HideLoading()
        {
            //Debug.Log("HideLoading");
            LoadingScreen.SetActive(false);
        }
        public bool isLoading()
        {
            return LoadingScreen.activeSelf;
        }
        public void ShowRating()
        {
            Rating.SetActive(true);
        }
        // registration
        public void ShowRegistration()
        {
            RegistrationObject.SetActive(true);
        }

        public void HideRegistration()
        {
            RegistrationObject.SetActive(false);
        }

        // login
        public void ShowLogin()
        {
            //Debug.Log("ShowLogin");
            LoginObject.SetActive(true);
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = true;
            Screen.orientation = ScreenOrientation.Portrait;
        }

        public void HideLogin()
        {
            //Debug.Log("HideLogin");
            LoginObject.SetActive(false);
            
        }
        public void OnLogout()
        {
            AppManager.USER_SETTINGS.Logout();
        }
        // user profile
        public void ShowUserProfile()
        {
            //UserProfileObject.GetComponent<Canvas>().worldCamera = GetMainCamera();
           // UserProfileObject.SetActive(true);
        }

        public void HideUserProfile()
        {
            //UserProfileObject.SetActive(false);
        }

        // message list
        public void ShowMessageList()
        {
           // MessegesListObject.SetActive(true);
        }

        public void HideMessageList()
        {
            //MessegesListObject.SetActive(false);
        }

        // another user profile
        public void ShowAnotherUserProfile(string _id)
        {
            //AnotherUserProfileObject.SetActive(true);
           // AnotherUserProfileObject.GetComponentInChildren<UserProfileLoader>().LoadUserInfo(_id);
           // AnotherUserProfileObject.GetComponentInChildren<FeedsDataLoader>().LoadUserContent(_id);
        }

        public void HideAnotherUserProfile()
        {
            //AnotherUserProfileObject.SetActive(false);
        }

        // show user friends
        public void ShowUserFriend(string _id)
        {
            //UserFriendsіListObject.SetActive(true);
           // UserFriendsіListObject.GetComponentInChildren<FriendsListLoader>().LoadUserFriends(_id);
        }

        public void HideUserFriends()
        {
            //UserFriendsіListObject.SetActive(false);
        }

        // feed preview
        public void ShowFeedPreview(FeedPreviewRequest _request)
        {
           // FeedPreviewObject.SetActive(true);
           // FeedPreviewObject.GetComponent<FeedPreviewController>().DisplayPreview(_request);
        }

        public void HideFeedPreview()
        {
            //FeedPreviewObject.SetActive(false);
        }

        // friend list
        public void ShowFriendsList()
        {
            //FriendListObject.SetActive(true);
        }

        public void HideFriendsList()
        {
           // FriendListObject.SetActive(false);
        }

        // settings
        public void ShowSettings()
        {
           // SettingsObject.SetActive(true);
        }

        public void HideSettings()
        {
           // SettingsObject.SetActive(false);
        }

        // navigation
        public void ShowNavigationPanel()
        {
            //NavigationPanelObject.SetActive(true);
            //AppManager.NAVIGATION.AddListeners();
            //if (AppManager.ADS_UI_CONTROLLER != null)
            //{
            //    AppManager.ADS_UI_CONTROLLER.HideBanner();
            //}
        }

        public void HideNavigationPanel()
        {
           // NavigationPanelObject.SetActive(false);
            //AppManager.NAVIGATION.RemoveListeners();
        }
        public bool isNavigationPanel()
        {
			return false;//NavigationPanelObject.activeInHierarchy;

        }

        // world news
        public void ShowWorldNews()
        {
            //WorldNewsObject.GetComponent<Canvas>().worldCamera = GetMainCamera();
            //WorldNewsObject.SetActive(true);
        }

        public void HideWorldNews()
        {
            //WorldNewsObject.SetActive(false);
        }

        // friends news
        public void ShowFriendsNews()
        {
            //FriendsNewsObject.GetComponent<Canvas>().worldCamera = GetMainCamera();
            //FriendsNewsObject.SetActive(true);
        }

        public void HideFriendsNews()
        {
            //FriendsNewsObject.SetActive(false);
        }

        // show messages with
        public void ShowMessagingWith(string _id)
        {
            HideNavigationPanel();
           // MessegingObject.SetActive(true);
           // MessegingObject.GetComponentInChildren<MessagesDataLoader>().LoadUserMessages(_id);
        }

        public void ShowMessagingWith(MessageGroupInfo _groupID)
        {
            HideNavigationPanel();
            //MessegingObject.SetActive(true);
            //MessegingObject.GetComponentInChildren<MessagesDataLoader>().LoadMessageGroup(_groupID);
        }

        public void HideUserMessanging()
        {
          // MessegingObject.SetActive(false);
        }

        // show post comments
        public void ShowPostComments(string _id)
        {
            HideNavigationPanel();
            //CommentsObject.SetActive(true);
           // CommentsObject.GetComponentInChildren<MessagesDataLoader>().LoadPostComments(_id);
        }

        public void HidePostComments()
        {
            //CommentsObject.SetActive(false);
        }

        public void HideEnterOnlyMobile()
        {
            CanvasLogin_EnterMobile.SetActive(false);
        }
        public void HideEnterReferralCode()
        {
            CanvasLogin_EnterReferralCode.SetActive(false);
        }
        public void HideEnterOTP()
        {
            CanvasLogin_EnterOTP.SetActive(false);
        }
        public void HideEnterName()
        {
            CanvasLogin_EnterName.SetActive(false);
        }
        public void ShowEnterOnlyMobile()
        {
            CanvasLogin_EnterMobile.SetActive(true);
        }
        public void ShowEnterReferralCode()
        {
            CanvasLogin_EnterReferralCode.SetActive(true);
        }
        public void ShowEnterOTP()
        {
            CanvasLogin_EnterOTP.SetActive(true);
#if UNITY_ANDROID && !UNITY_EDITOR
            if (!Permission.HasUserAuthorizedPermission("android.permission.RECEIVE_SMS"))
            {
                var callbacks = new PermissionCallbacks();
                callbacks.PermissionDenied += PermissionCallbacks_PermissionDenied;
                callbacks.PermissionGranted += PermissionCallbacks_PermissionGranted;
                callbacks.PermissionDeniedAndDontAskAgain += PermissionCallbacks_PermissionDeniedAndDontAskAgain;
                Permission.RequestUserPermission("android.permission.RECEIVE_SMS", callbacks);
               
            }
#endif
        }
        public void ShowEnterName()
        {
            CanvasLogin_EnterName.SetActive(true);
        }
        // show add chat window
        public void ShowAddNewChat()
        {
            //AddNewShatObject.SetActive(true);
           //AddNewShatObject.GetComponentInChildren<SelectFromFriendsLoader>().LoadWindow(AddNewChatType.ADD_NEW_CHAT);
            HideNavigationPanel();
        }

        public void ShowAddNewChatMembers(MessageGroupInfo _group)
        {
            //AddNewShatObject.SetActive(true);
            //AddNewShatObject.GetComponentInChildren<SelectFromFriendsLoader>().LoadWindow(AddNewChatType.ADD_NEW_MEMBERS, _group);
        }

        public void ShowChatMembers(MessageGroupInfo _group)
        {
           // AddNewShatObject.SetActive(true);
            //AddNewShatObject.GetComponentInChildren<SelectFromFriendsLoader>().LoadWindow(AddNewChatType.SHOW_CHAT_MEMBERS, _group);
        }

        public void HideAddNEwChat()
        {
            //AddNewShatObject.SetActive(false);
        }

        public void ShowFeedPopup(Action<FeedPopupAction> _action)
        {
           // FeedPopupObject.SetActive(true);
           // FeedPopupObject.GetComponent<FeedPopupViewController>().SetupWindows(_action);
        }

        public void HideFeedPopup()
        {
            //FeedPopupObject.SetActive(false);
        }

        public void StartCall(IncommingType _type, CallObject _call)
        {
            //CallWindowObject.SetActive(true);
            //CallWindowObject.GetComponent<CallController>().ShowIncomming(_type, _call);
        }

        public void HideCall()
        {
            //CallWindowObject.SetActive(false);
        }
        public void HideRating()
        {
            Rating.SetActive(false);
        }

        public bool IsCallWindowActive()
        {
			return false;//CallWindowObject.activeInHierarchy;
        }

        public void ShowPopupMessageForPurchase(PopupMessage _msg)
        {
            PopupObject.SetActive(true);
            PopupObject.GetComponent<PopupController>().ShowPurchase(_msg);
        }
        // all
        public void HideAllScreen()
        {
            HideLogin();
            HidePopupMessage();
            HideRegistration();
            HideLoading();
            HideFeedPreview();
            HideUserProfile();
            HideSettings();
            HideFriendsList();
            HideNavigationPanel();
            HideWorldNews();
            HideFriendsNews();
            HideAnotherUserProfile();
            HideUserFriends();
            HideUserMessanging();
            HidePostComments();
            HideAddNEwChat();
            HideFeedPopup();
            HideCall();
            HideRating();
            HideEnterName();
            HideEnterOnlyMobile();
            HideEnterReferralCode();
            HideEnterOTP();
            HideLogoutMessage();
            HideQuitMessage();
            HideVersion();
            HideMaintenance();
        }
        public void HideAllScreenExceptPopUp()
        {
            HideLogin();
            HideRegistration();
            HideLoading();
            HideFeedPreview();
            HideUserProfile();
            HideSettings();
            HideFriendsList();
            HideNavigationPanel();
            HideWorldNews();
            HideFriendsNews();
            HideAnotherUserProfile();
            HideUserFriends();
            HideUserMessanging();
            HidePostComments();
            HideAddNEwChat();
            HideFeedPopup();
            HideCall();
            HideRating();
            HideEnterName();
            HideEnterOnlyMobile();
            HideEnterReferralCode();
            HideEnterOTP();
            HideVersion();
            HideMaintenance();
        }

        // hide navigation group objects
        public void HideNavigationGroup()
        {
            HideUserProfile();
            HideSettings();
            HideFriendsList();
            HideWorldNews();
            HideFriendsNews();
            HideAnotherUserProfile();
            HideUserFriends();
            HideMessageList();
            HideUserMessanging();
            HidePostComments();
            HideEnterName();
            HideEnterOnlyMobile();
            HideEnterReferralCode();
            HideEnterOTP();
            HideVersion();
            HideMaintenance();
        }

        public void ShowPopupMSG(MessageCode _code, Action _callback = null)
        {
            PopupMessage msg = new PopupMessage();
            msg.Callback = _callback;
            switch (_code)
            {
                case MessageCode.EmptyEmail:
                    msg.Title = "Error";
                    msg.Message = "Email is empty";
                    break;
                case MessageCode.EmptyFirstName:
                    msg.Title = "Error";
                    msg.Message = "First Name is empty";
                    break;
                case MessageCode.EmptyLastName:
                    msg.Title = "Error";
                    msg.Message = "Last Name is empty";
                    break;
                case MessageCode.EmptyPassword:
                    msg.Title = "Error";
                    msg.Message = "Password is empty";
                    break;
                case MessageCode.PasswordNotMatch:
                    msg.Title = "Error";
                    msg.Message = "Passwords do not match";
                    break;
                case MessageCode.EmailNotValid:
                    msg.Title = "Error";
                    msg.Message = "Email is not valid";
                    break;
                case MessageCode.SmallPassword:
                    msg.Title = "Error";
                    msg.Message = "Password is too small. Min value is " + AppManager.APP_SETTINGS.MinAllowPasswordCharacters.ToString();
                    break;
                case MessageCode.RegistrationSuccess:
                    msg.Title = "Success";
                    msg.Message = "Registration Success!";
                    break;
                case MessageCode.RegistrationSuccessWithConfirm:
                    msg.Title = "Success";
                    msg.Message = "Registration Success! Please confirm your email address";
                    break;
                case MessageCode.VideoProcessing:
                    msg.Title = "Error";
                    msg.Message = "Video processing ...";
                    break;
                case MessageCode.MaxVideoSize:
                    msg.Title = "Error";
                    msg.Message = "Max allowed size is " + AppManager.APP_SETTINGS.MaxUploadVideoSizeMB.ToString() + " mb";
                    break;
                case MessageCode.FailedUploadFeed:
                    msg.Title = "Error";
                    msg.Message = "Fail to upload feed. Try again";
                    break;
                case MessageCode.EmailConfirm:
                    msg.Title = "Error";
                    msg.Message = "Please confirm your email address";
                    break;
                case MessageCode.FailedUploadImage:
                    msg.Title = "Error";
                    msg.Message = "Fail to upload image. Try again";
                    break;
                case MessageCode.SuccessPost:
                    msg.Title = "Success";
                    msg.Message = "Post add success";
                    break;
                case MessageCode.DeleteFeedOwnerError:
                    msg.Title = "Error";
                    msg.Message = "You are not the owner of this post";
                    break;
                case MessageCode.CallIsBisy:
                    msg.Title = "Line is bisy";
                    msg.Message = "User cannot speak now";
                    break;
                default:
                    Debug.Log("NOTHING");
                    break;
            }
            ShowPopupMessage(msg);
        }
		public void ShowBankPopupMSG(PopupMessage msg, Action _callback = null,int _new =0)
		{
			if(_callback!=null)
			{
				msg.Callback = _callback;
			}

			ShowBankPopupMessage(msg,_new);
		}
		public void ShowWithdrawPopupMSG(PopupMessage msg, Action _callback = null,int _new=0)
		{
			if(_callback!=null)
			{
				msg.Callback = _callback;
			}

			ShowWithdrawPopupMessage(msg,_new);
		}
        public void OnBackReferralCode()
        {
            AppManager.VIEW_CONTROLLER.HideAllScreen();
            AppManager.VIEW_CONTROLLER.ShowLogin();
            AppManager.USER_SETTINGS.Logout();
        }
        public void OnBackOTPCode()
        {
            AppManager.VIEW_CONTROLLER.HideAllScreen();
            AppManager.VIEW_CONTROLLER.ShowLogin();
            AppManager.USER_SETTINGS.Logout();
        }
        public void OnBackNameCode()
        {
            AppManager.VIEW_CONTROLLER.HideAllScreen();
            AppManager.VIEW_CONTROLLER.ShowLogin();
            AppManager.USER_SETTINGS.Logout();
        }
        public void OnBackMobileCode()
        {
            AppManager.VIEW_CONTROLLER.HideAllScreen();
            AppManager.VIEW_CONTROLLER.ShowLogin();
            AppManager.USER_SETTINGS.Logout();
        }
      
        private void OnCheckReferralCodeResponse(ReferralCodeResult _referralCodeResult)
        {
            //Debug.Log("_referralCodeResult " + _referralCodeResult.status);
            if (_referralCodeResult.status.Equals("200"))
            {
                if (_referralCodeResult.data != null)
                {
                    if (_referralCodeResult.data.isreferral)
                    {

                       
                        AppManager.VIEW_CONTROLLER.HideAllScreen();
                        AppManager.VIEW_CONTROLLER.OTPSend();
                        AppManager.VIEW_CONTROLLER.ShowEnterOTP();

                        StaticValues.mobileVerificationId = string.Empty;

                        if (PlayerSave.singleton != null)
                        {
                            PlayerSave.singleton.KhelTamashaGetUserDetails(StaticValues.FirebaseUserId, StaticValues.mobileVerificationId, OnKTLoginResponse, 0);
                        }

                    }
                    else
                    {
                        AppManager.VIEW_CONTROLLER.HideLoading();
                        AppManager.FIREBASE_CONTROLLER.ShowErrorMessage(_referralCodeResult.message);
                    }
                }
                else
                {
                    AppManager.VIEW_CONTROLLER.HideLoading();
                    AppManager.FIREBASE_CONTROLLER.ShowErrorMessage(_referralCodeResult.message);
                }
            }
            else
            {
                AppManager.VIEW_CONTROLLER.HideLoading();
                AppManager.FIREBASE_CONTROLLER.ShowErrorMessage(_referralCodeResult.message);
            }
        }
        public void UpdateAPK()
        {
            if (!string.IsNullOrEmpty(StaticValues.VersionUrl))
            {
                Application.OpenURL(StaticValues.VersionUrl);
            }
        }
    }

    public enum MessageCode
    {
        EmptyEmail,
        EmptyFirstName,
        EmptyLastName,
        EmptyPassword,
        PasswordNotMatch,
        EmailNotValid,
        SmallPassword,
        RegistrationSuccess,
        RegistrationSuccessWithConfirm,
        VideoProcessing,
        MaxVideoSize,
        FailedUploadFeed,
        FailedUploadImage,
        SuccessPost,
        EmailConfirm,
        DeleteFeedOwnerError,
        CallIsBisy
    }
}