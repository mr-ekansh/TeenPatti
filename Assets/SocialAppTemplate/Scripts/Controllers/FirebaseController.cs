using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity;
using Firebase.Storage;
using Firebase;
using System;
using System.Threading.Tasks;
using System.Linq;
using Firebase.Functions;
using Firebase.Extensions;
using System.Text.RegularExpressions;
using Google;
//using Facebook.Unity;
using System.Text;
using UnityEngine.SceneManagement;
using Firebase.DynamicLinks;
using UnityEngine.Networking;
using Firebase.AppCheck;
namespace SocialApp
{

    public class FirebaseController : MonoBehaviour
    {

        public FirebaseAuth Auth;
        protected FirebaseAuth otherAuth;
        private FirebaseDatabase Database;
        private FirebaseStorage Storage;
        private FirebaseFunctions Functions;
        private string CurrentDeviceToken;
        Firebase.Auth.FirebaseUser user;
        private bool FirebaseIsInited = false;
        public bool isDebug = true;
        private bool fetchingToken = false;
        private bool ForOnceRelauchOnly = false;
        FirebaseApp app;



        // Options used to setup secondary authentication object.
        private Firebase.AppOptions otherAuthOptions = new Firebase.AppOptions
        {
            ApiKey = "",
            AppId = "",
            ProjectId = ""
        };
        protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth = new Dictionary<string, Firebase.Auth.FirebaseUser>();
        public void InitFirebase()
        {
            OnFirebaseInit();
        }
     
