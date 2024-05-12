using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;

public class InitLauchScreen : MonoBehaviourPunCallbacks, IConnectionCallbacks
{
    public Button RegisterButton;
    public Button LoginButton;
    public GameObject panelLoad;
  
    public GameObject Panel_Login;
    public GameObject Panel_Register;
    public GameObject Panel_UserIsBlocked;

    public Button SignInButton;
    public Button SignUpButton;

    public Sprite EnableImage;
    public Sprite DisbaleImage;

    public InputField RInputName;
    public InputField RInputMobile;
    public InputField RInputEmail;
    public InputField RInputPassword;
    public InputField RInputConfirmPassword;

   
    public InputField LInputMobile;
   
    public InputField LInputPassword;
    public Text LCountryText;
    public Text RCountryText;

    public Color EnableColor;
    public Color DisableColor;

    private string Gender = "Male";

    public AudioSource audioSource;
    public AudioClip click;
    public AudioClip clickClose;

    public const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
#if UNITY_IOS
     private string gameId = "2590819";
#elif UNITY_ANDROID
    private string gameId = "2590818";
#endif  

    public ToggleGroup toggleGroupInstance;
    string gameVersion = "v0.15";

    public Toggle currentSelection
    {
        get { return toggleGroupInstance.ActiveToggles().FirstOrDefault(); }
    }
   
