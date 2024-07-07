using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun;
using Photon.Realtime;

[Serializable]
public struct TableInfo
{   
    public double blindLimit, chalLimit, potLimit, startBoot;
}

[Serializable]
public struct ManagerInfo
{
    public int currentPlayerStep,currentPlayerStepID,currentChance,currentBotHand;
    public double totalPot, currentStake;
    public bool isStartedGame, isGivingCards, isNextGameTimer,isCheckOnWin;
    public int isGivingBoot, isNextGameTimerValue;

    public int WhichPlayerChanceState;
    // public string textChat;
    public int playerIdStartedSideShow;
  
  	public bool isChaalPlayer;
    public bool isChaalPlayerUpdateBoot;
    #region FOR ANDAR BAHAR

    public int CurrentGameStep;
    public CardData JokerCard;
    public List<CardData> AndarCards;
    public List<CardData> BaharCards;

    #endregion
	//public PlayerData[] playerDatas;
    public double isChaalPlayerUpdateBootAmount;
    public bool isSideShowReceiver;
    public int playerIdRecievedSideShow;
    public string playerIdRecievedSideShowDeviceId;
    public string playerIdRecievedSideShowName;
    public string AllPhotonIds;
    public int GameRoom_2;
    public string CurrentRoomID;
    public string CurrentDIcon;
    

    [Header("Properties for what is currently going on/Which player chance is in current state")]
    public string IsCurrentIndex;
    public bool isCurrentSideShow;
    public bool isCurrentSentSideShow;
    public bool IsCurrentPacked;
    public string IsCurrentNamePlayer;
    public string _IsCurrentMobileNumber;
    public bool IsCurrentBot;
    public bool IsCurrentSeenCard;
    public bool IsCurrentHost;
    public bool IsCurrentPlayerDisconnected;
    public float IsCurrentTimer;
    public bool IsCurrentLocalPlayer;
    public bool IsCurrentPlayerDestroyed;
    public bool IsCurrentPlayerPause;
}

public class TeenPattiPhoton : MonoBehaviourPunCallbacks, IOnEventCallback,IConnectionCallbacks, IInRoomCallbacks, IPunObservable
{
    private DeckManager deckManager;
    public List<PlayerManagerPun> players = new List<PlayerManagerPun>();

    public RectTransform[] playersUiPositionCollection;
    public PlayerUI[] playersUiCollection;

    private const float timeStep = 1f;
    public ManagerInfo managerInfo;
    private TableInfo tableInfo; // need init on start
    public eTable typeTable;
    // private PlayerManagerPun playerStartedSideShow;

	float timer;
    #region BOT

    [SerializeField] public List<PlayerData> botsData;
    List<string> NewbotsData;

    [SerializeField] public List<UserInfo> botsServerData;
    public int botIndex = -1;
    public int RandomBot = 2;
    #endregion

    public bool GetBotFromServer = false;
    private bool isDebug = true;
    // Use this for initialization

    public override void OnEnable()
    {
        base.OnEnable();

        if (PhotonNetwork.IsMasterClient)
        {
            managerInfo.CurrentRoomID = System.Guid.NewGuid().ToString();
            PlayerSave.NewRoomName = managerInfo.CurrentRoomID;
            managerInfo.currentChance = -1;
        }
        if (PhotonNetwork.InRoom)
        {
            PlayerSave.FullRoomName = PhotonNetwork.CurrentRoom.Name;


        }



        _timerRange = Random.Range(10f, 20f);
        PhotonNetwork.NetworkingClient.StateChanged += this.OnStateChanged;
        if (isDebug)
        {
            //Debug.Log("<color=red>OnEnable teen  player...................................................</color>");
        }
    }

    private void OnStateChanged(ClientState arg1, ClientState arg2)
    {
        if (isDebug)
        {
            Debug.Log("<color=red>OnStateChanged teen  player...................................................</color>" + arg1.ToString() + "arg2 " + arg2.ToString());
        }

        if(arg1.ToString().Equals("Joined") && arg2.ToString().Equals("Disconnecting"))
        {
            CallOnDestroy2(!string.IsNullOrEmpty(PlayerSave.singleton.GetUserName()) ? PlayerSave.singleton.GetUserName() : PlayerSave.singleton.GetCurrentNamey(), PlayerSave.singleton.GetUserId(), 5);
        }
        if (arg1.ToString().Equals("Authenticating") && arg2.ToString().Equals("Joining"))
        {
            CallOnDestroy2(!string.IsNullOrEmpty(PlayerSave.singleton.GetUserName()) ? PlayerSave.singleton.GetUserName() : PlayerSave.singleton.GetCurrentNamey(), PlayerSave.singleton.GetUserId(), 5);
        }
        if (arg1.ToString().Equals("Authenticating") && arg2.ToString().Equals("ConnectedToMasterServer"))
        {
            CallOnDestroy2(!string.IsNullOrEmpty(PlayerSave.singleton.GetUserName()) ? PlayerSave.singleton.GetUserName() : PlayerSave.singleton.GetCurrentNamey(), PlayerSave.singleton.GetUserId(), 5);
        }
    }