        // init
        private void OnFirebaseInit()
        {
            // Configure the Debug Provider factory with your debug token.
            
           
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                   
                    FirebaseIsInited = true;

                    InitPushNotificationEvents();
                    

                    CleanTask(task);
                    UnityEngine.Debug.Log(System.String.Format("Resolve all Firebase dependencies: {0}", dependencyStatus));
                }
                else
                {
                    
                    CleanTask(task);
                    UnityEngine.Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                }
            });
        }
        

        private static Dictionary<string, string> parameters = new Dictionary<string, string>();

        public static bool HasKey(string key)
        {
            return parameters.ContainsKey(key);
        }

        // This can be called from Start(), but not earlier
        public static string GetValue(string key)
        {
            return parameters[key];
        }

        

        public void SetRequestParameters(string parametersString)
        {
            FirebaseController.parameters = new Dictionary<string, string>();
            char[] parameterDelimiters = new char[] { '?', '&' };
            string[] parameters = parametersString.Split(parameterDelimiters, System.StringSplitOptions.RemoveEmptyEntries);


            char[] keyValueDelimiters = new char[] { '=' };
            for (int i = 0; i < parameters.Length; ++i)
            {
                string[] keyValue = parameters[i].Split(keyValueDelimiters, System.StringSplitOptions.None);

                if (keyValue.Length >= 2)
                {
                    FirebaseController.parameters.Add(UnityWebRequest.UnEscapeURL(keyValue[0]), UnityWebRequest.UnEscapeURL(keyValue[1]));
                }
                else if (keyValue.Length == 1)
                {
                    FirebaseController.parameters.Add(UnityWebRequest.UnEscapeURL(keyValue[0]), "");
                }
            }
        }

        // Display the dynamic link received by the application.
        void OnDynamicLink(object sender, EventArgs args)
        {
            var dynamicLinkEventArgs = args as ReceivedDynamicLinkEventArgs;
            Debug.Log(String.Format("Received dynamic link {0}",
                                   dynamicLinkEventArgs.ReceivedDynamicLink.Url.OriginalString));

           

        }

        public void OtherOptions()
        {
            if (otherAuthOptions != null &&
            !(String.IsNullOrEmpty(otherAuthOptions.ApiKey) || String.IsNullOrEmpty(otherAuthOptions.AppId) || String.IsNullOrEmpty(otherAuthOptions.ProjectId)))
            {
                try
                {
                    otherAuth = Firebase.Auth.FirebaseAuth.GetAuth(Firebase.FirebaseApp.Create(
                      otherAuthOptions, "Secondary"));
                  
                    otherAuth.IdTokenChanged += IdTokenChanged;
                    if (isDebug)
                    {
                        Debug.Log("ERROR: Success to initialize OtherOptions object.");
                    }
                }
                catch (Exception)
                {
                    if (isDebug)
                    {
                        Debug.LogError("ERROR: Failed to initialize secondary authentication object.");
                    }
                }
            }
           
        }
        void IdTokenChanged(object sender, System.EventArgs eventArgs)
        {
            Debug.Log("run firebase functions");
            Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
            if (senderAuth == Auth && senderAuth.CurrentUser != null && !fetchingToken)
            {
                senderAuth.CurrentUser.TokenAsync(false).ContinueWithOnMainThread(
                  task =>
                  {
                      Debug.Log(string.Format("Full Token[0:8] = {0}", task.Result));
                      Debug.Log(string.Format("Token[0:8] = {0}", task.Result.Substring(0, 8)));
                      StaticValues.token = task.Result;
                  });
               
            }
            else
            {
                if (isDebug)
                {
                    Debug.Log("Failed IdTokenChanged");
                }


            }
        }
       
        internal void AfterLogout()
        {
            StaticValues.BlockCreatingDoubleProcess = false;

            StaticValues.Email = "";
            StaticValues.phoneNumber = "";
            StaticValues.phoneNumberWithoutPrefix = "";
            StaticValues.displayName = "";
            StaticValues.displayNameinUC = "";

            StaticValues.ReferralCode = "";
            StaticValues.MyReferralCode = "";
            StaticValues.CountryCode = "";
            StaticValues.custom = "";

            StaticValues.FirebaseUserId = "";
            StaticValues.customPicUrl = "";
            StaticValues.avatarPicUrl = "";
            StaticValues.ProviderId = "";
            StaticValues.HaveReferralCode = false;
            ForOnceRelauchOnly = false;
            PlayerPrefs.DeleteAll();
            Caching.ClearCache();
            StaticValues.UserNameValue = "";
            StaticValues.FirstNameValue = "";
            StaticValues.LastNameValue = "";
            StaticValues.GenderValue = "";
            StaticValues.MobileValue = "";
            StaticValues.Email = "";
            StaticValues.DOBValue = "";
            StaticValues.StreetValue_1 = "";
            StaticValues.StreetValue_2 = "";
            StaticValues.CityValue = "";
            StaticValues.StateValue = "";
            StaticValues.PinCodeValue = "";
            StaticValues.BankAccountNo = "";
            StaticValues.BankIFSCCode = "";
            StaticValues.AddressNo = "";
            StaticValues.PanDocNo = "";
            StaticValues.PanCardStatus = 0;
            StaticValues.AddressStatus = 0;
            StaticValues.TotalEarningAmount = 0.0;
            AppManager.VIEW_CONTROLLER.HideAllScreen();
            AppManager.VIEW_CONTROLLER.ClearText();
            StaticValues.isEmailVerify = false;
            StaticValues.isMobileVerify = false;
            StaticValues.OnlyOncePerSession = false;
            StaticValues.version = 0;
            StaticValues.VersionUrl = "";
            StaticValues.BotHand = 5;
            StaticValues.ismaintenance = 0;
            StaticValues.isBankStatusForNewRequest = "";
            StaticValues.isBankDetailsSubmitted = false;
            StaticValues.isBankUPIStatusForNewRequest = "";
            StaticValues.isBankUPIDetailsSubmitted = false;
            StaticValues.BankAccountNo_NR = "";
            StaticValues.BankIFSCCode_NR = "";
            StaticValues.BankUPIId = "";
            StaticValues.BankUPIId_NR = "";
            StaticValues.FirstTimeDepositPrompt = false;
            StaticValues.FirstTimeDepositPromptClose = false;
            StaticValues.FirstTimeDepositPromptOpen = false;
            StaticValues.OutOfLimitPopUp = false;
            StaticValues.StaticTrans_Continue = 0;
            StaticValues.mobileVerificationId = string.Empty;
            StaticValues.HaveReferralCode = false;
            if (AppManager.Instance != null)
            {
                AppManager.Instance.Init();
            }
            AppManager.VIEW_CONTROLLER.ShowLogin();
            AppManager.LOGIN_CONTROLLER.googlMessage = null;
            AppManager.LOGIN_CONTROLLER.facebookMessage = null;
            AppManager.LOGIN_CONTROLLER.phoneMessage = null;

            SceneManager.LoadScene("EmptyScene");

            Invoke("WaitFor2SecFoRPhone", 2f);
        }
        void WaitFor2SecFoRPhone()
        {
            if (UnitySMSReceiver.sMSReceiver != null)
            {
                UnitySMSReceiver.sMSReceiver.CallFromSignOut();
            }
			Screen.autorotateToLandscapeLeft = false;
			Screen.autorotateToLandscapeRight = false;
			Screen.autorotateToPortrait = true;
			Screen.autorotateToPortraitUpsideDown = true;
			Screen.orientation = ScreenOrientation.Portrait;
        }
        
       
        public void InitPushNotificationEvents()
        {
            Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
            Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
            Firebase.Messaging.FirebaseMessaging.GetTokenAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    CurrentDeviceToken = task.Result;
                }
                //Debug.Log("CurrentDeviceToken Manually............ " + CurrentDeviceToken + " task.Result " + task.Result);
                RegisterDeviceTokens();
                if (task != null)
                {
                    CleanTask(task);
                }
            });
        }

        public void RemovePushNotificationEvents()
        {
            Firebase.Messaging.FirebaseMessaging.TokenReceived -= OnTokenReceived;
            Firebase.Messaging.FirebaseMessaging.MessageReceived -= OnMessageReceived;
        }

        public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
        {
            UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
            Debug.Log("CurrentDeviceToken............ " + CurrentDeviceToken);
            CurrentDeviceToken = token.Token;
            RegisterDeviceTokens();
        }

        public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
        {
            UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
        }

        public void RegisterDeviceTokens()
        {
            StaticValues.token = CurrentDeviceToken;
            Debug.Log("CurrentDeviceToken " + CurrentDeviceToken);
            if (string.IsNullOrEmpty(CurrentDeviceToken))
                return;
            //Send token to server 
            if (!string.IsNullOrEmpty(StaticValues.FirebaseUserId))
            {
                PlayerSave.singleton.UpdateTokenAPICall(StaticValues.FirebaseUserId, CurrentDeviceToken);
            }
        }

        public void RemoveDeviceTokens()
        {
            //if (string.IsNullOrEmpty(CurrentDeviceToken))
            //    return;
            //DatabaseReference _tokenRef = Database.RootReference.Child(AppSettings.DeviceTokensKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).Child(CurrentDeviceToken);
            //_tokenRef.RemoveValueAsync();
        }

        public void ClearDeviceToken()
        {
            CurrentDeviceToken = string.Empty;
        }

        public void GetUserDeviceTokes(string _userId, Action<List<string>> _callback)
        {
            DatabaseReference _tokenRef = Database.RootReference.Child(AppSettings.DeviceTokensKey).Child(_userId);
            _tokenRef.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {

                }
                else if (task.IsCompleted && task.Result.Exists)
                {
                    List<string> _ids = new List<string>();
                    DataSnapshot snapshot = task.Result;
                    foreach (DataSnapshot _data in snapshot.Children)
                    {
                        _ids.Add(_data.Key);
                    }

                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_ids));
                }
            });
        }

        public void SendPushNotification(NotificationMessage _message)
        {
            string _userID = _message.UserId;
            var data = new Dictionary<string, object>();
            data["_userId"] = _userID;
            data["_title"] = _message.Title;
            data["_body"] = _message.Body;

            // Call the function and extract the operation from the result.
            HttpsCallableReference _function = Functions.GetHttpsCallable("SendFCM");
            _function.CallAsync(data);

        }

        // add new user
        public void AddNewUser(string _email, string _password, Action<RegistrationMessage> _callback)
        {
            RegistrationMessage _regMsg = new RegistrationMessage();
            Auth.CreateUserWithEmailAndPasswordAsync(_email, _password).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    _regMsg.ErrorMessage = "CreateUserWithEmailAndPasswordAsync was canceled";
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_regMsg));
                    CleanTask(task);
                    return;
                }
                if (task.IsFaulted)
                {
                    _regMsg.ErrorMessage = "CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception;
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_regMsg));
                    CleanTask(task);
                    return;
                }
                Firebase.Auth.FirebaseUser newUser = task.Result.User;
                _regMsg.UserID = newUser.UserId;
                _regMsg.IsComplete = true;
                UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_regMsg));
                CleanTask(task);
            });
        }

        // get user data
        public void GetUserData(string _userID, Action<User> _callback)
        {
            Query databaseQuery = Database.RootReference.Child(AppSettings.RootUserKey).Child(_userID);
            databaseQuery.GetValueAsync().ContinueWith(task =>
            {

                if (task.IsCompleted)
                {
                
                    try
                    {
                        if (!string.IsNullOrEmpty(task.Result.GetRawJsonValue()))
                        {
                            User _user = JsonUtility.FromJson<User>(task.Result.GetRawJsonValue().ToString());
                            if (_callback != null)
                            {
                                Debug.Log("callback task IsCompleted");
                                UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_user));
                            }
                        }
                        else
                        {
                            User _user = new User();
                            _user.UserID = "";
                            if (_callback != null)
                            {
                                Debug.Log("catch callback task IsCompleted");
                                UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_user));
                            }
                        }

                    }
                    catch(Exception e)
                    {
                        
                        User _user = new User();
                        _user.UserID = "";
                        if (_callback != null)
                        {
                            Debug.Log("catch callback task IsCompleted"+e.Message);
                            UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_user));
                        }
                    }
                   
                    CleanTask(task);

                }
                else
                {
                    
                    User _user = new User();
                    _user.UserID = "";
                    if (_callback != null)
                    {
                        Debug.Log("else callback task IsCompleted");
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_user));
                    }
                    CleanTask(task);
                }
               
            });
        }


        // set user data
        public void SetUserData(User _user, Action<SetUserDataMessage> _callback)
        {
            string json = JsonUtility.ToJson(_user);
            SetUserDataMessage _logMsg = new SetUserDataMessage();
            Database.RootReference.Child(AppSettings.RootUserKey).Child(_user.UserID).SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    _logMsg.ErrorMessage = "Set user data was canceled";
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_logMsg));
                    CleanTask(task);
                    return;
                }
                if (task.IsFaulted)
                {
                    _logMsg.ErrorMessage = "Set User Data encountered an error: " + task.Exception;
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_logMsg));
                    CleanTask(task);
                    return;
                }
                _logMsg.IsSuccess = true;
                _logMsg.UserID = _user.UserID;
                UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_logMsg));
                CleanTask(task);
            });
        }
        
        
        // get users who like post
        public void GetUsersWhoLikePost(string _postID, Action<List<User>> _callback)
        {
            Query databaseQuery;
            string rootKey = AppSettings.PostLikesKey;
            databaseQuery = Database.RootReference.Child(rootKey).Child(_postID).Child(AppSettings.ContainerListKey);


            List<User> _users = new List<User>();

            databaseQuery
                .GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(() => AppManager.VIEW_CONTROLLER.HideLoading());
                        CleanTask(task);
                    }
                    else if (task.IsCompleted && task.Result.Exists)
                    {
                        DataSnapshot snapshot = task.Result;


                        int _userCount = (int)snapshot.ChildrenCount;

                        List<Task> TaskList = new List<Task>();

                        for (int i = 0; i < _userCount; i++)
                        {
                            DataSnapshot userSnapshot = snapshot.Children.ElementAt(i);
                            string _userID = userSnapshot.Key;
                            DatabaseReference _friendRef = Database.RootReference.Child(AppSettings.RootUserKey).Child(_userID);
                            Task<DataSnapshot> _task = _friendRef.GetValueAsync();
                            TaskList.Add(_task);

                        }
                        Task.WhenAll(TaskList).ContinueWith(task2 =>
                        {
                            if (task2.IsCompleted)
                            {
                                foreach (Task<DataSnapshot> t in TaskList)
                                {
                                    if (t.IsCompleted && t.Result.Exists)
                                    {
                                        string _userId = t.Result.Key;
                                        string jsonUser = t.Result.GetRawJsonValue();
                                        if (!string.IsNullOrEmpty(jsonUser))
                                        {
                                            User _dataUser = JsonUtility.FromJson<User>(jsonUser);
                                            _users.Add(_dataUser);

                                        }
                                        CleanTask(t);
                                    }
                                }
                            }

                            UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_users));
                            CleanTask(task2);
                        });
                        CleanTask(task);
                    }
                });
        }


        // login
        public void LogIn(string _login, string _password, Action<LoginMessage> _callback, bool _silentMode = false)
        {
            LoginMessage _logMsg = new LoginMessage();
            Auth.SignInWithEmailAndPasswordAsync(_login, _password).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    _logMsg.IsSuccess = false;
                    _logMsg.ErrorMessage = "SignInWithEmailAndPasswordAsync was canceled.";
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_logMsg));
                    CleanTask(task);
                    return;
                }
                if (task.IsFaulted)
                {
                    _logMsg.IsSuccess = false;
                    _logMsg.ErrorMessage = task.Exception.Message;
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_logMsg));
                    CleanTask(task);
                    return;
                }
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    FirebaseUser newUser = task.Result.User;
                    if (AppManager.APP_SETTINGS.UseEmailConfirm && !_silentMode)
                    {
                        if (newUser.IsEmailVerified)
                        {
                            _logMsg.IsSuccess = true;
                            _logMsg.UserID = newUser.UserId;
                            _logMsg.FUser = newUser;
                            UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_logMsg));
                        }
                        else
                        {
                            AppManager.VIEW_CONTROLLER.ShowPopupMSG(MessageCode.EmailConfirm, AppManager.VIEW_CONTROLLER.ShowLogin);
                            AppManager.VIEW_CONTROLLER.HideLoading();
                            LogOut();
                        }
                        CleanTask(task);
                    }
                    else
                    {
                        _logMsg.IsSuccess = true;
                        _logMsg.UserID = newUser.UserId;
                        _logMsg.FUser = newUser;
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_logMsg));
                        CleanTask(task);
                    }
                });
            });
        }

        public void LogOut()
        {
           
            PlayerPrefs.SetInt(Utils.LOGGED, Utils.NONE);
            PlayerPrefs.Save();
            AfterLogout();
        }

        public void SendVerifcationEmail()
        {
            FirebaseAuth.DefaultInstance.CurrentUser.SendEmailVerificationAsync().ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SendEmailVerificationAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SendEmailVerificationAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("Email sent successfully.");
            });
        }

        public void UploadAvatar(UploadImageRequest _request, Action<UploadImageCallBack> _callback)
        {
            UploadImageCallBack uploadCallback = new UploadImageCallBack();
            Firebase.Storage.StorageReference avatar_ref = Storage.RootReference.Child(AppSettings.RootUserStorageKey).Child(_request.OwnerId).Child(AppSettings.UserAvatarKey + "/" + "Image_" + _request.Size.ToString() + ".jpg");
            avatar_ref.PutBytesAsync(_request.ImageBytes)
                .ContinueWith((task) =>
                {
                    if (task.IsFaulted || task.IsCanceled)
                    {
                        uploadCallback.IsFinish = true;
                        uploadCallback.IsSuccess = false;
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(uploadCallback));
                        CleanTask(task);
                    }
                    else
                    {
                        uploadCallback.IsFinish = true;
                        uploadCallback.IsSuccess = true;
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(uploadCallback));
                        CleanTask(task);
                    }
                });
        }

        public void GetProfileImage(GetProfileImageRequest _request, Action<GetProfileImageCallback> _callback)
        {
            GetProfileImageCallback _profileCallback = new GetProfileImageCallback();
            const long maxAllowedSize = 1 * 2048 * 2048;
            Firebase.Storage.StorageReference avatar_ref = Storage.RootReference.Child(AppSettings.RootUserStorageKey).Child(_request.Id).Child(AppSettings.UserAvatarKey + "/" + "Image_" + _request.Size.ToString() + ".jpg");
            avatar_ref.GetBytesAsync(maxAllowedSize).ContinueWith((Task<byte[]> task) =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    _profileCallback.IsSuccess = false;
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_profileCallback));
                    CleanTask(task);
                }
                else
                {
                    byte[] fileContents = task.Result;
                    _profileCallback.IsSuccess = true;
                    _profileCallback.ImageBytes = fileContents;
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_profileCallback));
                    CleanTask(task);
                }
            });
        }

        public void GetProfileImageUrl(GetProfileImageRequest _request, Action<GetProfileImageCallback> _callback)
        {
            GetProfileImageCallback _profileCallback = new GetProfileImageCallback();
            Firebase.Storage.StorageReference avatar_ref = Storage.RootReference.Child(AppSettings.RootUserStorageKey).Child(_request.Id).Child(AppSettings.UserAvatarKey + "/" + "Image_" + _request.Size.ToString() + ".jpg");
            avatar_ref.GetDownloadUrlAsync().ContinueWith((Task<Uri> task) =>
            {
                if (!task.IsFaulted && !task.IsCanceled)
                {
                    _profileCallback.IsSuccess = true;
                    _profileCallback.DownloadUrl = task.Result.ToString();
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_profileCallback));
                    CleanTask(task);
                }
                else
                {
                    _profileCallback.IsSuccess = false;
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_profileCallback));
                    CleanTask(task);
                }
            });
        }

        public void GetFeedsAt(FeedQuery _feedQuery)
        {
            Query databaseQuery;
            if (string.IsNullOrEmpty(_feedQuery.indexKey))
            {
                databaseQuery = Database.RootReference.Child(AppSettings.UserPostsKey).Child(_feedQuery.ownerID).OrderByKey().LimitToLast(_feedQuery.endIndex);
            }
            else if (_feedQuery.forward)
            {
                int count = _feedQuery.endIndex - _feedQuery.startIndex + 1;
                databaseQuery = Database.RootReference.Child(AppSettings.UserPostsKey).Child(_feedQuery.ownerID).OrderByKey().EndAt(_feedQuery.indexKey).LimitToLast(count);
            }
            else
            {
                int count = _feedQuery.endIndex - _feedQuery.startIndex + 1;
                databaseQuery = Database.RootReference.Child(AppSettings.UserPostsKey).Child(_feedQuery.ownerID).OrderByKey().EndAt(_feedQuery.indexKey).LimitToLast(count);
            }
            FeedCallback _callback = new FeedCallback();
            databaseQuery
                .GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        _callback.IsSuccess = false;
                    // Handle the error...
                    UnityMainThreadDispatcher.Instance().Enqueue(() => AppManager.VIEW_CONTROLLER.HideLoading());
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _feedQuery.callback.Invoke(_callback));
                        CleanTask(task);
                    }
                    else if (task.IsCompleted && task.Result.Exists)
                    {

                        DataSnapshot snapshot = task.Result;
                        List<Feed> feeds = new List<Feed>();

                        int _feedCount = (int)snapshot.ChildrenCount;

                        List<Task> TaskList = new List<Task>();

                        for (int i = 0; i < _feedCount; i++)
                        {
                            DataSnapshot feedSnapshot = snapshot.Children.ElementAt(i);
                            string _feedId = feedSnapshot.Key;
                            DatabaseReference _feedRef = Database.RootReference.Child(AppSettings.AllPostsKey).Child(_feedId);
                            Task<DataSnapshot> _task = _feedRef.GetValueAsync();
                            TaskList.Add(_task);

                        }
                        Task.WhenAll(TaskList).ContinueWith(task2 =>
                        {
                            if (task2.IsCompleted)
                            {
                                foreach (Task<DataSnapshot> t in TaskList)
                                {
                                    if (t.IsCompleted && t.Result.Exists)
                                    {
                                        string _feedId = t.Result.Key;
                                        string jsonFeed = t.Result.GetRawJsonValue();
                                        if (!string.IsNullOrEmpty(jsonFeed))
                                        {
                                            Feed _dataFeed = JsonUtility.FromJson<Feed>(jsonFeed);

                                            if (_dataFeed != null)
                                            {
                                                _dataFeed.Key = _feedId;
                                                if (_feedQuery.forward)
                                                {
                                                    if (_feedId != _feedQuery.indexKey)
                                                    {
                                                        feeds.Add(_dataFeed);
                                                    }
                                                }
                                                else
                                                {
                                                    feeds.Add(_dataFeed);
                                                }
                                            }
                                        }
                                        CleanTask(t);
                                    }
                                }
                                CleanTask(task);
                            }
                            _callback.IsSuccess = true;
                            feeds = feeds.OrderBy(p => DateTime.Parse(p.DateCreated)).ToList();
                            feeds.Reverse();
                            if (!_feedQuery.forward)
                            {
                                feeds.Reverse();
                            }

                            _callback.feeds = feeds;
                            _callback.forward = _feedQuery.forward;
                            UnityMainThreadDispatcher.Instance().Enqueue(() => _feedQuery.callback.Invoke(_callback));
                        });
                        CleanTask(task);
                    }
                });
        }

        public void GetFriendsFeedsAt(FeedQuery _feedQuery)
        {
            Query databaseQuery;
            if (string.IsNullOrEmpty(_feedQuery.indexKey))
            {
                databaseQuery = Database.RootReference.Child(AppSettings.FriendsPostsKey).Child(_feedQuery.ownerID).Child(AppSettings.ContainerListKey).OrderByKey().LimitToLast(_feedQuery.endIndex);
            }
            else if (_feedQuery.forward)
            {
                int count = _feedQuery.endIndex - _feedQuery.startIndex + 1;
                databaseQuery = Database.RootReference.Child(AppSettings.FriendsPostsKey).Child(_feedQuery.ownerID).Child(AppSettings.ContainerListKey).OrderByKey().EndAt(_feedQuery.indexKey).LimitToLast(count);
            }
            else
            {
                int count = _feedQuery.endIndex - _feedQuery.startIndex + 1;
                databaseQuery = Database.RootReference.Child(AppSettings.FriendsPostsKey).Child(_feedQuery.ownerID).Child(AppSettings.ContainerListKey).OrderByKey().EndAt(_feedQuery.indexKey).LimitToLast(count);
            }
            FeedCallback _callback = new FeedCallback();
            databaseQuery
                .GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        _callback.IsSuccess = false;
                    // Handle the error...
                    UnityMainThreadDispatcher.Instance().Enqueue(() => AppManager.VIEW_CONTROLLER.HideLoading());
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _feedQuery.callback.Invoke(_callback));
                        CleanTask(task);
                    }
                    else if (task.IsCompleted && task.Result.Exists)
                    {

                        DataSnapshot snapshot = task.Result;
                        List<Feed> feeds = new List<Feed>();

                        int _feedCount = (int)snapshot.ChildrenCount;

                        List<Task> TaskList = new List<Task>();

                        for (int i = 0; i < _feedCount; i++)
                        {
                            DataSnapshot feedSnapshot = snapshot.Children.ElementAt(i);
                            string _feedId = feedSnapshot.Key;
                            DatabaseReference _feedRef = Database.RootReference.Child(AppSettings.AllPostsKey).Child(_feedId);
                            Task<DataSnapshot> _task = _feedRef.GetValueAsync();
                            TaskList.Add(_task);

                        }
                        Task.WhenAll(TaskList).ContinueWith(task2 =>
                        {
                            if (task2.IsCompleted)
                            {
                                foreach (Task<DataSnapshot> t in TaskList)
                                {
                                    if (t.IsCompleted && t.Result.Exists)
                                    {
                                        string _feedId = t.Result.Key;
                                        string jsonFeed = t.Result.GetRawJsonValue();
                                        if (!string.IsNullOrEmpty(jsonFeed))
                                        {
                                            Feed _dataFeed = JsonUtility.FromJson<Feed>(jsonFeed);

                                            if (_dataFeed != null)
                                            {
                                                _dataFeed.Key = _feedId;
                                                if (_feedQuery.forward)
                                                {
                                                    if (_feedId != _feedQuery.indexKey)
                                                    {
                                                        feeds.Add(_dataFeed);
                                                    }
                                                }
                                                else
                                                {
                                                    feeds.Add(_dataFeed);
                                                }
                                            }
                                        }
                                        CleanTask(t);
                                    }
                                }
                                CleanTask(task);
                            }
                            _callback.IsSuccess = true;
                            feeds = feeds.OrderBy(p => DateTime.Parse(p.DateCreated)).ToList();
                            feeds.Reverse();
                            if (!_feedQuery.forward)
                            {
                                feeds.Reverse();
                            }

                            _callback.feeds = feeds;
                            _callback.forward = _feedQuery.forward;
                            UnityMainThreadDispatcher.Instance().Enqueue(() => _feedQuery.callback.Invoke(_callback));
                        });
                        CleanTask(task);
                    }
                });
        }

        public void GetFriendsAt(UsersQuery _friendQuery)
        {
            Query databaseQuery;
            string rootKey = AppSettings.UserFriendsKey;
            if (_friendQuery.Type == FriendsTabState.Pending)
                rootKey = AppSettings.UserPendingFriendsKey;
            if (_friendQuery.Type == FriendsTabState.Requested)
                rootKey = AppSettings.UserRequestedFriendsKey;
            if (string.IsNullOrEmpty(_friendQuery.indexKey))
            {
                databaseQuery = Database.RootReference.Child(rootKey).Child(_friendQuery.ownerID).Child(AppSettings.ContainerListKey).OrderByKey().LimitToLast(_friendQuery.endIndex);
            }
            else if (_friendQuery.forward)
            {
                int count = _friendQuery.endIndex - _friendQuery.startIndex + 1;
                databaseQuery = Database.RootReference.Child(rootKey).Child(_friendQuery.ownerID).Child(AppSettings.ContainerListKey).OrderByKey().EndAt(_friendQuery.indexKey).LimitToLast(count);
            }
            else
            {
                int count = _friendQuery.endIndex - _friendQuery.startIndex + 1;
                databaseQuery = Database.RootReference.Child(rootKey).Child(_friendQuery.ownerID).Child(AppSettings.ContainerListKey).OrderByKey().EndAt(_friendQuery.indexKey).LimitToLast(count);
            }

            UsersCallback _callback = new UsersCallback();
            databaseQuery
                .GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        _callback.IsSuccess = false;
                    // Handle the error...
                    UnityMainThreadDispatcher.Instance().Enqueue(() => AppManager.VIEW_CONTROLLER.HideLoading());
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _friendQuery.callback.Invoke(_callback));
                        CleanTask(task);
                    }
                    else if (task.IsCompleted && task.Result.Exists)
                    {
                        DataSnapshot snapshot = task.Result;
                        List<User> friends = new List<User>();

                        int _friendsCount = (int)snapshot.ChildrenCount;

                        List<Task> TaskList = new List<Task>();

                        for (int i = 0; i < _friendsCount; i++)
                        {
                            DataSnapshot friendSnapshot = snapshot.Children.ElementAt(i);
                            string _friendId = friendSnapshot.Key;
                            DatabaseReference _friendRef = Database.RootReference.Child(AppSettings.RootUserKey).Child(_friendId);
                            Task<DataSnapshot> _task = _friendRef.GetValueAsync();
                            TaskList.Add(_task);

                        }
                        Task.WhenAll(TaskList).ContinueWith(task2 =>
                        {
                            if (task2.IsCompleted)
                            {
                                foreach (Task<DataSnapshot> t in TaskList)
                                {
                                    if (t.IsCompleted && t.Result.Exists)
                                    {
                                        string _friendId = t.Result.Key;
                                        string jsonFriend = t.Result.GetRawJsonValue();
                                        if (!string.IsNullOrEmpty(jsonFriend))
                                        {
                                            User _dataFriend = JsonUtility.FromJson<User>(jsonFriend);

                                            if (_dataFriend != null)
                                            {
                                                _dataFriend.UserID = _friendId;
                                                if (_friendQuery.forward)
                                                {
                                                    if (_friendId != _friendQuery.indexKey)
                                                    {
                                                        friends.Add(_dataFriend);
                                                    }
                                                }
                                                else
                                                {
                                                    friends.Add(_dataFriend);
                                                }
                                            }
                                        }
                                        CleanTask(t);
                                    }
                                }
                            }
                            _callback.IsSuccess = true;
                            friends.Reverse();
                            if (!_friendQuery.forward)
                            {
                                friends.Reverse();
                            }
                            _callback.RequestID = _friendQuery.RequestID;
                            _callback.users = friends;
                            _callback.forward = _friendQuery.forward;
                            UnityMainThreadDispatcher.Instance().Enqueue(() => _friendQuery.callback.Invoke(_callback));
                            CleanTask(task2);
                        });
                        CleanTask(task);
                    }
                });
        }


        public void GetGroupUsersAt(UsersQuery _friendQuery)
        {
            Query databaseQuery;
            string rootKey = AppSettings.UserMessagesGroups;
            if (string.IsNullOrEmpty(_friendQuery.indexKey))
            {
                databaseQuery = Database.RootReference.Child(rootKey).Child(_friendQuery.ownerID).Child(AppSettings.RootUserKey).OrderByKey().LimitToLast(_friendQuery.endIndex);
            }
            else if (_friendQuery.forward)
            {
                int count = _friendQuery.endIndex - _friendQuery.startIndex + 1;
                databaseQuery = Database.RootReference.Child(rootKey).Child(_friendQuery.ownerID).Child(AppSettings.RootUserKey).OrderByKey().EndAt(_friendQuery.indexKey).LimitToLast(count);
            }
            else
            {
                int count = _friendQuery.endIndex - _friendQuery.startIndex + 1;
                databaseQuery = Database.RootReference.Child(rootKey).Child(_friendQuery.ownerID).Child(AppSettings.RootUserKey).OrderByKey().EndAt(_friendQuery.indexKey).LimitToLast(count);
            }

            UsersCallback _callback = new UsersCallback();
            databaseQuery
                .GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        _callback.IsSuccess = false;
                        // Handle the error...
                        UnityMainThreadDispatcher.Instance().Enqueue(() => AppManager.VIEW_CONTROLLER.HideLoading());
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _friendQuery.callback.Invoke(_callback));
                        CleanTask(task);
                    }
                    else if (task.IsCompleted && task.Result.Exists)
                    {
                        DataSnapshot snapshot = task.Result;
                        List<User> friends = new List<User>();

                        int _friendsCount = (int)snapshot.ChildrenCount;

                        List<Task> TaskList = new List<Task>();

                        for (int i = 0; i < _friendsCount; i++)
                        {
                            DataSnapshot friendSnapshot = snapshot.Children.ElementAt(i);
                            string _friendId = friendSnapshot.Value.ToString();
                            DatabaseReference _friendRef = Database.RootReference.Child(AppSettings.RootUserKey).Child(_friendId);
                            Task<DataSnapshot> _task = _friendRef.GetValueAsync();
                            TaskList.Add(_task);

                        }
                        Task.WhenAll(TaskList).ContinueWith(task2 =>
                        {
                            if (task2.IsCompleted)
                            {
                                foreach (Task<DataSnapshot> t in TaskList)
                                {
                                    if (t.IsCompleted && t.Result.Exists)
                                    {
                                        string _friendId = t.Result.Key;
                                        string jsonFriend = t.Result.GetRawJsonValue();
                                        if (!string.IsNullOrEmpty(jsonFriend))
                                        {
                                            User _dataFriend = JsonUtility.FromJson<User>(jsonFriend);

                                            if (_dataFriend != null)
                                            {
                                                _dataFriend.UserID = _friendId;
                                                if (_friendQuery.forward)
                                                {
                                                    if (_friendId != _friendQuery.indexKey)
                                                    {
                                                        friends.Add(_dataFriend);
                                                    }
                                                }
                                                else
                                                {
                                                    friends.Add(_dataFriend);
                                                }
                                            }
                                        }
                                        CleanTask(t);
                                    }
                                }
                            }
                            _callback.IsSuccess = true;
                            friends.Reverse();
                            if (!_friendQuery.forward)
                            {
                                friends.Reverse();
                            }
                            _callback.RequestID = _friendQuery.RequestID;
                            _callback.users = friends;
                            _callback.forward = _friendQuery.forward;
                            UnityMainThreadDispatcher.Instance().Enqueue(() => _friendQuery.callback.Invoke(_callback));
                            CleanTask(task2);
                        });
                        CleanTask(task);
                    }
                });
        }

        public void GetWorldFeedsAt(FeedQuery _feedQuery)
        {
            Query databaseQuery;
            if (string.IsNullOrEmpty(_feedQuery.indexKey))
            {
                databaseQuery = Database.RootReference.Child(AppSettings.AllPostsKey).OrderByKey().LimitToLast(_feedQuery.endIndex);
            }
            else if (_feedQuery.forward)
            {
                int count = _feedQuery.endIndex - _feedQuery.startIndex + 1;
                databaseQuery = Database.RootReference.Child(AppSettings.AllPostsKey).OrderByKey().EndAt(_feedQuery.indexKey).LimitToLast(count);
            }
            else
            {
                int count = _feedQuery.endIndex - _feedQuery.startIndex + 1;
                databaseQuery = Database.RootReference.Child(AppSettings.AllPostsKey).OrderByKey().EndAt(_feedQuery.indexKey).LimitToLast(count);
            }
            FeedCallback _callback = new FeedCallback();
            databaseQuery
                .GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        _callback.IsSuccess = false;
                    // Handle the error...
                    UnityMainThreadDispatcher.Instance().Enqueue(() => AppManager.VIEW_CONTROLLER.HideLoading());
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _feedQuery.callback.Invoke(_callback));
                    }
                    else if (task.IsCompleted && task.Result.Exists)
                    {

                        _callback.IsSuccess = true;
                        DataSnapshot snapshot = task.Result;
                        List<Feed> feeds = new List<Feed>();

                        int _feedCount = (int)snapshot.ChildrenCount;

                        for (int i = 0; i < _feedCount; i++)
                        {
                            DataSnapshot feedSnapshot = snapshot.Children.ElementAt(i);
                            string _feedId = feedSnapshot.Key;

                            string jsonFeed = feedSnapshot.GetRawJsonValue();
                            if (!string.IsNullOrEmpty(jsonFeed))
                            {
                                Feed _dataFeed = JsonUtility.FromJson<Feed>(jsonFeed);

                                if (_dataFeed != null)
                                {
                                    _dataFeed.Key = _feedId;
                                    if (_feedQuery.forward)
                                    {
                                        if (_feedId != _feedQuery.indexKey)
                                        {
                                            feeds.Add(_dataFeed);
                                        }
                                    }
                                    else
                                    {
                                        feeds.Add(_dataFeed);
                                    }
                                }
                            }
                        }
                        _callback.IsSuccess = true;
                        feeds.Reverse();
                        if (!_feedQuery.forward)
                        {
                            feeds.Reverse();
                        }

                        _callback.feeds = feeds;
                        _callback.forward = _feedQuery.forward;
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _feedQuery.callback.Invoke(_callback));
                    }
                    CleanTask(task);
                });
        }

        public void GetMessageListAt(MessageListQuery _listQuery)
        {
            Debug.Log("GetMessageListAt");
            Query databaseQuery;
            if (string.IsNullOrEmpty(_listQuery.indexKey))
            {
                databaseQuery = Database.RootReference.Child(AppSettings.UserMessagesList).Child(_listQuery.ownerID).OrderByKey().LimitToLast(_listQuery.endIndex);
            }
            else if (_listQuery.forward)
            {
                int count = _listQuery.endIndex - _listQuery.startIndex + 1;
                databaseQuery = Database.RootReference.Child(AppSettings.UserMessagesList).Child(_listQuery.ownerID).OrderByKey().EndAt(_listQuery.indexKey).LimitToLast(count);
            }
            else
            {
                int count = _listQuery.endIndex - _listQuery.startIndex + 1;
                databaseQuery = Database.RootReference.Child(AppSettings.UserMessagesList).Child(_listQuery.ownerID).OrderByKey().EndAt(_listQuery.indexKey).LimitToLast(count);
            }

            MessageListCallback _callback = new MessageListCallback();
            databaseQuery
                .GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    
                    if (task.IsCompleted && task.Result.Exists)
                    {
                        _callback.IsSuccess = true;
                        DataSnapshot snapshot = task.Result;
                        List<string> messages = new List<string>();

                        int _feedCount = (int)snapshot.ChildrenCount;

                        Debug.Log("Get form DB "+_feedCount);

                        for (int i = 0; i < _feedCount; i++)
                        {
                            DataSnapshot userSnapshot = snapshot.Children.ElementAt(i);
                            string _childId = userSnapshot.Key;
                            string _userId = userSnapshot.Value.ToString();

                            string jsonFeed = userSnapshot.GetRawJsonValue();
                            if (!string.IsNullOrEmpty(jsonFeed))
                            {
                                if (_listQuery.forward)
                                {
                                    if (_childId != _listQuery.indexKey)
                                    {
                                        messages.Add(_userId);
                                    }
                                }
                                else
                                {
                                    messages.Add(_userId);
                                }

                            }
                        }
                        _callback.IsSuccess = true;
                        messages.Reverse();
                        if (!_listQuery.forward)
                        {
                            messages.Reverse();
                        }

                        _callback.usersIds = messages;
                        _callback.forward = _listQuery.forward;
                        _listQuery.callback.Invoke(_callback);
                        CleanTask(task);
                    }
                    else
                    {
                        Debug.Log("fail to get form DB ");
                        _callback.IsSuccess = false;
                        // Handle the error...
                        AppManager.VIEW_CONTROLLER.HideLoading();
                        _listQuery.callback.Invoke(_callback);
                        CleanTask(task);
                    }
                });
        }

        public void UploadFile(FileUploadRequset _request, Action<FileUploadCallback> _callback)
        {
            FileUploadCallback _uploadCallback = new FileUploadCallback();

            string pathKey = string.Empty;
            if (_request.FeedType == FeedType.Video)
            {
                pathKey = AppSettings.FeedUploadVideoPath;
            }
            if (_request.FeedType == FeedType.Image)
            {
                pathKey = AppSettings.FeedUploadImagePath;
            }
            Firebase.Storage.StorageReference upload_ref = Storage.RootReference.Child(pathKey + _request.FileName);

            upload_ref.PutBytesAsync(_request.UploadBytes)
                .ContinueWith((Task<StorageMetadata> task) =>
                {
                    if (task.IsFaulted || task.IsCanceled)
                    {
                        _uploadCallback.IsComplete = true;
                        _uploadCallback.IsSuccess = false;
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_uploadCallback));
                        CleanTask(task);
                    }
                    else
                    {
                        Firebase.Storage.StorageMetadata metadata = task.Result;
                        metadata.Reference.GetDownloadUrlAsync().ContinueWith((Task<Uri> task2) =>
                        {
                            if (!task2.IsFaulted && !task2.IsCanceled)
                            {
                                _uploadCallback.IsComplete = true;
                                _uploadCallback.IsSuccess = true;
                                _uploadCallback.DownloadUrl = task2.Result.ToString();
                                UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_uploadCallback));
                                CleanTask(task2);
                            }
                            else
                            {
                                _uploadCallback.IsComplete = true;
                                _uploadCallback.IsSuccess = false;
                                UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_uploadCallback));
                                CleanTask(task2);
                            }
                        });
                        CleanTask(task);
                    }
                });
        }

        public void AddNewPost(Feed _feed, Action<FeedUploadCallback> _callback)
        {
            FeedUploadCallback _uploadCallback = new FeedUploadCallback();
            DatabaseReference _feedref = Database.RootReference.Child(AppSettings.AllPostsKey).Push();

            string _feedKey = _feedref.Key;
            _feed.Key = _feedKey;
            string json = JsonUtility.ToJson(_feed);
            _feedref.SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    _uploadCallback.IsSuccess = false;
                    _uploadCallback.IsComplete = true;
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_uploadCallback));
                    CleanTask(task);
                }
                else
                {
                    DatabaseReference _feedLinkRef = Database.RootReference.Child(AppSettings.UserPostsKey).Child(_feed.ToUserID).Child(_feedKey);
                    _feedLinkRef.SetValueAsync(0).ContinueWith(task2 =>
                    {
                        if (task2.IsFaulted || task2.IsCanceled)
                        {
                            _uploadCallback.IsSuccess = false;
                            _uploadCallback.IsComplete = true;
                            UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_uploadCallback));
                            CleanTask(task2);
                        }
                        else
                        {
                            Debug.Log("Upload to my feed complete");
                            _uploadCallback.IsSuccess = true;
                            _uploadCallback.IsComplete = true;

                            UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_uploadCallback));
                            CleanTask(task2);
                        }
                    });
                    CleanTask(task);
                }
            });
        }

        


        public void SearchUsers(UsersQuery _userQuery, string _search)
        {
            Query databaseQueryFirstName = Database.RootReference.Child(AppSettings.RootUserKey).OrderByChild(AppSettings.UserFirstNameKey).StartAt(_search).EndAt(_search + "\uf8ff");
            Query databaseQueryLastName = Database.RootReference.Child(AppSettings.RootUserKey).OrderByChild(AppSettings.UserLastNameKey).StartAt(_search).EndAt(_search + "\uf8ff");
            Query databaseQueryFullName = Database.RootReference.Child(AppSettings.RootUserKey).OrderByChild(AppSettings.UserFullNameKey).StartAt(_search).EndAt(_search + "\uf8ff");
            Query databaseQueryPhone = Database.RootReference.Child(AppSettings.RootUserKey).OrderByChild(AppSettings.UserPhoneKey).StartAt(_search).EndAt(_search + "\uf8ff");


            UsersCallback _callback = new UsersCallback();

            List<Task> TaskList = new List<Task>();

            TaskList.Add(databaseQueryFirstName.GetValueAsync());
            TaskList.Add(databaseQueryLastName.GetValueAsync());
            TaskList.Add(databaseQueryFullName.GetValueAsync());
            TaskList.Add(databaseQueryPhone.GetValueAsync());

            Task.WhenAll(TaskList).ContinueWith(task2 =>
            {
                if (task2.IsCompleted)
                {
                    _callback.IsSuccess = true;
                    List<User> users = new List<User>();
                    List<string> usersKeys = new List<string>();
                    foreach (Task<DataSnapshot> t in TaskList)
                    {
                        if (t.IsCompleted && t.Result.Exists)
                        {
                            DataSnapshot snapshot = t.Result;

                            for (int i = 0; i < snapshot.ChildrenCount; i++)
                            {
                                DataSnapshot userSnapshot = snapshot.Children.ElementAt(i);
                                string jsonMessage = userSnapshot.GetRawJsonValue();
                                if (!string.IsNullOrEmpty(jsonMessage))
                                {
                                    User _dataUser = JsonUtility.FromJson<User>(jsonMessage);
                                    //_dataUser.Key = userSnapshot.Key;
                                    if (_dataUser != null)
                                    {
                                        if (userSnapshot.Key != _userQuery.indexKey && !AppManager.USER_PROFILE.IsMine(_dataUser.UserID))
                                        {
                                            if (!usersKeys.Contains(userSnapshot.Key))
                                            {
                                                users.Add(_dataUser);
                                                usersKeys.Add(userSnapshot.Key);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (!_userQuery.forward)
                    {
                        users.Reverse();
                    }
                    _callback.RequestID = _userQuery.RequestID;
                    _callback.users = users;
                    _callback.forward = _userQuery.forward;
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _userQuery.callback.Invoke(_callback));
                    CleanTask(task2);
                }

            });


        }

        public void GetUserFullName(string _userID, Action<string> _callback)
        {
            Query databaseQuery = Database.RootReference.Child(AppSettings.RootUserKey).Child(_userID).Child(AppSettings.UserFullNameKey);
            databaseQuery.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result.Exists)
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(task.Result.Value.ToString()));
                    }
                    CleanTask(task);
                }
                else
                {
                    CleanTask(task);
                }
            });
        }

        public void GetUserFriendsCount(string _userID, Action<string> _callback)
        {
            Query databaseQuery = Database.RootReference.Child(AppSettings.UserFriendsKey).Child(_userID).Child(AppSettings.ListCountKey);
            databaseQuery.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result != null && task.Result.Value != null)
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(task.Result.Value.ToString()));
                    }
                    CleanTask(task);
                }
                else
                {
                    CleanTask(task);
                }
            });
        }

        public void GetPostLikesCount(string _postID, Action<string> _callback)
        {
            Query databaseQuery = Database.RootReference.Child(AppSettings.PostLikesKey).Child(_postID).Child(AppSettings.ListCountKey);
            databaseQuery.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result != null && task.Result.Value != null)
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(task.Result.Value.ToString()));
                    }
                    CleanTask(task);
                }
                else
                {
                    CleanTask(task);
                }
            });
        }

        public void LikPost(string _postID, Action<bool> _callback)
        {
            Task fTask = Database.RootReference.Child(AppSettings.PostLikesKey).Child(_postID).Child(AppSettings.ContainerListKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).SetValueAsync(0);

            fTask.ContinueWith(task2 =>
            {
                if (task2.IsCompleted)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(true));
                    CleanTask(task2);
                }
                else
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(false));
                    CleanTask(task2);
                }
            });
        }

        public void UnLikPost(string _postID, Action<bool> _callback)
        {
            Task fTask = Database.RootReference.Child(AppSettings.PostLikesKey).Child(_postID).Child(AppSettings.ContainerListKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).RemoveValueAsync();

            fTask.ContinueWith(task2 =>
            {
                if (task2.IsCompleted)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(true));
                    CleanTask(task2);
                }
                else
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(false));
                    CleanTask(task2);
                }
            });
        }

        public void IsLikedPost(string _postID, Action<bool> _callback)
        {
            Task<DataSnapshot> fTask = Database.RootReference.Child(AppSettings.PostLikesKey).Child(_postID).Child(AppSettings.ContainerListKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).GetValueAsync();

            fTask.ContinueWith(task2 =>
            {
                if (task2.IsCompleted)
                {
                    if (fTask.Result == null || fTask.Result.Value == null)
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(false));
                    }
                    else
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(true));
                    }
                    CleanTask(task2);
                }
                else
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(false));
                    CleanTask(task2);
                }
            });
        }

        public void GetPostCommentsCount(string _postID, Action<string> _callback)
        {
            Query databaseQuery = Database.RootReference.Child(AppSettings.PostCommentsKey).Child(_postID).Child(AppSettings.ListCountKey);
            databaseQuery.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result != null && task.Result.Value != null)
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(task.Result.Value.ToString()));
                    }
                    CleanTask(task);
                }
                else
                {
                    CleanTask(task);
                }
            });
        }

        public void GetLastMessageWithUser(string _userId, Action<string> _callback)
        {
            GetMessageReferece(_userId).LimitToLast(1).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Message _dataMsg = JsonUtility.FromJson<Message>(task.Result.Children.ElementAt<DataSnapshot>(0).GetRawJsonValue());
                    string _body = string.Empty;
                    if (_dataMsg.Type == ContentMessageType.TEXT) _body = _dataMsg.BodyTXT;
                    if (_dataMsg.Type == ContentMessageType.IMAGE) _body = _dataMsg.Type.ToString();
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_body));
                    CleanTask(task);
                }
                else
                {
                    CleanTask(task);
                }
            });
        }

        public void GetLastMessageAtGroup(string _chatId, Action<string> _callback)
        {
            GetGroupChatReferece(_chatId).LimitToLast(1).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Message _dataMsg = JsonUtility.FromJson<Message>(task.Result.Children.ElementAt<DataSnapshot>(0).GetRawJsonValue());
                    string _body = string.Empty;
                    if (_dataMsg.Type == ContentMessageType.TEXT) _body = _dataMsg.BodyTXT;
                    if (_dataMsg.Type == ContentMessageType.IMAGE) _body = _dataMsg.Type.ToString();
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_body));
                    CleanTask(task);
                }
                else
                {
                    CleanTask(task);
                }
            });
        }


        public void GetGroupMessagesAt(MessagesQuery _messageQuery)
        {
            string messageKey = _messageQuery.UserId;
            Query databaseQuery;
            if (string.IsNullOrEmpty(_messageQuery.indexKey))
            {
                databaseQuery = Database.RootReference.Child(AppSettings.UserMessagesKey).Child(messageKey).Child(AppSettings.ContainerListKey).OrderByKey().LimitToLast(_messageQuery.endIndex);
            }
            else if (_messageQuery.forward)
            {
                int count = _messageQuery.endIndex - _messageQuery.startIndex + 1;
                databaseQuery = Database.RootReference.Child(AppSettings.UserMessagesKey).Child(messageKey).Child(AppSettings.ContainerListKey).OrderByKey().StartAt(_messageQuery.indexKey).LimitToFirst(count);
            }
            else
            {
                int count = _messageQuery.endIndex - _messageQuery.startIndex + 1;
                databaseQuery = Database.RootReference.Child(AppSettings.UserMessagesKey).Child(messageKey).Child(AppSettings.ContainerListKey).OrderByKey().EndAt(_messageQuery.indexKey).LimitToLast(count);
            }
            MessagesCallback _callback = new MessagesCallback();
            databaseQuery
                .GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted || task.IsCanceled)
                    {
                        _callback.IsSuccess = false;
                        UnityMainThreadDispatcher.Instance().Enqueue(() => AppManager.VIEW_CONTROLLER.HideLoading());
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _messageQuery.callback.Invoke(_callback));
                        CleanTask(task);
                    }
                    else if (task.IsCompleted && task.Result != null && task.Result.ChildrenCount > 0)
                    {
                        _callback.IsSuccess = true;
                        DataSnapshot snapshot = task.Result;
                        List<Message> feeds = new List<Message>();
                        for (int i = 0; i < snapshot.ChildrenCount; i++)
                        {
                            DataSnapshot feedSnapshot = snapshot.Children.ElementAt(i);

                            string jsonMessage = feedSnapshot.GetRawJsonValue();
                            Message _dataFeed = JsonUtility.FromJson<Message>(jsonMessage);
                            _dataFeed.Key = feedSnapshot.Key;
                            if (_dataFeed != null)
                            {
                                if (feedSnapshot.Key != _messageQuery.indexKey)
                                {
                                    feeds.Add(_dataFeed);
                                }
                            }

                        }
                        if (!_messageQuery.forward)
                        {
                            feeds.Reverse();
                        }
                        _callback.messages = feeds;
                        _callback.forward = _messageQuery.forward;
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _messageQuery.callback.Invoke(_callback));
                        CleanTask(task);
                    }
                    else
                    {
                        _callback.IsSuccess = false;
                        UnityMainThreadDispatcher.Instance().Enqueue(() => AppManager.VIEW_CONTROLLER.HideLoading());
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _messageQuery.callback.Invoke(_callback));
                        CleanTask(task);
                    }
                });
        }

        public void GetMessagesAt(MessagesQuery _messageQuery)
        {
            string messageKey = GetUserMessageKey(AppManager.USER_PROFILE.FIREBASE_USER.UserId, _messageQuery.UserId);
            Query databaseQuery;
            if (string.IsNullOrEmpty(_messageQuery.indexKey))
            {
                databaseQuery = Database.RootReference.Child(AppSettings.UserMessagesKey).Child(messageKey).Child(AppSettings.ContainerListKey).OrderByKey().LimitToLast(_messageQuery.endIndex);
            }
            else if (_messageQuery.forward)
            {
                int count = _messageQuery.endIndex - _messageQuery.startIndex + 1;
                databaseQuery = Database.RootReference.Child(AppSettings.UserMessagesKey).Child(messageKey).Child(AppSettings.ContainerListKey).OrderByKey().StartAt(_messageQuery.indexKey).LimitToFirst(count);
            }
            else
            {
                int count = _messageQuery.endIndex - _messageQuery.startIndex + 1;
                databaseQuery = Database.RootReference.Child(AppSettings.UserMessagesKey).Child(messageKey).Child(AppSettings.ContainerListKey).OrderByKey().EndAt(_messageQuery.indexKey).LimitToLast(count);
            }
            MessagesCallback _callback = new MessagesCallback();
            databaseQuery
                .GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted || task.IsCanceled)
                    {
                        _callback.IsSuccess = false;
                        UnityMainThreadDispatcher.Instance().Enqueue(() => AppManager.VIEW_CONTROLLER.HideLoading());
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _messageQuery.callback.Invoke(_callback));
                        CleanTask(task);
                    }
                    else if (task.IsCompleted && task.Result != null && task.Result.ChildrenCount > 0)
                    {
                        _callback.IsSuccess = true;
                        DataSnapshot snapshot = task.Result;
                        List<Message> feeds = new List<Message>();
                        for (int i = 0; i < snapshot.ChildrenCount; i++)
                        {
                            DataSnapshot feedSnapshot = snapshot.Children.ElementAt(i);

                            string jsonMessage = feedSnapshot.GetRawJsonValue();
                            Message _dataFeed = JsonUtility.FromJson<Message>(jsonMessage);
                            _dataFeed.Key = feedSnapshot.Key;
                            if (_dataFeed != null)
                            {
                                if (feedSnapshot.Key != _messageQuery.indexKey)
                                {
                                    feeds.Add(_dataFeed);
                                }
                            }

                        }
                        if (!_messageQuery.forward)
                        {
                            feeds.Reverse();
                        }
                        _callback.messages = feeds;
                        _callback.forward = _messageQuery.forward;
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _messageQuery.callback.Invoke(_callback));
                        CleanTask(task);
                    }
                    else
                    {
                        _callback.IsSuccess = false;
                        UnityMainThreadDispatcher.Instance().Enqueue(() => AppManager.VIEW_CONTROLLER.HideLoading());
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _messageQuery.callback.Invoke(_callback));
                        CleanTask(task);
                    }
                });
        }

        public void GetCommentsAt(MessagesQuery _messageQuery)
        {
            string messageKey = _messageQuery.UserId;
            Query databaseQuery;
            if (string.IsNullOrEmpty(_messageQuery.indexKey))
            {
                databaseQuery = Database.RootReference.Child(AppSettings.PostCommentsKey).Child(messageKey).Child(AppSettings.ContainerListKey).OrderByKey().LimitToLast(_messageQuery.endIndex);
            }
            else if (_messageQuery.forward)
            {
                int count = _messageQuery.endIndex - _messageQuery.startIndex + 1;
                databaseQuery = Database.RootReference.Child(AppSettings.PostCommentsKey).Child(messageKey).Child(AppSettings.ContainerListKey).OrderByKey().StartAt(_messageQuery.indexKey).LimitToFirst(count);
            }
            else
            {
                int count = _messageQuery.endIndex - _messageQuery.startIndex + 1;
                databaseQuery = Database.RootReference.Child(AppSettings.PostCommentsKey).Child(messageKey).Child(AppSettings.ContainerListKey).OrderByKey().EndAt(_messageQuery.indexKey).LimitToLast(count);
            }
            MessagesCallback _callback = new MessagesCallback();
            databaseQuery
                .GetValueAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted || task.IsCanceled)
                    {
                        _callback.IsSuccess = false;
                        UnityMainThreadDispatcher.Instance().Enqueue(() => AppManager.VIEW_CONTROLLER.HideLoading());
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _messageQuery.callback.Invoke(_callback));
                        CleanTask(task);
                    }
                    else if (task.IsCompleted && task.Result != null && task.Result.ChildrenCount > 0)
                    {
                        _callback.IsSuccess = true;
                        DataSnapshot snapshot = task.Result;
                        List<Message> feeds = new List<Message>();
                        for (int i = 0; i < snapshot.ChildrenCount; i++)
                        {
                            DataSnapshot feedSnapshot = snapshot.Children.ElementAt(i);

                            string jsonMessage = feedSnapshot.GetRawJsonValue();
                            Message _dataFeed = JsonUtility.FromJson<Message>(jsonMessage);
                            _dataFeed.Key = feedSnapshot.Key;
                            if (_dataFeed != null)
                            {
                                if (feedSnapshot.Key != _messageQuery.indexKey)
                                {
                                    feeds.Add(_dataFeed);
                                }
                            }

                        }
                        if (!_messageQuery.forward)
                        {
                            feeds.Reverse();
                        }
                        _callback.messages = feeds;
                        _callback.forward = _messageQuery.forward;
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _messageQuery.callback.Invoke(_callback));
                        CleanTask(task);
                    }
                    else
                    {
                        _callback.IsSuccess = false;
                        UnityMainThreadDispatcher.Instance().Enqueue(() => AppManager.VIEW_CONTROLLER.HideLoading());
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _messageQuery.callback.Invoke(_callback));
                        CleanTask(task);
                    }
                });
        }

        public void GetFeedVideoFileUrl(string _fileName, Action<string> _callback)
        {
            StorageReference video_ref = Storage.RootReference.Child(AppSettings.FeedUploadVideoPath + _fileName + AppSettings.ConvertedVideoOutPath);
            video_ref.GetDownloadUrlAsync().ContinueWith((Task<Uri> task2) =>
            {
                if (!task2.IsFaulted && !task2.IsCanceled)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(task2.Result.ToString()));
                    CleanTask(task2);
                }
                else
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(string.Empty));
                    CleanTask(task2);
                }
            });
        }

        public void UploadAndCompressVideo(string _path, string _databasePath)
        {
            // Create the arguments to the callable function.
            var data = new Dictionary<string, object>();
            data["uploadPath"] = _path;
            data["bucketUrl"] = Storage.App.Options.StorageBucket;
            data["databasePath"] = _databasePath;

            // Call the function and extract the operation from the result.
            HttpsCallableReference _function = Functions.GetHttpsCallable("UploadAndCompressVideo");
            _function.CallAsync(data).ContinueWith((task) =>
            {
                if (!task.IsFaulted && !task.IsCanceled)
                {
                    CleanTask(task);
                }
                else
                {
                    CleanTask(task);
                }

            });
        }

        public void SharePostWithFriends(string _userId, string _postId)
        {
            Debug.Log("SharePostWithFriends "+_userId+" "+_postId);
            // Create the arguments to the callable function.
            var data = new Dictionary<string, object>();
            data["_userId"] = _userId;
            data["_postId"] = _postId;

            // Call the function and extract the operation from the result.
            HttpsCallableReference _function = Functions.GetHttpsCallable("SharePostWithFriends");
            _function.CallAsync(data).ContinueWith((task) =>
            {
                if (!task.IsFaulted && !task.IsCanceled && task.IsCompleted)
                {
                    Debug.Log("Success SharePostWithFriends");
                    CleanTask(task);
                }
                else
                {
                    Debug.Log("Failed SharePostWithFriends "+ task.Exception.StackTrace);
                    CleanTask(task);
                }
            });
        }

        public DatabaseReference GetMessageReferece(string _userId)
        {
            string messageKey = GetUserMessageKey(AppManager.USER_PROFILE.FIREBASE_USER.UserId, _userId);
            return Database.RootReference.Child(AppSettings.UserMessagesKey).Child(messageKey).Child(AppSettings.ContainerListKey);
        }

        public DatabaseReference GetGroupChatReferece(string _chatId)
        {
            string messageKey = _chatId;
            return Database.RootReference.Child(AppSettings.UserMessagesKey).Child(messageKey).Child(AppSettings.ContainerListKey);
        }

        public DatabaseReference GetTypingMessageReferece(string _userId)
        {
            string messageKey = GetUserMessageKey(AppManager.USER_PROFILE.FIREBASE_USER.UserId, _userId);
            return Database.RootReference.Child(AppSettings.UserMessagesKey).Child(messageKey).Child(AppSettings.TypingMSGKey).Child(_userId);
        }

        public void UpdateTypingMessage(string _userId, string _s)
        {
            string messageKey = GetUserMessageKey(AppManager.USER_PROFILE.FIREBASE_USER.UserId, _userId);
            Database.RootReference.Child(AppSettings.UserMessagesKey).Child(messageKey).Child(AppSettings.TypingMSGKey).Child((AppManager.USER_PROFILE.FIREBASE_USER.UserId)).SetValueAsync(_s);
        }

        public DatabaseReference GetPostCommentsReferece(string _postId)
        {
            return Database.RootReference.Child(AppSettings.PostCommentsKey).Child(_postId).Child(AppSettings.ContainerListKey);
        }

        public DatabaseReference GetMessageListReferece()
        {
            return Database.RootReference.Child(AppSettings.UserMessagesList).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId);
        }

        public DatabaseReference GetUnreadMessageWithUserReferense(string _userID)
        {
            return Database.RootReference.Child(AppSettings.UnreadMessagesKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).Child(AppSettings.ContainerListKey).Child(_userID);
        }

        public void CheckAndAddNewChatInfo(MessageGroupInfo _info, Action<ChatInfoMessage> _callback = null)
        {
            Task<DataSnapshot> fTask = Database.RootReference.Child(AppSettings.UserMessagesGroups).Child(_info.ChatID).GetValueAsync();
            ChatInfoMessage _callbackInfo = new ChatInfoMessage();

            fTask.ContinueWith(task2 =>
            {
                if (task2.IsCompleted)
                {
                    if (fTask.Result == null || fTask.Result.Value == null)
                    {
                        AddOrUpdateChatInfo(_info, _callback);
                    }
                    else
                    {
                        _callbackInfo.IsSuccess = false;
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _callback?.Invoke(_callbackInfo));
                    }
                    CleanTask(task2);
                }
                else
                {
                    _callbackInfo.IsSuccess = false;
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback?.Invoke(_callbackInfo));
                    CleanTask(task2);
                }
            });
        }

        public void AddOrUpdateChatInfo(MessageGroupInfo _info, Action<ChatInfoMessage> _callback = null)
        {
            string json = JsonUtility.ToJson(_info);
            ChatInfoMessage _callbackInfo = new ChatInfoMessage();

            Database.RootReference.Child(AppSettings.UserMessagesGroups).Child(_info.ChatID).SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    _callbackInfo.IsSuccess = false;
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback?.Invoke(_callbackInfo));
                    CleanTask(task);
                    // Uh-oh, an error occurred!
                }
                else
                {
                    _callbackInfo.IsSuccess = true;
                    _callbackInfo.Info = _info;
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback?.Invoke(_callbackInfo));
                    CleanTask(task);
                }
            });
        }

        public void GetGroupChatInfo(string _chatId, Action<ChatInfoMessage> _callback)
        {
            ChatInfoMessage _callbackInfo = new ChatInfoMessage();

            DatabaseReference _tokenRef = Database.RootReference.Child(AppSettings.UserMessagesGroups).Child(_chatId);
            _tokenRef.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    _callbackInfo.IsSuccess = false;
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_callbackInfo));
                }
                else if (task.IsCompleted && task.Result.Exists)
                {
                    _callbackInfo.IsSuccess = true;
                  
                    DataSnapshot snapshot = task.Result;
                    string _json = snapshot.GetRawJsonValue();
                    MessageGroupInfo _info = JsonUtility.FromJson<MessageGroupInfo>(_json);
                    _callbackInfo.Info = _info;

                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_callbackInfo));
                }
                else
                {
                    _callbackInfo.IsSuccess = false;
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(_callbackInfo));
                }
            });
        }

        public void UploadMessage(Message _msg, MessageGroupInfo _info)
        {
            string messageKey = _info.ChatID;
            string json = JsonUtility.ToJson(_msg);
            Database.RootReference.Child(AppSettings.UserMessagesKey).Child(messageKey).Child(AppSettings.ContainerListKey).Push().SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    CleanTask(task);
                    // Uh-oh, an error occurred!
                }
                else
                {
                    UpdateMessagesList(_info);
                    CleanTask(task);
                }
            });
        }

        public void UploadGroupMessage(Message _msg, MessageGroupInfo _info)
        {
            string messageKey = _info.ChatID;
            string json = JsonUtility.ToJson(_msg);
            Database.RootReference.Child(AppSettings.UserMessagesKey).Child(messageKey).Child(AppSettings.ContainerListKey).Push().SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    CleanTask(task);
                    // Uh-oh, an error occurred!
                }
                else
                {
                    // to do : update group list
                    UpdateMessagesList(_info);
                    CleanTask(task);
                }
            });
        }

        public void UploadComments(Message _msg, string _postId)
        {
            string messageKey = _postId;
            string json = JsonUtility.ToJson(_msg);
            Database.RootReference.Child(AppSettings.PostCommentsKey).Child(messageKey).Child(AppSettings.ContainerListKey).Push().SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    CleanTask(task);
                    // Uh-oh, an error occurred!
                }
                else
                {
                    CleanTask(task);
                }
            });
        }

        public void RemoveFromMessageList(MessageGroupInfo _info)
        {
            Database.RootReference.Child(AppSettings.UserMessagesList).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).OrderByValue().EqualTo(_info.ChatID).GetValueAsync().ContinueWithOnMainThread(task1 =>
            {
                if (task1.IsFaulted || task1.IsCanceled)
                {
                    CleanTask(task1);
                }
                else
                {
                    List<Task> TaskList = new List<Task>();
                    if (task1.Result != null)
                    {
                        foreach (DataSnapshot _data in task1.Result.Children)
                        {
                            Debug.Log(_data.Key);
                            Task _task = Database.RootReference.Child(AppSettings.UserMessagesList).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).Child(_data.Key).RemoveValueAsync();
                            TaskList.Add(_task);
                        }
                    }
                    if (TaskList.Count > 0)
                    {
                        Task.WhenAll(TaskList).ContinueWithOnMainThread(task2 =>
                        {
                            if (task2.IsCompleted)
                            {
                                
                            }
                        });
                    }

                    CleanTask(task1);
                }
            });
        }

        public void UpdateMessagesList(MessageGroupInfo _info)
        {
            //string _userId1 = _info.Users[0];
            //string _userId2 = _info.Users[1];

            for (int i = 0; i < _info.Users.Count; i++)
            {
                string _userID = _info.Users[i];

                FirebaseDatabase.DefaultInstance.RootReference.Child(AppSettings.UserMessagesList).Child(_userID).OrderByValue().EqualTo(_info.ChatID).GetValueAsync().ContinueWithOnMainThread(task1 =>
                {
                    if (task1.IsFaulted || task1.IsCanceled)
                    {
                        CleanTask(task1);
                    }
                    else
                    {
                        List<Task> TaskList = new List<Task>();
                        if (task1.Result != null)
                        {
                            foreach (DataSnapshot _data in task1.Result.Children)
                            {
                                Debug.Log("Key to remove = "+_data.Key);
                                Task _task = Database.RootReference.Child(AppSettings.UserMessagesList).Child(_userID).Child(_data.Key).RemoveValueAsync();
                                TaskList.Add(_task);
                            }
                        }
                        if (TaskList.Count > 0)
                        {
                            Task.WhenAll(TaskList).ContinueWithOnMainThread(task2 =>
                            {
                                if (task2.IsCompleted)
                                {
                                    Debug.Log("push new message to list");
                                    Database.RootReference.Child(AppSettings.UserMessagesList).Child(_userID).Push().SetValueAsync(_info.ChatID);
                                }
                            });
                        }
                        else
                        {
#if !UNITY_EDITOR
                            Debug.Log("push new message to list without removing");
                            Database.RootReference.Child(AppSettings.UserMessagesList).Child(_userID).Push().SetValueAsync((_info.ChatID));
#endif
                        }

                        CleanTask(task1);
                    }
                });
            }

            /*Database.RootReference.Child(AppSettings.UserMessagesList).Child(_userId2).OrderByValue().EqualTo(_info.ChatID).GetValueAsync().ContinueWithOnMainThread(task1 =>
            {
                if (task1.IsFaulted || task1.IsCanceled)
                {
                    CleanTask(task1);
                }
                else
                {
                    List<Task> TaskList = new List<Task>();
                    if (task1.Result != null)
                    {
                        foreach (DataSnapshot _data in task1.Result.Children)
                        {
                            Debug.Log(_data.Key);
                            Task _task = Database.RootReference.Child(AppSettings.UserMessagesList).Child(_userId2).Child(_data.Key).RemoveValueAsync();
                            TaskList.Add(_task);
                        }
                    }
                    if (TaskList.Count > 0)
                    {
                        Task.WhenAll(TaskList).ContinueWithOnMainThread(task2 =>
                        {
                            if (task2.IsCompleted)
                            {
                                Database.RootReference.Child(AppSettings.UserMessagesList).Child(_userId2).Push().SetValueAsync(_info.ChatID);
                            }
                        });
                    }
                    else
                    {
                        Database.RootReference.Child(AppSettings.UserMessagesList).Child(_userId2).Push().SetValueAsync((_info.ChatID));
                    }

                    CleanTask(task1);
                }
            });
            Database.RootReference.Child(AppSettings.UserMessagesList).Child(_userId1).OrderByValue().EqualTo(_info.ChatID).GetValueAsync().ContinueWithOnMainThread(task2 =>
            {
                if (task2.IsFaulted || task2.IsCanceled)
                {
                    CleanTask(task2);
                }
                else
                {
                    List<Task> TaskList = new List<Task>();
                    if (task2.Result != null)
                    {
                        foreach (DataSnapshot _data in task2.Result.Children)
                        {
                            Debug.Log(_data.Key);
                            Task _task = Database.RootReference.Child(AppSettings.UserMessagesList).Child(_userId1).Child(_data.Key).RemoveValueAsync();
                            TaskList.Add(_task);
                        }
                    }
                    if (TaskList.Count > 0)
                    {
                        Task.WhenAll(TaskList).ContinueWithOnMainThread(task3 =>
                        {
                            if (task3.IsCompleted)
                            {
                                Database.RootReference.Child(AppSettings.UserMessagesList).Child(_userId1).Push().SetValueAsync((_info.ChatID));
                            }
                        });
                    }
                    else
                    {
                        Database.RootReference.Child(AppSettings.UserMessagesList).Child(_userId1).Push().SetValueAsync((_info.ChatID));
                    }

                    CleanTask(task2);
                }
            });*/
        }

        public void AddToFriends(string _userId, Action _callback)
        {
            DatabaseReference _requestRef = Database.RootReference.Child(AppSettings.UserRequestedFriendsKey).Child(_userId).Child(AppSettings.ContainerListKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId);
            DatabaseReference _pendingRef = Database.RootReference.Child(AppSettings.UserPendingFriendsKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).Child(AppSettings.ContainerListKey).Child(_userId);
            _requestRef.SetValueAsync(0).ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    CleanTask(task);
                }
                else
                {
                    _pendingRef.SetValueAsync(0).ContinueWith(task2 =>
                    {
                        if (task2.IsFaulted || task2.IsCanceled)
                        {
                            CleanTask(task2);
                        }
                        else
                        {
                            if (_callback != null)
                            {
                                UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke());
                            }
                            AppManager.USER_PROFILE.GetUserFullName(_name =>
                            {
                                NotificationMessage _msg = new NotificationMessage();
                                _msg.UserId = _userId;
                                _msg.Title = _name;
                                _msg.Body = "Wants to be your friend";
                                SendPushNotification(_msg);
                            });

                            CleanTask(task2);
                        }
                    });
                    CleanTask(task);
                }
            });
        }

        public void RemoveFromFriend(string _userId, Action _callback)
        {
            DatabaseReference _requestRef = Database.RootReference.Child(AppSettings.UserFriendsKey).Child(_userId).Child(AppSettings.ContainerListKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId);
            DatabaseReference _pendingRef = Database.RootReference.Child(AppSettings.UserFriendsKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).Child(AppSettings.ContainerListKey).Child(_userId);
            _requestRef.RemoveValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    CleanTask(task);
                }
                else
                {
                    _pendingRef.RemoveValueAsync().ContinueWith(task2 =>
                    {
                        if (task2.IsFaulted || task2.IsCanceled)
                        {
                            CleanTask(task2);
                        }
                        else
                        {
                            if (_callback != null)
                                UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke());
                            CleanTask(task2);
                        }
                    });
                    CleanTask(task);
                }
            });
        }

        public void CancelPendingFromFriend(string _userId, Action _callback)
        {
            DatabaseReference _requestRef = Database.RootReference.Child(AppSettings.UserRequestedFriendsKey).Child(_userId).Child(AppSettings.ContainerListKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId);
            DatabaseReference _pendingRef = Database.RootReference.Child(AppSettings.UserPendingFriendsKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).Child(AppSettings.ContainerListKey).Child(_userId);
            _requestRef.RemoveValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    CleanTask(task);
                }
                else
                {
                    _pendingRef.RemoveValueAsync().ContinueWith(task2 =>
                    {
                        if (task2.IsFaulted || task2.IsCanceled)
                        {
                            CleanTask(task2);
                        }
                        else
                        {
                            if (_callback != null)
                                UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke());
                            CleanTask(task2);
                        }
                    });
                    CleanTask(task);
                }
            });
        }

        public void AcceptFriend(string _userId, Action _callback)
        {
            DatabaseReference _requestRef = Database.RootReference.Child(AppSettings.UserPendingFriendsKey).Child(_userId).Child(AppSettings.ContainerListKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId);
            DatabaseReference _pendingRef = Database.RootReference.Child(AppSettings.UserRequestedFriendsKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).Child(AppSettings.ContainerListKey).Child(_userId);
            DatabaseReference _friendRef1 = Database.RootReference.Child(AppSettings.UserFriendsKey).Child(_userId).Child(AppSettings.ContainerListKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId);
            DatabaseReference _friendRef2 = Database.RootReference.Child(AppSettings.UserFriendsKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).Child(AppSettings.ContainerListKey).Child(_userId);
            Task _task = _requestRef.RemoveValueAsync();
            Task _task2 = _pendingRef.RemoveValueAsync();
            Task _task3 = _friendRef1.SetValueAsync(0);
            Task _task4 = _friendRef2.SetValueAsync(0);
            List<Task> taskList = new List<Task>();
            taskList.Add(_task);
            taskList.Add(_task2);
            taskList.Add(_task3);
            taskList.Add(_task4);
            Task.WhenAll(taskList).ContinueWith(task2 =>
            {
                if (task2.IsCompleted)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke());
                    CleanTask(task2);
                    CleanTask(_task);
                    CleanTask(_task2);
                    CleanTask(_task3);
                    CleanTask(_task4);
                }
            });
        }

        public DatabaseReference GetFriendReferece(string _userID)
        {
            return Database.RootReference.Child(AppSettings.UserFriendsKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).Child(AppSettings.ContainerListKey).Child(_userID);
        }

        public DatabaseReference GetRequestFriendReferece(string _userID)
        {
            return Database.RootReference.Child(AppSettings.UserRequestedFriendsKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).Child(AppSettings.ContainerListKey).Child(_userID);
        }

        public DatabaseReference GetPendingFriendReferece(string _userID)
        {
            return Database.RootReference.Child(AppSettings.UserPendingFriendsKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).Child(AppSettings.ContainerListKey).Child(_userID);
        }

        public DatabaseReference GetFriendCountReferece(string _userID)
        {
            return Database.RootReference.Child(AppSettings.UserFriendsKey).Child(_userID).Child(AppSettings.ListCountKey);
        }

        public DatabaseReference GetRequestFriendCountReferece(string _userID)
        {
            return Database.RootReference.Child(AppSettings.UserRequestedFriendsKey).Child(_userID).Child(AppSettings.ListCountKey);
        }

        public DatabaseReference GetPendingFriendCountReferece(string _userID)
        {
            return Database.RootReference.Child(AppSettings.UserPendingFriendsKey).Child(_userID).Child(AppSettings.ListCountKey);
        }

        public DatabaseReference GetFriendFeedCountReferece(string _userID)
        {
            return Database.RootReference.Child(AppSettings.FriendsPostsKey).Child(_userID).Child(AppSettings.ListCountKey);
        }

        public DatabaseReference GetAllUnreadCountReferece(string _userID)
        {
            return Database.RootReference.Child(AppSettings.UnreadMessagesKey).Child(_userID).Child(AppSettings.ListCountKey);
        }

        public DatabaseReference GetUnreadCountWithUserReferece(string _userID)
        {
            return Database.RootReference.Child(AppSettings.UnreadMessagesKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).Child(AppSettings.ContainerListKey).Child(_userID);
        }

        public DatabaseReference GetUnreadCountInGroupReferece(string _groupID)
        {
            return Database.RootReference.Child(AppSettings.UnreadMessagesKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).Child(AppSettings.ContainerListKey).Child(_groupID);
        }

        public DatabaseReference GetPostLikesCountReferense(string _postID)
        {
            return Database.RootReference.Child(AppSettings.PostLikesKey).Child(_postID).Child(AppSettings.ListCountKey);
        }

        public void ClearUnreadMessagesWithUser(string _userID)
        {
            GetUnreadCountWithUserReferece(_userID).RemoveValueAsync(); 
        }

        public void ClearUnreadMessagesGroup(string _groupID)
        {
            GetUnreadCountInGroupReferece(_groupID).RemoveValueAsync();
        }

        public void ClearUnreadFriendsFeed()
        {
            GetFriendFeedCountReferece(AppManager.USER_PROFILE.FIREBASE_USER.UserId).RemoveValueAsync();
        }

        public void CanAddToFriend(string _userID, Action<bool> _callback)
        {
            Task<DataSnapshot> fTask = GetFriendReferece(_userID).GetValueAsync();
            Task<DataSnapshot> rTask = GetRequestFriendReferece(_userID).GetValueAsync();
            Task<DataSnapshot> pTask = GetPendingFriendReferece(_userID).GetValueAsync();
            List<Task> TaskList = new List<Task>();
            TaskList.Add(fTask);
            TaskList.Add(rTask);
            TaskList.Add(pTask);
            Task.WhenAll(TaskList).ContinueWith(task2 =>
            {
                if (task2.IsCompleted)
                {
                    if ((fTask.Result == null && rTask.Result == null && pTask.Result == null) ||
                    (fTask.Result.Value == null && rTask.Result.Value == null && pTask.Result.Value == null))
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(true));

                    }
                    else
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(false));
                    }
                    CleanTask(task2);
                }
                else
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(false));
                    CleanTask(task2);
                }
            });
        }

        public void IsInFriendsList(string _userID, Action<bool> _callback)
        {
            Task<DataSnapshot> fTask = GetFriendReferece(_userID).GetValueAsync();

            fTask.ContinueWith(task2 =>
            {
                if (task2.IsCompleted)
                {
                    if (fTask.Result == null || fTask.Result.Value == null)
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(false));
                    }
                    else
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(true));
                    }
                    CleanTask(task2);
                }
                else
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(false));
                    CleanTask(task2);
                }
            });
        }

        public void SendActivity()
        {
            Database.RootReference.Child(AppSettings.RootUserKey).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).Child(AppSettings.ActivityKey).SetValueAsync(ServerValue.Timestamp);
        }

        public void GetActivity(string _userID, Action<string> _callback)
        {
            Database.RootReference.Child(AppSettings.RootUserKey).Child(_userID).Child(AppSettings.ActivityKey).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    if (task.Result != null && task.Result.Value != null && !string.IsNullOrEmpty(task.Result.Value.ToString()))
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke(task.Result.Value.ToString()));
                    }
                    CleanTask(task);
                }
                else
                {
                    CleanTask(task);
                }
            });
        }

        public void RemovePost(string _PostID, Action _callback)
        {
            Database.RootReference.Child(AppSettings.AllPostsKey).Child(_PostID).RemoveValueAsync().ContinueWith(task2 =>
            {
                if (task2.IsCompleted)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => _callback.Invoke());
                }
            });
        }

        /// <summary>
        /// Update user last activity value
        /// </summary>
        /// <param name="_callback">Add completion method</param>
        public void UpdateUserActivity(Action<CallbackSetUserActivity> _callback = null)
        {
            CallbackSetUserActivity _response = new CallbackSetUserActivity();
            DatabaseReference databaseReferense = Database.RootReference.Child(AppSettings.RootUserKey)
                .Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).Child(AppSettings.UserActivityKey);
            databaseReferense.SetValueAsync(ServerValue.Timestamp).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    _response.IsSuccess = true;
                    CleanTask(task);
                }
                else
                {
                    _response.ErrorMessage = task.Exception.Message;
                    CleanTask(task);
                }
                if (_callback != null)
                {
                    _callback.Invoke(_response);
                }
            });
        }

   
        public void GetServerTime(Action<CallbackGetServerTimestamp> _callback)
        {
            CallbackGetServerTimestamp _response = new CallbackGetServerTimestamp();
            var data = new Dictionary<string, object>();
            HttpsCallableReference _function = Functions.GetHttpsCallable("GetServerTimeStamp");
            _function.CallAsync(data).ContinueWith((task) =>
            {
                if (!task.IsFaulted && !task.IsCanceled)
                {
                    _response.IsSuccess = true;
                    _response.Data = task.Result.Data.ToString();
                    if (_callback != null)
                    {
                        _callback.Invoke(_response);
                    }
                    Debug.Log("time " + _response.Data);
                    CleanTask(task);
                }
                else
                {
                    if (_callback != null)
                    {
                        _callback.Invoke(_response);
                    }
                    Debug.Log("not get time " + _response.Data);
                    CleanTask(task);
                }

            });
        }
        /// <summary>
        /// Get current server time. Availbale when user is logined
        /// </summary>
        /// <param name="_callback">Add completion method</param>
        public void GetServerTimestamp(Action<CallbackGetServerTimestamp> _callback)
        {
            CallbackGetServerTimestamp _response = new CallbackGetServerTimestamp();
            UpdateUserActivity(_msg =>
            {
                if (_msg.IsSuccess)
                {
                    DatabaseReference databaseReferense = Database.RootReference.Child(AppSettings.RootUserKey)
                    .Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId).Child(AppSettings.UserActivityKey);
                    databaseReferense.GetValueAsync().ContinueWithOnMainThread(task =>
                    {
                        if (task.IsCompleted)
                        {
                            _response.IsSuccess = true;
                            Debug.Log("task.Result.Value " + task.Result.Value);
                            _response.Data = task.Result.Value.ToString();
                            CleanTask(task);
                        }
                        else
                        {
                            _response.ErrorMessage = task.Exception.Message;
                            CleanTask(task);
                        }
                        if (_callback != null)
                        {
                            _callback.Invoke(_response);
                        }
                    });
                }
                else
                {
                    if (_callback != null)
                    {
                        _callback.Invoke(_response);
                    }
                }
            });
        }

        /// <summary>
        /// Get user last activity by user id.
        /// </summary>
        /// <param name="_userID">User ID</param>
        /// <param name="_callback">Add completion method</param>
        public void GetUserLastActivity(string _userID, Action<CallbackGetUserActivity> _callback)
        {
            CallbackGetUserActivity _response = new CallbackGetUserActivity();
            DatabaseReference databaseReferense = Database.RootReference.Child(AppSettings.RootUserKey)
                .Child(_userID).Child(AppSettings.UserActivityKey);
            databaseReferense.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    _response.IsSuccess = true;
                    _response.Data = task.Result.Value.ToString();
                    CleanTask(task);
                }
                else
                {
                    _response.ErrorMessage = task.Exception.Message;
                    CleanTask(task);
                }
                if (_callback != null)
                {
                    _callback.Invoke(_response);
                }
            });
        }

        public void MakeCallOffer(CallObject _call, Action<CallbackMakeCallMessage> _callback)
        {
            CallbackMakeCallMessage _response = new CallbackMakeCallMessage();

            GetServerTimestamp(_msg =>
            {
                if (_msg.IsSuccess)
                {
                    string _timeStamp = _msg.Data;
                    _call.CreateTimeStamp = _timeStamp;
                    //string _callKey = System.Guid.NewGuid().ToString();
                    DatabaseReference _callref = Database.RootReference.Child(AppSettings.UserCallList).Child(_call.TargetID).Push();
                    _call.DataKey = _callref.Key;
                    string json = JsonUtility.ToJson(_call);

                    _callref.SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
                    {
                        if (task.IsFaulted || task.IsCanceled)
                        {
                            CleanTask(task);
                            _callback.Invoke(_response);
                            // Uh-oh, an error occurred!
                        }
                        else
                        {
                            _response.IsSuccess = true;
                            _callback.Invoke(_response);
                            CleanTask(task);
                        }
                    });
                }
                else
                {
                    _callback.Invoke(_response);
                }
            });
        }

        public void CancelCallOffer(CallObject _call, Action _onComplete = null)
        {
            GetCallActiveStateReference(_call).SetValueAsync(false).ContinueWithOnMainThread(task =>
            {
                if (_onComplete != null)
                {
                    _onComplete.Invoke();
                }
            });
        }

        public void AnswerCallOffer(CallObject _call, Action _onComplete = null)
        {
            GetCallAnswerStateReference(_call).SetValueAsync(true).ContinueWithOnMainThread(task =>
            {
                if (_onComplete != null)
                {
                    _onComplete.Invoke();
                }
            });
        }

        public void SetCallBisy(CallObject _call, Action _onComplete = null)
        {
            GetCallBisyStateReference(_call).SetValueAsync(true).ContinueWithOnMainThread(task =>
            {
                if (_onComplete != null)
                {
                    _onComplete.Invoke();
                }
            });
        }

        public DatabaseReference GetCallReference()
        {
            return Database.RootReference.Child(AppSettings.UserCallList).Child(AppManager.USER_PROFILE.FIREBASE_USER.UserId);
        }

        public DatabaseReference GetCallActiveStateReference(CallObject _call)
        {
            return Database.RootReference.Child(AppSettings.UserCallList).Child(_call.TargetID).Child(_call.DataKey).Child(AppSettings.CallActiveKey);
        }

        public DatabaseReference GetCallBisyStateReference(CallObject _call)
        {
            return Database.RootReference.Child(AppSettings.UserCallList).Child(_call.TargetID).Child(_call.DataKey).Child(AppSettings.CallBisyKey);
        }

        public DatabaseReference GetCallAnswerStateReference(CallObject _call)
        {
            return Database.RootReference.Child(AppSettings.UserCallList).Child(_call.TargetID).Child(_call.DataKey).Child(AppSettings.CallHasAnswerKey);
        }

        public string GetUserMessageKey(string _id1, string _id2)
        {
            List<string> sList = new List<string>();
            sList.Add(_id1);
            sList.Add(_id2);
            sList.Sort();
            return sList[0] + "-" + sList[1];
        }

        public bool IsFirebaseInited()
        {
            return FirebaseIsInited;
        }

        private void CleanTask(System.Threading.Tasks.Task _task)
        {
            _task.Dispose();
            _task = null;
        }
        public string GetNewNameWithNumber()
        {
            const int MaxLength = 4;
            if (string.IsNullOrEmpty(StaticValues.displayName))
            {
                return string.Empty;
            }
            Debug.Log("StaticValues.displayName " + StaticValues.displayName);
            var name = StaticValues.displayName.Replace(" ", string.Empty).ToUpperInvariant();
            //Debug.Log("name " + name);
            if (name.Length > MaxLength)
                name = name.Substring(0, MaxLength);

            //Debug.Log("after name " + name);
            int newName =  UnityEngine.Random.Range(1000, 9999);

            //Debug.Log("after new name " + newName);
            name = name + newName.ToString() +RandomString(4,false);
            //Debug.Log("name 2 " + name);
            return name.ToUpperInvariant();
        }
        // Generates a random string with a given size.    
        public string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  
            System.Random rand = new System.Random();
            for (var i = 0; i < size; i++)
            {
                var _char = (char)rand.Next(offset, offset + lettersOffset);
                builder.Append(_char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
        

        
        
    

        public void VerifyPhonenumber(string phoneNumber, Action<VerifyPhoneMessage> _callback, bool _silentMode = false)
        {
            AppManager.VIEW_CONTROLLER.HideAllScreen();
         
           
            StaticValues.FirebaseUserId = phoneNumber;

            if (!StaticValues.HaveReferralCode)
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
                AppManager.VIEW_CONTROLLER.HideAllScreen();
                AppManager.VIEW_CONTROLLER.ShowEnterReferralCode();
            }

           
            if (isDebug)
            {
                Debug.Log("phoneNumber in FC " + phoneNumber);
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
        private void OnVerifyOTPResponse(ServerUserDetailsResponseAddPot verifyOTPResponse)
        {
            AppManager.VIEW_CONTROLLER.HideLoading();
            if (verifyOTPResponse != null)
            {
                AppManager.Instance.OnKTLoginResponse(verifyOTPResponse);
            }
        }
      
        public void VerifyReceivedPhoneCode(string receivedCd, Action<VerifyPhoneMessage> _callback, bool _silentMode = false)
        {
            // verify otp with mobile number to the server and after that login to home page

           
            PlayerSave.singleton.GameVerifyOTPAPICall(StaticValues.FirebaseUserId, receivedCd,OnVerifyOTPResponse);
            

        }
        
        
        #region TOAST
        public IEnumerator CodeIncorrect(string CodeUI)
        {
            
            //ShowErrorMessage(CodeUI);

            yield return null;
        }
        string toastString;
        AndroidJavaObject currentActivity;
        public void ShowErrorMessage(string value)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
               
                    showToastOnUiThread(value);
                
            }
            else
            {
                Debug.Log("Error Message :" + value);
            }
        }
        void showToastOnUiThread(string toastString)
        {
            AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            this.toastString = toastString;

            currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(showToast));
        }

        void showToast()
        {
            Debug.Log("Running on UI thread");
            AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
            AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
            AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", toastString);
            AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));
            toast.Call("show");
        }
        #endregion

       
       

    }

    public class RegistrationMessage
    {
        public bool IsComplete;
        public string ErrorMessage;
        public string UserID;
    }

    public class SetUserDataMessage
    {
        public bool IsSuccess;
        public string ErrorMessage;
        public string UserID;
    }

    public class LoginMessage
    {
        public bool IsSuccess;
        public string ErrorMessage;
        public string UserID;
        public FirebaseUser FUser;
    }

    public class NotificationMessage
    {
        public string UserToken;
        public string Title;
        public string Body;
        public string UserId;
    }

    public class ChatInfoMessage
    {
        public bool IsSuccess;
        public MessageGroupInfo Info;
    }

    [System.Serializable]
    public class MessageGroupInfo
    {
        public string ChatID;
        public string ChatName;
        public List<string> Users;
        public MessageType Type;
    }

    /// <summary>
    /// Get user activity callback class
    /// </summary>
    public class CallbackGetUserActivity
    {
        public bool IsSuccess;
        public string ErrorMessage;
        public string Data;
    }

    /// <summary>
    /// Get server time stamp callback class
    /// </summary>
    public class CallbackGetServerTimestamp
    {
        public bool IsSuccess;
        public string ErrorMessage;
        public string Data;
    }

    /// <summary>
    /// Set user activity callback class
    /// </summary>
    public class CallbackSetUserActivity
    {
        public bool IsSuccess;
        public string ErrorMessage;
    }

    public class CallbackMakeCallMessage
    {
        public bool IsSuccess;
    }
    [System.Serializable]
    public class PhoneCallbackLoginMessage
    {
        public bool IsSuccess;
        public string ErrorMessage;
        public ProfileMessageCode MessageCode;
        public string UserID;
        public UserCredentials Gredentials;
        public FirebaseUser FUser;
        public string AlreadyLogin = "";
    }
    [System.Serializable]
    public class GoogleCallbackLoginMessage
    {
        public bool IsSuccess;
        public string ErrorMessage;
        public ProfileMessageCode MessageCode;
        public string UserID;
        public UserCredentials Gredentials;
        public FirebaseUser FUser;
        public string AlreadyLogin = "";
    }
    [System.Serializable]
    public class FacebookCallbackLoginMessage
    {
        public bool IsSuccess;
        public string ErrorMessage;
        public ProfileMessageCode MessageCode;
        public string UserID;
        public UserCredentials Gredentials;
        public FirebaseUser FUser;
        public string AlreadyLogin = "";
    }
    public class UserCredentials
    {
        public string UserLogin;
        public string UserPassword;
    }
    [System.Serializable]
    public enum ProfileMessageCode
    {
        None,
        EmptyEmail,
        EmptyFirstName,
        EmptyLastName,
        EmptyNickName,
        EmptyPassword,
        PasswordNotMatch,
        EmailNotValid,
        SmallPassword,
        RegistrationSuccess,
        AlreadyRegistered,
        RegistrationSuccessWithConfirm,
        EmailConfirm,
        FailedUploadImage,
        RestorePassword,
        EmptyMobile,
        UserNameNotAvailable
    }
    public class VerifyPhoneMessage
    {
        public bool IsSuccess;
        public string ErrorMessage;
        public string phoneAuthVerificationId;
    }
}
public static class StaticValues
{
    internal static bool BlockCreatingDoubleProcess = false;
    internal static string Email = "";
    internal static string phoneNumber = "";
    internal static string phoneNumberWithoutPrefix = "";
    internal static string displayName = "";
    internal static string displayNameinUC = "";