    // public this for initialization
    void Start()
    {
        panelLoad.SetActive(true);

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;

        //Advertisement.Initialize(gameId);
        PlayerSave.singleton.ClearMoneyBeforeGame();

        RegisterButton.onClick.AddListener(OnRegisterTab);
        LoginButton.onClick.AddListener(OnLoginTab);
        OnLoginTab();

        SignInButton.onClick.AddListener(OnSignIn);
        SignUpButton.onClick.AddListener(OnNewSignUp);

        RInputName.onEndEdit.AddListener(OnSaveName);
        RInputName.onValueChanged.AddListener(OnSaveName);
      
        RInputMobile.onValueChanged.AddListener(OnColorSaveMobile);
        RInputMobile.onEndEdit.AddListener(OnSaveMobile);
        RInputEmail.onValueChanged.AddListener(onSaveEmail);
        RInputEmail.onEndEdit.AddListener(onSaveEmail);
        RInputPassword.onValueChanged.AddListener(OnSavePassword);
        RInputPassword.onEndEdit.AddListener(OnSavePassword);
        RInputConfirmPassword.onEndEdit.AddListener(OnSaveConfirmPassword);
        RInputConfirmPassword.onValueChanged.AddListener(OnSaveConfirmPassword);

        LInputMobile.onEndEdit.AddListener(OnGetMobile);
        LInputMobile.onValueChanged.AddListener(OnColorGetMobile);
       
        SelectToggle();

        

        Invoke("WaitForOne", 1f);
        
    }
    private void WaitForOne()
    {
        if (PlayerSave.singleton != null)
        {
            PlayerSave.singleton.ClearMoneyBeforeGame();
            if (!string.IsNullOrEmpty(PlayerSave.singleton.GetMobileId()))
            {
                LInputMobile.text = PlayerSave.singleton.GetMobileId();
                LInputPassword.text = PlayerSave.singleton.GetPassword();
                GetUser(PlayerSave.singleton.GetUserName(), PlayerSave.singleton.GetMobileId(), PlayerSave.singleton.GetEmail(), PlayerSave.singleton.GetPassword() , OnLoginResponse);
            }
            else
            {
                if(PhotonNetwork.IsConnected)
                {
                    panelLoad.SetActive(false);
                    Debug.Log("password22 ");
                }
                else
                {
                    Debug.Log("password11 " );
                    
                }
            }
        }
        if(PlayerSave.singleton!=null)
        {
            string password = PlayerSave.singleton.GetPassword();
            
            if(string.IsNullOrEmpty(password))
            {
          
                PlayerSave.singleton.playerSaveData = new PlayerSaveData();
            }
        }
    }
    private void OnRegisterTab()
    {
        RegisterButton.GetComponent<Image>().sprite = EnableImage;
        LoginButton.GetComponent<Image>().sprite = DisbaleImage;
        Panel_Login.SetActive(false);
        Panel_Register.SetActive(true);

        RInputName.text = string.Empty;
      
        RInputMobile.text = string.Empty;
        RInputEmail.text = string.Empty;
        RInputPassword.text = string.Empty;
        RInputConfirmPassword.text = string.Empty;

       if(audioSource!=null)
        {
            audioSource.clip = click;
            audioSource.Play();
        }
    }
    private void OnLoginTab()
    {
        RegisterButton.GetComponent<Image>().sprite = DisbaleImage;
        LoginButton.GetComponent<Image>().sprite = EnableImage;
        Panel_Login.SetActive(true);
        Panel_Register.SetActive(false);
        if (audioSource != null)
        {
            audioSource.clip = click;
            audioSource.Play();
        }
    }
    private void OnSignIn()
    {
        if (LInputMobile.text.Length != 0)
        {
            if (LInputMobile.text.Length >= 10)
            {
               
                    // Get details to database 
                    GetUser("", LInputMobile.text, "", LInputPassword.text, OnLoginResponse);
                
            }
            else
            {
               // PlayerSave.singleton.ShowErrorMessage("You must enter at least in one section.");
            }
        }
        else
        {
            //PlayerSave.singleton.ShowErrorMessage("You must enter at least in one section.");
        }
        if (audioSource != null)
        {
            audioSource.clip = click;
            audioSource.Play();
        }
    }
    private void OnSignUp()
    {
        // Input fields check
        if (RInputName.text.Length != 0)
        {
            if (RInputName.text.Length >=3)
            {
                if (RInputMobile.text.Length != 0 || RInputEmail.text.Length!=0)
                {
                    if (RInputMobile.text.Length >= 10)
                    {
                        if (validateEmail(RInputEmail.text))//Post details to database in both cases because email & mNumber is optional
                        {
                            // POST details to database 
                            RegisterUser(RInputName.text,RInputMobile.text,RInputEmail.text, Gender, OnRegistrationResponse);
                        }
                        else
                        {
                            // POST details to database
                            RegisterUser(RInputName.text, RInputMobile.text, "", Gender, OnRegistrationResponse);
                        }
                    }
                    else
                    {
                        if (validateEmail(RInputEmail.text))
                        {
                            // Success - Email valid
                            // POST details to database
                            RegisterUser(RInputName.text, "", RInputEmail.text, Gender, OnRegistrationResponse);
                        }
                        else
                        {
                            // Error - Email not valid
                            //PlayerSave.singleton.ShowErrorMessage("Your email is invalid." +
                             //   "\nPlease check the spelling and try again.");
                        }
                    }
                }
                else
                {

                    //PlayerSave.singleton.ShowErrorMessage("You must enter at least in one section.");
                }
            }
            else
            {
                //PlayerSave.singleton.ShowErrorMessage("Your name must contain at least 3 character.");
            }
           
        }
        else
        {
            // Error - Empty input fields
            //PlayerSave.singleton.ShowErrorMessage("You must enter your name!!!");
        }
        if (audioSource != null)
        {
            audioSource.clip = click;
            audioSource.Play();
        }
    }
    private void OnNewSignUp()
    {
        // Input fields check
        if (RInputName.text.Length != 0)
        {
            if (RInputName.text.Length >= 3)
            {
                if (RInputMobile.text.Length != 0 || RInputEmail.text.Length != 0)
                {
                        if (RInputMobile.text.Length >= 10)
                        {
                            if (validateEmail(RInputEmail.text))//Post details to database in both cases because email & mNumber is optional
                            {
                                if (RInputPassword.text.Equals(RInputConfirmPassword.text))
                                {
                                    if (RInputPassword.text.Length > 0)
                                    {
                                        // POST details to database 
                                        RegisterUser(RInputName.text, "", RInputMobile.text, RInputEmail.text, Gender, RInputPassword.text, OnRegistrationResponse);
                                    }
                                    else
                                    {
                                        //PlayerSave.singleton.ShowErrorMessage("Passwords do not match.");
                                    }

                                }
                                else
                                {
                                    //PlayerSave.singleton.ShowErrorMessage("Passwords do not match.");
                                }
                            }
                            else
                            {
                                if (RInputPassword.text.Equals(RInputConfirmPassword.text))
                                {
                                    if (RInputPassword.text.Length > 0)
                                    {
                                        // POST details to database
                                        RegisterUser(RInputName.text, "", RInputMobile.text, "", Gender, RInputPassword.text, OnRegistrationResponse);
                                    }
                                    else
                                    {
                                        //PlayerSave.singleton.ShowErrorMessage("Passwords do not match.");
                                    }
                                }
                                else
                                {
                                    //PlayerSave.singleton.ShowErrorMessage("Passwords do not match.");
                                }
                            }
                        }
                        else
                        {
                            
                               
                               // PlayerSave.singleton.ShowErrorMessage("Your mobile must contain 10 character.");
                           
                        }
                    }
                }
                else
                {

                    //PlayerSave.singleton.ShowErrorMessage("You must enter at least in one section.");
                }
            }
            else
            {
                //PlayerSave.singleton.ShowErrorMessage("Your name must contain at least 3 character.");
            }

       
    }
    private void RegisterUser(string _name,string _mobile,string _email ,string _Gender, Action<ServerUserDetailsResponse> _callBack)
    {
        panelLoad.SetActive(true);
        SignUpButton.interactable = false;
        
        
        if(PlayerSave.singleton!=null)
        {
            PlayerSave.singleton.AddNewUserAPICall(_name, _mobile, _email, _Gender, DeviceUniqueIdentifier, _callBack);
        }
    }
    private void RegisterUser(string _name,string _username, string _mobile, string _email, string _Gender, string _password ,Action<ServerUserDetailsResponse> _callBack)
    {
        panelLoad.SetActive(true);
        SignUpButton.interactable = false;


        if (PlayerSave.singleton != null)
        {
            PlayerSave.singleton.AddNewUserAPICall(_name,_username ,_mobile, _email, _Gender, _password, DeviceUniqueIdentifier, _callBack);
        }
    }
    private void OnRegistrationResponse(ServerUserDetailsResponse _serverUserDetailsResponse)
    {
        panelLoad.SetActive(false);
        SignUpButton.interactable = true;
        if (_serverUserDetailsResponse!=null)
        {
            if(_serverUserDetailsResponse.status.Equals("200"))
            {
                //PlayerSave.singleton.ShowErrorMessage( _serverUserDetailsResponse.message);
                OnLoginTab();
            }
            else if (_serverUserDetailsResponse.status.Equals("404"))
            {
                //PlayerSave.singleton.ShowErrorMessage( _serverUserDetailsResponse.message);
            }
            else
            {
                //PlayerSave.singleton.ShowErrorMessage( _serverUserDetailsResponse.message);
            }
        }
    }
   
