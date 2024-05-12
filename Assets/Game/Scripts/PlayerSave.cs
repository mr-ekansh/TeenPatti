using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using SocialApp;
using System.Collections.Generic;
//using Facebook.Unity;

[Serializable]
public struct PlayerSaveData
{
    public string namePlayer;
    public string userName;
    public string mobileNumber;
    public string Email;
    public string _distributionId;
    public string _userId;
    public string _gender;
    public string _password;
    public double moneyPlayer;
    public double moneyPlayerBeforeGame;
    public double chipsPlayer;
    public double chipsPlayerBeforeGame;
    public int score;
    public double experience;
    public int tableScreen;
    public int GirlScreen;
    public int BackgroundScreen;
    public int cardsScreen;
    public int avatarScreen;
    public string uploadPic;
}

public class PlayerSave : MonoBehaviour
{
    public static PlayerSave singleton;
    
    public PlayerSaveData playerSaveData;
    private double startMoney = 0;
    private double startChips = 500000;
    public eTable currentTable;

    
    public double bootAmount = 0;
    public double chalLimit = 0;
    public double potLimit = 0;
    public string _TableId = "";
    public bool debug = true;
    internal readonly string RegistrationAPI = "/api/registration";
    internal readonly string LoginAPI = "/api/registration?mobile=";
    internal string GetUserDetailsAPIUrl = "";
  
    private string _serverGetUserDetailsResponse;
    public static string roomName = "";
    public static string FullRoomName = "";
    public static string NewRoomName = "";
    internal readonly string BaseAPI = "https://kheltamasha.site/";

    #region GameEnter
    internal readonly string verifyOTPAPI = "api/registration";
    #endregion
    #region GameEnter
    internal readonly string GameEnterAPI = "/api/enter_game";
    #endregion

    #region BotEnter
    internal readonly string BotEnterAPI = "/api/getbot";
    #endregion

    #region GameExit
    internal readonly string GameExitAPI = "/api/gameexist";
    #endregion

    #region DepositDetails
    internal readonly string GetDepositDetailsAPI = "/api/getdepositdetails?mobile=";
    private string GetDepositDetailsAPIUrl = "";
    private string _serverGetDepositDetailsResponse;
    #endregion

    #region WithdrawDetails
    internal readonly string GetWithdrawDetailsAPI = "/api/getwithdrawdetials?mobile=";
    private string GetWithdrawDetailsAPIUrl = "";
    private string _serverGetWithdrawDetailsResponse;
    #endregion

    #region BonusDetails
    internal readonly string GetBonusDetailsAPI = "/api/getbonusdetails?mobile=";
    private string GetBonusDetailsAPIUrl = "";
    private string _serverGetBonusDetailsResponse;
    #endregion

    #region ReferDetails
    internal readonly string GetReferDetailsAPI = "/api/getreferdetails?mobile=";
    private string GetReferDetailsAPIUrl = "";
    private string _serverGetReferDetailsResponse;
    #endregion

    #region WithdrawRefundAPI
    internal readonly string WithdrawRefundAPI = "/api/refund";
    #endregion

    #region GetUserWallet
    internal readonly string GetUserWalletAPI = "/api/userWalletUpdate?mobile=";
    private string GetUserWalletAPIUrl = "";
    private string serverUserWalletResponse;
    #endregion

    #region UpdateDisId
    internal readonly string UpdateDistributorApi = "/api/updatedistributorId";
    #endregion

    #region CheckUserName
    internal readonly string CheckUserNameAPI = "/api/checkusername";
    internal string CheckUserNameAPIUrl = "";
    #endregion

    #region CheckReferralCode
    internal readonly string CheckReferralCodeAPI = "/api/referralverification?referralcode=";
    internal string CheckReferralCodeAPIUrl = "";
    #endregion

    #region NextBit
    internal readonly string NextBitAPI = "/api/nextbit";
    #endregion
    private bool UserNameExist = false;
    public GameObject ReporterObject;
    public bool GameExit=false;

    public double chaalTime = 15;
    public string RoomCodeName = "";
    public Texture2D playerTexture;

    #region PlayersOnlineAPI
    internal readonly string GetPlayerStatusAPI = "/api/GetRoominfo?roomtype=";
    internal string GetPlayerStatusAPIUrl = "";
    #endregion

    #region UpdateRefferralCode
    internal readonly string UpdateRefferCodeAPI = "/api/updatereferral";
    internal ServerReferCodeResponse serverUpdateRefferCodeResponse;
    #endregion

    #region GetBanner
    internal readonly string GetBannerAPI = "/api/get_banner";
    internal string GetBannerAPIUrl = "";


    internal readonly string GetBannerAPI2 = "/api/get_banner?uniqueId=";
    internal string GetBannerAPIUrl2 = "";
   
    public Sprite ReferImage;
    
    internal string _getBannerResponse;
    #endregion

    #region SavedCards
    internal readonly string SavedCardsAPI = "/api/save_carduser";
    #endregion

    #region UpdateProfile
    internal readonly string UpdateProfileAPI = "/api/updateuser";
    #endregion

    #region UpdateBankInfo
    internal readonly string UpdateBankAccountAPI = "/api/update_backaccount";
    #endregion

    #region UpdateKYCInfo
    internal readonly string UpdateKYCAPI = "/api/updatekyc";
    #endregion

    public int[] PublicPlayerData = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int[] FreePlayerData = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    public int[] PrivatePlayerData = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

    public Sprite[] _avatarImages;

    #region CheckBonusCode
    internal readonly string CheckBonusCodeAPI = "/api/get_banner";
    internal string CheckBonusCodeAPIUrl = "";
    #endregion

    public delegate void  onRefreshUIPotChaal();
    public static event onRefreshUIPotChaal OnRefreshUIPotChaal;


    public int _howManyBot = 0;
    public bool isCreatedRoom = false;
    public bool isJoinedRoom = false;
    [SerializeField] public List<UserInfo> botsServerData;
    // Use this for initialization
    #region UpdateToken
    internal readonly string UpdateTokenAPI = "/api/updatetoken";
    #endregion