    internal static string ReferralCode = "";
    internal static string MyReferralCode = "";
    internal static string CountryCode = "";
    internal static string custom = "";
    internal static string token = "";
    internal static string FirebaseUserId = "";
    internal static string mobileVerificationId = "";
    internal static string customPicUrl = "";
    internal static string avatarPicUrl = "";
    internal static string ProviderId = "";
    internal static string WhichProvider = "";
    internal static bool OnlyOncePerSession = false;
    internal static string DepositEarningCount = "0";
    internal static string PromoEarningCount = "0";
    internal static string WithdrawEarningCount = "0";
    internal static double TotalEarningAmount = 0.0;

    internal static string ChipsCount = "";
    internal static bool HaveReferralCode = false;
    internal static string UserNameValue = "";
    internal static string GenderValue = "Male";
    internal static string FirstNameValue = "";
    internal static string LastNameValue = "";
    internal static string MobileValue = "";
    internal static string DOBValue = "";
    internal static string StreetValue_1 = "";
    internal static string StreetValue_2 = "";
    internal static string CityValue = "";
    internal static string StateValue = "";
    internal static string PinCodeValue = "";

    internal static string BankAccountNo = "";
    internal static string BankIFSCCode = "";
    internal static string BankUPIId = "";

    internal static string BankAccountNo_NR = "";
    internal static string BankIFSCCode_NR = "";
    internal static string BankUPIId_NR = "";

