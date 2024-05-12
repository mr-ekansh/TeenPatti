using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class LocalPlayerPun : MonoBehaviour
{
    private Button homeButton;
    private Button ShowButton;
    private Button ChaalButton;
    private Button PackButton;
    private Button IncreaseBet, DecreaseBet;
    private Button AcceptSide, DeclineSide;
    private Button ChatButton;
    private Text textLocalInfo, textChat, textMoney;
    private GameObject textLocalInfoBlackBg;
    private GameObject PanelAcceptSideShow, PanelChat;
    private InputField inputChat;
    private PlayerManagerPun playerManager;

    private bool isButtonDown;
    private Button TipButton;
    public GameObject ChatMessageButtonPrefab;
    public GameObject gridView;
    private Text ChaalOrBlind;

    private GameObject PanelLeaveGame;
    private Button LeaveGameYes, LeaveGameNo;

    private GameObject PanelWithdrawMoney;
    private Button WithdrawGameYes, WithdrawGameNo;

	private GameObject PanelOutOfLimit;
	private Button OutofLimitGameYes, OutofLimitGameNo;


    private SpriteAtlas cardAtlas;
    private Animator m_Animator;
    private void Awake()
    {
        cardAtlas = Resources.Load<SpriteAtlas>("Cards");
    }
    // Use this for initialization
    protected void Start()
    {
        
        Transform gui = FindObjectOfType<TeenPatiHUD>().transform.Find("PlayerLocalPanel");
        m_Animator = gui.Find("ImageBack").gameObject.GetComponent<Animator>();
        ShowButton = gui.Find("ImageBack/ButtonShow").GetComponent<Button>();
        ShowButton.onClick.AddListener(OnSeenCard);
        
        ChaalButton = gui.Find("ImageBack/ButtonChall").GetComponent<Button>();
        ChaalButton.onClick.AddListener(OnChaal);
        ChaalOrBlind = gui.Find("ImageBack/ButtonChall/Text").GetComponent<Text>();
        IncreaseBet = gui.Find("ImageBack/ButtonAdd").GetComponent<Button>();
        IncreaseBet.onClick.AddListener(OnIncreaseBet);
        DecreaseBet = gui.Find("ImageBack/ButtonLess").GetComponent<Button>();
        DecreaseBet.onClick.AddListener(OnDecreaseBet);
        PackButton = gui.Find("ImageBack/ButtonPack").GetComponent<Button>();
        ChatButton = gui.Find("ButtonChat").GetComponent<Button>();
        ChatButton.onClick.AddListener(OnChatButton);
        PanelChat = gui.Find("PanelChat").gameObject;
        inputChat = gui.Find("PanelChat/InputField").GetComponent<InputField>();
        inputChat.onEndEdit.AddListener(OnEndChat);
        //textChat = gui.Find("PanelChat/Scroll View/Viewport/TextChat").GetComponent<Text>();
        gridView = gui.Find("PanelChat/Scroll View/Viewport/Content").gameObject;
        PanelChat.GetComponent<CanvasGroup>().alpha = 1;
        PanelChat.SetActive(false);
        PackButton.onClick.AddListener(OnPack);
        //textCurrentBoot = gui.Find("TextCurrentBoot").GetComponent<Text>();
        textLocalInfoBlackBg = gui.Find("TextLocalInfoBlackBg").gameObject;
        textLocalInfo = gui.Find("TextLocalInfoBlackBg/TextLocalInfo").GetComponent<Text>();
        PanelAcceptSideShow = gui.Find("PanelAcceptSideShow").gameObject;
        AcceptSide = PanelAcceptSideShow.transform.Find("ButtonYes").GetComponent<Button>();
        AcceptSide.onClick.AddListener(OnAcceptSideShow);
        DeclineSide = PanelAcceptSideShow.transform.Find("ButtonNo").GetComponent<Button>();
        DeclineSide.onClick.AddListener(OnDeclineSideShow);
        homeButton = GameObject.Find("HomeButton").GetComponent<Button>();
        homeButton.onClick.AddListener(OnOpenLeaveGamePanel);

        PanelLeaveGame = gui.Find("PanelLeaveGame").gameObject;
        LeaveGameYes = PanelLeaveGame.transform.Find("Display/ButtonYes").GetComponent<Button>();
        LeaveGameYes.onClick.AddListener(OnHomeButton);
        LeaveGameNo = PanelLeaveGame.transform.Find("Display/ButtonNo").GetComponent<Button>();
        LeaveGameNo.onClick.AddListener(OnCloseLeaveGamePanel);

        PanelWithdrawMoney = gui.Find("PanelWithdrawMoney").gameObject;
        WithdrawGameYes = PanelWithdrawMoney.transform.Find("Display/ButtonYes").GetComponent<Button>();
        WithdrawGameYes.onClick.AddListener(OnWithdrawHomeButton);
        WithdrawGameNo = PanelWithdrawMoney.transform.Find("Display/ButtonNo").GetComponent<Button>();
        WithdrawGameNo.onClick.AddListener(OnCloseWithdrawGamePanel);

		PanelOutOfLimit = gui.Find("PanelOutofPotLimit").gameObject;
		OutofLimitGameYes = PanelOutOfLimit.transform.Find("Display/ButtonYes").GetComponent<Button>();
		OutofLimitGameYes.onClick.AddListener(OnOutOfLimitAddCashButton);
		OutofLimitGameNo = PanelOutOfLimit.transform.Find("Display/ButtonNo").GetComponent<Button>();
		OutofLimitGameNo.onClick.AddListener(OnGoToLobbyButton);//send to lobby

        //TipButton = GameObject.Find("TipButton").GetComponent<Button>();
        //TipButton.onClick.AddListener(OnTipButton);
        PanelAcceptSideShow.SetActive(false);
        PanelAcceptSideShow.GetComponent<CanvasGroup>().alpha = 1;
        playerManager = GetComponent<PlayerManagerPun>();

        ShowButton.GetComponentInChildren<Text>().text = "Side Show";
        ShowButton.interactable = false;
        ChatMessageButtonPrefab = Resources.Load<GameObject>("Prefabs/SendChatMessageButton");

        if (ChaalOrBlind) ChaalOrBlind.text = "Blind " + "\n" + playerManager.playerData.currentBootPlayer.ToString("F2"); 
        DeactivateAllButtons();
        DeactivateInfoText();

        for (int i = 0; i < StaticStrings.chatMessages.Length; i++)
        {
            GameObject button = Instantiate(ChatMessageButtonPrefab);
            button.transform.GetChild(0).GetComponent<Text>().text = StaticStrings.chatMessages[i];
            button.transform.SetParent(gridView.transform);
            button.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
            string index = StaticStrings.chatMessages[i];
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            button.GetComponent<Button>().onClick.AddListener(() => SendMessageEvent(index));
        }
        OnCloseLeaveGamePanel();
        OnCloseWithdrawGamePanel2();
		OnCloseOutOfLimitPanel();
    }
    public void OnOpenLeaveGamePanel()
    {
        PanelLeaveGame.SetActive(true);
        PanelLeaveGame.GetComponent<CanvasGroup>().alpha = 1;
    }
	public void OnOpenOutOfLimitPanel()
	{
		PanelOutOfLimit.SetActive(true);
		PanelOutOfLimit.GetComponent<CanvasGroup>().alpha = 1;
	}
    public void OnOpenWithdrawGamePanel()
    {
		Debug.Log("StaticValues.FirstTimeDepositPromptOpen "+StaticValues.FirstTimeDepositPromptOpen);
		Debug.Log("PlayerPrefs.PermanentPopUpClose "+PlayerPrefs.GetInt("PermanentPopUpClose",0));
		if(PlayerPrefs.GetInt("PermanentPopUpClose",0)==0)
		{
			if(StaticValues.FirstTimeDepositPromptOpen)
			{
				StaticValues.FirstTimeDepositPromptOpen = false;
				StaticValues.FirstTimeDepositPromptClose = true;
				PanelWithdrawMoney.SetActive(true);
				PanelWithdrawMoney.GetComponent<CanvasGroup>().alpha = 1;
				PlayerPrefs.SetInt("PermanentPopUpClose",1);
			}
		}
        
    }
    public void OnCloseWithdrawGamePanel2()
    {
        PanelWithdrawMoney.SetActive(false);
        PanelWithdrawMoney.GetComponent<CanvasGroup>().alpha = 1;
    }
    public void OnCloseWithdrawGamePanel()
    {
        if (PanelWithdrawMoney.activeSelf)
        {
            StaticValues.FirstTimeDepositPromptClose = true;
            PanelWithdrawMoney.SetActive(false);
            PanelWithdrawMoney.GetComponent<CanvasGroup>().alpha = 1;
        }
    }
    public void OnCloseLeaveGamePanel()
    {
        PanelLeaveGame.SetActive(false);
        PanelLeaveGame.GetComponent<CanvasGroup>().alpha = 1;
    }
	public void OnCloseOutOfLimitPanel()
	{
		PanelOutOfLimit.SetActive(false);
		PanelOutOfLimit.GetComponent<CanvasGroup>().alpha = 1;
	}
    public void SendMessageEvent(string index)
    {
        Debug.Log("Button Clicked " + index);
       
        
        if (playerManager != null)
        {

            PhotonNetwork.RaiseEvent((int)EnumPhoton.SendChatMessage, index + ";" + playerManager.playerData._DeviceID, new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache }, SendOptions.SendReliable);

            //PanelChat.SetActive(false);


            if (playerManager.myUI != null)
            {
                int MyPositionId = playerManager.myUI.MyPositionID;
                //for (int i = 0; i < 5; i++)
                //{
                    playerManager.myUI.GetChatMessageBubble(MyPositionId).SetActive(true);
                    playerManager.myUI.GetChatMessageBubbleText(MyPositionId).GetComponent<Text>().text = index;
                    if (playerManager.myUI.GetChatMessageBubble(MyPositionId).GetComponent<Animator>().enabled)
                    {
                        playerManager.myUI.GetChatMessageBubble(MyPositionId).GetComponent<Animator>().Play("MessageBubbleAnimation");
                    }
                //}
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Home) || Input.GetKeyDown(KeyCode.Escape))
        {
            OnOpenLeaveGamePanel();
        }
    }
    public void ReplaceSidewithShow(int localValue)
    {
        if (localValue == 1)
        {
            ShowButton.GetComponentInChildren<Text>().text = "Show";
        }
        else
        {
            ShowButton.GetComponentInChildren<Text>().text = "Side Show";
        }
        if(playerManager!=null)
        {
            if(playerManager.managerMain!=null)
            {
                if (ShowButton.GetComponentInChildren<Text>().text == "Show")
                {
                    ShowButton.interactable = true;
                }
                else
                {
                    ShowButton.interactable = playerManager.managerMain.FindPreviousPlayerForShowOrSideShow(playerManager);
                }
            }
        }
    }
    public void RefreshMoneyTopBar()
    {
        //if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
        //{
        //    textMoney.text = PlayerSave.singleton.GetCurrentMoney().ToString("F2");
        //}
        //else if(PlayerSave.singleton.currentTable == eTable.Free)
        //{
        //    textMoney.text = PlayerSave.singleton.GetCurrentChips().ToString("F2");
        //}
    }

    public void SetTextMoneyTopBar(string _text)
    {
        //Debug.Log("SetTextMoneyTopBar  in lpp " + PlayerSave.singleton.GetCurrentMoney());
        //if(textMoney == null)
        //    textMoney = GameObject.Find("CoinBarPlayer/Text").GetComponent<Text>();
        //textMoney.text = _text;
    }

    public void NewChatText(string _textAdd, int uiOrder)
    {
        PanelChat.SetActive(true);
        if (uiOrder == 0)
            _textAdd = string.Format("<color=blue>{0}</color>", _textAdd);
        else if (uiOrder == 1)
            _textAdd = string.Format("<color=green>{0}</color>", _textAdd);
        else if (uiOrder == 2)
            _textAdd = string.Format("<color=orange>{0}</color>", _textAdd);
        else if (uiOrder == 3)
            _textAdd = string.Format("<color=red>{0}</color>", _textAdd);
        else if (uiOrder == 4)
            _textAdd = string.Format("<color=black>{0}</color>", _textAdd);
        _textAdd += "\r\n";
        //textChat.text += _textAdd;
    }

    public void StarSideShow()
    {
        Debug.Log("StarSideShow  ");
        SetLocalInfoText("start side show");
        if(PanelAcceptSideShow)PanelAcceptSideShow.SetActive(true);
        //Invoke("WaitForSixSecond", 6f);
       
        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.OnSideShowSound();
        }
    }
    public void PanelAcceptOff()
    {
        //Debug.Log("hello in localPlayer.show false .");
        if(PanelAcceptSideShow)PanelAcceptSideShow.SetActive(false);
    }
    public void WaitForSixSecond()
    {
        CancelInvoke("WaitForSixSecond");
        if (PanelAcceptSideShow)
        {
            if (PanelAcceptSideShow.activeSelf)
            {
                OnDeclineSideShow();
            }
        }
    }
    public void OnAcceptSideShow()
    {
        CancelInvoke("WaitForSixSecond");
        PanelAcceptSideShow.SetActive(false);
        
        if (playerManager != null)
        {
            if (playerManager.photonView != null)
            {
                playerManager.photonView.RPC("AcceptSideShow", RpcTarget.All);
                playerManager.photonView.RPC("AcceptSideShowOnlyMaster", RpcTarget.MasterClient);
            }
        }
    }
   
     public void OnDeclineSideShow()
    {
        CancelInvoke("WaitForSixSecond");
        PanelAcceptSideShow.SetActive(false);
        
        if (playerManager != null)
        {
            if (playerManager.photonView != null)
            {
                playerManager.photonView.RPC("DeclineSideShow", RpcTarget.All);
                playerManager.photonView.RPC("DeclineSideShowOnlyMaster", RpcTarget.MasterClient);
            }
        }

        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.OnSideShowRefuseSound();
        }
    }

    public void OnSeenCard()
    {
        

        if (playerManager != null)
        {
            playerManager.OnPlayerSeenCard();
        }
        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.OnSeenSound();
        }

    }

    public void SeenCardText()
    {
        if (playerManager != null)
        {
            if (playerManager.playerData.IsSeenCard)
            {
                //ShowButton.GetComponentInChildren<Text>().text = "Side Show";

                if (playerManager.managerMain != null)
                {
                    if (ShowButton.GetComponentInChildren<Text>().text == "Show")
                    {
                        ShowButton.interactable = true;
                    }
                    else
                    {
                        ShowButton.interactable = playerManager.managerMain.FindPreviousPlayerForShowOrSideShow(playerManager);
                    }
                }

                if (ChaalOrBlind) ChaalOrBlind.text = "Chaal" + "\n" + playerManager.playerData.currentBootPlayer.ToString("F2"); 
            }
        }
        if (PackButton.interactable)
        {
            //ShowButton.interactable = true;
        }
        else
        {
            if (playerManager != null)
            {
                if (playerManager.playerData.IsSeenCard)
                {
                    //ShowButton.interactable = false;
                   
                    if (ChaalOrBlind) ChaalOrBlind.text = "Chaal" + "\n" + playerManager.playerData.currentBootPlayer.ToString("F2");
                }
                else
                {
                    //ShowButton.interactable = true;
                    if (ChaalOrBlind) ChaalOrBlind.text = "Blind" + "\n" + playerManager.playerData.currentBootPlayer.ToString("F2");
                }
            }
            else
            {
                if (ChaalOrBlind) ChaalOrBlind.text = "Blind" + "\n" + playerManager.playerData.currentBootPlayer.ToString("F2");
            }
        }
    }

    public void TextShowButtonToSeen()
    {
        //ShowButton.GetComponentInChildren<Text>().text = "Seen";
        if (playerManager != null)
        {
            if (ChaalOrBlind) ChaalOrBlind.text = "Blind" + "\n" + playerManager.playerData.currentBootPlayer.ToString("F2");
        }
        else
        {
            if (ChaalOrBlind) ChaalOrBlind.text = "Blind" + "\n" + "0.00";
        }
    }

    public void TextCurrebyBoot(double _boot)
    {
        //textCurrentBoot.text = _boot.ToString("F2");
        if (!playerManager.playerData.IsSeenCard)
        {
            if (ChaalOrBlind) ChaalOrBlind.text = "Blind" + "\n" + _boot.ToString("F2");
        }
        else
        {
            if (ChaalOrBlind) ChaalOrBlind.text = "Chaal" + "\n" + _boot.ToString("F2");
        }
    }

    public void OnDecreaseBet()
    {
        IncreaseBet.interactable = true;
        DecreaseBet.interactable = false;
        if (playerManager != null)
        {
            //textCurrentBoot.text = playerManager.playerData.currentBootPlayer.ToString("F2");
            //if (ChaalOrBlind) ChaalOrBlind.text = ChaalOrBlind.text + "\n" + playerManager.playerData.currentBootPlayer.ToString("F2");
            if (!playerManager.playerData.IsSeenCard)
            {
                if (ChaalOrBlind) ChaalOrBlind.text = "Blind" + "\n" + playerManager.playerData.currentBootPlayer.ToString("F2");
            }
            else
            {
                if (ChaalOrBlind) ChaalOrBlind.text = "Chaal" + "\n" + playerManager.playerData.currentBootPlayer.ToString("F2");
            }
        }

        if (playerManager != null)
        {
            if (playerManager.photonView != null)
            {
                playerManager.photonView.RPC("DecreaseBet", RpcTarget.All);
            }
        }
    }

    public void OnIncreaseBet()
    {
        
        if (playerManager != null)
        {
            if (playerManager.CanIncreaseBoot())
            {
                IncreaseBet.interactable = false;
                DecreaseBet.interactable = true;
                //textCurrentBoot.text = (playerManager.playerData.currentBootPlayer * 2).ToString("F2");
                //if (ChaalOrBlind) ChaalOrBlind.text = ChaalOrBlind.text + "\n" + (playerManager.playerData.currentBootPlayer * 2).ToString("F2");
                if (!playerManager.playerData.IsSeenCard)
                {
                    if (ChaalOrBlind) ChaalOrBlind.text = "Blind" + "\n" + (playerManager.playerData.currentBootPlayer * 2).ToString("F2");
                }
                else
                {
                    if (ChaalOrBlind) ChaalOrBlind.text = "Chaal" + "\n" + (playerManager.playerData.currentBootPlayer * 2).ToString("F2");
                }
                if (playerManager.photonView != null)
                {
                    playerManager.photonView.RPC("IncreaseBet", RpcTarget.All);
                }
            }
            else
            {
                IncreaseBet.interactable = false;
                DecreaseBet.interactable = false;
            }
        }
        else
        {
            IncreaseBet.interactable = false;
            DecreaseBet.interactable = false;
        }
    }

    private void OnEndChat(string _chatText)
    {
        //playerManager.photonView.RPC("EndTypeChat", RpcTarget.MasterClient, playerManager.playerData.NamePlayer + ": " + _chatText);
        if (playerManager != null)
        {
            if (playerManager.photonView != null)
            {
                PhotonNetwork.RaiseEvent((int)EnumPhoton.SendTextMessage, _chatText + ";" + playerManager.playerData._DeviceID, new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache }, SendOptions.SendReliable);
                PanelChat.SetActive(false);


                if (playerManager.myUI != null)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        playerManager.myUI.GetChatMessageBubble(i).SetActive(true);
                        playerManager.myUI.GetChatMessageBubbleText(i).GetComponent<Text>().text = _chatText;
                        if (playerManager.myUI.GetChatMessageBubble(i).GetComponent<Animator>().enabled)
                        {
                            playerManager.myUI.GetChatMessageBubble(i).GetComponent<Animator>().Play("MessageBubbleAnimation");
                        }
                    }
                }
            }
        }
    }

    private void OnChatButton()
    {
        if (PanelChat.activeInHierarchy)
            PanelChat.SetActive(false);
        else
            PanelChat.SetActive(true);
    }
	public void OnOutOfLimitAddCashButton()
	{
		StaticValues.OutOfLimitPopUp = true;
		if (!isButtonDown)
		{
			if (playerManager != null)
			{
				if (playerManager.photonView != null)
				{
					playerManager.photonView.RPC("Disconnect", RpcTarget.All);
					isButtonDown = true;
				}
			}

		}
		TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
		if (hud)
		{
			hud.ClickSound();
		}
		OnCloseOutOfLimitPanel();

	}
	public void OnGoToLobbyButton()
	{
		if (!isButtonDown)
		{
			if (playerManager != null)
			{
				if (playerManager.photonView != null)
				{
					playerManager.photonView.RPC("Disconnect", RpcTarget.All);
					isButtonDown = true;
				}
			}

		}
		TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
		if (hud)
		{
			hud.ClickSound();
		}
		OnCloseOutOfLimitPanel();

	}
    public void OnHomeButton()
    {
       
       
        if(homeButton)
        {
            if(!homeButton.interactable)
            {
                return;
            }
        }
        if (!isButtonDown)
        {
            if (playerManager != null)
            {
                if (playerManager.photonView != null)
                {
                    playerManager.photonView.RPC("Disconnect", RpcTarget.All);
                    isButtonDown = true;
                }
            }
           
        }
        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.ClickSound();
        }
        OnCloseLeaveGamePanel();
    }
    public void OnWithdrawHomeButton()
    {
        StaticValues.FirstTimeDepositPromptClose = true;
        StaticValues.FirstTimeDepositPrompt = true;
        if (!isButtonDown)
        {
            if (playerManager != null)
            {
                if (playerManager.photonView != null)
                {
                    playerManager.photonView.RPC("Disconnect", RpcTarget.All);
                    isButtonDown = true;
                }
            }

        }
        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.ClickSound();
        }
        OnCloseWithdrawGamePanel();
    }
    private void OnTipButton()
    {
        
        if (playerManager != null)
        {
            if (playerManager.photonView != null)
            {

                playerManager.photonView.RPC("OnTipButton", RpcTarget.All);
            }
        }
        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.ClickSound();
        }
    }
    public void OnPack()
    {
        //Debug.Log("onn pack in lpp " +playerManager.playerData.NamePlayer);
        DeactivateAllButtons();
        
        if (playerManager != null)
        {
            if (playerManager.photonView != null)
            {

                playerManager.photonView.RPC("PackPlayer", RpcTarget.All);
            }
        }

        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.OnPackSound();
        }
    }

    public void OnChaal()
    {
        //Debug.Log("onn OnChaal in lpp");
        DeactivateAllButtons();
        
        if (playerManager != null)
        {
            if (playerManager.photonView != null)
            {

                playerManager.photonView.RPC("ChaalPlayer", RpcTarget.All);
            }
        }

        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (!playerManager.playerData.IsSeenCard)
        { 
            if (hud)
            {
                hud.OnBlindSound();
            }
        }
        else
        {
            if (hud)
            {
                hud.OnChaalSound();
            }
        }

    }

    public void SetLocalInfoText(string _text)
    {
        textLocalInfoBlackBg.SetActive(true);
        textLocalInfo.text = _text;
        Invoke("DeactivateInfoText", 1.5f);
    }

    public void DeactivateInfoText()
    {
        textLocalInfoBlackBg.SetActive(false);
    }

    public void ActivateAllButtons(bool changeToShowButton)
    {
        //Debug.Log("ActivateAllButtons-------------------------------");
        m_Animator.SetBool("Show", true);
        ShowButton.interactable = false;
        ChaalButton.interactable = true;

        if (playerManager == null)
        {
            return;
        }
        if (playerManager != null)
        {


            if (playerManager.playerData.currentBootPlayer < playerManager.GetChallLimit() || playerManager.GetChallLimit() == 0)
                IncreaseBet.interactable = true;
            else
                IncreaseBet.interactable = false;
            DecreaseBet.interactable = false;
            PackButton.interactable = true;

            if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
            {
                if (playerManager.playerData.Money < playerManager.playerData.currentBootPlayer)
                {
                    //Debug.Log("ActivateAllButtons pack in lpp");
                    OnPack();
                    DeactivateAllButtons();
                    PackButton.interactable = false;
                }
                else
                {
                    //Debug.Log("ActivateAllButtons else pack in lpp " + playerManager.playerData.currentBootPlayer);
                }
                SetTextMoneyTopBar(playerManager.playerData.Money.ToString("F2"));
            }
            else if (PlayerSave.singleton.currentTable == eTable.Free)
            {
                if (playerManager.playerData.Chips < playerManager.playerData.currentBootPlayer)
                {
                    //Debug.Log("ActivateAllButtons pack in lpp");
                    OnPack();
                    DeactivateAllButtons();
                    PackButton.interactable = false;
                }
                else
                {
                    //Debug.Log("ActivateAllButtons else pack in lpp " + playerManager.playerData.currentBootPlayer);
                }
                SetTextMoneyTopBar(playerManager.playerData.Chips.ToString("F2"));
            }
            

            if (!playerManager.playerData.IsSeenCard)
            {
                //if (_step <= 1)
                //{
                //    if (ShowButton.GetComponentInChildren<Text>().text.Equals("Show"))
                //    {
                //        ShowButton.interactable = false;
                //    }
                //    else
                //    {
                //        ShowButton.interactable = true;
                //    }
                //}
              
            }
            if (!playerManager.playerData.IsSeenCard)
            {
                TextShowButtonToSeen();
                ShowButton.interactable = false;
            }
            if (changeToShowButton)
                ShowCardText();
            else
            {
                SideShowCardText();
                SeenCardText();
            }
            //if (PackButton.interactable)
            //{
            //    if (ShowButton.GetComponentInChildren<Text>().text.Equals("Show"))
            //    {
            //        ShowButton.interactable = true;
            //    }
            //}
        }
        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.OnYourTurnSound();
        }

    }
    public void ShowCardText()
    {
        ShowButton.GetComponentInChildren<Text>().text = "Show";
        ShowButton.interactable = true;
    }
    public void SideShowCardText()
    {
        ShowButton.GetComponentInChildren<Text>().text = "Side Show";
    }
    public void SetActiveHomeButton(bool _active)
    {
        if (homeButton) homeButton.interactable = _active;
    }
    public void ActivateOnlyShowButtons()
    {
        //if (playerManager.playerData.playerType == ePlayerType.PlayerStartGame)
        //{
        //    //ShowButton.interactable = true;
        //    if (ChaalOrBlind) ChaalOrBlind.text = "Blind";
        //}
        //else
        //{
        //    //ShowButton.interactable = false;
        //}
        if (playerManager != null)
        {
            if (ChaalOrBlind) ChaalOrBlind.text = "Blind" + "\n" + playerManager.playerData.currentBootPlayer.ToString("F2"); 
        }
        else
        {
            if (ChaalOrBlind) ChaalOrBlind.text = "Blind" + "\n" + "0.00";
        }
        //ShowButton.interactable = false;
    }
    public void SetJoker(string jokerStr)
    {
        //if (_jokerSprite == null || cardAtlas == null) return;

        //if (string.IsNullOrEmpty(jokerStr))
        //{
        //    _jokerSprite.gameObject.SetActive(false);
        //    return;
        //}

        //var jokerCard = JsonUtility.FromJson<CardData>(jokerStr);
        //_jokerSprite.gameObject.SetActive(true);
        //_jokerSprite.sprite = cardAtlas.GetSprite(jokerCard.suitCard.ToString() + jokerCard.rankCard);
        //_jokerSprite.SetNativeSize();
        //_jokerSprite.rectTransform.localScale = new Vector3(.65f, .65f, 1);
    }
    public void AB_ActivateAllButtons(bool flag, int step)
    {
        //_andarBtn.interactable = flag;
        //_andarAddBtn.interactable = flag;
        //_andarSubBtn.interactable = flag;
        //_skipBtn.interactable = step != 1 && flag;
        //_baharBtn.interactable = flag;
        //_baharAddBtn.interactable = flag;
        //_baharSubBtn.interactable = flag;
        //_andarText.text = Mathf.Min(playerManager.playerData.Money, 100).ToString();
        //_baharText.text = Mathf.Min(playerManager.playerData.Money, 100).ToString();
    }

    public void SetAndar(string andarStr)
    {
        //if (_andarSprite == null || cardAtlas == null) return;

        //if (string.IsNullOrEmpty(andarStr))
        //{
        //    _andarSprite.gameObject.SetActive(false);
        //    return;
        //}

        //var card = JsonUtility.FromJson<CardData>(andarStr);
        //_andarSprite.gameObject.SetActive(true);
        //_andarSprite.sprite = cardAtlas.GetSprite(card.suitCard.ToString() + card.rankCard);
        //_andarSprite.SetNativeSize();
        //_andarSprite.rectTransform.localScale = new Vector3(.65f, .65f, 1);
    }
    public void SetBahar(string baharStr)
    {
        //if (_baharSprite == null || cardAtlas == null) return;

        //if (string.IsNullOrEmpty(baharStr))
        //{
        //    _baharSprite.gameObject.SetActive(false);
        //    return;
        //}

        //var card = JsonUtility.FromJson<CardData>(baharStr);
        //_baharSprite.gameObject.SetActive(true);
        //_baharSprite.sprite = cardAtlas.GetSprite(card.suitCard.ToString() + card.rankCard);
        //_baharSprite.SetNativeSize();
        //_baharSprite.rectTransform.localScale = new Vector3(.65f, .65f, 1);
    }
    public void DeactivateAllButtons()
    {
        //Debug.Log("DeactivateAllButtons-------------------------------");
        if (playerManager != null)
        {
            if (playerManager.playerData.playerType == ePlayerType.PlayerStartGame)
            {
                if (playerManager.playerData.IsSeenCard)
                {

                    //ShowButton.interactable = false;
                   
                    if (ChaalOrBlind) ChaalOrBlind.text = "Chaal" + "\n" + playerManager.playerData.currentBootPlayer.ToString("F2");
                }
                else
                {
                    if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
                    {
                        if (playerManager.playerData.Money >= playerManager.playerData.currentBootPlayer)
                        {

                            //ShowButton.interactable = true;
                            if (ChaalOrBlind) ChaalOrBlind.text = "Blind" + "\n" + playerManager.playerData.currentBootPlayer.ToString("F2");
                        }
                        else
                        {
                            //ShowButton.interactable = false;
                            if (ChaalOrBlind) ChaalOrBlind.text = "Blind" + "\n" + playerManager.playerData.currentBootPlayer.ToString("F2");
                        }
                    }
                    else if (PlayerSave.singleton.currentTable == eTable.Free)
                    {
                        if (playerManager.playerData.Chips >= playerManager.playerData.currentBootPlayer)
                        {

                            //ShowButton.interactable = true;
                            if (ChaalOrBlind) ChaalOrBlind.text = "Blind" + "\n" + playerManager.playerData.currentBootPlayer.ToString("F2");
                        }
                        else
                        {
                            //ShowButton.interactable = false;
                            if (ChaalOrBlind) ChaalOrBlind.text = "Blind" + "\n" + playerManager.playerData.currentBootPlayer.ToString("F2");
                        }
                    }
                }
            }
            else
            {
                //Debug.Log("in local player pun else else " + playerManager.playerData.NamePlayer);
                //ShowButton.interactable = false;

            }
        }
        ShowButton.interactable = false;
        ChaalButton.interactable = false;
        IncreaseBet.interactable = false;
        DecreaseBet.interactable = false;
        PackButton.interactable = false;
        if (playerManager != null)
        {
            if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
            {
                SetTextMoneyTopBar(playerManager.playerData.Money.ToString("F2"));
            }
            else
            {
                SetTextMoneyTopBar(playerManager.playerData.Chips.ToString("F2"));
            }
        }
        m_Animator.SetBool("Show", false);
        

    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {

            PhotonNetwork.SendAllOutgoingCommands();
            //Debug.Log("Application pause");
        }
        else
        {
            if (PanelAcceptSideShow)
            {
                if (PanelAcceptSideShow.activeSelf)
                {
                    PanelAcceptSideShow.SetActive(false);
                }
            }
            PhotonNetwork.SendAllOutgoingCommands();
            //Debug.Log("Application resume");
        }
    }
    private void OnApplicationQuit()
    {
        // OnHomeButton();
        PhotonNetwork.SendAllOutgoingCommands();

    }
    private void OnDestroy()
    {
       // OnHomeButton();

    }
}

