using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

[Serializable]
public struct PlayerData
{
    public ePlayerType playerType;
    public eCombination currentCombination;
    public double Money;
    public double Chips;
    public int step;
    public double currentBootPlayer;
    public double experience;
    public bool IsDoubleBoot;
    public bool IsSeenCard;
    public bool IsLocalPlayer;
    public bool isSideShow;
    public bool isSentSideShow;
    public bool IsPacked;
    public string NamePlayer;
    public string _MobileNumber;
    public int _RandNext;
    public string _Email;
    public string _DeviceID;
    public string _DistributionId;
    public string _Gender;
    public CardData[] currentCards;
    public int BlindCount;
    public bool IsBot;
    #region FOR ANDAR BAHAR

    public bool IsFinished;
    public double AndarBet;
    public double BaharBet;

    #endregion
    public int AvatarPic;
    public string uploadPic;
    public float _CurrentTimer;
    public bool isPlayerActive;
    public bool isDisconnect;
    public bool isDestroyed;
    public bool isPause;
}

public class PlayerManagerPun : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback, IConnectionCallbacks, IMatchmakingCallbacks, IPunObservable
{
    public PlayerData playerData;
    
    public PlayerUI myUI;
    public LocalPlayerPun localPlayer;
    public TeenPattiPhoton managerMain;
    public TeenPatiHUD hud;
    private PlayerSave _playerSave;
    private TableInfo tableInfo;

    private bool isDebug = false;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        //info.Sender.TagObject = gameObject;
        //base.OnPhotonInstantiate(info);


        _playerSave = FindObjectOfType<PlayerSave>();
        playerData.currentCards = new CardData[3];
       
        managerMain = FindObjectOfType<TeenPattiPhoton>();

        hud = FindObjectOfType<TeenPatiHUD>();
        try
        {
            hud.ClearTextGlobalInfo();
        }
        catch
        {

        }
        object[] data = (object[])info.photonView.InstantiationData;