    void Awake()
    {            
        if (singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
         
        playerSaveData = new PlayerSaveData();
        playerSaveData.moneyPlayer = startMoney;
        playerSaveData.chipsPlayer = startChips;
        playerSaveData.namePlayer = "new player";
        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if(!PlayerPrefs.HasKey("SoundOn"))
        {
            PlayerPrefs.SetInt("SoundOn", 0);//sound on
        }
        if (!PlayerPrefs.HasKey("VibrateOn"))
        {
            PlayerPrefs.SetInt("VibrateOn", 0);//vibration on
        }

        debug = true;
        
    }
    public void RaiseOnRefreshUIButtonClick()
    {
        if (OnRefreshUIPotChaal != null)
        {
            OnRefreshUIPotChaal();
        }
    }
    public void ResetData()
    {
        string save = PlayerPrefs.GetString("SavePlayer", "");
        if (string.IsNullOrEmpty(save))
        {
            playerSaveData = new PlayerSaveData();
            playerSaveData.moneyPlayer = startMoney;
            playerSaveData.namePlayer = "new player";
        }
        


    }
    public double GetCurrentMoney()
    {
        return playerSaveData.moneyPlayer;
    }

    public double GetMoneyBeforeGame()
    {
        return playerSaveData.moneyPlayerBeforeGame;
    }

    public double GetCurrentChips()
    {
        return playerSaveData.chipsPlayer;
    }

    public double GetChipsBeforeGame()
    {
        return playerSaveData.chipsPlayerBeforeGame;
    }

    public string GetCurrentNamey()
    {
        return playerSaveData.namePlayer;
    }

    public string GetMobileId()
    {
        return playerSaveData.mobileNumber;
    }
    public string GetUserId()
    {
        return playerSaveData._userId;
    }
    public string GetDistributionId()
    {
        return playerSaveData._distributionId;
    }
    public string GetUserName()
    {
        return playerSaveData.userName;
    }
    public string GetPassword()
    {
        return playerSaveData._password;
    }
    public string GetEmail()
    {
        return playerSaveData.Email;
    }
    public int GetAvatar()
    { 
        return playerSaveData.avatarScreen;
    }
    public string GetPic()
    {
        return playerSaveData.uploadPic;
    }
    public string GetGender()
    {
        return playerSaveData._gender;
    }
    public void SaveUserName(string userName)
    {
        playerSaveData.userName = userName;
        PlayerPrefs.SetString("SavePlayer", JsonUtility.ToJson(playerSaveData));
    }
    public void SavePic(string userPic)
    {
        playerSaveData.uploadPic = userPic;
        PlayerPrefs.SetString("SavePlayer", JsonUtility.ToJson(playerSaveData));
    }
    public void SavePassword(string password)
    {
        playerSaveData._password = password;
        PlayerPrefs.SetString("SavePlayer", JsonUtility.ToJson(playerSaveData));
    }
    public void SaveNewName(string newName)
    {
        if (!string.IsNullOrEmpty(newName))
        {
            playerSaveData.namePlayer = newName;
            PlayerPrefs.SetString("SavePlayer", JsonUtility.ToJson(playerSaveData));
        }
    }

    public void SaveNewMoney(double newCountMoney)
    {
       
            playerSaveData.moneyPlayer = newCountMoney;
            PlayerPrefs.SetString("SavePlayer", JsonUtility.ToJson(playerSaveData));
       
       
    }
    public void SaveNewChips(double newCountMoney)
    {
       
            playerSaveData.chipsPlayer = newCountMoney;
            PlayerPrefs.SetString("SavePlayer", JsonUtility.ToJson(playerSaveData));
        

    }
    public void SaveMobileId(string mNo)
    {
        playerSaveData.mobileNumber = mNo;
        PlayerPrefs.SetString("SavePlayer", JsonUtility.ToJson(playerSaveData));
    }
    public void SaveTableScreen(int _no)
    {
        playerSaveData.tableScreen=_no;
        PlayerPrefs.SetString("SavePlayer", JsonUtility.ToJson(playerSaveData));
    }
    public void SaveCardsScreen(int _no)
    {
        playerSaveData.cardsScreen = _no;
        PlayerPrefs.SetString("SavePlayer", JsonUtility.ToJson(playerSaveData));
    }
    public void SaveAvatarScreen(int _no)
    {
        playerSaveData.avatarScreen = _no;
        PlayerPrefs.SetString("SavePlayer", JsonUtility.ToJson(playerSaveData));
    }
    public void SaveGirlScreen(int _no)
    {
        playerSaveData.GirlScreen = _no;
        PlayerPrefs.SetString("SavePlayer", JsonUtility.ToJson(playerSaveData));
    }
    public void SaveBgScreen(int _no)
    {
        playerSaveData.BackgroundScreen = _no;
        PlayerPrefs.SetString("SavePlayer", JsonUtility.ToJson(playerSaveData));
    }
    public void SaveUserId(string userId)
    {
        playerSaveData._userId = userId;
        PlayerPrefs.SetString("SavePlayer", JsonUtility.ToJson(playerSaveData));
    }

    public void SaveDistributionId(string distributionID)
    {
        playerSaveData._distributionId = distributionID;
        PlayerPrefs.SetString("SavePlayer", JsonUtility.ToJson(playerSaveData));
    }
    public void SaveGender(string gender)
    {
        playerSaveData._gender = gender;
        PlayerPrefs.SetString("SavePlayer", JsonUtility.ToJson(playerSaveData));
    }
    public void SaveEmail(string _email)
    {
        playerSaveData.Email = _email;
        PlayerPrefs.SetString("SavePlayer", JsonUtility.ToJson(playerSaveData));
    }

    public void SaveMoneyBeforeGame()
    {
        playerSaveData.moneyPlayerBeforeGame = playerSaveData.moneyPlayer;

    }

    public void ClearMoneyBeforeGame()
    {
        playerSaveData.moneyPlayerBeforeGame = 0;

    }
    public int GetTableScreen()
    {
        //playerSaveData.tableScreen = 3;
        return playerSaveData.tableScreen;
    }
    public int GetGirlScreen()
    {
        //playerSaveData.GirlScreen = 3;
        return playerSaveData.GirlScreen;
    }
    public int GetCardsScreen()
    {
       // playerSaveData.cardsScreen = 1;
        return playerSaveData.cardsScreen;
    }
    public int GetBgScreen()
    {
        //playerSaveData.BackgroundScreen = 0;
        return playerSaveData.BackgroundScreen;
    }
    public double GetExp()
    {
        return playerSaveData.experience;
    }

    public void AddExp(double totalBet)
    {
        double AddEx = 0;
        if (playerSaveData.experience < 1000)
            AddEx = totalBet / 10;
        else if (playerSaveData.experience < 20000 && playerSaveData.experience > 1000)
            AddEx = totalBet / 100;
        else if (playerSaveData.experience > 20000)
            AddEx = totalBet / 500;

        if (AddEx > 0)
            playerSaveData.experience += AddEx;
    }

    public void AddNewUserAPICall(string _name,string _mobile,string _email,string _gender,string _deviceIdentifier,Action<ServerUserDetailsResponse> _callback)
    {
        UserInfo _userInfo = new UserInfo();
        _userInfo.username = _name;
        _userInfo.mobile = _mobile;
        _userInfo.email = _email;
        _userInfo.gender = _gender;
        string str_split = _name;
        string[] name_str = str_split.Split(' ');
        if(name_str!=null)
        {
            if(name_str.Length>0)
            {
                for(int i=0;i<name_str.Length;i++)
                {
                    if(i==0)
                    {
                        _userInfo.FirstName = name_str[0];
                    }
                    else
                    {
                        _userInfo.LastName += name_str[i];
                    }
                }
            }
        }
        _userInfo.custom = _deviceIdentifier;
        AddNewUser(_userInfo, _callback);
    }
    public void AddNewUserAPICall(string _name,string _userName, string _mobile, string _email, string _gender,string _password, string _deviceIdentifier, Action<ServerUserDetailsResponse> _callback)
    {
        UserInfo _userInfo = new UserInfo();
        _userInfo.username = _userName;
        _userInfo.mobile = _mobile;
        _userInfo.email = _email;
        _userInfo.gender = _gender;
        _userInfo.password = _password;
        string str_split = _name;
        string[] name_str = str_split.Split(' ');
        if (name_str != null)
        {
            if (name_str.Length > 0)
            {
                for (int i = 0; i < name_str.Length; i++)
                {
                    if (i == 0)
                    {
                        _userInfo.FirstName = name_str[0];
                    }
                    else
                    {
                        _userInfo.LastName += name_str[i];
                    }
                }
            }
        }
        _userInfo.custom = _deviceIdentifier;
        AddNewUser(_userInfo, _callback);
    }
    public void KhelTamashaAddNewUserAPICall(Action<ServerUserDetailsResponse> _callback)
    {
        UserInfo _userInfo = new UserInfo();
        _userInfo.custom2 = StaticValues.displayName;
        _userInfo.mobile = StaticValues.FirebaseUserId;
        _userInfo.email = StaticValues.Email;
        _userInfo.gender = "";
        _userInfo.password = StaticValues.FirebaseUserId;
        if (!string.IsNullOrEmpty(StaticValues.displayName))
        {
            string[] name_str = StaticValues.displayName.Split(' ');
            if (name_str != null)
            {
                if (name_str.Length > 0)
                {
                    for (int i = 0; i < name_str.Length; i++)
                    {
                        if (i == 0)
                        {
                            _userInfo.FirstName = name_str[0];
                        }
                        else
                        {
                            _userInfo.LastName += name_str[i];
                        }
                    }
                }
            }
        }
        _userInfo.referral_code = StaticValues.MyReferralCode;
        _userInfo.referral_codeby = StaticValues.ReferralCode;
        _userInfo.UserID = StaticValues.FirebaseUserId;
        _userInfo.custom = StaticValues.FirebaseUserId;
        _userInfo.custom5 = StaticValues.customPicUrl;
        _userInfo.HaveReferralCode = StaticValues.HaveReferralCode;
        Texture2D texture = ConvertSpriteToTexture(_avatarImages[Random.Range(0,_avatarImages.Length)]);
        if (texture != null)
        {
            byte[] uploadBytes = ImageConversion.EncodeToJPG(texture, 128);
            string savePic = System.Convert.ToBase64String(uploadBytes);
            if (PlayerSave.singleton != null)
            {
                StaticValues.avatarPicUrl = savePic;
                PlayerSave.singleton.SavePic(savePic);
                
            }
        }
        _userInfo.image_str = StaticValues.avatarPicUrl;
        _userInfo.mobile2 = StaticValues.phoneNumberWithoutPrefix;
        if(StaticValues.WhichProvider == "phone")
        {
            _userInfo.ismobile_verify = true;
            _userInfo.isemailverify = false;
            StaticValues.isMobileVerify = true;
            StaticValues.isEmailVerify = false;
        }
        else if (StaticValues.WhichProvider == "google")
        {
            _userInfo.isemailverify = true;
            _userInfo.ismobile_verify = false;
            StaticValues.isMobileVerify = false;
            StaticValues.isEmailVerify = true;
        }
       
        AddNewUser(_userInfo, _callback);
    }
    Texture2D ConvertSpriteToTexture(Sprite sprite)
    {
        try
        {
            if (sprite.rect.width != sprite.texture.width)
            {
                Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                Color[] colors = newText.GetPixels();
                Color[] newColors = sprite.texture.GetPixels((int)System.Math.Ceiling(sprite.textureRect.x),
                                                             (int)System.Math.Ceiling(sprite.textureRect.y),
                                                             (int)System.Math.Ceiling(sprite.textureRect.width),
                                                             (int)System.Math.Ceiling(sprite.textureRect.height));
                Debug.Log(colors.Length + "_" + newColors.Length);
                newText.SetPixels(newColors);
                newText.Apply();
                return newText;
            }
            else
                return sprite.texture;
        }
        catch
        {
            return sprite.texture;
        }
    }
    public void AddNewUser(UserInfo serverUserDetails, Action<ServerUserDetailsResponse> _callback)
    {
        var jsonString = JsonUtility.ToJson(serverUserDetails) ?? "";
        StartCoroutine(AddNewUserRequest(BaseAPI + "" + RegistrationAPI, jsonString.ToString(),_callback));
    }
    IEnumerator AddNewUserRequest(string url, string json, Action<ServerUserDetailsResponse> _callback)
    {
        if (debug)
        {
            Debug.Log("json in AddNewUserRequest" + json);
        }
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
            ServerUserDetailsResponse serverUserDetailsResponse = new ServerUserDetailsResponse();
            serverUserDetailsResponse.status = "500";
            serverUserDetailsResponse.message = uwr.error;
            if(_callback!=null)
            {
                _callback.Invoke(serverUserDetailsResponse);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
            ServerRegistrationResponseHandling(uwr.downloadHandler.text.ToString(),_callback);
        }
    }
    public void ServerRegistrationResponseHandling(string _serverUserDetailsResponse, Action<ServerUserDetailsResponse> _callback)
    {
        try
        {
            string result = _serverUserDetailsResponse;

            ServerUserDetailsResponse serverUserDetailsResponse = JsonUtility.FromJson<ServerUserDetailsResponse>(result.ToString());

            if (serverUserDetailsResponse != null)
            {
                if (debug)
                {
                    Debug.Log("serverUserDetailsResponse.status  " + serverUserDetailsResponse.status);
                }
                string status = serverUserDetailsResponse.status;
                if (debug)
                {
                    Debug.Log("status   " + status);
                }
                if (status.Contains("200"))
                {
                    
                    if (debug)
                    {
                        Debug.Log("Successfully registered users! ");
                    }
                    if(serverUserDetailsResponse.data!=null)
                    {

                        //debug = serverUserDetailsResponse.data.debuger;
                        StaticValues.UserNameValue = serverUserDetailsResponse.data.username;
                        
                        if (ReporterObject)
                        {
                            ReporterObject.SetActive(debug);
                        }
                    }
                    try
                    {
#if USEFBLOGAPPEVENT
                    FB.LogAppEvent(AppEventName.CompletedRegistration,null,
                       new Dictionary<string, object>()
                       {
                            { AppEventParameterName.Description, "Registered 'Log AppEvent' "+StaticValues.UserNameValue }
                       });
#endif
                    }
                    catch
                    {

                    }

                }
                else if (status.Contains("404"))
                {
                    if (debug)
                    {
                        Debug.Log("User already exists! ");
                    }
                    
                   
                }
                if (_callback != null)
                {
                    _callback.Invoke(serverUserDetailsResponse);
                }
            }
            else
            {
                ServerUserDetailsResponse _serverUDetailsResponse = new ServerUserDetailsResponse();
                _serverUDetailsResponse.status = "500";
                _serverUDetailsResponse.message = "Parsing Error!!!";
                if (_callback != null)
                {
                    _callback.Invoke(_serverUDetailsResponse);
                }
            }
            if (debug)
            {
                Debug.Log("hello   ResponseHandling ");
            }
        }
        catch
        {
            if (debug)
            {
                Debug.LogWarning("File was not in correct format");

            }
            ServerUserDetailsResponse serverUserDetailsResponse = new ServerUserDetailsResponse();
            serverUserDetailsResponse.status = "500";
            serverUserDetailsResponse.message = "Format Error!!!";
            if (_callback != null)
            {
                _callback.Invoke(serverUserDetailsResponse);
            }
        }
    }

#region GetUserDetails
    public void GetUserDetails(string _userName,string phoneNumberWithoutPrefix,string emailId,string _password, Action<ServerUserDetailsResponseAddPot> _callback,int _second)
    {
        string newId = "";
        if (!string.IsNullOrEmpty(phoneNumberWithoutPrefix))
        {
            if (phoneNumberWithoutPrefix.Length >= 10)
            {
                newId = phoneNumberWithoutPrefix;
            }
            else
            {
                if (!string.IsNullOrEmpty(emailId))
                {
                    newId = emailId;
                }
                else
                {
                    newId = _userName;
                }
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(emailId))
            {
                newId = emailId;
            }
            else
            {
                newId = _userName;
            }
        }
        if (!string.IsNullOrEmpty(newId))
        {
            GetUserDetailsAPIUrl = BaseAPI + "" + LoginAPI + newId.ToString();// + "&&password="+_password.ToString();
            if (GetUserDetailsAPIUrl != "" && (GetUserDetailsAPIUrl.StartsWith("http") || GetUserDetailsAPIUrl.StartsWith("file")))
            {
                StopCoroutine(GetUserDetailsRequest(GetUserDetailsAPIUrl, newId, _callback,_second));
                StartCoroutine(GetUserDetailsRequest(GetUserDetailsAPIUrl, newId, _callback,_second));
            }
        }
        else
        {
            ServerUserDetailsResponseAddPot serverUserDetailsResponse = new ServerUserDetailsResponseAddPot();
            serverUserDetailsResponse.status = "500";
            serverUserDetailsResponse.message = "";
            if (_callback != null)
            {
                _callback.Invoke(serverUserDetailsResponse);
            }
        }
    }
    public void KhelTamashaGetUserDetails(string _userID,string mobile2, Action<ServerUserDetailsResponseAddPot> _callback,int _second)
    {
        
        if (!string.IsNullOrEmpty(_userID))
        {
			GetUserDetailsAPIUrl = BaseAPI + "" + LoginAPI + _userID.ToString()+ "&&mobileVerificationID=" + mobile2.ToString();
            if (GetUserDetailsAPIUrl != "" && (GetUserDetailsAPIUrl.StartsWith("http") || GetUserDetailsAPIUrl.StartsWith("file")))
            {
                StopCoroutine(GetUserDetailsRequest(GetUserDetailsAPIUrl, _userID, _callback,_second));
                StartCoroutine(GetUserDetailsRequest(GetUserDetailsAPIUrl, _userID, _callback,_second));
            }
        }
        else
        {
            ServerUserDetailsResponseAddPot serverUserDetailsResponse = new ServerUserDetailsResponseAddPot();
            serverUserDetailsResponse.status = "500";
            serverUserDetailsResponse.message = "";
            if (_callback != null)
            {
                _callback.Invoke(serverUserDetailsResponse);
            }
        }
    }
    public IEnumerator GetUserDetailsRequest(string url, bool debug, string newId, Action<ServerUserDetailsResponseAddPot> _callback,int _second)
    {
        //Debug.Log("url " + url);
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            if (debug)
            {
                Debug.LogWarning("Could not download config file " + www.error);
            }
            ServerUserDetailsResponseAddPot serverUserDetailsResponse = new ServerUserDetailsResponseAddPot();
            serverUserDetailsResponse.status = "500";
            serverUserDetailsResponse.message = "Network Error!!!";
            if (_callback != null)
            {
                _callback.Invoke(serverUserDetailsResponse);
            }
        }
        else
        {
            if (debug)
            {
               Debug.Log("www getuserDetails" + www.downloadHandler.text);
            }
            _serverGetUserDetailsResponse = www.downloadHandler.text;
        }
    }
    private IEnumerator GetUserDetailsRequest(string url, string newId, Action<ServerUserDetailsResponseAddPot> _callback,int _second)
    {
        if (debug)
        {
            Debug.Log("URL: " + url);
        }
        StopCoroutine(GetUserDetailsRequest(url, debug, newId, _callback,_second));
        yield return StartCoroutine(GetUserDetailsRequest(url, debug, newId, _callback,_second));

        try
        {
            string result = _serverGetUserDetailsResponse;

            if (debug)
            {
                Debug.Log("result " + result);
            }
            ServerUserDetailsResponseAddPot serverUserDetailsResponse = JsonUtility.FromJson<ServerUserDetailsResponseAddPot>(result.ToString());

            if (serverUserDetailsResponse != null)
            {
                if (debug)
                {
                    //Debug.Log("Server user  custom2  " + serverUserDetailsResponse.data.custom);
                }
                if (serverUserDetailsResponse.status.Equals("200"))
                {
                    if (serverUserDetailsResponse.data != null)
                    {

                        //debug =  serverUserDetailsResponse.data.debuger;
                        if (!string.IsNullOrEmpty(serverUserDetailsResponse.data.username))
                        {
                            StaticValues.UserNameValue = serverUserDetailsResponse.data.username;
                        }
                        if (!string.IsNullOrEmpty(serverUserDetailsResponse.data.FirstName))
                        {
                            StaticValues.FirstNameValue = serverUserDetailsResponse.data.FirstName;
                        }
                        if (!string.IsNullOrEmpty(serverUserDetailsResponse.data.LastName))
                        {
                            StaticValues.LastNameValue = serverUserDetailsResponse.data.LastName;
                        }
                        StaticValues.GenderValue = serverUserDetailsResponse.data.gender;
                        StaticValues.MobileValue = serverUserDetailsResponse.data.mobile2;
                        if (!string.IsNullOrEmpty(serverUserDetailsResponse.data.email))
                        {
                            StaticValues.Email = serverUserDetailsResponse.data.email;
                        }
                        if (!string.IsNullOrEmpty(serverUserDetailsResponse.data.referral_code))
                        {
                            StaticValues.MyReferralCode = serverUserDetailsResponse.data.referral_code;
                        }
                        StaticValues.DOBValue = serverUserDetailsResponse.data.dob;
                        StaticValues.StreetValue_1 = serverUserDetailsResponse.data.street1;
                        StaticValues.StreetValue_2 = serverUserDetailsResponse.data.street2;
                        StaticValues.CityValue = serverUserDetailsResponse.data.city;
                        StaticValues.StateValue = serverUserDetailsResponse.data.state;
                        StaticValues.PinCodeValue = serverUserDetailsResponse.data.zip;
                        StaticValues.BankAccountNo = serverUserDetailsResponse.data.Account_no;
                        StaticValues.BankIFSCCode = serverUserDetailsResponse.data.back_ifsc_code;
                        StaticValues.AddressNo = serverUserDetailsResponse.data.addressProofno;
                        StaticValues.PanDocNo = serverUserDetailsResponse.data.PancardNo;
                        StaticValues.PanCardStatus = serverUserDetailsResponse.data.pancardstatus;
                        StaticValues.AddressStatus = serverUserDetailsResponse.data.addressp_status;
                        StaticValues.isbotStatus = serverUserDetailsResponse.data.isbotstatus;
                        StaticValues.BotHand = serverUserDetailsResponse.data.botthreshold;
                        StaticValues.ismaintenance = serverUserDetailsResponse.data.ismaintenance;
                        StaticValues.FirebaseUserId = serverUserDetailsResponse.data.mobile;
                        if (!string.IsNullOrEmpty(serverUserDetailsResponse.data.otp_ref_Id))
                        {
                            StaticValues.mobileVerificationId = serverUserDetailsResponse.data.otp_ref_Id;
                        }
                        if (!string.IsNullOrEmpty(serverUserDetailsResponse.data.avatar_url))
                        {
                            if (serverUserDetailsResponse.data.avatar_url.Length <= 200)
                            {
                                StaticValues.avatarPicUrl = BaseAPI + serverUserDetailsResponse.data.avatar_url;
                                PlayerSave.singleton.SavePic(StaticValues.avatarPicUrl);
                            }
                            else
                            {
                                StaticValues.avatarPicUrl = serverUserDetailsResponse.data.avatar_url;
                                PlayerSave.singleton.SavePic(StaticValues.avatarPicUrl);
                            }

                        }
                        if (!string.IsNullOrEmpty(StaticValues.FirstNameValue) || !string.IsNullOrEmpty(StaticValues.LastNameValue))
                        {
                            StaticValues.displayName = StaticValues.FirstNameValue + " " + StaticValues.LastNameValue;
                        }
                        if (!string.IsNullOrEmpty(StaticValues.displayName))
                        {
                            StaticValues.displayNameinUC = StaticValues.displayName.ToUpperInvariant();
                        }
                        if (!string.IsNullOrEmpty(StaticValues.MobileValue))
                        {
                            StaticValues.phoneNumberWithoutPrefix = StaticValues.MobileValue;
                        }
                        if (PlayerSave.singleton != null)
                        {
                            PlayerSave.singleton.SaveUserName(StaticValues.UserNameValue);
                            PlayerSave.singleton.SaveNewName(StaticValues.displayName);
                            PlayerSave.singleton.SaveMobileId(StaticValues.phoneNumberWithoutPrefix);
                            PlayerSave.singleton.SaveEmail(StaticValues.Email);
                            PlayerSave.singleton.SaveUserId(StaticValues.FirebaseUserId);
                            PlayerSave.singleton.SaveDistributionId(StaticValues.MyReferralCode);
                            PlayerSave.singleton.SaveGender(StaticValues.GenderValue);
                            PlayerSave.singleton.SavePassword(StaticValues.FirebaseUserId);
                            PlayerPrefs.SetString(AppSettings.LoginSaveKey, StaticValues.FirebaseUserId);
                            PlayerPrefs.SetString(AppSettings.PasswordSaveKey, StaticValues.mobileVerificationId);
                        }

                        if (ReporterObject)
                        {
                            ReporterObject.SetActive(debug);
                        }
                        StaticValues.isEmailVerify = serverUserDetailsResponse.data.isemailverify;
                        StaticValues.isMobileVerify = serverUserDetailsResponse.data.ismobile_verify;
                        StaticValues.BankAccountNo_NR = serverUserDetailsResponse.data.NewAccount_no;
                        StaticValues.BankIFSCCode_NR = serverUserDetailsResponse.data.Newback_ifsc_code;
                        StaticValues.isBankDetailsSubmitted = serverUserDetailsResponse.data.isBankDetailsSubmitted;
                        StaticValues.isBankStatusForNewRequest = serverUserDetailsResponse.data.isBankStatusForNewRequest;

                        StaticValues.BankUPIId = serverUserDetailsResponse.data.BankUPIId;
                        StaticValues.BankUPIId_NR = serverUserDetailsResponse.data.BankUPIId_NR;
                        StaticValues.isBankUPIStatusForNewRequest = serverUserDetailsResponse.data.BankUPIId_status;
                        StaticValues.isBankUPIDetailsSubmitted = serverUserDetailsResponse.data.isUPIBankDetailsSubmitted;
                    }
                }
                if (_second == 0)
                {
                    SetPotChaalBoot(serverUserDetailsResponse);
                    SetVersion(serverUserDetailsResponse);
                }
                if (_callback != null)
                {
                    _callback.Invoke(serverUserDetailsResponse);
                }

            }
            else
            {


                if (_callback != null)
                {
                    _callback.Invoke(serverUserDetailsResponse);
                }
            }
            if (debug)
            {
                Debug.Log("hello   GetUserDetails status  " + serverUserDetailsResponse.status);
            }
        }
        catch
        {
            if (debug)
            {
                Debug.LogWarning("File was not in correct format");

            }
            ServerUserDetailsResponseAddPot _serverUserDetails = new ServerUserDetailsResponseAddPot();
            _serverUserDetails.status = "500";
            if (_callback != null)
            {
                _callback.Invoke(_serverUserDetails);
            }
        }

    }
    void SetPotChaalBoot(ServerUserDetailsResponseAddPot serverUserDetailsResponseAddPot)
    {
        if (serverUserDetailsResponseAddPot != null)
        {
            if (serverUserDetailsResponseAddPot.data_2 != null)
            {
                if (serverUserDetailsResponseAddPot.data_2.Length > 0)
                {

                    for (int i = 0; i < serverUserDetailsResponseAddPot.data_2.Length; i++)
                    {
                        if (!double.IsInfinity(serverUserDetailsResponseAddPot.data_2[i].PotLimit) && !double.IsNaN(serverUserDetailsResponseAddPot.data_2[i].PotLimit))
                        {
                            StaticValues.PotLimit[i] = serverUserDetailsResponseAddPot.data_2[i].PotLimit;
                        }
                        if (!double.IsInfinity(serverUserDetailsResponseAddPot.data_2[i].chaallimited) && !double.IsNaN(serverUserDetailsResponseAddPot.data_2[i].chaallimited))
                        {
                            StaticValues.ChaalLimit[i] = serverUserDetailsResponseAddPot.data_2[i].chaallimited;
                        }
                        if (!double.IsInfinity(serverUserDetailsResponseAddPot.data_2[i].bootamount) && !double.IsNaN(serverUserDetailsResponseAddPot.data_2[i].bootamount))
                        {
                            StaticValues.BootAmount[i] = serverUserDetailsResponseAddPot.data_2[i].bootamount;
                        }
                    }
                }
       
            }
        }
        RaiseOnRefreshUIButtonClick();
    }
    void InWalletSetVersion(Addversion addversion)
    {
        if (addversion != null)
        {
            
                StaticValues.VersionUrl = addversion.url;
                StaticValues.version = addversion.version;
            Debug.Log("new version " + StaticValues.version);
        }
    }
    void InWalletRAF(string RAFSMS,string RAFWhatsapp,int maintenance)
    {
        if(!string.IsNullOrEmpty(RAFSMS))
        {
            StaticValues.RAF_SMS = RAFSMS;
        }
        if (!string.IsNullOrEmpty(RAFWhatsapp))
        {
            StaticValues.RAF_Whatsapp = RAFWhatsapp;
        }
        StaticValues.ismaintenance = maintenance;

        Debug.Log("RAF_Whatsapp version " + StaticValues.RAF_Whatsapp);

    }
    void SetVersion(ServerUserDetailsResponseAddPot serverUserDetailsResponseAddPot)
    {
        if (serverUserDetailsResponseAddPot != null)
        {
            if (serverUserDetailsResponseAddPot.data_3 != null)
            {
                StaticValues.VersionUrl = serverUserDetailsResponseAddPot.data_3.url;
                StaticValues.version = serverUserDetailsResponseAddPot.data_3.version;
            }
        }
    }
    void SetPotChaalBoot2(OnlinePlayerDetails onlinePlayerDetails)
    {
        if (onlinePlayerDetails != null)
        {
            if (onlinePlayerDetails.data_2 != null)
            {
                if (onlinePlayerDetails.data_2.Length > 0)
                {

                    for (int i = 0; i < onlinePlayerDetails.data_2.Length; i++)
                    {
                        if (!double.IsInfinity(onlinePlayerDetails.data_2[i].PotLimit) && !double.IsNaN(onlinePlayerDetails.data_2[i].PotLimit))
                        {
                            StaticValues.PotLimit[i] = onlinePlayerDetails.data_2[i].PotLimit;
                        }
                        if (!double.IsInfinity(onlinePlayerDetails.data_2[i].chaallimited) && !double.IsNaN(onlinePlayerDetails.data_2[i].chaallimited))
                        {
                            StaticValues.ChaalLimit[i] = onlinePlayerDetails.data_2[i].chaallimited;
                        }
                        if (!double.IsInfinity(onlinePlayerDetails.data_2[i].bootamount) && !double.IsNaN(onlinePlayerDetails.data_2[i].bootamount))
                        {
                            StaticValues.BootAmount[i] = onlinePlayerDetails.data_2[i].bootamount;
                        }
                    }
                }

            }
        }
        RaiseOnRefreshUIButtonClick();
    }
#endregion
    public static string FirstCharToUpper(string s)
    {
        // Check for empty string.  
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        // Return char and concat substring.  
        return char.ToUpper(s[0]) + s.Substring(1);
    }
#region TOAST

    string toastString;
    AndroidJavaObject currentActivity;
    public void ShowErrorMessage(string value)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!string.IsNullOrEmpty(value))
            {
                showToastOnUiThread(value);
            }
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

