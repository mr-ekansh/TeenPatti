//using Photon.Pun.UtilityScripts;
using Photon.Pun;
using Photon.Realtime;
using SocialApp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MatchMakingPhoton : MonoBehaviourPunCallbacks
{
    public GameObject panelLoad;
    public GameObject PanelAfterGame;
  
    public GameObject panelRooms;
    public GameObject panelBottom;
    public static MatchMakingPhoton makingPhoton;
    bool isOnceList = false;
	bool isPrivateOnceList=false;
	bool isPrivateCreateOnceList=false;
    // Use this for initialization


  
    public GameObject Panel_DontHaveMoney;
    public Text Text_DontHaveMoney;
    public Text TitleText_DontHaveMoney;
    public GameObject PanelEnterCode;
    public GameObject PanelUserName;
    public GameObject PanelPopUpBonus;
    public Text BonusTitle;
    public Text BonusDescription;

    public enum BootAmount
    {
        None,
        Standard,
        Free,
        NoLimit,
        Joker,
        Private
    }
    public BootAmount bootAmount = BootAmount.None;

    public AudioSource audioSource;
    public AudioClip click;
    public AudioClip clickClose;

    string gameVersion = "v0.15";

    public InputField JoinRoomCode;

    public Text[] PlayersOnline;
    public GameObject[] ForFree;

    public GameObject Panel_ForChangeOrientationOnly;

    public Text[] SelectedText;
    public Text[] NonSelectedText;

   
    public Image _imageReferBanner;

    private int TotalPublicData = 0;
    private int TotalPrivateData = 0;
    private int TotalFreeData = 0;

    public GameObject CoinsBar;
    public GameObject ChipsBar;

    public RectTransform pointScrollViewRectTransform;

    public GameObject PrivateRoomCode;

    public Text[] ChipsText;
    public Button[] ChipsButton;

    public InputField _userNameInputField;

    public Image[] _playNow;
    public Sprite PlayNowImage;
    public Sprite RsAddCash;
    public override void OnEnable()
    {
     
        base.OnEnable();
        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            if (Panel_ForChangeOrientationOnly) Panel_ForChangeOrientationOnly.SetActive(true);
        }
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.Landscape;

        if (PlayerSave.singleton!=null)
        {
           PlayerSave.singleton.OnPlayersOnline("Public",0,OnPublicResponse);
           PlayerSave.singleton.OnPlayersOnline("Private",1,OnPrivateResponse);
           PlayerSave.singleton.OnPlayersOnline("Free", 2, OnFreeResponse);
           PlayerSave.singleton._howManyBot = 0;
           PlayerSave.singleton.isCreatedRoom = false;
           PlayerSave.singleton.isJoinedRoom = false;
           PlayerSave.singleton.botsServerData = new List<UserInfo>();
        }
        TotalPublicData = 0;
        TotalPrivateData = 0;
        TotalFreeData = 0;

        PlayerSave.OnRefreshUIPotChaal += OnRefreshUIPotChaal;

        if (string.IsNullOrEmpty(StaticValues.UserNameValue))
        {
            if (PanelUserName) PanelUserName.SetActive(true);
            if (PanelPopUpBonus) PanelPopUpBonus.SetActive(false);
        }
        else
        {
            if (PanelUserName) PanelUserName.SetActive(false);
            if (PanelPopUpBonus) PanelPopUpBonus.SetActive(false);
        }

        if (!string.IsNullOrEmpty(StaticValues.FirebaseUserId))
        {
            PlayerSave.singleton.UpdateTokenAPICall(StaticValues.FirebaseUserId, StaticValues.token);
        }
        try
        {
            AndroidRuntimePermissions.Permission ExternalResult = AndroidRuntimePermissions.CheckPermission("android.permission.POST_NOTIFICATIONS");

            if (ExternalResult != AndroidRuntimePermissions.Permission.Granted)
            {
                AndroidRuntimePermissions.Permission result = AndroidRuntimePermissions.RequestPermission("android.permission.POST_NOTIFICATIONS");
                if (result == AndroidRuntimePermissions.Permission.Granted)
                    Debug.Log("We have permission to access external POST_NOTIFICATIONS!");
                else
                    Debug.Log("Permission state: " + result);
            }
        }
        catch
        {

        }

    }
    public override void OnDisable()
    {
        base.OnDisable();
        PlayerSave.OnRefreshUIPotChaal -= OnRefreshUIPotChaal;
    }
    private void OnPublicResponse(int _value)
    {
        if (NonSelectedText[0]) NonSelectedText[0].text = PlayerPublicOnline().ToString();
        if (NonSelectedText[1]) NonSelectedText[1].text = PlayerPrivateOnline().ToString();
        if (NonSelectedText[2]) NonSelectedText[2].text = PlayerFreeOnline().ToString();

        if (SelectedText[0]) SelectedText[0].text = PlayerPublicOnline().ToString();
        if (SelectedText[1]) SelectedText[1].text = PlayerPrivateOnline().ToString();
        if (SelectedText[2]) SelectedText[2].text = PlayerFreeOnline().ToString();
    }
    private void OnPrivateResponse(int _value)
    {
        if (NonSelectedText[0]) NonSelectedText[0].text = PlayerPublicOnline().ToString();
        if (NonSelectedText[1]) NonSelectedText[1].text = PlayerPrivateOnline().ToString();
        if (NonSelectedText[2]) NonSelectedText[2].text = PlayerFreeOnline().ToString();
    }
    private void OnFreeResponse(int _value)
    {
        if (NonSelectedText[0]) NonSelectedText[0].text = PlayerPublicOnline().ToString();
        if (NonSelectedText[1]) NonSelectedText[1].text = PlayerPrivateOnline().ToString();
        if (NonSelectedText[2]) NonSelectedText[2].text = PlayerFreeOnline().ToString();
    }
    public void OnPublic()
    {
        if (SelectedText[0]) SelectedText[0].gameObject.SetActive(true);
        if (SelectedText[1]) SelectedText[1].gameObject.SetActive(false);
        if (SelectedText[2]) SelectedText[2].gameObject.SetActive(false);

        if (NonSelectedText[0]) NonSelectedText[0].gameObject.SetActive(false);
        if (NonSelectedText[1]) NonSelectedText[1].gameObject.SetActive(true);
        if (NonSelectedText[2]) NonSelectedText[2].gameObject.SetActive(true);

        if (SelectedText[0]) SelectedText[0].text = PlayerPublicOnline().ToString();
        if (SelectedText[1]) SelectedText[1].text = PlayerPrivateOnline().ToString();
        if (SelectedText[2]) SelectedText[2].text = PlayerFreeOnline().ToString();

        if (NonSelectedText[0]) NonSelectedText[0].text = PlayerPublicOnline().ToString();
        if (NonSelectedText[1]) NonSelectedText[1].text = PlayerPrivateOnline().ToString();
        if (NonSelectedText[2]) NonSelectedText[2].text = PlayerFreeOnline().ToString();

        //if (pointScrollViewRectTransform) pointScrollViewRectTransform.offsetMin = new Vector2(447.8434f, 0f);
        //if (pointScrollViewRectTransform) pointScrollViewRectTransform.offsetMax = new Vector2(0f, -179.0419f);

        if (CoinsBar) CoinsBar.SetActive(true);
        if (ChipsBar) ChipsBar.SetActive(false);
        if(PrivateRoomCode)PrivateRoomCode.SetActive(false);
        for (int i = 0; i < ForFree.Length; i++)
        {
            if (ForFree[i]) ForFree[i].SetActive(false);
        }

        for(int i=0;i<_playNow.Length;i++)
        {
			if (StaticValues.BootAmount[i] <= (float)PlayerSave.singleton.GetCurrentMoney())
            {
				if (((float)StaticValues.PotLimit[i] / 2) <= (float)PlayerSave.singleton.GetCurrentMoney())
              {
                  if (_playNow[i]) _playNow[i].sprite = PlayNowImage;
              }
              else
              {
                  if (_playNow[i]) _playNow[i].sprite = RsAddCash;
              }   
            }
            else
            {
                if (_playNow[i]) _playNow[i].sprite = RsAddCash;
            }
        }
    }
    public void OnPrivate()
    {
        if (SelectedText[0]) SelectedText[0].gameObject.SetActive(false);
        if (SelectedText[1]) SelectedText[1].gameObject.SetActive(true);
        if (SelectedText[2]) SelectedText[2].gameObject.SetActive(false);

        if (NonSelectedText[0]) NonSelectedText[0].gameObject.SetActive(true);
        if (NonSelectedText[1]) NonSelectedText[1].gameObject.SetActive(false);
        if (NonSelectedText[2]) NonSelectedText[2].gameObject.SetActive(true);

        if (SelectedText[0]) SelectedText[0].text = PlayerPublicOnline().ToString();
        if (SelectedText[1]) SelectedText[1].text = PlayerPrivateOnline().ToString();
        if (SelectedText[2]) SelectedText[2].text = PlayerFreeOnline().ToString();

        if (NonSelectedText[0]) NonSelectedText[0].text = PlayerPublicOnline().ToString();
        if (NonSelectedText[1]) NonSelectedText[1].text = PlayerPrivateOnline().ToString();
        if (NonSelectedText[2]) NonSelectedText[2].text = PlayerFreeOnline().ToString();

        //if (pointScrollViewRectTransform) pointScrollViewRectTransform.offsetMin = new Vector2(447.8434f, 97.642f);
        //if (pointScrollViewRectTransform) pointScrollViewRectTransform.offsetMax = new Vector2(-0.003540039f, -179.0419f);

        if (CoinsBar) CoinsBar.SetActive(true);
        if (ChipsBar) ChipsBar.SetActive(false);
        if (PrivateRoomCode) PrivateRoomCode.SetActive(true);

        for (int i = 0; i < ForFree.Length; i++)
        {
            if (ForFree[i]) ForFree[i].SetActive(false);
        }

        for (int i = 0; i < _playNow.Length; i++)
        {
			if (StaticValues.BootAmount[i] <= (float)PlayerSave.singleton.GetCurrentMoney())
            {
				if (((float)StaticValues.PotLimit[i] / 2) <= (float)PlayerSave.singleton.GetCurrentMoney())
                {
                    if (_playNow[i]) _playNow[i].sprite = PlayNowImage;
                }
                else
                {
                    if (_playNow[i]) _playNow[i].sprite = RsAddCash;
                }
            }
            else
            {
                if (_playNow[i]) _playNow[i].sprite = RsAddCash;
            }
        }
    }
    public void OnFree()
    {
        if (SelectedText[0]) SelectedText[0].gameObject.SetActive(false);
        if (SelectedText[1]) SelectedText[1].gameObject.SetActive(false);
        if (SelectedText[2]) SelectedText[2].gameObject.SetActive(true);

        if (NonSelectedText[0]) NonSelectedText[0].gameObject.SetActive(true);
        if (NonSelectedText[1]) NonSelectedText[1].gameObject.SetActive(true);
        if (NonSelectedText[2]) NonSelectedText[2].gameObject.SetActive(false);

        if (SelectedText[0]) SelectedText[0].text = PlayerPublicOnline().ToString();
        if (SelectedText[1]) SelectedText[1].text = PlayerPrivateOnline().ToString();
        if (SelectedText[2]) SelectedText[2].text = PlayerFreeOnline().ToString();

        if (NonSelectedText[0]) NonSelectedText[0].text = PlayerPublicOnline().ToString();
        if (NonSelectedText[1]) NonSelectedText[1].text = PlayerPrivateOnline().ToString();
        if (NonSelectedText[2]) NonSelectedText[2].text = PlayerFreeOnline().ToString();

        //if (pointScrollViewRectTransform) pointScrollViewRectTransform.offsetMin = new Vector2(447.8434f, 0f);
        //if (pointScrollViewRectTransform) pointScrollViewRectTransform.offsetMax = new Vector2(0f, -179.0419f);

        if (CoinsBar) CoinsBar.SetActive(true);
        if (ChipsBar) ChipsBar.SetActive(false);
        if (PrivateRoomCode) PrivateRoomCode.SetActive(false);

        for (int i = 0; i < ForFree.Length; i++)
        {
            if (ForFree[i]) ForFree[i].SetActive(false);
        }

        for (int i = 0; i < _playNow.Length; i++)
        {
            if (StaticValues.BootAmount[i] <= (int)PlayerSave.singleton.GetCurrentMoney())
            {
				if (((int)StaticValues.PotLimit[i] / 2) <= (int)PlayerSave.singleton.GetCurrentMoney())
                {
                    if (_playNow[i]) _playNow[i].sprite = PlayNowImage;
                }
                else
                {
                    if (_playNow[i]) _playNow[i].sprite = PlayNowImage;
                }
            }
            else
            {
                if (_playNow[i]) _playNow[i].sprite = PlayNowImage;
            }
        }
    }
    void Start ()
    {
        makingPhoton = this;


        AppManager.VIEW_CONTROLLER.HideAllScreenExceptPopUp();
        if (Panel_DontHaveMoney) Panel_DontHaveMoney.SetActive(false);
        if (!PhotonNetwork.IsConnected)
        {
            panelLoad.SetActive(true);
            //PhotonNetwork.autoCleanUpPlayerObjects = true;//PhotonNetwork.autoCleanUpPlayerObjects is gone. The setting is per room, so it's now in the RoomOptions. Example: PhotonNetwork.CreateRoom(null, new RoomOptions() { CleanupCacheOnLeave = true });.
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = "1";
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.AuthValues = new AuthenticationValues();
            PhotonNetwork.AuthValues.UserId = PlayerSave.singleton.GetUserId();
        }
        else
        {
            panelLoad.SetActive(false);
            if(PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
              
        }
        if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
        {
            if (PlayerSave.singleton.GameExit)
            {
                PlayerSave.singleton.GameExit = false;
                if (PlayerSave.singleton != null)
                {
                    PlayerSave.singleton.CallGameExit(0,"P",OnGameExitResponse);
                }

            }
        }
        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        PlayerSave.singleton.ClearMoneyBeforeGame();
        JoinRoomCode.onEndEdit.AddListener(OnJoinRoomCode);

        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.Landscape;

        for(int i=0;i< PlayersOnline.Length;i++)
        {
            if (PlayersOnline[i]) PlayersOnline[i].text = "0 Players Online";
        }

        if (Panel_ForChangeOrientationOnly)
        {
            Invoke("WaitPanel_ForChangeOrientationOnly", 3f);
        }
        if (PrivateRoomCode) PrivateRoomCode.SetActive(false);
        OnCreateOrJoinGameStandart();
        OnPublic();

        if (PlayerSave.singleton != null)
        {
            PlayerSave.singleton.GetBannerDetails(PlayerSave.singleton.newID(),OnBannerListLoaded);
        }

        if(_userNameInputField)_userNameInputField.onValidateInput += delegate (string input, int charIndex, char addedChar) { return MyValidate(input, charIndex,addedChar); };
    }
    public void OnBannerListLoaded(GetBannerResponse _callback)
    {
        if(_callback!=null)
        {
            if(_callback.status == 200)
            {
                if(MainMenuUI.menuUI!=null)
                {
                    Debug.Log("MainMenuUI.menuUI.Canvas_AddCash.activeInHierarchy " + MainMenuUI.menuUI.Canvas_AddCash.activeInHierarchy);
                    if (MainMenuUI.menuUI.Canvas_AddCash.activeInHierarchy)
                    {
                        MainMenuUI.menuUI.CallBeforeRefreshAddCashPage();
                        MainMenuUI.menuUI.RefreshAddCashPage(0);
                    }
                }
            }
            else
            {

            }
        }
    }
    private void OnGameExitResponse(GameExitResponse gameExitResponse)
    {
        if(gameExitResponse!=null)
        {
            if (PlayerSave.singleton != null)
            {
                PlayerSave.singleton.OnPlayersOnline("Public", 0, OnPublicResponse);
                PlayerSave.singleton.OnPlayersOnline("Private", 1, OnPrivateResponse);
                PlayerSave.singleton.OnPlayersOnline("Free", 2, OnFreeResponse);
            }
        }
    }
    void WaitPanel_ForChangeOrientationOnly()
    {
        if (Panel_ForChangeOrientationOnly) Panel_ForChangeOrientationOnly.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Home) || 
        {
            //PhotonNetwork.Disconnect();
            AppManager.VIEW_CONTROLLER.ShowQuitMessage();
            //Application.Quit();
        }
        if (PlayerSave.singleton != null)
        {
            if (bootAmount == BootAmount.Standard)
            {
                if (PlayersOnline != null)
                {
                    for (int i = 0; i < PlayersOnline.Length; i++)
                    {

                        if (PlayersOnline[i]) PlayersOnline[i].text = PlayerSave.singleton.PublicPlayerData[i] + " Players Online";

                    }

                }

            }
            else if (bootAmount == BootAmount.Free)
            {
                if (PlayersOnline != null)
                {
                    for (int i = 0; i < PlayersOnline.Length; i++)
                    {

                        if (PlayersOnline[i]) PlayersOnline[i].text = PlayerSave.singleton.FreePlayerData[i] + " Players Online";

                    }
                    OnFreeResponse(2);
                }


            }
            else if (bootAmount == BootAmount.Private)
            {
                if (PlayersOnline != null)
                {
                    for (int i = 0; i < PlayersOnline.Length; i++)
                    {
                        if (PlayersOnline[i]) PlayersOnline[i].text = PlayerSave.singleton.PrivatePlayerData[i] + " Players Online";
                    }
                    OnPrivateResponse(1);
                }
            }
            OnPublicResponse(0);

        }



        if (_imageReferBanner != null)
        {
            if (PlayerSave.singleton != null)
            {
                if (PlayerSave.singleton.ReferImage != null)
                {
                    _imageReferBanner.sprite = PlayerSave.singleton.ReferImage;
                }
            }
        }

        try
        {
            if (_playNow != null)
            {
                if (PlayerSave.singleton != null)
                {
                    if (bootAmount == BootAmount.Standard || bootAmount == BootAmount.Private)
                    {
                        for (int i = 0; i < _playNow.Length; i++)
                        {
							if (StaticValues.BootAmount[i] <= (float)PlayerSave.singleton.GetCurrentMoney())
                            {
								if (((float)StaticValues.PotLimit[i] / 2) <= (float)PlayerSave.singleton.GetCurrentMoney())
                                {
                                    if (_playNow[i]) _playNow[i].sprite = PlayNowImage;
                                }
                                else
                                {
                                    if (_playNow[i]) _playNow[i].sprite = RsAddCash;
                                }
                            }
                            else
                            {
                                if (_playNow[i]) _playNow[i].sprite = RsAddCash;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < _playNow.Length; i++)
                        {
                            if (_playNow[i]) _playNow[i].sprite = PlayNowImage;
                        }
                    }
                }
            }
        }
        catch
        {

        }
    }
    public int PlayerPublicOnline()
    {
        TotalPublicData = 0;
        for (int i = 0; i < PlayerSave.singleton.PublicPlayerData.Length; i++)
        {
            TotalPublicData = TotalPublicData + PlayerSave.singleton.PublicPlayerData[i];
        }
        return TotalPublicData;
    }
    public int PlayerFreeOnline()
    {
        TotalFreeData = 0;
        for (int i = 0; i < PlayerSave.singleton.FreePlayerData.Length; i++)
        {
            TotalFreeData = TotalFreeData + PlayerSave.singleton.FreePlayerData[i];
        }
        return TotalFreeData;
    }
    public int PlayerPrivateOnline()
    {
        TotalPrivateData = 0;
        for (int i = 0; i < PlayerSave.singleton.PrivatePlayerData.Length; i++)
        {
            TotalPrivateData = TotalPrivateData + PlayerSave.singleton.PrivatePlayerData[i];
        }
        return TotalPrivateData;
    }
    public void OnLogout()
    {
        AppManager.VIEW_CONTROLLER.ShowLogoutMessage();
    }
    private void DailyReward()
    {
        int lastDayConnected = PlayerPrefs.GetInt("LastDay", 0);
        int nowDay = System.DateTime.Now.Day;
        if (lastDayConnected != nowDay)
        {
            GameObject.Find("PanelDailyReward").transform.GetChild(0).gameObject.SetActive(true);
            int daysConnected = PlayerPrefs.GetInt("DaysConnected", 0);
            int dailyReward = 300;
            if (lastDayConnected + 1 == nowDay)
            {
                daysConnected++;
                if (daysConnected == 1)
                    dailyReward = 500;
                else if (daysConnected == 2)
                    dailyReward = 900;
                else if (daysConnected == 3)
                    dailyReward = 1200;
                else if (daysConnected == 4)
                    dailyReward = 1500;
                else if (daysConnected > 4)
                    dailyReward = 2000;
            }
            else
            {
                daysConnected = 0;
            }
            GameObject.Find("PanelDailyReward/Panel/TextReward").GetComponent<Text>().text = dailyReward.ToString();
            PlayerSave.singleton.SaveNewMoney(PlayerSave.singleton.GetCurrentMoney() + dailyReward);
            PlayerPrefs.SetInt("DaysConnected", daysConnected);
            PlayerPrefs.SetInt("LastDay", nowDay);
        }
    }

    public override void OnConnected()
    {
        Debug.Log("OnConnectedToPhoton Room ");
        base.OnJoinedLobby();
        base.OnConnected();
        panelLoad.SetActive(false);

        
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby Room ");

        panelLoad.SetActive(false);

        if (isOnceList)
        {
            isOnceList = false;
            JoinRoomAndStartGame();
        }

		if(isPrivateOnceList)
		{
			isPrivateOnceList=false;
			NewJoinPrivateRoom();
		}

		if(isPrivateCreateOnceList)
		{
			isPrivateCreateOnceList=false;
			CreatePrivateRoom(eTable.Private);
		}
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster Room ");

        panelLoad.SetActive(false);

        if (isOnceList)
        {
            isOnceList = false;
            JoinRoomAndStartGame();
        }
		if(isPrivateOnceList)
		{
			isPrivateOnceList=false;
			NewJoinPrivateRoom();
		}

		if(isPrivateCreateOnceList)
		{
			isPrivateCreateOnceList=false;
			CreatePrivateRoom(eTable.Private);
		}
        
    }
    public void OnCreateOrJoinGameStandart()
    {
        isOnceList = false;
		isPrivateOnceList=false;
		isPrivateCreateOnceList=false;
        bootAmount = BootAmount.Standard;
       
        ClickSound();
    }
    public void OnCreateOrJoinGameFree()
    {
        isOnceList = false;
		isPrivateOnceList=false;
		isPrivateCreateOnceList=false;
        bootAmount = BootAmount.Free;

        ClickSound();
    }
    private void OnCreateOrJoinGameNoLimit()
    {
        isOnceList = false;
		isPrivateOnceList=false;
		isPrivateCreateOnceList=false;
        bootAmount = BootAmount.NoLimit;
        //CreateOrJoinGame(eTable.NoLimit);
        if (!PhotonNetwork.IsConnected)
        {
            panelLoad.SetActive(true);
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.AuthValues = new AuthenticationValues();
            PhotonNetwork.AuthValues.UserId = PlayerSave.singleton.GetUserId();
        }
        else
        {
            //if (Panel_BootAmount) Panel_BootAmount.SetActive(true);
        }
        ClickSound();

    }

    private void OnCreateOrJoinGameJoker()
    {
        isOnceList = false;
		isPrivateOnceList=false;
		isPrivateCreateOnceList=false;
        bootAmount = BootAmount.Joker;
        //CreateOrJoinGame(eTable.Joker);     
        if (!PhotonNetwork.IsConnected)
        {
            panelLoad.SetActive(true);
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.AuthValues = new AuthenticationValues();
            PhotonNetwork.AuthValues.UserId = PlayerSave.singleton.GetUserId();
        }
        else
        {
            //if (Panel_BootAmount) Panel_BootAmount.SetActive(true);
        }
        ClickSound();
    }
    public void OnCreateOrJoinGamePrivate()
    {
        PlayerSave.singleton.chaalTime = 25f;
        isOnceList = false;
		isPrivateOnceList=false;
		isPrivateCreateOnceList=false;
        bootAmount = BootAmount.Private;

       
        ClickSound();

    }
  
    public void SetBootAmount(double _amount)
    {
        PlayerSave.singleton.bootAmount = _amount;
        PlayerSave.singleton._TableId = "";
        isOnceList = false;
		isPrivateOnceList=false;
		isPrivateCreateOnceList=false;

        if (bootAmount == BootAmount.Standard || bootAmount == BootAmount.Private)
        {
            if (!string.IsNullOrEmpty(StaticValues.UserNameValue))
            {
				if ((float)_amount <= (float)PlayerSave.singleton.GetCurrentMoney())
                {
                    SetChaalLimit(_amount);
                    if (bootAmount == BootAmount.Standard)
                    {
                        SetChaalLimit(_amount);
						if (((float)PlayerSave.singleton.potLimit/2) <= (float)PlayerSave.singleton.GetCurrentMoney())
                        {
                            CreateOrJoinGame(eTable.Standard);
                        }
                        else
                        {
                            //if (Panel_DontHaveMoney) Panel_DontHaveMoney.SetActive(true);
                            //if (Text_DontHaveMoney) Text_DontHaveMoney.text = "You don't have enough money to" + "\n" + " play game." + "\n" + "Please add cash to play the game.";
                            //if (TitleText_DontHaveMoney) TitleText_DontHaveMoney.text = "Out of Pot Limit!!!";
                            if(MainMenuUI.menuUI!=null)
                            {
                                MainMenuUI.menuUI.OnAddCashbutton();
                            }
                        }
                    }
                    else if (bootAmount == BootAmount.NoLimit)
                    {
                        SetChaalLimit(_amount);
						if (((float)PlayerSave.singleton.potLimit / 2) <= (float)PlayerSave.singleton.GetCurrentMoney())
                        {
                            CreateOrJoinGame(eTable.NoLimit);
                        }
                        else
                        {
                            //if (Panel_DontHaveMoney) Panel_DontHaveMoney.SetActive(true);
                            //if (Text_DontHaveMoney) Text_DontHaveMoney.text = "You don't have enough money to" + "\n" + " play game." + "\n" + "Please add cash to play the game.";
                            //if (TitleText_DontHaveMoney) TitleText_DontHaveMoney.text = "Out of Pot Limit!!!";

                            if (MainMenuUI.menuUI != null)
                            {
                                MainMenuUI.menuUI.OnAddCashbutton();
                            }
                        }
                    }
                    else if (bootAmount == BootAmount.Joker)
                    {
                        SetChaalLimit(_amount);
						if (((float)PlayerSave.singleton.potLimit / 2) <= (float)PlayerSave.singleton.GetCurrentMoney())
                        {
                            CreateOrJoinGame(eTable.Joker);
                        }
                        else
                        {
                            //if (Panel_DontHaveMoney) Panel_DontHaveMoney.SetActive(true);
                            //if (Text_DontHaveMoney) Text_DontHaveMoney.text = "You don't have enough money to" + "\n" + " play game." + "\n" + "Please add cash to play the game.";
                            //if (TitleText_DontHaveMoney) TitleText_DontHaveMoney.text = "Out of Pot Limit!!!";

                            if (MainMenuUI.menuUI != null)
                            {
                                MainMenuUI.menuUI.OnAddCashbutton();
                            }

                        }
                    }
                    else if (bootAmount == BootAmount.Private)
                    {
                        SetChaalLimit(_amount);
						if (((float)PlayerSave.singleton.potLimit / 2) <= (float)PlayerSave.singleton.GetCurrentMoney())
                        {
                            CreatePrivateRoom(eTable.Private);
                        }
                        else
                        {
                            //if (Panel_DontHaveMoney) Panel_DontHaveMoney.SetActive(true);
                            //if (Text_DontHaveMoney) Text_DontHaveMoney.text = "You don't have enough money to" + "\n" + " play game." + "\n" + "Please add cash to play the game.";
                            //if (TitleText_DontHaveMoney) TitleText_DontHaveMoney.text = "Out of Pot Limit!!!";

                            if (MainMenuUI.menuUI != null)
                            {
                                MainMenuUI.menuUI.OnAddCashbutton();
                            }
                        }
                    }
                }
                else
                {
                    //if (Panel_DontHaveMoney) Panel_DontHaveMoney.SetActive(true);
                    //if (Text_DontHaveMoney) Text_DontHaveMoney.text = "You don't have enough money to" + "\n" + " play game." + "\n" + "Please add cash to play the game.";
                    //if (TitleText_DontHaveMoney) TitleText_DontHaveMoney.text = "Out of Money!!!";

                    if (MainMenuUI.menuUI != null)
                    {
                        MainMenuUI.menuUI.OnAddCashbutton();
                    }
                }
            }
            else
            {
                if (PanelUserName) PanelUserName.SetActive(true);
                if (PanelPopUpBonus) PanelPopUpBonus.SetActive(false);
            }
        }
        else
        {
			if ((float)_amount <= (float)PlayerSave.singleton.GetCurrentChips())
            {
                SetChaalLimit(_amount);
                if (bootAmount == BootAmount.Free)
                {
                    SetChaalLimit(_amount);
                    CreateOrJoinGame(eTable.Free);
                }
               
            }
            else
            {
                if (Panel_DontHaveMoney) Panel_DontHaveMoney.SetActive(true);
                if (Text_DontHaveMoney) Text_DontHaveMoney.text = "You don't have enough chips to" + "\n" + " play game." + "\n" + "Please add cash to play the game.";
                if (TitleText_DontHaveMoney) TitleText_DontHaveMoney.text = "Out of Chips!!!";
            }
        }
      
       

        ClickSound();
    }
    public void SetChaalLimit(double _chaalLimit)
    {
        switch(_chaalLimit)
        {
            case 0.01:
                {
                    PlayerSave.singleton.chalLimit = 1.28;
                    PlayerSave.singleton.potLimit = 10.24;
                    SetChaalLimitRefresh(_chaalLimit);
                }
                break;

            case 0.05:
                {
                    PlayerSave.singleton.chalLimit = 6.4;
                    PlayerSave.singleton.potLimit = 51.2;
                    SetChaalLimitRefresh(_chaalLimit);
                }
                break;

          

            case 0.1:
                {
                    PlayerSave.singleton.chalLimit = 12.8;
                    PlayerSave.singleton.potLimit = 102.4;
                    SetChaalLimitRefresh(_chaalLimit);
                }
                break;

            case 0.25:
                {
                    PlayerSave.singleton.chalLimit = 32;
                    PlayerSave.singleton.potLimit = 256;
                    SetChaalLimitRefresh(_chaalLimit);
                }
                break;

            case 0.5:
                {
                    PlayerSave.singleton.chalLimit = 64;
                    PlayerSave.singleton.potLimit = 512;
                    SetChaalLimitRefresh(_chaalLimit);
                }
                break;

            case 1:
                {
                    PlayerSave.singleton.chalLimit = 128;
                    PlayerSave.singleton.potLimit = 1024;
                    SetChaalLimitRefresh(_chaalLimit);
                }
                break;

            //case 1.5:
            //    {
            //        PlayerSave.singleton.chalLimit = 192;
            //        PlayerSave.singleton.potLimit = 1536;
            //    }
            //    break;

            case 2:
                {
                    PlayerSave.singleton.chalLimit = 256;
                    PlayerSave.singleton.potLimit = 2048;
                    SetChaalLimitRefresh(_chaalLimit);
                }
                break;
            //case 2.5:
            //    {
            //        PlayerSave.singleton.chalLimit = 320;
            //        PlayerSave.singleton.potLimit = 2560;
            //    }
            //    break;

            case 3:
                {
                    PlayerSave.singleton.chalLimit = 384;
                    PlayerSave.singleton.potLimit = 3072;
                    SetChaalLimitRefresh(_chaalLimit);
                }
                break;

            //case 3.5:
            //    {
            //        PlayerSave.singleton.chalLimit = 448;
            //        PlayerSave.singleton.potLimit = 3584;
            //    }
            //    break;

            case 4:
                {
                    PlayerSave.singleton.chalLimit = 512;
                    PlayerSave.singleton.potLimit = 4096;
                    SetChaalLimitRefresh(_chaalLimit);
                }
                break;

            //case 4.5:
            //    {
            //        PlayerSave.singleton.chalLimit = 576;
            //        PlayerSave.singleton.potLimit = 4608;
            //    }
            //    break;

            case 5:
                {
                    PlayerSave.singleton.chalLimit = 640;
                    PlayerSave.singleton.potLimit = 5120;
                    SetChaalLimitRefresh(_chaalLimit);
                }
                break;

            //case 6:
            //    {
            //        PlayerSave.singleton.chalLimit = 768;
            //        PlayerSave.singleton.potLimit = 6144;
            //    }
            //    break;

            //case 7:
            //    {
            //        PlayerSave.singleton.chalLimit = 896;
            //        PlayerSave.singleton.potLimit = 7168;
            //    }
            //    break;

            //case 8:
            //    {
            //        PlayerSave.singleton.chalLimit = 1024;
            //        PlayerSave.singleton.potLimit = 8192;
            //    }
            //    break;

            //case 9:
            //    {
            //        PlayerSave.singleton.chalLimit = 1152;
            //        PlayerSave.singleton.potLimit = 9216;
            //    }
            //    break;

            case 10:
                {
                    PlayerSave.singleton.chalLimit = 1280;
                    PlayerSave.singleton.potLimit = 10240;
                    SetChaalLimitRefresh(_chaalLimit);
                }
                break;

            //case 12:
            //    {
            //        PlayerSave.singleton.chalLimit = 1536;
            //        PlayerSave.singleton.potLimit = 12288;
            //    }
            //    break;

            //case 14:
            //    {
            //        PlayerSave.singleton.chalLimit = 1792;
            //        PlayerSave.singleton.potLimit = 14336;
            //    }
            //    break;

            //case 16:
            //    {
            //        PlayerSave.singleton.chalLimit = 2048;
            //        PlayerSave.singleton.potLimit = 16384;
            //    }
            //    break;

            //case 18:
            //    {
            //        PlayerSave.singleton.chalLimit = 2304;
            //        PlayerSave.singleton.potLimit = 18432;
            //    }
            //    break;

            case 20:
                {
                    PlayerSave.singleton.chalLimit = 2560;
                    PlayerSave.singleton.potLimit = 20480;
                    SetChaalLimitRefresh(_chaalLimit);
                }
                break;

            //case 30:
            //    {
            //        PlayerSave.singleton.chalLimit = 3840;
            //        PlayerSave.singleton.potLimit = 30720;
            //    }
            //    break;

            //case 40:
            //    {
            //        PlayerSave.singleton.chalLimit = 5120;
            //        PlayerSave.singleton.potLimit = 40960;
            //    }
            //    break;

            //case 50:
            //    {
            //        PlayerSave.singleton.chalLimit = 6400;
            //        PlayerSave.singleton.potLimit = 51200;
            //    }
            //    break;

            //case 75:
            //    {
            //        PlayerSave.singleton.chalLimit = 9600;
            //        PlayerSave.singleton.potLimit = 76800;
            //    }
            //    break;

            //case 100:
            //    {
            //        PlayerSave.singleton.chalLimit = 12800;
            //        PlayerSave.singleton.potLimit = 102400;
            //    }
            //    break;

            

            default:
                {
                    PlayerSave.singleton.chalLimit = 1.28;
                    PlayerSave.singleton.potLimit = 10.24;
                    SetChaalLimitRefresh(_chaalLimit);
                }
                break;

                
        }
        
    }
    private void CreateOrJoinGame(eTable eTableRoom)
    {



        if (StaticValues.CurrentVersion == StaticValues.version)
        {
            if (StaticValues.ismaintenance == 0)
            {
                Invoke("ForceQuit", 5);
                panelLoad.SetActive(true);


                PlayerSave.singleton.currentTable = eTableRoom;


                JoinRoomAndStartGame();
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
    public void CreatePrivateRoom(eTable nameRoom)
    {

        if (StaticValues.CurrentVersion == StaticValues.version)
        {
            if (StaticValues.ismaintenance == 0)
            {
                PlayerSave.singleton.chaalTime = 25f;
                Invoke("ForceQuit", 5);
                panelLoad.SetActive(true);

                PlayerSave.singleton.currentTable = nameRoom;

				if (!PhotonNetwork.IsConnected)
				{
					isOnceList = false;
					isPrivateOnceList=false;
					isPrivateCreateOnceList=true;
					panelLoad.SetActive(true);
					PhotonNetwork.ConnectUsingSettings();
					PhotonNetwork.GameVersion = gameVersion;
					PhotonNetwork.AuthValues = new AuthenticationValues();
					PhotonNetwork.AuthValues.UserId = PlayerSave.singleton.GetUserId();
				}
				else
				{
					isPrivateCreateOnceList=false;
	                RoomOptions roomOptions = new RoomOptions();
	                roomOptions.MaxPlayers = 5;
	                string roomName = "";
	                for (int i = 0; i < 8; i++)
	                {
	                    roomName = roomName + UnityEngine.Random.Range(0, 10);
	                }
	                roomOptions.CustomRoomPropertiesForLobby = new string[] { "pc" };
	                roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {
	                    {"pc", roomName.ToString()+";"+PlayerSave.singleton.bootAmount.ToString()}
	                        };
	                roomOptions.PlayerTtl = 0;
	                roomOptions.IsVisible = true;
	                roomOptions.IsOpen = true;
	                roomOptions.CleanupCacheOnLeave = true;
	                roomOptions.PublishUserId = true;
	                roomOptions.EmptyRoomTtl = 0;
	                PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
				}
            }
            else
            {
                //PlayerSave.singleton.ShowErrorMessage("Game is on maintenance!!!");
				isPrivateCreateOnceList=false;
                AppManager.VIEW_CONTROLLER.HideAllScreen();
                AppManager.VIEW_CONTROLLER.ShowMaintenance();
            }

        }
        else
        {
            //PlayerSave.singleton.ShowErrorMessage("Please update apk");
			isPrivateCreateOnceList=false;
            AppManager.VIEW_CONTROLLER.HideAllScreen();
            AppManager.VIEW_CONTROLLER.ShowVersion();

        }
       
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("OnCreateRoomFailed message " + message);
    }
    private string roomCode = "";
    public void OnRoomShareClicked()
    {
        ClickSound();
        string share = " Join me in Teen Patti.My private room code is " + roomCode;
#if UNITY_ANDROID
        // Get the required Intent and UnityPlayer classes.
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        // Construct the intent.
        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent");
        intent.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        intent.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), share.ToString());
        intent.Call<AndroidJavaObject>("setType", "text/plain");

        // Display the chooser.
        AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intent, "Share");
        currentActivity.Call("startActivity", chooser);
#endif
    }
    private void OnJoinRoomCode(string _input)
    {
        PlayerSave.singleton.RoomCodeName = _input.Replace(" ", string.Empty);


    }
    public void LeavePrivateRoomBalance(int _whichOne)
    {
        PlayerSave.singleton.RoomCodeName = "";
        if (PhotonNetwork.InRoom)
        {
            panelLoad.SetActive(false);
            PhotonNetwork.LeaveRoom(false);
            if (_whichOne == 0)
            {
                //PlayerSave.singleton.ShowErrorMessage("You left the room beacuse of insufficient balance!!!");
            }
            else
            {
                //PlayerSave.singleton.ShowErrorMessage("You left the room because of insufficient balance!!!");
            }
        }
    }
    public void LeavePrivateRoom()
    {
        PlayerSave.singleton.RoomCodeName = "";
        if (PhotonNetwork.InRoom)
        {
            panelLoad.SetActive(false);
            PhotonNetwork.LeaveRoom(false);
            //PlayerSave.singleton.ShowErrorMessage("You left the room!!!");
        }
    }
    public void OpenEnterCode()
    {
        if (StaticValues.CurrentVersion == StaticValues.version)
        {
            if (StaticValues.ismaintenance == 0)
            {
                panelLoad.SetActive(false);
                PanelEnterCode.SetActive(true);
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
    public void NewJoinPrivateRoom()
    {
        if (!string.IsNullOrEmpty(PlayerSave.singleton.RoomCodeName))
        {
            if (StaticValues.CurrentVersion == StaticValues.version)
            {
                if (StaticValues.ismaintenance == 0)
                {
					if (!PhotonNetwork.IsConnected)
					{
						isOnceList = false;
						isPrivateOnceList = true;
						isPrivateCreateOnceList=false;
						panelLoad.SetActive(true);
						PhotonNetwork.ConnectUsingSettings();
						PhotonNetwork.GameVersion = gameVersion;
						PhotonNetwork.AuthValues = new AuthenticationValues();
						PhotonNetwork.AuthValues.UserId = PlayerSave.singleton.GetUserId();
					}
					else
					{
						isOnceList = false;
						isPrivateOnceList=false;
						isPrivateCreateOnceList=false;
	                    PlayerSave.singleton.currentTable = eTable.Private;
	                    bootAmount = BootAmount.Private;
	                    Invoke("ForceQuit", 5f);
	                    panelLoad.SetActive(true);
	                    PhotonNetwork.JoinRoom(PlayerSave.singleton.RoomCodeName);
					}
                }
                else
                {
                    //PlayerSave.singleton.ShowErrorMessage("Game is on maintenance!!!");
					isPrivateOnceList=false;
					isPrivateCreateOnceList=false;
                    AppManager.VIEW_CONTROLLER.HideAllScreen();
                    AppManager.VIEW_CONTROLLER.ShowMaintenance();
                }

            }
            else
            {
                //PlayerSave.singleton.ShowErrorMessage("Please update apk");
				isPrivateOnceList=false;
				isPrivateCreateOnceList=false;
                AppManager.VIEW_CONTROLLER.HideAllScreen();
                AppManager.VIEW_CONTROLLER.ShowVersion();

            }
            
        }
        else
        {
			isPrivateOnceList=false;
			isPrivateCreateOnceList=false;
            //PlayerSave.singleton.ShowErrorMessage("Please enter room code!!!");
        }
    }
   
    private void CreateOrJoinPrivateGame(eTable nameRoom)
    {
        PlayerSave.singleton.chaalTime = 25f;
        Invoke("ForceQuit", 5);
        panelLoad.SetActive(true);

        PlayerSave.singleton.currentTable = nameRoom;


        //if (!PanelCreateOrJoin.activeSelf)
        //{
        //    panelLoad.SetActive(false);
        //    PanelCreateOrJoin.SetActive(true);

        //}
    }

    private void ForceQuit()
    {
        panelLoad.SetActive(false);
    }

   

    public override void OnDisconnected(DisconnectCause disconnectCause)
    {
       
        base.OnDisconnected(disconnectCause);
       if(panelLoad) panelLoad.SetActive(false);
    }

    public override void OnJoinRandomFailed(short msg, string msg2)
    {
       
        base.OnJoinRandomFailed(msg,msg2);
        
        IsCreateRoom(PlayerSave.singleton.currentTable.ToString());
       
    }
    
    public override void OnJoinRoomFailed(short msg, string msg2)
    {
        
        base.OnJoinRoomFailed(msg, msg2);
        Debug.Log("OnJoinRoomFailed Room " + msg + " msg2 "+ msg2);
        if (PlayerSave.singleton.currentTable == eTable.Private)
        {
            PopupMessage popupMessage = new PopupMessage();
            popupMessage.Title = "Error";
            popupMessage.Message = msg2;
            AppManager.VIEW_CONTROLLER.ShowPopupMessage(popupMessage);
        }
        CancelInvoke("ForceQuit");
        if (panelLoad) panelLoad.SetActive(false);
    }
    private int _RandomBot = 0;
    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom Room "+ bootAmount);

       
        CancelInvoke("ForceQuit");
        PlayerSave.singleton.SaveMoneyBeforeGame();
        base.OnCreatedRoom();
        if (PhotonNetwork.InRoom)
        {
            PlayerSave.FullRoomName = PhotonNetwork.CurrentRoom.Name;
            
        }

        panelLoad.SetActive(false);
        //PhotonNetwork.LoadLevel(2);
        if (PlayerSave.singleton.currentTable == eTable.Standard)
        {
            if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
            {
                _RandomBot = UnityEngine.Random.Range(0, 10);
                //_RandomBot = 1;
                Debug.Log("_RandomBot ......................." + _RandomBot);
                if (_RandomBot > 5)
                {
                    PlayerSave.singleton.isCreatedRoom = true;
                    PlayerSave.singleton.isJoinedRoom = false;
                    PlayerSave.singleton._howManyBot = 0;
                    int _howManyBotInRoom= UnityEngine.Random.Range(1, 4);
                    //_howManyBotInRoom = 1;
                    Debug.Log("_howManyBotInRoom ......................." + _howManyBotInRoom);
                    PlayerSave.singleton.CallBotEnter(PlayerSave.singleton.bootAmount, PlayerSave.singleton.chalLimit, PlayerSave.singleton.potLimit, PlayerSave.singleton._TableId, "P", OnBotEnterResponseStart, "", _howManyBotInRoom);
                }
                else
                {
                    PlayerSave.singleton.isCreatedRoom = true;
                    PlayerSave.singleton.isJoinedRoom = false;
                    PlayerSave.singleton._howManyBot = 0;
                    BotEnterResponse botEnterResponse = new BotEnterResponse();
                    botEnterResponse.status = "500";
                    OnBotEnterResponseStart(botEnterResponse);
                }
            }
        }
    }
    private void OnBotEnterResponseStart(BotEnterResponse botEnterResponse)
    {
        Debug.Log("OnBotEnterResponseStart ");
        if (botEnterResponse != null)
        {
            if (botEnterResponse.status.Contains("200"))
            {
                if (botEnterResponse.data != null)
                {
                    if (botEnterResponse.data.Length > 0)
                    {
                        StaticValues.isbotStatus = true;
                        PlayerSave.singleton.botsServerData = new List<UserInfo>();
                        for (int i = 0; i < botEnterResponse.data.Length; i++)
                        {
                            if (botEnterResponse.data[i].totalamount >= PlayerSave.singleton.bootAmount)
                            {
                                PlayerSave.singleton.botsServerData.Add(botEnterResponse.data[i]);
                            }
                        }
                        PlayerSave.singleton._howManyBot = PlayerSave.singleton.botsServerData.Count;
                    }
                    else
                    {
                        Debug.Log("botEnterResponse server data length " + botEnterResponse.data.Length);
                        PlayerSave.singleton.botsServerData = new List<UserInfo>();
                        StaticValues.isbotStatus = false;
                        PlayerSave.singleton._howManyBot = 0;
                    }
                }
                else
                {
                    PlayerSave.singleton.botsServerData = new List<UserInfo>();
                    StaticValues.isbotStatus = false;
                    PlayerSave.singleton._howManyBot = 0;
                }
            }
        }
        else
        {
            PlayerSave.singleton.botsServerData = new List<UserInfo>();
            StaticValues.isbotStatus = false;
            PlayerSave.singleton._howManyBot = 0;
        }
        if(PlayerSave.singleton.isCreatedRoom && PlayerSave.singleton.isJoinedRoom)
        {
            //Debug.Log("TryToJoinGameAfterBotResponse before ");

            PhotonNetwork.LoadLevel(2);
        }
        else
        {
            //Debug.Log("TryToJoinGameAfterBotResponse in ");
            StopCoroutine(TryToJoinGameAfterBotResponse());
            StartCoroutine(TryToJoinGameAfterBotResponse());
        }

    }
    public IEnumerator TryToJoinGameAfterBotResponse()
    {
        while (true)
        {
            if (PlayerSave.singleton.isCreatedRoom && PlayerSave.singleton.isJoinedRoom && PhotonNetwork.IsMasterClient)
            {
               // Debug.Log("TryToJoinGameAfterBotResponse ");
                PhotonNetwork.LoadLevel(2);

                StopCoroutine(TryToJoinGameAfterBotResponse());
                break;
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
  
    public override void OnJoinedRoom()
    {
       //Debug.Log("OnJoined Room "+PhotonNetwork.IsMasterClient);
       
        CancelInvoke("ForceQuit");
        PlayerSave.singleton.SaveMoneyBeforeGame();
        base.OnJoinedRoom();

        if (PhotonNetwork.InRoom)
        {
            PlayerSave.FullRoomName = PhotonNetwork.CurrentRoom.Name;
        }
       
        panelLoad.SetActive(true);
        if (PlayerSave.singleton.currentTable == eTable.Private)
        {

            string[] message = PhotonNetwork.CurrentRoom.CustomProperties["pc"].ToString().Split(';');
            PlayerSave.singleton.isCreatedRoom = false;
            PlayerSave.singleton.isJoinedRoom = false;
            //Debug.Log("message 1..." + message[1]);
            if (message != null)
            {
                if (message.Length > 1)
                {
                    try
                    {
                        PlayerSave.singleton.bootAmount = double.Parse(message[1]);
                        SetChaalLimit(PlayerSave.singleton.bootAmount);
                        if (PlayerSave.singleton.bootAmount <= (int)PlayerSave.singleton.GetCurrentMoney())
                        {
                            if ((PlayerSave.singleton.potLimit / 2) <= (int)PlayerSave.singleton.GetCurrentMoney())
                            {
                                PhotonNetwork.LoadLevel(2);
                            }
                            else
                            {
                                panelLoad.SetActive(false);
                                LeavePrivateRoomBalance(0);
                                //if (Panel_DontHaveMoney) Panel_DontHaveMoney.SetActive(true);
                                //if (Text_DontHaveMoney) Text_DontHaveMoney.text = "You don't have enough money to" + "\n" + " play game." + "\n" + "Please add cash to play the game.";
                                //if (TitleText_DontHaveMoney) TitleText_DontHaveMoney.text = "Out of Pot Limit!!!";

                                if (MainMenuUI.menuUI != null)
                                {
                                    MainMenuUI.menuUI.OnAddCashbutton();
                                }
                            }
                        }
                        else
                        {
                            panelLoad.SetActive(false);
                            LeavePrivateRoomBalance(0);
                            //if (Panel_DontHaveMoney) Panel_DontHaveMoney.SetActive(true);
                            //if (Text_DontHaveMoney) Text_DontHaveMoney.text = "You don't have enough money to" + "\n" + " play game." + "\n" + "Please add cash to play the game.";
                            //if (TitleText_DontHaveMoney) TitleText_DontHaveMoney.text = "Out of Money!!!";

                            if (MainMenuUI.menuUI != null)
                            {
                                MainMenuUI.menuUI.OnAddCashbutton();
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        //PlayerSave.singleton.ShowErrorMessage(e.ToString());
                        panelLoad.SetActive(false);
                    }

                   
                }
                else
                {
                    //PlayerSave.singleton.ShowErrorMessage("Room Not Exists");
                    panelLoad.SetActive(false);
                }

            }
            else
            {
                //PlayerSave.singleton.ShowErrorMessage("Room Not Exists");
                panelLoad.SetActive(false);
            }
            if (PanelEnterCode) PanelEnterCode.SetActive(false);
        }
        else
        {
            if (PlayerSave.singleton.currentTable == eTable.Standard)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PlayerSave.singleton.isCreatedRoom = true;
                    PlayerSave.singleton.isJoinedRoom = true;
                    //Debug.Log("OnJoinedRoom in ");
                }
                else
                {
                    PlayerSave.singleton.isCreatedRoom = false;
                    PlayerSave.singleton.isJoinedRoom = false;
                    PhotonNetwork.LoadLevel(2);
                }
            }
            else
            {
                PlayerSave.singleton.isCreatedRoom = false;
                PlayerSave.singleton.isJoinedRoom = false;
                PhotonNetwork.LoadLevel(2);
            }
        }

    }
    
    public bool CheckRoomPassword(RoomInfo checkingRoom,string pass)
    {
        if (checkingRoom == null)
        {
            //Debug.Log("Checking room is not assigned more!");
            return false;
        }

        if (checkingRoom.PlayerCount <= checkingRoom.MaxPlayers)
        {
             
                
                    PlayerSave.FullRoomName = checkingRoom.Name;
                    PhotonNetwork.JoinRoom(checkingRoom.Name);
               
           
            return true;
        }
        else
        {
            return false;
        }
    }
    public void JoinRoomAndStartGame()
    {

        if (!PhotonNetwork.IsConnected)
        {
            isOnceList = true;
			isPrivateOnceList=false;
			isPrivateCreateOnceList=false;
            panelLoad.SetActive(true);
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.AuthValues = new AuthenticationValues();
            PhotonNetwork.AuthValues.UserId = PlayerSave.singleton.GetUserId();
        }
        else
        {
            isOnceList = false;
			isPrivateOnceList=false;
			isPrivateCreateOnceList=false;
            ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {
                   {"m", PlayerSave.singleton.currentTable.ToString()+PlayerSave.singleton.bootAmount.ToString()}
                     };

            PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0);
        }

    }
    public void IsCreateRoom(string roomName)
    {
                
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 5;
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "m", "v" };
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {
                    {"m", roomName.ToString()+PlayerSave.singleton.bootAmount.ToString()}
                        };
        roomOptions.PlayerTtl = 0;// 45000;
        //roomOptions.PlayerTtl = 0;
        roomOptions.EmptyRoomTtl = 0;
        roomOptions.PublishUserId = true;
        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }
   
    
    [SerializeField]
    private List<RoomInfo> roomList;
    public override void OnRoomListUpdate(List<RoomInfo> roomInfos)
    {
        // Debug.Log("Room list received." );

        roomList = roomInfos;
    }
    public void RefreshRoomListings()
    {
        
    }
    public void ClickSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource != null)
            {
                audioSource.clip = click;
                audioSource.Play();
            }
        }
    }
    public void ClickCloseSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource != null)
            {
                audioSource.clip = clickClose;
                audioSource.Play();
            }
        }
    }
    public void OnSettingsShareClicked()
    {
        ClickSound();
        string share = " Join me in TeenPatti .Download TeenPatti from : " + "https://www.google.com";
#if UNITY_ANDROID
        // Get the required Intent and UnityPlayer classes.
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        // Construct the intent.
        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent");
        intent.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        intent.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), share.ToString());
        intent.Call<AndroidJavaObject>("setType", "text/plain");

        // Display the chooser.
        AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intent, "Share");
        currentActivity.Call("startActivity", chooser);