        //Debug.Log("info.photonView " + info.photonView.Controller + "photonView.IsMine " + info.photonView.Owner+" "+ info.photonView.AmOwner + "MasterClient "+PhotonNetwork.IsMasterClient +" "+PhotonNetwork.LocalPlayer);
        if (photonView.IsMine)
        {
            //if (PlayerSave.singleton.currentTable == eTable.Standard)
            //{
            //    //GameObject.Find("Table").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("1");
            //    //GameObject.Find("TableBg").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("1");
            //    //GameObject.Find("Girl").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Girl_3");
            //}
            //else if (PlayerSave.singleton.currentTable == eTable.NoLimit)
            //{
            //    //GameObject.Find("Table").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("2");
            //    //GameObject.Find("TableBg").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("2");
            //    //GameObject.Find("Girl").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Girl_6");
            //}
            //else if (PlayerSave.singleton.currentTable == eTable.Joker)
            //{
            //    //GameObject.Find("Table").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("3");
            //    //GameObject.Find("TableBg").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("3");
            //    //GameObject.Find("Girl").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Girl_15");
            //}

            //Debug.Log("managerMain.botIndex----------------------------------------------------- " + managerMain.botIndex);
            playerData._MobileNumber = (string)data[0];
            playerData.Money = (double)data[1];
            playerData.Chips = (double)data[2];
            playerData.NamePlayer = (string)data[3];
            playerData.experience = (double)data[4];
            playerData._Email = (string)data[5];
            playerData.IsBot = (bool)data[6];
            playerData._DeviceID = (string)data[7];
            playerData._DistributionId = (string)data[8];
            playerData._Gender = (string)data[9];
            playerData.AvatarPic = (int)data[10];
            playerData.uploadPic = (string)data[11];
            playerData.isPlayerActive = (bool)data[12];

            if (info.photonView.AmOwner)
            {
                localPlayer = gameObject.AddComponent<LocalPlayerPun>();
            }

            if (photonView != null)
            {
                photonView.RPC("InitPLayerInManager", RpcTarget.MasterClient, JsonUtility.ToJson(playerData));
            }
            

   //         if (managerMain.botIndex < 0)
   //         {
   //         playerData._MobileNumber = PlayerSave.singleton.GetMobileId();
   //         playerData.Money = _playerSave.GetCurrentMoney();
   //         playerData.Chips = _playerSave.GetCurrentChips();
   //         playerData.NamePlayer = !string.IsNullOrEmpty(_playerSave.GetUserName()) ? _playerSave.GetUserName() : _playerSave.GetCurrentNamey();
   //         playerData.experience = _playerSave.GetExp();
   //         playerData._Email = _playerSave.GetEmail();
   //         playerData._DeviceID = _playerSave.GetUserId();
   //         playerData._DistributionId = _playerSave.GetDistributionId();
   //         playerData._Gender = _playerSave.GetGender();
   //         playerData.AvatarPic = _playerSave.GetAvatar();
   //         playerData.uploadPic = _playerSave.GetPic();
   //         localPlayer = gameObject.AddComponent<LocalPlayerPun>();
   //         if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
   //         {
   //             if (localPlayer) localPlayer.SetTextMoneyTopBar(playerData.Money.ToString());
   //         }
   //         else if(PlayerSave.singleton.currentTable == eTable.Free)
   //         {
   //                 if (localPlayer) localPlayer.SetTextMoneyTopBar(playerData.Chips.ToString());
   //             }
            
   //             if (photonView != null)
   //             {
   //                 photonView.RPC("InitPLayerInManager", RpcTarget.MasterClient, JsonUtility.ToJson(playerData));
   //             }
			//}
			//else
			//{
			//    ////Debug.LogWarning($"photonView.isBot = TRUE BotID: {managerMain.botIndex}" + managerMain.botsData[managerMain.botIndex].Money);
   //             var pData = managerMain.botsData[managerMain.botIndex];
   //             playerData._MobileNumber = pData._MobileNumber;
   //             playerData.Money = pData.Money;
   //             playerData.Chips = pData.Chips;
   //             playerData.NamePlayer = pData.NamePlayer;
   //             playerData.experience = pData.experience;
   //             playerData._Email = pData._Email;
   //             playerData.IsBot = pData.IsBot;
   //             playerData._DeviceID = pData._DeviceID;
   //             playerData._DistributionId = pData._DistributionId;
   //             playerData._Gender = pData._Gender;
   //             playerData.AvatarPic = pData.AvatarPic == 0 ? UnityEngine.Random.Range(0,27) : pData.AvatarPic;
   //             playerData.uploadPic = "";
   //             photonView.RPC("InitPLayerInManager", RpcTarget.MasterClient, JsonUtility.ToJson(playerData));
			
			//}
            //Debug.Log("PhotonNe   " + PhotonNetwork.IsMasterClient + " " + "InitPLayerInManager");
            //info.Sender.NickName = photonView.ViewID.ToString();
			Invoke("CheckAfterFiveSeconds", 5f);
            ////Debug.Log(" info.sender.NickName " + info.sender.NickName);
        }
    }
	private void CheckAfterFiveSeconds()
    {
        CancelInvoke("CheckAfterThreeSeconds");
        if(myUI==null)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
    public bool IsMine => photonView.IsMine && !playerData.IsBot;

    public bool IsBotMine => photonView.IsMine && playerData.IsBot;
    public bool IsMaster => PhotonNetwork.IsMasterClient; // && !playerData.IsBot;

    public bool IsMaster2 => PhotonNetwork.IsMasterClient && !playerData.IsBot;
    public override void OnLeftRoom()
    {
        //Debug.Log("OnLeftRoom ");
        playerData.playerType = ePlayerType.Empty;
        if (myUI)
        {
            myUI.ClearUI(0);
        }
        base.OnLeftRoom();
       // Debug.Log("OnLeftRoom.......... " + PlayerSave.singleton.GetUserId() + " playerData._DeviceID " + playerData._DeviceID);
        if (PlayerSave.singleton.GetUserId() == playerData._DeviceID)
        {
           // Debug.Log("OnLeftRoom " + PlayerSave.singleton.GetUserId() + " playerData._DeviceID "+ playerData._DeviceID);
            if (myUI) myUI.CallOnDestroy(playerData,3);
        }

        //UnityEngine.SceneManagement.SceneManager.LoadScene(1);

    }

    public override void OnDisconnected(DisconnectCause disconnectCause)
    {
        //Debug.Log("OnDisconnectedFromPhoton "+ disconnectCause.ToString());
        playerData.playerType = ePlayerType.Empty;
        if (myUI)
        {
            myUI.ClearUI(0);
        }
        base.OnDisconnected(disconnectCause);

        PhotonNetwork.LoadLevel(1);
        
    }
    public void OnRefuseSideSound()
    {
        if(hud)
        {
            hud.OnSideShowRefuseSound();
        }
    }
    public void OnWinSideSound()
    {
        if (hud)
        {
            hud.OnSideShowWinSound();
        }
    }
    private float _timerNew;
    public void Update2()
    {
        if (playerData.playerType == ePlayerType.PlayerStartGame)
        {
            return;
        }
           
        if (PhotonNetwork.IsConnected && PhotonNetwork.PlayerList.Length >= 1)
        {
            _timerNew = _timerNew + Time.deltaTime;
            if (_timerNew >= 1f && _timerNew < 2f)
            {
                try
                {
                    if (managerMain!=null)
                    {
                        if (hud != null)
                        {
                            if (managerMain.managerInfo.isStartedGame)
                            {
                                if (!playerData.IsBot)
                                {
									if (playerData.playerType == ePlayerType.PlayerInRoom || playerData.playerType == ePlayerType.Empty)
                                    {
                                        hud.SetGameRunningGlobalInfo("Wait for next Game to start.");
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {

                }
            }
            else if (_timerNew >= 2f && _timerNew < 3f)
            {
                try
                {
                    if (managerMain != null)
                    {
                        if (hud != null)
                        {
                            if (managerMain.managerInfo.isStartedGame)
                            {
                                if (!playerData.IsBot)
                                {
									if (playerData.playerType == ePlayerType.PlayerInRoom || playerData.playerType == ePlayerType.Empty)
                                    {
                                        hud.SetGameRunningGlobalInfo("Wait for next Game to start..");
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {

                }
            }
            else if (_timerNew >= 3f && _timerNew < 4f)
            {
                try
                {
                    if (managerMain != null)
                    {
                        if (hud != null)
                        {
                            if (managerMain.managerInfo.isStartedGame)
                            {
                                if (!playerData.IsBot)
                                {
									if (playerData.playerType == ePlayerType.PlayerInRoom || playerData.playerType == ePlayerType.Empty)
                                    {
                                        hud.SetGameRunningGlobalInfo("Wait for next Game to start...");
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {

                }
            }
            else if (_timerNew >= 4f && _timerNew<5f)
            {
                try
                {
                    if (managerMain != null)
                    {
                        if (hud != null)
                        {
                            if (managerMain.managerInfo.isStartedGame)
                            {
                                if (!playerData.IsBot)
                                {
									if (playerData.playerType == ePlayerType.PlayerInRoom || playerData.playerType == ePlayerType.Empty)
                                    {
                                        hud.ClearTextGlobalInfo();
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {

                }
            }
            else
            {
                _timerNew = 0f;
            }
           
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //Debug.Log("OnMasterClientSwitched");
         base.OnMasterClientSwitched(newMasterClient);

        if(hud!=null)
        {
            hud.UpdateShareIconForMasterOnlyInPrivate();
        }
    }    

    [PunRPC]
    public void InitPLayerInManager( string playerSync)
    {
        //Debug.Log("InitPLayerInManager playerData = " + playerSync);
        playerData = JsonUtility.FromJson<PlayerData>(playerSync);
        if (managerMain) managerMain.InitPlayer(this);
        
    }

    [PunRPC]
    public void OnlySetPLayerDataInManager(string playerSync)
    {
        
        playerData = JsonUtility.FromJson<PlayerData>(playerSync);
        ////Debug.LogWarning("OnlySetPLayerDataInManager playerData = " + playerSync);
        if (myUI) myUI.RefreshCoinsValue();

    }

    [PunRPC]
    public void ChaalPlayer()
    {
        StopStep();
        if (IsMaster)
        {
			if (managerMain) managerMain.managerInfo.isChaalPlayer = true;
            if (managerMain) managerMain.Chaal(this);
        }
    }
    public void PackPlayerUI()
    {
        StopStep();
        playerData.IsPacked = true;
        playerData.isSideShow = false;
        playerData.isSentSideShow = false;
        if (myUI) myUI.SetPacked();
    }
    [PunRPC]
    public void PackPlayer()
    {
        ////Debug.Log("pack player " + playerData.NamePlayer);
        if (managerMain != null && managerMain.typeTable == eTable.AndarBahar) return;
        PackPlayerUI();
        if (IsMaster)
        {
            ////Debug.Log($"[{playerData.NamePlayer}]: Packed");
            ProceedPackOnServer();
        }
		try
        {
            if (photonView != null)
            {
                if (photonView.IsMine)
                {
                    if (PhotonNetwork.InRoom)
                    {
                        if (_playerSave != null)
                        {
                            if (_playerSave.currentTable == eTable.Standard || _playerSave.currentTable == eTable.Private)
                            {
                                if (IsMine)
                                {
                                    CallUpdateAmount(0, PhotonNetwork.CurrentRoom.Name, "-", "Pack", "P", managerMain.managerInfo.GameRoom_2.ToString(), managerMain.managerInfo.CurrentRoomID);
                                }
                            }
                        }

                    }
                }
            }
        }
        catch
        {

        }
        if (hud)
        {
            hud.OnPackSound();
        }
    }
    [PunRPC]
    public void PackBot()
    {
        PackPlayerUI();
        if (photonView != null)
        {
            if (photonView.IsMine)
            {
                if (PhotonNetwork.InRoom)
                {
                    if (IsBotMine)
                    {
                        CallUpdateAmountForBotOnly(playerData._MobileNumber,0, PhotonNetwork.CurrentRoom.Name, "-", "Pack", "B",managerMain.managerInfo.GameRoom_2.ToString(), managerMain.managerInfo.CurrentRoomID);
                    }

                }
            }
        }
    }
    [PunRPC]
    public void ProceedPackOnServer()
    {
        if (managerMain) managerMain.PackPlayerFromClient();
    }
    [PunRPC]
    public void PackPlayerByServer()
    {
        StopStep();
        playerData.IsPacked = true;
        playerData.isSentSideShow = false;
        playerData.isSideShow = false;
        if (myUI) myUI.SetPacked();
    }  

    [PunRPC]
    public void EndTypeChat(string _text)
    {
        if (managerMain) managerMain.AddChat(_text,myUI.IdOrder);
    }
    [PunRPC]
    public void PauseActivity()
    {
        ////Debug.Log("PauseActivity  name " + playerData.NamePlayer);
        playerData.isPause = true;
        if (IsMaster2)
            if (managerMain) managerMain.PauseActivity(this);
    }
    [PunRPC]
    public void UnPauseActivity()
    {
        ////Debug.Log("PauseActivity  name " + playerData.NamePlayer);
        playerData.isPause = false;
        if (IsMaster2)
            if (managerMain) managerMain.UnPauseActivity(this);
    }
    
    [PunRPC]
    public void Disconnect()
    {
        playerData.isDisconnect = true;
        if (isDebug)
        {
            Debug.Log("isDisconnect " + playerData.isDisconnect + " name " + playerData.NamePlayer);
        }
        if (IsMaster2)
            if (managerMain) managerMain.PlayerDisconnected(this);

        if(IsMaster)
        {
            if(managerMain.managerInfo._IsCurrentMobileNumber == playerData._DeviceID)
            {
                managerMain.managerInfo.IsCurrentPlayerDisconnected=true;
            }
        }
        try
        {
            if (photonView != null)
            {
                if (photonView.IsMine)
                {
                    if (PhotonNetwork.InRoom)
                    {
                        if (_playerSave != null)
                        {
                            if (_playerSave.currentTable == eTable.Standard || _playerSave.currentTable == eTable.Private)
                            {
                                if (IsMine)
                                {
                                    CallUpdateAmount(0, PhotonNetwork.CurrentRoom.Name, "-", "Disconnect", "P", managerMain.managerInfo.GameRoom_2.ToString(), managerMain.managerInfo.CurrentRoomID);
                                }
                                else if (IsBotMine)
                                {
                                    CallUpdateAmountForBotOnly(playerData._MobileNumber, 0, PhotonNetwork.CurrentRoom.Name, "-", "Disconnect", "B", managerMain.managerInfo.GameRoom_2.ToString(),managerMain.managerInfo.CurrentRoomID);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {

        }
    } 
    
    [PunRPC]
    public void DecreaseBet()
    {
        playerData.IsDoubleBoot = false;
    }
    [PunRPC]
    public void IncreaseBet()
    {
        if (CanIncreaseBoot())
        {
            playerData.IsDoubleBoot = true;
        }
    }

    [PunRPC]
    public void CallSideShow()
    {
        //Debug.Log("<color=red> --------CallSideShow -----------</color> " + playerData.NamePlayer);
        StopStep();
        playerData.isSideShow = true;
        playerData.isSentSideShow = true;
        
    }

    [PunRPC]
    public void AcceptSideShow()
    {
        //Debug.Log("<color=red> --------AcceptSideShow -----------</color> " + playerData.NamePlayer);
        playerData.isSideShow = false;
        playerData.isSentSideShow = false;
        //if (managerMain) managerMain.AcceptSideShow(this);//this will call only on master client
        if (myUI) myUI.StopSideShow();
    }
    [PunRPC]
    public void AcceptSideShowOnlyMaster()
    {
        //Debug.Log("<color=red> --------AcceptSideShowOnlyMaster -----------</color> " + playerData.NamePlayer);
        if (managerMain) managerMain.AcceptSideShow(this);
    }
    [PunRPC]
    public void ProceedCallSideShowOnServer()
    {
        //Debug.Log("<color=red> --------ProceedCallSideShowOnServer -----------</color> " + playerData.NamePlayer);
        if (managerMain) managerMain.StartSideShow(this);
    }
    [PunRPC]
    public void DeclineSideShow()
    {
        //Debug.Log("<color=red> --------DeclineSideShow -----------</color> " + playerData.NamePlayer);
        playerData.isSideShow = false;
        playerData.isSentSideShow = false;
        //if (managerMain) managerMain.DeclineSideShow(this);//this will call only on master client
        if (myUI) myUI.StopSideShow();
    }
    [PunRPC]
    public void DeclineSideShowOnlyMaster()
    {
        //Debug.Log("<color=red> --------DeclineSideShow -----------</color> " + playerData.NamePlayer);
      
        if (managerMain) managerMain.DeclineSideShow(this);
        
    }
    [PunRPC]
    public void FinishedTurn(int result, int add, int andar, int bahar)
    {
        StopStep();

        if (IsMaster)
            managerMain.FinishedTurn(this, result, add, andar, bahar);
    }
    [PunRPC]
    public void NewChat(string _textAdd, int uiOrder)
    {
        if (photonView != null)
        {
            if (IsMine)
                if (localPlayer) localPlayer.NewChatText(_textAdd, uiOrder);
        }
    }
    [PunRPC]
    public void ClearFromGame()
    {
        ////Debug.Log("clear from game..." + playerData.NamePlayer);
        playerData.IsLocalPlayer = false;
        playerData.IsSeenCard = false;
       // playerData.isSentSideShow = false;
        playerData.playerType = ePlayerType.Empty;
        if(myUI)myUI.ClearUI(0);     
        
        if (photonView != null)
        {
            if (IsMine)
            {
                PhotonNetwork.LeaveRoom(false);
                // PhotonNetwork.Disconnect();          
            }
        }
    }
    [PunRPC]
    public void NewClearFromGame()
    {
        playerData.playerType = ePlayerType.Empty;//PlayerInRoom
        if (myUI) myUI.ClearUI(0);
        
    }

    [PunRPC]
    public void ShowCardForAll()
    {
        if (!playerData.IsPacked)
        {
            playerData.IsSeenCard = true;
            if (playerData.playerType == ePlayerType.PlayerStartGame)
                if (myUI) myUI.SetInfoBlind(playerData.IsSeenCard);
                else
                if (myUI) myUI.SetInfoEmpty();
            //if (myUI) myUI.SetInfoBlind(playerData.IsSeenCard);
            StopStep();
            ShowCard();
        }
    }

    [PunRPC]
    public void InitTableData(string _tableInfo, double total)
    {
        TableInfo _tableInfoSync = JsonUtility.FromJson<TableInfo>(_tableInfo);
        tableInfo = _tableInfoSync;
        //  tableInfo.startBoot = _tableInfoSync.startBoot;
        // tableInfo.blindLimit = _tableInfoSync.blindLimit;
        //  tableInfo.chalLimit = _tableInfoSync.chalLimit;
        //  tableInfo.potLimit = _tableInfoSync.potLimit;
        //hud = FindObjectOfType<TeenPatiHUD>();
        if (tableInfo.blindLimit >0)
        {
            if (hud) hud.bootAmount.text = tableInfo.startBoot.ToString("F2");
            if (hud) hud.blindLimit.text = tableInfo.blindLimit.ToString("F2");
            if (hud) hud.chalLimit.text = tableInfo.chalLimit.ToString("F2");
            if (hud) hud.potLimit.text = tableInfo.potLimit.ToString("F2");
        }
        else
        {
            if (hud) hud.bootAmount.text = tableInfo.startBoot.ToString("F2");
            if (hud) hud.blindLimit.text = "no limit".ToUpper();
            if (hud) hud.chalLimit.text = "no limit".ToUpper();
            if (hud) hud.potLimit.text = "no limit".ToUpper();
        }
       
        playerData.playerType = ePlayerType.Empty;
        if(hud)hud.TotalBoot(total.ToString("F2"));
    }   

    [PunRPC]
    public void InitUI(string currentUI,string playerSync)
    {
        ////Debug.Log("currentUI.... " + currentUI);

       
        playerData = JsonUtility.FromJson<PlayerData>(playerSync);

        //Debug.Log("InitUI from player with name = " + playerData.NamePlayer + "currentUI " + currentUI + " playerSync " + playerSync);
        if (myUI == null)
            myUI = GameObject.Find(currentUI).GetComponent<PlayerUI>();

        if (myUI == null)
        {
            ////Debug.LogWarning("Error! Didn't find myUI for this player!");
        }
        else
        {
            ////Debug.Log($"InitUI with UIName: {currentUI}");
            if (playerData.isPlayerActive)
            {
                myUI.Init(this);
            }
            else
            {
                myUI.WaitForSomeSecondsToInit(this);
            }
        }

        if (IsMine)
        {
            //Debug.Log($"ReorderChairsPositionsOnTable: {myUI.IdOrder}");
            managerMain.ReorderChairsPositionsOnTable(myUI.IdOrder);
        }
        //for (int i = 0; i < playerData.currentCards.Length; i++)
        //{
        //    if (myUI)  myUI.SetCard(playerData.currentCards[i], i, playerData.currentCards[i].isClose);
        //}
        if (playerData.playerType == ePlayerType.PlayerStartGame)
            if (myUI)  myUI.SetInfoBlind(playerData.IsSeenCard);
        else
            if (myUI)  myUI.SetInfoEmpty();

    }
    
   

    [PunRPC]
    public void SetTotal(double total)
    {
        
        if (photonView != null)
        {
            if (photonView.IsMine)
                if (hud) hud.TotalBoot(total.ToString("F2"));
        }

        
    }

    [PunRPC]
    public void GlobalInfo(string _globalInfo)
    {
        if (photonView != null)
        {
            if (photonView.IsMine)
            {
                if (hud) hud.SetTextGlobalInfo(_globalInfo);
            }
        }
    }
    [PunRPC]
    public void PanelOff(string _globalInfo)
    {
        if (photonView != null)
        {
            if (photonView.IsMine)
            {
                if (hud) hud.SetTextGlobalInfo(_globalInfo);
                if (localPlayer) localPlayer.PanelAcceptOff();
            }
        }
    }
    [PunRPC]
    public void SetInGame()
    {
        try
        {
            if (photonView != null)
            {
                if (photonView.IsMine)
                {
                    if (localPlayer) localPlayer.TextShowButtonToSeen();
                    if (localPlayer) localPlayer.TextCurrebyBoot(0);
                    if (IsMine)
                    {
                        playerData.Money = _playerSave.GetCurrentMoney();
						//Debug.Log(playerData.Money +" playerData.Name "+playerData.NamePlayer);
                        if (photonView != null)
                        {
                            photonView.RPC("OnlySetPLayerDataInManager", RpcTarget.All, JsonUtility.ToJson(playerData));
                        }
                        ////Debug.Log("_playerSave.GetCurrentMoney() " + _playerSave.GetCurrentMoney() + _playerSave.GetCurrentNamey());

                    }
                }
            }
        }
        catch
        {

        }
        if (playerData.isPlayerActive)
        {
            playerData.playerType = ePlayerType.PlayerStartGame;
        }
        else
        {
            playerData.playerType = ePlayerType.PlayerInRoom;
        }
        playerData.IsPacked = false;
        playerData.BlindCount = 0;
        playerData.IsSeenCard = false;
 		playerData.IsLocalPlayer = false;
 		playerData.isSideShow = false;
        playerData.isSentSideShow = false;
        playerData.currentBootPlayer = 0;
        playerData.currentCombination = eCombination.Empty;
        try
        {
            if (playerData.currentCards.Length > 0)
            {
                playerData.currentCards[0] = new CardData(0, 0, false);
            }
            if (playerData.currentCards.Length > 1)
            {
                playerData.currentCards[1] = new CardData(0, 0, false);
            }
            if (playerData.currentCards.Length > 2)
            {
                playerData.currentCards[2] = new CardData(0, 0, false);
            }
        }
        catch
        {

        }
        playerData.currentBootPlayer = 0;
        playerData.step = 0;
        playerData._RandNext = 0;
        playerData.IsLocalPlayer = false;
        if (myUI != null)
            myUI.ClearCards();
        if (myUI != null)
            myUI.ReloadUI();
       
        if (!PhotonNetwork.IsMasterClient)
            managerMain.ReloadPlayerList();
        ////Debug.Log("AB_ActivateAllButtons");
        if (IsMine)
        {
            if (localPlayer)
            {
                localPlayer.AB_ActivateAllButtons(false, 0);

                ////Debug.Log("SetAndar");
                localPlayer.SetAndar(string.Empty);
                ////Debug.Log("SetBahar");
                localPlayer.SetBahar(string.Empty);
                ////Debug.Log("SetJoker");
                localPlayer.SetJoker(string.Empty);
            }
        }

        playerData.IsFinished = false;
        playerData.AndarBet = 0;
        playerData.BaharBet = 0;

        if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
        {
			if ((float)playerData.Money < (float)PlayerSave.singleton.bootAmount)
            {
				playerData.playerType = ePlayerType.PlayerOutOfLimit;
            }
			if(PlayerSave.singleton.potLimit>0)
			{
				if ((float)playerData.Money < (float)(PlayerSave.singleton.potLimit/2f))
				{
					playerData.playerType = ePlayerType.PlayerOutOfLimit;
				}
			}
        }
        else if(PlayerSave.singleton.currentTable == eTable.Free)
        {
			if ((float)playerData.Chips < (float)PlayerSave.singleton.bootAmount)
            {
				playerData.playerType = ePlayerType.PlayerOutOfLimit;
            }
			if(PlayerSave.singleton.potLimit>0)
			{
				if ((float)playerData.Chips < (float)(PlayerSave.singleton.potLimit/2f))
				{
					playerData.playerType = ePlayerType.PlayerOutOfLimit;
				}
			}
        }

        
    }

    [PunRPC]
    public void GameStartAPI()
    {
        if (IsMine)
        {
            if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
            {

                if (PhotonNetwork.InRoom)
                {

                    CallUpdateAmount(0, PhotonNetwork.CurrentRoom.Name, "-", "Game Start", "P", managerMain.managerInfo.GameRoom_2.ToString(), managerMain.managerInfo.CurrentRoomID);

                }
            }

        }
        else if (IsBotMine)
        {
            if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
            {

                CallUpdateAmountForBotOnly(playerData._MobileNumber, 0, PhotonNetwork.CurrentRoom.Name, "-", "Game Start", "B", managerMain.managerInfo.GameRoom_2.ToString(), managerMain.managerInfo.CurrentRoomID.ToString());

            }

        }
    }
    [PunRPC]
    public void GiveMoney(double bootAmount, int result, int currentValue,string _Message)
    {
        //if (IsMine)
        //{
        //    playerData.Money = _playerSave.GetCurrentMoney();
           
        //    ////Debug.Log("_playerSave.GetCurrentMoney() " + _playerSave.GetCurrentMoney() + _playerSave.GetCurrentNamey());

        //}
        playerData.IsFinished = true;
	
        if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
        {
	        if ((double)bootAmount <= playerData.Money)
            {
            playerData.Money -= bootAmount;
				//Debug.Log(playerData.Money +" playerData.Name "+playerData.NamePlayer);
			}
        }
        else if (PlayerSave.singleton.currentTable == eTable.Free)
        {
			if ((double)bootAmount <= playerData.Chips)
            {
                playerData.Chips -= bootAmount;
			}
        }
        if (result > 0)
            playerData.AndarBet = currentValue + bootAmount;
        else if (result < 0)
            playerData.BaharBet = currentValue + bootAmount;

        if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
        {
            if(myUI)myUI.GiveTextMoney(managerMain.typeTable, playerData.Money, bootAmount, result, currentValue + bootAmount);
			
        }
        else
        {
            if (myUI) myUI.GiveTextMoney(managerMain.typeTable, playerData.Chips, bootAmount, result, currentValue + bootAmount);
        }

        if (IsMine)
        {
            if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
            {
                if(localPlayer)localPlayer.SetTextMoneyTopBar(playerData.Money.ToString("F2"));
                _playerSave.SaveNewMoney(playerData.Money);
                if (PhotonNetwork.InRoom)
                {
                    if (playerData.IsSeenCard)
                    {
                        if (_Message == "Boot")
                        {
                            CallUpdateAmount(bootAmount, PhotonNetwork.CurrentRoom.Name, "-", "Boot", "P", managerMain.managerInfo.GameRoom_2.ToString(), managerMain.managerInfo.CurrentRoomID);
                        }
                        else
                        {
                            CallUpdateAmount(bootAmount, PhotonNetwork.CurrentRoom.Name, "-", "Chaal", "P", managerMain.managerInfo.GameRoom_2.ToString(), managerMain.managerInfo.CurrentRoomID);
                        }
                    }
                    else
                    {
                        if (_Message == "Boot")
                        {
                            CallUpdateAmount(bootAmount, PhotonNetwork.CurrentRoom.Name, "-", "Boot", "P", managerMain.managerInfo.GameRoom_2.ToString(), managerMain.managerInfo.CurrentRoomID);
                        }
                        else
                        {
                            CallUpdateAmount(bootAmount, PhotonNetwork.CurrentRoom.Name, "-", "Blind", "P", managerMain.managerInfo.GameRoom_2.ToString(), managerMain.managerInfo.CurrentRoomID);
                        }
                    }
                }
            }
            else if (PlayerSave.singleton.currentTable == eTable.Free)
            {
                if (localPlayer) localPlayer.SetTextMoneyTopBar(playerData.Chips.ToString("F2"));
                _playerSave.SaveNewChips(playerData.Chips);
            }
        }
        else if(IsBotMine)
        {
            if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
            {
               
                if (PhotonNetwork.InRoom)
                {
                    if (playerData.IsSeenCard)
                    {
                        if (_Message == "Boot")
                        {
                            CallUpdateAmountForBotOnly(playerData._MobileNumber, bootAmount, PhotonNetwork.CurrentRoom.Name, "-", "Boot", "B", managerMain.managerInfo.GameRoom_2.ToString(), managerMain.managerInfo.CurrentRoomID);
                        }
                        else
                        {
                            CallUpdateAmountForBotOnly(playerData._MobileNumber, bootAmount, PhotonNetwork.CurrentRoom.Name, "-", "Chaal", "B", managerMain.managerInfo.GameRoom_2.ToString(), managerMain.managerInfo.CurrentRoomID);
                        }
                    }
                    else
                    {
                        if (_Message == "Boot")
                        {
                            CallUpdateAmountForBotOnly(playerData._MobileNumber, bootAmount, PhotonNetwork.CurrentRoom.Name, "-", "Boot", "B", managerMain.managerInfo.GameRoom_2.ToString(), managerMain.managerInfo.CurrentRoomID);
                        }
                        else
                        {
                            CallUpdateAmountForBotOnly(playerData._MobileNumber, bootAmount, PhotonNetwork.CurrentRoom.Name, "-", "Blind", "B", managerMain.managerInfo.GameRoom_2.ToString(), managerMain.managerInfo.CurrentRoomID);
                        }
                    }
                }
            }
           
        }
    }
    public void CallUpdateAmountForBotOnly(string newId, double bitAmount, string _FullRoomName, string plusOrMinus_Symbol, string _BidType, string _userType, string GameRoom_2,string _NewGeneratedId)
    {
        if (!string.IsNullOrEmpty(_FullRoomName))
        {

            GameUpdateNextBitAPICall(_FullRoomName, plusOrMinus_Symbol, bitAmount.ToString(), newId, _BidType, _userType, GameRoom_2, _NewGeneratedId);

        }
    }
    public void GameUpdateNextBitAPICall(string game_room, string plusOrMinus_Symbol, string bitAmount, string mobileNumber, string _BidType, string _userType, string GameRoom_2,string _NewGeneratedId)
    {
        NextBit _nextBit = new NextBit();
        _nextBit.game_room = game_room;
        _nextBit.symboles = plusOrMinus_Symbol;
        _nextBit.amount = bitAmount;
        _nextBit.mobile = mobileNumber;
        _nextBit.game_name = _playerSave.currentTable.ToString() + " : Next Bit";
        _nextBit.BidType = _BidType;
        _nextBit.usertype = _userType;
        _nextBit.GameRoom_2 = GameRoom_2;
        _nextBit.NewGeneratedId = _NewGeneratedId;
        GameUpdateNextBitAPICallDetails(_nextBit, _userType,mobileNumber);
    }

    public void GameUpdateNextBitAPICallDetails(NextBit _nextBit, string _userType, string mobileNumber)
    {

        var jsonString = JsonUtility.ToJson(_nextBit) ?? "";
        StartCoroutine(GameUpdateNextBitAPICallDetailsRequest("https://kheltamasha.site/api/nextbit", jsonString.ToString(), _userType, mobileNumber));
    }


    IEnumerator GameUpdateNextBitAPICallDetailsRequest(string url, string json, string _userType,string mobileNumber)
    {
        //Debug.Log("<color=red> GameUpdateNextBitAPICallDetailsRequest " + json + "</color>");
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            
        }
        else
        {
            ////Debug.Log("<color=red> uwr.downloadHandler.text " + uwr.downloadHandler.text + "</color>");
            GameUpdateNextBitAPICallDetailsHandling(uwr.downloadHandler.text.ToString(), _userType, mobileNumber);
        }
    }
    public void GameUpdateNextBitAPICallDetailsHandling(string _gameUpdateNextBitResponseDetails, string _userType,string mobileNumber)
    {
        try
        {
            string result = _gameUpdateNextBitResponseDetails;

            NextBitResponse _nextBitResponse = JsonUtility.FromJson<NextBitResponse>(result.ToString());

            if (_nextBitResponse != null)
            {
               
                string status = _nextBitResponse.status;
               
                if (status.Contains("200"))
                {
                    if (_userType == "B" && playerData._MobileNumber == mobileNumber)
                    {
                        playerData._RandNext = _nextBitResponse.rand_next;
                    }
                    if (_nextBitResponse.data != null)
                    {
						if(_nextBitResponse.data.mobile== mobileNumber)
						{
							//.Log(playerData._MobileNumber +" playerData._MobileNumber "+mobileNumber +"result "+result );
	                        if (_userType == "B" && playerData._MobileNumber == mobileNumber)
	                        {
	                            if (!double.IsInfinity(_nextBitResponse.data.Deposit_Cash) && !double.IsNaN(_nextBitResponse.data.Deposit_Cash))
	                            {
	                               
	                                playerData.Money = _nextBitResponse.data.Deposit_Cash;

	                                if (!double.IsInfinity(_nextBitResponse.data.Wining_Cash) && !double.IsNaN(_nextBitResponse.data.Wining_Cash))
	                                {

	                                    playerData.Money = _nextBitResponse.data.Deposit_Cash + _nextBitResponse.data.Wining_Cash;
	                                    if (!double.IsInfinity(_nextBitResponse.data.Bonus_Cash) && !double.IsNaN(_nextBitResponse.data.Bonus_Cash))
	                                    {

	                                        playerData.Money = _nextBitResponse.data.Deposit_Cash + _nextBitResponse.data.Wining_Cash + _nextBitResponse.data.Bonus_Cash;

	                                    }
	                                    
	                                }
	                                else
	                                {
	                                    if (!double.IsInfinity(_nextBitResponse.data.Bonus_Cash) && !double.IsNaN(_nextBitResponse.data.Bonus_Cash))
	                                    {

	                                        playerData.Money = _nextBitResponse.data.Deposit_Cash + _nextBitResponse.data.Bonus_Cash;

	                                    }
	                                }
	                            }
	                            else
	                            {
	                                if (!double.IsInfinity(_nextBitResponse.data.Wining_Cash) && !double.IsNaN(_nextBitResponse.data.Wining_Cash))
	                                {


	                                        playerData.Money = _nextBitResponse.data.Wining_Cash;
	                                  
	                                        if (!double.IsInfinity(_nextBitResponse.data.Bonus_Cash) && !double.IsNaN(_nextBitResponse.data.Bonus_Cash))
	                                        {
	                                            playerData.Money =  _nextBitResponse.data.Wining_Cash + _nextBitResponse.data.Bonus_Cash;
	                                        }
	                                    

	                                }
	                                else
	                                {
	                                    if (!double.IsInfinity(_nextBitResponse.data.Bonus_Cash) && !double.IsNaN(_nextBitResponse.data.Bonus_Cash))
	                                    {
	                                        playerData.Money =  _nextBitResponse.data.Bonus_Cash;
	                                    }
	                                }
	                            }
								//Debug.Log(playerData.Money +" playerData.Name "+playerData.NamePlayer);
	                        }
						}
                    }
                    if (photonView != null)
                    {
                        photonView.RPC("OnlySetPLayerDataInManager", RpcTarget.All, JsonUtility.ToJson(playerData));
                    }
                }
               
            }
           
        }
        catch
        {
            
        }
    }
    public void CallUpdateAmountFromSideShow(string newId,double bitAmount, string _FullRoomName, string plusOrMinus_Symbol, string _BidType, string _userType, string GameRoom_2, string _NewGeneratedId)
    {
        if (!string.IsNullOrEmpty(_FullRoomName))
        {

           

            GameUpdateNextBitAPICallForPlayer(_FullRoomName, plusOrMinus_Symbol, bitAmount.ToString(), newId, _BidType, _userType, GameRoom_2, _NewGeneratedId);

        }
    }
    public void CallUpdateAmount(double bitAmount, string _FullRoomName, string plusOrMinus_Symbol, string _BidType, string _userType, string GameRoom_2,string _NewGeneratedId)
    {
        if (!string.IsNullOrEmpty(_FullRoomName))
        {

            string newId = _playerSave.newID();

            GameUpdateNextBitAPICallForPlayer(_FullRoomName, plusOrMinus_Symbol, bitAmount.ToString(), newId, _BidType, _userType, GameRoom_2, _NewGeneratedId);

        }
    }
    public void GameUpdateNextBitAPICallForPlayer(string game_room, string plusOrMinus_Symbol, string bitAmount, string mobileNumber, string _BidType, string _userType, string GameRoom_2, string _NewGeneratedId)
    {
		//Debug.Log("mobileNumber "+mobileNumber  +"plusOrMinus_Symbol "+plusOrMinus_Symbol);
        NextBit _nextBit = new NextBit();
        _nextBit.game_room = game_room;
        _nextBit.symboles = plusOrMinus_Symbol;
        _nextBit.amount = bitAmount;
        _nextBit.mobile = mobileNumber;
        _nextBit.game_name = _playerSave.currentTable.ToString() + " : Next Bit";
        _nextBit.BidType = _BidType;
        _nextBit.usertype = _userType;
        _nextBit.GameRoom_2 = GameRoom_2;
        _nextBit.NewGeneratedId = _NewGeneratedId;
        GameUpdateNextBitAPICallDetailsForPlayer(_nextBit, _userType, mobileNumber,_BidType);
    }

    public void GameUpdateNextBitAPICallDetailsForPlayer(NextBit _nextBit, string _userType,string _mobileNumber,string _BidType)
    {
		
        var jsonString = JsonUtility.ToJson(_nextBit) ?? "";
        StartCoroutine(GameUpdateNextBitAPICallDetailsRequestForPlayer("https://kheltamasha.site/api/nextbit", jsonString.ToString(), _userType, _mobileNumber,_BidType));
    }


    IEnumerator GameUpdateNextBitAPICallDetailsRequestForPlayer(string url, string json, string _userType,string _mobileNumber,string _BidType)
    {
		//Debug.Log("<color=red> GameUpdateNextBitAPICallDetailsRequest player " + json + "</color>");
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            
        }
        else
        {
			if (_userType == "P" && _mobileNumber == StaticValues.FirebaseUserId)
			{
				//Debug.Log("uwr.downloadHandler.text.ToString() "+uwr.downloadHandler.text.ToString());
			}
            GameUpdateNextBitAPICallDetailsHandlingForPlayer(uwr.downloadHandler.text.ToString(), _userType, _mobileNumber,_BidType);
        }
    }
    public void GameUpdateNextBitAPICallDetailsHandlingForPlayer(string _gameUpdateNextBitResponseDetails, string _userType,string _mobileNumber,string _BidType)
    {
        try
        {
            string result = _gameUpdateNextBitResponseDetails;

            NextBitResponse _nextBitResponse = JsonUtility.FromJson<NextBitResponse>(result.ToString());

            if (_nextBitResponse != null)
            {
                
                string status = _nextBitResponse.status;
                
                if (status.Contains("200"))
                {
                   
                    if (_nextBitResponse.data != null)
                    {
						if(_nextBitResponse.data.mobile == _mobileNumber)
						{
							//Debug.Log("<color=red> StaticValues.FirebaseUserId " + _mobileNumber + "</color>" +StaticValues.FirebaseUserId +" result "+result);
	                        if (_userType == "P" && _mobileNumber == StaticValues.FirebaseUserId)
	                        {
	                            if (!double.IsInfinity(_nextBitResponse.data.Deposit_Cash) && !double.IsNaN(_nextBitResponse.data.Deposit_Cash))
	                            {
	                                StaticValues.DepositEarningCount = _nextBitResponse.data.Deposit_Cash.ToString("F2");
	                                StaticValues.TotalEarningAmount = _nextBitResponse.data.Deposit_Cash;
	                                _playerSave.SaveNewMoney(StaticValues.TotalEarningAmount);
	                                playerData.Money = _nextBitResponse.data.Deposit_Cash;

	                            }
	                            if (!double.IsInfinity(_nextBitResponse.data.Wining_Cash) && !double.IsNaN(_nextBitResponse.data.Wining_Cash))
	                            {
	                                StaticValues.WithdrawEarningCount = _nextBitResponse.data.Wining_Cash.ToString("F2");

	                                
	                                if (!double.IsInfinity(_nextBitResponse.data.Deposit_Cash) && !double.IsNaN(_nextBitResponse.data.Deposit_Cash))
	                                {

	                                    StaticValues.TotalEarningAmount = _nextBitResponse.data.Deposit_Cash + _nextBitResponse.data.Wining_Cash;
	                                    _playerSave.SaveNewMoney(StaticValues.TotalEarningAmount);
	                                    playerData.Money = _nextBitResponse.data.Deposit_Cash + _nextBitResponse.data.Wining_Cash;
	                                }
	                                try
	                                {
	                                    StaticValues.MinimumAmount = _nextBitResponse.data.minimum_withdraw_amount;
										//Debug.Log("StaticValues.MinimumAmount "+StaticValues.MinimumAmount );
										if(StaticValues.MinimumAmount >= 10 && StaticValues.MinimumAmount <= 20)
	                                    {
	                                        if(_nextBitResponse.data.Wining_Cash >= StaticValues.MinimumAmount)
	                                        {
	                                            //Debug.Log("_nextBitResponse.data.Wining_Cash"+_nextBitResponse.data.Wining_Cash );
	                                            if (_BidType == "Winner")
	                                            {
	                                                if (!StaticValues.FirstTimeDepositPromptClose)
	                                                {
	                                                    //Debug.Log("StaticValues.FirstTimeDepositPromptOpen.... "+StaticValues.FirstTimeDepositPromptOpen);
	                                                    StaticValues.FirstTimeDepositPromptOpen = true;
	                                                }
	                                            }
	                                        }
	                                    }

	                                }
	                                catch
	                                {

	                                }
	                            }
	                            if (!double.IsInfinity(_nextBitResponse.data.Bonus_Cash) && !double.IsNaN(_nextBitResponse.data.Bonus_Cash))
	                            {
	                                StaticValues.PromoEarningCount = _nextBitResponse.data.Bonus_Cash.ToString("F2");

	                                
	                                if (!double.IsInfinity(_nextBitResponse.data.Deposit_Cash) && !double.IsNaN(_nextBitResponse.data.Deposit_Cash) && !double.IsInfinity(_nextBitResponse.data.Wining_Cash) && !double.IsNaN(_nextBitResponse.data.Wining_Cash))
	                                {

	                                    StaticValues.TotalEarningAmount = _nextBitResponse.data.Bonus_Cash + _nextBitResponse.data.Deposit_Cash + _nextBitResponse.data.Wining_Cash;
	                                    _playerSave.SaveNewMoney(StaticValues.TotalEarningAmount);
	                                    playerData.Money = _nextBitResponse.data.Deposit_Cash + _nextBitResponse.data.Wining_Cash+ _nextBitResponse.data.Bonus_Cash;

	                                }
	                            }
								//Debug.Log(playerData.Money +" playerData.Name "+playerData.NamePlayer);
	                        }
						}

                    }

                    if (photonView != null)
                    {
                        photonView.RPC("OnlySetPLayerDataInManager", RpcTarget.All, JsonUtility.ToJson(playerData));
                    }
                }
                
            }
            
        }
        catch
        {
            
        }
    }
    [PunRPC]
    public void FirstBet(int gameStep)
    {
        ////Debug.Log(
        //    $"-------------------------------------- [{playerData.NamePlayer}] --------------------------------------");
        playerData.step++;
        playerData.IsFinished = false;

        if (IsMine)
        {
            //localPlayer.AB_ActivateAllButtons(true, gameStep);
        }
        else if (playerData.IsBot)
        {
            BotManager.Instance.SetBotTurn(0, 25, this, playerData, myUI,
                managerMain, hud, tableInfo);
        }

        myUI.StopStep();
        myUI.SetCurrentStep();
    }
    [PunRPC]
    public void EndAndarBaharGame(int result)
    {
        ////Debug.Log(
        //    $"-------------------------------------- [{playerData.NamePlayer}] --------------------------------------");
        playerData.step++;
        playerData.IsFinished = true;
        double win = 0;
        if (result > 0)
        {
            playerData.Money += playerData.AndarBet * 2;
            win += playerData.AndarBet - playerData.BaharBet;
        }
        else if (result < 0)
        {
            playerData.Money += playerData.BaharBet * 2;
            win += playerData.BaharBet - playerData.AndarBet;
        }

        myUI.EndAndarBaharGame(result);
        if (IsMine)
        {
            localPlayer.SetLocalInfoText(win > 0 ? $"Win: {win}" : $"Lose: {win}");
            if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
            {
                localPlayer.SetTextMoneyTopBar(playerData.Money.ToString());
                _playerSave.SaveNewMoney(playerData.Money);
            }
            else
            {
                localPlayer.SetTextMoneyTopBar(playerData.Chips.ToString());
                _playerSave.SaveNewChips(playerData.Chips);
            }
        }
    }
    [PunRPC]
    public void StartStep(double newBoot)
    {
        ////Debug.Log(
        //    $"-------------------------------------- [{playerData.NamePlayer}] --------------------------------------");
        playerData.currentBootPlayer = newBoot;
 		playerData.IsLocalPlayer = true;
        playerData.IsDoubleBoot = false;
        playerData.step++;

        if (IsMine)
        {
            if (localPlayer)
            {
                localPlayer.ActivateAllButtons(managerMain.CountPlayersInGame() == 2);
                localPlayer.TextCurrebyBoot(playerData.currentBootPlayer);
            }

            if (managerMain.typeTable != eTable.PotBlind &&
                // Add this condition so in BotBlind mode, players card won't automatically
                tableInfo.blindLimit != 0 && playerData.step >= tableInfo.blindLimit && !playerData.IsSeenCard)
            {
                photonView.RPC("SetSeenCardTrue", RpcTarget.All);
                if (localPlayer)
                {
                    localPlayer.SeenCardText();
                    localPlayer.SetLocalInfoText("Blind Limit  4 turns");
                }
                if (managerMain)//Add  this code by me 
                {

                    double HC = managerMain.GetHighestChaal();
                    if (HC > playerData.currentBootPlayer && HC > 0)
                    {
                        playerData.currentBootPlayer = HC;
                    }
                    else
                    {
                        playerData.currentBootPlayer *= 2;
                    }
                    ////Debug.Log("playerData.currentBootPlayer ... " + playerData.currentBootPlayer);
                    if (playerData.currentBootPlayer >= tableInfo.chalLimit && tableInfo.chalLimit > 0 && playerData.currentBootPlayer > 0)
                    {
                        playerData.currentBootPlayer = tableInfo.chalLimit;
                        ////Debug.Log("High Chaal in limit 4");
                    }
                    if (photonView != null)
                    {
                        photonView.RPC("UpdateBootAmount", RpcTarget.All, playerData.currentBootPlayer);
                    }
                }
                if (localPlayer) localPlayer.TextCurrebyBoot(playerData.currentBootPlayer);
            }
        }
        else if (playerData.IsBot)
        {
            BotManager.Instance.SetBotTurn(0, 25, this, playerData, myUI,
                managerMain, hud, tableInfo);
        }

        if (managerMain) managerMain.managerInfo.IsCurrentTimer = 0f;
        if (myUI) myUI.SetCurrentStep();
    }
   
    [PunRPC]
    public void ReplaceSideWithShow(int _showValue)
    {
       playerData.isSideShow = false;
        playerData.isSentSideShow = false;
        if(localPlayer) localPlayer.ReplaceSidewithShow(_showValue);
    }

    [PunRPC]
    private void StopStep()
    {
        ////Debug.Log($"[{playerData.NamePlayer}]: End turn");
        playerData.IsLocalPlayer = false;
        if (myUI) myUI.StopStep();
        
        if (photonView != null)
        {
            if (IsMine)
                if (localPlayer) localPlayer.DeactivateAllButtons();
        }
        if (managerMain) managerMain.managerInfo.IsCurrentTimer = 0f;
    }

    [PunRPC]
    public void SetNewCards(string card, int numCard)
    {
        CardData newCard = JsonUtility.FromJson<CardData>(card);
        playerData.currentCards[numCard] = newCard;

        if (myUI)
        {
            myUI.SetCard(newCard, numCard, false);
        }
        if (numCard == 2)
        {
            LastCardInHand();
            if (photonView != null)
            {
                if (photonView.IsMine)
                {
                    if (localPlayer)
                    {
                        localPlayer.ActivateOnlyShowButtons();
                        if (myUI) myUI.ActiveSeeButton();
                        ////Debug.Log("from where ActiveSeeButton");
                        SetCardsForServer("P");
                    }
                }
            }
            if (PhotonNetwork.IsMasterClient)
            {
                if (playerData.IsBot)
                {
                    SetCardsForServer("B");
                }
            }
        }
        if (hud)
        {
            hud.OnCardSound();
        }
    }
    public void SetCardsForServer(string PlayerOrBot)
    {
        string card_1 = "", card_2 = "", card_3 = "", priorty = "";
        int suitCard_1 = 0, suitCard_2 = 0, suitCard_3 = 0;
        if (playerData.currentCards != null)
        {
            for (int i = 0; i < playerData.currentCards.Length; i++)
            {
                if (i == 0)
                {
                    if (playerData.currentCards.Length > 0)
                    {
                        suitCard_1 = (int)playerData.currentCards[0].suitCard;
                        card_1 = suitCard_1.ToString() + "_" + playerData.currentCards[0].rankCard.ToString();
                        priorty = playerData.currentCombination.ToString();
                    }
                }
                else if (i == 1)
                {
                    if (playerData.currentCards.Length > 1)
                    {
                        suitCard_2 = (int)playerData.currentCards[1].suitCard;
                        card_2 = suitCard_2.ToString() + "_" + playerData.currentCards[1].rankCard.ToString();
                        priorty = playerData.currentCombination.ToString();
                    }
                }
                else if (i == 2)
                {
                    if (playerData.currentCards.Length > 2)
                    {
                        suitCard_3 = (int)playerData.currentCards[2].suitCard;
                        card_3 = suitCard_3.ToString() + "_" + playerData.currentCards[2].rankCard.ToString();
                        priorty = playerData.currentCombination.ToString();
                    }
                }
            }
        }
        if (PhotonNetwork.InRoom)
        {
            if (PlayerOrBot == "P")
            {
                _playerSave.CallSavedCards(PhotonNetwork.CurrentRoom.Name, card_1, card_2, card_3, priorty, PlayerOrBot, managerMain.managerInfo.GameRoom_2.ToString(), playerData._RandNext,_playerSave.bootAmount, managerMain.managerInfo.CurrentRoomID, managerMain.managerInfo.CurrentDIcon.ToString());
            }
            else
            {
                _playerSave.CallSavedCardsForBotOnly(playerData._MobileNumber,PhotonNetwork.CurrentRoom.Name, card_1, card_2, card_3, priorty, PlayerOrBot,managerMain.managerInfo.GameRoom_2.ToString() , playerData._RandNext,_playerSave.bootAmount, managerMain.managerInfo.CurrentRoomID, managerMain.managerInfo.CurrentDIcon.ToString());
            }
        }
        else
        {
            if (PlayerOrBot == "P")
            {
                _playerSave.CallSavedCards("", card_1, card_2, card_3, priorty, PlayerOrBot, managerMain.managerInfo.GameRoom_2.ToString(), playerData._RandNext, _playerSave.bootAmount, managerMain.managerInfo.CurrentRoomID, managerMain.managerInfo.CurrentDIcon.ToString());
            }
            else
            {
                _playerSave.CallSavedCardsForBotOnly(playerData._MobileNumber, "", card_1, card_2, card_3, priorty, PlayerOrBot, managerMain.managerInfo.GameRoom_2.ToString(), playerData._RandNext,_playerSave.bootAmount, managerMain.managerInfo.CurrentRoomID, managerMain.managerInfo.CurrentDIcon.ToString());
            }
        }

    }
    [PunRPC]
    public void WinHand(double totalBet)
    {
	
	if (managerMain)
        {
            managerMain.managerInfo.totalPot = 0;
            managerMain.managerInfo.currentPlayerStepID = 0;
            managerMain.managerInfo.playerIdStartedSideShow = 0;
            
            
            managerMain.managerInfo.currentPlayerStepID = 0;
            

        }
        //if (IsMine)
        //{
        //    playerData.Money = _playerSave.GetCurrentMoney();
           
        //    ////Debug.Log("_playerSave.GetCurrentMoney() " + _playerSave.GetCurrentMoney() + _playerSave.GetCurrentNamey());

        //}
        if(myUI)myUI.SetWinText();
        //if (myUI) myUI.ClearCards();
        if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
        {
            playerData.Money += totalBet;
			//Debug.Log(playerData.Money +" playerData.Name "+playerData.NamePlayer);
            if (myUI) myUI.WinTextMoney(playerData.Money, totalBet);
        }
        else if(PlayerSave.singleton.currentTable == eTable.Free)
        {
            playerData.Chips += totalBet;
            if (myUI) myUI.WinTextMoney(playerData.Chips, totalBet);
        }
        StopStep();

        if (IsMine)
        {
            if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
            {
                localPlayer.SetTextMoneyTopBar(playerData.Money.ToString());
				//Debug.Log(playerData.Money +" playerData.Name "+playerData.NamePlayer);
                _playerSave.AddExp(totalBet);
                _playerSave.SaveNewMoney(playerData.Money);
                if (PhotonNetwork.InRoom)
                {
                    if (totalBet >= 0)
                    {
                        CallUpdateAmount(totalBet, PhotonNetwork.CurrentRoom.Name, "+", "Winner","P", managerMain.managerInfo.GameRoom_2.ToString(), managerMain.managerInfo.CurrentRoomID);
                    }
                }
                
            }
            else if (PlayerSave.singleton.currentTable == eTable.Free)
            {
                localPlayer.SetTextMoneyTopBar(playerData.Chips.ToString());
                _playerSave.AddExp(totalBet);
                _playerSave.SaveNewChips(playerData.Chips);
            }
        }
        else if(IsBotMine)
        {
            if (PhotonNetwork.InRoom)
            {
                if (totalBet > 0)
                {
                    CallUpdateAmountForBotOnly(playerData._MobileNumber, totalBet, PhotonNetwork.CurrentRoom.Name, "+", "Winner", "B", managerMain.managerInfo.GameRoom_2.ToString(), managerMain.managerInfo.CurrentRoomID);
                }
            }
        }
    }

    [PunRPC]
    public void StartSideShow(string nameCalledPlayer,string deviceID)
    {
        //Debug.Log("nameCalledPlayer " + nameCalledPlayer + " playerData.IsBot " + playerData.IsBot + " playerData.NamePlayer "+ playerData.NamePlayer +" startValue "+ deviceID);
        if (IsMine)
        {
            if(localPlayer)localPlayer.StarSideShow();
        }
        else if (playerData.IsBot)
        {
            BotManager.Instance.OnReceiveSideShow(0, this, playerData, myUI, managerMain, hud, tableInfo);
        }
 		playerData.isSideShow = true;
        if (myUI) myUI.SetSideShow(deviceID);//add this line by me
    }
    [PunRPC]
    public void StartSideShowNotReverse()
    {

    }
    [PunRPC]
    public void ShowCardsOpponet(string namePl)
    {
        
        if (photonView != null)
        {
            if (IsMine)
            {
                FindObjectsOfType<PlayerManagerPun>().First(x => x.playerData.NamePlayer == namePl).ShowCard();
            }
        }
    } 
    [PunRPC]
    public void SetDealerIcon(int playerIndex,int currentDealer)
    {
        ////Debug.Log("SetDealerIcon <color=red>SetDealerIcon**********************************************i </color>" + playerIndex + " playerIndex " + " currentDealer "+ currentDealer +" "+ playerData.NamePlayer);
        if (myUI!=null)
        {
            if(playerIndex == currentDealer)
            {
                myUI.ShowDealerIcon(currentDealer);
                if(IsMaster)
                {
                    if (managerMain) managerMain.managerInfo.CurrentDIcon = playerData._DeviceID;
                }
            }
        }
        else 
        {
            ////Debug.Log("SetDealerIcon <color=red>SetDealerIcon else not set**********************************************i </color>");
        }
    }

    public int GetIdOrderUI()
    {
        if (myUI)
            return myUI.MyPositionID;

        return 0;

    }
   
    public void OnPlayerSeenCard()
    {
        //if (!playerData.IsSeenCard && managerMain.typeTable != eTable.PotBlind)
        //{
        //    photonView.RPC("SetSeenCardTrue", RpcTarget.All);
        //}
        //else
        //{
        //Debug.Log("OnPlayerSeenCard");
        photonView.RPC("CallSideShow", RpcTarget.All);
        photonView.RPC("ProceedCallSideShowOnServer", RpcTarget.MasterClient);
        //}
    }
    public void OnSeenCard()
    {
        if (!playerData.IsSeenCard && managerMain.typeTable != eTable.PotBlind)
        {
            //Debug.Log("OnSeenCard");
            photonView.RPC("SetSeenCardTrue", RpcTarget.All);
        }
        
    }

    public CardData[] GetCurrentCards()
    {
        return playerData.currentCards;
    }
    public int GetCurrentRankCard(int _index)
    {
        return playerData.currentCards[_index].rankCard;
    }
    public string GetNameUI()
    {
        if (myUI)
            return myUI.name;

        return "";
    }

    public int GetId()
    {
        if(myUI)
        {
            return myUI.MyPositionID;
        }
        return -1;
    }
    public bool CanIncreaseBoot()
    {
        double newBoot = playerData.currentBootPlayer * 2;
        if (tableInfo.chalLimit == 0 || newBoot <= tableInfo.chalLimit)
        {
            if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
            {
                if (playerData.Money >= newBoot)
                    return true;
                else
                    return false;
            }
            else
            {
                if (playerData.Chips >= newBoot)
                    return true;
                else
                    return false;
            }
        }
        else
            return false;
    }

    private void ShowCard()
    {
        if (playerData.IsPacked) return;
        for (int i = 0; i < playerData.currentCards.Length; i++)
        {
            playerData.currentCards[i].isClose = false;
            if(myUI)myUI.SetCard(playerData.currentCards[i], i, false);
        }
        if (myUI) myUI.SetCardCombinationText(playerData.currentCombination);
    }
    private void ShowCardAfterLastHand()//this method is for to set the cards again if any one is disable
    {
        if (playerData.IsPacked) return;
        for (int i = 0; i < playerData.currentCards.Length; i++)
        {
           
            if (myUI) myUI.MovingCloseCardToPlayerComplete(i);
        }
      
    }
    private void ShowCardOff()
    {
        if (playerData.IsPacked) return;
        for (int i = 0; i < playerData.currentCards.Length; i++)
        {
            playerData.currentCards[i].isClose = true;
            if (myUI) myUI.SetCard(playerData.currentCards[i], i, true);
        }
        if (myUI) myUI.SetCardCombinationText(playerData.currentCombination);
    }
    [PunRPC]
    public void SetSeenCardTrue()
    {
        playerData.IsSeenCard = true;
        if (playerData.playerType == ePlayerType.PlayerStartGame)
            if (myUI) myUI.SetInfoBlind(playerData.IsSeenCard);
            else
            if (myUI) myUI.SetInfoEmpty();
        //myUI.SetInfoBlind(playerData.IsSeenCard);
        if (IsMine)
        {
            localPlayer.SeenCardText();
            ShowCard();

            if (tableInfo.blindLimit != 0 && playerData.step < tableInfo.blindLimit)//Add this code by me
            {
                if (managerMain)
                {
                    double HC = managerMain.GetHighestChaal();
                    if (HC > playerData.currentBootPlayer)
                    {
                        playerData.currentBootPlayer = HC;
                    }
                    else
                    {
                        playerData.currentBootPlayer *= 2;
                    }
                    if (playerData.currentBootPlayer >= tableInfo.chalLimit && tableInfo.chalLimit > 0 && playerData.currentBootPlayer > 0)
                    {
                        playerData.currentBootPlayer = tableInfo.chalLimit;
                        ////Debug.Log("High Chaal in limit 2");
                    }
                    ////Debug.Log("playerData.currentBootPlayer22 ... " + playerData.currentBootPlayer);
                    if (photonView != null)
                    {
                        photonView.RPC("UpdateBootAmount", RpcTarget.All, playerData.currentBootPlayer);
                    }
                }
                if (localPlayer) localPlayer.TextCurrebyBoot(playerData.currentBootPlayer);

            }
            else if (tableInfo.blindLimit == 0)
            {
                if (managerMain)
                {
                    double HC = managerMain.GetHighestChaal();
                    if (HC > playerData.currentBootPlayer)
                    {
                        playerData.currentBootPlayer = HC;
                    }
                    else
                    {
                        playerData.currentBootPlayer *= 2;
                    }

                    if (photonView != null)
                    {
                        photonView.RPC("UpdateBootAmount", RpcTarget.All, playerData.currentBootPlayer);
                    }
                }
                if (localPlayer) localPlayer.TextCurrebyBoot(playerData.currentBootPlayer);
            }
        }
        else if(IsBotMine)
        {
            if (tableInfo.blindLimit != 0 && playerData.step < tableInfo.blindLimit)//Add this code by me
            {
                if (managerMain)
                {
                    double HC = managerMain.GetHighestChaal();
                    if (HC > playerData.currentBootPlayer)
                    {
                        playerData.currentBootPlayer = HC;
                    }
                    else
                    {
                        playerData.currentBootPlayer *= 2;
                    }
                    if (playerData.currentBootPlayer >= tableInfo.chalLimit && tableInfo.chalLimit > 0 && playerData.currentBootPlayer > 0)
                    {
                        playerData.currentBootPlayer = tableInfo.chalLimit;
                        ////Debug.Log("High Chaal in limit 2");
                    }
                    ////Debug.Log("playerData.currentBootPlayer22 ... " + playerData.currentBootPlayer);
                    if (photonView != null)
                    {
                        photonView.RPC("UpdateBootAmount", RpcTarget.All, playerData.currentBootPlayer);
                    }
                }
                if (localPlayer) localPlayer.TextCurrebyBoot(playerData.currentBootPlayer);

            }
            else if (tableInfo.blindLimit == 0)
            {
                if (managerMain)
                {
                    double HC = managerMain.GetHighestChaal();
                    if (HC > playerData.currentBootPlayer)
                    {
                        playerData.currentBootPlayer = HC;
                    }
                    else
                    {
                        playerData.currentBootPlayer *= 2;
                    }

                    if (photonView != null)
                    {
                        photonView.RPC("UpdateBootAmount", RpcTarget.All, playerData.currentBootPlayer);
                    }
                }
                if (localPlayer) localPlayer.TextCurrebyBoot(playerData.currentBootPlayer);
            }
        }
    }
   

    [PunRPC]
    public void UpdateBootAmount(double bootAmount)
    {
        ////Debug.Log("update boot " + bootAmount + " "+playerData.NamePlayer);
        playerData.currentBootPlayer = bootAmount;
       
        if (photonView != null)
        {
            if (photonView.IsMine)
            {
                if (localPlayer) localPlayer.TextCurrebyBoot(playerData.currentBootPlayer);//actual code
            }
        }
    }

    public double GetChallLimit()
    {
        return tableInfo.chalLimit;
    } 

    private void LastCardInHand()
    {
        // if (myUI) myUI.SetInfoBlind(playerData.IsSeenCard);
        if (playerData.playerType == ePlayerType.PlayerStartGame)
            if (myUI) myUI.SetInfoBlind(playerData.IsSeenCard);
            else
            if (myUI) myUI.SetInfoEmpty();
        playerData.currentCards = playerData.currentCards.OrderBy(x => x.rankCard).ToArray();
        playerData.IsDoubleBoot = false;
        if (playerData.currentCombination == eCombination.Empty)
        {
            playerData.step = 0;
        }
        playerData.currentCombination = CardCombination.GetCombinationFromCard(playerData.currentCards);
        if (playerData.IsBot)
        {
            playerData._RandNext = CalculateRandomNumberForBots();
        }
        else
        {
            playerData._RandNext = 0;
        }
    }
    private int CalculateRandomNumberForBots()
    {
        int rankCard_1 = 0, rankCard_2 = 0, rankCard_3 = 0;
      
        rankCard_1 = playerData.currentCards[0].rankCard;
        rankCard_2 = playerData.currentCards[1].rankCard;
        rankCard_3 = playerData.currentCards[2].rankCard;
        

        string priority_game = playerData.currentCombination.ToString();

        if (priority_game == "Pair")
        {
            return Random.Range(7, 15);// " Give me chaal number in range of 7-15 Show"
        }
        else if (priority_game == "Color")
        {
            return Random.Range(12, 20);// " Give me chaal number in range of 12-20 Show"
        }
        else if (priority_game == "Sequence")
        {
            return Random.Range(15, 22);// " Give me chaal number in range of 15-22 Show"
        }
        else if (priority_game == "PureSequence")
        {
            return Random.Range(20, 30);// " Give me chaal number in range of 20-30 with Show"
        }
        else if (priority_game == "Trail")
        {
            return 100;// "Continuous playing without pack and without getting any chaal number for show Don't Show"
        }
        else if (rankCard_1 <= 11 && rankCard_2 <= 11 && rankCard_3 <= 11)
        {
            return Random.Range(1, 3);// "Pack"
        }
        else if (rankCard_1 > 11 || rankCard_2 > 11 || rankCard_3 > 11)
        {
            return Random.Range(3, 7);// " Give me chaal number in range of 3-7 Show"
        }

            return 0;//Pack
    }
    public void DebugLog(string Log)
    {
        ////Debug.Log("PUNLog is : " + Log);
    }

    void OnDestroy()//call for bot only but after confirmation
    {
      
        if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
        {
           
                if (PlayerSave.singleton != null)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        //Debug.Log("playerData.IsBot " + playerData.IsBot + " playerData._MobileNumber " + playerData._MobileNumber);
                        if (playerData.IsBot)
                        {
                            PlayerSave.singleton.CallGameExitForBotOnly(playerData._DeviceID,0, "B", null);
                        }
                    }
                    else if(PhotonNetwork.PlayerList.Length==0)
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            if (playerData.IsBot)
                            {
                                PlayerSave.singleton.CallGameExitForBotOnly(playerData._DeviceID, 0, "B", null);
                            }
                        }
                    }
                    
                }

        }
        

    }
}