#region GameEnter


    public void GameEnterDetailsAPICall(string game_room, string game_name, string bitAmount, string mobileNumber,string _chaalLimit,string _potLimit,string _TableId,string _userType, Action<GameEnterResponse> _action,string _NewGeneratedID)
    {
        GameEnterDetails _gameEnterDetails = new GameEnterDetails();
        _gameEnterDetails.game_room = game_room;
        _gameEnterDetails.game_name = game_name;
        _gameEnterDetails.bitamount = bitAmount;
        _gameEnterDetails.mobile = mobileNumber;
        _gameEnterDetails.MaxChaal = _chaalLimit;
        _gameEnterDetails.PotLimit = _potLimit;
        _gameEnterDetails.TableId = _TableId;
        _gameEnterDetails.usertype = _userType;
        _gameEnterDetails.BidType = "Start";
        _gameEnterDetails.NewGeneratedId = _NewGeneratedID;
        GameenterDetails(_gameEnterDetails,_userType,_action);
    }
    
    public void GameenterDetails(GameEnterDetails gameEnterDetails,string _userType, Action<GameEnterResponse> _action)
    {

        var jsonString = JsonUtility.ToJson(gameEnterDetails) ?? "";
        StartCoroutine(GameenterDetailsRequest(BaseAPI + "" + GameEnterAPI, jsonString.ToString(), _userType,_action));
    }


    IEnumerator GameenterDetailsRequest(string url, string json,string _userType, Action<GameEnterResponse> _action)
    {
        if (debug)
        {
            //Debug.Log("json in gameEnterResponseDetails " + json);
        }
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
                //Debug.Log("Error While Sending: " + uwr.error);
            }
            GameEnterResponse _gameEnterResponse = new GameEnterResponse();
            _gameEnterResponse.status = "500";
            if (_userType == "P")
            {
                if (_action != null)
                {
                    _action.Invoke(_gameEnterResponse);
                }
            }
            
        }
        else
        {
            if (debug)
            {
                //Debug.Log("Received: " + uwr.downloadHandler.text);
            }
            GameenterDetailsResponseHandling(uwr.downloadHandler.text.ToString(), _userType,_action);
        }
    }
    public void GameenterDetailsResponseHandling(string _gameEnterResponseDetails,string _userType,Action<GameEnterResponse> _action)
    {
        try
        {
            string result = _gameEnterResponseDetails;

            GameEnterResponse gameEnterResponse = JsonUtility.FromJson<GameEnterResponse>(result.ToString());

            if (gameEnterResponse != null)
            {
                if (debug)
                {
                    //Debug.Log("serverUserDetailsResponse.status  " + gameEnterResponse.status);
                }
                string status = gameEnterResponse.status;
                if (debug)
                {
                    //Debug.Log("status   " + status);
                }
                if (_userType == "P")
                {
                    if (_action != null)
                    {
                        _action.Invoke(gameEnterResponse);
                    }
                }
                
            }
            else
            {
                GameEnterResponse _gameEnterResponse = new GameEnterResponse();
                _gameEnterResponse.status = "500";
                if (_userType == "P")
                {
                    if (_action != null)
                    {
                        _action.Invoke(_gameEnterResponse);
                    }
                }
            }
            if (debug)
            {
                //Debug.Log("hello   GameenterDetailsResponseHandling ");
            }
        }
        catch
        {
            if (debug)
            {
               // Debug.LogWarning("File was not in correct format");

            }
            GameEnterResponse _gameEnterResponse = new GameEnterResponse();
            _gameEnterResponse.status = "500";
            if (_userType == "P")
            {
                if (_action != null)
                {
                    _action.Invoke(_gameEnterResponse);
                }
            }
        }
    }

    public void GameEnterDetailsAPICallForBotOnly(string game_room, string game_name, string bitAmount, string mobileNumber, string _chaalLimit, string _potLimit, string _TableId, string _userType,string _NewGeneratedID, Action<GameEnterResponse> _action)
    {
        GameEnterDetails _gameEnterDetails = new GameEnterDetails();
        _gameEnterDetails.game_room = game_room;
        _gameEnterDetails.game_name = game_name;
        _gameEnterDetails.bitamount = bitAmount;
        _gameEnterDetails.mobile = mobileNumber;
        _gameEnterDetails.MaxChaal = _chaalLimit;
        _gameEnterDetails.PotLimit = _potLimit;
        _gameEnterDetails.TableId = _TableId;
        _gameEnterDetails.usertype = _userType;
        _gameEnterDetails.BidType = "Start";
        _gameEnterDetails.NewGeneratedId = _NewGeneratedID;
        GameenterDetailsForBotOnly(_gameEnterDetails, _userType,_action);
    }

    public void GameenterDetailsForBotOnly(GameEnterDetails gameEnterDetails, string _userType,Action<GameEnterResponse> _action)
    {

        var jsonString = JsonUtility.ToJson(gameEnterDetails) ?? "";
        StartCoroutine(GameenterDetailsRequestForBotOnly(BaseAPI + "" + GameEnterAPI, jsonString.ToString(), _userType,_action, gameEnterDetails.mobile));
    }


    IEnumerator GameenterDetailsRequestForBotOnly(string url, string json, string _userType,Action<GameEnterResponse> _action,string mobileNumber)
    {
        if (debug)
        {
            //Debug.Log("json in gameEnterResponseDetails " + json);
        }
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
               // Debug.Log("Error While Sending: " + uwr.error);
            }

            GameEnterResponse _gameEnterResponse = new GameEnterResponse();
            _gameEnterResponse.status = "500";
            _gameEnterResponse.message = mobileNumber;
            if (_userType == "B")
            {
                if (_action != null)
                {
                    _action.Invoke(_gameEnterResponse);
                }
            }
        }
        else
        {
            if (debug)
            {
               // Debug.Log("Received: " + uwr.downloadHandler.text);
            }
            GameenterDetailsResponseHandlingForBotOnly(uwr.downloadHandler.text.ToString(), _userType,_action, mobileNumber);
        }
    }
    public void GameenterDetailsResponseHandlingForBotOnly(string _gameEnterResponseDetails, string _userType, Action<GameEnterResponse> _action,string mobileNumber)
    {
        try
        {
            string result = _gameEnterResponseDetails;

            GameEnterResponse gameEnterResponse = JsonUtility.FromJson<GameEnterResponse>(result.ToString());

            if (gameEnterResponse != null)
            {
                if (debug)
                {
                    //Debug.Log("serverUserDetailsResponse.status  " + gameEnterResponse.status);
                }
                string status = gameEnterResponse.status;
                gameEnterResponse.message = mobileNumber;
                if (debug)
                {
                    //Debug.Log("status   " + status);
                }
                if (_userType == "B")
                {
                    if (_action != null)
                    {
                        _action.Invoke(gameEnterResponse);
                    }
                }

            }
            else
            {

                GameEnterResponse _gameEnterResponse = new GameEnterResponse();
                _gameEnterResponse.status = "500";
                _gameEnterResponse.message = mobileNumber;
                if (_userType == "B")
                {
                    if (_action != null)
                    {
                        _action.Invoke(_gameEnterResponse);
                    }
                }
            }
            if (debug)
            {
                //Debug.Log("hello   GameenterDetailsResponseHandling ");
            }
        }
        catch
        {
            if (debug)
            {
                //Debug.LogWarning("File was not in correct format");

            }

            GameEnterResponse _gameEnterResponse = new GameEnterResponse();
            _gameEnterResponse.status = "500";
            _gameEnterResponse.message = mobileNumber;
            if (_userType == "B")
            {
                if (_action != null)
                {
                    _action.Invoke(_gameEnterResponse);
                }
            }

        }
    }
#endregion

#region GameExit
    public void GameExitDetailsAPICall(string game_room, string mobileNumber,string _userType, Action<GameExitResponse> action)
    {
        GameExitDetails _gameExitDetails = new GameExitDetails();
        _gameExitDetails.game_room = game_room;
        _gameExitDetails.gamestatus = "close";
        _gameExitDetails.mobile = mobileNumber;
        _gameExitDetails.usertype = _userType;
        GameExitDetails(_gameExitDetails,action);

        
    }
    public void GameExitDetails(GameExitDetails gameExitDetails, Action<GameExitResponse> action)
    {

        var jsonString = JsonUtility.ToJson(gameExitDetails) ?? "";
        StartCoroutine(GameExitDetailsRequest(BaseAPI + "" + GameExitAPI, jsonString.ToString(),action));
    }


    IEnumerator GameExitDetailsRequest(string url, string json, Action<GameExitResponse> action)
    {
        if (debug)
        {
            //Debug.Log("json in GameExitDetailsRequest " + json);
        }
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
                //Debug.Log("Error While Sending: " + uwr.error);
            }
            GameExitResponse gameExitResponse1 = new GameExitResponse();
            gameExitResponse1.status = "500";
            if (action != null)
            {
                action.Invoke(gameExitResponse1);
            }
        }
        else
        {
            if (debug)
            {
                //Debug.Log("Received: " + uwr.downloadHandler.text);
            }
            GameExitDetailsResponseHandling(uwr.downloadHandler.text.ToString(),action);
            
        }
    }
    public void GameExitDetailsResponseHandling(string _gameExitResponseDetails, Action<GameExitResponse> action)
    {
        try
        {
            string result = _gameExitResponseDetails;

            GameExitResponse gameExitResponse = JsonUtility.FromJson<GameExitResponse>(result.ToString());

            if (gameExitResponse != null)
            {
                if (debug)
                {
                    //Debug.Log("GameExitDetailsResponseHandling.status  " + gameExitResponse.status);
                }
                if(action!=null)
                {
                    action.Invoke(gameExitResponse);
                }
            }
            else
            {
                GameExitResponse gameExitResponse1 = new GameExitResponse();
                gameExitResponse1.status = "500";
                if (action != null)
                {
                    action.Invoke(gameExitResponse1);
                }
            }
            if (debug)
            {
                //Debug.Log("hello   GameExitDetailsResponseHandling ");
            }
        }
        catch
        {
            if (debug)
            {
                //Debug.LogWarning("File was not in correct format");

            }
            GameExitResponse gameExitResponse1 = new GameExitResponse();
            gameExitResponse1.status = "500";
            if (action != null)
            {
                action.Invoke(gameExitResponse1);
            }

        }
    }
#endregion
    public void CallGameEnterForBotOnly(string newId ,double bitAmount, double _challLimit, double _PotLimit, string _TableId, string _userType, Action<GameEnterResponse> _action,string _NewGeneratedID)
    {
        if (!string.IsNullOrEmpty(FullRoomName))
        {
            GameEnterDetailsAPICallForBotOnly(FullRoomName, currentTable.ToString(), bitAmount.ToString(), newId, _challLimit.ToString(), _PotLimit.ToString(), _TableId, _userType, _NewGeneratedID, _action);

        }
    }
    public void CallGameEnter(double bitAmount, double _challLimit, double _PotLimit, string _TableId,string _userType, Action<GameEnterResponse> _action, string _NewGeneratedID)
    {
        if (!string.IsNullOrEmpty(FullRoomName))
        {
            string newId = newID();
            GameEnterDetailsAPICall(FullRoomName, currentTable.ToString(), bitAmount.ToString(), newId, _challLimit.ToString(), _PotLimit.ToString(), _TableId, _userType,_action, _NewGeneratedID);
            
        }
    }
    public string newID()
    {
        string newId = StaticValues.FirebaseUserId;
        //if (!string.IsNullOrEmpty(GetMobileId()))
        //{
        //    if (GetMobileId().Length >= 10)
        //    {
        //        newId = GetMobileId();
        //    }
        //    else
        //    {
        //        if (!string.IsNullOrEmpty(GetEmail()))
        //        {
        //            newId = GetEmail();
        //        }
        //        else
        //        {
        //            newId = GetUserName();
        //        }
        //    }
        //}
        //else
        //{
        //    if (!string.IsNullOrEmpty(GetEmail()))
        //    {
        //        newId = GetEmail();
        //    }
        //    else
        //    {
        //        newId = GetUserName();
        //    }
        //}

        return newId;
    }
    public void CallGameExitForBotOnly(string newId,double difference, string _userType, Action<GameExitResponse> action)
    {
        if (!string.IsNullOrEmpty(FullRoomName))
        {
           
                GameExitDetailsAPICall(FullRoomName, newId, _userType, action);
            
        }
    }
    public void CallGameExit(double difference,string _userType, Action<GameExitResponse> action)
    {
        if (!string.IsNullOrEmpty(FullRoomName))
        {
            string newId = newID();
            if (difference > 0)
            {
                GameExitDetailsAPICall(FullRoomName, newId, _userType, action);
            }
            else
            {
                GameExitDetailsAPICall(FullRoomName, newId, _userType, action);
            }
        }
    }
#region DepositDetails
    //-------------------------------------------------- 
    //---------------------getdeposit details-----------

    public void GetDepositDetails(string phoneNumberWithoutPrefix, string pageIndex, string pageSize, Action<GetDepositDetails> _callback)
    {
        if (debug)
        {
            Debug.Log("GetDeposit");
        }
        GetDepositDetailsAPIUrl = BaseAPI + "" + GetDepositDetailsAPI + phoneNumberWithoutPrefix.ToString() + "&&pageIndex=" + pageIndex.ToString() + "&&PageS=" + pageSize.ToString();
        if (GetDepositDetailsAPIUrl != "" && (GetDepositDetailsAPIUrl.StartsWith("http") || GetDepositDetailsAPIUrl.StartsWith("file")))
        {
            StopCoroutine(GetDepositDetailsRequest(GetDepositDetailsAPIUrl, phoneNumberWithoutPrefix, _callback));
            StartCoroutine(GetDepositDetailsRequest(GetDepositDetailsAPIUrl, phoneNumberWithoutPrefix, _callback));
        }
    }
    public void GetDepositDetails(string phoneNumberWithoutPrefix, Action<GetDepositDetails> _callback)
    {
        if (debug)
        {
            Debug.Log("GetDeposit");
        }
        GetDepositDetailsAPIUrl = BaseAPI + "" + GetDepositDetailsAPI + phoneNumberWithoutPrefix.ToString();
        if (GetDepositDetailsAPIUrl != "" && (GetDepositDetailsAPIUrl.StartsWith("http") || GetDepositDetailsAPIUrl.StartsWith("file")))
        {
            StopCoroutine(GetDepositDetailsRequest(GetDepositDetailsAPIUrl, phoneNumberWithoutPrefix, _callback));
            StartCoroutine(GetDepositDetailsRequest(GetDepositDetailsAPIUrl, phoneNumberWithoutPrefix, _callback));
        }
    }
    public IEnumerator GetDepositDetailsRequest(string url, bool debug, string phoneNumberWithoutPrefix, Action<GetDepositDetails> _callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);


        yield return www.SendWebRequest();


        if (www.isNetworkError || www.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Could not download config file " + www.error);
            }
            GetDepositDetails depositDetails = new GetDepositDetails();
            depositDetails.status = "500";
            if (_callback != null)
            {
                _callback.Invoke(depositDetails);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("www getDepositDetails" + www.downloadHandler.text);
            }
            _serverGetDepositDetailsResponse = www.downloadHandler.text;
        }
    }
    private IEnumerator GetDepositDetailsRequest(string url, string phoneNumberWithoutPrefix, Action<GetDepositDetails> _callback)
    {
        if (debug)
        {
            Debug.Log("URL: " + url);
        }
        StopCoroutine(GetDepositDetailsRequest(url, debug, phoneNumberWithoutPrefix, _callback));
        yield return StartCoroutine(GetDepositDetailsRequest(url, debug, phoneNumberWithoutPrefix, _callback));

        try
        {
            string result = _serverGetDepositDetailsResponse;

            GetDepositDetails _depositDetails = JsonUtility.FromJson<GetDepositDetails>(result.ToString());

            if (_depositDetails != null)
            {
                if (debug)
                {
                    Debug.Log("hello   GetDepositDetails status  " + _depositDetails.status);
                }
                if (_callback != null)
                {
                    _callback.Invoke(_depositDetails);
                }

            }
            else
            {
                GetDepositDetails depositDetails = new GetDepositDetails();
                depositDetails.status = "500";
                if (_callback != null)
                {
                    _callback.Invoke(depositDetails);
                }
            }
            
        }
        catch
        {
            if (debug)
            {
                Debug.Log("File was not in correct format");

            }
            GetDepositDetails _depositDetails = new GetDepositDetails();
            _depositDetails.status = "500";
            if (_callback != null)
            {
                _callback.Invoke(_depositDetails);
            }
        }

    }

#endregion

#region GetUserWallet
    public void GetUserWalletDetails(string phoneNumberWithoutPrefix)
    {
        if (debug)
        {
            Debug.Log("GetUserWallet");
        }
        GetUserWalletAPIUrl = BaseAPI + "" + GetUserWalletAPI + phoneNumberWithoutPrefix.ToString();
        if (GetUserWalletAPIUrl != "" && (GetUserWalletAPIUrl.StartsWith("http") || GetUserWalletAPIUrl.StartsWith("file")))
        {
            StopCoroutine(GetUserWalletRequest(GetUserWalletAPIUrl, phoneNumberWithoutPrefix));
            StartCoroutine(GetUserWalletRequest(GetUserWalletAPIUrl, phoneNumberWithoutPrefix));
        }
    }
    public IEnumerator GetUserWalletRequest(string url, bool debug, string phoneNumberWithoutPrefix)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);


        yield return www.SendWebRequest();


        if (www.isNetworkError || www.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Could not download config file " + www.error);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("www getPaymentDetails" + www.downloadHandler.text);
            }
            serverUserWalletResponse = www.downloadHandler.text;
        }
    }
    public void RefreshCoins()
    {
        if (PlayerSave.singleton != null)
        {
            PlayerSave.singleton.GetUserWalletDetails(PlayerSave.singleton.newID());
        }
    }
    private IEnumerator GetUserWalletRequest(string url, string phoneNumberWithoutPrefix)
    {
        if (debug)
        {
            //Debug.Log("URL: " + url);
        }
        StopCoroutine(GetUserWalletRequest(url, debug, phoneNumberWithoutPrefix));
        yield return StartCoroutine(GetUserWalletRequest(url, debug, phoneNumberWithoutPrefix));

        try
        {
            string result = serverUserWalletResponse;

            ServerUserWalletResponse _serverUserWalletResponse = JsonUtility.FromJson<ServerUserWalletResponse>(result.ToString());

            if (_serverUserWalletResponse != null)
            {

                if (_serverUserWalletResponse.status.Contains("200"))
                {
                   
                    if (!double.IsInfinity(_serverUserWalletResponse.data.Deposit_Cash) && !double.IsNaN(_serverUserWalletResponse.data.Deposit_Cash))
                    {
                        StaticValues.DepositEarningCount = _serverUserWalletResponse.data.Deposit_Cash.ToString("F2");
                        StaticValues.TotalEarningAmount = _serverUserWalletResponse.data.Deposit_Cash;
                        SaveNewMoney(StaticValues.TotalEarningAmount);
                    }
                    if (!double.IsInfinity(_serverUserWalletResponse.data.Wining_Cash) && !double.IsNaN(_serverUserWalletResponse.data.Wining_Cash))
                    {
                        StaticValues.WithdrawEarningCount = _serverUserWalletResponse.data.Wining_Cash.ToString("F2");

                        if (debug)
                        {
                            //Debug.Log("_serverUserWalletResponse.data.Wining_Cash " + _serverUserWalletResponse.data.Wining_Cash);
                        }
                        if (!double.IsInfinity(_serverUserWalletResponse.data.Deposit_Cash) && !double.IsNaN(_serverUserWalletResponse.data.Deposit_Cash))
                        {

                            StaticValues.TotalEarningAmount = _serverUserWalletResponse.data.Deposit_Cash + _serverUserWalletResponse.data.Wining_Cash;
                            SaveNewMoney(StaticValues.TotalEarningAmount);
                            if (debug)
                            {
                                Debug.Log("StaticValues.TotalEarningAmount " + StaticValues.TotalEarningAmount);
                            }
                        }
                    }
                    if (!double.IsInfinity(_serverUserWalletResponse.data.Bonus_Cash) && !double.IsNaN(_serverUserWalletResponse.data.Bonus_Cash))
                    {
                        StaticValues.PromoEarningCount = _serverUserWalletResponse.data.Bonus_Cash.ToString("F2");

                        if (debug)
                        {
                            //Debug.Log("_serverUserWalletResponse.data.Bonus_Cash " + _serverUserWalletResponse.data.Bonus_Cash);
                        }
                        if (!double.IsInfinity(_serverUserWalletResponse.data.Deposit_Cash) && !double.IsNaN(_serverUserWalletResponse.data.Deposit_Cash) && !double.IsInfinity(_serverUserWalletResponse.data.Wining_Cash) && !double.IsNaN(_serverUserWalletResponse.data.Wining_Cash))
                        {

                            StaticValues.TotalEarningAmount = _serverUserWalletResponse.data.Bonus_Cash + _serverUserWalletResponse.data.Deposit_Cash + _serverUserWalletResponse.data.Wining_Cash;
                            SaveNewMoney(StaticValues.TotalEarningAmount);
                            if (debug)
                            {
                                Debug.Log("StaticValues.TotalEarningAmount " + StaticValues.TotalEarningAmount);
                            }
                        }
                    }


                    StaticValues.MinimumAmount = _serverUserWalletResponse.data.minimum_withdraw_amount;
					Debug.Log("StaticValues.MinimumAmount... "+StaticValues.MinimumAmount );

                    InWalletSetVersion(_serverUserWalletResponse.version);
                    InWalletRAF(_serverUserWalletResponse.RAF_SMS, _serverUserWalletResponse.RAF_Whatsapp, _serverUserWalletResponse.ismaintenance);
                }
                else
                {
                    
                }

                
            }
           
        }
        catch
        {
            if (debug)
            {
                Debug.Log("File was not in correct format");

            }
            
        }
        //RefreshCoins();
    }

#endregion

#region UpdateDisID


    public void OnUpdateDisId(string distributor_Id,Action<UpdateDistributorResponse> _callback)
    {
        DistributorResponse _distributorResponse = new DistributorResponse();
        _distributorResponse.mobile = newID();
        _distributorResponse.distributor_Id = distributor_Id;
        OnUpdateDisIdDetails(_distributorResponse, _callback);
    }

    public void OnUpdateDisIdDetails(DistributorResponse _distributorResponse, Action<UpdateDistributorResponse> _callback)
    {

        var jsonString = JsonUtility.ToJson(_distributorResponse) ?? "";
        StartCoroutine(OnUpdateDisIdDetailsRequest(BaseAPI + "" + UpdateDistributorApi, jsonString.ToString(), _callback));
    }


    IEnumerator OnUpdateDisIdDetailsRequest(string url, string json, Action<UpdateDistributorResponse> _callback)
    {
        if (debug)
        {
            Debug.Log("json in OnUpdateDisIdDetailsRequest " + json);
        }
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
            OnUpdateDisIdDetailsRequestHandling(uwr.downloadHandler.text.ToString(), _callback);
        }
    }
    public void OnUpdateDisIdDetailsRequestHandling(string _ResponseDetails,Action<UpdateDistributorResponse> _callback)
    {
        try
        {
            string result = _ResponseDetails;

            UpdateDistributorResponse updateDisResponse = JsonUtility.FromJson<UpdateDistributorResponse>(result.ToString());

            if (updateDisResponse != null)
            {
                if (debug)
                {
                    Debug.Log("updateDisResponse.status  " + updateDisResponse.status);
                }
                string status = updateDisResponse.status;
                if (debug)
                {
                    Debug.Log("status   " + status);
                }
                if (status.Contains("200"))
                {
                    if (debug)
                    {
                        Debug.Log("successfully update dis id "+updateDisResponse);
                    }
                    if (updateDisResponse.data != null)
                    {
                        SaveDistributionId(updateDisResponse.data.distributor_Id.ToString());
                    }
                }
                else if (status.Contains("404"))
                {
                    if (debug)
                    {
                        Debug.Log("Invalid Distributor Id! ");
                    }
                }
                if(_callback!=null)
                {
                    _callback.Invoke(updateDisResponse);
                }
            }
            if (debug)
            {
                Debug.Log("hello   OnUpdateDisIdDetailsRequestHandling ");
            }
        }
        catch
        {
            if (debug)
            {
                Debug.LogWarning("File was not in correct format");

            }
            UpdateDistributorResponse updateDisResponse = new UpdateDistributorResponse();
            updateDisResponse.status = "500";
            if (_callback != null)
            {
                _callback.Invoke(updateDisResponse);
            }
        }
    }
