using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocialApp;
using Firebase.Database;
using System;

namespace SocialApp
{

    public class AppManager : MonoBehaviour
    {

        // instance
        private static AppManager instance;
        public static AppManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<AppManager>();
                }
                return instance;
            }

        }

        // device
        public DeviceController Device;
        public static DeviceController DEVICE_CONTROLLER
        {
            get
            {
                return Instance.Device;
            }
        }

        // firebase
        public FirebaseController Firebase;
        public static FirebaseController FIREBASE_CONTROLLER
        {
            get
            {
                if (Instance == null)
                {
                    return null;
                }
                else
                {
                    return Instance.Firebase;
                }
            }
        }

        // registration
        public RegistrationController Registration;
        public static RegistrationController REGISTRATION_CONTROLLER
        {
            get
            {
                return Instance.Registration;
            }
        }

        // login
        public LoginController Login;
        public static LoginController LOGIN_CONTROLLER
        {
            get
            {
                return Instance.Login;
            }
        }

        // view
        public ViewController View;
        public static ViewController VIEW_CONTROLLER
        {
            get
            {
                return Instance.View;
            }
        }

       

        // app setings
        private AppSettings Settings;
        public static AppSettings APP_SETTINGS
        {
            get
            {
                if (Instance == null)
                    return null;
                if (Instance.Settings == null)
                {
                    Instance.Settings = Resources.Load<AppSettings>(AppSettings.AppSettingPath);
                }
                return Instance.Settings;
            }
        }

        // profile
        public ProfileController Profile;
        public static ProfileController USER_PROFILE
        {
            get
            {
                if (Instance == null)
                {
                    return null;
                }
                else
                {
                    return Instance.Profile;
                }

            }
        }

       
      

        // settings
        public SettingsController UserSettings;
        public static SettingsController USER_SETTINGS
        {
            get
            {
                return Instance.UserSettings;
            }
        }

        private event Action<GoogleCallbackLoginMessage> OnGLoginActions;
        private event Action<FacebookCallbackLoginMessage> OnFLoginActions;
        private event Action<PhoneCallbackLoginMessage> OnPLoginActions;
        private void Start()
        {
            
            Init();
            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
            RemoveCallListener();
        }

        private void AddListeners()
        {
            
            LOGIN_CONTROLLER.OnLogoutEvent += OnLogoutSuccess;
        }

        private void RemoveListeners()
        {
           
            LOGIN_CONTROLLER.OnLogoutEvent -= OnLogoutSuccess;
        }

        public void Init()
        {
            Debug.Log("Application.targetFrameRate " + Application.targetFrameRate);
            Application.targetFrameRate = 60;
            VIEW_CONTROLLER.HideAllScreen();
            VIEW_CONTROLLER.ShowLoading();
            FIREBASE_CONTROLLER.InitFirebase();
            
            StartCoroutine(WaitForFirebaseReady());
        }

        private IEnumerator WaitForFirebaseReady()
        {
            VIEW_CONTROLLER.ShowLoading();
            while (!FIREBASE_CONTROLLER.IsFirebaseInited())
            {
                yield return new WaitForFixedUpdate();
            }
            VIEW_CONTROLLER.HideLoading();
            CheckLogin();
        }

        private void CheckLogin()
        {
           
            int savedCred = PlayerPrefs.GetInt(Utils.LOGGED,Utils.NONE);
            //Debug.Log("savedCred " + savedCred);

            if (savedCred ==0)
            {
                VIEW_CONTROLLER.ShowLogin();
            }
            else //phone
            {
                AutoLogin();
            }
           
        }

        public void AutoLogin()
        {
            string savedEmail = PlayerPrefs.GetString(AppSettings.LoginSaveKey,string.Empty);//email is for mobile number
            string savedPassword = PlayerPrefs.GetString(AppSettings.PasswordSaveKey, string.Empty);// pwd is for auto login

            StaticValues.FirebaseUserId = savedEmail;
            StaticValues.mobileVerificationId = savedPassword;
            AppManager.VIEW_CONTROLLER.HideAllScreen();
            AppManager.VIEW_CONTROLLER.HideLogin();

            AppManager.VIEW_CONTROLLER.ShowLoading();
            if (PlayerSave.singleton != null)
            {
                PlayerSave.singleton.SaveUserName(StaticValues.UserNameValue);
                PlayerSave.singleton.SaveNewName(StaticValues.displayName);
                PlayerSave.singleton.SaveMobileId(StaticValues.phoneNumberWithoutPrefix);
                PlayerSave.singleton.SaveEmail(StaticValues.Email);
                PlayerSave.singleton.SaveUserId(StaticValues.FirebaseUserId);
                PlayerSave.singleton.SaveDistributionId(StaticValues.MyReferralCode);
                PlayerSave.singleton.SaveGender("Male");
                PlayerSave.singleton.SavePassword(StaticValues.FirebaseUserId);
            }

            if (PlayerSave.singleton != null)
            {
                PlayerSave.singleton.GetBannerDetails(PlayerSave.singleton.newID(), null);
            }
            if (PlayerSave.singleton != null)
            {
                PlayerSave.singleton.KhelTamashaGetUserDetails(StaticValues.FirebaseUserId, StaticValues.mobileVerificationId, OnKTLoginResponse, 0);
            }
        }
        public void AutoLoginGoogle()
        {
            
        }
       
        
        internal void OnKTLoginResponse(ServerUserDetailsResponseAddPot _serverUserDetailsResponse)
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
                    //PlayerSave.singleton.ShowErrorMessage(_serverUserDetailsResponse.message);

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
                    else if(_serverUserDetailsResponse.message.Contains("Oops. Something went wrong. Please try again later"))
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
        private void OnLogoutSuccess()
        {
            RemoveCallListener();
        }

        Query CallReference;

        private void AddCallListener()
        {
            //if (APP_SETTINGS._EnableVideoAudioCalls)
            //{
            //    CallReference = FIREBASE_CONTROLLER.GetCallReference().LimitToLast(1);
            //    CallReference.ChildAdded += HandleCallAdded;
            //}
        }

        private void RemoveCallListener()
        {
            //if (APP_SETTINGS._EnableVideoAudioCalls && CallReference != null)
            //{
            //    CallReference.ChildAdded -= HandleCallAdded;
            //    CallReference = null;
            //}
        }

        void HandleCallAdded(object sender, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null)
            {
                return;
            }
            else
            {
                CallObject _callMsg = JsonUtility.FromJson<CallObject>(args.Snapshot.GetRawJsonValue());

                // check is call valid
                AppManager.FIREBASE_CONTROLLER.GetServerTimestamp(_msg =>
                {
                    string _time = _msg.Data;
                    long timeStamp;
                    bool isInteger = long.TryParse(_time, out timeStamp);

                    if (isInteger)
                    {
                        long callTimeStamp;
                        bool IsSucces = long.TryParse(_callMsg.CreateTimeStamp, out callTimeStamp);
                        if (IsSucces)
                        {
                            long _timePassed = (long)Mathf.Abs(timeStamp - callTimeStamp);
                            int _timePassedSeconds = (int)_timePassed / 100;
                            if (_timePassedSeconds < (long)AppSettings.IncomingCallMaxTime)
                            {
                                if (_callMsg.IsActive)
                                {
                                    if (VIEW_CONTROLLER.IsCallWindowActive())
                                    {
                                        // send bisy
                                        FIREBASE_CONTROLLER.SetCallBisy(_callMsg);
                                    }
                                    else
                                    {
                                        VIEW_CONTROLLER.StartCall(IncommingType.ANSWERS, _callMsg);
                                    }
                                }
                            }
                        }
                    }
                });
            }
        }

        public void RaiseOnGoogleLoginActions(GoogleCallbackLoginMessage _callback)
        {
            OnGLoginActions?.Invoke(_callback);
        }
        /// <summary>
        /// Subscribe to log in event
        /// </summary>
        /// <param name="_callback">Subscribe method</param>
        public void AddOnGoogleLoginAction(Action<GoogleCallbackLoginMessage> _callback)
        {
            OnGLoginActions += _callback;
        }

        /// <summary>
        /// Remove subscribe to log in event
        /// </summary>
        /// <param name="_callback">Subscribe method</param>
        public void RemoveOnGoogleLoginAction(Action<GoogleCallbackLoginMessage> _callback)
        {
            OnGLoginActions -= _callback;
        }
        /// <summary>
        /// Subscribe to log in event
        /// </summary>
        /// <param name="_callback">Subscribe method</param>
        public void AddOnFacebookLoginAction(Action<FacebookCallbackLoginMessage> _callback)
        {
            OnFLoginActions += _callback;
        }

        /// <summary>
        /// Remove subscribe to log in event
        /// </summary>
        /// <param name="_callback">Subscribe method</param>
        public void RemoveOnFacebookLoginAction(Action<FacebookCallbackLoginMessage> _callback)
        {
            OnFLoginActions -= _callback;
        }
        /// <summary>
        /// Raises a facebook Login data event
        /// </summary>
        /// <param name="_callbak">Callback response object</param>
        public void RaiseOnFacebookLoginActions(FacebookCallbackLoginMessage _callback)
        {
            OnFLoginActions?.Invoke(_callback);
        }


        public void AddOnPhoneLoginAction(Action<PhoneCallbackLoginMessage> _callback)
        {
            OnPLoginActions += _callback;
        }

      
        public void RemoveOnPhoneLoginAction(Action<PhoneCallbackLoginMessage> _callback)
        {
            OnPLoginActions -= _callback;
        }
       
        public void RaiseOnPhoneLoginActions(PhoneCallbackLoginMessage _callback)
        {
            OnPLoginActions?.Invoke(_callback);
        }

        public void FacebookLogIn()
        {
           
        }
        public void CallFBLogin()
        {
           
        }
        public void GoogleLogIn()
        {
            
        }
    }
}