    private void GetUser(string _username,string _mobile, string _email,string _password, Action<ServerUserDetailsResponseAddPot> _callBack)
    {
        panelLoad.SetActive(true);
        SignInButton.interactable = false;


        if (PlayerSave.singleton != null)
        {
            PlayerSave.singleton.GetUserDetails(_username,_mobile, _email, _password, _callBack,0);
        }
    }
    private void OnLoginResponse(ServerUserDetailsResponseAddPot _serverUserDetailsResponse)
    {
        panelLoad.SetActive(false);
        SignInButton.interactable = true;
        if (_serverUserDetailsResponse != null)
        {
            if (_serverUserDetailsResponse.status.Equals("200"))
            {
               // PlayerSave.singleton.ShowErrorMessage(_serverUserDetailsResponse.message);
                if (_serverUserDetailsResponse.data != null)
                {
                    PlayerSave.singleton.SaveNewName(_serverUserDetailsResponse.data.FirstName +" "+_serverUserDetailsResponse.data.LastName);
                    LoadSceneMainMenu();
                }
            }
            else if (_serverUserDetailsResponse.status.Equals("404"))
            {
               //PlayerSave.singleton.ShowErrorMessage(_serverUserDetailsResponse.message);
                if (!_serverUserDetailsResponse.message.Equals("User is blocked!"))
                {
                    OnRegisterTab();
                }
                else
                {
                    if(Panel_UserIsBlocked)
                    {
                        Panel_UserIsBlocked.SetActive(true);
                    }
                }
            }
            else
            {
                
                  //  PlayerSave.singleton.ShowErrorMessage(_serverUserDetailsResponse.message);
                
            }
        }
    }
    private void OnSaveName(string newName)
    {
        if (newName.Length >= 3)
        {
            RInputName.interactable = true;
           
            RInputMobile.interactable = true;
            RInputEmail.interactable = true;
            RInputPassword.interactable = true;
            RInputConfirmPassword.interactable = true;
            SignUpButton.interactable = false;
        }
        else
        {
            RInputName.interactable = true;
          
            RInputMobile.interactable = true;
            RInputEmail.interactable = true;
            RInputPassword.interactable = true;
            RInputConfirmPassword.interactable = true;
            SignUpButton.interactable = false;
        }
    }
   
    
    private void OnUserNameResponse(UserNameResult _userNameResult)
    {
        Debug.Log("_userNameResult " + _userNameResult.status);
        if (_userNameResult.status.Equals("200"))
        {
            if(_userNameResult.data!=null)
            {
                if(!_userNameResult.data.exist)
                {
                   
                    RInputName.interactable = true;
                   
                    RInputMobile.interactable = true;
                    RInputEmail.interactable = true;
                    RInputPassword.interactable = true;
                    RInputConfirmPassword.interactable = false;
                    SignUpButton.interactable = false;
                    
                   
                    PlayerSave.singleton.SetUserNameExist(true);
                }
                else
                {
                   
                    PlayerSave.singleton.SetUserNameExist(false);
                }
            }
        }
        else
        {
          
            PlayerSave.singleton.SetUserNameExist(false);
        }
    }
    private void OnSavePassword(string newName)
    {
        RInputName.interactable = true;
       
        RInputMobile.interactable = true;
        RInputEmail.interactable = true;
        RInputPassword.interactable = true;
        RInputConfirmPassword.interactable = true;
        SignUpButton.interactable = false;
    }
    private void OnSaveConfirmPassword(string newName)
    {
        RInputName.interactable = true;
      
        RInputMobile.interactable = true;
        RInputEmail.interactable = true;
        RInputPassword.interactable = true;
        RInputConfirmPassword.interactable = true;
        SignUpButton.interactable = true;
    }
    private void OnSaveMobile(string newMobile)
    {
        if (newMobile.Length > 0)
        {
            RCountryText.color = EnableColor;
            if(newMobile.Length>=10)
            {
               
                RInputPassword.interactable = true;
                RInputConfirmPassword.interactable = false;
            }
        }
        else
        {
            RCountryText.color = DisableColor;
            
        }
    }
    private void OnColorSaveMobile(string newMobile)
    {
        if (newMobile.Length > 0)
        {
            RCountryText.color = EnableColor;
        }
        else
        {
            RCountryText.color = DisableColor;
            
        }
    }
    private void onSaveEmail(string newEmail)
    {
        if(validateEmail(newEmail))
        {
            RInputPassword.interactable = true;
            RInputConfirmPassword.interactable = false;
        }
        else
        {
            
        }
    }
    public bool validateEmail(string email)
    {
        if (email != null)
            return Regex.IsMatch(email, MatchEmailPattern);
        else
            return false;
    }
    private void OnGetMobile(string newMobile)
    {
        if(newMobile.Length>0)
        {
            LCountryText.color = EnableColor;
        }
        else
        {
            LCountryText.color = DisableColor;
        }
    }
    private void OnColorGetMobile(string newMobile)
    {
        if (newMobile.Length > 0)
        {
            LCountryText.color = EnableColor;
        }
        else
        {
            LCountryText.color = DisableColor;
        }
    }
    public void SelectToggle()
    {
        var toggles = toggleGroupInstance.GetComponentsInChildren<Toggle>();
        for(int i=0;i<toggles.Length;i++)
        {
            if (i == 0)
            {
                toggles[i].onValueChanged.AddListener(MaleToggle);
            }
            else
            {
                toggles[i].onValueChanged.AddListener(FemaleToggle);
            }
        }
    }
    private void MaleToggle(bool isOn)
    {
        Debug.Log("isOn in male " + isOn);
        if(isOn)
        {
            Gender = "Male";
        }
        if (audioSource != null)
        {
            audioSource.clip = click;
            audioSource.Play();
        }
    }
    private void FemaleToggle(bool isOn)
    {
        Debug.Log("isOn in female " + isOn);
        if (isOn)
        {
            Gender = "Female";
        }
        if (audioSource != null)
        {
            audioSource.clip = click;
            audioSource.Play();
        }
    }
   
    public string DeviceUniqueIdentifier
    {
        get
        {
            var deviceId = "";
#if UNITY_ANDROID && !UNITY_EDITOR
                AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject> ("currentActivity");
                AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject> ("getContentResolver");
                AndroidJavaClass secure = new AndroidJavaClass ("android.provider.Settings$Secure");
                deviceId = secure.CallStatic<string> ("getString", contentResolver, "android_id");
#else
            deviceId = SystemInfo.deviceUniqueIdentifier;
#endif
            return deviceId;
        }
    }

    public override void OnConnected()
    {
        base.OnConnected();
        base.OnJoinedLobby();
        panelLoad.SetActive(false);
       
    }
    
    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void SaveNewName(string newName)
    {
        if (!string.IsNullOrEmpty(newName))
        {
            //InputName.gameObject.SetActive(false);
            PlayerSave.singleton.SaveNewName(newName);
            LoadSceneMainMenu();
        }
        else
        {
            //InputName.gameObject.SetActive(true);
        }
    }

    private void LoadSceneMainMenu()
    {
        panelLoad.SetActive(true);
        PlayerSave.singleton.ClearMoneyBeforeGame();
        
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        
    }
	

}