#endregion

#region CheckUserName
    public void CallUserName(string _userName,Action<UserNameResult> _callback)
    {
        CheckUserNameAPIUrl = BaseAPI + "" + CheckUserNameAPI + _userName.ToString();
        if (CheckUserNameAPIUrl != "" && (CheckUserNameAPIUrl.StartsWith("http") || CheckUserNameAPIUrl.StartsWith("file")))
        {
            StopCoroutine(CallUserNameRequest(CheckUserNameAPIUrl, _userName, _callback));
            StartCoroutine(CallUserNameRequest(CheckUserNameAPIUrl, _userName, _callback));
        }
    }
    public void CallUserNamePost(string userName, Action<UserNameResult> _callback)
    {
        UserNameEnter userNameEnter = new UserNameEnter();
        userNameEnter.mobile = newID();
        userNameEnter._userName = userName;
        OnUpdateUserNameDetails(userNameEnter, _callback);
    }

    public void OnUpdateUserNameDetails(UserNameEnter _userNameEnter, Action<UserNameResult> _callback)
    {

        var jsonString = JsonUtility.ToJson(_userNameEnter) ?? "";
        StartCoroutine(OnUpdateUserNameDetailsRequest(BaseAPI + "" + CheckUserNameAPI, jsonString.ToString(), _callback));
    }


    IEnumerator OnUpdateUserNameDetailsRequest(string url, string json, Action<UserNameResult> _callback)
    {
        if (debug)
        {
            Debug.Log("json in OnUpdateUserNameDetailsRequest " + json);
        }
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
            UserNameResult _userNameResult = new UserNameResult();
            _userNameResult.status = "500";
            _userNameResult.message = uwr.error;
            if (_callback != null)
            {
                _callback.Invoke(_userNameResult);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
            OnUpdateUserNameDetailsRequestHandling(uwr.downloadHandler.text.ToString(), _callback);
        }
    }
    public void OnUpdateUserNameDetailsRequestHandling(string _ResponseDetails, Action<UserNameResult> _callback)
    {
        try
        {
            string result = _ResponseDetails;

            UserNameResult _userNameResult = JsonUtility.FromJson<UserNameResult>(result.ToString());

            if (_userNameResult != null)
            {
               
                if (_callback != null)
                {
                    _callback.Invoke(_userNameResult);
                }

            }
            else
            {
                UserNameResult userNameResult = new UserNameResult();
                userNameResult.status = "500";
                _userNameResult.message = "Format Error!!!";
                if (_callback != null)
                {
                    _callback.Invoke(userNameResult);
                }
            }
        }
        catch (Exception e)
        {
            if (debug)
            {
                Debug.LogWarning("File was not in correct format");

            }
            UserNameResult _userNameResult = new UserNameResult();
            _userNameResult.status = "500";
            _userNameResult.message = e.Message;
            if (_callback != null)
            {
                _callback.Invoke(_userNameResult);
            }
        }
    }
    public IEnumerator CallUserNameRequest(string url, string _userName, Action<UserNameResult> _callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            if (debug)
            {
                Debug.LogWarning("Could not download config file " + www.error);
            }
            UserNameResult _userNameResult = new UserNameResult();
            _userNameResult.status = "500";
            _userNameResult.message = www.error;
            if (_callback != null)
            {
                _callback.Invoke(_userNameResult);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("www getuserDetails" + www.downloadHandler.text);
            }
            try
            {
                string result = www.downloadHandler.text;

                if (debug)
                {
                    Debug.Log("result " + result);
                }
                UserNameResult _userNameResult = JsonUtility.FromJson<UserNameResult>(result.ToString());

                if (_userNameResult != null)
                {
                    if (debug)
                    {
                        Debug.Log("Server user  custom2  " + _userNameResult.status);
                    }
                  
                    if (_callback != null)
                    {
                        _callback.Invoke(_userNameResult);
                    }

                }
                else
                {
                    UserNameResult userNameResult = new UserNameResult();
                    userNameResult.status = "500";
                    _userNameResult.message = "Format Error!!!";
                    if (_callback != null)
                    {
                        _callback.Invoke(userNameResult);
                    }
                }
                
            }
            catch(Exception e)
            {
                if (debug)
                {
                    Debug.LogWarning("File was not in correct format");

                }
                UserNameResult _userNameResult = new UserNameResult();
                _userNameResult.status = "500";
                _userNameResult.message = e.Message;
                if (_callback != null)
                {
                    _callback.Invoke(_userNameResult);
                }
            }
        }
    }
    public void SetUserNameExist(bool active)
    {
        UserNameExist = active;
    }
    public bool IsUserNameExist()
    {
        return UserNameExist;
    }
#endregion
#region NextBit

    public void CallUpdateAmountForDestroyCallByMaster(string newId,double bitAmount, string _FullRoomName, string plusOrMinus_Symbol, string _BidType, string _userType, string GameRoom_2, string _NewGeneratedId)
    {
        if (!string.IsNullOrEmpty(_FullRoomName))
        {



            GameUpdateNextBitAPICallForPlayerCallByMaster(_FullRoomName, plusOrMinus_Symbol, bitAmount.ToString(), newId, _BidType, _userType, GameRoom_2, _NewGeneratedId);

        }
    }
    public void GameUpdateNextBitAPICallForPlayerCallByMaster(string game_room, string plusOrMinus_Symbol, string bitAmount, string mobileNumber, string _BidType, string _userType, string GameRoom_2, string _NewGeneratedId)
    {
        NextBit _nextBit = new NextBit();
        _nextBit.game_room = game_room;
        _nextBit.symboles = plusOrMinus_Symbol;
        _nextBit.amount = bitAmount;
        _nextBit.mobile = mobileNumber;
        _nextBit.game_name = currentTable.ToString() + " : Next Bit";
        _nextBit.BidType = _BidType;
        _nextBit.usertype = _userType;
        _nextBit.GameRoom_2 = GameRoom_2;
        _nextBit.NewGeneratedId = _NewGeneratedId;
        GameUpdateNextBitAPICallDetailsForPlayerCallByMaster(_nextBit, _userType);
    }

    public void GameUpdateNextBitAPICallDetailsForPlayerCallByMaster(NextBit _nextBit, string _userType)
    {

        var jsonString = JsonUtility.ToJson(_nextBit) ?? "";
        StartCoroutine(GameUpdateNextBitAPICallDetailsRequestForPlayerCallByMaster(BaseAPI + "" + NextBitAPI, jsonString.ToString(), _userType));
    }


    IEnumerator GameUpdateNextBitAPICallDetailsRequestForPlayerCallByMaster(string url, string json, string _userType)
    {
        if (debug)
        {
            Debug.Log("json in GameUpdateNextBitAPICallDetailsRequestForPlayerCallByMaster " + json);
        }
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
            
        }
    }
    public void CallUpdateAmount2(double bitAmount, string _FullRoomName, string plusOrMinus_Symbol, string _BidType,string _userType, string GameRoom_2,string _NewGeneratedId)
    {
        if (!string.IsNullOrEmpty(_FullRoomName))
        {

            string newId = newID();

            GameUpdateNextBitAPICallForPlayer(_FullRoomName , plusOrMinus_Symbol, bitAmount.ToString(), newId, _BidType, _userType,GameRoom_2, _NewGeneratedId);

        }
    }
    public void GameUpdateNextBitAPICallForPlayer(string game_room, string plusOrMinus_Symbol, string bitAmount, string mobileNumber, string _BidType,string _userType, string GameRoom_2,string _NewGeneratedId)
    {
        NextBit _nextBit = new NextBit();
        _nextBit.game_room = game_room;
        _nextBit.symboles = plusOrMinus_Symbol;
        _nextBit.amount = bitAmount;
        _nextBit.mobile = mobileNumber;
        _nextBit.game_name = currentTable.ToString() + " : Next Bit";
        _nextBit.BidType = _BidType;
        _nextBit.usertype = _userType;
        _nextBit.GameRoom_2 = GameRoom_2;
        _nextBit.NewGeneratedId = _NewGeneratedId;
        GameUpdateNextBitAPICallDetailsForPlayer(_nextBit,_userType);
    }

    public void GameUpdateNextBitAPICallDetailsForPlayer(NextBit _nextBit,string _userType)
    {

        var jsonString = JsonUtility.ToJson(_nextBit) ?? "";
        StartCoroutine(GameUpdateNextBitAPICallDetailsRequestForPlayer(BaseAPI + "" + NextBitAPI, jsonString.ToString(),_userType));
    }


    IEnumerator GameUpdateNextBitAPICallDetailsRequestForPlayer(string url, string json,string _userType)
    {
        if (debug)
        {
            Debug.Log("json in GameUpdateNextBitAPICallDetailsRequestForPlayer " + json);
        }
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
                //Debug.Log("Error While Sending: " + uwr.error);
            }
        }
        else
        {
            if (debug)
            {
               // Debug.Log("Received: " + uwr.downloadHandler.text);
            }
            GameUpdateNextBitAPICallDetailsHandlingForPlayer(uwr.downloadHandler.text.ToString(),_userType);
        }
    }
    public void GameUpdateNextBitAPICallDetailsHandlingForPlayer(string _gameUpdateNextBitResponseDetails,string _userType)
    {
        try
        {
            string result = _gameUpdateNextBitResponseDetails;

            NextBitResponse _nextBitResponse = JsonUtility.FromJson<NextBitResponse>(result.ToString());

            if (_nextBitResponse != null)
            {
                if (debug)
                {
                    Debug.Log("_nextBitResponse.status  " + _nextBitResponse.status);
                }
                string status = _nextBitResponse.status;
                if (debug)
                {
                    Debug.Log("status   " + status);
                }
                if (status.Contains("200"))
                {
                    if (debug)
                    {
                        Debug.Log("_nextBitResponse 200  "+ _nextBitResponse.message);
                    }
                    if (_nextBitResponse.data != null)
                    {

                        if (_userType == "P")
                        {
                            if (!double.IsInfinity(_nextBitResponse.data.Deposit_Cash) && !double.IsNaN(_nextBitResponse.data.Deposit_Cash))
                            {
                                StaticValues.DepositEarningCount = _nextBitResponse.data.Deposit_Cash.ToString("F2");
                                StaticValues.TotalEarningAmount = _nextBitResponse.data.Deposit_Cash;
                                SaveNewMoney(StaticValues.TotalEarningAmount);
                               
                                if (debug)
                                {
                                    Debug.Log("_nextBitResponse.data.Deposit_Cash " + _nextBitResponse.data.Deposit_Cash);
                                }
                            }
                            if (!double.IsInfinity(_nextBitResponse.data.Wining_Cash) && !double.IsNaN(_nextBitResponse.data.Wining_Cash))
                            {
                                StaticValues.WithdrawEarningCount = _nextBitResponse.data.Wining_Cash.ToString("F2");

                                if (debug)
                                {
                                    Debug.Log("_nextBitResponse.data.Wining_Cash " + _nextBitResponse.data.Wining_Cash);
                                }
                                if (!double.IsInfinity(_nextBitResponse.data.Deposit_Cash) && !double.IsNaN(_nextBitResponse.data.Deposit_Cash))
                                {

                                    StaticValues.TotalEarningAmount = _nextBitResponse.data.Deposit_Cash + _nextBitResponse.data.Wining_Cash;
                                    SaveNewMoney(StaticValues.TotalEarningAmount);
                                    if (debug)
                                    {
                                        Debug.Log("StaticValues.TotalEarningAmount " + StaticValues.TotalEarningAmount);
                                    }
                                }
                            }
                            if (!double.IsInfinity(_nextBitResponse.data.Bonus_Cash) && !double.IsNaN(_nextBitResponse.data.Bonus_Cash))
                            {
                                StaticValues.PromoEarningCount = _nextBitResponse.data.Bonus_Cash.ToString("F2");


                                if (!double.IsInfinity(_nextBitResponse.data.Deposit_Cash) && !double.IsNaN(_nextBitResponse.data.Deposit_Cash) && !double.IsInfinity(_nextBitResponse.data.Wining_Cash) && !double.IsNaN(_nextBitResponse.data.Wining_Cash))
                                {

                                    StaticValues.TotalEarningAmount = _nextBitResponse.data.Bonus_Cash + _nextBitResponse.data.Deposit_Cash + _nextBitResponse.data.Wining_Cash;
                                    SaveNewMoney(StaticValues.TotalEarningAmount);
                                    

                                }
                            }
                        }
                    }
                }
                else if (status.Contains("404"))
                {
                    if (debug)
                    {
                        Debug.Log("_nextBitResponse 404  " + _nextBitResponse.message);
                    }
                }
            }
            if (debug)
            {
                Debug.Log("_nextBitResponse ---  " + _nextBitResponse.message);
            }
        }
        catch
        {
            if (debug)
            {
                Debug.LogWarning("File was not in correct format");

            }
        }
    }
#endregion

#region CheckReferralCodeExits
    public void CheckReferralCode(string _referralCode, Action<ReferralCodeResult> _callback)
    {
        CheckReferralCodeAPIUrl = BaseAPI + "" + CheckReferralCodeAPI + _referralCode.ToString();
        if (CheckReferralCodeAPIUrl != "" && (CheckReferralCodeAPIUrl.StartsWith("http") || CheckReferralCodeAPIUrl.StartsWith("file")))
        {
            StopCoroutine(CheckReferralCodeRequest(CheckReferralCodeAPIUrl, _referralCode, _callback));
            StartCoroutine(CheckReferralCodeRequest(CheckReferralCodeAPIUrl, _referralCode, _callback));
        }
    }
    public IEnumerator CheckReferralCodeRequest(string url, string _userName, Action<ReferralCodeResult> _callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            if (debug)
            {
                Debug.LogWarning("Could not download config file " + www.error);
            }
            ReferralCodeResult _referralCodeResult = new ReferralCodeResult();
            _referralCodeResult.status = "500";
            _referralCodeResult.message = "Error : "+www.error;
            if (_callback != null)
            {
                _callback.Invoke(_referralCodeResult);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("www check referral" + www.downloadHandler.text);
            }
            try
            {
                string result = www.downloadHandler.text;

                if (debug)
                {
                    Debug.Log("result " + result);
                }
                ReferralCodeResult _referralCodeResult = JsonUtility.FromJson<ReferralCodeResult>(result.ToString());

                if (_referralCodeResult != null)
                {
                    if (debug)
                    {
                        Debug.Log("Server user  custom2  " + _referralCodeResult.status);
                    }

                    if (_callback != null)
                    {
                        _callback.Invoke(_referralCodeResult);
                    }

                }
                else
                {
                    _referralCodeResult = new ReferralCodeResult();
                    _referralCodeResult.status = "500";
                    _referralCodeResult.message = "Format Error!!!";
                    if (_callback != null)
                    {
                        _callback.Invoke(_referralCodeResult);
                    }
                }
                if (debug)
                {
                    Debug.Log("hello _userNameResult status  " + _referralCodeResult.status);
                }
            }
            catch(Exception e)
            {
                if (debug)
                {
                    Debug.LogWarning("File was not in correct format");

                }
                ReferralCodeResult _referralCodeResult = new ReferralCodeResult();
                _referralCodeResult.status = "500";
                _referralCodeResult.message = e.Message;
                if (_callback != null)
                {
                    _callback.Invoke(_referralCodeResult);
                }
            }
        }
    }
#endregion