    public override void OnDisable()
    {
        CallOnDestroy2(!string.IsNullOrEmpty(PlayerSave.singleton.GetUserName()) ? PlayerSave.singleton.GetUserName() : PlayerSave.singleton.GetCurrentNamey(), PlayerSave.singleton.GetUserId(), 4);
        base.OnDisable();
        PhotonNetwork.NetworkingClient.StateChanged -= this.OnStateChanged;
        if (isDebug)
        {
            //Debug.Log("<color=red>OnDisable teen  player...................................................</color>");
        }
    }
    public void CallOnDestroy2(string playerDataNamePlayer, string playerDataDeviceId, int fromWhere)
    {
        if (isDebug)
        {
            //Debug.Log("CallOnDestroy2.... " + playerDataNamePlayer + "fromWhere " + fromWhere);
        }


        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.RaiseEvent((int)EnumPhoton.SendDestroyMessage, playerDataNamePlayer + ";" + playerDataDeviceId, new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache }, SendOptions.SendReliable);
        }
        else
        {
            if (isDebug)
            {
                //Debug.Log("CallOnDestroy not in roomww");
            }
        }
            PhotonNetwork.SendAllOutgoingCommands();
        
    }
    void Start()
    {
        //managerInfo.playerDatas = new PlayerData[5];
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.Landscape;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
       
       
        if (PhotonNetwork.InRoom)
        {
            var room = PhotonNetwork.CurrentRoom;

            if (room != null)
            {
                if (isDebug)
                {
                    Debug.Log($"In room: {room.Name}\n{PhotonNetwork.CurrentRoom.PlayerCount}");
                }
   
            }

        }
        
       
        if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Joker || PlayerSave.singleton.currentTable == eTable.Private)
        {
            tableInfo.startBoot = PlayerSave.singleton.bootAmount;//100
            if (isDebug)
            {
                ////Debug.Log(" tableInfo.startBoot  " + tableInfo.startBoot);
            }
            tableInfo.blindLimit = 4;
            tableInfo.chalLimit = PlayerSave.singleton.chalLimit;//12800
            tableInfo.potLimit = PlayerSave.singleton.potLimit;// 200000;
        }
        else if(PlayerSave.singleton.currentTable == eTable.Free)
        {
            tableInfo.startBoot = PlayerSave.singleton.bootAmount;//100
            if (isDebug)
            {
                ////Debug.Log(" tableInfo.startBoot  " + tableInfo.startBoot);
            }
            tableInfo.blindLimit = 4;
            tableInfo.chalLimit = PlayerSave.singleton.chalLimit;//12800
            tableInfo.potLimit = PlayerSave.singleton.potLimit;// 200000;
        }
        else if (PlayerSave.singleton.currentTable == eTable.NoLimit)
        {
            tableInfo.startBoot = PlayerSave.singleton.bootAmount;//100
            tableInfo.blindLimit = 0; tableInfo.chalLimit = 0; tableInfo.potLimit = 0;
        }
        //playerUI = FindObjectsOfType<PlayerUI>();
        deckManager = FindObjectOfType<DeckManager>();

        if (PlayerSave.singleton != null)
        {
            typeTable = PlayerSave.singleton.currentTable;
            if (isDebug)
            {
                Debug.Log("PlayerSave.singleton._howManyBot " + PlayerSave.singleton._howManyBot);
            }
            CreateLocalPlayer((PhotonNetwork.IsMasterClient && typeTable == eTable.Standard && PlayerSave.singleton._howManyBot > 0) ? false : true);
            if (PlayerSave.singleton.currentTable == eTable.Standard)
            {
                if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
                {

                    if (!GetBotFromServer)
                    {
                        BotEnterResponse botEnterResponse = new BotEnterResponse();
                        if (PlayerSave.singleton._howManyBot > 0)
                        {
                            GetBotFromServer = true;

                            botEnterResponse.status = "200";
                            botEnterResponse.data = new UserInfo[PlayerSave.singleton._howManyBot];
                            if (PlayerSave.singleton.botsServerData != null)
                            {
                                if (PlayerSave.singleton.botsServerData.Count == PlayerSave.singleton._howManyBot)
                                {
                                    for (int i = 0; i < botEnterResponse.data.Length; i++)
                                    {
                                        botEnterResponse.data[i] = PlayerSave.singleton.botsServerData[i];
                                    }
                                }
                            }

                        }
                        else
                        {
                            botEnterResponse.status = "500";
                            botEnterResponse.data = new UserInfo[0];
                        }
                        OnBotEnterResponseStart(botEnterResponse);
                        CallGameEnterAPIIFMasterForStandardOnly();
                    }
                }
            }
        }

        PhotonNetwork.LocalPlayer.NickName = !string.IsNullOrEmpty(PlayerSave.singleton.GetUserName()) ? PlayerSave.singleton.GetUserName() : PlayerSave.singleton.GetCurrentNamey();
        if (PhotonNetwork.IsMasterClient)
        {
            if (PlayerSave.singleton.currentTable == eTable.Private)
            {
                if (PlayerSave.singleton != null)
                {
                    if (PhotonNetwork.InRoom)
                    {
                        PlayerSave.FullRoomName = PhotonNetwork.CurrentRoom.Name;
                    }
                    
                    PlayerSave.singleton.CallGameEnter(PlayerSave.singleton.bootAmount, PlayerSave.singleton.chalLimit, PlayerSave.singleton.potLimit, PlayerSave.singleton._TableId, "P", OnGameEnterResponse, managerInfo.CurrentRoomID);
                    PlayerSave.singleton.GameExit = true;
                }
            }
           
        }
        //PhotonNetwork.OnEventCall += this.OnEvent;
        if (isDebug)
        {
            ////Debug.Log("StaticValues.isbotStatus  " + StaticValues.isbotStatus);
        }



            //AfterNormalResponse();//when we want to comment OnGameEnterResponse
        if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Joker || PlayerSave.singleton.currentTable == eTable.Private)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(GameStartAPI());
            }
        }
        
        if (PhotonNetwork.InRoom)
        {
            
            PlayerPrefs.SetString(PlayerSave.singleton.GetMobileId(), PhotonNetwork.CurrentRoom.Name);
            
        }
    }
    private void CreateLocalPlayer(bool isActive)
    {
        if (isDebug)
        {
            Debug.Log("isActive " + isActive);
        }

        object[] data = new object[]
        {
            PlayerSave.singleton.GetMobileId(),
            PlayerSave.singleton.GetCurrentMoney(),
            PlayerSave.singleton.GetCurrentChips(),
            !string.IsNullOrEmpty(PlayerSave.singleton.GetUserName()) ? PlayerSave.singleton.GetUserName() : PlayerSave.singleton.GetCurrentNamey(),
            PlayerSave.singleton.GetExp(),
            PlayerSave.singleton.GetEmail(),
            false,
            PlayerSave.singleton.GetUserId(),
            PlayerSave.singleton.GetDistributionId(),
            PlayerSave.singleton.GetGender(),
            PlayerSave.singleton.GetAvatar(),
            PlayerSave.singleton.GetPic(),
            isActive,
        };
        PhotonNetwork.Instantiate("PlayerConectedPun", Vector3.zero, Quaternion.identity, 0, data);
    }
    IEnumerator GameStartAPI()
    {
        yield return new WaitForSeconds(0.1f);//0.1f
        if (isDebug)
        {
            //Debug.Log("GameStartAPI before " + managerInfo.CurrentRoomID);
        }
        while (managerInfo.CurrentRoomID == "" || string.IsNullOrEmpty(managerInfo.CurrentRoomID)) yield return null;

        if (isDebug)
        {
            //Debug.Log("GameStartAPI after " + managerInfo.CurrentRoomID);
        }

        if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Joker || PlayerSave.singleton.currentTable == eTable.Private)
        {
            if (PlayerSave.singleton != null)
            {
                if (PhotonNetwork.InRoom)
                {
                    PlayerSave.FullRoomName = PhotonNetwork.CurrentRoom.Name;
                }
                if (!string.IsNullOrEmpty(managerInfo.CurrentRoomID))
                {

                    PlayerSave.singleton.CallGameEnter(PlayerSave.singleton.bootAmount, PlayerSave.singleton.chalLimit, PlayerSave.singleton.potLimit, PlayerSave.singleton._TableId, "P", OnGameEnterResponse, managerInfo.CurrentRoomID);
                    PlayerSave.singleton.GameExit = true;
                }
                else
                {
                    if (!PhotonNetwork.IsMasterClient)
                    {
                        StopCoroutine(GameStartAPI());
                        StartCoroutine(GameStartAPI());
                    }
                }
            }
        }
        if (isDebug)
        {
            //Debug.Log("GameStartAPI " + managerInfo.CurrentRoomID);
        }
    }
    private void AfterNormalResponse()
    {
        if (typeTable != eTable.Private && typeTable != eTable.AndarBahar)
        {
            RandomBot = Random.Range(1, 4);
            if (StaticValues.isbotStatus)
            {
                StartCoroutine(CheckAndGenerateBot());
            }
            else
            {
                botIndex = -1;
                if (players.Count == 1)
                {
                    botIndex = -1;
                    Invoke("Delay", 0.1f);
                }
            }
        }
        else
        {
            if (players.Count == 1)
            {
                botIndex = -1;
                Invoke("Delay", 0.1f);
            }
        }
    }
    
    private void AfterServerResponse(float time)
    {
        if (typeTable == eTable.Standard || typeTable == eTable.Free)
        {
            if (isDebug)
            {
                Debug.Log("StaticValues.isbotStatus " + StaticValues.isbotStatus);
            }
            if (StaticValues.isbotStatus)
            {
                StartCoroutine(CheckAndGenerateBotFromAPI(time));
            }
            else
            {
                botIndex = -1;
                if (players != null)
                {
                    if (players.Count == 1)
                    {
                        botIndex = -1;
                        Invoke("Delay", 0.1f);
                    }
                }
            }
        }
        else
        {
            if (players != null)
            {
                if (players.Count == 1)
                {
                    botIndex = -1;
                    Invoke("Delay", 0.1f);
                }
            }
        }
    }
   
    private void OnGameEnterResponse(GameEnterResponse _gameEnterResponse)
    {
        if(_gameEnterResponse!=null)
        {
            if(_gameEnterResponse.status.Equals("200"))
            {
               
                //if(_gameEnterResponse.data!=null)
                //{
                //    if(_gameEnterResponse.data.Length>0)
                //    {
                //        StaticValues.isbotStatus = true;
                //        botsServerData = new List<UserInfo>();
                //        for(int i=0;i<_gameEnterResponse.data.Length;i++)
                //        {
                //            if (_gameEnterResponse.data[i].totalamount >= PlayerSave.singleton.bootAmount)
                //            {
                //                botsServerData.Add(_gameEnterResponse.data[i]);
                //            }
                //        }

                //    }
                //    else
                //    {
                //        ////Debug.Log("_gameEnterResponse server data length " + _gameEnterResponse.data.Length);
                //        botsServerData = new List<UserInfo>();
                //        StaticValues.isbotStatus = false;

                //    }
                //}
                //else
                //{
                //    botsServerData = new List<UserInfo>();
                //    StaticValues.isbotStatus = false;
                //}
            }
            else
            {
                botsServerData = new List<UserInfo>();
                StaticValues.isbotStatus = false;

                players = FindObjectsOfType<PlayerManagerPun>().ToList();
                try
                {
                    int currentPlayer = players.FindIndex(a => a.playerData._DeviceID.Equals(PlayerSave.singleton.GetUserId()));
                    PlayerDisconnected(players[currentPlayer]);
                }
                catch
                {
                    if (isDebug)
                    {
                        Debug.Log("Not Exits");
                    }
                }
            }
        }
        else
        {
            botsServerData = new List<UserInfo>();
            StaticValues.isbotStatus = false;

            players = FindObjectsOfType<PlayerManagerPun>().ToList();
            try
            {
                int currentPlayer = players.FindIndex(a => a.playerData._DeviceID.Equals(PlayerSave.singleton.GetUserId()));
                PlayerDisconnected(players[currentPlayer]);
            }
            catch
            {
                if (isDebug)
                {
                    Debug.Log("Not Exits");
                }
            }
        }
        AddServerDataAndBotDataForNonMaster();


        AfterServerResponse(0.1f);
    }

    public void AddServerDataAndBotDataForNonMaster()
    {
        //Debug.Log("AddServerDataAndBotDataForNonMaster");
        if (PlayerSave.singleton.currentTable == eTable.Standard)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                players = FindObjectsOfType<PlayerManagerPun>().ToList();
                players = players.OrderBy(x => x.GetIdOrderUI()).ToList();
                if (players != null)
                {
                    if (players.Count > 1)
                    {
                        //Debug.Log("AddServerDataAndBotDataForNonMaster inneer.........");
                        for (int i = 0; i < players.Count; i++)
                        {
                            if (players[i].playerData.IsBot)
                            {
                                botsServerData = new List<UserInfo>();
                                StaticValues.isbotStatus = true;
                                botsData = new List<PlayerData>();
                            }
                        }
                        for (int i = 0; i < players.Count; i++)
                        {
                            if (players[i].playerData.IsBot)
                            {
                                UserInfo userInfo = new UserInfo();
                                userInfo.mobile = players[i].playerData._MobileNumber;
                                userInfo.email = players[i].playerData._Email;
                                userInfo.totalamount = players[i].playerData.Money;
                                userInfo.username = players[i].playerData.NamePlayer;
                                userInfo.totalcount = players[i].playerData.AvatarPic;
                                userInfo.gender = players[i].playerData._Gender;
                                botsServerData.Add(userInfo);

                                botsData.Add(new PlayerData
                                {
                                    playerType = ePlayerType.Bot,
                                    currentCombination = eCombination.Empty,
                                    Money = botsServerData[botsServerData.Count - 1].totalamount,
                                    Chips = Random.Range(1000, 50000),

                                    NamePlayer = botsServerData[botsServerData.Count - 1].username,
                                    experience = 1000,
                                    _DistributionId = botsServerData[botsServerData.Count - 1].mobile,
                                    _MobileNumber = botsServerData[botsServerData.Count - 1].mobile,
                                    _Email = string.IsNullOrEmpty(botsServerData[botsServerData.Count - 1].email) ? "" : botsServerData[botsServerData.Count - 1].email.ToLowerInvariant(),
                                    _DeviceID = botsServerData[botsServerData.Count - 1].mobile,
                                    AvatarPic = botsServerData[botsServerData.Count - 1].totalcount == 0 ? Random.Range(0, 27) : botsServerData[botsServerData.Count - 1].totalcount,
                                    _Gender = string.IsNullOrEmpty(botsServerData[botsServerData.Count - 1].gender) ? "M" : botsServerData[botsServerData.Count - 1].gender,
                                    IsBot = true
                                });
                            }
                        }
                    }
                    botsServerData = new List<UserInfo>();
                }
            }
        }
    }

    int IfAnyBotExists = 0;
    int OtherCurrentPlayer = -1;
    private void Update()
    {
        timer += Time.deltaTime;
      
        if (timer > timeStep)
        {
           
            timer = 0;
            if (PhotonNetwork.IsConnected)
            {

             

                if ((PhotonNetwork.IsMasterClient) && (PhotonNetwork.PlayerList.Length > 1))
                {
                   
                    if (photonView != null)
                    {

                        if (PhotonNetwork.InRoom)
                        {
                            photonView.RPC("SyncRoom", RpcTarget.AllBuffered, JsonUtility.ToJson(managerInfo));
                        }
                    }
                    
                }
                else if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length <= 1)
                {
                    if (managerInfo.isStartedGame)
                    {
                        if (managerInfo.totalPot > 0)
                        {
                            players = FindObjectsOfType<PlayerManagerPun>().ToList();
                            players = players.OrderBy(x => x.GetIdOrderUI()).ToList();
                            if (players.Count >= 0 && players.Count < 2)
                            {
                                if (CountPlayersInGame() == 1)
                                {
                                    ////Debug.Log("in fixed update reset condition ");
                                    managerInfo.totalPot = 0;
                                    double worstScene = managerInfo.totalPot;
                                    ResetGame();
                                    if (PlayerSave.singleton != null)
                                    {
                                        if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
                                        {
                                            if (PhotonNetwork.InRoom)
                                            {
                                                PlayerSave.singleton.CallUpdateAmount2(0, PhotonNetwork.CurrentRoom.Name, "-", "Worst_" + worstScene.ToString(), "P", managerInfo.GameRoom_2.ToString(), managerInfo.CurrentRoomID.ToString());
                                            }
                                            else
                                            {
                                                PlayerSave.singleton.CallUpdateAmount2(0, PlayerSave.FullRoomName, "-", "Worst_" + worstScene.ToString(), "P", managerInfo.GameRoom_2.ToString(), managerInfo.CurrentRoomID.ToString());
                                            }
                                        }
                                    }
                                }
                                else if (CountPlayersInGame() == 0)
                                {
                                    ////Debug.Log("in fixed update reset else condition ");
                                    managerInfo.totalPot = 0;
                                    double worstScene = managerInfo.totalPot;
                                    ResetGame();
                                    if (PlayerSave.singleton != null)
                                    {
                                        if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
                                        {
                                            if (PhotonNetwork.InRoom)
                                            {
                                                PlayerSave.singleton.CallUpdateAmount2(0, PhotonNetwork.CurrentRoom.Name, "-", "Worst_" + worstScene.ToString(), "P", managerInfo.GameRoom_2.ToString(), managerInfo.CurrentRoomID.ToString());
                                            }
                                            else
                                            {
                                                PlayerSave.singleton.CallUpdateAmount2(0, PlayerSave.FullRoomName, "-", "Worst_" + worstScene.ToString(), "P", managerInfo.GameRoom_2.ToString(), managerInfo.CurrentRoomID.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    else
                    {
                        WaitingStart = true;
                    }
                }
            }
        }
        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length <= 1)
        {
            if (players != null)
            {
                if (players.Count == 1 && WaitingStart)
                {
                    if (players[0].hud != null)
                    {

                        _timer2 = _timer2 + Time.deltaTime;
                        
                        if (_timer2 >= 1f && _timer2 < 2f)
                        {
                            players[0].hud.SetGameRunningGlobalInfo("Waiting for other players.");
                        }
                        else if (_timer2 >= 2f && _timer2 < 3f)
                        {
                            players[0].hud.SetGameRunningGlobalInfo("Waiting for other players..");
                        }
                        else if (_timer2 >= 3f)
                        {
                            players[0].hud.SetGameRunningGlobalInfo("Waiting for other players...");
                            _timer2 = 0;
                            //if (!players[0].playerData.IsBot)
                            //{
                            //    if (PlayerSave.singleton != null)
                            //    {
                            //        if (PlayerSave.singleton.currentTable == eTable.Standard)
                            //        {
                            //            if (PhotonNetwork.InRoom)
                            //            {
                            //                if (!GetBotFromServer)
                            //                {
                            //                    GetBotFromServer = true;
                            //                    PlayerSave.singleton.CallBotEnter(PlayerSave.singleton.bootAmount, PlayerSave.singleton.chalLimit, PlayerSave.singleton.potLimit, PlayerSave.singleton._TableId, "P", OnBotEnterResponse, managerInfo.CurrentRoomID, 1);
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                        }
                    }
                }
                else if(WaitingStart)
                {
                    WaitingStart = false;
                    if (players != null)
                    {
                        if (players.Count == 1)
                        {
                            if (players[0].hud != null)
                            {
                                players[0].hud.ClearTextGlobalInfo();
                            }
                        }
                    }
                }
               
            }
        }
        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient && (PhotonNetwork.PlayerList.Length <= 2))// || PhotonNetwork.PlayerList.Length == 1))
        {
            if (players != null)
            {
                if (players.Count >0 && players.Count<=4)
                {

                    _timerNew = _timerNew + Time.deltaTime;
                    //Debug.Log("_timerNew " + _timerNew + " _timerRange "+ _timerRange);
                    if (_timerNew >= _timerRange)
                    {
                        _timerNew = 0;
                        if (PlayerSave.singleton != null)
                        {
                            if (PlayerSave.singleton.currentTable == eTable.Standard)
                            {
                                if (PhotonNetwork.InRoom)
                                {
                                    //Debug.Log("<color=red>GetBotFromServer " + GetBotFromServer+"</color>");
                                    if (!GetBotFromServer)
                                    {
                                        GetBotFromServer = true;
                                        _timerRange = Random.Range(10f, 40f);
                                        int _howManyBotInRoom = Random.Range(1, 4);
                                        //Debug.Log("_howManyBotInRoomInBetween before......................." + _howManyBotInRoom);
                                        int playersCount = players.Count;
               
                                        int remainingBot = 4 - playersCount;
                                        if((_howManyBotInRoom + playersCount) > 4)
                                        {
                                            _howManyBotInRoom = remainingBot;
                                        }
                                        //Debug.Log("_howManyBotInRoomInBetween ......................." + _howManyBotInRoom);
                                        //Debug.Log("GetBotFromServer " + GetBotFromServer);
                                        if (_howManyBotInRoom > 0)
                                        {
                                            Debug.Log("pot limit is " + PlayerSave.singleton.potLimit);
                                            PlayerSave.singleton.CallBotEnter(PlayerSave.singleton.bootAmount, PlayerSave.singleton.chalLimit, PlayerSave.singleton.potLimit, PlayerSave.singleton._TableId, "P", OnBotEnterResponse, managerInfo.CurrentRoomID, _howManyBotInRoom);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

        if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length >= 1)
        {
            if (players != null)
            {
                if (players.Count >=1)
                {

                    _timerNew = _timerNew + Time.deltaTime;
                    //Debug.Log("_timerNew " + _timerNew + " _timerRange "+ _timerRange);

                   
                    if (_timerNew >= 5f)
                    {
                        _timerNew = 0;

                       
                       
                            if (PlayerSave.singleton != null)
                            {
                                if (PlayerSave.singleton.currentTable == eTable.Standard)
                                {
                                    if (PhotonNetwork.InRoom)
                                    {
                                        if (isDebug)
                                        {
                                            Debug.Log("<color=red>GetBotFromServer " + GetBotFromServer + "</color>");
                                        }

                                        IfAnyBotExists = 0;
                                        OtherCurrentPlayer = -1;
                                        foreach (var item in players)
                                        {
                                            if (item.playerData.IsBot)
                                            {
                                                IfAnyBotExists++;
                                                break;
                                            }
                                        }
                                        if (isDebug)
                                        {
                                            Debug.Log("GetBotFromServer " + GetBotFromServer);
                                        }
                                        if (IfAnyBotExists > 0)
                                        {
                                            AddServerDataAndBotDataForNonMaster();
                                        }
                                        else
                                        {
                                            if (botsData != null)
                                            {
                                                botsData.Clear();
                                                botsData.TrimExcess();
                                            }
                                        }
                                    }
                                }
                            }
                        
                    }
                }

            }
        }
    }
    private void OnBotEnterResponse(BotEnterResponse botEnterResponse)
    {
        IfAnyBotExists = 0;
        foreach (var item in players)
        {
            if (item.playerData.IsBot)
            {
                IfAnyBotExists++;
               
            }
        }
        int currentPlayerExists = -1;
        //Debug.Log("IfAnyBotExists only one  " + IfAnyBotExists);
        if (IfAnyBotExists < 5)
        {
            if (botEnterResponse != null)
            {
                if (botEnterResponse.status.Contains("200"))
                {
                    if (botEnterResponse.data != null)
                    {
                        if (botEnterResponse.data.Length > 0)
                        {
                            StaticValues.isbotStatus = true;
                            botsServerData = new List<UserInfo>();
                            for (int i = 0; i < botEnterResponse.data.Length; i++)
                            {
                                if (botEnterResponse.data[i].totalamount >= PlayerSave.singleton.bootAmount)
                                {
                                    //foreach (var item in players)
                                    //{
                                    //    Debug.Log("item.playerData._DeviceID= " + item.playerData._DeviceID + " botEnterResponse.data[i].mobile " + botEnterResponse.data[i].mobile);
                                       
                                        
                                    //}
                                    currentPlayerExists = players.FindIndex(a => a.playerData._DeviceID.Equals(botEnterResponse.data[i].mobile));
                                    //Debug.Log("currentPlayerExists " + currentPlayerExists);
                                    if (currentPlayerExists == -1)
                                    {
                                        botsServerData.Add(botEnterResponse.data[i]);
                                    }
                                }
                            }
                            if (botsServerData != null)
                            {
                                if (botsServerData.Count > 0)
                                {
                                    List<UserInfo> uniqueLst = botsServerData.Distinct().ToList();
                                    if (uniqueLst != null)
                                    {
                                        if (uniqueLst.Count > 0)
                                        {
                                            botsServerData = uniqueLst.ToList();
                                        }
                                    }
                                }
                            }
                            if (botsServerData.Count == 0)
                            {
                                //Debug.Log("reset botsServerData ");
                                botsServerData = new List<UserInfo>();
                                StaticValues.isbotStatus = false;
                            }
                        }
                        else
                        {
                            //Debug.Log("botEnterResponse server data length " + botEnterResponse.data.Length);
                            botsServerData = new List<UserInfo>();
                            StaticValues.isbotStatus = false;

                        }
                    }
                    else
                    {
                        botsServerData = new List<UserInfo>();
                        StaticValues.isbotStatus = false;
                    }
                }
            }
            AfterServerResponse(0f);
        }
    }

    private void OnBotEnterResponseStart(BotEnterResponse botEnterResponse)
    {
        
            if (botEnterResponse != null)
            {
                if (botEnterResponse.status.Contains("200"))
                {
                    if (botEnterResponse.data != null)
                    {
                        if (botEnterResponse.data.Length > 0)
                        {
                            StaticValues.isbotStatus = true;
                            botsServerData = new List<UserInfo>();
                            for (int i = 0; i < botEnterResponse.data.Length; i++)
                            {
                                if (botEnterResponse.data[i].totalamount >= PlayerSave.singleton.bootAmount)
                                {
                                    botsServerData.Add(botEnterResponse.data[i]);
                                }
                            }
                            if (botsServerData != null)
                            {
                                if (botsServerData.Count > 0)
                                {
                                    List<UserInfo> uniqueLst = botsServerData.Distinct().ToList();
                                    if (uniqueLst != null)
                                    {
                                        if (uniqueLst.Count > 0)
                                        {
                                            botsServerData = uniqueLst.ToList();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Debug.Log("botEnterResponse server data length " + botEnterResponse.data.Length);
                            botsServerData = new List<UserInfo>();
                            StaticValues.isbotStatus = false;

                        }
                    }
                    else
                    {
                        botsServerData = new List<UserInfo>();
                        StaticValues.isbotStatus = false;
                    }
                }
            }
        AfterServerResponse(0f);

    }
    private void OnGameEnterResponseStart(GameEnterResponse _gameEnterResponse)
    {
        if (_gameEnterResponse != null)
        {
            if (_gameEnterResponse.status.Equals("200"))
            {

            }
            else
            {
                botsServerData = new List<UserInfo>();
                StaticValues.isbotStatus = false;

                players = FindObjectsOfType<PlayerManagerPun>().ToList();
                try
                {
                    int currentPlayer = players.FindIndex(a => a.playerData._DeviceID.Equals(PlayerSave.singleton.GetUserId()));
                    PlayerDisconnected(players[currentPlayer]);
                }
                catch
                {
                    if (isDebug)
                    {
                        Debug.Log("Not Exits");
                    }
                }
            }
        }
        else
        {
            botsServerData = new List<UserInfo>();
            StaticValues.isbotStatus = false;

            players = FindObjectsOfType<PlayerManagerPun>().ToList();
            try
            {
                int currentPlayer = players.FindIndex(a => a.playerData._DeviceID.Equals(PlayerSave.singleton.GetUserId()));
                PlayerDisconnected(players[currentPlayer]);
            }
            catch
            {
                if (isDebug)
                {
                    Debug.Log("Not Exits");
                }
            }
        }
        
        
    }
    private void CallGameEnterAPIIFMasterForStandardOnly()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PlayerSave.singleton.currentTable == eTable.Standard)
            {
                if (PlayerSave.singleton != null)
                {
                    if (PhotonNetwork.InRoom)
                    {
                        PlayerSave.FullRoomName = PhotonNetwork.CurrentRoom.Name;
                    }
                    
                    PlayerSave.singleton.CallGameEnter(PlayerSave.singleton.bootAmount, PlayerSave.singleton.chalLimit, PlayerSave.singleton.potLimit, PlayerSave.singleton._TableId, "P", OnGameEnterResponseStart, managerInfo.CurrentRoomID);
                    PlayerSave.singleton.GameExit = true;
                }
            }
        }
    }
    private bool WaitingStart = false;
    private float _timer2;
    private float _timerNew;
    private float _timerRange=10f;
    public void OnPlayerId()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null)
            {
                if (players[i].photonView != null)
                {
                    if (managerInfo.currentPlayerStepID == players[i].photonView.ViewID && !players[i].playerData.IsPacked)
                    {
                        managerInfo.currentChance = players[i].GetId();
                        ////Debug.Log("managerInfo.currentChance in OnPlayerId " + managerInfo.currentChance);
                    }
                }
            }
        }
        //int SideIndex = players.FindIndex(x => x.photonView.viewID == managerInfo.currentPlayerStepID && !x.playerData.IsPacked);
        //managerInfo.currentChance = players[SideIndex].GetId();
    }


    public void CheckPlayerIds()//if i call this function so the sequence of D icon is disturbed
    {
        managerInfo.currentChance = -1;
        if (!string.IsNullOrEmpty(managerInfo.AllPhotonIds))
        {
            if (managerInfo.AllPhotonIds.Length > 0)
            {
                string founderMinus1 = managerInfo.AllPhotonIds.Remove(managerInfo.AllPhotonIds.Length - 1, 1);

                string[] d12 = founderMinus1.Split(';');
                //int[] arr = Array.ConvertAll<string, int>(d12, int.Parse);
                int SideIndex = -1;
                string NewStepId = "";
                if (d12 != null)
                {
                    ////Debug.Log("arr " + arr.Length);
                    if (d12.Length > 0)
                    {
                        for (int i = 0; i < (d12.Length); i++)
                        {
                            if (d12[i] == managerInfo.IsCurrentIndex)
                            {
                                SideIndex = i;
                                ////Debug.Log("SideIndex " + SideIndex);
                                SideIndex++;
                                if (SideIndex > d12.Length -1)
                                {
                                    SideIndex = 0;

                                    NewStepId = d12[SideIndex];

                                    //Debug.Log("inner if SideIndex " + SideIndex + "NewStepId " + NewStepId);
                                }
                                else
                                {

                                    NewStepId = d12[SideIndex];

                                    //Debug.Log("inner else SideIndex " + SideIndex + "NewStepId " + NewStepId);
                                }
                                break;
                            }
                        }
                       // Debug.Log("before NewStepId " + NewStepId);
                        if (NewStepId != "")
                        {
                            for (int i = 0; i < players.Count; i++)
                            {
                                if (players[i] != null)
                                {
                                    if (players[i].photonView != null)
                                    {
                                        if (NewStepId == players[i].playerData._DeviceID && players[i].playerData.playerType == ePlayerType.PlayerStartGame)
                                        {
                                            managerInfo.currentChance = i;
                                            //Debug.Log("new managerInfo.currentChance else SideIndex  " + managerInfo.currentChance);
                                            break;
                                        }
                                    }
                                }
                            }
                            if (managerInfo.currentChance == -1)
                            {
                                if (SideIndex >= players.Count)
                                {
                                    managerInfo.currentChance = 0;
                                    //Debug.Log(".....new managerInfo.currentChance else SideIndex  " + managerInfo.currentChance + "SideIndex " + SideIndex + "players.Count " + players.Count);
                                }
                                else if (SideIndex > -1 && SideIndex < players.Count)
                                {
                                    managerInfo.currentChance = SideIndex;
                                    //Debug.Log("....in .new managerInfo.currentChance else SideIndex  " + managerInfo.currentChance + "SideIndex " + SideIndex + "players.Count " + players.Count);
                                }
                                if (managerInfo.currentChance == -1)
                                {
                                    managerInfo.currentChance = managerInfo.currentPlayerStep;
                                    //Debug.Log("....out..new managerInfo.currentChance else SideIndex  " + managerInfo.currentChance);
                                }
                            }
                        }

                    }
                }
            }
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
    
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (isDebug)
        {
            Debug.Log("#$#$#$#$ OnMasterClientSwitched - name: " + newMasterClient.NickName + " PhotonNetwork.IsMasterClient " + PhotonNetwork.IsMasterClient);
        }
        base.OnMasterClientSwitched(newMasterClient);
        players = FindObjectsOfType<PlayerManagerPun>().ToList();
        players = players.OrderBy(x => x.GetIdOrderUI()).ToList();

        bool playerStepDisconect = false;
        bool isSentSideShow = false;
        //bool isNextPlayerStepMethodCall = false;
        if (PhotonNetwork.IsMasterClient)
        {
            if (isDebug)
            {
                Debug.Log("migration on manager");
            }

            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].playerData.isDisconnect)
                {
					try
					
                    {
                    	playersUiCollection.First(x => x.name == players[i].GetNameUI()).IsFull = true;
   						////Debug.Log("in try ");
                    }
                    catch
                    {
                        ////Debug.Log("in catch ");
                        PhotonNetwork.LeaveRoom(false);
                    }
                    if (isDebug)
                    {
                        Debug.Log("migration on manager in if... " + managerInfo.currentPlayerStepID + " " + players[i].photonView.ViewID);
                    }
                   
                    if(managerInfo.currentPlayerStepID == players[i].photonView.ViewID)
                    {

                        if (isDebug)
                        {
                            Debug.Log("migration on manager in if if " + newMasterClient.IsInactive + "id...." + managerInfo.currentPlayerStepID);
                        }

                        if(managerInfo.isChaalPlayer)
                        {
                            if(managerInfo.isChaalPlayerUpdateBoot)
                            {
                                managerInfo.isChaalPlayerUpdateBoot = false;
                                if (managerInfo.isChaalPlayerUpdateBootAmount > 0)
                                {
                                    MoneyPlayerToTable(players[i], managerInfo.isChaalPlayerUpdateBootAmount,0,0 , "OnMasterClientSwitched");
                                }
                            }
                        }
                        if(managerInfo._IsCurrentMobileNumber == players[i].playerData._DeviceID)
                        {
                            managerInfo.IsCurrentPlayerDestroyed = true;
                        }
                    }
                }
                else
                {
                    if (isDebug)
                    {
                        Debug.Log("migration on manager in else..." + managerInfo.currentPlayerStepID + " " + players[i].photonView.ViewID);
                    }
                    if (managerInfo.currentPlayerStepID == players[i].photonView.ViewID)
                    {
                        playerStepDisconect = true;
                        //managerInfo.InAnyNextCondition = true;
                        managerInfo.IsCurrentLocalPlayer = false;
                        isSentSideShow = players[i].playerData.isSentSideShow;
                        
                    }

                    playersUiCollection.First(x => x.name == players[i].GetNameUI()).IsFull = false;
                    players[i].photonView.RPC("ClearFromGame", RpcTarget.All);
                    players.Remove(players[i]);
                    i--;
                }
            }

            for (int i = 0; i < playersUiCollection.Length; i++)
            {
                if (!playersUiCollection[i].IsFull)
                {
                    playersUiCollection[i].ClearUI(0);
                }
            }
            if (isDebug)
            {
                Debug.Log(" ismasterswitch " + playerStepDisconect + " isGivingCards " + managerInfo.isGivingCards + " isNextGameTimer " + managerInfo.isNextGameTimer + " isCheckOnWin " + managerInfo.isCheckOnWin + " isChaalPlayer " + managerInfo.isChaalPlayer);
            }
            bool ISCheckOnWin = false;
            ISCheckOnWin = managerInfo.isCheckOnWin;
            bool ISGivingCards = false;
            ISGivingCards = managerInfo.isGivingCards;
            bool IsNextGameTimer = false;
            IsNextGameTimer = managerInfo.isNextGameTimer;
            if (managerInfo.isGivingCards)
            {
                StopCoroutine("GiveMovingCards");
                //managerInfo.InAnyNextCondition = true;
            }
            else if (managerInfo.isNextGameTimer)
            {
                StopCoroutine("GlobalInformation15");
                //managerInfo.InAnyNextCondition = true;
            }
            else if(managerInfo.isChaalPlayer)
            {
                //managerInfo.InAnyNextCondition = true;
            }
            else if (managerInfo.isSideShowReceiver)
            {
                
                    managerInfo.isSideShowReceiver = false;
                    managerInfo.playerIdRecievedSideShow = 0;
                    MasterDeclineSideShow(managerInfo.playerIdRecievedSideShowName);
                    managerInfo.playerIdRecievedSideShowName = "";
                    managerInfo.playerIdRecievedSideShowDeviceId = "";
                    managerInfo.IsCurrentLocalPlayer = false;
            }
            else if(managerInfo.isCheckOnWin)
            {
                CheckOnWin();
                managerInfo.IsCurrentLocalPlayer = false;
                //managerInfo.InAnyNextCondition = true;
            }
            if (CountPlayersInGame() == 1)
            {
                if (isDebug)
                {
                    Debug.Log("PlayerWinGame -------------------OnMasterClientSwitched--------------------PlayerWinGame");
                }
                PlayerWinGame(players[GetNumWinPlayer()]);
                managerInfo.IsCurrentLocalPlayer = false;
            }
            else
            {
                if (managerInfo.isGivingCards)
                {
                    CancelInvoke();
                    GiveCardsToPlayers();
                    managerInfo.IsCurrentLocalPlayer = false;
                }
                else if (managerInfo.isNextGameTimer)//Added by me
                {
                    if (isDebug)
                    {
                        Debug.Log("NextGameGlobalInformation ---------nps isNextGameTimer true");
                    }
					StopCoroutine(NextGameGlobalInformation());
					StartCoroutine(NextGameGlobalInformation());
                    managerInfo.IsCurrentLocalPlayer = false;
                }
				else if (playerStepDisconect && !isSentSideShow)
				{

                    if (isDebug)
                    {
                        Debug.Log("nps on 1-------------------------------------------------------------------");
                    }
                    NextPlayerStep();
	
	            }
				else if(managerInfo.isChaalPlayer)
                {
                    managerInfo.isChaalPlayer = false;
                    managerInfo.IsCurrentLocalPlayer = false;
                    if (isDebug)
                    {
                        Debug.Log("nps on 2-------------------------------------------------------------------");
                    }
                    NextPlayerStep();
                    
                }
				else
				{
				                    ////Debug.Log("no need change");
					if (CountPlayersInGame() == 0)
                    {
                        if(!ISCheckOnWin && !IsNextGameTimer && !ISGivingCards && !playerStepDisconect && !isSentSideShow)
                        {
                            if (isDebug)
                            {
                                Debug.Log("no need change check condition all false ");
                            }
                            managerInfo.IsCurrentLocalPlayer = false;
                            CheckOnWin();
                            
                        }
                        else
                        {
                            if (isDebug)
                            {
                                Debug.Log("no need change check condition all not false  " + ISCheckOnWin + "_" + IsNextGameTimer + "_" + ISGivingCards + "_" + playerStepDisconect + "_" + isSentSideShow);
                            }
                        }
                    }
                    else
                    {
                        if (isDebug)
                        {
                            Debug.Log("no need change...in else  ");
                        }
                       
                    }
	             }
           }
		   UpdateRoomStatus();
	    }

        
    }
    IEnumerator NextGameGlobalInformation()
    {
        yield return new WaitForSeconds(5f);
        try
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (photonView != null)
                {
                    PhotonNetwork.RemoveRPCs(photonView);
                }
            }
        }
        catch
        {

        }
        players = FindObjectsOfType<PlayerManagerPun>().ToList();
        players = players.OrderBy(x => x.GetIdOrderUI()).ToList();
       
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null)
            {
                if (players[i].photonView != null)
                {
                    players[i].photonView.RPC("SetInGame", RpcTarget.All);
                    //////Debug.Log("NextGameGlobalInformation <color=red>SetDealerIcon**********************************************i </color> " + i + " managerInfo.currentPlayerStep " + managerInfo.currentPlayerStep);
                    players[i].photonView.RPC("SetDealerIcon", RpcTarget.All, i, managerInfo.currentPlayerStep);
                }
            }
        }
        if(PhotonNetwork.IsMasterClient)
        {
            managerInfo.CurrentRoomID = System.Guid.NewGuid().ToString();
            PlayerSave.NewRoomName = managerInfo.CurrentRoomID;
            if (photonView != null)
            {
                photonView.RPC("SyncRoom", RpcTarget.AllBuffered, JsonUtility.ToJson(managerInfo));
            }
        }
        int inew = 7;
        managerInfo.AllPhotonIds = "";
        if (!managerInfo.isNextGameTimer)
        {
            managerInfo.isNextGameTimerValue = 7;
            managerInfo.isNextGameTimer = true;
        }
        else
        {
            inew = managerInfo.isNextGameTimerValue;
            //////Debug.Log("remaining timer " + inew);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null)
            {
                if (players[i].photonView != null)
                {
                    players[i].photonView.RPC("GameStartAPI", RpcTarget.All);
                    
                }
            }
        }
        for (inew = managerInfo.isNextGameTimerValue; inew > 0; inew -= 1)
        {

            if (players != null)
            {
                if (players.Count >= 2)
                {
                    foreach (var item in players)
                    {
                        if (item != null)
                        {
                            if (item.photonView != null)
                            {
                                managerInfo.isNextGameTimerValue = inew;
                                item.photonView.RPC("GlobalInfo", RpcTarget.All, "Next Game starts in " + inew.ToString() + " sec");
                                if (item.localPlayer != null)
                                {
                                    item.localPlayer.PanelAcceptOff();
                                }
								if(inew<=1)
								{
	                                if (item.localPlayer != null)
	                                {
	                                    item.localPlayer.OnCloseWithdrawGamePanel();
	                                }
								}
								else
								{
									if (item.localPlayer != null)
									{
										item.localPlayer.OnOpenWithdrawGamePanel();
									}

									if(showOutofLimitPopUp)
									{
										if (item.localPlayer != null)
										{
											item.localPlayer.OnOpenOutOfLimitPanel();
										}
									}
								}
                            }
                        }
                    }
                }
            }

            yield return new WaitForSeconds(1f);
        }
        managerInfo.isNextGameTimerValue = 0;
        managerInfo.isNextGameTimer = false;
        managerInfo.playerIdRecievedSideShowDeviceId = "";
        managerInfo.IsCurrentPlayerPause = false;
        CancelInvoke();
        GiveCardsToPlayers();

       
    }
    private IEnumerator CheckAndGenerateBot()
    {
        yield return new WaitForSeconds(0.1f);//0.1f
        while (players == null || players.Count == 0) yield return null;
        if (typeTable == eTable.Standard)
        {
            yield return StartCoroutine(GenerateBots((double)players[0].playerData.Money, (double)players[0].playerData.Chips, players[0].playerData.experience,
            players[0].playerData._DistributionId));
        }
        else
        {
            yield return StartCoroutine(GenerateBots((double)players[0].playerData.Money,(double)players[0].playerData.Chips, players[0].playerData.experience,
          players[0].playerData._DistributionId));
        }
    }
    
    private IEnumerator CheckAndGenerateBotFromAPI(float time)
    {
        yield return new WaitForSeconds(time);//0.1f
        while (players == null || players.Count == 0) yield return null;
        if (typeTable == eTable.Standard)
        {
            if (players.Count < 5)
            {
                yield return StartCoroutine(GenerateBotsFromServer(time));
            }
            else
            {
                yield return new WaitForSeconds(0f);
            }
        }
        else
        {
            yield return StartCoroutine(GenerateBots((double)players[0].playerData.Money, (double)players[0].playerData.Chips, players[0].playerData.experience,
          players[0].playerData._DistributionId));
        }
    }
   
    private IEnumerator GenerateBotsFromServer(float time)
    {
        if (!PhotonNetwork.IsMasterClient) yield break;

        if (isDebug)
        {
            Debug.Log("MasterClient, generating bot data...");
        }
        if (botsData == null)
        {
            botsData = new List<PlayerData>();
        }
        if(botsServerData!=null)
        {
            if (botsServerData.Count > 0)
            {
                RandomBot = botsServerData.Count;//always add bot one by one
            }
            else
            {
                RandomBot = 0;
            }
        }
        if (botsServerData.Count > 0)
        {
            botIndex = botsServerData.Count - 1;
        }
        else
        {
            botIndex = 0;
        }
        if (isDebug)
        {
            Debug.Log("RandomBot " + RandomBot + "botIndex " + botIndex);
        }
        for (botIndex = 0; botIndex < RandomBot; botIndex++)
        {
            try
            {
                botsData.Add(new PlayerData
                {
                    playerType = ePlayerType.Bot,
                    currentCombination = eCombination.Empty,
                    Money = botsServerData[botIndex].totalamount,
                    Chips = Random.Range(1000, 50000),

                    NamePlayer = botsServerData[botIndex].username,
                    experience = 1000,
                    _DistributionId = botsServerData[botIndex].mobile,
                    _MobileNumber = botsServerData[botIndex].mobile,
                    _Email = string.IsNullOrEmpty(botsServerData[botIndex].email) ? "" : botsServerData[botIndex].email.ToLowerInvariant(),
                    _DeviceID = botsServerData[botIndex].mobile,
                    AvatarPic = botsServerData[botIndex].totalcount == 0 ? Random.Range(0, 27) : botsServerData[botIndex].totalcount,
                    _Gender = string.IsNullOrEmpty(botsServerData[botIndex].gender) ? "M" : botsServerData[botIndex].gender,
                    IsBot = true
                });


                object[] data = new object[]
                {
                botsServerData[botIndex].mobile,
                botsServerData[botIndex].totalamount,
                botsData[botsData.Count-1].Chips,
                botsServerData[botIndex].username,
                botsData[botsData.Count-1].experience,
                botsData[botsData.Count-1]._Email,
                true,
                botsData[botsData.Count-1]._DistributionId,
                botsData[botsData.Count-1]._MobileNumber,
                botsData[botsData.Count-1]._Gender,
                botsData[botsData.Count-1].AvatarPic,
                "",
                true,
                };
                //////Debug.Log("botsData[botIndex].Money " + " botIndex "+ botIndex +"  " + botsData[botIndex].Money);
                //botsData[botIndex].experience = Mathf.Max(0, botsData[botIndex].experience);
                //botsData[botIndex].IsBot = true;

                PhotonNetwork.InstantiateRoomObject("PlayerConectedPun", Vector3.zero, Quaternion.identity, 0, data);
                if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Joker || PlayerSave.singleton.currentTable == eTable.Private)
                {
                    if (PlayerSave.singleton != null)
                    {
                        if (PhotonNetwork.InRoom)
                        {
                            PlayerSave.FullRoomName = PhotonNetwork.CurrentRoom.Name;
                        }
                        PlayerSave.singleton.CallGameEnterForBotOnly(botsData[botsData.Count - 1]._MobileNumber, PlayerSave.singleton.bootAmount, PlayerSave.singleton.chalLimit, PlayerSave.singleton.potLimit, PlayerSave.singleton._TableId, "B", UpdateBotEnterResponse, managerInfo.CurrentRoomID);
                        PlayerSave.singleton.GameExit = true;
                    }
                }
            }
            catch
            {

            }
            yield return new WaitForSeconds(time);
           
        }

        botIndex = -1;
        botsServerData = new List<UserInfo>();
        RemoveBotEnterResponse();
        


    }
    private void UpdateBotEnterResponse(GameEnterResponse gameEnterResponse)
    {
        if(gameEnterResponse != null)
        {
            if(gameEnterResponse.status=="200")
            {

            }
            else
            {
                if (NewbotsData == null)
                {
                    NewbotsData = new List<string>();
                }
                NewbotsData.Add(gameEnterResponse.message);
               
            }
        }
    }
    public void RemoveBotEnterResponse()
    {
        if (NewbotsData != null)
        {
            while (NewbotsData.Count > 0)
            {

                players = FindObjectsOfType<PlayerManagerPun>().ToList();
                if (players != null)
                {
                    players = players.OrderBy(x => x.GetIdOrderUI()).ToList();
                    var p = players.Find(x => x.playerData.IsBot && x.playerData._DeviceID == NewbotsData[0]);
                    if (p != null)
                    {

                        if (PhotonNetwork.IsMasterClient)
                        {

                            if (p.playerData.IsBot)
                            {
                                PlayerSave.singleton.CallGameExitForBotOnly(p.playerData._MobileNumber, 0, "B", null);
                            }

                            p.Disconnect();
                            try
                            {
                                int newIndex = botsData.FindIndex(a => a._DeviceID.Equals(NewbotsData[0]));
                                if (newIndex > 0)
                                {
                                    NewbotsData.RemoveAt(0);
                                }

                            }
                            catch
                            {

                            }
                            PhotonNetwork.Destroy(p.gameObject);
                        }


                    }
                }

            }


            NewbotsData.TrimExcess();
            NewbotsData.Clear();
        }
    }
    private IEnumerator GenerateBots(double playerMoney, double playerChips, double playerExp, string playerLocation)
    {
        if (!PhotonNetwork.IsMasterClient) yield break;

        Debug.Log("MasterClient, generating bot data...");
        botsData = new List<PlayerData>();
        for (botIndex = 0; botIndex < RandomBot; botIndex++)
        {
            Debug.Log("add bot money");
            botsData.Add(new PlayerData
            {
                playerType = ePlayerType.Bot,
                currentCombination = eCombination.Empty,
                Money = playerMoney + Random.Range(1000, 50000),
                //Mathf.Min(playerMoney+1000, playerMoney + 10000),
                //Mathf.Max(playerMoney+5000, playerMoney + 50000)),
                Chips = playerChips + Random.Range(1000, 50000),
                //Random.Range(
                //    Mathf.Min(playerChips+1000, playerChips + 10000),
                //    Mathf.Max(playerChips+5000, playerChips + 50000)),
                NamePlayer = $"B{(botIndex + 1):00}",
                experience = playerExp + Random.Range(-1000, 1000),
                _DistributionId = playerLocation,
                IsBot = true
            });

            ////Debug.Log("botsData[botIndex].Money " + " botIndex " + botIndex + "  " + botsData[botIndex].Money);
            //botsData[botIndex].experience = Mathf.Max(0, botsData[botIndex].experience);
            //botsData[botIndex].IsBot = true;

            PhotonNetwork.InstantiateRoomObject("PlayerConectedPun", Vector3.zero, Quaternion.identity, 0);
            //if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Joker || PlayerSave.singleton.currentTable == eTable.Private)
            //{
            //    if (PlayerSave.singleton != null)
            //    {
            //        if (PhotonNetwork.InRoom)
            //        {
            //            PlayerSave.FullRoomName = PhotonNetwork.CurrentRoom.Name;
            //        }
            //        PlayerSave.singleton.CallGameEnter(PlayerSave.singleton.bootAmount, PlayerSave.singleton.chalLimit, PlayerSave.singleton.potLimit, PlayerSave.singleton._TableId, "B", null);
            //        PlayerSave.singleton.GameExit = true;
            //    }
            //}
            yield return new WaitForSeconds(0.2f);

        }

        botIndex = -1;
    }
    private void SetFirstPlayerStepByCurrentStepId()
    {
        if (managerInfo.WhichPlayerChanceState < players.Count -1)
        {
            managerInfo.WhichPlayerChanceState++;
        }
        else
        {
            managerInfo.WhichPlayerChanceState = 0;
        }
        if (managerInfo.WhichPlayerChanceState < players.Count)
        {
            //managerInfo.currentPlayerStep = players[managerInfo.WhichPlayerChanceState].GetIdOrderUI();
            managerInfo.currentPlayerStep = managerInfo.WhichPlayerChanceState;
            //////Debug.Log(" <color=red>**********************************************managerInfo.currentPlayerStep---------------------------------------------</color> " + managerInfo.currentPlayerStep);
        }
        else
        {
            //////Debug.Log(" <color=red**********************************************managerInfo.currentPlayerStep else ---------------------------------------------</color> " + managerInfo.currentPlayerStep + "managerInfo.WhichPlayerChanceState "+ managerInfo.WhichPlayerChanceState);
        }
        //for (int i = 0; i < players.Count; i++)
        //{
        //    if (players[i].photonView.viewID == managerInfo.currentPlayerStepID)
        //    {
        //        if (i > 0)
        //        {
        //            managerInfo.currentPlayerStep = i - 1;
        //        }
        //        else
        //        {
        //            managerInfo.currentPlayerStep = players.Count - 1;
        //        }
        //        break;
        //    }
           
        //}
        //managerInfo.currentPlayerStep = 0;
        managerInfo.currentPlayerStepID = 0;
    }
    private void Delay()
    {
        if (isDebug)
        {
            Debug.Log("ReorderChairsPositionsOnTable " + players[0].myUI.IdOrder);
        }
        ReorderChairsPositionsOnTable(players[0].myUI.IdOrder);
    }
    [PunRPC]
    public void ReorderChairsPositionsOnTable(int p_startPositionIndex)
    {
        int __shiftCount = 0;

        while (p_startPositionIndex != 2)
        {
            p_startPositionIndex++;

            if (p_startPositionIndex > 4)
            {
                p_startPositionIndex = 0;
            }
          
            __shiftCount++;
           
        }

        for (int i = 0; i < 5; i++)
        {
            RectTransform __uiRTr = playersUiCollection[i].GetComponent<RectTransform>();
            int __positionIndex = i + __shiftCount;

            
            if (__positionIndex > 4)
            {
                __positionIndex -= 5;
            }
          
            __uiRTr.transform.SetParent(playersUiPositionCollection[__positionIndex]);
            __uiRTr.anchoredPosition3D = Vector3.zero;
            playersUiCollection[i].EnableSlot(__positionIndex);
        }
    }

    public void ReloadPlayerList()
    {
        players = FindObjectsOfType<PlayerManagerPun>().ToList();
    }

    public bool IsMaster(PlayerManagerPun player) => PhotonNetwork.IsMasterClient && !player.playerData.IsBot;

   

    

    [PunRPC]
    public void SyncRoom(string _managerInfo)
    {

        
        managerInfo = JsonUtility.FromJson<ManagerInfo>(_managerInfo);

        PlayerSave.NewRoomName = managerInfo.CurrentRoomID;

        //Debug.Log(PhotonNetwork.LocalPlayer.NickName +"#$#$#$#$ managerInfo " + _managerInfo );
    }

    public void InitPlayer(PlayerManagerPun connectedPlayer)
    {
        ////Debug.LogWarning("InitPlayer " + connectedPlayer.playerData.NamePlayer +" "  +connectedPlayer.playerData.Money);

        if (!players.Contains(connectedPlayer))
        {
            //////Debug.LogWarning("Player not added yet... adding now!");
            players.Add(connectedPlayer);
            if (PhotonNetwork.IsMasterClient)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                    PhotonNetwork.CurrentRoom.IsVisible = false;
                    if (isDebug)
                    {
                        Debug.Log("[Room]: Closed now in Joined Room");
                    }
                }
            }
            UpdateRoomStatus();
        }

        List<int> randomFull = new List<int>();
        PlayerUI emptyUI = null;

        for (int i = 0; i <= players.Count; i++)
        {
            int randomUI = UnityEngine.Random.Range(0, playersUiCollection.Length);

            
            if (randomFull.Contains(randomUI) || playersUiCollection[randomUI].GetIsFull())
                randomFull.Add(randomUI);
            else
            {
                emptyUI = playersUiCollection[randomUI];

                break;
            }
        }

        if (emptyUI == null)
            emptyUI = playersUiCollection.First(x => x.GetIsFull() == false).GetComponent<PlayerUI>();

        emptyUI.SetIsFull(true);

        if (connectedPlayer)
        {
            if (connectedPlayer.photonView != null)
            {
                connectedPlayer.photonView.RPC("InitTableData", RpcTarget.All, JsonUtility.ToJson(tableInfo),
            managerInfo.totalPot);
                connectedPlayer.photonView.RPC("InitUI", RpcTarget.All, emptyUI.name,
                    JsonUtility.ToJson(connectedPlayer.playerData));
            }
            else
            {
                if (connectedPlayer)
                {
                    if (connectedPlayer.localPlayer)
                    {
                        //////Debug.Log("call diconnentttt 6...");
                        connectedPlayer.localPlayer.OnHomeButton();
                    }
                }
            }
        }
        else
        {
            if (connectedPlayer)
            {
                if (connectedPlayer.localPlayer)
                {
                    //////Debug.Log("call diconnentttt 6...");
                    connectedPlayer.localPlayer.OnHomeButton();
                }
            }
        }

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null && players[i] != connectedPlayer && !players[i].playerData.isDisconnect)
            {
                players[i].photonView.RPC("InitUI", RpcTarget.All, players[i].GetNameUI(),
                    JsonUtility.ToJson(players[i].playerData));
            }
        }
        if (isDebug)
        {
            Debug.Log("#$#$#$#$ managerInfo " + JsonUtility.ToJson(managerInfo) + " PhotonNetwork.LocalPlayer.NickName " + PhotonNetwork.LocalPlayer.NickName);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            if (!managerInfo.isStartedGame)
            {
                if (typeTable == eTable.AndarBahar)
                {
                    if (players.Count >= 1)
                    {
                        managerInfo.GameRoom_2 += 1;
                        managerInfo.isStartedGame = true;
                        Invoke("GrabJokerCard", 3);
                    }
                }
                else
                {
                    int CountActivePlayers = 0;
                    foreach(var item in players)
                    {
                        if(item.playerData.isPlayerActive)
                        {
                            CountActivePlayers++;
                        }
                    }
                    if (players.Count >= 2 && CountActivePlayers>=2)
                    {
                        managerInfo.GameRoom_2 += 1;
                        managerInfo.isStartedGame = true;
                        managerInfo.isGivingBoot = 0;//Added by me
                        managerInfo.currentPlayerStep = 0;
                        if (isDebug)
                        {
                            Debug.Log("<color=green>NextGameGlobalInformation*********** " + PhotonNetwork.LocalPlayer.NickName + "* **********************************</color> " + managerInfo.currentPlayerStep);
                        }
                        StopCoroutine(NextGameGlobalInformation());
                        CancelInvoke("GiveCardsToPlayers");
                        Invoke("GiveCardsToPlayers", 3);
                    }
                }
            }
            else
            {
                ////Debug.Log("#$#$#$#$ managerInfo " + JsonUtility.ToJson(managerInfo));
                //StopCoroutine(NextGameGlobalInformation());
                //StartCoroutine(NextGameGlobalInformation());
            }
           
        }
    }

    public void PlayerDisconnected(PlayerManagerPun player)
    {
        if (isDebug)
        {
            Debug.Log("#$#$#$#$ PlayerDisconnected - name: " + player.playerData.NamePlayer);
        }

        PlayerPrefs.SetString(player.playerData._MobileNumber, "");
        if (managerInfo.isGivingCards)
            StopCoroutine("GiveMovingCards");

        int PlayerViewId = 0;
        bool PlayerIsDisconnect = false;
        //int currentChance = -1;
        bool isSentSideShow = false;
        //bool isSideShow = false;
        if (player != null)
        {
            if (player.photonView != null)
            {
                PlayerViewId = player.photonView.ViewID;
                PlayerIsDisconnect = player.playerData.isDisconnect;
                if (isDebug)
                {
                    Debug.Log("playerStepDisconect.... " + managerInfo.currentPlayerStepID + " view Id " + player.photonView.ViewID + " players " + players.Count);
                }
                if (player.photonView.IsMine)
                {
                    if (IsMaster(player) && PhotonNetwork.PlayerList.Length > 1)
                    {
                        if (managerInfo.isNextGameTimer)
                            StopCoroutine(NextGameGlobalInformation());

                        if (managerInfo.isCheckOnWin)
                        {
                            CancelInvoke("CheckOnWin");
                        }
                    }
                }
            }
        }

        playersUiCollection.First(x => x.name == player.GetNameUI()).SetIsFull(false);
        players.Remove(player);

        UpdateRoomStatus();

        player.photonView.RPC("ClearFromGame", RpcTarget.All);

        if (!player.playerData.IsBot && player.photonView.IsMine && PhotonNetwork.IsMasterClient &&
            PhotonNetwork.PlayerList.Length > 1)
        {
            photonView.RPC("SyncRoom", RpcTarget.AllBuffered, JsonUtility.ToJson(managerInfo));
            PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient.GetNext());// PhotonNetwork.otherPlayers[0]);

            return;
        }

        bool playerStepDisconect = false;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null)
            {
                if (players[i].playerData.isDisconnect)
                {
                    if (managerInfo.currentPlayerStepID == players[i].photonView.ViewID)
                        playerStepDisconect = true;

                    if (isDebug)
                    {
                        Debug.Log("playerStepDisconect " + playerStepDisconect + " view Id " + players[i].photonView.ViewID);
                    }

                }

            }

        }
        if (!playerStepDisconect)
        {
            if (PlayerIsDisconnect)
            {
                if (managerInfo.currentPlayerStepID == PlayerViewId)
                {
                    playerStepDisconect = true;
                    managerInfo.IsCurrentLocalPlayer = false;

                }
            }
        }
        
        bool ISCheckOnWin = false;
        ISCheckOnWin = managerInfo.isCheckOnWin;
        bool ISGivingCards = false;
        ISGivingCards = managerInfo.isGivingCards;
        bool IsNextGameTimer = false;
        IsNextGameTimer = managerInfo.isNextGameTimer;
        if (isDebug)
        {
            Debug.Log("PlayerWinGame -------------------PlayerDisconnected------------------" + managerInfo.isGivingCards + " managerInfo.isCheckOnWin " + managerInfo.isCheckOnWin + " " + managerInfo.isSideShowReceiver);
        }
        if (CountPlayersInGame() == 1)
        {
            ////Debug.Log("PlayerWinGame -------------------PlayerDisconnected--------------------PlayerWinGame");
            managerInfo.IsCurrentLocalPlayer = false;
            PlayerWinGame(players[GetNumWinPlayer()]);
        }
        else
        {
            if (managerInfo.isGivingCards)
            {
                managerInfo.IsCurrentLocalPlayer = false;
                CancelInvoke();
                GiveCardsToPlayers();
            }
            else if (managerInfo.isCheckOnWin)
            {
                managerInfo.IsCurrentLocalPlayer = false;
                CheckOnWin();
               
            }
            else if (managerInfo.isNextGameTimer)
            {
                if (isDebug)
                {
                    Debug.Log("NextGameGlobalInformation --------------------------nps isNextGameTimer true");
                }
                managerInfo.IsCurrentLocalPlayer = false;
                StopCoroutine(NextGameGlobalInformation());
                StartCoroutine(NextGameGlobalInformation());
                
            }
            else if (managerInfo.isChaalPlayer)
            {
                managerInfo.isChaalPlayer = false;
                managerInfo.IsCurrentLocalPlayer = false;
                if (isDebug)
                {
                    Debug.Log("nps on 3-------------------------------------------------------------------");
                }
                NextPlayerStep();
                //managerInfo.InAnyNextCondition = true;
            }
            else if (managerInfo.isSideShowReceiver)
            {
                if (managerInfo.playerIdRecievedSideShow == PlayerViewId)
                {
                    managerInfo.isSideShowReceiver = false;
                    managerInfo.playerIdRecievedSideShow = 0;
                    managerInfo.IsCurrentLocalPlayer = false;
                    DeclineSideShow(player);
                }
            }
            else if (playerStepDisconect && !isSentSideShow)//agar is sent side show false hai to ye next player ko call nhi karega kyuki decline wale main automatic next player step chalega
            {
                ////Debug.Log("nps on 4-------------------------------------------------------------------");
                managerInfo.IsCurrentLocalPlayer = false;
                NextPlayerStep();
            }
            
            else
            {
                ////Debug.Log("no need change");
                if (CountPlayersInGame() == 0)
                {
                    if (!ISCheckOnWin && !IsNextGameTimer && !ISGivingCards && !playerStepDisconect && !isSentSideShow)
                    {
                        if (isDebug)
                        {
                            Debug.Log("no need change check condition all false ");
                        }
                        managerInfo.IsCurrentLocalPlayer = false;
                        CheckOnWin();
                       
                    }
                    else
                    {
                        if (isDebug)
                        {
                            Debug.Log("no need change check condition all not false  " + ISCheckOnWin + "_" + IsNextGameTimer + "_" + ISGivingCards + "_" + playerStepDisconect + "_" + isSentSideShow);
                        }
                    }
                }
                else
                {
                    if (isDebug)
                    {
                        Debug.Log("no need change...in else  ");
                    }

                }
            }
        }
        AddServerDataAndBotDataForNonMaster();
    }
   
    public void PauseActivity(PlayerManagerPun player)
    {

        if (isDebug)
        {
            Debug.Log("PauseActivity " + player.playerData.NamePlayer);
        }
        player.playerData.isPause = true;
        //int PlayerViewId = 0;
        //int currentChance = -1;
        bool isSentSideShow = false;
        bool isSideShow = false;
        if (player)
        {
            if (player.photonView != null)
            {
                // ////Debug.Log("PauseActivity in view " + player.playerData.NamePlayer );

                //PlayerViewId = player.photonView.viewID;
                //currentChance = player.GetId();
                isSentSideShow = player.playerData.isSentSideShow;
                isSideShow = player.playerData.isSideShow;

            }

        }
        if (player != null)
        {
            if (isDebug)
            {
                Debug.Log("PauseActivity  inner" + player.playerData.NamePlayer);
            }
            if (managerInfo._IsCurrentMobileNumber == player.playerData._DeviceID)
            {
                managerInfo.IsCurrentPlayerPause = true;
            }
        }
        if (isSideShow && !isSentSideShow)
        {
            if (player.localPlayer != null)
            {
                player.localPlayer.WaitForSixSecond();
                managerInfo.isSideShowReceiver = false;
                managerInfo.playerIdRecievedSideShow = 0;
                managerInfo.playerIdRecievedSideShowName = "";
                managerInfo.playerIdRecievedSideShowDeviceId = "";
            }
        }
        PhotonNetwork.SendAllOutgoingCommands();


        if (PhotonNetwork.IsMasterClient)
        {
            if (photonView != null)//Newly Line Added by me
            {
                photonView.RPC("SyncRoom", RpcTarget.AllBuffered, JsonUtility.ToJson(managerInfo));
            }
        }
    }
    public void UnPauseActivity(PlayerManagerPun player)
    {

        //Debug.Log("UnPauseActivity " + player.playerData.NamePlayer);
        player.playerData.isPause = false;
        
        PhotonNetwork.SendAllOutgoingCommands();

        if (player != null)
        {
            //Debug.Log("UnPauseActivity  inner" + player.playerData.NamePlayer);
            if (managerInfo._IsCurrentMobileNumber == player.playerData._DeviceID)
            {
                managerInfo.IsCurrentPlayerPause = false;
            }
        }
        if (PhotonNetwork.IsMasterClient)
        {
            if (photonView != null)//Newly Line Added by me
            {
                photonView.RPC("SyncRoom", RpcTarget.AllBuffered, JsonUtility.ToJson(managerInfo));
            }
        }
    }
    public void Chaal(PlayerManagerPun player)
    {
        //////Debug.Log($"[{player.playerData.NamePlayer}]: Chaal");

        //if (player.playerData.IsDoubleBoot)
        //{
        //    managerInfo.currentStake *= 2;
        //}//Comment this code by me
        managerInfo.isChaalPlayer = true;
        managerInfo.isChaalPlayerUpdateBoot = true;
        managerInfo.isChaalPlayerUpdateBootAmount = 0;
        if (player.playerData.IsDoubleBoot) // Add this code by me
        {
            player.playerData.currentBootPlayer *= 2;

            if (player.playerData.currentBootPlayer >= tableInfo.chalLimit && tableInfo.chalLimit > 0 && player.playerData.currentBootPlayer > 0)
            {
                player.playerData.currentBootPlayer = tableInfo.chalLimit;
            }
            if (player)
            {
                if (player.localPlayer)
                {
                    player.localPlayer.TextCurrebyBoot(player.playerData.currentBootPlayer);
                }
            }
        }
        double calculateBoot = player.playerData.currentBootPlayer;// CalculatePlayersBoot(player);//Change this code by me

        if (player)//Add this code by me
        {
            if (player.photonView != null)
            {
                player.photonView.RPC("UpdateBootAmount", RpcTarget.All, player.playerData.currentBootPlayer);
            }
        }
        if (typeTable == eTable.Standard || typeTable == eTable.Private)
        {
            if (player.playerData.Money < calculateBoot)
            {
                //////Debug.Log("not money = packed");
                if (player.photonView != null)
                {
                    player.photonView.RPC("PackPlayerByServer", RpcTarget.All);
                }
                ////Debug.Log("nps on 5-------------------------------------------------------------------");
                NextPlayerStep();
            }
            else
            {
                MoneyPlayerToTable(player, calculateBoot, 0, 0 ,"Chaal");

                if (tableInfo.potLimit == 0 || managerInfo.totalPot < tableInfo.potLimit)
                {
                    ////Debug.Log("nps on 6-------------------------------------------------------------------");
                    NextPlayerStep();
                }
                else
                {
                    StartCoroutine("RisePotLimit");
                }
            }
        }
        else
        {
            if (player.playerData.Chips < calculateBoot)
            {
                //////Debug.Log("not chips = packed");
                if (player.photonView != null)
                {
                    player.photonView.RPC("PackPlayerByServer", RpcTarget.All);
                }
                ////Debug.Log("nps on 7-------------------------------------------------------------------");
                NextPlayerStep();
            }
            else
            {
                MoneyPlayerToTable(player, calculateBoot, 0, 0,"Chaal");

                if (tableInfo.potLimit == 0 || managerInfo.totalPot < tableInfo.potLimit)
                {
                    ////Debug.Log("nps on 8-------------------------------------------------------------------");
                    NextPlayerStep();
                }
                else
                {
                    StartCoroutine("RisePotLimit");
                }
            }
        }
        managerInfo.isChaalPlayer = false;
        managerInfo.isChaalPlayerUpdateBoot = false;
        managerInfo.isChaalPlayerUpdateBootAmount = 0;

        if (PhotonNetwork.IsMasterClient)
        {
            if (photonView != null)//Newly Line Added by me
            {
                photonView.RPC("SyncRoom", RpcTarget.AllBuffered, JsonUtility.ToJson(managerInfo));
            }
        }
    }
    

    public void PackPlayerFromClient()
    {
        managerInfo.isCheckOnWin = true;
        CancelInvoke("CheckOnWin");
        Invoke("CheckOnWin", 2);

        if (PhotonNetwork.IsMasterClient)
        {
            if (photonView != null)//Newly Line Added by me
            {
                photonView.RPC("SyncRoom", RpcTarget.AllBuffered, JsonUtility.ToJson(managerInfo));
            }
        }

    }


    public void StartSideShow(PlayerManagerPun firstPlayer)
    {

        //Debug.Log("<color=red> --------StartSideShow -----------</color> " + firstPlayer.playerData.NamePlayer);
        if (firstPlayer == null)
            return;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == firstPlayer)
                managerInfo.playerIdStartedSideShow = i;
        }
        if (firstPlayer != null)
        {
            // playerStartedSideShow = firstPlayer;
            StartCoroutine("SideShowCorotine", firstPlayer);
        }
    }

    public void AcceptSideShow(PlayerManagerPun callPlayer)
    {
        ////Debug.Log("<color=red> --------AcceptSideShow -----------</color> " + callPlayer.playerData.NamePlayer);
        CancelInvoke("WaitForSixSecond");
        managerInfo.isSideShowReceiver = false;
        managerInfo.playerIdRecievedSideShow = 0;
        managerInfo.playerIdRecievedSideShowName = "";
        if (callPlayer != null)
            StartCoroutine("AcceptShowCorotine", callPlayer);

        if (PhotonNetwork.IsMasterClient)
        {
            if (photonView != null)//Newly Line Added by me
            {
                photonView.RPC("SyncRoom", RpcTarget.AllBuffered, JsonUtility.ToJson(managerInfo));
            }
        }
    }

    public void MasterDeclineSideShow(string NamePlayer)
    {
        ////Debug.Log("<color=red> --------MasterDeclineSideShow -----------</color> " + NamePlayer);
        managerInfo.isSideShowReceiver = false;
        managerInfo.playerIdRecievedSideShow = 0;
        CancelInvoke("WaitForSixSecond");

        foreach (var item in players)
        {
            if (item != null)
            {
                if (item.photonView != null)
                {

                    item.photonView.RPC("PanelOff", RpcTarget.All, NamePlayer + " rejected side show");

                }
            }
        }
        //////Debug.Log("nps on 442422");
        ////Debug.Log("nps on 9-------------------------------------------------------------------");
        NextPlayerStep();


    }
    public void DeclineSideShow(PlayerManagerPun firstPlayer)
    {
        ////Debug.Log("<color=red> --------DeclineSideShow -----------</color> " + firstPlayer.playerData.NamePlayer);
        managerInfo.isSideShowReceiver = false;
        managerInfo.playerIdRecievedSideShow = 0;
        
        managerInfo.isCurrentSideShow = false;
        managerInfo.isCurrentSentSideShow = false;
        
        CancelInvoke("WaitForSixSecond");
        if (firstPlayer == null)
        {
            ////Debug.Log("nps on 10-------------------------------------------------------------------");
            NextPlayerStep();
        }
        else
        {
            foreach (var item in players)
            {
                item.photonView.RPC("GlobalInfo", RpcTarget.All,
                    firstPlayer.playerData.NamePlayer + " rejected side show");
            }

            //MoneyPlayerToTable(firstPlayer,firstPlayer.playerData.currentBootPlayer , 0, 0);//managerInfo.currentStake//Change this code by me
            ////Debug.Log("nps on 11-------------------------------------------------------------------");
            NextPlayerStep();
        }
    }

    public void AddChat(string _textAdd, int uiOrder)
    {
        // managerInfo.textChat = managerInfo.textChat + "\r\n" + _textAdd;
        foreach (var item in players)
        {
            if (item != null)
            {
                if (item.photonView != null)
                {
                    //item.photonView.RPC("NewChat", RpcTarget.All, managerInfo.textChat);
                    item.photonView.RPC("NewChat", RpcTarget.All, _textAdd, uiOrder);
                }
            }
        }
    }

    private double CalculatePlayersBoot(PlayerManagerPun playerData)
    {
        double bootForPlayer = playerData.playerData.currentBootPlayer;// managerInfo.currentStake;//Change this code by me
        bool previousSeen = FindPreviousPlayer().playerData.IsSeenCard;
        bool currentSeen = playerData.playerData.IsSeenCard;

        if (!currentSeen && previousSeen)
            bootForPlayer =  playerData.playerData.currentBootPlayer; //bootForPlayer /= 2;//Change this code by me
        else if (currentSeen && !previousSeen)
            bootForPlayer *= 2;

        return bootForPlayer;
    }


    private PlayerManagerPun FindPreviousPlayer(PlayerManagerPun firstPlayer)
    {
        //int previousPlayer = firstPlayer.myUI.MyPositionID;
        int previousPlayer = players.FindIndex(a => a.playerData._DeviceID.Equals(firstPlayer.playerData._DeviceID));
        //////Debug.Log("previousPlayer " + previousPlayer);
        for (int i = 0; i < players.Count; i++)
        {
            previousPlayer--;

            if (previousPlayer < 0)
                previousPlayer = players.Count - 1;

            if (previousPlayer >= 0 && previousPlayer < players.Count)
            {
                if (players[previousPlayer] != null)
                {
                    if (players[previousPlayer].playerData.playerType == ePlayerType.PlayerStartGame && !players[previousPlayer].playerData.IsPacked)
                    {
                        //////Debug.Log("previousPlayer " + previousPlayer + "players[previousPlayer] "+ players[previousPlayer].myUI.MyPositionID);
                        return players[previousPlayer];
                    }
                }
            }
        }

        return null;
    }
    public bool FindPreviousPlayerForShowOrSideShow(PlayerManagerPun firstPlayer)
    {
        //int previousPlayer = firstPlayer.myUI.MyPositionID;
        int previousPlayer = players.FindIndex(a => a.playerData._DeviceID.Equals(firstPlayer.playerData._DeviceID));
        //////Debug.Log("previousPlayer " + previousPlayer);
        for (int i = 0; i < players.Count; i++)
        {
            previousPlayer--;

            if (previousPlayer < 0)
                previousPlayer = players.Count - 1;

            if (previousPlayer >= 0 && previousPlayer < players.Count)
            {
                if (players[previousPlayer] != null)
                {
                    if (players[previousPlayer].playerData.playerType == ePlayerType.PlayerStartGame && players[previousPlayer]!=firstPlayer && !players[previousPlayer].playerData.IsPacked && players[previousPlayer].playerData.IsSeenCard)
                    {
                        //////Debug.Log("previousPlayer " + previousPlayer + "players[previousPlayer] " + players[previousPlayer].myUI.MyPositionID);
                        return true;
                    }
                    else if (players[previousPlayer].playerData.playerType == ePlayerType.PlayerStartGame && players[previousPlayer] != firstPlayer && !players[previousPlayer].playerData.IsPacked && !players[previousPlayer].playerData.IsSeenCard)
                    {
                        //////Debug.Log("previousPlayer " + previousPlayer + "players[previousPlayer] " + players[previousPlayer].myUI.MyPositionID);
                        return false;
                    }
                }
            }
        }

        return false;
    }
    private PlayerManagerPun FindPreviousPlayer()
    {
        int previousPlayer = managerInfo.currentPlayerStep;
        for (int i = 0; i < players.Count; i++)
        {
            previousPlayer--;

            if (previousPlayer < 0)
                previousPlayer = players.Count - 1;

            if (previousPlayer >= 0 && previousPlayer < players.Count)
            {
                if (players[previousPlayer] != null)
                {
                    if (players[previousPlayer].playerData.playerType == ePlayerType.PlayerStartGame && !players[previousPlayer].playerData.IsPacked)
                    {
                        return players[previousPlayer];
                    }
                }
            }
        }

        return null;
    }
    public void GiveCardsToPlayersFromPlayerUIOnceOnly()
    {
        if (isDebug)
        {
            Debug.Log("GiveCardsToPlayersFromPlayerUIOnceOnly");
        }
        CancelInvoke();
        Invoke("GiveCardsToPlayers", 3f);
    }
    private void GiveCardsToPlayers()
    {
        if (isDebug)
        {
            Debug.Log("GiveCardsToPlayers");
        }
        players = FindObjectsOfType<PlayerManagerPun>().ToList();
        players = players.OrderBy(x => x.GetIdOrderUI()).ToList();

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null)
            {
                players[i].photonView.RPC("SetInGame", RpcTarget.All);
                //////Debug.Log("GiveCardsToPlayers <color=red>SetDealerIcon**********************************************i </color>" + i + " managerInfo.currentPlayerStep " + managerInfo.currentPlayerStep);
                players[i].photonView.RPC("SetDealerIcon", RpcTarget.All, i, managerInfo.currentPlayerStep);
            }
        }

        if (players.Count < 2)
        {
            managerInfo.isStartedGame = false;
            managerInfo.isGivingBoot = 0;//Added by me
            return;
        }

        foreach (var p in players)
        {
            if (typeTable == eTable.Standard || typeTable == eTable.Private)
            {
                if (p.playerData.Money > tableInfo.startBoot) continue;
            }
            else
            {
                if (p.playerData.Chips > tableInfo.startBoot) continue;
            }
            managerInfo.isStartedGame = false;
            return;
        }
        int pCounter = 0;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null)
            {
                if (players[i].playerData.playerType == ePlayerType.PlayerStartGame)
                {
                    players[i].playerData.currentBootPlayer = tableInfo.startBoot;
                    pCounter++;
                }
            }
        }
        if (pCounter < 2)
        {
            managerInfo.isStartedGame = false;
            managerInfo.isGivingBoot = 0;//Added by me
            return;
        }
        managerInfo.GameRoom_2 += 1;
        managerInfo.isStartedGame = true;
        managerInfo.currentStake = tableInfo.startBoot;
        deckManager.NewDeck();
        //////Debug.Log("next global information -----------------------------------");
        StopCoroutine(NextGameGlobalInformation());
        StopCoroutine(GiveMovingCards());
        StartCoroutine(GiveMovingCards());
        if (PhotonNetwork.IsMasterClient)
        {
            if (photonView != null)//Newly Line Added by me
            {
                photonView.RPC("SyncRoom", RpcTarget.AllBuffered, JsonUtility.ToJson(managerInfo));
            }
        }
    }
    public void DetectPlayersAfterDestroy()
    {
        //managerInfo.InAnyNextCondition = true;
        CancelInvoke("RefreshPlayers");
        Invoke("RefreshPlayers", 1f);
    }
    public void RefreshPlayers()
    {
        players = FindObjectsOfType<PlayerManagerPun>().ToList();
        players = players.OrderBy(x => x.GetIdOrderUI()).ToList();
        ////Debug.Log("nps on 12-------------------------------------------------------------------");
        NextPlayerStep();
    }
    private void PlayerDontMakeStep()
    {
        players = FindObjectsOfType<PlayerManagerPun>().ToList();
        players = players.OrderBy(x => x.GetIdOrderUI()).ToList();
        if (managerInfo.currentPlayerStep < players.Count)
        {
            if (players[managerInfo.currentPlayerStep] == null)
            {
                ////Debug.Log("nps on 13-------------------------------------------------------------------");
                NextPlayerStep();
                return;
            }
            if (players[managerInfo.currentPlayerStep].photonView == null)
            {
                ////Debug.Log("nps on 14-------------------------------------------------------------------");
                NextPlayerStep();
                return;
            }
            players[managerInfo.currentPlayerStep].photonView.RPC("PackPlayerByServer", RpcTarget.All);
        }
    }
    private void NextPlayerStep()
    {
        if (photonView != null)
        {
            if (isDebug)
            {
                Debug.Log("<color=red>NextPlayerStep -------------------TPP--------------------</color> " + PhotonNetwork.IsMasterClient + " photonView " + photonView.IsMine + " photonView.Controller.IsMasterClient " + photonView.Controller.IsMasterClient + " ppp " + photonView.Controller.NickName);
            }
        }
        if (CountPlayersInGame() == 1)
        {
            //////Debug.Log("PlayerWinGame -------------------NextPlayerStep--------------------PlayerWinGame");
            PlayerWinGame(players[GetNumWinPlayer()]);

            return;
        }
        CheckPlayerIds();
        managerInfo.AllPhotonIds = "";
        managerInfo.playerIdRecievedSideShowDeviceId = "";
        bool findPlayer = false;

        int ActivePlayers = 0;
        string result = "";
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null)
            {
                if (players[i].playerData.playerType == ePlayerType.PlayerStartGame && !players[i].playerData.IsPacked)
                {

                    ActivePlayers++;
                    if(players[i].localPlayer!=null)
                    {
                        players[i].localPlayer.PanelAcceptOff();
                    }
                }
                
            }
        }

        if (ActivePlayers <= 2)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] != null)
                {
                    if (players[i].playerData.playerType == ePlayerType.PlayerStartGame && !players[i].playerData.IsPacked)
                    {
                        if (players[i].photonView != null)
                        {
                            players[i].photonView.RPC("ReplaceSideWithShow", RpcTarget.All, 1);
                            players[i].photonView.RPC("StopStep", RpcTarget.All);
                            CountdownTimer.Stoptimer();
                            //players[i].photonView.RPC("OnlySetPLayerDataInManager", RpcTarget.All, JsonUtility.ToJson(players[i].playerData));
                            result += players[i].playerData._DeviceID;
                            result += ";";
                        }
                    }

                }
            }
        }
        else if (ActivePlayers > 2)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] != null)
                {
                    if (players[i].playerData.playerType == ePlayerType.PlayerStartGame && !players[i].playerData.IsPacked)
                    {
                        if (players[i].photonView != null)
                        {

                            players[i].photonView.RPC("ReplaceSideWithShow", RpcTarget.All, 0);
                            players[i].photonView.RPC("StopStep", RpcTarget.All);
                            CountdownTimer.Stoptimer();
                            //players[i].photonView.RPC("OnlySetPLayerDataInManager", RpcTarget.All, JsonUtility.ToJson(players[i].playerData));
                            result += players[i].playerData._DeviceID;
                            result += ";";
                        }

                    }

                }
            }
        }

        for (int i = 0; i < players.Count; i++)
        {

            //Debug.Log("before managerInfo.currentPlayerStep " + managerInfo.currentPlayerStep);
            if (managerInfo.currentChance > -1)
            {
                managerInfo.currentPlayerStep = managerInfo.currentChance;
                //Debug.Log("**********************************************managerInfo.currentChance-------------------------------------------------------------------- " + managerInfo.currentChance);
            }
            else
            {
                managerInfo.currentPlayerStep++;
                //Debug.Log("**********************************************NewNextChance else--------------------------------------------------------------------------- " + managerInfo.currentPlayerStep);
            }
            managerInfo.currentChance = -1;
            if (managerInfo.currentPlayerStep > players.Count - 1)
            {
                managerInfo.currentPlayerStep = 0;
                //Debug.Log("**********************************************Reset NewNextChance else--------------------------------------------------------------------------- " + managerInfo.currentPlayerStep);
            }

            managerInfo.currentPlayerStepID = players[managerInfo.currentPlayerStep].photonView.ViewID;

            if (players[managerInfo.currentPlayerStep].playerData.playerType == ePlayerType.PlayerStartGame && !players[managerInfo.currentPlayerStep].playerData.IsPacked)
            {
                

                if (!players[managerInfo.currentPlayerStep].playerData.IsSeenCard)
                {
                    double HighestBlind = GetHighestBlind();
                    double HighBlindChaal = GetHighestChaal();

                    if (HighBlindChaal > HighestBlind)
                    {
                        HighestBlind = HighBlindChaal / 2;
                    }

                    if (tableInfo.chalLimit > 0 && HighestBlind >= tableInfo.chalLimit && HighestBlind > 0)
                    {
                        HighestBlind = tableInfo.chalLimit;
                        ////Debug.Log("High Chaal in limit 3");
                    }
                    ////Debug.Log("HighBlindChaal "+ HighBlindChaal+"HighestBlind " + HighestBlind + " managerInfo.currentStake " + managerInfo.currentStake +" "+ players[managerInfo.currentPlayerStep].playerData.currentBootPlayer);
                    if (HighestBlind > 0 && HighestBlind >= players[managerInfo.currentPlayerStep].playerData.currentBootPlayer)
                    {
                        if (players[managerInfo.currentPlayerStep] != null)
                        {
                            if (players[managerInfo.currentPlayerStep].photonView != null)
                            {
                                players[managerInfo.currentPlayerStep].playerData.currentBootPlayer = HighestBlind;
                                players[managerInfo.currentPlayerStep].playerData.IsLocalPlayer = true;
                                players[managerInfo.currentPlayerStep].photonView.RPC("StartStep", RpcTarget.All, HighestBlind);
                                CountdownTimer.StartTimer(25f);//, players[managerInfo.currentPlayerStep]);
                            }
                        }

                    }
                    else
                    {
                        ////Debug.Log("HighestBlind..1.. " + HighestBlind + " managerInfo.currentStake " + managerInfo.currentStake + " " + players[managerInfo.currentPlayerStep].playerData.currentBootPlayer);
                        if (managerInfo.currentStake >= players[managerInfo.currentPlayerStep].playerData.currentBootPlayer)
                        {
                            if (players[managerInfo.currentPlayerStep] != null)
                            {
                                if (players[managerInfo.currentPlayerStep].photonView != null)
                                {
                                    players[managerInfo.currentPlayerStep].playerData.currentBootPlayer = managerInfo.currentStake;
                                    players[managerInfo.currentPlayerStep].playerData.IsLocalPlayer = true;
                                    players[managerInfo.currentPlayerStep].photonView.RPC("StartStep", RpcTarget.All, managerInfo.currentStake);
                                    CountdownTimer.StartTimer(25f);//, players[managerInfo.currentPlayerStep]);
                                }
                            }

                        }
                        else
                        {
                            if (players[managerInfo.currentPlayerStep] != null)
                            {
                                if (players[managerInfo.currentPlayerStep].photonView != null)
                                {
                                    players[managerInfo.currentPlayerStep].playerData.IsLocalPlayer = true;
                                    players[managerInfo.currentPlayerStep].photonView.RPC("StartStep", RpcTarget.All, players[managerInfo.currentPlayerStep].playerData.currentBootPlayer);
                                    CountdownTimer.StartTimer(25f);//, players[managerInfo.currentPlayerStep]);
                                }
                            }

                        }

                    }
                }
                else
                {
                    double HighestBlind2 = GetHighestBlind();
                    double HighChaal = GetHighestChaal();

                    // //Debug.Log(" HighestBlind  in limit 4....... " + HighestBlind2);
                    ////Debug.Log(" HighChaal  in limit 4....... " + HighChaal);
                    if (HighestBlind2 > 0 && HighestBlind2 >= HighChaal && HighChaal >= 0)
                    {
                        HighChaal = HighestBlind2 * 2;
                        ////Debug.Log(" HighChaal  in limit 4 " + HighChaal);
                    }
                    if (tableInfo.chalLimit > 0 && HighChaal >= tableInfo.chalLimit && HighChaal > 0)
                    {
                        HighChaal = tableInfo.chalLimit;
                        ////Debug.Log("High Chaal in limit 1");
                    }
                    ////Debug.Log("HighestBlind " + HighestBlind+ " HighChaal.... " + HighChaal + " managerInfo.currentStake " + managerInfo.currentStake + " " + players[managerInfo.currentPlayerStep].playerData.currentBootPlayer);
                    if (HighChaal > 0 && HighChaal >= players[managerInfo.currentPlayerStep].playerData.currentBootPlayer)
                    {
                        if (players[managerInfo.currentPlayerStep] != null)
                        {
                            if (players[managerInfo.currentPlayerStep].photonView != null)
                            {
                                players[managerInfo.currentPlayerStep].playerData.currentBootPlayer = HighChaal;
                                players[managerInfo.currentPlayerStep].playerData.IsLocalPlayer = true;
                                players[managerInfo.currentPlayerStep].photonView.RPC("StartStep", RpcTarget.All, HighChaal);
                                CountdownTimer.StartTimer(25f);//, players[managerInfo.currentPlayerStep]);
                            }
                        }

                    }
                    else
                    {
                        ////Debug.Log("HighestBlind2...5... " + HighestBlind2 + " HighChaal.... " + HighChaal + " managerInfo.currentStake " + managerInfo.currentStake + " " + players[managerInfo.currentPlayerStep].playerData.currentBootPlayer);
                        if (managerInfo.currentStake >= players[managerInfo.currentPlayerStep].playerData.currentBootPlayer)
                        {
                            if (players[managerInfo.currentPlayerStep] != null)
                            {
                                if (players[managerInfo.currentPlayerStep].photonView != null)
                                {
                                    players[managerInfo.currentPlayerStep].playerData.currentBootPlayer = managerInfo.currentStake;
                                    players[managerInfo.currentPlayerStep].playerData.IsLocalPlayer = true;
                                    players[managerInfo.currentPlayerStep].photonView.RPC("StartStep", RpcTarget.All, managerInfo.currentStake);
                                    CountdownTimer.StartTimer(25f);//, players[managerInfo.currentPlayerStep]);
                                }
                            }

                        }
                        else
                        {
                            ////Debug.Log("HighestBlind2...7... " + HighestBlind2 + " HighChaal.... " + HighChaal + " managerInfo.currentStake " + managerInfo.currentStake + " " + players[managerInfo.currentPlayerStep].playerData.currentBootPlayer);
                            if (players[managerInfo.currentPlayerStep] != null)
                            {
                                if (players[managerInfo.currentPlayerStep].photonView != null)
                                {
                                    players[managerInfo.currentPlayerStep].playerData.IsLocalPlayer = true;
                                    players[managerInfo.currentPlayerStep].photonView.RPC("StartStep", RpcTarget.All, players[managerInfo.currentPlayerStep].playerData.currentBootPlayer);
                                    CountdownTimer.StartTimer(25f);//, players[managerInfo.currentPlayerStep]);
                                }
                            }
                        }


                    }
                    //players[managerInfo.currentPlayerStep].photonView.RPC("StartStep", RpcTarget.All, managerInfo.currentStake);
                }
                if (players[managerInfo.currentPlayerStep] != null)
                    {
                        if (players[managerInfo.currentPlayerStep].photonView != null)
                        {
                            
                                managerInfo.currentPlayerStepID = players[managerInfo.currentPlayerStep].photonView.ViewID;
                                managerInfo.IsCurrentIndex = players[managerInfo.currentPlayerStep].playerData._DeviceID;
                                managerInfo.IsCurrentNamePlayer = players[managerInfo.currentPlayerStep].playerData.NamePlayer;
                                managerInfo._IsCurrentMobileNumber = players[managerInfo.currentPlayerStep].playerData._DeviceID;
                                managerInfo.IsCurrentBot = players[managerInfo.currentPlayerStep].playerData.IsBot;
                                managerInfo.IsCurrentPacked = players[managerInfo.currentPlayerStep].playerData.IsPacked;
                                managerInfo.isCurrentSentSideShow = players[managerInfo.currentPlayerStep].playerData.isSentSideShow;
                                managerInfo.isCurrentSideShow = players[managerInfo.currentPlayerStep].playerData.isSideShow;
                                managerInfo.IsCurrentTimer = players[managerInfo.currentPlayerStep].playerData._CurrentTimer;
                                managerInfo.IsCurrentLocalPlayer = players[managerInfo.currentPlayerStep].playerData.IsLocalPlayer;
                                managerInfo.IsCurrentSeenCard = players[managerInfo.currentPlayerStep].playerData.IsSeenCard;
                                managerInfo.IsCurrentHost = players[managerInfo.currentPlayerStep].IsMaster2;
                                managerInfo.IsCurrentPlayerDisconnected = players[managerInfo.currentPlayerStep].playerData.isDisconnect;
                                managerInfo.AllPhotonIds = result;
                                photonView.RPC("SyncRoom", RpcTarget.AllBuffered, JsonUtility.ToJson(managerInfo));
                            
                            ////Debug.Log("in NextPlayerStep.... " + managerInfo.currentPlayerStepID + " view Id " + players[managerInfo.currentPlayerStep].photonView.viewID);
                        }
                    }
                
                findPlayer = true;

                break;
            }
        }

        if (!findPlayer && players.Count > 1)
        {
            ////Debug.Log("PlayerWinGame -------------------NextPlayerStep findPlayer false--------------------PlayerWinGame");
            PlayerWinGame(players[GetNumWinPlayer()]);
        }
        
    }
    private void PreviousNextPlayerStep()
    {
        if (CountPlayersInGame() == 1)
        {
            ////Debug.Log("PlayerWinGame -------------------PreviousNextPlayerStep--------------------PlayerWinGame");
            PlayerWinGame(players[GetNumWinPlayer()]);

            return;
        }
        
        bool findPlayer = false;

        for (int i = 0; i < players.Count; i++)
        {
            managerInfo.currentPlayerStep++;
            ////Debug.Log("***************************************PreviousNextPlayerStep " + managerInfo.currentPlayerStep);

            if (managerInfo.currentPlayerStep > players.Count - 1)
                managerInfo.currentPlayerStep = 0;

            ////Debug.Log("***************************************PreviousNextPlayerStep after " + managerInfo.currentPlayerStep);

            managerInfo.currentPlayerStepID = players[managerInfo.currentPlayerStep].photonView.ViewID;

            if (players[managerInfo.currentPlayerStep].playerData.playerType == ePlayerType.PlayerStartGame && !players[managerInfo.currentPlayerStep].playerData.IsPacked)
            {
                players[managerInfo.currentPlayerStep].photonView.RPC("StartStep", RpcTarget.All, managerInfo.currentStake);
                findPlayer = true;

                break;
            }
        }

        if (!findPlayer && players.Count > 1)
        {
            ////Debug.Log("PlayerWinGame -------------------PreviousNextPlayerStep findPlayer false--------------------PlayerWinGame");
            PlayerWinGame(players[GetNumWinPlayer()]);
        }
    }
    public double GetHighestChaal()
    {
        double HighestChaal = 0;
        
       
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] != null)
                {
                    if (players[i].playerData.playerType == ePlayerType.PlayerStartGame && players[i].playerData.IsSeenCard)
                    {
                        if (players[i].playerData.currentBootPlayer >= HighestChaal)
                        {
                            HighestChaal = players[i].playerData.currentBootPlayer;
                        }
                    }
                }
            }
        
        return HighestChaal;
    }
    public double GetHighestBlind()
    {
        double HighestBlind = 0;


        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null)
            {
                if (players[i].playerData.playerType == ePlayerType.PlayerStartGame && !players[i].playerData.IsSeenCard)
                {
                    if (players[i].playerData.currentBootPlayer >= HighestBlind)
                    {
                        HighestBlind = players[i].playerData.currentBootPlayer;
                    }
                }
            }
        }

        return HighestBlind;
    }
    


    private void CheckOnWin()
    {
        managerInfo.isCheckOnWin = false;
        if (CountPlayersInGame() == 1)
        {
            ////Debug.Log("PlayerWinGame -------------------CheckOnWin--------------------PlayerWinGame");
            PlayerWinGame(players[GetNumWinPlayer()]);
        }
        else
        {
            if (CountPlayersInGame() == 0)
            {
                double worstScene = managerInfo.totalPot;
                ResetGame();
                if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
                {
                    if (PhotonNetwork.InRoom)
                    {
                        PlayerSave.singleton.CallUpdateAmount2(0, PhotonNetwork.CurrentRoom.Name, "-", "Worst_" + worstScene.ToString(),"P", managerInfo.GameRoom_2.ToString(), managerInfo.CurrentRoomID.ToString());
                    }
                    else
                    {
                        PlayerSave.singleton.CallUpdateAmount2(0, PlayerSave.FullRoomName, "-", "Worst_" + worstScene.ToString(), "P", managerInfo.GameRoom_2.ToString(), managerInfo.CurrentRoomID.ToString());
                    }
                }
            }
            else
            {
                players = FindObjectsOfType<PlayerManagerPun>().ToList();
                players = players.OrderBy(x => x.GetIdOrderUI()).ToList();
                //Debug.Log("nps on 15-------------------------------------------------------------------");
                NextPlayerStep();
            }
        }
    }
    private void ResetGame()
    {
        ////Debug.Log("reset game...");
        managerInfo.totalPot = 0;
        managerInfo.currentPlayerStepID = 0;
        managerInfo.playerIdStartedSideShow = 0;
        managerInfo.playerIdRecievedSideShowDeviceId = "";
        managerInfo.playerIdRecievedSideShowName = "";
        managerInfo.WhichPlayerChanceState = 0;
        managerInfo.IsCurrentLocalPlayer = false;
        managerInfo.isCurrentSentSideShow = false;
        managerInfo.IsCurrentBot = false;
        managerInfo.IsCurrentHost = false;
        managerInfo.IsCurrentIndex = "";
        managerInfo.IsCurrentNamePlayer = "";
        managerInfo.IsCurrentSeenCard = false;
        managerInfo.IsCurrentPlayerDestroyed = false;
        managerInfo.IsCurrentPlayerDisconnected = false;
        managerInfo.IsCurrentPlayerPause = false;
        managerInfo.IsCurrentTimer = 0f;
        managerInfo.isCurrentSideShow = false;
        managerInfo.IsCurrentPacked = false;
        managerInfo._IsCurrentMobileNumber = "";
        
        managerInfo.currentPlayerStep = 0;
        ////Debug.Log("<color=green>***************************************ResetGame after </color>" + managerInfo.currentPlayerStep);
        if (players.Count > 0)
        {
            foreach (var item in players)
            {
                if (item != null)
                {
                    if (item.photonView != null)
                    {
                        item.photonView.RPC("SetTotal", RpcTarget.All, managerInfo.totalPot);
                    }
                }
            }
        }
        if (players.Count >= 2)
        {
            managerInfo.currentPlayerStep = 0;
            ////Debug.Log("<color=green>NextGameGlobalInformation***************************************ResetGame after.... </color>" + managerInfo.currentPlayerStep);
            managerInfo.GameRoom_2 += 1;
            managerInfo.isStartedGame = true;
            managerInfo.isGivingBoot = 0;//Added by me
            StopCoroutine(NextGameGlobalInformation());
            StartCoroutine(NextGameGlobalInformation());
        }
        else
        {
            managerInfo.isStartedGame = false;
            managerInfo.isGivingBoot = 0;//Added by me
        }
    }
	private bool showOutofLimitPopUp=false;
    private void PlayerWinGame(PlayerManagerPun playerWin)
    {
		showOutofLimitPopUp=false;

        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
			try
			{
	            if (Camera.main.GetComponent<AudioSource>())
	            {
	                if (!Camera.main.GetComponent<AudioSource>().isPlaying)
	                {
	                    Camera.main.GetComponent<AudioSource>().Play();
	                }
	            }
			}
			catch
			{

			}
        }
		if(playerWin!=null)
		{
			if(playerWin.photonView!=null)
			{
        		playerWin.photonView.RPC("WinHand", RpcTarget.All, managerInfo.totalPot);
			}
		}
        managerInfo.totalPot = 0;
        managerInfo.currentPlayerStepID = 0;
        managerInfo.playerIdStartedSideShow = 0;
        managerInfo.isStartedGame = false;
        managerInfo.IsCurrentBot = false;
        managerInfo.IsCurrentHost = false;
        managerInfo.IsCurrentIndex = "";
        managerInfo.IsCurrentLocalPlayer = false;
        managerInfo.IsCurrentNamePlayer = "";
        managerInfo.IsCurrentPacked = false;
        managerInfo.IsCurrentPlayerDestroyed = false;
        managerInfo.IsCurrentPlayerDisconnected = false;
        managerInfo.IsCurrentSeenCard = false;
        managerInfo.isCurrentSentSideShow = false;
        managerInfo.isCurrentSideShow = false;
        managerInfo.IsCurrentTimer = 0f;
        managerInfo._IsCurrentMobileNumber = "";
        managerInfo.AllPhotonIds = "";
        managerInfo.IsCurrentPlayerPause = false;
        GetBotFromServer = false;
        foreach (var item in players)
        {
            item.photonView.RPC("SetTotal", RpcTarget.All, managerInfo.totalPot);
            item.photonView.RPC("StopStep", RpcTarget.All);
            CountdownTimer.Stoptimer();
            if (item.localPlayer != null)
            {
                item.localPlayer.PanelAcceptOff();
                item.localPlayer.DeactivateAllButtons();//Newly Added by me
				if (item.localPlayer) item.localPlayer.OnOpenWithdrawGamePanel();
            }
        }
		bool forcefullyInToOutBotFromTable=false;
		int forcefullyInToOutBot = 0;
        if (players.Count >= 2)
        {
            if(PhotonNetwork.PlayerList.Length>=1)//if (players.Count >= PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                managerInfo.currentBotHand++;
                if (isDebug)
                {
                    Debug.Log("StaticValues.BotHand " + StaticValues.BotHand);
                }
				try
				{
					if (botsData.Count > 0)
					{
						while(forcefullyInToOutBot<botsData.Count)
						{
							try
							{
								var botData55 = botsData[forcefullyInToOutBot];
								var p55 = players.Find(x => x.playerData.IsBot && x.playerData.NamePlayer == botData55.NamePlayer);

								if(p55!=null)
								{
									if ((float)tableInfo.startBoot <= (float)p55.playerData.Money)
									{
										if(tableInfo.potLimit > 0 )
										{
											if ((float)(tableInfo.potLimit/ 2) <= (float)p55.playerData.Money)
											{
												forcefullyInToOutBotFromTable=false;
												forcefullyInToOutBot++;
											}
											else
											{
												forcefullyInToOutBotFromTable=true;
												try
												{
													var botData22 = botsData[forcefullyInToOutBot];
													var p22 = players.Find(x => x.playerData.IsBot && x.playerData.NamePlayer == botData22.NamePlayer);
													if (p22 != null)
													{
														
															if (PhotonNetwork.IsMasterClient)
															{

																if (p22.playerData.IsBot)
																{
																	PlayerSave.singleton.CallGameExitForBotOnly(p22.playerData._MobileNumber, 0, "B", null);
																}
															}
															p22.Disconnect();
															botsData.RemoveAt(forcefullyInToOutBot);
															botIndex = botsData.Count - 1;
															PhotonNetwork.Destroy(p22.gameObject);
													}
												}
												catch
												{
													forcefullyInToOutBotFromTable=false;
													forcefullyInToOutBot++;
												}
											}   
										}
										else
										{
											forcefullyInToOutBotFromTable=false;
											forcefullyInToOutBot++;
										}
									}
									else
									{
										forcefullyInToOutBotFromTable=true;
										try
										{
											var botData22 = botsData[forcefullyInToOutBot];
											var p22 = players.Find(x => x.playerData.IsBot && x.playerData.NamePlayer == botData22.NamePlayer);
											if (p22 != null)
											{

												if (PhotonNetwork.IsMasterClient)
												{

													if (p22.playerData.IsBot)
													{
														PlayerSave.singleton.CallGameExitForBotOnly(p22.playerData._MobileNumber, 0, "B", null);
													}
												}
												p22.Disconnect();
												botsData.RemoveAt(forcefullyInToOutBot);
												botIndex = botsData.Count - 1;
												PhotonNetwork.Destroy(p22.gameObject);
											}
										}
										catch
										{
											forcefullyInToOutBotFromTable=false;
											forcefullyInToOutBot++;
										}
									}
								}
							}
							catch
							{
								forcefullyInToOutBotFromTable=false;
								forcefullyInToOutBot++;
							}
						}
					}
				}
				catch
				{
					forcefullyInToOutBotFromTable=false;
					forcefullyInToOutBot++;
				}

				try
				{
					if(players.Count>0)
					{
						var currentPlayer = players.Find(x => !x.playerData.IsBot && x.playerData._DeviceID.Equals(PlayerSave.singleton.GetUserId()));
						if(currentPlayer!=null)
						{
							if ((float)tableInfo.startBoot  <= (float)PlayerSave.singleton.GetCurrentMoney())
							{
								if (tableInfo.potLimit> 0)
								{
									if ((float)(tableInfo.potLimit / 2) <= (float)PlayerSave.singleton.GetCurrentMoney())
									{
										//not show popup
										showOutofLimitPopUp=false;

									}
									else
									{
										//showpopup
										showOutofLimitPopUp=true;
									} 
								}
								else
								{
									//not show popup
									showOutofLimitPopUp=false;
								}
							}
							else
							{
								//showpopup
								showOutofLimitPopUp=true;
							}
						}
					}
				}
				catch
				{
					//not show popup
					showOutofLimitPopUp=false;
				}
				if (managerInfo.currentBotHand > StaticValues.BotHand)
                {
                    int botRand = Random.Range(0, 10);
                    if (isDebug)
                    {
                        Debug.Log("botRand " + botRand);
                    }
					if(players.Count >= 5)
					{
						botRand = 10;
					}
                    if (botRand > 5)
                    {
                        
                        if (botsData.Count > 0)
                        {
                            int ExcludeHowManyBot = Random.Range(1, botsData.Count);
                            if (isDebug)
                            {
                                Debug.Log("ExcludeHowManyBot " + ExcludeHowManyBot);
                            }
                            while (ExcludeHowManyBot > 0)
                            {
                                var botData = botsData[botsData.Count - 1];
                                var p = players.Find(x => x.playerData.IsBot && x.playerData.NamePlayer == botData.NamePlayer);
                                if (p != null)
                                {
                                    if (!playerWin.Equals(p))
                                    {
                                        if (PhotonNetwork.IsMasterClient)
                                        {

                                            if (p.playerData.IsBot)
                                            {
                                                PlayerSave.singleton.CallGameExitForBotOnly(p.playerData._MobileNumber, 0, "B", null);
                                            }
                                        }
                                        p.Disconnect();
                                        botsData.RemoveAt(botsData.Count - 1);
                                        botIndex = botsData.Count - 1;
                                        PhotonNetwork.Destroy(p.gameObject);
                                    }
                                    else
                                    {
                                        if (botsData != null)
                                        {
                                            if (botsData.Count > 1)
                                            {
                                                var botData2 = botsData[0];
                                                var p2 = players.Find(x => x.playerData.IsBot && x.playerData.NamePlayer == botData2.NamePlayer);
                                                if (p2 != null)
                                                {
                                                    if (!playerWin.Equals(p2))
                                                    {
                                                        if (PhotonNetwork.IsMasterClient)
                                                        {

                                                            if (p2.playerData.IsBot)
                                                            {
                                                                PlayerSave.singleton.CallGameExitForBotOnly(p2.playerData._MobileNumber, 0, "B", null);
                                                            }
                                                        }
                                                        p2.Disconnect();
                                                        botsData.RemoveAt(0);
                                                        botIndex = botsData.Count - 1;
                                                        PhotonNetwork.Destroy(p2.gameObject);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                ExcludeHowManyBot--;
                            }
                        }
                    }
                }
            }

            managerInfo.currentPlayerStepID = playerWin.photonView.ViewID;
            SetFirstPlayerStepByCurrentStepId();
            managerInfo.GameRoom_2 += 1;
            managerInfo.isStartedGame = true;
            managerInfo.isGivingBoot = 0;//Added by me
            //Invoke("GiveCardsToPlayers", 5);//Commented by me
            ////Debug.Log("NextGameGlobalInformation ------------PlayerWinGame---------------------------NextGameGlobalInformation ");
            StopCoroutine(NextGameGlobalInformation());//Added by me
            StartCoroutine(NextGameGlobalInformation());//Added by me
            
        }
        else
        {
            managerInfo.isStartedGame = false;
            managerInfo.isGivingBoot = 0;//Added by me
        }
        if (PhotonNetwork.IsMasterClient)
        {
            if (photonView != null)//Newly Line Added by me
            {
                photonView.RPC("SyncRoom", RpcTarget.AllBuffered, JsonUtility.ToJson(managerInfo));
            }
        }
      
    }
    
   

    private void TipToDealer(PlayerManagerPun playerTipMoney, int calculatedTip)
    {
       
        if (playerTipMoney != null)
        {
            if (playerTipMoney.photonView != null)
            {
                playerTipMoney.photonView.RPC("TipMoney", RpcTarget.All, calculatedTip);
            }
        }
    }

    private int GetNumWinPlayer()
    {
        int numPlayer = 0;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null)
            {
                if (players[i].playerData.playerType == ePlayerType.PlayerStartGame && !players[i].playerData.IsPacked)
                {
                    numPlayer = i;
                }
			}
        }

        return numPlayer;
    }

    public int CountPlayersInGame()
    {
        int playersInGame = 0;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == null)
            {
                players.Remove(players[i]);
                i--;
                continue;
            }

            if (players[i].playerData.playerType == ePlayerType.PlayerStartGame && !players[i].playerData.IsPacked)
            {
                playersInGame++;
            }
        }

        UpdateRoomStatus();

        return playersInGame;
    }

    private void UpdateRoomStatus()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        var room = PhotonNetwork.CurrentRoom;
        if (players != null)
        {
            if (players.Count >= room.MaxPlayers)
            {
                room.IsOpen = false;
                room.IsVisible = false;
                if (isDebug)
                {
                    Debug.Log("[Room]: Closed now");
                }
            }
            else
            {
                room.IsOpen = true;
                room.IsVisible = true;
                if (isDebug)
                {
                    Debug.Log("[Room]: Opened now");
                }
            }
        }
       
    }
    private void UpdateRoomStatus(bool focusState)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (PlayerSave.singleton.currentTable == eTable.Private) return;
        var room = PhotonNetwork.CurrentRoom;
        if (players != null)
        {
            if (players.Count >= room.MaxPlayers)
            {
                room.IsOpen = false;
                room.IsVisible = false;
                if (isDebug)
                {
                    Debug.Log("[Room]: Closed now");
                }
            }
            else
            {
                room.IsOpen = focusState;
                room.IsVisible = focusState;
                if (isDebug)
                {
                    Debug.Log(room.IsOpen ? "[Room]: Else Opened now" : "[Room]: Else Closed now");
                }
            }
        }

    }

    private IEnumerator RisePotLimit()
    {
        foreach (var item in players)
        {
            item.photonView.RPC("GlobalInfo", RpcTarget.All, " Pot limit");
        }

        foreach (var item in players)
        {
            if (item.playerData.playerType == ePlayerType.PlayerStartGame && !item.playerData.IsPacked)
            {
                item.photonView.RPC("ShowCardForAll", RpcTarget.All);

                yield return new WaitForSeconds(1f);
            }
        }

        yield return new WaitForSeconds(1f);

        //PlayerWinGame(CardCombination.FindWinnerFromAll(players.ToArray()));
        ////Debug.Log("PlayerWinGame -------------------RisePotLimit--------------------PlayerWinGame");
        PlayerWinGame(CardCombination.decideWinner(players.ToArray()));
    }

    private IEnumerator AcceptShowCorotine(PlayerManagerPun callPlayer)
    {
        //Debug.Log("<color=red> --------AcceptShowCorotine -----------</color> " + callPlayer.playerData.NamePlayer);
        foreach (var item in players)
        {
            item.photonView.RPC("GlobalInfo", RpcTarget.All,
                callPlayer.playerData.NamePlayer + " accept side show");
        }
        
        PlayerManagerPun playerStartedSideShow = players[managerInfo.playerIdStartedSideShow];

        
        if (playerStartedSideShow!=null)
        {
            if (callPlayer)
            {
                if (callPlayer.photonView != null)
                {
                    if (!callPlayer.playerData.IsSeenCard)
                    {
                        callPlayer.photonView.RPC("SetSeenCardTrue", RpcTarget.All);
                    }
                }
            }
            if (playerStartedSideShow.playerData._DeviceID.Equals(managerInfo._IsCurrentMobileNumber))
            {
                managerInfo.isCurrentSideShow = false;
                managerInfo.isCurrentSentSideShow = false;
                managerInfo.playerIdRecievedSideShowDeviceId = "";
            }
            if (playerStartedSideShow)//Add by me
            {
                if (playerStartedSideShow.photonView != null)
                {
                    if (!playerStartedSideShow.playerData.IsSeenCard)
                    {
                        playerStartedSideShow.photonView.RPC("SetSeenCardTrue", RpcTarget.All);
                    }
                }
            }
            if (callPlayer)
            {
                if (callPlayer.photonView != null)
                {
                    if (playerStartedSideShow != null)
                    {
                        callPlayer.photonView.RPC("ShowCardsOpponet", RpcTarget.All, playerStartedSideShow.playerData.NamePlayer);
                    }
                }
            }
            if (playerStartedSideShow)
            {
                if (playerStartedSideShow.photonView != null)
                {
                    if (callPlayer != null)
                    {
                        playerStartedSideShow.photonView.RPC("ShowCardsOpponet", RpcTarget.All, callPlayer.playerData.NamePlayer);
                    }
                }
            }
            yield return new WaitForSeconds(1f);//1f se 4f kiya hai
            bool firstPlayerWin = false;
            if (callPlayer)
            {
                if (playerStartedSideShow)
                {
                    firstPlayerWin = CardCombination.CompareCards(playerStartedSideShow.GetCurrentCards(), callPlayer.GetCurrentCards(), typeTable);
                }
            }
        
            if (!firstPlayerWin)
            {
                if (playerStartedSideShow)
                {
                    if (playerStartedSideShow.photonView != null)
                    {
                        playerStartedSideShow.photonView.RPC("PackPlayerByServer", RpcTarget.All);
                    }
                }
                foreach (var item in players)
                {
                    if (item != null)
                    {
                        if (item.photonView != null)
                        {
                            if (callPlayer != null)
                            {
                                item.photonView.RPC("GlobalInfo", RpcTarget.All, callPlayer.playerData.NamePlayer + " win side show");
                                item.OnWinSideSound();
                            }
                        }
                    }
                }
            }
            else
            {
                if (callPlayer)
                {
                    if (callPlayer.photonView != null)
                    {
                        callPlayer.photonView.RPC("PackPlayerByServer", RpcTarget.All);
                    }
                }
                foreach (var item in players)
                {
                    if (item != null)
                    {
                        if (item.photonView != null)
                        {
                            if (playerStartedSideShow != null)
                            {

                                item.photonView.RPC("GlobalInfo", RpcTarget.All, playerStartedSideShow.playerData.NamePlayer + " win side show");
                                item.OnWinSideSound();

                            }
                        }
                    }
                }
            }
        }
        else
        {
            ////Debug.Log("player is null so wait for two seconds and then call next player step");
        }
        yield return new WaitForSeconds(2f);
        if (isDebug)
        {
            Debug.Log("nps on 16-------------------------------------------------------------------");
        }
        NextPlayerStep();
    }

    private IEnumerator SideShowCorotine(PlayerManagerPun firstPlayer)
    {
        if (firstPlayer != null)
        {
            MoneyPlayerToTable(firstPlayer, firstPlayer.playerData.currentBootPlayer, 0, 0, "SideShow : SideShowCorotine");////managerInfo.currentStake//Change this code by me
        }
        PlayerManagerPun callPreviousPlayer = FindPreviousPlayer(firstPlayer);

        if (CountPlayersInGame() == 2)
        {
            foreach (var item in players)
            {
                if (item != null)
                {
                    if (item.photonView != null)
                    {
                        item.photonView.RPC("ShowCardForAll", RpcTarget.All);
                        //item.photonView.RPC("GlobalInfo", RpcTarget.All,firstPlayer.playerData.NamePlayer + " call compromise with " +callPreviousPlayer.playerData.NamePlayer);

                        if (item.localPlayer)
                        {
                            item.localPlayer.DeactivateAllButtons();
                        }
                    }
                }
            }

            yield return new WaitForSeconds(1f);//1f se 4f kiya hai

            //bool firstPlayerWin =CardCombination.CompareCards(firstPlayer.GetCurrentCards(), callPreviousPlayer.GetCurrentCards(), typeTable);
            bool firstPlayerWin = false;
            if (callPreviousPlayer != null)
            {
                if (firstPlayer != null)
                {
                    firstPlayerWin = CardCombination.CompareCards(firstPlayer.GetCurrentCards(), callPreviousPlayer.GetCurrentCards(), typeTable);
                }
            }
            if (!firstPlayerWin)
            {
                foreach (var item in players)
                {
                    if (item != null)
                    {
                        if (item.photonView != null)
                        {
                            if (callPreviousPlayer != null)
                            {
                                item.photonView.RPC("GlobalInfo", RpcTarget.All,callPreviousPlayer.playerData.NamePlayer + " win hand");
                                item.OnWinSideSound();
                            }
                        }
                    }
                }

                if (callPreviousPlayer == null)
                {
                    //Debug.Log("PlayerWinGame -------------------callPreviousPlayer null in Side Show--------------------PlayerWinGame");
                    PlayerWinGame(firstPlayer);
                }
                else
                {
                    if (firstPlayer != null)
                    {
                        if (firstPlayer.photonView != null)
                        {
                            firstPlayer.photonView.RPC("PackPlayerByServer", RpcTarget.All);
                            try
                            {
                                if (firstPlayer.playerData.IsBot)
                                {
                                    firstPlayer.CallUpdateAmountForBotOnly(firstPlayer.playerData._DeviceID, 0, PhotonNetwork.CurrentRoom.Name, "-", "Pack On Side Show", "B", managerInfo.GameRoom_2.ToString(), managerInfo.CurrentRoomID);
                                }
                                else
                                {
                                    firstPlayer.CallUpdateAmountFromSideShow(firstPlayer.playerData._DeviceID, 0, PhotonNetwork.CurrentRoom.Name, "-", "Pack On Side Show", "P", managerInfo.GameRoom_2.ToString(), managerInfo.CurrentRoomID);
                                }
                            }
                            catch
                            {

                            }

                        }
                    }
                    if (callPreviousPlayer != null)
                    {
                        //Debug.Log("PlayerWinGame -------------------callPreviousPlayer not  null in Side Show--------------------PlayerWinGame");
                        PlayerWinGame(callPreviousPlayer);
                    }
                }
            }
            else
            {
                foreach (var item in players)
                {
                    if (item != null)
                    {
                        if (item.photonView != null)
                        {
                            if (firstPlayer != null)
                            {
                                item.photonView.RPC("GlobalInfo", RpcTarget.All,
                        firstPlayer.playerData.NamePlayer + " win hand");
                                item.OnWinSideSound();
                            }
                        }
                    }
                }

                if (callPreviousPlayer != null)
                {
                    if (callPreviousPlayer.photonView != null)
                    {
                        callPreviousPlayer.photonView.RPC("PackPlayerByServer", RpcTarget.All);
                        try
                        {
                            if (callPreviousPlayer.playerData.IsBot)
                            {
                                callPreviousPlayer.CallUpdateAmountForBotOnly(callPreviousPlayer.playerData._DeviceID, 0, PhotonNetwork.CurrentRoom.Name, "-", "Pack On Side Show", "B", managerInfo.GameRoom_2.ToString(), managerInfo.CurrentRoomID);
                            }
                            else
                            {
                                callPreviousPlayer.CallUpdateAmountFromSideShow(callPreviousPlayer.playerData._DeviceID, 0, PhotonNetwork.CurrentRoom.Name, "-", "Pack On Side Show", "P", managerInfo.GameRoom_2.ToString(), managerInfo.CurrentRoomID);
                            }
                        }
                        catch
                        {

                        }
                    }
                }

                if (firstPlayer != null)
                {
                    //Debug.Log("PlayerWinGame -------------------firstplayer not null in Side Show--------------------PlayerWinGame");
                    PlayerWinGame(firstPlayer);
                }
            }
        }
        else
        {
            if (players.Count > 0)
            {
                foreach (var item in players)
                {
                    if (item != null)
                    {
                        if (item.photonView != null)
                        {
                            if (firstPlayer != null)
                            {
                                if (callPreviousPlayer != null)
                                {
                                    firstPlayer.playerData.isSideShow = true;
                                    firstPlayer.playerData.isSentSideShow = true;
                                    callPreviousPlayer.playerData.isSideShow = true;
                                    callPreviousPlayer.playerData.isSentSideShow = false;
                                    if (firstPlayer.playerData._DeviceID.Equals(managerInfo._IsCurrentMobileNumber))
                                    {
                                        managerInfo.isCurrentSideShow = true;
                                        managerInfo.isCurrentSentSideShow = true;
                                    }
                                    item.photonView.RPC("GlobalInfo", RpcTarget.All,
                                        firstPlayer.playerData.NamePlayer + " requested side show with " + callPreviousPlayer.playerData.NamePlayer);
                                }
                            }
                        }
                    }
                }
            }

            //callPreviousPlayer.photonView.RPC("StartSideShow", RpcTarget.All, firstPlayer.playerData.NamePlayer);

            if (firstPlayer != null)
            {
                if (callPreviousPlayer != null)
                {
                    managerInfo.isSideShowReceiver = true;
                    managerInfo.playerIdRecievedSideShow = callPreviousPlayer.photonView.ViewID;
                    managerInfo.playerIdRecievedSideShowName = callPreviousPlayer.playerData.NamePlayer;
                    managerInfo.playerIdRecievedSideShowDeviceId = callPreviousPlayer.playerData._DeviceID;
                    //Debug.Log("callPreviousPlayer.playerData._DeviceID " + callPreviousPlayer.playerData.NamePlayer);
                    //Debug.Log("firstPlayer.playerData._DeviceID " + firstPlayer.playerData.NamePlayer);
                    callPreviousPlayer.photonView.RPC("StartSideShow", RpcTarget.All, firstPlayer.playerData.NamePlayer, firstPlayer.playerData._DeviceID);//this is reverse
                    //firstPlayer.photonView.RPC("StartSideShowNotReverse", RpcTarget.All, callPreviousPlayer.playerData.NamePlayer, playersUiPositionCollection[callPreviousPlayer.myUI.MyPositionID]);//this is actual
                    //Invoke("WaitForSixSecond", 6f);
                    CountdownTimer.StartTimer(15f);//, callPreviousPlayer);
                }
            }
        }
    }
    void WaitForSixSecond()
    {
        ////Debug.Log("after six seconds.......");
        foreach (var item in players)
        {
            if (item != null)
            {
                if (item.photonView != null)
                {
                    if (item.playerData.isSideShow && !item.playerData.isSentSideShow)
                    {
                        item.photonView.RPC("DeclineSideShow", RpcTarget.All);
                        item.photonView.RPC("DeclineSideShowOnlyMaster", RpcTarget.MasterClient);
                    }
                }
            }
        }
    }

    [PunRPC]
    private void DealJoker(string jokerStr)
    {
        var pun = FindObjectOfType<LocalPlayerPun>();
        pun.SetJoker(jokerStr);
    }

    [PunRPC]
    private void DealAndarBahar(string andarStr, string baharStr)
    {
        var pun = FindObjectOfType<LocalPlayerPun>();
        if (!string.IsNullOrEmpty(andarStr))
            pun.SetAndar(andarStr);
        if (!string.IsNullOrEmpty(baharStr))
            pun.SetBahar(baharStr);
    }
    private IEnumerator GetJoker()
    {
        managerInfo.CurrentGameStep = 0;

        yield return new WaitForSeconds(1.0f);

        managerInfo.JokerCard = deckManager.GetRandomCard();
        photonView.RPC("DealJoker", RpcTarget.All, JsonUtility.ToJson(managerInfo.JokerCard));

        yield return new WaitForSeconds(1.0f);

        managerInfo.CurrentGameStep = 1;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].playerData.playerType == ePlayerType.PlayerStartGame)
            {
                players[i].photonView.RPC("FirstBet", RpcTarget.All, managerInfo.CurrentGameStep);
            }
        }
    }
    static PlayerManagerPun[] rotLeft(PlayerManagerPun[] a, int d)
    {
        var innerLoop = a.Length - 1;
        PlayerManagerPun res=null;
        try
        {
            for (var loop = 0; loop < d; loop++)
            {
                if (innerLoop >= 0 && innerLoop < a.Length)
                {
                    if (a[innerLoop] != null)
                    {
                        res = a[innerLoop];
                    }
                }
                for (var i = innerLoop; i >= 0; i--)
                {
                    var tempI = i - 1;
                    if (tempI < 0)
                    {
                        tempI = innerLoop;
                    }
                    var yolo = a[tempI];
                    a[tempI] = res;
                    res = yolo;
                }
            }
            return a;
        }
        catch
        {
            return a;
        }
    }
    private IEnumerator GiveMovingCards()
    {
        managerInfo.isGivingCards = true;
        if (managerInfo.isGivingBoot == 0)
        {
            managerInfo.isGivingBoot = 0;
            yield return new WaitForSeconds(0.1f);

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].playerData.playerType == ePlayerType.PlayerStartGame)
                    MoneyPlayerToTable(players[i], managerInfo.currentStake, 0, 0, "Boot");
            }
            managerInfo.isGivingBoot = 1;
        }
        if (photonView != null)//Newly Line Added by me
        {
            photonView.RPC("SyncRoom", RpcTarget.AllBuffered, JsonUtility.ToJson(managerInfo));
        }
        yield return new WaitForSeconds(1.0f);

        CardData[] cacheCards = new CardData[players.Count];
        for (int i = 0; i < players.Count; i++)
        {
            cacheCards[i] = new CardData(0, 0, true);
        }
       
        int _currentPlayerIndex = 0;
        if(players != null)
        {
            for(int i=0;i< players.Count;i++)
            {
                if (players[i].playerData.playerType == ePlayerType.PlayerStartGame)
                {
                    if (i == managerInfo.currentPlayerStep)
                    {
                        _currentPlayerIndex = i;
                    }
                }
            }
        }
        var NewPlayers = players.ToArray();
        NewPlayers = rotLeft(NewPlayers, _currentPlayerIndex+1);
        for (int step = 0; step < 3; step++)
        {
            if (NewPlayers != null)
            {
                for (int i = 0; i < NewPlayers.Length; i++)
                {
                    if (NewPlayers[i] != null)
                    {
						if(!isDebug)
						{
                      		  Debug.Log("NewPlayers[i].playerData.playerType " + NewPlayers[i].playerData.playerType + "NewPlayers[i].playerData.currentCombination " + NewPlayers[i].playerData.currentCombination + "step " + step);
						}
						if (NewPlayers[i].playerData.currentCombination == eCombination.Empty && NewPlayers[i].playerData.playerType != ePlayerType.PlayerOutOfLimit)
                        {
                            yield return new WaitForSeconds(0.2f);

                            CardData randomCard;
                            bool valid;
                            //int checkCount;
                            do
                            {
                                if (typeTable == eTable.Ak47 && step == 2 && cacheCards[i].suitCard != eCardSuit.None)
                                {
                                    randomCard = cacheCards[i];
                                }
                                else
                                {
                                    //Debug.Log("getting RandomCards");
                                    //if (!NewPlayers[i].playerData.IsBot)
                                    //{
                                    //    randomCard = deckManager.GetCustomCard();
                                    //}
                                    //else
                                    //{
                                        randomCard = deckManager.GetRandomCard();
                                    //}
									if(isDebug)
									{
                                   		 Debug.Log("randomCard " + randomCard.rankCard);
									}
                                }
                                if (randomCard.suitCard != 0 || randomCard.rankCard != 0)
                                {
                                    valid = true;
                                }
                                else
                                {
                                    valid = false;
									if(isDebug)
									{
                                    	Debug.Log("isvalid...." + valid);
									}
                                }

                                if (typeTable == eTable.Joker && step == 2)
                                {
                                    randomCard.suitCard = eCardSuit.Joker;
                                    randomCard.rankCard = 15;
                                }

                                if (typeTable == eTable.Ak47)
                                {
                                    if (randomCard.rankCard == 1 ||
                                        randomCard.rankCard == 4 ||
                                        randomCard.rankCard == 7 ||
                                        randomCard.rankCard == 13)
                                    {
                                        if (step != 2)
                                        {
                                            cacheCards[i] = randomCard;
                                            valid = false;
                                        }
                                        else
                                        {
                                            randomCard.originalRankCard = randomCard.rankCard;
                                            randomCard.originalSuitCard = randomCard.suitCard;

                                            randomCard.suitCard = eCardSuit.Joker;
                                            randomCard.rankCard = 15;
                                        }
                                    }
                                }
                            } while (!valid);
                            if (NewPlayers[i] != null)
                            {
                                if (NewPlayers[i].photonView != null)
                                {
                                    NewPlayers[i].photonView.RPC("SetNewCards", RpcTarget.All, JsonUtility.ToJson(randomCard), step);
                                }

                            }
                        }

                        else if (NewPlayers[i].playerData.currentCombination != eCombination.Empty)
                        {
                            if(NewPlayers[i].myUI)
                            {
                                NewPlayers[i].myUI.SetCardsAgain();
                            }
                        }
                    }
                }
            }
        }
  		managerInfo.currentChance = -1;
        //managerInfo.currentPlayerStep =  UnityEngine.Random.Range(-1, players.Count);
        managerInfo.isGivingCards = false;
        managerInfo.isGivingBoot = 0;
        //Debug.Log("nps on 17-------------------------------------------------------------------");
        NextPlayerStep();
    }

    public override void OnDisconnected(DisconnectCause disconnectCause)
    {
        base.OnDisconnected(disconnectCause);
        //Debug.Log("OnDisconnectedFromPhoton ");
        StartCoroutine(MainReconnect());
        AddServerDataAndBotDataForNonMaster();

    }
    private IEnumerator MainReconnect()
    {
        while (PhotonNetwork.NetworkingClient.LoadBalancingPeer.PeerState != ExitGames.Client.Photon.PeerStateValue.Disconnected)
        {
            //Debug.Log("Waiting for client to be fully disconnected..", this);

            yield return new WaitForSeconds(0.2f);
        }

        //Debug.Log("Client is disconnected!", this);

        if (!PhotonNetwork.ReconnectAndRejoin())
        {
            if (PhotonNetwork.Reconnect())
            {
                //Debug.Log("Successful reconnected!", this);
            }
        }
        else
        {
            //Debug.Log("Successful reconnected and joined!", this);
        }
    }
    public override void OnConnected()
    {
        //Debug.Log("OnConnectedToPhoton Room ");
        base.OnJoinedLobby();
        base.OnConnected();

       

    }
    
    public override void OnJoinedLobby()
    {
        //Debug.Log("OnJoinedLobby Room ");
        

    }
    public override void OnConnectedToMaster()
    {
        //Debug.Log("OnConnectedToMaster Room ");

       


    }
    public override void OnJoinedRoom()
    {
        if (isDebug)
        {
            Debug.Log("OnJoined Room " + PhotonNetwork.IsMasterClient + " PhotonNetwork.CurrentRoom.PlayerCount " + PhotonNetwork.CurrentRoom.PlayerCount);
        }
        base.OnJoinedRoom();
        if(PhotonNetwork.IsMasterClient)
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                if (isDebug)
                {
                    Debug.Log("[Room]: Closed now in Joined Room");
                }
            }
        }
        UpdateRoomStatus();
    }
    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        //Debug.Log("OnLeftLobby ");
       
        
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        //Debug.Log("OnLeftRoom ");


        PhotonNetwork.LoadLevel(1);

    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer,Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer,changedProps);
        if (isDebug)
        {
            Debug.Log("OnPlayerPropertiesUpdate " + targetPlayer.ActorNumber + " " + targetPlayer.ToStringFull() + " " + changedProps.ToStringFull());
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                if (isDebug)
                {
                    Debug.Log("[Room]: Closed now in Joined Room");
                }
            }
        }
        UpdateRoomStatus();
    }
    //public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    //{
    //    base.OnPhotonPlayerDisconnected(otherPlayer);
    //    //Debug.Log("OnPhotonPlayerDisconnected " + otherPlayer.IsMasterClient + "otherPlayer.IsMasterClient " + PhotonNetwork.isMasterClient + " " + otherPlayer.ID + "  " + PhotonNetwork.masterClient.ID);
    //    if (PhotonNetwork.isMasterClient)
    //    {
    //        if (managerInfo.DisconnectPlayer)
    //        {
    //            if (!managerInfo.InAnyNextCondition)
    //            {
    //                if (managerInfo.currentPlayerStepID == managerInfo.DisconnectStepId && managerInfo.currentPlayerStepID > 0)
    //                {
    //                    //Debug.Log("managerInfo.DisconnectActorId " + managerInfo.DisconnectActorId);
    //                    if (managerInfo.DisconnectActorId.Equals(otherPlayer.NickName))
    //                    {
    //                        managerInfo.DisconnectPlayer = false;
    //                        managerInfo.InAnyNextCondition = true;
    //                        managerInfo.currentPlayerStepID = 0;
    //                        managerInfo.DisconnectStepId = 0;
    //                        managerInfo.DisconnectActorId = "";
    //                        players = FindObjectsOfType<PlayerManagerPun>().ToList();
    //                        players = players.OrderBy(x => x.GetIdOrderUI()).ToList();
    //                        CancelInvoke("CheckOnWin");
    //                        Invoke("CheckOnWin",2f);
    //                    }
    //                }
    //            }

    //        }

    //    }
    //}
    public override void OnPlayerEnteredRoom(Player otherPlayer)
    {
        base.OnPlayerEnteredRoom(otherPlayer);
        if (isDebug)
        {
            Debug.Log("OnPlayerEnteredRoom  " + otherPlayer.NickName);
        }

       

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                if (isDebug)
                {
                    Debug.Log("[Room]: Closed now in Joined Room");
                }
            }
            if (players != null)
            {
                if (players.Count >= 5)
                {
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                    PhotonNetwork.CurrentRoom.IsVisible = false;
                    if (isDebug)
                    {
                        Debug.Log("[Room]: Closed now in OnPlayerEnteredRoom............ Room");
                    }
                    
                }
            }
        }
        UpdateRoomStatus();

        AddServerDataAndBotDataForNonMaster();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerEnteredRoom(otherPlayer);
        if (isDebug)
        {
            Debug.Log("OnPlayerLeftRoom  " + otherPlayer.NickName + "otherPlayer.UserId " + otherPlayer.UserId + " reason " + otherPlayer.IsInactive);
        }

        if (PlayerSave.singleton != null)
        {
            PlayerSave.singleton.ShowErrorMessage(otherPlayer.NickName + " has left the room ");
        }
        if(managerInfo.isStartedGame)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                PlayerSave.singleton.GameExitErrorDetailsAPICall(JsonUtility.ToJson(managerInfo), "OnPlayerLeftRoom", PhotonNetwork.LocalPlayer.UserId, PhotonNetwork.LocalPlayer.NickName, managerInfo._IsCurrentMobileNumber, managerInfo.CurrentRoomID);
                if (managerInfo.IsCurrentNamePlayer == otherPlayer.NickName)
                {
                    
                    if (managerInfo.IsCurrentHost)
                    {
                        if (managerInfo.IsCurrentLocalPlayer)
                        {
                            if (!string.IsNullOrEmpty(managerInfo.AllPhotonIds))
                            {
                                if (managerInfo.AllPhotonIds.Length > 0)
                                {
                                    string founderMinus1 = managerInfo.AllPhotonIds.Remove(managerInfo.AllPhotonIds.Length - 1, 1);

                                    string[] d12 = founderMinus1.Split(';');

                                    if(d12!=null)
                                    {
                                        if(d12.Length>0)
                                        {
                                            if (isDebug)
                                            {
                                                Debug.Log("d12.Length " + d12.Length);
                                            }
                                            if (d12.Length == 2)
                                            {
                                                int count = 0;
                                                foreach (var item in d12)
                                                {
                                                    if (item == managerInfo._IsCurrentMobileNumber)
                                                    {
                                                        count++;
                                                        break;
                                                    }
                                                }
                                                if (count == 1)
                                                {
                                                    foreach (var item in d12)
                                                    {
                                                        if (item != managerInfo._IsCurrentMobileNumber)
                                                        {
                                                            var p = players.Find(x => !x.playerData.IsBot && x.playerData._DeviceID == managerInfo._IsCurrentMobileNumber);
                                                            if (p != null)
                                                            {
                                                                managerInfo._IsCurrentMobileNumber = "";
                                                                PlayerWinGame(p);
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            else if(d12.Length>2)
                                            {
                                                int count = 0;
                                                foreach (var item in d12)
                                                {
                                                    if (item == managerInfo._IsCurrentMobileNumber)
                                                    {
                                                        count++;
                                                        break;
                                                    }
                                                }
                                                if (count == 1)
                                                {
                                                    foreach (var item in d12)
                                                    {
                                                        if (item != managerInfo._IsCurrentMobileNumber)
                                                        {
                                                            var p = players.Find(x => !x.playerData.IsBot && x.playerData._DeviceID == managerInfo._IsCurrentMobileNumber);
                                                            if (p != null)
                                                            {
                                                                if (!otherPlayer.IsInactive)//if playerttl value is greater than zero than this value will be true if player disconnected by call or other scenario and if playerttl value reach zero or set zero than this value always return false
                                                                {
                                                                    managerInfo._IsCurrentMobileNumber = "";
                                                                    if (isDebug)
                                                                    {
                                                                        Debug.Log("block this state and call next player... step");
                                                                    }
                                                                    NextPlayerStep();
                                                                }
                                                            }
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if(otherPlayer.UserId == managerInfo._IsCurrentMobileNumber)
                            {
                                if(managerInfo.IsCurrentPlayerDestroyed)
                                {
                                    managerInfo.IsCurrentLocalPlayer = false;
                                    if (isDebug)
                                    {
                                        Debug.Log("block this state and call next player step");
                                    }
                                    NextPlayerStep();
                                }
                            }
                        }
                    }
                }
            }
        }


        AddServerDataAndBotDataForNonMaster();

    }
    
    void OnDestroy()
    {
        //PhotonNetwork.OnEventCall -= this.OnEvent;
        if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
        {
            if (PlayerSave.singleton.GameExit)
            {
                PlayerSave.singleton.GameExit = false;
                if (PlayerSave.singleton != null)
                {
                    
                    PlayerSave.singleton.CallGameExit(0, "P",null);
                }

            }
        }
        CallOnDestroy2(!string.IsNullOrEmpty(PlayerSave.singleton.GetUserName()) ? PlayerSave.singleton.GetUserName() : PlayerSave.singleton.GetCurrentNamey(), PlayerSave.singleton.GetUserId(),2);

    }
    public void OnEvent(EventData eventcode)
    {
        players = FindObjectsOfType<PlayerManagerPun>().ToList();
        players = players.OrderBy(x => x.GetIdOrderUI()).ToList();
        if (eventcode.Code != 200)
        {
            //Debug.Log("received event: " + eventcode);
        }
        if (eventcode.Code == (int)EnumPhoton.SendChatMessage)
        {
            string[] message = ((string)eventcode.CustomData).Split(';');
           // //Debug.Log("Received message " + message[0] + " from " + message[1]);
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] != null)
                {
                    if (players[i].myUI != null)
                    {
                        if (players[i].playerData._DeviceID.Equals(message[1]))
                        {
                            for (int ih = 0; ih < 5; ih++)
                            {
                                players[i].myUI.GetChatMessageBubble(ih).SetActive(true);

                                players[i].myUI.GetChatMessageBubbleText(ih).GetComponent<Text>().text = message[0];
                                if (players[i].myUI.GetChatMessageBubble(ih).GetComponent<Animator>().enabled)
                                {
                                    players[i].myUI.GetChatMessageBubble(ih).GetComponent<Animator>().Play("MessageBubbleAnimation");
                                }
                            }
                        }
                       // //Debug.Log("Received message in else " + message[0] + " from " + message[1] + " " + players[i].playerData._MobileNumber);
                    }
                    else
                    {
                       // //Debug.Log("Received message in else " + message[0] + " from " + message[1] + " " + players[i].playerData._MobileNumber);
                    }
                }
            }
        }
        else if (eventcode.Code == (int)EnumPhoton.SendTextMessage)
        {
            string[] message = ((string)eventcode.CustomData).Split(';');
            ////Debug.Log("Received message " + message[0] + " from " + message[1]);
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] != null)
                {
                    if (players[i].myUI != null)
                    {

                        if (players[i].playerData._DeviceID.Equals(message[1]))
                        {
                            for (int ih = 0; ih < 5; ih++)
                            {
                                players[i].myUI.GetChatMessageBubble(ih).SetActive(true);

                                players[i].myUI.GetChatMessageBubbleText(ih).GetComponent<Text>().text = message[0];
                                if (players[i].myUI.GetChatMessageBubble(ih).GetComponent<Animator>().enabled)
                                {
                                    players[i].myUI.GetChatMessageBubble(ih).GetComponent<Animator>().Play("MessageBubbleAnimation");
                                }
                            }
                        }
                       // //Debug.Log("Received message in else " + message[0] + " from " + message[1] + " " + players[i].playerData._MobileNumber);
                    }
                    else
                    {
                        ////Debug.Log("Received message in else " + message[0] + " from " + message[1] +" "+ players[i].playerData._MobileNumber);
                    }
                }
            }
        }
        else if(eventcode.Code == (int)EnumPhoton.SendDestroyMessage)
        {
            string[] message = ((string)eventcode.CustomData).Split(';');
            if (isDebug)
            {
                Debug.Log("Received message " + message[0] + " from " + message[1]);
            }
            if (PhotonNetwork.IsMasterClient)
            {
                try
                {
                    DestroyActivityEvent(message[0], message[1]);
                }
                catch
                {

                }
            }
        }
        else if (eventcode.Code == 204)
        {
            if (isDebug)
            {
                Debug.Log("RecieveDestroyMessage message " + eventcode.CustomData.ToString());
                Debug.Log("RecieveDestroyMessage Sender " + eventcode.Sender);
                Debug.Log("RecieveDestroyMessage SenderKey " + eventcode.SenderKey);
            }
            CallOnDestroy2(!string.IsNullOrEmpty(PlayerSave.singleton.GetUserName()) ? PlayerSave.singleton.GetUserName() : PlayerSave.singleton.GetCurrentNamey(), PlayerSave.singleton.GetUserId(), 1);
        }
    }
   
    
   
    private void MoneyPlayerToTable(PlayerManagerPun playerGiveMoney, double calculatedBoot, int result, int currentValue,string _Message)
    {
        managerInfo.isChaalPlayerUpdateBoot = true;
        managerInfo.isChaalPlayerUpdateBootAmount = calculatedBoot;
        if (playerGiveMoney != null)
        {
            if (playerGiveMoney.photonView != null)
            {
                playerGiveMoney.photonView.RPC("GiveMoney", RpcTarget.All, calculatedBoot, result, currentValue, _Message);
            }
        }

        if (typeTable != eTable.AndarBahar)
        {
            managerInfo.totalPot += calculatedBoot;
            foreach (var item in players)
            {
                if (item != null)
                    item.photonView.RPC("SetTotal", RpcTarget.All, managerInfo.totalPot);
            }
        }

        managerInfo.isChaalPlayerUpdateBoot = false;
        managerInfo.isChaalPlayerUpdateBootAmount = 0;

    }
    public void FinishedTurn(PlayerManagerPun player, int result, int add, int andar, int bahar)
    {
        ////Debug.Log($"[{player.playerData.NamePlayer}]: Finished bet {result} {add} {andar} {bahar}");


        if (player.playerData.Money < add)
        {
            ////Debug.Log("not money => skip bet");
            player.photonView.RPC("SkipBetByServer", RpcTarget.All); // PackPlayerByServer
        }

        if (result > 0)
            MoneyPlayerToTable(player, add, result, andar, "FinishedTurn :If");
        else if (result < 0)
            MoneyPlayerToTable(player, add, result, bahar, "FinishedTurn : Else If");
        else
            MoneyPlayerToTable(player, 0, 0, 0, "FinishedTurn : Else");

        bool nextTurn = true;
        foreach (var playerManagerPun in players)
        {
            if (playerManagerPun.playerData.playerType == ePlayerType.PlayerStartGame)
                nextTurn &= playerManagerPun.playerData.IsFinished;
        }

        if (nextTurn)
        {
            if (managerInfo.CurrentGameStep == 1) GrabAndarBaharCard();
            else StartCoroutine(_FindJokerCard());
        }
    }
    private void GrabAndarBaharCard()
    {
        managerInfo.CurrentGameStep = 2;
        if (managerInfo.AndarCards == null) managerInfo.AndarCards = new List<CardData>();
        managerInfo.AndarCards.Add(deckManager.GetRandomCard());
        if (managerInfo.BaharCards == null) managerInfo.BaharCards = new List<CardData>();
        managerInfo.BaharCards.Add(deckManager.GetRandomCard());

        photonView.RPC("DealJoker", RpcTarget.All,
            JsonUtility.ToJson(managerInfo.JokerCard));

        photonView.RPC("DealAndarBahar", RpcTarget.All,
            JsonUtility.ToJson(managerInfo.AndarCards[managerInfo.AndarCards.Count - 1]),
            JsonUtility.ToJson(managerInfo.BaharCards[managerInfo.BaharCards.Count - 1]));

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].playerData.playerType == ePlayerType.PlayerStartGame)
            {
                players[i].photonView.RPC("FirstBet", RpcTarget.All, managerInfo.CurrentGameStep);
            }
        }
    }
    private IEnumerator _FindJokerCard()
    {
        managerInfo.CurrentGameStep = 3;

        yield return new WaitForSeconds(1);

        var result = 0;

        while (result == 0)
        {
            if (managerInfo.AndarCards == null) managerInfo.AndarCards = new List<CardData>();
            var card = deckManager.GetRandomCard();
            managerInfo.AndarCards.Add(card);
            photonView.RPC("DealJoker", RpcTarget.All,
                JsonUtility.ToJson(managerInfo.JokerCard));
            photonView.RPC("DealAndarBahar", RpcTarget.All, JsonUtility.ToJson(card), string.Empty);
            if (card.rankCard == managerInfo.JokerCard.rankCard)
                result = 1;
            else
            {
                yield return new WaitForSeconds(1);
                if (managerInfo.BaharCards == null) managerInfo.BaharCards = new List<CardData>();
                card = deckManager.GetRandomCard();
                managerInfo.BaharCards.Add(card);
                photonView.RPC("DealJoker", RpcTarget.All,
                    JsonUtility.ToJson(managerInfo.JokerCard));
                photonView.RPC("DealAndarBahar", RpcTarget.All, string.Empty, JsonUtility.ToJson(card));
                if (card.rankCard == managerInfo.JokerCard.rankCard) result = -1;
                else
                    yield return new WaitForSeconds(1);
            }
        }

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].playerData.playerType == ePlayerType.PlayerStartGame)
            {
                players[i].photonView.RPC("EndAndarBaharGame", RpcTarget.All, result);
            }
        }

        Invoke("GrabJokerCard", 5);
    }
    private void GrabJokerCard()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null)
            {
                players[i].photonView.RPC("SetInGame", RpcTarget.All);
            }
        }

        if (players.Count < 1)
        {
            managerInfo.isStartedGame = false;
            return;
        }

        foreach (var p in players)
        {
            if (p.playerData.Money > tableInfo.startBoot) continue;
            managerInfo.isStartedGame = false;
            return;
        }
        managerInfo.GameRoom_2 +=1;
        managerInfo.isStartedGame = true;
        managerInfo.currentStake = tableInfo.startBoot;
        deckManager.NewDeck(typeTable);
        StopCoroutine(GetJoker());
        StartCoroutine(GetJoker());
    }
    
    void OnApplicationPause(bool pauseStatus)//this will not working proper //commented by me on 27 Nov 2020 when master player goes in background when the next timer start so the onapplication is not working proper
    {
        if (pauseStatus)
        {
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length > 1)
            {
                if (managerInfo.isNextGameTimer)
                {
                    CancelInvoke("GiveCardsToPlayers");
                    StopCoroutine(NextGameGlobalInformation());
                }
                if (managerInfo.isCheckOnWin)
                {
                    CancelInvoke("CheckOnWin");
                }
                //if (photonView != null)
                //{
                //    photonView.RPC("SyncRoom", RpcTarget.AllViaServer, JsonUtility.ToJson(managerInfo));
                //}
                UpdateRoomStatus(!pauseStatus);
                PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient.GetNext());
                PhotonNetwork.SendAllOutgoingCommands();
                
                
                //Debug.Log("is if master "+ managerInfo.isNextGameTimer + "managerInfo.isCheckOnWin "+ managerInfo.isCheckOnWin);
            }
            else if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length == 1)
            {
                UpdateRoomStatus(!pauseStatus);
                PhotonNetwork.SendAllOutgoingCommands();
            }
            if (isDebug)
            {
                Debug.Log("[Room]: ApplicationPause Closed now");
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length == 1)
            {
                UpdateRoomStatus(!pauseStatus);
                PhotonNetwork.SendAllOutgoingCommands();
            }
            else if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length > 1 && PhotonNetwork.PlayerList.Length < 5)
            {
                UpdateRoomStatus(!pauseStatus);

                PhotonNetwork.SendAllOutgoingCommands();

                if (isDebug)
                {
                    //Debug.Log("is else master " + managerInfo.isNextGameTimer + "managerInfo.isCheckOnWin " + managerInfo.isCheckOnWin);
                }
            }
            if (isDebug)
            {
                Debug.Log("[Room]: ApplicationPause Open now");
            }
        }
    }
    [PunRPC]
    public void DestroyActivity(PlayerManagerPun player)
    {
        if (isDebug)
        {
            Debug.Log("DestroyActivity ");
        }
        if(player!=null)
        {
            if (isDebug)
            {
                Debug.Log("DestroyActivity  inner" + player.playerData.NamePlayer);
            }
            if(managerInfo._IsCurrentMobileNumber == player.playerData._DeviceID)
            {
                managerInfo.IsCurrentPlayerDestroyed = true;
                if (PlayerSave.singleton != null)
                {
                   
                    PlayerSave.singleton.CallUpdateAmountForDestroyCallByMaster(player.playerData._DeviceID, 0, PlayerSave.FullRoomName, "-", "Destroy", "P", managerInfo.GameRoom_2.ToString(), managerInfo.CurrentRoomID);
                }
            }
        }
    }
    public void DestroyActivityEvent(string playerDataName,string playerDataDeviceID)
    {

        if (isDebug)
        {
            Debug.Log("DestroyActivityEvent  inner" + playerDataName);
        }
            if (managerInfo._IsCurrentMobileNumber == playerDataDeviceID)
            {
                managerInfo.IsCurrentPlayerDestroyed = true;
                if (PlayerSave.singleton != null)
                {

                    PlayerSave.singleton.CallUpdateAmountForDestroyCallByMaster(playerDataDeviceID, 0, PlayerSave.FullRoomName, "-", "Destroy", "P", managerInfo.GameRoom_2.ToString(), managerInfo.CurrentRoomID);
                }
            }
        
    }
}
public static class StaticStrings
{
    public static string[] chatMessages = new string[] {
            "Jaldi Khelo",
            "Play Fast",
            "Please Play",
            "You are good",
            "Well played",
            "I won",
            "Hehehe",
            "Unlucky",
            "Thanks",
            "Yeah",
            "Hi all!!!",
            "Good Game",
            "Oops",
            "Today is my day",
            "All the best",
            "Hi",
            "Hello",
            "Wow!!!",
            "Thank you"
        };
}
public enum EnumPhoton
{
   
    SendChatMessage = 175,
    SendTextMessage = 176,
    SendDestroyMessage=177
    
}