    internal static bool isBankDetailsSubmitted;
    internal static string isBankStatusForNewRequest;

    internal static bool isBankUPIDetailsSubmitted;
    internal static string isBankUPIStatusForNewRequest;

    internal static string PanDocNo = "";
    internal static string AddressNo = "";
    internal static string AddressType = "";
    internal static int PanCardStatus = 0;//0 // Not Provided //1// Status Pending //2// Verified //3//VerificationCancel
    internal static int AddressStatus = 0;//0 // Not Provided //1// Status Pending //2// Verified //3//VerificationCancel
    internal static string SaveAddressDocumentPic = "";
    internal static string SavePancardDocumentPic = "";
    internal static bool isbotStatus = false;
    internal static double[] PotLimit = new double[12] { 10.24, 51.2 ,102.4,256,512,1024,2048,3072,4096,5120,10240,20480};
    internal static double[] ChaalLimit = new double[12] { 1.28, 6.4, 12.8, 32, 64, 128, 256, 384, 512, 640, 1280, 2560 };
    internal static double[] BootAmount = new double[12] { 0.01, 0.05, 0.1, 0.25, 0.5, 1, 2, 3, 4, 5, 10, 20 };
    internal static bool isEmailVerify = false;
    internal static bool isMobileVerify = false;
    internal static string VersionUrl = "";
    internal static int version = 0;
    internal static int CurrentVersion = 21;
    internal static int BotHand = 5;
    internal static int ismaintenance = 0;//is maintenance off
    internal static string RAF_SMS="";
    internal static string RAF_Whatsapp="";
    internal static string Btitle = "";
    internal static string Bdescription = "";
    internal static int MinimumAmount = 50;
    internal static int FirstWDAmount = 10;
    internal static int SecondWDAmount = 50;
    internal static List<GetBannerImageDetail> getBannerImageDetails=new List<GetBannerImageDetail>();
    internal static double LastDeposit = 0;
    internal static int CurrentAddCashIndex = 0;
    internal static string CurrentBonusCode = "";
    internal static bool FirstTimeDepositPrompt = false;
    internal static bool FirstTimeDepositPromptClose = false;
    internal static bool FirstTimeDepositPromptOpen = false;
    internal static int StaticTrans_Continue=0;
    internal static bool OutOfLimitPopUp =false;
}