#region PlayersOnlineAPI
    //-------------------------------------------------- 
    //---------------------PlayersOnlineAPI--------------
    public void OnPlayersOnline(string PublicPrivate,int whichOne,Action<int> action)
    {
        if (debug)
        {
            Debug.Log("OnPlayersOnline");
        }
        //if(whichOne==0)//public
        //{
        //    PublicPlayerData = new int[] { Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50) };
        //}
        //else if(whichOne==1)//private
        //{
        //    PrivatePlayerData = new int[] { Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50)};

        //}
        //else if(whichOne==2)//free
        //{
        //    FreePlayerData = new int[] { Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50), Random.Range(30, 50)};
        //}
        GetPlayerStatusAPIUrl = BaseAPI + "" + GetPlayerStatusAPI + PublicPrivate.ToString();
        if (GetPlayerStatusAPIUrl != "" && (GetPlayerStatusAPIUrl.StartsWith("http") || GetPlayerStatusAPIUrl.StartsWith("file")))
        {
            StopCoroutine(GetPlayerStatusAPIRequest(GetPlayerStatusAPIUrl, whichOne, action));
            StartCoroutine(GetPlayerStatusAPIRequest(GetPlayerStatusAPIUrl, whichOne, action));
        }
    }
    public IEnumerator GetPlayerStatusAPIRequest(string url,int whichOne, Action<int> action)
    {
        if(debug)
        {
            Debug.Log("url in GetPlayerStatusAPIRequest" + url);
        }
        UnityWebRequest www = UnityWebRequest.Get(url);


        yield return www.SendWebRequest();


        if (www.isNetworkError || www.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Could not download config file....6 " + www.error);
            }
            if(action!=null)
            {
                action.Invoke(-1);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("www GetPlayerStatusAPIRequest" + www.downloadHandler.text);
            }
            try
            {
                string result = www.downloadHandler.text;

                if (!string.IsNullOrEmpty(result))
                {
                    OnlinePlayerDetails _OnlinePlayerDetails = JsonUtility.FromJson<OnlinePlayerDetails>(result.ToString());

                    if (_OnlinePlayerDetails != null)
                    {

                        if (_OnlinePlayerDetails.status == 200)
                        {
                            if (_OnlinePlayerDetails.data != null)
                            {
                                if (_OnlinePlayerDetails.data.Length > 0)
                                {
                                    if (whichOne == 0)
                                    {
                                        FetchPublicPlayersOnlineData(_OnlinePlayerDetails,action);
                                        if (action != null)
                                        {
                                            action.Invoke(0);
                                        }
                                    }
                                    else if (whichOne == 1)
                                    {
                                        FetchPrivatePlayersOnlineData(_OnlinePlayerDetails,action);
                                        if (action != null)
                                        {
                                            action.Invoke(1);
                                        }
                                    }
                                    else if (whichOne == 2)
                                    {
                                        FetchFreePlayersOnlineData(_OnlinePlayerDetails, action);
                                        if (action != null)
                                        {
                                            action.Invoke(2);
                                        }
                                    }

                                }

                            }
                            if (debug)
                            {
                                Debug.Log("hello   PlayersOnline  ");
                            }

                            InWalletSetVersion(_OnlinePlayerDetails.version);
                            InWalletRAF(_OnlinePlayerDetails.RAF_SMS, _OnlinePlayerDetails.RAF_Whatsapp, _OnlinePlayerDetails.ismaintenance);
                        }
                        SetPotChaalBoot2(_OnlinePlayerDetails);
                    }
                    if (debug)
                    {
                        Debug.Log("hello   GetPlayerStatusAPIRequest status  " + _OnlinePlayerDetails.status);
                    }
                }
            }
            catch(Exception e)
            {
                if (debug)
                {
                    Debug.LogError("File was not in correct format"+e.Message);

                }
                if (action != null)
                {
                    action.Invoke(-1);
                }
            }
        }
    }
    void FetchPublicPlayersOnlineData(OnlinePlayerDetails onlinePlayerDetails, Action<int> action)
    {
        for (int i = 0; i < onlinePlayerDetails.data.Length; i++)
        {
            if (onlinePlayerDetails.data[i].total_amount == 0.01)
            {
                //PublicPlayerData[0] = Mathf.Max(Random.Range(30,50),onlinePlayerDetails.data[i].Activeuser);
                PublicPlayerData[0] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 0.05)
            {
                //PublicPlayerData[1] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PublicPlayerData[1] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 0.10)
            {
                //PublicPlayerData[2] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PublicPlayerData[2] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 0.25)
            {
                //PublicPlayerData[3] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PublicPlayerData[3] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 0.50)
            {
                //PublicPlayerData[4] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PublicPlayerData[4] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 1)
            {
                //PublicPlayerData[5] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PublicPlayerData[5] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 2)
            {
                //PublicPlayerData[6] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PublicPlayerData[6] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 3)
            {
                //PublicPlayerData[7] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PublicPlayerData[7] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 4)
            {
                //PublicPlayerData[8] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PublicPlayerData[8] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 5)
            {
                //PublicPlayerData[9] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PublicPlayerData[9] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 10)
            {
                //PublicPlayerData[10] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PublicPlayerData[10] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 20)
            {
                //PublicPlayerData[11] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PublicPlayerData[11] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            
        }
    }
    void FetchPrivatePlayersOnlineData(OnlinePlayerDetails onlinePlayerDetails, Action<int> action)
    {
        for (int i = 0; i < onlinePlayerDetails.data.Length; i++)
        {
            if (onlinePlayerDetails.data[i].total_amount == 0.01)
            {
                //PrivatePlayerData[0] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PrivatePlayerData[0] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 0.05)
            {
                //PrivatePlayerData[1] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PrivatePlayerData[1] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 0.10)
            {
                //PrivatePlayerData[2] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PrivatePlayerData[2] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 0.25)
            {
                //PrivatePlayerData[3] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PrivatePlayerData[3] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 0.50)
            {
                //PrivatePlayerData[4] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PrivatePlayerData[4] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 1)
            {
                //PrivatePlayerData[5] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PrivatePlayerData[5] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 2)
            {
                //PrivatePlayerData[6] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PrivatePlayerData[6] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 3)
            {
                //PrivatePlayerData[7] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PrivatePlayerData[7] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 4)
            {
                //PrivatePlayerData[8] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PrivatePlayerData[8] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 5)
            {
                //PrivatePlayerData[9] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PrivatePlayerData[9] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 10)
            {
                //PrivatePlayerData[10] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PrivatePlayerData[10] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 20)
            {
                ///PrivatePlayerData[11] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                PrivatePlayerData[11] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
           
        }
    }
    void FetchFreePlayersOnlineData(OnlinePlayerDetails onlinePlayerDetails, Action<int> action)
    {
        for (int i = 0; i < onlinePlayerDetails.data.Length; i++)
        {
            if (onlinePlayerDetails.data[i].total_amount == 0.01)
            {
                //FreePlayerData[0] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                FreePlayerData[0] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 0.05)
            {
                //FreePlayerData[1] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                FreePlayerData[1] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 0.10)
            {
                //FreePlayerData[2] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                FreePlayerData[2] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 0.25)
            {
                //FreePlayerData[3] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                FreePlayerData[3] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 0.50)
            {
                //FreePlayerData[4] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                FreePlayerData[4] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 1)
            {
                //FreePlayerData[5] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                FreePlayerData[5] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 2)
            {
                //FreePlayerData[6] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                FreePlayerData[6] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 3)
            {
                //FreePlayerData[7] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                FreePlayerData[7] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 4)
            {
                //FreePlayerData[8] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                FreePlayerData[8] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 5)
            {
                //FreePlayerData[9] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                FreePlayerData[9] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 10)
            {
                //FreePlayerData[10] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                FreePlayerData[10] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            else if (onlinePlayerDetails.data[i].total_amount == 20)
            {
               // FreePlayerData[11] = Mathf.Max(Random.Range(30, 50), onlinePlayerDetails.data[i].Activeuser);
                FreePlayerData[11] = Mathf.Max(0, onlinePlayerDetails.data[i].Activeuser);
            }
            
        }
    }
#endregion

#region UpdateRefferralCode
    public void UpdateRefferralCodeAPICall(string code, Action<ServerReferCodeResponse> _callback)
    {
        UpdateRefferCode updateRefferCode = new UpdateRefferCode();
        updateRefferCode.mobile = StaticValues.FirebaseUserId;
        updateRefferCode.referral_codeby = code;
        updateRefferCode.second = "Update";
        UpdateRefferralCodeCall(updateRefferCode, _callback);
    }
    public void UpdateRefferralCodeCall(UpdateRefferCode _updateRefferCode, Action<ServerReferCodeResponse> _callback)
    {
        var jsonString = JsonUtility.ToJson(_updateRefferCode) ?? "";
        StartCoroutine(UpdateRefferralCodeRequest(BaseAPI + "" + UpdateRefferCodeAPI, jsonString.ToString(), _callback));
    }


    IEnumerator UpdateRefferralCodeRequest(string url, string json, Action<ServerReferCodeResponse> _callback)
    {
        if (debug)
        {
            Debug.Log("json in UpdateReferWalletRequest " + json);
        }
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Error While Sending: " + uwr.error);

            }

            if (_callback != null)
            {
                ServerReferCodeResponse _serverReferCodeResponse = new ServerReferCodeResponse();
                _serverReferCodeResponse.status = "500";
                if (_callback != null)
                {
                    _callback.Invoke(_serverReferCodeResponse);
                }
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
            try
            {
                string result = uwr.downloadHandler.text;

                serverUpdateRefferCodeResponse = JsonUtility.FromJson<ServerReferCodeResponse>(result.ToString());

                if (serverUpdateRefferCodeResponse != null)
                {
                    if (debug)
                    {
                        Debug.Log("serverUpdateRefferCodeResponse.status  " + serverUpdateRefferCodeResponse.status);
                    }
                    string status = serverUpdateRefferCodeResponse.status;
                    if (_callback != null)
                    {
                        _callback.Invoke(serverUpdateRefferCodeResponse);
                    }
                    if (debug)
                    {
                        Debug.Log("status   " + status);
                    }
                    if (status.Contains("200"))
                    {
                        if (debug)
                        {
                            Debug.Log("Successfully Update users ");
                        }
                    }
                    else if (status.Contains("404"))
                    {
                        if (debug)
                        {
                            Debug.Log("Referral already exists! ");
                        }
                    }
                }
                else
                {
                    if (_callback != null)
                    {
                        ServerReferCodeResponse _serverReferCodeResponse = new ServerReferCodeResponse();
                        _serverReferCodeResponse.status = "500";
                        _callback.Invoke(_serverReferCodeResponse);
                    }
                }
                if (debug)
                {
                    Debug.Log("hello  in serverUpdateRefferCodeResponse ");
                }

            }
            catch
            {
                if (debug)
                {
                    Debug.LogWarning("File was not in correct format");

                }

                if (_callback != null)
                {
                    ServerReferCodeResponse _serverReferCodeResponse = new ServerReferCodeResponse();
                    _serverReferCodeResponse.status = "500";
                    _callback.Invoke(_serverReferCodeResponse);
                }
            }
        }
    }
#endregion

#region GetBanner


    private IEnumerator OnReferLoadGraphic(GetReferImageDetail getReferImageDetail)
    {
        string _url = BaseAPI +"" + getReferImageDetail.Url;
        if (!string.IsNullOrEmpty(_url) && (_url.StartsWith("http") || _url.StartsWith("file")))
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(_url);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D _texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                if (_texture != null)
                {
                    ReferImage = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                }
            }
        }
    }
    public void GeBannerDetails()
    {
        GetBannerAPIUrl = BaseAPI + "" + GetBannerAPI;
        if (GetBannerAPIUrl != "" && (GetBannerAPIUrl.StartsWith("http") || GetBannerAPIUrl.StartsWith("file")))
        {
            StopCoroutine(GetBannerRequest(GetBannerAPIUrl));
            StartCoroutine(GetBannerRequest(GetBannerAPIUrl));
        }
    }
    public IEnumerator GetBannerRequest(string url, bool debug)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            if (debug)
            {
                Debug.LogWarning("Could not download config file " + www.error);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("www getuserDetails" + www.downloadHandler.text);
            }
            _getBannerResponse = www.downloadHandler.text;
        }
    }
    private IEnumerator GetBannerRequest(string url)
    {
        if (debug)
        {
            Debug.Log("URL: " + url);
        }
        StopCoroutine(GetBannerRequest(url, debug));
        yield return StartCoroutine(GetBannerRequest(url, debug));

        try
        {
            string result = _getBannerResponse;

            if (debug)
            {
                Debug.Log("result " + result);
            }
            GetBannerResponse _getBannerResponseDetails = JsonUtility.FromJson<GetBannerResponse>(result.ToString());

            if (_getBannerResponseDetails != null)
            {
                if (_getBannerResponseDetails.status == 200)
                {
                    if (_getBannerResponseDetails.data != null)
                    {
                        if (_getBannerResponseDetails.data.Length > 0)
                        {
                            if (debug)
                            {
                                Debug.Log("Server user  getBannerResponseDetails  " + _getBannerResponseDetails.data[0].benner_source);
                            }
                            if (StaticValues.getBannerImageDetails != null)
                            {
                                StaticValues.getBannerImageDetails.Clear();
                                StaticValues.getBannerImageDetails.TrimExcess();

                                for (int i = 0; i < _getBannerResponseDetails.data.Length; i++)
                                {
                                    StaticValues.getBannerImageDetails.Add(_getBannerResponseDetails.data[i]);
                                }

                            }
                        }
                        else
                        {
                            if (StaticValues.getBannerImageDetails != null)
                            {
                                StaticValues.getBannerImageDetails.Clear();
                                StaticValues.getBannerImageDetails.TrimExcess();
                            }
                        }

                        
                    }

                    if (_getBannerResponseDetails.data2 != null)
                    {
                       
                            if (debug)
                            {
                                Debug.Log("Server user  getBannerResponseDetails2  " + _getBannerResponseDetails.data2.Url);
                            }
                            StartCoroutine(OnReferLoadGraphic(_getBannerResponseDetails.data2));
                        
                    }

                    
                        StaticValues.LastDeposit = _getBannerResponseDetails.Last_Diposit;
                    
                }
                else
                {
                    if (StaticValues.getBannerImageDetails != null)
                    {
                        StaticValues.getBannerImageDetails.Clear();
                        StaticValues.getBannerImageDetails.TrimExcess();
                    }
                    StaticValues.LastDeposit = 0;
                }
            }
            if (debug)
            {
                Debug.Log("hello   getBannerResponseDetails status  " + _getBannerResponseDetails.status);
            }
        }
        catch
        {
            if (debug)
            {
                Debug.LogWarning("File was not in correct format");

            }

        }

    }


    public void GetBannerDetails(string uniqueId,Action<GetBannerResponse> _callback)
    {
        GetBannerAPIUrl2 = BaseAPI + ""+ GetBannerAPI2+ ""+ uniqueId;

        //Debug.Log("GetBannerAPIUrl2 " + GetBannerAPIUrl2);
        if (GetBannerAPIUrl2 != "" && (GetBannerAPIUrl2.StartsWith("http") || GetBannerAPIUrl2.StartsWith("file")))
        {
            StopCoroutine(GetBannerRequest(GetBannerAPIUrl2,_callback));
            StartCoroutine(GetBannerRequest(GetBannerAPIUrl2, _callback));
        }
    }
    public IEnumerator GetBannerRequest(string url, bool debug, Action<GetBannerResponse> _callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            if (debug)
            {
                Debug.LogWarning("Could not download config file " + www.error);
            }
            GetBannerResponse getBannerResponse = new GetBannerResponse();
            getBannerResponse.status = 500;
            if(_callback!=null)
            {
                _callback.Invoke(getBannerResponse);
            }
            if (StaticValues.getBannerImageDetails != null)
            {
                StaticValues.getBannerImageDetails.Clear();
                StaticValues.getBannerImageDetails.TrimExcess();
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("www getuserDetails" + www.downloadHandler.text);
            }
            _getBannerResponse = www.downloadHandler.text;
        }
    }
    private IEnumerator GetBannerRequest(string url, Action<GetBannerResponse> _callback)
    {
        if (debug)
        {
            Debug.Log("URL: " + url);
        }
        StopCoroutine(GetBannerRequest(url, debug,_callback));
        yield return StartCoroutine(GetBannerRequest(url, debug,_callback));

        try
        {
            string result = _getBannerResponse;

            if (debug)
            {
                Debug.Log("result " + result);
            }
            GetBannerResponse _getBannerResponseDetails = JsonUtility.FromJson<GetBannerResponse>(result.ToString());

            if (_getBannerResponseDetails != null)
            {
                if (_getBannerResponseDetails.status == 200)
                {
                    if (_getBannerResponseDetails.data != null)
                    {
                        if (_getBannerResponseDetails.data.Length > 0)
                        {
                            if (debug)
                            {
                                Debug.Log("Server user  getBannerResponseDetails  " + _getBannerResponseDetails.data[0].benner_source);
                            }
                            if (StaticValues.getBannerImageDetails != null)
                            {
                                if (debug)
                                {
                                    Debug.Log("in if condition ");
                                }
                                StaticValues.getBannerImageDetails.Clear();
                                StaticValues.getBannerImageDetails.TrimExcess();

                                for (int i = 0; i < _getBannerResponseDetails.data.Length; i++)
                                {
                                    StaticValues.getBannerImageDetails.Add(_getBannerResponseDetails.data[i]);
                                    StartCoroutine(OnOfferImageLoadGraphic(StaticValues.getBannerImageDetails[i].extralbanner_url,i));
                                }
                            }
                            else
                            {
                                if (debug)
                                {
                                    Debug.Log("in else condition ");
                                }
                            }
                        }
                        else
                        {
                            if (StaticValues.getBannerImageDetails != null)
                            {
                                StaticValues.getBannerImageDetails.Clear();
                                StaticValues.getBannerImageDetails.TrimExcess();

                               
                            }
                        }

                    }

                    if (_getBannerResponseDetails.data2 != null)
                    {

                        if (debug)
                        {
                            Debug.Log("Server user  getBannerResponseDetails2  " + _getBannerResponseDetails.data2.Url);
                        }
                        StartCoroutine(OnReferLoadGraphic(_getBannerResponseDetails.data2));

                    }

                   
                    StaticValues.LastDeposit =  _getBannerResponseDetails.Last_Diposit;
                    
                }
                else
                {
                    if (StaticValues.getBannerImageDetails != null)
                    {
                        StaticValues.getBannerImageDetails.Clear();
                        StaticValues.getBannerImageDetails.TrimExcess();

                       
                    }
                    StaticValues.LastDeposit = 0;
                }


               
                if (_callback != null)
                {
                    _callback.Invoke(_getBannerResponseDetails);
                }
            }
            else
            {
                GetBannerResponse getBannerResponse = new GetBannerResponse();
                getBannerResponse.status = 500;
                if (_callback != null)
                {
                    _callback.Invoke(getBannerResponse);
                }
                if (StaticValues.getBannerImageDetails != null)
                {
                    StaticValues.getBannerImageDetails.Clear();
                    StaticValues.getBannerImageDetails.TrimExcess();
                }
            }
            if (debug)
            {
                Debug.Log("hello   getBannerResponseDetails status  " + _getBannerResponseDetails.status);
            }
        }
        catch
        {
            if (debug)
            {
                Debug.LogWarning("File was not in correct format");

            }
            GetBannerResponse getBannerResponse = new GetBannerResponse();
            getBannerResponse.status = 500;
            if (_callback != null)
            {
                _callback.Invoke(getBannerResponse);
            }
            if (StaticValues.getBannerImageDetails != null)
            {
                StaticValues.getBannerImageDetails.Clear();
                StaticValues.getBannerImageDetails.TrimExcess();

                
            }
        }

    }
    private IEnumerator OnOfferImageLoadGraphic(string _url2,int _i)
    {
        string _url = PlayerSave.singleton.BaseAPI + "" + _url2;
        if (!string.IsNullOrEmpty(_url) && (_url.StartsWith("http") || _url.StartsWith("file")))
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(_url);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                
                Debug.Log(www.error);
            }
            else
            {
                Texture2D _texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                if (_texture != null)
                {
                    if (StaticValues.getBannerImageDetails != null)
                    {
                        if(StaticValues.getBannerImageDetails.Count > _i)
                        {
                            StaticValues.getBannerImageDetails[_i].sprite1 = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                        }
                    }
                  
                }
            }
        }
    }


#endregion

#region UpdateProfile
    public void UpdateProfileAPICall(string _userId, string _FirstName,string _LastName,string _gender,string _mobile2,string _email,string _dob,string _street1,string _street2,string _city,string _state, string _pincode,Action<ServerUserDetailsResponse> _callBack)
    {
        UserInfo updateProfileUser = new UserInfo();
        updateProfileUser.mobile = _userId;
        updateProfileUser.FirstName = _FirstName;
        updateProfileUser.LastName = _LastName;
        updateProfileUser.gender = _gender;
        updateProfileUser.mobile2 = _mobile2;
        updateProfileUser.email = _email;
        updateProfileUser.dob = _dob;
        updateProfileUser.street1 = _street1;
        updateProfileUser.street2 = _street2;
        updateProfileUser.city = _city;
        updateProfileUser.state = _state;
        updateProfileUser.zip = _pincode;
        UpdateProfileUser(updateProfileUser, _callBack);
    }
    public void UpdateAvatarAPICall(string _userId, string _avatar, Action<ServerUserDetailsResponse> _callBack)
    {
        UserInfo updateProfileUser = new UserInfo();
        updateProfileUser.mobile = _userId;
        updateProfileUser.image_str = _avatar;
        if (!string.IsNullOrEmpty(_avatar) && (!_avatar.StartsWith("http") || !_avatar.StartsWith("file")))
        {
            if (_avatar.Length >= 200)
            {
                UpdateProfileUser(updateProfileUser, _callBack);
            }
            else
            {
                AppManager.VIEW_CONTROLLER.HideLoading();
                //ShowErrorMessage("Avatar Pic Updated!!!");
            }
        }
        else
        {
            AppManager.VIEW_CONTROLLER.HideLoading();
            //ShowErrorMessage("Avatar Pic Updated!!!");
        }
    }

    public void UpdateProfileUser(UserInfo updateProfileUser, Action<ServerUserDetailsResponse> _callBack)
    {
        var jsonString = JsonUtility.ToJson(updateProfileUser) ?? "";
        StartCoroutine(UpdateProfileUserRequest(BaseAPI + "" + UpdateProfileAPI, jsonString.ToString(), _callBack));
    }
    IEnumerator UpdateProfileUserRequest(string url, string json, Action<ServerUserDetailsResponse> _callback)
    {
        if (debug)
        {
            Debug.Log("json in UpdateProfile" + url + " " + json);
        }
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
            ServerUserDetailsResponse _serverUDetailsResponse = new ServerUserDetailsResponse();
            _serverUDetailsResponse.status = "500";
            _serverUDetailsResponse.message = uwr.error;
            if (_callback != null)
            {
                _callback.Invoke(_serverUDetailsResponse);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
            try
            {
                string result = uwr.downloadHandler.text;

                ServerUserDetailsResponse serverUserDetailsResponse = JsonUtility.FromJson<ServerUserDetailsResponse>(result.ToString());

                if (serverUserDetailsResponse != null)
                {
                    if (debug)
                    {
                        Debug.Log("serverUserDetailsResponse.status  " + serverUserDetailsResponse.status);
                    }
                    string status = serverUserDetailsResponse.status;
                    if (debug)
                    {
                        Debug.Log("status   " + status);
                    }
                    if (status.Contains("200"))
                    {

                        if (debug)
                        {
                            Debug.Log("Update users! ");
                        }
                        if (serverUserDetailsResponse.data != null)
                        {

                            //debug = serverUserDetailsResponse.data.debuger;
                            if (!string.IsNullOrEmpty(serverUserDetailsResponse.data.username))
                            {
                                StaticValues.UserNameValue = serverUserDetailsResponse.data.username;
                            }
                            StaticValues.FirstNameValue = serverUserDetailsResponse.data.FirstName;
                            StaticValues.LastNameValue = serverUserDetailsResponse.data.LastName;
                            StaticValues.GenderValue = serverUserDetailsResponse.data.gender;
                            StaticValues.MobileValue = serverUserDetailsResponse.data.mobile2;
                            StaticValues.Email = serverUserDetailsResponse.data.email;
                            StaticValues.DOBValue = serverUserDetailsResponse.data.dob;
                            StaticValues.StreetValue_1 = serverUserDetailsResponse.data.street1;
                            StaticValues.StreetValue_2 = serverUserDetailsResponse.data.street2;
                            StaticValues.CityValue = serverUserDetailsResponse.data.city;
                            StaticValues.StateValue = serverUserDetailsResponse.data.state;
                            StaticValues.PinCodeValue = serverUserDetailsResponse.data.zip;
                            StaticValues.BankAccountNo = serverUserDetailsResponse.data.Account_no;
                            StaticValues.BankIFSCCode = serverUserDetailsResponse.data.back_ifsc_code;
                            StaticValues.AddressNo = serverUserDetailsResponse.data.addressProofno;
                            StaticValues.PanDocNo = serverUserDetailsResponse.data.PancardNo;
                            StaticValues.PanCardStatus = serverUserDetailsResponse.data.pancardstatus;
                            StaticValues.AddressStatus = serverUserDetailsResponse.data.addressp_status;
                            StaticValues.isbotStatus = serverUserDetailsResponse.data.isbotstatus;
                            StaticValues.BotHand = serverUserDetailsResponse.data.botthreshold;
                            StaticValues.ismaintenance = serverUserDetailsResponse.data.ismaintenance;
                            StaticValues.FirebaseUserId = serverUserDetailsResponse.data.mobile;
                            if (!string.IsNullOrEmpty(serverUserDetailsResponse.data.otp_ref_Id))
                            {
                                StaticValues.mobileVerificationId = serverUserDetailsResponse.data.otp_ref_Id;
                            }
                            if (!string.IsNullOrEmpty(serverUserDetailsResponse.data.referral_code))
                            {
                                StaticValues.MyReferralCode = serverUserDetailsResponse.data.referral_code;
                            }
                            if (!string.IsNullOrEmpty(serverUserDetailsResponse.data.avatar_url))
                            {
                                if (serverUserDetailsResponse.data.avatar_url.Length <= 200)
                                {
                                    StaticValues.avatarPicUrl = BaseAPI +  serverUserDetailsResponse.data.avatar_url;
                                    PlayerSave.singleton.SavePic(StaticValues.avatarPicUrl);
                                }
                                else
                                {
                                    StaticValues.avatarPicUrl = serverUserDetailsResponse.data.avatar_url;
                                    PlayerSave.singleton.SavePic(StaticValues.avatarPicUrl);
                                }

                            }
                            StaticValues.displayName = StaticValues.FirstNameValue + " " + StaticValues.LastNameValue;
                            StaticValues.displayNameinUC = StaticValues.displayName.ToUpperInvariant();
                            StaticValues.phoneNumberWithoutPrefix = StaticValues.MobileValue;
                            if (PlayerSave.singleton != null)
                            {
                                PlayerSave.singleton.SaveUserName(StaticValues.UserNameValue);
                                PlayerSave.singleton.SaveNewName(StaticValues.displayName);
                                PlayerSave.singleton.SaveMobileId(StaticValues.phoneNumberWithoutPrefix);
                                PlayerSave.singleton.SaveEmail(StaticValues.Email);
                                PlayerSave.singleton.SaveUserId(StaticValues.FirebaseUserId);
                                PlayerSave.singleton.SaveDistributionId(StaticValues.MyReferralCode);
                                PlayerSave.singleton.SaveGender(StaticValues.GenderValue);
                                PlayerSave.singleton.SavePassword(StaticValues.FirebaseUserId);
                                PlayerPrefs.SetString(AppSettings.LoginSaveKey, StaticValues.FirebaseUserId);
                                PlayerPrefs.SetString(AppSettings.PasswordSaveKey, StaticValues.mobileVerificationId);
                            }
                           
                            if (ReporterObject)
                            {
                                ReporterObject.SetActive(debug);
                            }
                            StaticValues.isEmailVerify = serverUserDetailsResponse.data.isemailverify;
                            StaticValues.isMobileVerify = serverUserDetailsResponse.data.ismobile_verify;
                            StaticValues.BankAccountNo_NR = serverUserDetailsResponse.data.NewAccount_no;
                            StaticValues.BankIFSCCode_NR = serverUserDetailsResponse.data.Newback_ifsc_code;
                            StaticValues.isBankDetailsSubmitted = serverUserDetailsResponse.data.isBankDetailsSubmitted;
                            StaticValues.isBankStatusForNewRequest = serverUserDetailsResponse.data.isBankStatusForNewRequest;

                            StaticValues.BankUPIId = serverUserDetailsResponse.data.BankUPIId;
                            StaticValues.BankUPIId_NR = serverUserDetailsResponse.data.BankUPIId_NR;
                            StaticValues.isBankUPIStatusForNewRequest = serverUserDetailsResponse.data.BankUPIId_status;
                            StaticValues.isBankUPIDetailsSubmitted = serverUserDetailsResponse.data.isUPIBankDetailsSubmitted;
                        }

                    }
                    
                    if (_callback != null)
                    {
                        _callback.Invoke(serverUserDetailsResponse);
                    }
                }
                else
                {
                    ServerUserDetailsResponse _serverUDetailsResponse = new ServerUserDetailsResponse();
                    _serverUDetailsResponse.status = "500";
                    _serverUDetailsResponse.message = "Parsing Error!!!";
                    if (_callback != null)
                    {
                        _callback.Invoke(_serverUDetailsResponse);
                    }
                }
                if (debug)
                {
                    Debug.Log("hello   ResponseHandling ");
                }
            }
            catch(Exception e)
            {
                if (debug)
                {
                    Debug.LogWarning("File was not in correct format");

                }
                ServerUserDetailsResponse serverUserDetailsResponse = new ServerUserDetailsResponse();
                serverUserDetailsResponse.status = "500";
                serverUserDetailsResponse.message = e.Message;
                if (_callback != null)
                {
                    _callback.Invoke(serverUserDetailsResponse);
                }
            }
        }

    }
#endregion

#region UpdateBankInfo
    public void UpdateBankInfoCall(string _userId, string Account_no, string back_ifsc_code, string back_name,string option, Action<ServerBankDetailsResponse> _callBack)
    {
        ServerBankDetailsRequest updateProfileUser = new ServerBankDetailsRequest();
        updateProfileUser.mobile = _userId;
       
        if (option == "1")
        {
            updateProfileUser.back_ifsc_code = back_ifsc_code;
            updateProfileUser.back_name = back_name;
            updateProfileUser.Account_no = Account_no;
            updateProfileUser.BankUPIId = "";
            updateProfileUser.BankUPIId_NR = "";
        }
        else if(option=="2")
        {
            updateProfileUser.Newback_ifsc_code = back_ifsc_code;
            updateProfileUser.NewAccount_no = Account_no;
            updateProfileUser.back_name = "";
            updateProfileUser.BankUPIId = "";
            updateProfileUser.BankUPIId_NR = "";
        }
        else if(option == "11")
        {
           
            updateProfileUser.BankUPIId = Account_no;
            updateProfileUser.BankUPIId_NR = "";
        }
        else if(option == "12")
        {
            updateProfileUser.BankUPIId = "";
            updateProfileUser.BankUPIId_NR = Account_no;
        }
        updateProfileUser.option = option;
        UpdateBankInfoUser(updateProfileUser, _callBack);
    }
    public void UpdateBankInfoUser(ServerBankDetailsRequest updateProfileUser, Action<ServerBankDetailsResponse> _callBack)
    {
        var jsonString = JsonUtility.ToJson(updateProfileUser) ?? "";
        StartCoroutine(UpdateBankInfoRequest(BaseAPI + "" + UpdateBankAccountAPI, jsonString.ToString(), _callBack));
    }
    IEnumerator UpdateBankInfoRequest(string url, string json, Action<ServerBankDetailsResponse> _callback)
    {
        if (debug)
        {
            Debug.Log("json in BankInfo" + url + " " + json);
        }
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
            ServerBankDetailsResponse _serverUDetailsResponse = new ServerBankDetailsResponse();
            _serverUDetailsResponse.status = "500";
            _serverUDetailsResponse.message = uwr.error;
            if (_callback != null)
            {
                _callback.Invoke(_serverUDetailsResponse);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
            try
            {
                string result = uwr.downloadHandler.text;

                ServerBankDetailsResponse serverUserDetailsResponse = JsonUtility.FromJson<ServerBankDetailsResponse>(result.ToString());

                if (serverUserDetailsResponse != null)
                {
                    if (debug)
                    {
                        Debug.Log("serverUserDetailsResponse.status  " + serverUserDetailsResponse.status);
                    }
                    
                    

                    if (_callback != null)
                    {
                        _callback.Invoke(serverUserDetailsResponse);
                    }
                }
                else
                {
                    ServerBankDetailsResponse _serverUDetailsResponse = new ServerBankDetailsResponse();
                    _serverUDetailsResponse.status = "500";
                    _serverUDetailsResponse.message = "Parsing Error!!!";
                    if (_callback != null)
                    {
                        _callback.Invoke(_serverUDetailsResponse);
                    }
                }
                if (debug)
                {
                    Debug.Log("hello   ResponseHandling ");
                }
            }
            catch (Exception e)
            {
                if (debug)
                {
                    Debug.LogWarning("File was not in correct format");

                }
                ServerBankDetailsResponse serverUserDetailsResponse = new ServerBankDetailsResponse();
                serverUserDetailsResponse.status = "500";
                serverUserDetailsResponse.message = e.Message;
                if (_callback != null)
                {
                    _callback.Invoke(serverUserDetailsResponse);
                }
            }
        }

    }
#endregion

#region UpdatKYCInfo
    public void UpdateKYCInfoCall(string _userId, string PancardNo, string add_proof_type, string addressProofno,string pan_doc,string address_Proof_doc, Action<ServerKYCDetailsResponse> _callBack)
    {
        ServerKYCDetailsRequest updateProfileUser = new ServerKYCDetailsRequest();
        updateProfileUser.mobile = _userId;
        updateProfileUser.PancardNo = PancardNo;
        updateProfileUser.pan_doc = pan_doc;
        updateProfileUser.add_proof_type = add_proof_type;
        updateProfileUser.addressProofno = addressProofno;
        updateProfileUser.address_Proof_doc = address_Proof_doc;
        UpdateKYCInfoUser(updateProfileUser, _callBack);
    }
    public void UpdateKYCInfoUser(ServerKYCDetailsRequest updateProfileUser, Action<ServerKYCDetailsResponse> _callBack)
    {
        var jsonString = JsonUtility.ToJson(updateProfileUser) ?? "";
        StartCoroutine(UpdateKYCInfoRequest(BaseAPI + "" + UpdateKYCAPI, jsonString.ToString(), _callBack));
    }
    IEnumerator UpdateKYCInfoRequest(string url, string json, Action<ServerKYCDetailsResponse> _callback)
    {
        if (debug)
        {
            Debug.Log("json in UpdateKYCInfoRequest" + url + " " + json);
        }
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
            ServerKYCDetailsResponse _serverUDetailsResponse = new ServerKYCDetailsResponse();
            _serverUDetailsResponse.status = "500";
            _serverUDetailsResponse.message = uwr.error;
            if (_callback != null)
            {
                _callback.Invoke(_serverUDetailsResponse);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
            try
            {
                string result = uwr.downloadHandler.text;

                ServerKYCDetailsResponse serverUserDetailsResponse = JsonUtility.FromJson<ServerKYCDetailsResponse>(result.ToString());

                if (serverUserDetailsResponse != null)
                {
                    if (debug)
                    {
                        Debug.Log("serverUserDetailsResponse.status  " + serverUserDetailsResponse.status);
                    }



                    if (_callback != null)
                    {
                        _callback.Invoke(serverUserDetailsResponse);
                    }
                }
                else
                {
                    ServerKYCDetailsResponse _serverUDetailsResponse = new ServerKYCDetailsResponse();
                    _serverUDetailsResponse.status = "500";
                    _serverUDetailsResponse.message = "Parsing Error!!!";
                    if (_callback != null)
                    {
                        _callback.Invoke(_serverUDetailsResponse);
                    }
                }
                if (debug)
                {
                    Debug.Log("hello   ResponseHandling ");
                }
            }
            catch (Exception e)
            {
                if (debug)
                {
                    Debug.LogWarning("File was not in correct format");

                }
                ServerKYCDetailsResponse serverUserDetailsResponse = new ServerKYCDetailsResponse();
                serverUserDetailsResponse.status = "500";
                serverUserDetailsResponse.message = e.Message;
                if (_callback != null)
                {
                    _callback.Invoke(serverUserDetailsResponse);
                }
            }
        }

    }
#endregion

#region SavedCards
    public void CallSavedCardsForBotOnly(string newId,string _FullRoomName, string _Card_1, string _Card_2, string _Card_3, string _Priority, string _BotType, string GameRoom_2, int Rand_next, double _bootAmount, string _NewGeneratedId,string _start_chal)
    {
        
        if (!string.IsNullOrEmpty(_FullRoomName))
        {
            GameSaveCardsAPICall(_FullRoomName, newId, _Card_1, _Card_2, _Card_3, _Priority, _BotType, GameRoom_2, Rand_next, _bootAmount, _NewGeneratedId, _start_chal);

        }
        else
        {
            GameSaveCardsAPICall(_FullRoomName, newId, _Card_1, _Card_2, _Card_3, _Priority, _BotType, GameRoom_2, Rand_next, _bootAmount, _NewGeneratedId, _start_chal);
        }
    }
    public void CallSavedCards(string _FullRoomName, string _Card_1, string _Card_2, string _Card_3, string _Priority,string _BotType,string GameRoom_2,int Rand_next,double _bootAmount, string _NewGeneratedId, string _start_chal)
    {
        string newId = newID();
        if (!string.IsNullOrEmpty(_FullRoomName))
        {
            GameSaveCardsAPICall(_FullRoomName, newId, _Card_1, _Card_2, _Card_3, _Priority, _BotType, GameRoom_2, Rand_next, _bootAmount, _NewGeneratedId, _start_chal);

        }
        else
        {
            GameSaveCardsAPICall(_FullRoomName, newId, _Card_1, _Card_2, _Card_3, _Priority, _BotType,GameRoom_2, Rand_next, _bootAmount, _NewGeneratedId, _start_chal);
        }
    }
    public void GameSaveCardsAPICall(string game_room, string mobileNumber, string _Card_1, string _Card_2, string _Card_3, string _Priority, string _BotType, string GameRoom_2, int Rand_next, double _bootAmount, string _NewGeneratedId, string _start_chal)
    {
        SavedCards _savedCards = new SavedCards();
        _savedCards.mobile = mobileNumber;
        _savedCards.GameName = currentTable.ToString();
        _savedCards.GameId = game_room;
        _savedCards.Card1 = _Card_1;
        _savedCards.Card2 = _Card_2;
        _savedCards.Card3 = _Card_3;
        _savedCards.Priority = _Priority;
        _savedCards.BotType = _BotType;
        _savedCards.GameRoom_2 = GameRoom_2;
        _savedCards.Rand_next = Rand_next;
        _savedCards.amount = _bootAmount;
        _savedCards.NewGeneratedId = _NewGeneratedId;
        if (mobileNumber == _start_chal)
        {
            _savedCards.start_chal = "D";
        }
        else
        {
            _savedCards.start_chal = "";
        }
        GameSaveCardsAPICallDetails(_savedCards);
    }

    public void GameSaveCardsAPICallDetails(SavedCards _savedCards)
    {

        var jsonString = JsonUtility.ToJson(_savedCards) ?? "";
        StartCoroutine(GameSaveCardsAPICallDetailsRequest(BaseAPI + "" + SavedCardsAPI, jsonString.ToString()));
    }


    IEnumerator GameSaveCardsAPICallDetailsRequest(string url, string json)
    {
        if (debug)
        {
            //Debug.Log("json in GameSaveCardsAPICallDetailsRequest " + json);
        }
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
                //Debug.Log("Error While Sending: " + uwr.error);
            }
        }
        else
        {
            if (debug)
            {
                // Debug.Log("Received: " + uwr.downloadHandler.text);
            }
            
        }
    }
#endregion

#region CheckBonusCode
    public void CallBonusCode(BonusCodeRequest _bonusCodeRequest, Action<BonusCodeResult> _callback)
    {
        //CheckBonusCodeAPIUrl = CheckBonusCodeAPI + _bonusCode.ToString() + "&&amount="+amount;
        //if (CheckBonusCodeAPIUrl != "" && (CheckBonusCodeAPIUrl.StartsWith("http") || CheckBonusCodeAPIUrl.StartsWith("file")))
        //{
        //    StopCoroutine(BonusCodeRequest(CheckBonusCodeAPIUrl, _bonusCode, _callback));
        //    StartCoroutine(BonusCodeRequest(CheckBonusCodeAPIUrl, _bonusCode, _callback));
        //}
        var jsonString = JsonUtility.ToJson(_bonusCodeRequest) ?? "";
        StartCoroutine(BonusCodeRequest(BaseAPI + "" + CheckBonusCodeAPI, jsonString.ToString(), _callback));
    }
    public IEnumerator BonusCodeRequest(string url,string json, Action<BonusCodeResult> _callback)
    {
        var www = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            if (debug)
            {
                Debug.LogWarning("Could not download config file " + www.error);
            }
            BonusCodeResult _bonusCodeResult = new BonusCodeResult();
            _bonusCodeResult.status = "500";
            _bonusCodeResult.message = www.error;
            if (_callback != null)
            {
                _callback.Invoke(_bonusCodeResult);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("www BonusCodeRequest" + www.downloadHandler.text);
            }
            try
            {
                string result = www.downloadHandler.text;

                if (debug)
                {
                    Debug.Log("result " + result);
                }
                BonusCodeResult _bonusCodeResult = JsonUtility.FromJson<BonusCodeResult>(result.ToString());

                if (_bonusCodeResult != null)
                {
                    if (debug)
                    {
                        Debug.Log("Server user  BonusCodeRequest  " + _bonusCodeResult.status);
                    }

                    if (_callback != null)
                    {
                        _callback.Invoke(_bonusCodeResult);
                    }

                }
                else
                {
                    BonusCodeResult bonusCodeResult = new BonusCodeResult();
                    bonusCodeResult.status = "500";
                    bonusCodeResult.message = "Format Error";
                    if (_callback != null)
                    {
                        _callback.Invoke(bonusCodeResult);
                    }
                }
                
            }
            catch(Exception e)
            {
                if (debug)
                {
                    Debug.LogWarning("File was not in correct format");

                }
                BonusCodeResult _bonusCodeResult = new BonusCodeResult();
                _bonusCodeResult.status = "500";
                _bonusCodeResult.message = e.Message;
                if (_callback != null)
                {
                    _callback.Invoke(_bonusCodeResult);
                }
            }
        }
    }

#endregion

#region WithdrawDetails
    //-------------------------------------------------- 
    //---------------------withdraw details-----------

    public void GetWithdrawDetails(string phoneNumberWithoutPrefix, string pageIndex, string pageSize, Action<GetWithdrawDetails> _callback)
    {
        if (debug)
        {
            Debug.Log("GetWithdraw");
        }
        GetWithdrawDetailsAPIUrl = BaseAPI + "" + GetWithdrawDetailsAPI + phoneNumberWithoutPrefix.ToString() + "&&pageIndex=" + pageIndex.ToString() + "&&PageS=" + pageSize.ToString();
        if (GetWithdrawDetailsAPIUrl != "" && (GetWithdrawDetailsAPIUrl.StartsWith("http") || GetWithdrawDetailsAPIUrl.StartsWith("file")))
        {
            StopCoroutine(GetWithdrawDetailsRequest(GetWithdrawDetailsAPIUrl, phoneNumberWithoutPrefix, _callback));
            StartCoroutine(GetWithdrawDetailsRequest(GetWithdrawDetailsAPIUrl, phoneNumberWithoutPrefix, _callback));
        }
    }
    public void GetWithdrawDetails(string phoneNumberWithoutPrefix, Action<GetWithdrawDetails> _callback)
    {
        if (debug)
        {
            Debug.Log("GetPaymant");
        }
        GetWithdrawDetailsAPIUrl = BaseAPI + "" + GetWithdrawDetailsAPI + phoneNumberWithoutPrefix.ToString();
        if (GetWithdrawDetailsAPIUrl != "" && (GetWithdrawDetailsAPIUrl.StartsWith("http") || GetWithdrawDetailsAPIUrl.StartsWith("file")))
        {
            StopCoroutine(GetWithdrawDetailsRequest(GetWithdrawDetailsAPIUrl, phoneNumberWithoutPrefix, _callback));
            StartCoroutine(GetWithdrawDetailsRequest(GetWithdrawDetailsAPIUrl, phoneNumberWithoutPrefix, _callback));
        }
    }
    public IEnumerator GetWithdrawDetailsRequest(string url, bool debug, string phoneNumberWithoutPrefix, Action<GetWithdrawDetails> _callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);


        yield return www.SendWebRequest();


        if (www.isNetworkError || www.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Could not download config file " + www.error);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("www getPaymentDetails" + www.downloadHandler.text);
            }
            _serverGetWithdrawDetailsResponse = www.downloadHandler.text;
        }
    }
    private IEnumerator GetWithdrawDetailsRequest(string url, string phoneNumberWithoutPrefix, Action<GetWithdrawDetails> _callback)
    {
        if (debug)
        {
            Debug.Log("URL: " + url);
        }
        StopCoroutine(GetWithdrawDetailsRequest(url, debug, phoneNumberWithoutPrefix, _callback));
        yield return StartCoroutine(GetWithdrawDetailsRequest(url, debug, phoneNumberWithoutPrefix, _callback));

        try
        {
            string result = _serverGetWithdrawDetailsResponse;

            GetWithdrawDetails _withdrawDetails = JsonUtility.FromJson<GetWithdrawDetails>(result.ToString());

            if (_withdrawDetails != null)
            {
                if (_callback != null)
                {
                    _callback.Invoke(_withdrawDetails);
                }

            }
            else
            {
                GetWithdrawDetails withdrawDetails = new GetWithdrawDetails();
                withdrawDetails.status = "500";
                if (_callback != null)
                {
                    _callback.Invoke(withdrawDetails);
                }
            }
            
        }
        catch
        {
            if (debug)
            {
                Debug.Log("File was not in correct format");

            }
            GetWithdrawDetails _withdrawDetails = new GetWithdrawDetails();
            _withdrawDetails.status = "500";
            if (_callback != null)
            {
                _callback.Invoke(_withdrawDetails);
            }
        }

    }

#endregion


#region BonusDetails
    //-------------------------------------------------- 
    //---------------------bonus details-----------

    public void GetBonusDetails(string phoneNumberWithoutPrefix, string pageIndex, string pageSize, Action<GetBonusDetails> _callback)
    {
        if (debug)
        {
            Debug.Log("GetBonus");
        }
        GetBonusDetailsAPIUrl = BaseAPI + "" + GetBonusDetailsAPI + phoneNumberWithoutPrefix.ToString() + "&&pageIndex=" + pageIndex.ToString() + "&&PageS=" + pageSize.ToString();
        if (GetBonusDetailsAPIUrl != "" && (GetBonusDetailsAPIUrl.StartsWith("http") || GetBonusDetailsAPIUrl.StartsWith("file")))
        {
            StopCoroutine(GetBonusDetailsRequest(GetBonusDetailsAPIUrl, phoneNumberWithoutPrefix, _callback));
            StartCoroutine(GetBonusDetailsRequest(GetBonusDetailsAPIUrl, phoneNumberWithoutPrefix, _callback));
        }
    }
    public void GetBonusDetails(string phoneNumberWithoutPrefix, Action<GetBonusDetails> _callback)
    {
        if (debug)
        {
            Debug.Log("GetBonusDetails");
        }
        GetBonusDetailsAPIUrl = BaseAPI + "" + GetBonusDetailsAPI + phoneNumberWithoutPrefix.ToString();
        if (GetBonusDetailsAPIUrl != "" && (GetBonusDetailsAPIUrl.StartsWith("http") || GetBonusDetailsAPIUrl.StartsWith("file")))
        {
            StopCoroutine(GetBonusDetailsRequest(GetBonusDetailsAPIUrl, phoneNumberWithoutPrefix, _callback));
            StartCoroutine(GetBonusDetailsRequest(GetBonusDetailsAPIUrl, phoneNumberWithoutPrefix, _callback));
        }
    }
    public IEnumerator GetBonusDetailsRequest(string url, bool debug, string phoneNumberWithoutPrefix, Action<GetBonusDetails> _callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);


        yield return www.SendWebRequest();


        if (www.isNetworkError || www.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Could not download config file " + www.error);
            }
            GetBonusDetails getBonusDetails = new GetBonusDetails();
            getBonusDetails.status = "500";
            if (_callback != null)
            {
                _callback.Invoke(getBonusDetails);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("www GetBonusDetailsRequest" + www.downloadHandler.text);
            }
            _serverGetBonusDetailsResponse = www.downloadHandler.text;
        }
    }
    private IEnumerator GetBonusDetailsRequest(string url, string phoneNumberWithoutPrefix, Action<GetBonusDetails> _callback)
    {
        if (debug)
        {
            Debug.Log("URL: " + url);
        }
        StopCoroutine(GetBonusDetailsRequest(url, debug, phoneNumberWithoutPrefix, _callback));
        yield return StartCoroutine(GetBonusDetailsRequest(url, debug, phoneNumberWithoutPrefix, _callback));

        try
        {
            string result = _serverGetBonusDetailsResponse;

            GetBonusDetails _getBonusDetails = JsonUtility.FromJson<GetBonusDetails>(result.ToString());

            if (_getBonusDetails != null)
            {
                if (_callback != null)
                {
                    _callback.Invoke(_getBonusDetails);
                }

            }
            else
            {
                GetBonusDetails getBonusDetails = new GetBonusDetails();
                getBonusDetails.status = "500";
                if (_callback != null)
                {
                    _callback.Invoke(getBonusDetails);
                }
            }

        }
        catch
        {
            if (debug)
            {
                Debug.Log("File was not in correct format");

            }
            GetBonusDetails _getBonusDetails = new GetBonusDetails();
            _getBonusDetails.status = "500";
            if (_callback != null)
            {
                _callback.Invoke(_getBonusDetails);
            }
        }

    }

#endregion

#region ReferDetails
    //-------------------------------------------------- 
    //---------------------refer details-----------

    public void GetReferDetails(string phoneNumberWithoutPrefix, string pageIndex, string pageSize, Action<GetReferDetails> _callback)
    {
        if (debug)
        {
            Debug.Log("GetRefer");
        }
        GetReferDetailsAPIUrl = BaseAPI + "" + GetReferDetailsAPI + phoneNumberWithoutPrefix.ToString() + "&&pageIndex=" + pageIndex.ToString() + "&&PageS=" + pageSize.ToString();
        if (GetReferDetailsAPIUrl != "" && (GetReferDetailsAPIUrl.StartsWith("http") || GetReferDetailsAPIUrl.StartsWith("file")))
        {
            StopCoroutine(GetReferDetailsRequest(GetReferDetailsAPIUrl, phoneNumberWithoutPrefix, _callback));
            StartCoroutine(GetReferDetailsRequest(GetReferDetailsAPIUrl, phoneNumberWithoutPrefix, _callback));
        }
    }
    public void GetReferDetails(string phoneNumberWithoutPrefix, Action<GetReferDetails> _callback)
    {
        if (debug)
        {
            Debug.Log("GetReferDetails");
        }
        GetReferDetailsAPIUrl = BaseAPI + "" + GetReferDetailsAPI + phoneNumberWithoutPrefix.ToString();
        if (GetReferDetailsAPIUrl != "" && (GetReferDetailsAPIUrl.StartsWith("http") || GetReferDetailsAPIUrl.StartsWith("file")))
        {
            StopCoroutine(GetReferDetailsRequest(GetReferDetailsAPIUrl, phoneNumberWithoutPrefix, _callback));
            StartCoroutine(GetReferDetailsRequest(GetReferDetailsAPIUrl, phoneNumberWithoutPrefix, _callback));
        }
    }
    public IEnumerator GetReferDetailsRequest(string url, bool debug, string phoneNumberWithoutPrefix, Action<GetReferDetails> _callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);


        yield return www.SendWebRequest();


        if (www.isNetworkError || www.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Could not download config file " + www.error);
            }
            GetReferDetails getReferDetails = new GetReferDetails();
            getReferDetails.status = "500";
            if (_callback != null)
            {
                _callback.Invoke(getReferDetails);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("www GetReferDetailsRequest" + www.downloadHandler.text);
            }
            _serverGetReferDetailsResponse = www.downloadHandler.text;
        }
    }
    private IEnumerator GetReferDetailsRequest(string url, string phoneNumberWithoutPrefix, Action<GetReferDetails> _callback)
    {
        if (debug)
        {
            Debug.Log("URL: " + url);
        }
        StopCoroutine(GetReferDetailsRequest(url, debug, phoneNumberWithoutPrefix, _callback));
        yield return StartCoroutine(GetReferDetailsRequest(url, debug, phoneNumberWithoutPrefix, _callback));

        try
        {
            string result = _serverGetReferDetailsResponse;

            GetReferDetails _getReferDetails = JsonUtility.FromJson<GetReferDetails>(result.ToString());

            if (_getReferDetails != null)
            {
                if (_callback != null)
                {
                    _callback.Invoke(_getReferDetails);
                }

            }
            else
            {
                GetReferDetails getReferDetails = new GetReferDetails();
                getReferDetails.status = "500";
                if (_callback != null)
                {
                    _callback.Invoke(getReferDetails);
                }
            }

        }
        catch
        {
            if (debug)
            {
                Debug.Log("File was not in correct format");

            }
            GetReferDetails _getReferDetails = new GetReferDetails();
            _getReferDetails.status = "500";
            if (_callback != null)
            {
                _callback.Invoke(_getReferDetails);
            }
        }

    }

#endregion


#region WithdrawRefundAPI


    public void GameWithdrawDetailsAPICall(string mobile, string mobile2, string email, string AccountNumber, string amount, string bank_name, string ifsc_code,string username_in_bank,Action<WithdrawRefundDetailsResponse> _action,string transtype,int trans_Continue)
    {
        WithdrawRefundDetails withdrawRefundDetails = new WithdrawRefundDetails();
        withdrawRefundDetails.mobile = mobile;
        withdrawRefundDetails.mobile2 = mobile2;
        withdrawRefundDetails.email = email;
        withdrawRefundDetails.AccountNumber = AccountNumber;
        withdrawRefundDetails.amount = amount;
        withdrawRefundDetails.bank_name = bank_name;
        withdrawRefundDetails.ifsc_code = ifsc_code;
        withdrawRefundDetails.username_in_bank = username_in_bank;
        withdrawRefundDetails.Tran_type = transtype;
		withdrawRefundDetails.Tran_Continue=trans_Continue;
        GameWithdrawDetails(withdrawRefundDetails,_action);
    }

    public void GameWithdrawDetails(WithdrawRefundDetails withdrawRefundDetails,Action<WithdrawRefundDetailsResponse> _action)
    {

        var jsonString = JsonUtility.ToJson(withdrawRefundDetails) ?? "";
        StartCoroutine(GameWithdrawDetailsRequest(BaseAPI + "" + WithdrawRefundAPI, jsonString.ToString(),_action));
    }


    IEnumerator GameWithdrawDetailsRequest(string url, string json,Action<WithdrawRefundDetailsResponse> _action)
    {
        if (debug)
        {
            Debug.Log("json in GameWithdrawDetailsRequest " + json);
        }
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
            WithdrawRefundDetailsResponse withdrawRefundDetailsResponse = new WithdrawRefundDetailsResponse();
            withdrawRefundDetailsResponse.status = "500";
            withdrawRefundDetailsResponse.message = uwr.error;
            if (_action!=null)
            {
                _action.Invoke(withdrawRefundDetailsResponse);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
            GameWithdrawDetailsRequestResponseHandling(uwr.downloadHandler.text.ToString(),_action);
        }
    }
    public void GameWithdrawDetailsRequestResponseHandling(string _withdrawRefundDetailsResponse, Action<WithdrawRefundDetailsResponse> _action)
    {
        try
        {
            string result = _withdrawRefundDetailsResponse;

            WithdrawRefundDetailsResponse withdrawRefundDetailsResponse = JsonUtility.FromJson<WithdrawRefundDetailsResponse>(result.ToString());

            if (withdrawRefundDetailsResponse != null)
            {
                if (debug)
                {
                    Debug.Log("serverUserDetailsResponse.status  " + withdrawRefundDetailsResponse.status);
                }
                string status = withdrawRefundDetailsResponse.status;
                if (debug)
                {
                    Debug.Log("status   " + status);
                }
                if (status.Contains("200"))
                {
                    if (debug)
                    {
                        Debug.Log("successfully withdraw from game ");
                    }
                    if (withdrawRefundDetailsResponse.data != null)
                    {
                        if (!double.IsInfinity(withdrawRefundDetailsResponse.data.Deposit_Cash) && !double.IsNaN(withdrawRefundDetailsResponse.data.Deposit_Cash))
                        {
                            StaticValues.DepositEarningCount = withdrawRefundDetailsResponse.data.Deposit_Cash.ToString("F2");
                            StaticValues.TotalEarningAmount = withdrawRefundDetailsResponse.data.Deposit_Cash;
                            SaveNewMoney(StaticValues.TotalEarningAmount);
                            if (debug)
                            {
                                Debug.Log("withdrawRefundDetailsResponse.data.Deposit_Cash " + withdrawRefundDetailsResponse.data.Deposit_Cash);
                            }
                        }
                        if (!double.IsInfinity(withdrawRefundDetailsResponse.data.Wining_Cash) && !double.IsNaN(withdrawRefundDetailsResponse.data.Wining_Cash))
                        {
                            StaticValues.WithdrawEarningCount = withdrawRefundDetailsResponse.data.Wining_Cash.ToString("F2");
                            
                            if (debug)
                            {
                                Debug.Log("withdrawRefundDetailsResponse.data.Wining_Cash " + withdrawRefundDetailsResponse.data.Wining_Cash);
                            }
                            if (!double.IsInfinity(withdrawRefundDetailsResponse.data.Deposit_Cash) && !double.IsNaN(withdrawRefundDetailsResponse.data.Deposit_Cash))
                            {
                                
                                StaticValues.TotalEarningAmount = withdrawRefundDetailsResponse.data.Deposit_Cash +withdrawRefundDetailsResponse.data.Wining_Cash;
                                SaveNewMoney(StaticValues.TotalEarningAmount);
                                if (debug)
                                {
                                    Debug.Log("StaticValues.TotalEarningAmount " + StaticValues.TotalEarningAmount);
                                }
                            }
                        }
                        if (!double.IsInfinity(withdrawRefundDetailsResponse.data.Bonus_Cash) && !double.IsNaN(withdrawRefundDetailsResponse.data.Bonus_Cash))
                        {
                            StaticValues.PromoEarningCount = withdrawRefundDetailsResponse.data.Bonus_Cash.ToString("F2");

                            if (debug)
                            {
                                Debug.Log("withdrawRefundDetailsResponse.data.Bonus_Cash " + withdrawRefundDetailsResponse.data.Bonus_Cash);
                            }
                            if (!double.IsInfinity(withdrawRefundDetailsResponse.data.Deposit_Cash) && !double.IsNaN(withdrawRefundDetailsResponse.data.Deposit_Cash) && !double.IsInfinity(withdrawRefundDetailsResponse.data.Wining_Cash) && !double.IsNaN(withdrawRefundDetailsResponse.data.Wining_Cash))
                            {

                                StaticValues.TotalEarningAmount = withdrawRefundDetailsResponse.data.Bonus_Cash+ withdrawRefundDetailsResponse.data.Deposit_Cash + withdrawRefundDetailsResponse.data.Wining_Cash;
                                SaveNewMoney(StaticValues.TotalEarningAmount);
                                if (debug)
                                {
                                    Debug.Log("StaticValues.TotalEarningAmount " + StaticValues.TotalEarningAmount);
                                }
                            }
                        }

                        StaticValues.MinimumAmount = withdrawRefundDetailsResponse.data.minimum_withdraw_amount;
						Debug.Log("StaticValues.MinimumAmount.... "+StaticValues.MinimumAmount );

                    }
                }
              
                if (_action != null)
                {
                    _action.Invoke(withdrawRefundDetailsResponse);
                }
            }
            else
            {
                WithdrawRefundDetailsResponse _withdrawRefundDetailsResponse2 = new WithdrawRefundDetailsResponse();
                _withdrawRefundDetailsResponse2.status = "500";
                withdrawRefundDetailsResponse.message = "Format Error";
                if (_action != null)
                {
                    _action.Invoke(_withdrawRefundDetailsResponse2);
                }
            }
            
        }
        catch(Exception e)
        {
            if (debug)
            {
                Debug.LogWarning("File was not in correct format");

            }
            WithdrawRefundDetailsResponse withdrawRefundDetailsResponse = new WithdrawRefundDetailsResponse();
            withdrawRefundDetailsResponse.status = "500";
            withdrawRefundDetailsResponse.message = e.Message;
            if (_action != null)
            {
                _action.Invoke(withdrawRefundDetailsResponse);
            }
        }
    }
#endregion


#region GameBotEnter

    public void CallBotEnter(double bitAmount, double _challLimit, double _PotLimit, string _TableId, string _userType, Action<BotEnterResponse> _action, string _NewGeneratedID,int _howManyBot)
    {
        if (!string.IsNullOrEmpty(FullRoomName))
        {
            string newId = newID();
            GameBotEnterDetailsAPICall(FullRoomName, currentTable.ToString(), bitAmount.ToString(), newId, _challLimit.ToString(), _PotLimit.ToString(), _TableId, _userType, _action, _NewGeneratedID, _howManyBot);

        }
    }
    public void GameBotEnterDetailsAPICall(string game_room, string game_name, string bitAmount, string mobileNumber, string _chaalLimit, string _potLimit, string _TableId, string _userType, Action<BotEnterResponse> _action, string _NewGeneratedID,int _howManyBot)
    {
        BotEnterDetails _botEnterDetails = new BotEnterDetails();
        _botEnterDetails.game_room = game_room;
        _botEnterDetails.game_name = game_name;
        _botEnterDetails.bitamount = bitAmount;
        _botEnterDetails.mobile = mobileNumber;
        _botEnterDetails.MaxChaal = _chaalLimit;
        _botEnterDetails.PotLimit = _potLimit;
        _botEnterDetails.TableId = _TableId;
        _botEnterDetails.usertype = _userType;
        _botEnterDetails.BidType = "Start";
        _botEnterDetails.NewGeneratedId = _NewGeneratedID;
        _botEnterDetails.howManyBot = _howManyBot;
        BotEnterDetails(_botEnterDetails, _userType, _action);
    }

    public void BotEnterDetails(BotEnterDetails botEnterDetails, string _userType, Action<BotEnterResponse> _action)
    {

        var jsonString = JsonUtility.ToJson(botEnterDetails) ?? "";
        StartCoroutine(BotEnterDetailsRequest(BaseAPI + "" + BotEnterAPI, jsonString.ToString(), _userType, _action));
    }


    IEnumerator BotEnterDetailsRequest(string url, string json, string _userType, Action<BotEnterResponse> _action)
    {
        if (debug)
        {
            Debug.Log("json in BotEnterResponse  " + json);
            Debug.Log("url in BotEnterResponse  " + url);
        }
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
                //Debug.Log("Error While Sending: " + uwr.error);
            }
            BotEnterResponse _botEnterResponse = new BotEnterResponse();
            _botEnterResponse.status = "500";
            if (_userType == "P")
            {
                if (_action != null)
                {
                    _action.Invoke(_botEnterResponse);
                }
            }

        }
        else
        {
            if (debug)
            {
               Debug.Log("Received: " + uwr.downloadHandler.text);
            }
            BotEnterDetailsResponseHandling(uwr.downloadHandler.text.ToString(), _userType, _action);
        }
    }
    public void BotEnterDetailsResponseHandling(string _botEnterResponseDetails, string _userType, Action<BotEnterResponse> _action)
    {
        try
        {
            string result = _botEnterResponseDetails;

            BotEnterResponse botEnterResponse = JsonUtility.FromJson<BotEnterResponse>(result.ToString());

            if (botEnterResponse != null)
            {
                if (debug)
                {
                    //Debug.Log("serverUserDetailsResponse.status  " + gameEnterResponse.status);
                }
                string status = botEnterResponse.status;
                if (debug)
                {
                    //Debug.Log("status   " + status);
                }
                if (_userType == "P")
                {
                    if (_action != null)
                    {
                        _action.Invoke(botEnterResponse);
                    }
                }

            }
            else
            {
                BotEnterResponse _botEnterResponse = new BotEnterResponse();
                _botEnterResponse.status = "500";
                if (_userType == "P")
                {
                    if (_action != null)
                    {
                        _action.Invoke(_botEnterResponse);
                    }
                }
            }
            if (debug)
            {
                //Debug.Log("hello   GameenterDetailsResponseHandling ");
            }
        }
        catch
        {
            if (debug)
            {
                // Debug.LogWarning("File was not in correct format");

            }
            BotEnterResponse _botEnterResponse = new BotEnterResponse();
            _botEnterResponse.status = "500";
            if (_userType == "P")
            {
                if (_action != null)
                {
                    _action.Invoke(_botEnterResponse);
                }
            }
        }
    }
#endregion
#region GameExitAPIError
    private readonly string GameExitErrorAPI = "/api/geterror";
  
    public void GameExitErrorDetailsAPICall(string message, string Custom, string Custom2, string Custom3, string mobileNumber,string _gameRoom)
    {
        PostGameExitAPIErrorDetails _postGameExitAPIErrorDetails = new PostGameExitAPIErrorDetails();
        _postGameExitAPIErrorDetails.message = message;
        _postGameExitAPIErrorDetails.Custom = Custom;
        _postGameExitAPIErrorDetails.Custom2 = Custom2;
        _postGameExitAPIErrorDetails.Custom3 = Custom3;
        _postGameExitAPIErrorDetails.mobile = mobileNumber;
        _postGameExitAPIErrorDetails.Gameroom = _gameRoom;
        GameExitErrorDetails(_postGameExitAPIErrorDetails);
    }
    public void GameExitErrorDetails(PostGameExitAPIErrorDetails postGameExitAPIErrorDetails)
    {

        var jsonString = JsonUtility.ToJson(postGameExitAPIErrorDetails) ?? "";
        StartCoroutine(GameExitErrorDetailsRequest(BaseAPI +""+GameExitErrorAPI, jsonString.ToString()));
    }


    IEnumerator GameExitErrorDetailsRequest(string url, string json)
    {
        if (debug)
        {
            Debug.Log("json in GameExitErrorDetailsRequest " + json);
        }
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
        }
        else
        {
            if (debug)
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
        }
    }
    #endregion


    #region VerifyOTp
    public void GameVerifyOTPAPICall(string mobileNumber, string _otp, Action<ServerUserDetailsResponseAddPot> _action)
    {
        VerifyOTPDetails verifyOTPDetails = new VerifyOTPDetails();
        
        verifyOTPDetails.mobile = mobileNumber;
        verifyOTPDetails.otp = _otp;

        GameVerifyOTPDetails(verifyOTPDetails, _action);
    }

    public void GameVerifyOTPDetails(VerifyOTPDetails verifyOTPDetails, Action<ServerUserDetailsResponseAddPot> _action)
    {

        var jsonString = JsonUtility.ToJson(verifyOTPDetails) ?? "";
        StartCoroutine(GameVerifyOTPRequest(BaseAPI + "" + verifyOTPAPI,verifyOTPDetails.mobile,verifyOTPDetails.otp, jsonString.ToString(), _action));
    }


    IEnumerator GameVerifyOTPRequest(string url, string mobileno, string OTP, string json, Action<ServerUserDetailsResponseAddPot> _action)
    {
        if (debug)
        {
            Debug.Log("url is " +  url);
            Debug.Log("json in GameVerifyOTPRequest " + json);
        }
        WWWForm form = new WWWForm();
        form.AddField("myField", "myData");

        var uwr = new UnityWebRequest(url, "PUT");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
            ServerUserDetailsResponseAddPot verifyOTPResponse = new ServerUserDetailsResponseAddPot();
            verifyOTPResponse.status = "500";
            if (_action != null)
            {
                _action.Invoke(verifyOTPResponse);
            }

        }
        else
        {
            if (debug)
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
            GameVerifyOTPDetailsResponseHandling(uwr.downloadHandler.text.ToString(), _action, mobileno);
        }
    }
    public void GameVerifyOTPDetailsResponseHandling(string _gameVerify, Action<ServerUserDetailsResponseAddPot> _action, string mobileno, int _second = 0)
    {
        try
        {
            string result = _gameVerify;

            ServerUserDetailsResponseAddPot serverUserDetailsResponse = JsonUtility.FromJson<ServerUserDetailsResponseAddPot>(result.ToString());

            if (serverUserDetailsResponse != null)
            {
                if (debug)
                {
                    Debug.Log("Server user  custom2  " + serverUserDetailsResponse.data.custom);
                }
                if (serverUserDetailsResponse.status.Equals("200"))
                {      
                    if (!string.IsNullOrEmpty(serverUserDetailsResponse.data.otp_ref_Id))
                    {
                        StaticValues.mobileVerificationId = serverUserDetailsResponse.data.otp_ref_Id;
                    }
                    else
                    {
                        StaticValues.mobileVerificationId = "2s4657265234";
                    }
                    StartCoroutine(GetUserDetailsRequest(BaseAPI + "" + LoginAPI + mobileno + "&&mobileVerificationID=" + StaticValues.mobileVerificationId, null, _action, _second));
                }
                else
                {
                    AppManager.VIEW_CONTROLLER.ClearText();
                    AppManager.VIEW_CONTROLLER.HideLoading();
                    ShowErrorMessage("Invalid OTP entered !!!");
                }

            }
            else
            {


                if (_action != null)
                {
                    _action.Invoke(serverUserDetailsResponse);
                }
            }
            if (debug)
            {
                //Debug.Log("hello   GameenterDetailsResponseHandling ");
            }
        }
        catch
        {
            if (debug)
            {
                // Debug.LogWarning("File was not in correct format");

            }
            ServerUserDetailsResponseAddPot _verifyOTPResponse = new ServerUserDetailsResponseAddPot();
            _verifyOTPResponse.status = "500";
            if (_action != null)
            {
                _action.Invoke(_verifyOTPResponse);
            }
        }
    }
    #endregion

    DateTime expiryTime;
    
    public bool TimerisOnOrOff()
    {
        if (DateTime.Now > expiryTime)
        {
            return true;
        }
        return false;
    }
    void ScheduleTimer()
    {
        expiryTime = DateTime.Now.AddDays(1.0);
        this.WriteTimestamp("timer");
    }
    public void ScheduleTimer(string key)
    {
        expiryTime = DateTime.Now.AddDays(1.0);
        this.WriteTimestamp(key);
    }
    public bool ReadTimestamp(string key)
    {
        long tmp = Convert.ToInt64(PlayerPrefs.GetString(key, "0"));
        if (tmp == 0)
        {
            expiryTime = DateTime.Now;
            WriteTimestamp(key);
            return true;
        }
        expiryTime = DateTime.FromBinary(tmp);
        return true;
    }

    private void WriteTimestamp(string key)
    {
        PlayerPrefs.SetString(key, expiryTime.ToBinary().ToString());
    }
    #region UpdateToken
    public void UpdateTokenAPICall(string mobile,string token)
    {
        TokenDetails tokenDetails = new TokenDetails();
        tokenDetails.mobile = mobile;
        tokenDetails.token = token;
        UpdateTokenUser(tokenDetails);
    }
    

    public void UpdateTokenUser(TokenDetails tokenDetails)
    {
        var jsonString = JsonUtility.ToJson(tokenDetails) ?? "";
        StartCoroutine(UpdateTokenUserRequest(BaseAPI + "" + UpdateTokenAPI, jsonString.ToString()));
    }
    IEnumerator UpdateTokenUserRequest(string url, string json)
    {
        if (debug)
        {
            Debug.Log("json in UpdateTokenUserRequest" + url + " " + json);
        }
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
            
        }
        else
        {
            if (debug)
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
          
        }

    }
    #endregion

}
#region UpdateProfile
[Serializable]
public class UpdateProfileUser
{
    public string UserId;
    public string Image;
}
#endregion


[Serializable]
public class UserInfo
{
    public bool debuger;
    public string username;
    public string mobile;
    public string email;
    public string FirstName;
    public string LastName;
    public string gender;
    public string custom;
    public string UserID;
    public string password;
    public string referral_code;
    public string referral_codeby;
    public int UserActive;
    public string image_str;
    public string distributor_Id;
    public bool isdistibutor;
    public int pancardstatus;
    public int addressp_status;
    public int totalcount;
    public string playtime;
    public string mobile2;
    public string avatar_url;
    public string PancardNo;
    public string pan_doc;
    public string add_proof_type;
    public string addressProofno;
    public string address_Proof_doc;
    public bool iskycfverify;
    public string Account_no;
    public string back_ifsc_code;
    public string back_name;

    public string facebookid;
    public string userid;

    public string custom2;
    public string custom3;
    public string custom4;
    public string custom5;

    public double amount;
    public double totalamount;


    public string CountryCode;
    public string usertype;
    public string state;
    public string street1;
    public string street2;
    public string zip;
    public string dob;
    public string city;
    public double percent;
    public string companyId;
    public string status;
    public bool isbotstatus;

    public bool isemailverify;
    public bool ismobile_verify;

    public string NewAccount_no;
    public string Newback_ifsc_code;
    public bool isBankDetailsSubmitted;
    public string isBankStatusForNewRequest;
    public string BankUPIId_NR;
    public string BankUPIId;
    public string BankUPIId_status;
    public bool isUPIBankDetailsSubmitted;
    public int ismaintenance;
    public int botthreshold;
    public string otp;
    public string otp_ref_Id;
    public bool isotpVerify;
    public bool HaveReferralCode;
}
[Serializable]
public class ServerUserDetailsResponse
{
    public string status;
    public string message;
    public UserInfo data;
    
}
[Serializable]
public class ServerUserDetailsResponseAddPot
{
    public string status;
    public string message;
    public UserInfo data;
    public boot_challimited[] data_2;
    public Addversion data_3;
}
public static class PropertiesKeys
{
    //Room
    public const string RoomPassword = "rPsw";
    public const string RoomAmount = "rPam";
    public const string RoomName = "rPna";

}
#region GameEnter
[Serializable]
public class GameEnterResponse
{
    public string status;
    public string message;
    //public UserInfo[] data;
}
[Serializable]
public class GameEnterDetails
{
    public string game_name;
    public string game_room;
    public string bitamount;
    public string mobile;
    public string PotLimit;
    public string MaxChaal;
    public string TableId;
    public string usertype;
    public string BidType;
    public string NewGeneratedId;
}

#endregion

#region BotEnter
[Serializable]
public class BotEnterResponse
{
    public string status;
    public string message;
    public UserInfo[] data;
}
[Serializable]
public class BotEnterDetails
{
    public string game_name;
    public string game_room;
    public string bitamount;
    public string mobile;
    public string PotLimit;
    public string MaxChaal;
    public string TableId;
    public string usertype;
    public string BidType;
    public string NewGeneratedId;
    public int howManyBot;
}
#endregion
#region GameExit
[Serializable]
public class GameExitResponse
{
    public string status;
    public string message;
    public GameExitResponseDetails data;
}
[Serializable]
public class GameExitResponseDetails
{
    public double amount;
}
[Serializable]
public class GameExitDetails
{
    public string gamename;
    public string game_room;
    public string gamestatus;
    public string mobile;
    public string usertype;
}
#endregion
#region UpdateUserWallet
[Serializable]
public class ServerUserWalletResponse
{
    public string status;
    public string message;
    public int ismaintenance;
    public Addversion version;
    public string RAF_SMS;
    public string RAF_Whatsapp;
    public ServerUserWalletDetails data;
}
[Serializable]
public class ServerUserWalletDetails
{
    public double Bonus_Cash;
    public double Deposit_Cash;
    public double Wining_Cash;
    public int minimum_withdraw_amount;
}
[Serializable]
public class ServerUserWalletDetailsNB
{
	public double Bonus_Cash;
	public double Deposit_Cash;
	public double Wining_Cash;
	public int minimum_withdraw_amount;
	public string mobile;
}
#endregion
#region DepositDetails
[Serializable]
public class GetDepositDetails
{
    public string status;
    public string message;
    public DepositDetails data;
}
[Serializable]
public class DepositDetails
{
    public string Name;
    public string Email;
    public string Mobile;
    public string PageIndex;
    public string PageSize;
    public string RecordCount;
    public DepositList[] paymnetlist;
}
[Serializable]
public class DepositList
{
    public string DepositAmount;
    public string Id;
    public string DepositStatus;
    public string DepositDate;
}
#endregion

#region WithdrawDetails
[Serializable]
public class GetWithdrawDetails
{
    public string status;
    public string message;
    public WithdrawDetails data;
}
[Serializable]
public class WithdrawDetails
{
    public string Name;
    public string Email;
    public string Mobile;
    public string PageIndex;
    public string PageSize;
    public string RecordCount;
    public WithdrawList[] paymnetlist;
}
[Serializable]
public class WithdrawList
{
    public string WithdrawAmount;
    public string Id;
    public string WithdrawStatus;
    public string WithdrawDate;
}
#endregion
#region BonusDetails
[Serializable]
public class GetBonusDetails
{
    public string status;
    public string message;
    public BonusDetails data;
}
[Serializable]
public class BonusDetails
{
    public string Name;
    public string Email;
    public string Mobile;
    public string PageIndex;
    public string PageSize;
    public string RecordCount;
    public BonusList[] bonusList;
}
[Serializable]
public class BonusList
{
    public string Id;
    public string RedeemDateLabel;
    public string RedeemBonusName;
    public string RedeemBonusReleased;
    public string PendingBonus;
    public string StatusLabel;
}
#endregion
#region ReferDetails
[Serializable]
public class GetReferDetails
{
    public string status;
    public string message;
    public ReferDetails data;
}
[Serializable]
public class ReferDetails
{
    public string Name;
    public string Email;
    public string Mobile;
    public string PageIndex;
    public string PageSize;
    public string RecordCount;
    public ReferList[] referList;
}
[Serializable]
public class ReferList
{
    public string Id;
    public string RegDateLabel;
    public string FriendName;
    public string FriendUserName;
    public string BonusReleased;
    public string DaysLeft;
}
#endregion
[Serializable]
public class UpdateDistributorResponse
{
    public string status;
    public string message;
    public DistributorResponse data;
}
[Serializable]
public class DistributorResponse
{
    public string mobile;
    public string distributor_Id;
}

#region UserName
[Serializable]
public class UserNameResult
{
    public string status;
    public string message;
    public UserNameResponse data;
    public welcomebonustitle data2;
}
[Serializable]
public class welcomebonustitle
{
    public string title;
    public string description;
}
[Serializable]
public class UserNameResponse
{
    public string username;
    public bool exist;
}
[Serializable]
public class UserNameEnter
{
    public string _userName;
    public string mobile;
}
#endregion

#region BonusCode
[Serializable]
public class BonusCodeRequest
{
    public double amount;
    public string couponcode;
    public string mobile;
}
[Serializable]
public class BonusCodeResult
{
    public string status;
    public string message;
    public object data;
}
#endregion

#region CheckReferralCode
[Serializable]
public class ReferralCodeResult
{
    public string status;
    public string message;
    public ReferralCodeResponse data;
}
[Serializable]
public class ReferralCodeResponse
{
    public bool isreferral;
}
#endregion

#region Nextbit
[Serializable]
public class NextBit
{
    public string amount;
    public string symboles;
    public string mobile;
    public string game_room;
    public string game_name;
    public string BidType;
    public string usertype;
    public string GameRoom_2;
    public string NewGeneratedId;
}
[Serializable]
public class NextBitResponse
{
    public string status;
    public string message;
    public int rand_next;
    public ServerUserWalletDetailsNB data;
}
#endregion

#region OnlinePlayerDetails
[Serializable]
public class OnlinePlayerDetails
{
    public int status;
    public string message;
    public int ismaintenance;
    public Addversion version;
    public string RAF_SMS;
    public string RAF_Whatsapp;
    public OnlinePlayerList[] data;
    public boot_challimited[] data_2;
}
[Serializable]
public class OnlinePlayerList
{
    public int Activeuser;
    public double total_amount;
}
#endregion
#region UpdateRefferralCode
[Serializable]
public class UpdateRefferCode
{
    public string referral_codeby;
    public string mobile;
    public string second;
    //public string device_uniqueId;
}
[Serializable]
public class ServerReferCodeResponse
{
    public string status;
    public string message;
    public System.Object data;
}
#endregion
#region GetBanner
[Serializable]
public class GetBannerResponse
{
    public int status;
    public string message;
    public GetBannerImageDetail[] data;
    public GetReferImageDetail data2;
    public double Last_Diposit;
}
[Serializable]
public class GetBannerImageDetail
{
    public string benner_source;
    public string couponcode;
    public int bonus_persent;
    public double amount;
    public double mininum_amount;
    public string extralbanner_url;
    public bool extralbannercheck;
    public Sprite sprite1;
}
[Serializable]
public class GetReferImageDetail
{
    public string Url;
    public string Text;
}
#endregion

#region UpdateBankDetails
[Serializable]
public class ServerBankDetailsResponse
{
    public string status;
    public string message;
    public bank_details data;
}
[Serializable]
public class bank_details
{
    public string NewAccount_no;
    public string Newback_ifsc_code;
    public bool isBankDetailsSubmitted;
    public string isBankStatusForNewRequest;
    public string Account_no;
    public string back_ifsc_code;
    public string back_name;
    public string mobile;
    public int option;
    public bool isUPIBankDetailsSubmitted;
    public string BankUPIId_status;
    public string BankUPIId;
    public string BankUPIId_NR;
}
[Serializable]
public class ServerBankDetailsRequest
{
    public string mobile;
    public string Account_no;
    public string back_ifsc_code;
    public string back_name;
    public string option;
    public string NewAccount_no;
    public string Newback_ifsc_code;
    public string BankUPIId;
    public string BankUPIId_NR;
}
#endregion

#region UpdateKYCDetails
[Serializable]
public class ServerKYCDetailsResponse
{
    public string status;
    public string message;
    public ServerKYCDetailsData data;
}
[Serializable]
public class ServerKYCDetailsData
{
    public int pancardstatus;
    public int addressp_status;
}
[Serializable]
public class ServerKYCDetailsRequest
{
    public string mobile;
    public string PancardNo;
    public string add_proof_type;
    public string address_Proof_doc;
    public string addressProofno;
    public string pan_doc;
}
#endregion
#region SavedCards
[Serializable]
public class SavedCards
{
    public string mobile;
    public string GameId;
    public string GameName;
    public string Card1;
    public string Card2;
    public string Card3;
    public string Priority;
    public string BotType;
    public string GameRoom_2;
    public int Rand_next;
    public double amount;
    public string NewGeneratedId;
    public string start_chal;
}
#endregion

#region BootChaalLimit
[Serializable]
public class boot_challimited
{
    public double bootamount;
    public double PotLimit;
    public double chaallimited;
    public string Id;
}
#endregion

#region WithdrawRefund
[Serializable]
public class WithdrawRefundDetails
{
    public string mobile;
    public string amount;
    public string bank_name;
    public string AccountNumber;
    public string ifsc_code;
    public string mobile2;
    public string email;
    public string username_in_bank;
    public string Tran_type;
	public int Tran_Continue;

}
[Serializable]
public class WithdrawRefundDetailsResponse
{
    public string status;
    public string message;
    public ServerUserWalletDetails data;
    public bank_details data2;
}
#endregion

[Serializable]
public class Addversion
{
    public string url;
    public int version;
    public string Id;
    public string type;
}
#region GameExitAPIError
[Serializable]
public class PostGameExitAPIErrorDetails
{
    public string message;
    public string Custom;
    public string Custom2;
    public string Custom3;
    public string mobile;
    public string Gameroom;
}
#endregion

[Serializable]
public class VerifyOTPDetails
{
    public string mobile;
    public string otp;
}
[Serializable]
public class TokenDetails
{
    public string mobile;
    public string token;
}