#endif
    }
    public void OnSettingsContactUsButton()
    {
        ClickSound();
        SendEmail();
    }
    void SendEmail()
    {
        string email = "support@test.com";
        string subject = MyEscapeURL("Teen Patti Feedback " + PlayerSave.singleton.GetUserName());
        string body = MyEscapeURL("Please\r\nprovide your feedback here...");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }
    string MyEscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }
    void CreateOrJoinPrivateGame(eTable nameRoom, int totalPlayer,int MaxRoom,string TableName,string uniqueId)
    {
        totalPlayer = Mathf.Clamp(totalPlayer, 2, 5);
        

        Invoke("ForceQuit", 5);
        if (panelLoad) panelLoad.SetActive(true);
       
        

        

        PlayerSave.singleton.currentTable = nameRoom;
        for (int i = 0; i < roomList.Count; i++)
        {
            if (CheckPrivateRoomPassword(roomList[i], PlayerSave.singleton.bootAmount, PlayerSave.singleton.currentTable.ToString(), uniqueId))
            {
                return;
            }
        }
        IsCreatePrivateRoom(nameRoom.ToString(),uniqueId, totalPlayer);
    }
    public void IsCreatePrivateRoom(string roomName,string uniqueId, int totalPlayer)
    {
        string NewRoomName = string.Format("[PRIVATE]-{0}-{1}-{2}-{3}", roomName, UnityEngine.Random.Range(1000, 9999), UnityEngine.Random.Range(111111, 999999), UnityEngine.Random.Range(11111, 99999));
        //Debug.Log("NewRoomName " + NewRoomName);
        //Save Room properties for load in room
        ExitGames.Client.Photon.Hashtable roomOption = new ExitGames.Client.Photon.Hashtable();
        roomOption[PropertiesKeys.RoomPassword] = PlayerSave.singleton.GetDistributionId();
        roomOption[PropertiesKeys.RoomAmount] = PlayerSave.singleton.bootAmount;
        roomOption[PropertiesKeys.RoomName] = roomName;

        string[] properties = new string[3];
        properties[0] = PropertiesKeys.RoomPassword;
        properties[1] = PropertiesKeys.RoomAmount;
        properties[2] = PropertiesKeys.RoomName;

        PlayerSave.FullRoomName = NewRoomName;
        //PhotonNetwork.CreateRoom(nameRoom.ToString() + rooms.Length + 1, roomOption, TypedLobby.Default);
        PhotonNetwork.CreateRoom(NewRoomName, new RoomOptions()
        {
            MaxPlayers = (byte)5,
            IsVisible = true,
            IsOpen = true,
            CustomRoomProperties = roomOption,
            CleanupCacheOnLeave = true,
            CustomRoomPropertiesForLobby = properties,
            PublishUserId = true,
            EmptyRoomTtl = 0,
            PlayerTtl=1000
        }, null);
    }
    public bool CheckPrivateRoomPassword(RoomInfo checkingRoom, double _bootAmount, string _roomName,string uniqueId)
    {
        if (checkingRoom == null)
        {
            //Debug.Log("Checking room is not assigned more!");
            return false;
        }
        //Debug.Log("checkingRoom.CustomProperties[PropertiesKeys.RoomPassword] " + checkingRoom.CustomProperties[PropertiesKeys.RoomPassword]);
        //Debug.Log("checkingRoom.CustomProperties[PropertiesKeys.RoomAmount] " + checkingRoom.CustomProperties[PropertiesKeys.RoomAmount]);
        if (checkingRoom.CustomProperties[PropertiesKeys.RoomAmount].Equals(_bootAmount) && checkingRoom.PlayerCount <= checkingRoom.MaxPlayers)
        {

            if (checkingRoom.CustomProperties[PropertiesKeys.RoomName].Equals(_roomName))
            {

                if (checkingRoom.CustomProperties[PropertiesKeys.RoomPassword].Equals(uniqueId))
                {
                    PlayerSave.FullRoomName = checkingRoom.Name;
                    PhotonNetwork.JoinRoom(checkingRoom.Name);
                    return true;
                }
                else
                {
                    return false;
                }

               
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public void OnRefreshUIPotChaal()
    {
        if(ChipsText!=null)
        {
            for(int i=0; i<ChipsText.Length;i++)
            {
                if (StaticValues.BootAmount.Length>0 && StaticValues.BootAmount.Length <= ChipsText.Length)
                {
                    ChipsText[i].text = StaticValues.BootAmount[i] + "\nChips";
                }
            }
        }
    }
    private int _SelectedIndex = 0;
    public void SetBootAmount2(int _Index)
    {
        if (_Index >= 0 && _Index < 12)
        {
            _SelectedIndex = _Index;
            SetBootAmount(StaticValues.BootAmount[_Index]);
        }
        else
        {
            //PlayerSave.singleton.ShowErrorMessage("Out of Index Exception");
        }
    }
    public void SetChaalLimitRefresh(double _chaalLimit)
    {
        switch (_chaalLimit)
        {
            case 0.01:
                {
                    if (StaticValues.ChaalLimit.Length > 0 && StaticValues.PotLimit.Length > 0)
                    {
                        PlayerSave.singleton.chalLimit = StaticValues.ChaalLimit[0];
                        PlayerSave.singleton.potLimit = StaticValues.PotLimit[0];
                    }
                }
                break;

            case 0.05:
                {
                    if (StaticValues.ChaalLimit.Length > 1 && StaticValues.PotLimit.Length > 1)
                    {
                        PlayerSave.singleton.chalLimit = StaticValues.ChaalLimit[1];
                        PlayerSave.singleton.potLimit = StaticValues.PotLimit[1];
                    }
                }
                break;



            case 0.1:
                {
                    if (StaticValues.ChaalLimit.Length > 2 && StaticValues.PotLimit.Length > 2)
                    {
                        PlayerSave.singleton.chalLimit = StaticValues.ChaalLimit[2];
                        PlayerSave.singleton.potLimit = StaticValues.PotLimit[2];
                    }
                }
                break;

            case 0.25:
                {
                    if (StaticValues.ChaalLimit.Length > 3 && StaticValues.PotLimit.Length > 3)
                    {
                        PlayerSave.singleton.chalLimit = StaticValues.ChaalLimit[3];
                        PlayerSave.singleton.potLimit = StaticValues.PotLimit[3];
                    }
                }
                break;

            case 0.5:
                {
                    if (StaticValues.ChaalLimit.Length > 4 && StaticValues.PotLimit.Length > 4)
                    {
                        PlayerSave.singleton.chalLimit = StaticValues.ChaalLimit[4];
                        PlayerSave.singleton.potLimit = StaticValues.PotLimit[4];
                    }
                }
                break;

            case 1:
                {
                    if (StaticValues.ChaalLimit.Length > 5 && StaticValues.PotLimit.Length > 5)
                    {
                        PlayerSave.singleton.chalLimit = StaticValues.ChaalLimit[5];
                        PlayerSave.singleton.potLimit = StaticValues.PotLimit[5];
                    }
                }
                break;

         

            case 2:
                {
                    if (StaticValues.ChaalLimit.Length > 6 && StaticValues.PotLimit.Length > 6)
                    {
                        PlayerSave.singleton.chalLimit = StaticValues.ChaalLimit[6];
                        PlayerSave.singleton.potLimit = StaticValues.PotLimit[6];
                    }
                }
                break;
         

            case 3:
                {
                    if (StaticValues.ChaalLimit.Length > 7 && StaticValues.PotLimit.Length > 7)
                    {
                        PlayerSave.singleton.chalLimit = StaticValues.ChaalLimit[7];
                        PlayerSave.singleton.potLimit = StaticValues.PotLimit[7];
                    }
                }
                break;

      

            case 4:
                {
                    if (StaticValues.ChaalLimit.Length > 8 && StaticValues.PotLimit.Length > 8)
                    {
                        PlayerSave.singleton.chalLimit = StaticValues.ChaalLimit[8];
                        PlayerSave.singleton.potLimit = StaticValues.PotLimit[8];
                    }
                }
                break;

            case 5:
                {
                    if (StaticValues.ChaalLimit.Length > 9 && StaticValues.PotLimit.Length > 9)
                    {
                        PlayerSave.singleton.chalLimit = StaticValues.ChaalLimit[9];
                        PlayerSave.singleton.potLimit = StaticValues.PotLimit[9];
                    }
                }
                break;

          

            case 10:
                {
                    if (StaticValues.ChaalLimit.Length > 10 && StaticValues.PotLimit.Length > 10)
                    {
                        PlayerSave.singleton.chalLimit = StaticValues.ChaalLimit[10];
                        PlayerSave.singleton.potLimit = StaticValues.PotLimit[10];
                    }
                }
                break;

          
            case 20:
                {
                    if (StaticValues.ChaalLimit.Length > 11 && StaticValues.PotLimit.Length > 11)
                    {
                        PlayerSave.singleton.chalLimit = StaticValues.ChaalLimit[11];
                        PlayerSave.singleton.potLimit = StaticValues.PotLimit[11];
                    }
                }
                break;

          
          
            default:
                {
                    PlayerSave.singleton.chalLimit = 1.28;
                    PlayerSave.singleton.potLimit = 10.24;
                    if (_SelectedIndex >= 0 && _SelectedIndex < 12)
                    {
                        PlayerSave.singleton.chalLimit = StaticValues.ChaalLimit[_SelectedIndex];
                        PlayerSave.singleton.potLimit = StaticValues.PotLimit[_SelectedIndex];
                    }
                }
                break;
        }

    }

    private char MyValidate(string input, int charIndex,char addedChar)
    {
     
        char temp = addedChar;
        char tempLastChar = '\0';
        if (input != "")
        {
            tempLastChar = input.ToCharArray()[input.Length - 1];
        }
        //Check if added care is a permitted character.
        addedChar = addedChar.ToString().ToLower().ToCharArray()[0];        //Set the Char input to lower to minimize checks.
        if (addedChar != 'q' && addedChar != 'w' && addedChar != 'e' && addedChar != 'r' && addedChar != 't' && addedChar != 'y'
            && addedChar != 'u' && addedChar != 'i' && addedChar != 'o' && addedChar != 'p' && addedChar != 'a' && addedChar != 's'
            && addedChar != 'd' && addedChar != 'f' && addedChar != 'g' && addedChar != 'h' && addedChar != 'j' && addedChar != 'k'
            && addedChar != 'l' && addedChar != 'z' && addedChar != 'x' && addedChar != 'c' && addedChar != 'v' && addedChar != 'b'
            && addedChar != 'n' && addedChar != 'm'  && addedChar != '@' && addedChar != '0' && addedChar != '1' && addedChar != '2' && addedChar != '3'
            && addedChar != '4' && addedChar != '5' && addedChar != '6' && addedChar != '7' && addedChar != '8' && addedChar != '9')
        {
            temp = '\0';
        }
        else
        {
            
            //Do not allow the name to begin with a number or @
            if (charIndex == 0 && addedChar == '@' || charIndex == 0 && addedChar == '0' || charIndex == 0 && addedChar == '1' || charIndex == 0 && addedChar == '2' || charIndex == 0 && addedChar == '3'
                || charIndex == 0 && addedChar == '4' || charIndex == 0 && addedChar == '5' || charIndex == 0 && addedChar == '6' || charIndex == 0 && addedChar == '7' || charIndex == 0 && addedChar == '8'
                || charIndex == 0 && addedChar == '9')
            {
                addedChar = '\0';
            }

            //Do not allow more than one apostrophe per name.
            if (addedChar == '@')
            {
                if (input.Contains("@"))
                {
                    addedChar = '\0';
                }
            }
            temp = addedChar;        //Set temp to the entered char

            ////If the text is selected, then a lower case character is input, the selected text is deleted and replaced with the lower case input.
            ////The following code corrects the lower case to upper case once an additional character is input.
            //if (charIndex == 1)
            //{
            //    tempLastChar = tempLastChar.ToString().ToLower().ToCharArray()[0];
            //    if (_userNameInputField.isFocused)
            //    {
            //        _userNameInputField.text = tempLastChar.ToString();
            //    }
                
            //}
            //if (input.Length >= 2)
            //{
            //    if (input.ToCharArray()[charIndex - 2] == '@')
            //    {
            //        tempLastChar = tempLastChar.ToString().ToLower().ToCharArray()[0];
            //        input = input.Remove(input.Length - 1, 1);
            //        input = input.Insert(input.Length, tempLastChar.ToString());
            //        if (_userNameInputField.isFocused)
            //        {
            //            _userNameInputField.text = input;
            //        }
                    
            //    }
            //}
        }
        return temp;
       
    }
    public void OnUserNameSubmitButton()
    {
        _userNameInputField.text = _userNameInputField.text.Trim();
        if (!string.IsNullOrEmpty(_userNameInputField.text))
        {
            if (_userNameInputField.text.Length >= 4 && _userNameInputField.text.Length <=12)
            {
                panelLoad.SetActive(true);
                PlayerSave.singleton.CallUserNamePost(_userNameInputField.text.ToLowerInvariant(), OnUserNameResponse);
            }
            else
            {
                PlayerSave.singleton.ShowErrorMessage("Please enter your user name in Range (4-12) characters before proceed!!!");
            }
        }
        else
        {
            PlayerSave.singleton.ShowErrorMessage("Please enter your user name before proceed!!!");
        }
    }
    public void OnUserNameResponse(UserNameResult _userNameResult)
    {
        panelLoad.SetActive(false);
        if(_userNameResult != null)
        {
            if(_userNameResult.status.Equals("200"))
            {
                //PlayerSave.singleton.ShowErrorMessage(_userNameResult.message);
                if (PanelUserName) PanelUserName.SetActive(false);
                if (PanelPopUpBonus) PanelPopUpBonus.SetActive(false);
                if (_userNameResult.data != null)
                {
                    StaticValues.UserNameValue = _userNameResult.data.username;
                    if (!string.IsNullOrEmpty(_userNameResult.data.username))
                    {
                        StaticValues.MyReferralCode = _userNameResult.data.username;
                    }
                }
                else
                {
                    StaticValues.UserNameValue = _userNameInputField.text;
                    StaticValues.MyReferralCode = StaticValues.UserNameValue;
                }
                PlayerSave.singleton.SaveUserName(StaticValues.UserNameValue);
                PlayerSave.singleton.SaveDistributionId(StaticValues.MyReferralCode);

                if(_userNameResult.data2!=null)
                {
                    if(!string.IsNullOrEmpty(_userNameResult.data2.title))
                    {
                        StaticValues.Btitle = _userNameResult.data2.title;
                        if (BonusTitle) BonusTitle.text = StaticValues.Btitle;
                    }
                    if (!string.IsNullOrEmpty(_userNameResult.data2.description))
                    {
                        StaticValues.Bdescription = _userNameResult.data2.description;
                        if (PanelPopUpBonus) PanelPopUpBonus.SetActive(true);
                        if (BonusDescription) BonusDescription.text = StaticValues.Bdescription;
                    }
                }
            }
            else
            {
                PlayerSave.singleton.ShowErrorMessage(_userNameResult.message);
            }
        }
        else
        {
            PlayerSave.singleton.ShowErrorMessage(_userNameResult.message);
        }
    }

}
