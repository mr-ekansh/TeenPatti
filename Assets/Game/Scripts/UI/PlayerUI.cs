using ExitGames.Client.Photon;
//using Facebook.Unity;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using TMPro;
using UIHealthAlchemy;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.U2D;
using UnityEngine.UI;


public class PlayerUI : MonoBehaviour
{

    public  int IdOrder;
    public string id;
    public bool IsFull;
    public Sprite  defaultAvatar;

   
 
    private Image[][] cards;
    private Image[][] BiggerCards;

    private SpriteAtlas cardAtlas;

    private RectTransform startCardPositionMove;
    private RectTransform finishMoneyPosition;
    private CanvasGroup myCanvasGroup;
    public PlayerManagerPun currentPlayer;
    private Color colorPaked = new Color(.2f, .2f, .2f);
    private GameObject panelPlayerInfo;
    public bool GameActive = false;

    //private Text avatarTimerText;
    private GameObject ChatBubbleText;
    private GameObject ChatBubble;

    public int MyPositionID = -1;
    public int _currentPlayerIndex = -1;

    public float _CurrentTimer = 0f;
    private Vector3 _startValue;
    private Vector3 _startSideShowPosition;
    public void OnEnable()
    {

       
        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        MyPositionID = -1;
        CountdownTimer.OnCountdownTimerHasUpdated += OnCountdownTimerHasUpdated;
        CallAwakeFromEnable();
    }
    public void EnableSlot(int p_slotIndex)
    {
        for (int i = 0; i < 5; i++)
        {
            transform.Find(i.ToString()).gameObject.SetActive(i == p_slotIndex);
           
            if (i == p_slotIndex)
            {
                
                MyPositionID = p_slotIndex;

                _startSideShowPosition = GetCurrentSideShowHighlighter(MyPositionID).gameObject.transform.position;
                panelPlayerInfo = transform.Find($"{i}/PanelPlayerInfo").gameObject;
                //var t = transform.Find($"{i}/AvatarVideo").GetComponent<RectTransform>();
                //switch (i)
                //{
                //    case 0:
                //        t.anchoredPosition = new Vector2(170, 200);
                //        break;
                //    case 1:
                //        t.anchoredPosition = new Vector2(20, 0);
                //        break;
                //    case 2:
                //        t.anchoredPosition = new Vector2(55, 100);
                //        break;
                //    case 3:
                //        t.anchoredPosition = new Vector2(20, -90);
                //        break;
                //    case 4:
                //        t.anchoredPosition = new Vector2(-330, 190);
                //        break;
                //}
            }
        }
    }
    
    public void CallAwakeFromEnable()
    {
        myCanvasGroup = GetComponent<CanvasGroup>();
        startCardPositionMove = GameObject.Find("PositionStartCard").GetComponent<RectTransform>();
        finishMoneyPosition = GameObject.Find("PositionFinishMoney").GetComponent<RectTransform>();
        cardAtlas = Resources.Load<SpriteAtlas>("NewCards");
        cards = new Image[5][];
        BiggerCards = new Image[5][];
        ////Debug.Log("Awake <color=red>ShowDealerIcon : ---------------------------------</color>" );
        for (int i = 0; i < 5; i++)
        {
            GetAvatarCirculParent(i).transform.gameObject.SetActive(false);
            GetChaalBlindMoney(i).transform.parent.gameObject.SetActive(false);
            GetDealerIcon(i).gameObject.SetActive(false);
            GetCurrentPlayerHighlighter(i).gameObject.SetActive(false);
            GetCurrentSideShowHighlighter(i).gameObject.SetActive(false);
            Image __avatarPlayer = GetAvatarPlayer(i);
            GetMoney(i).text = "";
            GetChaalBlindMoney(i).text = "0.00";
            GetAvatarPlayer(i).sprite = defaultAvatar;
            GetAvatarCircul(i).Value = 0.48f;
            GetAvatarCirculLine(i).localScale = new Vector3(1f, 0f, 1f);
            GetWinnerBase(i).SetActive(false);
            //Image __avatarFilled = GetAvatarFilled(i);

            //__avatarFilled.transform.localPosition = __avatarPlayer.transform.localPosition;
            //__avatarFilled.transform.localScale = Vector3.one;
            //__avatarFilled.type = Image.Type.Filled;
            __avatarPlayer.raycastTarget = true;

            //Button __avatarButton = __avatarPlayer.gameObject.AddComponent<Button>();

            //__avatarButton.onClick.AddListener(OnAvatarButton);

            Button __seeButton = GetSeenButton(i).gameObject.GetComponent<Button>();

            __seeButton.onClick.AddListener(onSeeButton);
            GetPanelPlayerInfo(i).SetActive(false);
            cards[i] = new Image[3];
            BiggerCards[i] = new Image[3];

            for (int j = 0; j < 3; j++)
            {
                if (j == 0)
                {
                    cards[i][j] = transform.Find(i.ToString() + "/Cards/Card").GetComponent<Image>();//transform.GetChild(j).GetComponent<Image>();
                    BiggerCards[i][j] = transform.Find(i.ToString() + "/CardsVertically/BigCard").GetComponent<Image>();//.transform.GetChild(j).GetComponent<Image>();
                }
                else
                {
                    cards[i][j] = transform.Find(i.ToString() + "/Cards/Card"+j.ToString()).GetComponent<Image>();//transform.GetChild(j).GetComponent<Image>();
                    BiggerCards[i][j] = transform.Find(i.ToString() + "/CardsVertically/BigCard" + j.ToString()).GetComponent<Image>();//.transform.GetChild(j).GetComponent<Image>();
                }
            }

            GetCardClose(i).sizeDelta = cards[i][0].rectTransform.sizeDelta;
        }
        myCanvasGroup.alpha = 1;
        ClearUI(1);
    }
    public Text GetMoney(int p_index)
    {
        return transform.Find(p_index.ToString() + "/ImageMoney/TextMoney").GetComponent<Text>();
    }
    public Text GetChaalBlindMoney(int p_index)
    {
        return transform.Find(p_index.ToString() + "/ChaalBase/TextMoney").GetComponent<Text>();
    }

    public Image GetDealerIcon(int p_index)
    {
        return transform.Find(p_index.ToString() + "/Dealer").GetComponent<Image>();
    }
    public Image GetCurrentPlayerHighlighter(int p_index)
    {
        return transform.Find(p_index.ToString() + "/DealerCards").GetComponent<Image>();
    }
    public RectTransform GetCurrentSideShowHighlighter(int p_index)
    {
        return transform.Find(p_index.ToString() + "/SideShowAnimation").GetComponent<RectTransform>();
    }
    public RectTransform GetCurrentSideShowHighlighterInner1(int p_index)
    {
        return transform.Find(p_index.ToString() + "/SideShowAnimation/SideShowCards_1").GetComponent<RectTransform>();
    }
    public RectTransform GetCurrentSideShowHighlighterInner2(int p_index)
    {
        return transform.Find(p_index.ToString() + "/SideShowAnimation/SideShowCards_2").GetComponent<RectTransform>();
    }
    public RectTransform GetCurrentSideShowHighlighterInner3(int p_index)
    {
        return transform.Find(p_index.ToString() + "/SideShowAnimation/SideShowCards_3").GetComponent<RectTransform>();
    }
    public Text GetInfoText(int p_index)
    {
        return transform.Find(p_index.ToString() + "/ImageInfoBase/InfoText").GetComponent<Text>();
    }
    public Text GetAvatarTimeText(int p_index)
    {
        return transform.Find(p_index.ToString() + "/AvatarCircul/AvatarTimerText").GetComponent<Text>();
    }

    public Text GetCardCombination(int p_index)
    {
        return transform.Find(p_index.ToString() + "/InfoCardText").GetComponent<Text>();
    }

    public Text GetInfoNamePlayer(int p_index)
    {
        return transform.Find(p_index.ToString() + "/ImagePlayerBase/InfoNamePlayer").GetComponent<Text>();
    }

   
    public Image GetAvatarPlayer(int p_index)
    {
        return transform.Find(p_index.ToString() + "/Avatar/AvatarFacebook").GetComponent<Image>();
    }
    public Image GetSeenButton(int p_index)
    {
        return transform.Find(p_index.ToString() + "/SeeButton").GetComponent<Image>(); 
    }

    public Image GetAvatarFilled(int p_index)
    {
        return transform.Find(p_index.ToString() + "/Avatar/AvatarFilled").GetComponent<Image>();
    }

    public MaterialHealhBar GetAvatarCircul(int p_index)
    {
        return transform.Find(p_index.ToString() + "/AvatarCircul/PlayerTimer").GetComponent<MaterialHealhBar>();
    }
    public RectTransform GetAvatarCirculLine(int p_index)
    {
        return transform.Find(p_index.ToString() + "/AvatarCircul/PlayerTimer/Back/line").GetComponent<RectTransform>();
    }
    public RectTransform GetAvatarCirculParent(int p_index)
    {
        return transform.Find(p_index.ToString() + "/AvatarCircul").GetComponent<RectTransform>();
    }
    public GameObject GetChatMessageBubble(int p_index)
    {
        return transform.Find(p_index.ToString() + "/MessageBubble").gameObject;
    }
    public GameObject GetChatMessageBubbleText(int p_index)
    {
        return transform.Find(p_index.ToString() + "/MessageBubble/Text").gameObject;
    }
    public GameObject GetPanelPlayerInfo(int p_index)
    {
        return transform.Find(p_index.ToString() + "/PanelPlayerInfo").gameObject;
    }

    public Image GetMovingMoney(int p_index)
    {
        return transform.Find(p_index.ToString() + "/GiveMoney").GetComponent<Image>();
    }

    public GameObject GetWinnerBase(int p_index)
    {
        return transform.Find(p_index.ToString() + "/WinnerBase").gameObject;
    }
    public Animator GetWinnerBaseAnim(int p_index)
    {
        return transform.Find(p_index.ToString() + "/WinnerBase/Winner").GetComponent<Animator>();
    }

    public Text GetTextMovingMoney(int p_index)
    {
        return GetMovingMoney(p_index).GetComponentInChildren<Text>();
    }
    public RectTransform GetCardClose(int p_index)
    {
        return transform.Find(p_index.ToString() + "/CardClose").GetComponent<RectTransform>();
    }

    public RectTransform GetAB(int p_index)
    {
        return transform.Find(p_index.ToString() + "/AB").GetComponent<RectTransform>();
    }

    
    public void ClearCards()
    {
        if (cards == null)
        {
            return;
        }

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                cards[i][j].enabled = false;
            }
        }

        if(BiggerCards ==null)
        {
            return;
        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                BiggerCards[i][j].enabled = false;
            }
        }
    }
    public void ActiveSeeButton()// these conditions are commented because of third card to self player is missing the sequence animation.
    {
        //Debug.Log("ActiveSeeButton ");
        //for (int i = 0; i < 5; i++)
        //{
        //    //for (int j = 0; j < 3; j++)
        //    //{
        //    //    //cards[i][j].enabled = true;
        //    //    //BiggerCards[i][j].enabled = false;
        //    //}
            
        //    //GetSeenButton(i).gameObject.SetActive(true);
        //    //GetCardClose(i).gameObject.SetActive(false);
        //}
        for (int i = 0; i < 5; i++)
        {
            if(currentPlayer!=null)
            {
                if(currentPlayer.playerData.IsBot)
                {
                    GetSeenButton(i).gameObject.SetActive(false);
                }
                else
                {
                    if (currentPlayer.playerData.playerType == ePlayerType.PlayerStartGame)
                    {
                        //Debug.Log("GetSeen button");
                        if (currentPlayer.playerData.currentCombination != eCombination.Empty)
                        {
                            GetSeenButton(i).gameObject.SetActive(true);
                        }
                    }
                }
            }
            else
            {
                GetSeenButton(i).gameObject.SetActive(false);
            }
        }
           
    }
    public void onSeeButton()
    {
        ////Debug.LogWarning("OnSeeButton outer");
        if (currentPlayer != null)
        {
            if (currentPlayer.photonView != null)
            {
                if (currentPlayer.photonView.IsMine)
                {
                    if (!currentPlayer.playerData.IsSeenCard)
                    {
                        ////Debug.LogWarning("OnSeeButton inner");
                        currentPlayer.photonView.RPC("SetSeenCardTrue", RpcTarget.All);
                    }
                }
            }
        }
        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.OnPlayerSound();
        }
    }
    public void SetIsFull(bool _value)
    {
        AnimationCounter = false;
        IsFull = _value;
        for (int i = 0; i < 5; i++)
        {
            
            GetSeenButton(i).gameObject.SetActive(false);
           
        }
        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.OnForceStopSound();
        }
       
       
        //SwapSeats(IdOrder);
    }
    public bool GetIsFull()
    {
        return IsFull;
    }
    public void OnAvatarButton()
    {
        //panelPlayerInfo.SetActive(true);
        if (!currentPlayer.photonView.IsMine)
        {
                        
        }
        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if(hud)
        {
            hud.OnPlayerSound();
        }
    }
    public void ShowDealerIcon(int currentPlayerIndex)
    {
        ////Debug.Log("ShowDealerIcon <color=red>ShowDealerIcon : {0}---------------------------------</color>" + currentPlayerIndex);
        _currentPlayerIndex = currentPlayerIndex;
        for (int i = 0; i < 5; i++)
        {
            GetDealerIcon(i).gameObject.SetActive(true);
        }
    }
    public void WaitForSomeSecondsToInit(PlayerManagerPun _currentPlayer)
    {
        //Debug.Log("WaitForSomeSecondsToInit");
        currentPlayer = _currentPlayer;
        CancelInvoke("InitUIAfterSomeSeconds");
        Invoke("InitUIAfterSomeSeconds", UnityEngine.Random.Range(1.5f, 2.5f));
    }
    void InitUIAfterSomeSeconds()
    {
        //Debug.Log("InitUIAfterSomeSeconds");
       
       
        if (!currentPlayer.playerData.IsBot)
        {
            ThisInitForMasterOnly(currentPlayer);
        }
    }
    public void ThisInitForMasterOnly(PlayerManagerPun _currentPlayer)
    {
        //Debug.Log("ThisInitForMasterOnly");
        if (_currentPlayer == null)
        {
            return;
        }
       
        AnimationCounter = false;
        currentPlayer = _currentPlayer;
        GameActive = true;
        //int level = (int) currentPlayer.playerData.experience / 100;

        ////Debug.Log("Init <color=red>ShowDealerIcon : {0}---------------------------------</color> dealer icon false  " + _currentPlayer.playerData.Money);
        for (int i = 0; i < 5; i++)
        {
            GetAvatarPlayer(i).enabled = true;
            GetAvatarPlayer(i).sprite = defaultAvatar;//ADDED LINE BY ME
            GetAvatarCirculParent(i).transform.gameObject.SetActive(false);
            BelowAvatarCircul(i);
            GetDealerIcon(i).gameObject.SetActive(false);
            GetCurrentPlayerHighlighter(i).gameObject.SetActive(false);

            GetCurrentSideShowHighlighter(i).gameObject.SetActive(false);
            GetWinnerBase(i).SetActive(false);
            //var text = GetPanelPlayerInfo(i).transform.Find("TextLevel").GetComponent<Text>();
            //text.text = level.ToString();
            //text.color = Color.black;

            //text = GetPanelPlayerInfo(i).transform.Find("TextName").GetComponent<Text>(); 
            //text.text = currentPlayer.playerData.NamePlayer;
            //text.color = Color.black;

            //text = GetPanelPlayerInfo(i).transform.Find("TextEarnings").GetComponent<Text>();
            //text.text = currentPlayer.playerData.Money.ToString();
            //text.color = Color.black;

            //text = GetPanelPlayerInfo(i).transform.Find("TextLocation").GetComponent<Text>();
            //// text.text = text.text = currentPlayer.playerData.Location;
            //text.text = currentPlayer.playerData.experience.ToString();
            //text.color = Color.black;

            //text = GetPanelPlayerInfo(i).transform.Find("TextRank").GetComponent<Text>();
            //text.text = currentPlayer.playerData.experience.ToString();
            //text.color = Color.black;
            GetInfoNamePlayer(i).transform.parent.gameObject.SetActive(true);
            GetInfoNamePlayer(i).text = _currentPlayer.playerData.NamePlayer;
            GetMoney(i).transform.parent.gameObject.SetActive(true);
            GetMoney(i).text = _currentPlayer.playerData.Money.ToString("F2");
            if (_currentPlayer.managerMain != null)
            {
                if (_currentPlayer.managerMain.typeTable == eTable.AndarBahar)
                {
                    ////Debug.Log("GetAB");
                    GetAB(i).gameObject.SetActive(true);
                    ////Debug.Log("GetAB_A");
                   // GetAB_A_Text(i).text = string.Empty;
                    ////Debug.Log("GetAB_B");
                   // GetAB_B_Text(i).text = string.Empty;

                    ////Debug.Log("GetMovingMoney");
                    GetMovingMoney(i).gameObject.SetActive(false);
                    ////Debug.Log("GetCard");
                    var cards = GetCards(i);
                    if (cards != null && cards.Length > 0) cards[0].transform.parent.gameObject.SetActive(false);
                    ////Debug.Log("FinishCard");
                }
                else
                {

                    GetMovingMoney(i).gameObject.SetActive(false);
                    GetMovingMoney(i).transform.position = GetMoney(i).transform.position;
                }
            }
            if (_currentPlayer.playerData.AvatarPic >= 0 && _currentPlayer.playerData.AvatarPic < 27)
            {
                GetAvatarPlayer(i).sprite = PlayerSave.singleton._avatarImages[_currentPlayer.playerData.AvatarPic];
            }
            else
            {
                GetAvatarPlayer(i).sprite = PlayerSave.singleton._avatarImages[0];
            }

        }


        ////Debug.Log("GiveTextMoney");
        if (PlayerSave.singleton.currentTable == eTable.Free)
        {
            GiveTextMoney(_currentPlayer.playerData.Chips, 0);
        }
        else if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
        {
            GiveTextMoney(_currentPlayer.playerData.Money, 0);
        }

        //if (!string.IsNullOrEmpty(currentPlayer.playerData.FacebookId))
        //{
        //    FB.API(currentPlayer.playerData.FacebookId + "/picture?width=50&height=50", HttpMethod.GET, AvatarCallBack);
        //}
        //else
        //{
        for (int i = 0; i < 5; i++)
        {
            //GetAvatarPlayer(i).sprite = defaultAvatar;
            //GetAvatarFilled(i).sprite = defaultAvatar;
            GetInfoText(i).text = "";
            GetInfoText(i).transform.parent.gameObject.SetActive(false);
            GetAvatarTimeText(i).text = "";
            GetSeenButton(i).gameObject.SetActive(false);
        }
        //}
        OnceDownloadImage = false;
        for (int i = 0; i < 5; i++)
        {

            if (currentPlayer.playerData.uploadPic != null)
            {


                if (!string.IsNullOrEmpty(currentPlayer.playerData.uploadPic))
                {
                    CallOnceInFrame(currentPlayer.playerData.uploadPic);
                }

            }
        }

        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.OnForceStopSound();
        }
        if (_currentPlayer.managerMain != null)
        {
            if (_currentPlayer.managerMain.managerInfo.isStartedGame)
            {
                //Debug.Log("_currentPlayer.managerMain.managerInfo.isStartedGame " + _currentPlayer.managerMain.managerInfo.isStartedGame);
                for (int i = 0; i < _currentPlayer.playerData.currentCards.Length; i++)
                {
                    GetCardClose(i).gameObject.SetActive(false);
                    if (_currentPlayer.playerData.playerType == ePlayerType.PlayerStartGame)
                    {
                        SetInitCard(_currentPlayer.playerData.currentCards[i], i, true);
                        if (_currentPlayer.playerData.IsPacked)
                        {
                            SetPacked();
                        }
                    }
                    else
                    {
                        //SetInitCard(_currentPlayer.playerData.currentCards[i], i, false);
                    }

                }
            }
        }
        if (currentPlayer != null)
        {
            currentPlayer.playerData.isPlayerActive = true;
        }
        if (PhotonNetwork.IsMasterClient)
        {
            List<PlayerManagerPun> players = FindObjectsOfType<PlayerManagerPun>().ToList();
            if (players.Count == 2)
            {
                if (!currentPlayer.managerMain.managerInfo.isStartedGame)
                {
                    currentPlayer.managerMain.GiveCardsToPlayersFromPlayerUIOnceOnly();
                }
            }
        }

    }
    public void Init(PlayerManagerPun _currentPlayer)
    {

        if(_currentPlayer ==null)
        {
            return;
        }
        if(currentPlayer !=null)
        {
            return;
        }
        AnimationCounter = false;
        currentPlayer = _currentPlayer;
        GameActive = true;
        //int level = (int) currentPlayer.playerData.experience / 100;
       
        ////Debug.Log("Init <color=red>ShowDealerIcon : {0}---------------------------------</color> dealer icon false  " + _currentPlayer.playerData.Money);
        for (int i = 0; i < 5; i++)
        {
            GetAvatarPlayer(i).enabled = true;
            GetAvatarPlayer(i).sprite = defaultAvatar;//ADDED LINE BY ME
            GetAvatarCirculParent(i).transform.gameObject.SetActive(false);
            BelowAvatarCircul(i);
            GetDealerIcon(i).gameObject.SetActive(false);
            GetCurrentPlayerHighlighter(i).gameObject.SetActive(false);

            GetCurrentSideShowHighlighter(i).gameObject.SetActive(false);
            GetWinnerBase(i).SetActive(false);
            //var text = GetPanelPlayerInfo(i).transform.Find("TextLevel").GetComponent<Text>();
            //text.text = level.ToString();
            //text.color = Color.black;

            //text = GetPanelPlayerInfo(i).transform.Find("TextName").GetComponent<Text>(); 
            //text.text = currentPlayer.playerData.NamePlayer;
            //text.color = Color.black;

            //text = GetPanelPlayerInfo(i).transform.Find("TextEarnings").GetComponent<Text>();
            //text.text = currentPlayer.playerData.Money.ToString();
            //text.color = Color.black;

            //text = GetPanelPlayerInfo(i).transform.Find("TextLocation").GetComponent<Text>();
            //// text.text = text.text = currentPlayer.playerData.Location;
            //text.text = currentPlayer.playerData.experience.ToString();
            //text.color = Color.black;

            //text = GetPanelPlayerInfo(i).transform.Find("TextRank").GetComponent<Text>();
            //text.text = currentPlayer.playerData.experience.ToString();
            //text.color = Color.black;
            GetInfoNamePlayer(i).transform.parent.gameObject.SetActive(true);
            GetInfoNamePlayer(i).text = _currentPlayer.playerData.NamePlayer;
            GetMoney(i).transform.parent.gameObject.SetActive(true);
            GetMoney(i).text = _currentPlayer.playerData.Money.ToString("F2");
            if (_currentPlayer.managerMain != null)
            {
                if (_currentPlayer.managerMain.typeTable == eTable.AndarBahar)
                {
                    ////Debug.Log("GetAB");
                    GetAB(i).gameObject.SetActive(true);
                    ////Debug.Log("GetAB_A");
                    //GetAB_A_Text(i).text = string.Empty;
                    ////Debug.Log("GetAB_B");
                    //GetAB_B_Text(i).text = string.Empty;

                    ////Debug.Log("GetMovingMoney");
                    GetMovingMoney(i).gameObject.SetActive(false);
                    ////Debug.Log("GetCard");
                    var cards = GetCards(i);
                    if (cards != null && cards.Length > 0) cards[0].transform.parent.gameObject.SetActive(false);
                    ////Debug.Log("FinishCard");
                }
                else
                {

                    GetMovingMoney(i).gameObject.SetActive(false);
                    GetMovingMoney(i).transform.position = GetMoney(i).transform.position;
                }
            }
            if (_currentPlayer.playerData.AvatarPic >= 0 && _currentPlayer.playerData.AvatarPic<27)
            {
                GetAvatarPlayer(i).sprite = PlayerSave.singleton._avatarImages[_currentPlayer.playerData.AvatarPic];
            }
            else
            {
                GetAvatarPlayer(i).sprite = PlayerSave.singleton._avatarImages[0];
            }
           
        }
        

        ////Debug.Log("GiveTextMoney");
        if (PlayerSave.singleton.currentTable == eTable.Free)
        {
            GiveTextMoney(_currentPlayer.playerData.Chips, 0);
        }
        else if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
        {
            GiveTextMoney(_currentPlayer.playerData.Money, 0);
        }
       
        //if (!string.IsNullOrEmpty(currentPlayer.playerData.FacebookId))
        //{
        //    FB.API(currentPlayer.playerData.FacebookId + "/picture?width=50&height=50", HttpMethod.GET, AvatarCallBack);
        //}
        //else
        //{
        for (int i = 0; i < 5; i++)
        {
                //GetAvatarPlayer(i).sprite = defaultAvatar;
                //GetAvatarFilled(i).sprite = defaultAvatar;
                GetInfoText(i).text = "";
                GetInfoText(i).transform.parent.gameObject.SetActive(false);
                GetAvatarTimeText(i).text = "";
                GetSeenButton(i).gameObject.SetActive(false);
        }
        //}
        OnceDownloadImage = false;
        for (int i = 0; i < 5; i++)
        {
            
            if (currentPlayer.playerData.uploadPic != null)
            {
                
                
                if (!string.IsNullOrEmpty(currentPlayer.playerData.uploadPic))
                {
                    CallOnceInFrame(currentPlayer.playerData.uploadPic);
                }
                
            }
        }

        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.OnForceStopSound();
        }
        if(_currentPlayer.managerMain!=null)
        {
            if(_currentPlayer.managerMain.managerInfo.isStartedGame)
            {
                //Debug.Log("_currentPlayer.managerMain.managerInfo.isStartedGame " + _currentPlayer.managerMain.managerInfo.isStartedGame);
                for (int i = 0; i < _currentPlayer.playerData.currentCards.Length; i++)
                {
                    GetCardClose(i).gameObject.SetActive(false);
                    if (_currentPlayer.playerData.playerType == ePlayerType.PlayerStartGame)
                    {
                        SetInitCard(_currentPlayer.playerData.currentCards[i], i, true);
                        if (_currentPlayer.playerData.IsPacked)
                        {
                            SetPacked();
                        }
                    }
                    else
                    {
                        //SetInitCard(_currentPlayer.playerData.currentCards[i], i, false);
                    }
                    
                }
            }
        }
       
    }
    private bool OnceDownloadImage = false;
    private void CallOnceInFrame(string _imageUrl)
    {
        if (!OnceDownloadImage)
        {
            DownloadImage(_imageUrl);
        }
    }
    private void DownloadImage(string _imageUrl)
    {
        if (!string.IsNullOrEmpty(_imageUrl))
        {
            StartCoroutine(OnLoadGraphic(_imageUrl));
            OnceDownloadImage = true;
        }

    }
    private IEnumerator OnLoadGraphic(string imageUrl)
    {
        string _url = imageUrl;

        if (!string.IsNullOrEmpty(_url) && (_url.StartsWith("http") || _url.StartsWith("file")))
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(_url);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                ////Debug.Log(www.error);
                OnceDownloadImage = false;
            }
            else
            {
                Texture2D _texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                if (_texture != null)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        GetAvatarPlayer(i).sprite = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);

                        
                    }
                    OnceDownloadImage = true;
                }
                else
                {
                    OnceDownloadImage = false;
                }
            }
        }
    }
    public void SetSideShow(string _deviceID)
    {
        //Debug.Log("SetSideShow");
        newSec = -1f;
        SideShowCounter = false;
        inBackground = false;


        Vector3 startValue = Vector3.zero;
        int NewPositionId = -1;
        int NewYPosition = 0;
        int NewXPosition = 0;
        try
        {
            List<PlayerManagerPun> players = FindObjectsOfType<PlayerManagerPun>().ToList();
            int currentPlayer = players.FindIndex(a => a.playerData._DeviceID.Equals(_deviceID));
            if (currentPlayer > -1)
            {
                NewPositionId = players[currentPlayer].myUI.MyPositionID;
                startValue = players[currentPlayer].myUI.GetCurrentSideShowHighlighter(NewPositionId).gameObject.transform.position;
            }
            //Debug.Log("<color=red>MyPositionID...... " + MyPositionID + " .........NewPositionId.......... " + NewPositionId + " </color>");
         

            if ((NewPositionId == 0 && MyPositionID ==4) || (NewPositionId == 1 && MyPositionID == 3) || (NewPositionId == 2 && MyPositionID == 3) || (NewPositionId == 1 && MyPositionID == 2))
            {
                NewXPosition = -1;
                NewYPosition = 0;
            }
            else if ((NewPositionId == 0 && MyPositionID == 3) || (NewPositionId == 0 && MyPositionID == 2))
            {
                NewXPosition = -1;
                NewYPosition = -1;
            }
            else if ((NewPositionId == 0 && MyPositionID == 1) || (NewPositionId == 4 && MyPositionID == 3))
            {
                NewXPosition = 0;
                NewYPosition = -1;
            }
            else if ((NewPositionId == 1 && MyPositionID == 0) || (NewPositionId == 3 && MyPositionID == 4))
            {
                NewXPosition = 0;
                NewYPosition = 1;
            }
            else if ((NewPositionId == 1 && MyPositionID == 4) || (NewPositionId == 2 && MyPositionID == 4) )
            {
                NewXPosition = -1;
                NewYPosition = 1;
            }
            else if ((NewPositionId == 2 && MyPositionID == 0) || (NewPositionId == 3 && MyPositionID == 0))
            {
                NewXPosition = 1;
                NewYPosition = 1;
            }
            else if ((NewPositionId == 3 && MyPositionID == 1) || (NewPositionId == 4 && MyPositionID == 0) || (NewPositionId == 2 && MyPositionID == 1) || (NewPositionId == 3 && MyPositionID == 2))
            {
                NewXPosition = 1;
                NewYPosition = 0;
            }
            else if ((NewPositionId == 4 && MyPositionID == 1) || (NewPositionId == 4 && MyPositionID == 2))
            {
                NewXPosition = 1;
                NewYPosition = -1;
            }

            //Debug.Log("after....<color=red>MyPositionID...... " + MyPositionID + " .........NewPositionId.......... " + NewPositionId + " </color>");
            GetCurrentSideShowHighlighterInner1(MyPositionID).gameObject.transform.localPosition = new Vector3(NewXPosition *- 40f, NewYPosition * -40f, 0f);
            GetCurrentSideShowHighlighterInner1(MyPositionID).gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            GetCurrentSideShowHighlighterInner2(MyPositionID).gameObject.transform.localPosition = new Vector3(NewXPosition *- 80f, NewYPosition * -80f, 0f);
            GetCurrentSideShowHighlighterInner2(MyPositionID).gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            GetCurrentSideShowHighlighterInner3(MyPositionID).gameObject.transform.localPosition = new Vector3(NewXPosition *- 120f, NewYPosition * -120f, 0f);
            GetCurrentSideShowHighlighterInner3(MyPositionID).gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            
       }
        catch
        {
            startValue = Vector3.zero;
        }
        _startValue = startValue;
        
       
        SideShowAnimationReverse(_startValue, new float[] { NewPositionId, NewYPosition,NewXPosition, _startValue.x, _startValue .y, _startValue .z});
        StopCoroutine("CurrentSideShow");
        StartCoroutine("CurrentSideShow");
    }
    public void StopSideShow()
    {
        ////Debug.Log("stopsideshow");
        SideShowCounter = false;
        StopCoroutine("CurrentSideShow");
        inBackground = false;
        int i = MyPositionID;
        GetAvatarCirculParent(i).transform.gameObject.SetActive(false);
        GetWinnerBase(i).SetActive(false);
        //GetAvatarCircul(i).color = new Color32(255, 255, 255, 255);
        BelowAvatarCircul(i);
        iTween.Stop(GetCurrentSideShowHighlighter(MyPositionID).gameObject);
        GetCurrentSideShowHighlighter(MyPositionID).gameObject.transform.position = _startSideShowPosition;
        GetCurrentSideShowHighlighter(i).gameObject.SetActive(false);
        GetCurrentPlayerHighlighter(i).gameObject.SetActive(false);
        newSec = -1f;


      

      

        if (currentPlayer)
        {
            if (!currentPlayer.playerData.IsBot)
            {
                if (currentPlayer.localPlayer != null)
                {
                    currentPlayer.localPlayer.PanelAcceptOff();
                }
            }
        }
    }
    private IEnumerator OldCurrentSideShow()
    {

        inBackground = false;

        int ik = MyPositionID;
        GetAvatarCirculParent(ik).transform.gameObject.SetActive(false);
        //GetAvatarCircul(ik).color = new Color32(255, 255, 255, 255);
        BelowAvatarCircul(ik);
        iTween.Stop(GetCurrentSideShowHighlighter(ik).gameObject);
        GetCurrentSideShowHighlighter(ik).gameObject.SetActive(false);
        GetCurrentPlayerHighlighter(ik).gameObject.SetActive(false);
        for (float i = 6; i > 0; i -= 1f)
        {
            if (newSec > -1f && newSec < i && inBackground)
            {
                inBackground = false;
                i = newSec;
                ////Debug.Log("i " + i + "newSecCurrentSideShow " + newSec);
            }
            //if (i > 1f)
            //{
            //    avatarFilled.fillAmount = Mathf.Clamp(1f - (i / 15f), 0f, 1f);
            //}
            //else
            //{
            //    avatarFilled.fillAmount = 1f;
            //}
            if (i <= 2 && !SideShowCounter)
            {
                SideShowCounter = true;

            }
            //avatarTimerText.text = i.ToString();
            yield return new WaitForSeconds(1f);

        }
        int ij = MyPositionID;
        GetAvatarCirculParent(ij).transform.gameObject.SetActive(false);
        //GetAvatarCircul(ij).color = new Color32(255, 255, 255, 255);
        BelowAvatarCircul(ij);
      
        GetCurrentPlayerHighlighter(ij).gameObject.SetActive(false);
        iTween.Stop(GetCurrentSideShowHighlighter(ij).gameObject);
        GetCurrentSideShowHighlighter(ij).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        if (SideShowCounter)
        {
            AutoStopSideShow();
        }
    }
    private IEnumerator CurrentSideShow()
    {
        _tempCurrentValueTime = CurrentSideShowValueTime;
        _tempDoubleCurrentValueTime = DoubleCurrentSideShowValueTime;
        newSec = -1f;
        inBackground = false;
        AnimationCounter = false;
        SideShowCounter = false;
        ////Debug.Log("Start CurrentSideShow  -------------------------- getName---------------------------- " + currentPlayer.playerData.NamePlayer);

        for (var i = 0; i < 5; i++)
        {
            //GetAvatarFilled(i).color = Color.green;
            GetAvatarCirculParent(i).transform.gameObject.SetActive(true);
            //GetAvatarCircul(i).color = new Color32(255, 255, 255, 255);
            GetAvatarCirculLine(i).localScale = new Vector3(1f, 0f, 1f);
            GetAvatarCircul(i).Value = 0.48f;

            BelowAvatarCircul(i);

            GetCurrentPlayerHighlighter(i).gameObject.SetActive(true);
            GetMovingMoney(i).transform.position = GetMoney(i).transform.position;
        }

        float step = (float)25 / 100;

        RunTime();
        for (float i = 0; i <= 2; i += 0.033f)
        {
            for (int j = 0; j < 5; j++)
            {
                if (newSec > -1f && newSec <= 2 && inBackground)
                {
                    inBackground = false;
                    i = newSec;
                    //Debug.Log("i " + i + "newSec "+ newSec);
                }
                if (i >= 0.12f && i < 1f)
                {

                    GetCurrentPlayerHighlighter(j).gameObject.SetActive(false);
                }

                //GetAvatarTimeText(j).text = i.ToString();

                GetAvatarCirculLine(j).localScale = new Vector3(1f, i, 1f);


            }

            if (i >= 1.75f && !SideShowCounter)
            {
                SideShowCounter = true;

            }
            yield return new WaitForSeconds(step);
            ////Debug.Log("step  --------------------------" + step +" i---------------------------- "+ i + " getName---------------------------- " + currentPlayer.playerData.NamePlayer);
        }

        ////Debug.Log("countLoop  -------------------------- getName---------------------------- " + currentPlayer.playerData.NamePlayer);
        for (int i = 0; i < 5; i++)
        {
            GetAvatarCirculParent(i).transform.gameObject.SetActive(false);
            GetAvatarCircul(i).Value = 0.48f;
            iTween.Stop(GetCurrentSideShowHighlighter(i).gameObject);
            GetCurrentSideShowHighlighter(i).gameObject.SetActive(false);
            GetCurrentPlayerHighlighter(i).gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(1f);
        if (SideShowCounter)
        {
            AutoStopSideShow();
        }
    }
    public void AutoStopSideShow()
    {
        //Debug.Log("AutoStopSideShow");
        SideShowCounter = false;
        inBackground = false;
        
        int i = MyPositionID;
        GetAvatarCirculParent(i).transform.gameObject.SetActive(false);
        //GetAvatarCircul(i).color = new Color32(255, 255, 255, 255);
        BelowAvatarCircul(i);
        iTween.Stop(GetCurrentSideShowHighlighter(i).gameObject);
        GetCurrentSideShowHighlighter(i).gameObject.SetActive(false);
        GetCurrentPlayerHighlighter(i).gameObject.SetActive(false);
        GetCurrentSideShowHighlighter(i).gameObject.transform.position = _startSideShowPosition;
        if (currentPlayer)
        {
            if (!currentPlayer.playerData.IsPacked && currentPlayer.playerData.isSideShow && !currentPlayer.playerData.isSentSideShow && !currentPlayer.playerData.IsBot)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    //Debug.Log("AutoStopSideShow only call master ");
                   
                    currentPlayer.photonView.RPC("DeclineSideShow", RpcTarget.All);
                    currentPlayer.photonView.RPC("DeclineSideShowOnlyMaster", RpcTarget.MasterClient);
                }
                else
                {
                    //Debug.Log("AutoStopSideShow not any call master ");
                }
                if (currentPlayer.localPlayer != null)
                {
                    currentPlayer.localPlayer.PanelAcceptOff();
                }
            }
        }
    }



	private void AvatarCallBack()//IGraphResult result)
    {
        if (IsFull)
        {
            for (int i = 0; i < 5; i++)
            {
                //GetAvatarPlayer(i).sprite =
                //    Sprite.Create(result.Texture, new Rect(0, 0, 50, 50), new Vector2(0.5f, 0.5f));
                //GetAvatarFilled(i).sprite = GetAvatarPlayer(i).sprite;
            }
        }
    }
	public void Update()
    {
        if(GameActive)
        {
            if(currentPlayer == null)
            {
                ////Debug.Log("Clear UI ");
                GameActive = false;
                NewClearUI();
            }
        }
    }
    
    public void CallOnDestroy(PlayerData playerData,int fromWhere)
    {
        //Debug.Log("CallOnDestroy "+ playerData.NamePlayer + "fromWhere "+ fromWhere);
        //Debug.Log(currentPlayer == null ? "CallOnDestroy currentplayer is null" : "CallOnDestroy currentplayer is not null");
        if (currentPlayer != null)
        {
            //Debug.Log(currentPlayer.managerMain == null ? "CallOnDestroy currentplayer.managerMain is null" : "CallOnDestroy currentplayer.managerMain is not null");
            if (currentPlayer.managerMain != null)
            {
                //Debug.Log(currentPlayer.managerMain.photonView == null ? "CallOnDestroy currentplayer.managerMain.photonView is null" : "CallOnDestroy currentplayer.managerMain.photonView is not null");
                if (currentPlayer.managerMain.photonView != null)
                {
                    currentPlayer.managerMain.photonView.RPC("DestroyActivity", RpcTarget.MasterClient,currentPlayer);
                }
                else
                {
                    if (PhotonNetwork.InRoom)
                    {
                        PhotonNetwork.RaiseEvent((int)EnumPhoton.SendDestroyMessage, playerData.NamePlayer + ";" + playerData._DeviceID, new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache }, SendOptions.SendReliable);
                    }
                    else
                    {
                        //Debug.Log("CallOnDestroy not in room");
                    }
                }
            }
            else
            {
                if (PhotonNetwork.InRoom)
                {
                    PhotonNetwork.RaiseEvent((int)EnumPhoton.SendDestroyMessage, playerData.NamePlayer + ";" + playerData._DeviceID, new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache }, SendOptions.SendReliable);
                }
                else
                {
                    //Debug.Log("CallOnDestroy not in roomq");
                }
            }
            PhotonNetwork.SendAllOutgoingCommands();
          
        }
        else
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.RaiseEvent((int)EnumPhoton.SendDestroyMessage, playerData.NamePlayer + ";" + playerData._DeviceID, new RaiseEventOptions() { CachingOption = EventCaching.AddToRoomCache }, SendOptions.SendReliable);
            }
            else
            {
               //Debug.Log("CallOnDestroy not in roomww");
            }
            PhotonNetwork.SendAllOutgoingCommands();
        }
    }
    public void NewClearUI()
    {
       // //Debug.Log("hello  in NewClearUI");
        ClearUI(0);
        SetIsFull(false);
    }
   
   
    //private void AvatarCallBack(IGraphResult result)
    //{
    //    avatarPlayer.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 50, 50), new Vector2(0.5f, 0.5f));
    //    avatarFilled.sprite = avatarPlayer.sprite;
    //}

    public void ClearUI(int _where)
    {
        AnimationCounter = false;
        //Debug.Log("ClearUI ------<color=red>ShowDealerIcon :---------------------------------</color>Init dealer icon false  ");
        StopAllCoroutines();
        ClearCards();
        //movingMoney.gameObject.SetActive(false);
        //movingTipMoney.gameObject.SetActive(false);
        //closeMovingCard.gameObject.SetActive(false);
        //avatarCircul.enabled = false;
        //avatarCircul.fillAmount = 0f;
        //money.transform.parent.gameObject.SetActive(false);
       
        //avatarPlayer.enabled = false;
        //avatarCircul.enabled = false;
     
        //nameText.text = "";
        //money.text = "";
        //infoText.text = "";
        //currentPlayer = null;
        //avatarTimerText.text = "";
        
        //AnimationCounter = false;
       
       
        //if (SeeButton) SeeButton.gameObject.SetActive(false);

        for (int i = 0; i < 5; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
            GetMovingMoney(i).gameObject.SetActive(false);
            GetMovingMoney(i).transform.position = GetMoney(i).transform.position;
            GetCardClose(i).gameObject.SetActive(false);
            //GetAvatarFilled(i).enabled = false;
            GetMoney(i).transform.parent.gameObject.SetActive(false);
            GetAvatarPlayer(i).enabled = true;//CHANGED BY ME
            GetAvatarPlayer(i).sprite = defaultAvatar;
            GetWinnerBase(i).SetActive(false);
            GetAvatarCirculParent(i).transform.gameObject.SetActive(false);
            GetAvatarPlayer(i).color = Color.white;
            GetInfoNamePlayer(i).text = "";
            GetMoney(i).text = "";
            GetChaalBlindMoney(i).text = "0.00";
            GetChaalBlindMoney(i).transform.parent.gameObject.SetActive(false);
            GetInfoText(i).text = "";
            GetInfoText(i).transform.parent.gameObject.SetActive(false);

            GetAvatarTimeText(i).text = "";
            if (_where == 0)
            {
                currentPlayer = null;
            }
            GetSeenButton(i).gameObject.SetActive(false);
            GetDealerIcon(i).gameObject.SetActive(false);
            GetCurrentPlayerHighlighter(i).gameObject.SetActive(false);

            GetCurrentSideShowHighlighter(i).gameObject.SetActive(false);
           
        }
        if(MyPositionID>=0 && MyPositionID<5)
        {
            transform.GetChild(MyPositionID).gameObject.SetActive(true);
        }
        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.OnForceStopSound();
        }
    }
   
   
    public void PlayerEmpty()
    {
        //Debug.Log("<color=red>PlayerEmpty : ---------------------------------</color> dealer icon false  ");
        for (int i = 0; i < 5; i++)
        {
            myCanvasGroup.alpha = 0.4f;
            GetMoney(i).text = "";
            GetInfoText(i).text = "";
            GetInfoText(i).transform.parent.gameObject.SetActive(false);
            GetChaalBlindMoney(i).text = "0.00";
            GetChaalBlindMoney(i).transform.parent.gameObject.SetActive(false);
            GetSeenButton(i).gameObject.SetActive(false);
            GetDealerIcon(i).gameObject.SetActive(false);
            GetCurrentPlayerHighlighter(i).gameObject.SetActive(false);
           
            GetCurrentSideShowHighlighter(i).gameObject.SetActive(false);
        }
        //if (SeeButton) SeeButton.gameObject.SetActive(false);
        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.OnForceStopSound();
        }
    }
   
    public void ReloadUI()
    {
        AnimationCounter = false;
        ////Debug.Log("Reload ui ");
        ////Debug.Log("Reload UI <color=red>ShowDealerIcon : {0}---------------------------------</color> dealer icon false  ");
        for (int i = 0; i < 5; i++)
        {
            GetInfoText(i).text = "";
            GetInfoText(i).transform.parent.gameObject.SetActive(false);
            GetAvatarTimeText(i).text = "";
            GetWinnerBase(i).SetActive(false);
            GetAvatarCirculParent(i).transform.gameObject.SetActive(false);
            SetCardCombinationText(eCombination.Empty);
            GetAvatarPlayer(i).color = Color.white;
            GetSeenButton(i).gameObject.SetActive(false);
            if (currentPlayer != null)
            {
                GetMoney(i).text = currentPlayer.playerData.Money.ToString("F2");
            }
            else
            {
                GetMoney(i).text = "";
            }
            
            for (int j = 0; j < 3; j++)
            {
                GetCards(i)[j].color = Color.white;
                GetCards(i)[j].enabled = false;
                GetBiggerCards(i)[j].color = Color.white;
                GetBiggerCards(i)[j].enabled = false;
            }
           
            GetDealerIcon(i).gameObject.SetActive(false);
            GetCurrentPlayerHighlighter(i).gameObject.SetActive(false);
            GetCurrentSideShowHighlighter(i).gameObject.SetActive(false);
            GetChaalBlindMoney(i).text = "0.00";
            //GetAB_A_Text(i).text = string.Empty;
            //GetAB_B_Text(i).text = string.Empty;
        }
        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.OnForceStopSound();
        }
        
    }
    public void RefreshCoinsValue()
    {
        for (int i = 0; i < 5; i++)
        {
            if (currentPlayer != null)
            {
                GetMoney(i).text = currentPlayer.playerData.Money.ToString("F2");
            }
        }
    }
    public void SetPacked()
    {
        AnimationCounter = false;
        for (int i = 0; i < 5; i++)
        {
            GetInfoText(i).text = "Packed";
            GetInfoText(i).transform.parent.gameObject.SetActive(true);
            GetAvatarPlayer(i).color = colorPaked;
            GetWinnerBase(i).SetActive(false);
            GetAvatarCirculParent(i).transform.gameObject.SetActive(false);
            GetCurrentSideShowHighlighter(i).gameObject.SetActive(false);
            GetCurrentPlayerHighlighter(i).gameObject.SetActive(false);
            GetAvatarTimeText(i).text = "";
            foreach (var item in GetCards(i))
            {
                item.color = colorPaked;
            }
            foreach (var item in GetBiggerCards(i))
            {
                item.color = colorPaked;
            }
            GetSeenButton(i).gameObject.SetActive(false);
            // StartCoroutine("PackedCards");      
            SetCardCombinationText(eCombination.Empty);
        }

        StopStep();
       
        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.OnForceStopSound();
        }
    }
    public void SetWinText()
    {


        AnimationCounter = false;

        //avatarTimerText.text = "";
        //avatarCircul.enabled = false;
        for (int i = 0; i < 5; i++)
        {
            GetAvatarTimeText(i).text = "";
            GetAvatarCirculParent(i).transform.gameObject.SetActive(false);
            GetCurrentSideShowHighlighter(i).gameObject.SetActive(false);
            GetCurrentPlayerHighlighter(i).gameObject.SetActive(false);
            GetSeenButton(i).gameObject.SetActive(false);
            GetInfoText(i).text = "Win";
            GetInfoText(i).transform.parent.gameObject.SetActive(false);
            GetWinnerBase(i).SetActive(true);
            GetWinnerBaseAnim(i).Play("Winner");
        }
        //avatarCircul.fillAmount = 0f;
        //avatarCircul.enabled = false;
        //if (SeeButton) SeeButton.gameObject.SetActive(false);
        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.OnForceStopSound();
        }
    }

    public void SetInfoBlind(bool isSeen)
    {
        if (isSeen)
        {

            for (int i = 0; i < 5; i++)
            {
                GetInfoText(i).text = "Seen";
                GetInfoText(i).transform.parent.gameObject.SetActive(true);

                if (currentPlayer != null)
                {
                    if (currentPlayer.playerData.IsSeenCard)
                    {
                        GetInfoText(i).text = "Seen";
                    }
                    if (currentPlayer.playerData.IsPacked)
                    {
                        GetInfoText(i).text = "Packed";
                    }
                }
            }

            if (currentPlayer != null)
            {
                if (currentPlayer.photonView != null)
                {
                    if (currentPlayer.photonView.IsMine)
                    {
                        if (!currentPlayer.playerData.IsPacked)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                GetSeenButton(i).gameObject.SetActive(false);
                                //GetInfoText(i).text = "";
                                //GetInfoText(i).transform.parent.gameObject.SetActive(false);
                            }
						}
						else
						{
                            for (int i = 0; i < 5; i++)
                            {
                                GetSeenButton(i).gameObject.SetActive(false);
                                //GetInfoText(i).text = "";
                                //GetInfoText(i).transform.parent.gameObject.SetActive(false);
                            }
                        }
                       
                        

                    }
                }
            }
        }
        else
        {


            for (int i = 0; i < 5; i++)
            {
                GetInfoText(i).text = "Blind";
                GetInfoText(i).transform.parent.gameObject.SetActive(true);
                if(currentPlayer!=null)
                {
                    if(currentPlayer.playerData.IsSeenCard)
                    {
                        GetInfoText(i).text = "Seen";
                    }
                    if(currentPlayer.playerData.IsPacked)
                    {
                        GetInfoText(i).text = "Packed";
                    }
                }

            }

            if (currentPlayer != null)
            {
                if (currentPlayer.photonView != null)
                {
                    if (currentPlayer.photonView.IsMine)
                    {
                        if (!currentPlayer.playerData.IsPacked)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                if (!currentPlayer.playerData.IsBot)
                                {
                                    if (currentPlayer.playerData.playerType == ePlayerType.PlayerStartGame)
                                    {
                                        //Debug.Log("GetSeen button");
                                        if (currentPlayer.playerData.currentCombination != eCombination.Empty)
                                        {
                                            GetSeenButton(i).gameObject.SetActive(true);
                                        }
                                        else
                                        {
                                            GetSeenButton(i).gameObject.SetActive(false);
                                        }
                                    }
                                    else
                                    {
                                        GetSeenButton(i).gameObject.SetActive(false);
                                    }
                                }
                                else
                                {
                                    GetSeenButton(i).gameObject.SetActive(false);
                                }
                                //GetInfoText(i).text = "";
                                if (currentPlayer.playerData.currentCombination != eCombination.Empty)
                                {
                                    GetInfoText(i).text = "Blind";
                                    GetInfoText(i).transform.parent.gameObject.SetActive(true);
                                }
                                ////Debug.Log("SetnofBlind " + currentPlayer.playerData.IsBot);
                            }


                            
						}
						else
						{
                            for (int i = 0; i < 5; i++)
                            {
                                GetSeenButton(i).gameObject.SetActive(false);
                                //GetInfoText(i).text = "";
                                GetInfoText(i).text = "Packed";
                                GetInfoText(i).transform.parent.gameObject.SetActive(true);
                            }
                           
						}
                    }
                }
            }
        }
    }

    public void SetInfoEmpty()
    {

        //Debug.Log("info empty");
        for (int i = 0; i < 5; i++)
        {
            GetInfoText(i).text = "";
            GetInfoText(i).transform.parent.gameObject.SetActive(false);
            GetSeenButton(i).gameObject.SetActive(false);
        }
      
       
       
        
    }

   
    public void SetCardCombinationText(eCombination _combination)
    {
        if (_combination != eCombination.Empty)
        {
            for (int i = 0; i < 5; i++)
            {
                //GetCardCombination(i).text = _combination == eCombination.StraightFlush
                //    ? "STRAIGHT FLUSH"
                //    : _combination.ToString().ToUpper();
                GetCardCombination(i).text = "";
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                GetCardCombination(i).text = "";
            }
        }
    }
    public void GiveTextMoney(double currentMoney, double giveMovey)
    {
        //money.text = giveMovey.ToString();
        ////Debug.Log("MyPositionID " + MyPositionID + "currentMoney "+ currentMoney);


        if (giveMovey != 0)
        {
            for (int i = 0; i < 5; i++)
            {
                if (MyPositionID == 2)
                {
                    GetMoney(i).text = currentMoney.ToString("F2");//currentMoney
                    GetChaalBlindMoney(i).transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    GetMoney(i).text = currentMoney.ToString("F2");//currentMoney
                    GetChaalBlindMoney(i).text = giveMovey.ToString("F2");//currentMovey
                    GetChaalBlindMoney(i).transform.parent.gameObject.SetActive(true);
                }

            }

            for (int i = 0; i < 5; i++)
            {
                GetMovingMoney(i).gameObject.SetActive(true);
                GetTextMovingMoney(i).text = giveMovey.ToString("F2");
                GetMovingMoney(i).gameObject.SetActive(true);
                GetMovingMoney(i).transform.position = GetMoney(i).rectTransform.position;
            }
            ////Debug.Log("second  inner give money " + giveMovey);
            StartCoroutine("MovingGiveMoney");
        }   
        else
        {
            ////Debug.Log("second  outer give money " + giveMovey);
            for (int i = 0; i < 5; i++)
            {
                if (MyPositionID == 2)
                {
                    GetMoney(i).text = currentMoney.ToString("F2");//currentMoney
                    
                }
                else
                {
                    GetMoney(i).text = currentMoney.ToString("F2");//currentMoney
                    
                }

            }
        }
    }
    public void GiveTipMoney(double tipMoney)
    {
        for (int i = 0; i < 5; i++)
        {
            GetMoney(i).text = tipMoney.ToString("F2");
        }
        if (tipMoney != 0)
        {
            for (int i = 0; i < 5; i++)
            {
                GetMovingMoney(i).gameObject.SetActive(true);
                GetTextMovingMoney(i).text = tipMoney.ToString();
            }
            StartCoroutine("MovingTipMoney");
        }
    }

    
    public void WinTextMoney(double currentMoney, double giveMovey)
    {
        for (int i = 0; i < 5; i++)
        {
            GetMovingMoney(i).gameObject.SetActive(true);
            GetTextMovingMoney(i).text = giveMovey.ToString("F2");
        }
        if (giveMovey != 0)
        {
            StartCoroutine("MovingWinMoney", currentMoney);
        }
    }
    public void GiveTextMoney(eTable tableType, double currentMoney, double giveMoney, int result = 0, double newValue = 0)
    {
        ////Debug.Log("giveMoney " + giveMoney);
        ////Debug.Log("GiveTextMoney MyPositionID " + MyPositionID + "currentMoney " + currentMoney);
        if (giveMoney != 0)
        {
            if (tableType != eTable.AndarBahar)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (MyPositionID == 2)
                    {
                        GetMoney(i).text = currentMoney.ToString("F2");// currentMoney.ToString("F2");
                        GetChaalBlindMoney(i).transform.parent.gameObject.SetActive(false);
                    }
                    else
                    {
                        GetMoney(i).text = currentMoney.ToString("F2");// currentMoney.ToString("F2");
                        GetChaalBlindMoney(i).text = giveMoney.ToString("F2");// currentMoney.ToString("F2");
                        GetChaalBlindMoney(i).transform.parent.gameObject.SetActive(true);
                    }
                }

                for (int i = 0; i < 5; i++)
                {
                    GetMovingMoney(i).gameObject.SetActive(true);
                    GetTextMovingMoney(i).text = giveMoney.ToString("F2");
                }
                ////Debug.Log("giveMoney inner block " + giveMoney);
                StartCoroutine("MovingGiveMoney");
            }
            else
            {
                ////Debug.Log("Start GiveTextMoney");
                //for (int i = 0; i < 5; i++)
                //{
                    //if (result > 0) GetAB_A_Text(i).text = newValue.ToString();
                   // else if (result < 0) GetAB_B_Text(i).text = newValue.ToString();
               // }
                ////Debug.Log("End GiveTextMoney");
            }
        }
        else
        {
            ////Debug.Log("giveMoney outer block" + giveMoney);
        }
    }
    public void SetInitCard(CardData card, int numCard, bool staticCard)
    {
        
            for (int i = 0; i < 5; i++)
            {
                GetCards(i)[numCard].sprite = cardAtlas.GetSprite("Back");
                GetBiggerCards(i)[numCard].sprite = cardAtlas.GetSprite("Back");
                if (!staticCard)
                {
                    GetCards(i)[numCard].enabled = true;
                    GetCards(i)[numCard].color = colorPaked;
                }
                else
                {
                    GetCards(i)[numCard].enabled = true;
                }
            }
    }
    public void SetCard(CardData card, int numCard, bool staticCard)
    {
        if (card.isClose)
        {
            for (int i = 0; i < 5; i++)
            {
                GetCards(i)[numCard].sprite = cardAtlas.GetSprite("Back");
                GetBiggerCards(i)[numCard].sprite = cardAtlas.GetSprite("Back");
            }

            if (!staticCard)
            {
                StartCoroutine("MovingCloseCardToPlayer", numCard);
                
            }
        }
        else
        {
            if (card.rankCard == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    GetCards(i)[numCard].sprite = cardAtlas.GetSprite("Back");
                    GetBiggerCards(i)[numCard].sprite = cardAtlas.GetSprite("Back");
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (card.suitCard == eCardSuit.Joker && card.originalSuitCard != eCardSuit.Joker)
                    {
                        GetCards(i)[numCard].sprite =cardAtlas.GetSprite(card.originalSuitCard.ToString() + card.originalRankCard);
                        GetBiggerCards(i)[numCard].sprite = cardAtlas.GetSprite(card.originalSuitCard.ToString() + card.originalRankCard);
                        this.card = card;
                        this.numCard = numCard;
                        Invoke("TransformJoker", 1.0f);
                    }
                    else
                    {
                        GetCards(i)[numCard].sprite = cardAtlas.GetSprite(card.suitCard.ToString() + card.rankCard);
                        GetCards(i)[numCard].enabled = false;
                        GetBiggerCards(i)[numCard].sprite = cardAtlas.GetSprite(card.suitCard.ToString() + card.rankCard);
                        GetBiggerCards(i)[numCard].enabled = true;
                    }
                }
            }
        }
    }
    private CardData card;
    private int numCard;
    public void TransformJoker()
    {
        for (int i = 0; i < 5; i++)
        {
            GetCards(i)[numCard].sprite =
                cardAtlas.GetSprite(card.suitCard.ToString() + card.rankCard);
            GetBiggerCards(i)[numCard].sprite =
                cardAtlas.GetSprite(card.suitCard.ToString() + card.rankCard);
        }
    }
    public void SetCurrentStep()
    {
        newSec = -1f;
        AnimationCounter = false;
        SideShowCounter = false;
        inBackground = false;
        ////Debug.Log("SetCurrentStep " + currentPlayer.GetNameUI());
        StopStep();
        StopCoroutine("CurrentStep");
        StartCoroutine("CurrentStep");
        
    }

    public void ForceStopStep()
    {
        AnimationCounter = false;
        StopCoroutine("CurrentStep");
        inBackground = false;
        for (var i = 0; i < 5; i++)
        {
            GetAvatarCirculParent(i).transform.gameObject.SetActive(false);
            GetAvatarTimeText(i).text = "";
            GetCurrentPlayerHighlighter(i).gameObject.SetActive(false);
            GetCurrentSideShowHighlighter(i).gameObject.SetActive(false);
        }
        
    }
    public void StopStep()
    {
        AnimationCounter = false;
        StopCoroutine("CurrentStep");
        inBackground = false;
        for (var i = 0; i < 5; i++)
        {
            GetAvatarCirculParent(i).transform.gameObject.SetActive(false);
            GetAvatarTimeText(i).text = "";
            GetCurrentPlayerHighlighter(i).gameObject.SetActive(false);
            GetCurrentSideShowHighlighter(i).gameObject.SetActive(false);
        }
        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.OnForceStopSound();
        }
        _CurrentTimer = 0f;
        if (currentPlayer != null)
        {
            currentPlayer.playerData._CurrentTimer = _CurrentTimer;
        }
    }

    void DeactiveButtons()
    {
        inBackground = false;
        if (currentPlayer != null)
        {
            if (currentPlayer.localPlayer != null)
            {
                currentPlayer.localPlayer.DeactivateAllButtons();
            }
        }
    }
    private void AutoStopStep()
    {
        inBackground = false;
        AnimationCounter = false;
        if (currentPlayer.managerMain.typeTable == eTable.AndarBahar)
        {
            currentPlayer.photonView.RPC("FinishedTurn", RpcTarget.All,
                0, 0, currentPlayer.playerData.AndarBet, currentPlayer.playerData.BaharBet);
        }
        else
        {
            if (currentPlayer)
            {
                if (!currentPlayer.playerData.IsPacked)
                {
                    currentPlayer.photonView.RPC("PackPlayer", RpcTarget.All);
                }
            }
            TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
            if (hud)
            {
                hud.OnForceStopSound();
            }
        }
        _CurrentTimer = 0f;
        if (currentPlayer != null)
        {
            currentPlayer.playerData._CurrentTimer = _CurrentTimer;
        }
    }
    
    
    private IEnumerator PackedCards()
    {
        for (int i = 0; i < 5; i++)
        {
            RectTransform parentCards = GetCards(i)[0].transform.parent.GetComponent<RectTransform>();
            Vector3 startPos = parentCards.localPosition;
            for (float distance = Vector3.Distance(parentCards.position, finishMoneyPosition.position); distance >= 2;)
            {
                parentCards.position = Vector3.MoveTowards(parentCards.position, finishMoneyPosition.position, 12);
                parentCards.Rotate(0, 0, -8);
                distance = Vector3.Distance(parentCards.position, finishMoneyPosition.position);
                yield return new WaitForFixedUpdate();
            }

            parentCards.localPosition = startPos;
            parentCards.localEulerAngles = Vector3.zero;
            for (int j = 0; j < 5; j++)
            {
                GetCards(j)[i].enabled = false;
                GetBiggerCards(j)[i].enabled = false;
            }
        }
    }
   
    private IEnumerator MovingWinMoney(double currentMoney)
    {
        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.OnGiveCoinsSound();
        }
        yield return new WaitForSeconds(0.5f);
        int i = MyPositionID;
        if(i>=0 && i<5)//for (int i = 0; i < 5; i++)
        {
            GetMovingMoney(i).gameObject.SetActive(true);
            GetMovingMoney(i).transform.position = finishMoneyPosition.position;

            //for (float distance = Vector3.Distance(GetMovingMoney(i).transform.position, GetMoney(i).transform.position); distance >= 2;)
            //{
            //    //for (int j = 0; j < 5; j++)
            //    //{
            //        GetMovingMoney(i).transform.position =
            //            Vector3.MoveTowards(GetMovingMoney(i).transform.position, GetMoney(i).transform.position, 15);
            //    //}

            //    distance = Vector3.Distance(GetMovingMoney(i).transform.position, GetMoney(i).transform.position);
            //    yield return new WaitForFixedUpdate();
            //}
            iTween.MoveTo(GetMovingMoney(i).gameObject, iTween.Hash("position", GetMoney(i).transform.position, "time", 0.8f, "easeType", iTween.EaseType.linear, "oncomplete", "MovingWinMoneyComplete", "oncompletetarget", gameObject, "oncompleteparams", currentMoney));
            
            //GetMovingMoney(i).gameObject.SetActive(false);
        }
    }
    
    private IEnumerator MovingGiveMoney()
    {
        TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
        if (hud)
        {
            hud.OnGiveCoinsSound();
        }
        int i = MyPositionID;
        ////Debug.Log("this.gameObject.name " + this.gameObject.name + " player name  " + currentPlayer.playerData.NamePlayer + "MyPositionID "+ MyPositionID);

        yield return new WaitForSeconds(0f);
        if (i>=0 && i<5)//for (int i = 0; i < 5; i++)
        {
            GetMovingMoney(i).gameObject.SetActive(true);
            
            GetMovingMoney(i).transform.position = GetMoney(i).transform.position;
            ////Debug.Log("distance " + i+" " + Vector3.Distance(GetMovingMoney(i).position, finishMoneyPosition.position) + GetMovingMoney(i).position);
            //for (float distance = Vector3.Distance(GetMovingMoney(i).transform.position, finishMoneyPosition.position);
            //    distance >= 2;)
            //{
            //    //for (int j = 0; j < 5; j++)
            //    //{
                   
            //    ////Debug.Log("distance " + i + " " + Vector3.Distance(GetMovingMoney(i).position, finishMoneyPosition.position) + GetMovingMoney(i).position +" "+finishMoneyPosition.position);
            //    GetMovingMoney(i).transform.position = Vector3.MoveTowards(GetMovingMoney(i).transform.position, finishMoneyPosition.position, 5);
            //        distance = Vector3.Distance(GetMovingMoney(i).transform.position, finishMoneyPosition.position);
            //    //}

            //    yield return new WaitForSeconds(0.01f);
            //}

            //iTween.MoveFrom(GetMovingMoney(i).gameObject,finishMoneyPosition.position,)
            iTween.MoveTo(GetMovingMoney(i).gameObject, iTween.Hash("position", finishMoneyPosition.position, "time", 0.5f, "easeType", iTween.EaseType.linear, "oncomplete", "MovingGiveMoneyComplete", "oncompletetarget", gameObject));
            //GetMovingMoney(i).gameObject.SetActive(false);
            
            //GetMovingMoney(i).position = GetMoney(i).transform.position;
        }
    }
    void MovingGiveMoneyComplete()
    {
        ////Debug.Log("MovingGiveMoneyComplete " + this.gameObject.name + " player name  " + currentPlayer.playerData.NamePlayer + "MyPositionID " + MyPositionID);
        for (int i = 0; i < 5; i++)
        {
            GetMovingMoney(i).gameObject.SetActive(false);
        }
    }
    void MovingWinMoneyComplete(double currentMoney)
    {
        ////Debug.Log("MovingGiveMoneyComplete " + this.gameObject.name + " player name  " + currentPlayer.playerData.NamePlayer + "MyPositionID " + MyPositionID);
        for (int i = 0; i < 5; i++)
        {
            GetMoney(i).text = currentMoney.ToString("F2");
            GetMovingMoney(i).gameObject.SetActive(false);
        }
    }

    private IEnumerator CurrentStep()
    {
        _tempCurrentValueTime = CurrentStepValueTime;
        _tempDoubleCurrentValueTime = DoubleCurrentStepValueTime;

        newSec = -1f;
        inBackground = false;
        AnimationCounter = false;
        _CurrentTimer = 0f;
        ////Debug.Log("Start CurrentStep  -------------------------- getName---------------------------- " + currentPlayer.playerData.NamePlayer);
        //if (!GetAvatarPlayer(MyPositionID).enabled)
        //{
        //    ClearUI();
        //}
        for (var i = 0; i < 5; i++)
        {
            //GetAvatarFilled(i).color = Color.green;
            GetAvatarCirculParent(i).transform.gameObject.SetActive(true);
            //GetAvatarCircul(i).color = new Color32(255, 255, 255, 255);
            GetAvatarCirculLine(i).localScale = new Vector3(1f, 0f, 1f);
            GetAvatarCircul(i).Value = 0.48f;

            BelowAvatarCircul(i);
            GetCurrentSideShowHighlighter(i).gameObject.SetActive(false);
            GetCurrentPlayerHighlighter(i).gameObject.SetActive(true);
            GetMovingMoney(i).transform.position = GetMoney(i).transform.position;
            if(currentPlayer!=null)
            {
                currentPlayer.playerData._CurrentTimer = _CurrentTimer;
            }
        }

        float step = (float)25 / 100;
     
        RunTime();
        for (float i = 0; i <= 2; i += 0.02f)
        {
           for (int j = 0; j < 5; j++)
           {
                if (newSec > -1f && newSec <= 2 && inBackground)
                {
                    inBackground = false;
                    i = newSec;
                   
                    //Debug.Log("i................. " + i + "newSec "+ newSec);
                }
                if (i >= 0.12f && i < 1f)
                {
                  
                   GetCurrentPlayerHighlighter(j).gameObject.SetActive(false);
                }
                _CurrentTimer = i;
                if (currentPlayer != null)
                {
                    currentPlayer.playerData._CurrentTimer = _CurrentTimer;
                    if (currentPlayer.managerMain != null)
                    {

                        currentPlayer.managerMain.managerInfo.IsCurrentTimer = _CurrentTimer;
                    }
                }
                //GetAvatarTimeText(j).text = i.ToString();

                GetAvatarCirculLine(j).localScale = new Vector3(1f, i, 1f);
              

            }

            if (i >= 1.75f && !AnimationCounter)
            {
                AnimationCounter = true;

            }
            yield return new WaitForSeconds(step);
            //Debug.Log("step  --------------------------" + step +" i---------------------------- "+ i + " getName---------------------------- " + currentPlayer.playerData.NamePlayer);
       }
       
        ////Debug.Log("countLoop  -------------------------- getName---------------------------- " + currentPlayer.playerData.NamePlayer);
        for (int i = 0; i < 5; i++)
        {
            GetAvatarCirculParent(i).transform.gameObject.SetActive(false);
            GetAvatarCircul(i).Value = 0.48f;
           
            GetCurrentPlayerHighlighter(i).gameObject.SetActive(false);
            GetCurrentSideShowHighlighter(i).gameObject.SetActive(false);
            _CurrentTimer = 0f;
            if (currentPlayer != null)
            {
                currentPlayer.playerData._CurrentTimer = _CurrentTimer;
            }
        }
        yield return new WaitForSeconds(1f);
        if (AnimationCounter)
        {
            AutoStopStep();
            _CurrentTimer = 0f;
            if (currentPlayer != null)
            {
                currentPlayer.playerData._CurrentTimer = _CurrentTimer;
            }
        }
    }
    
    private void BelowAvatarCircul(int j)
    {
        //float ratio = 0;
        //GetAvatarCircul(j).rectTransform.localPosition = new Vector3(0, GetAvatarCircul(j).rectTransform.rect.height * ratio - GetAvatarCircul(j).rectTransform.rect.height, 0);
        GetAvatarCircul(j).Value = 0.48f;

    }
    private void UpdateAvatarCircul(int j)
    {
        //float ratio = manaPoint / maxManaPoint;
        //GetAvatarCircul(j).rectTransform.localPosition = new Vector3(0, GetAvatarCircul(j).rectTransform.rect.height * ratio - GetAvatarCircul(j).rectTransform.rect.height, 0);
        
    }
    public Image[] GetCards(int p_index)
    {
        if (cards == null) return null;
        if (cards.Length <= p_index) return null;
        return cards[p_index];
    }

    public Image[] GetBiggerCards(int p_index)
    {
        if (BiggerCards == null) return null;
        if (BiggerCards.Length <= p_index) return null;
        return BiggerCards[p_index];
    }


    private IEnumerator MovingCloseCardToPlayer(int _numCard)
    {
        int i = MyPositionID;
        yield return new WaitForSeconds(0f);
        ////Debug.Log(" MovingCloseCardToPlayer -------------- his.gameObject.name " + this.gameObject.name + " player name  " + currentPlayer.playerData.NamePlayer + "MyPositionID " + MyPositionID);
        if (i >= 0 && i < 5)//for (int i = 0; i < 5; i++)
        {
            //for (int i = 0; i < 5; i++)
        //{
            GetCardClose(i).gameObject.SetActive(true);
            GetCardClose(i).position = startCardPositionMove.position;
            GetCardClose(i).eulerAngles = GetCards(i)[_numCard].transform.eulerAngles;

            ////Debug.Log("i " + i +"__"+ currentPlayer.GetNameUI()+ " "+ Vector3.Distance(GetCardClose(i).position, transform.position));
            //for (float distance = Vector3.Distance(GetCardClose(i).position, GetCards(i)[numCard].transform.position); distance >= 2;)
            //{
            //    GetCardClose(i).position =Vector3.MoveTowards(GetCardClose(i).position, GetCards(i)[numCard].transform.position, 50);
            //    distance = Vector3.Distance(GetCardClose(i).position, GetCards(i)[numCard].transform.position);

            //    yield return new WaitForFixedUpdate();
            //}
            //iTween.Stop(GetCardClose(i).gameObject);//if player disconnect when card is distributed then first card or last card is not show to user
            iTween.MoveTo(GetCardClose(i).gameObject, iTween.Hash("position", GetCards(i)[_numCard].transform.position, "time", 0.1f, "easeType", iTween.EaseType.linear, "oncomplete", "MovingCloseCardToPlayerComplete", "oncompletetarget", gameObject, "oncompleteparams", _numCard));
            //GetCardClose(i).gameObject.SetActive(false);
            //GetCards(i)[numCard].enabled = true;
            //GetBiggerCards(i)[numCard].enabled = false;
        }
    }
    public void SetCardsAgain()
    {
        for (int i = 0; i < 3; i++)
        {
            MovingCloseCardToPlayerComplete(i);
        }
    }
   public void MovingCloseCardToPlayerComplete(int _numCard)
   {
        //Debug.Log("MovingCloseCardToPlayerComplete "+ _numCard);
        for(int i=0;i<5;i++)
        {
            GetCardClose(i).gameObject.SetActive(false);
            GetCards(i)[_numCard].enabled = true;
            GetBiggerCards(i)[_numCard].enabled = false;
        }
        
    }
    void RunTime()
    {
        if (currentPlayer)
        {
            if (currentPlayer.localPlayer)
            {
                TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
                if (hud)
                {
                    hud.OnRunTimerSound();
                }
            }
        }
    }
    void StopTime()
    {
        if (currentPlayer)
        {
            if (currentPlayer.localPlayer)
            {
                TeenPatiHUD hud = FindObjectOfType<TeenPatiHUD>();
                if (hud)
                {
                    hud.OnStopTimerSound();
                }
            }
        }
    }
    
   
    public void EndAndarBaharGame(int result)
    {
//        for (int j = 0; j < 5; j++)
//        {
//            GetAB_A_Text(j).text = string.Empty;
//            GetAB_B_Text(j).text = string.Empty;
//        }
    }
    public void SideShowAnimationReverse(Vector3 startPosition,float[] newValues)
    {
        GetCurrentSideShowHighlighter(MyPositionID).gameObject.transform.position = _startSideShowPosition;
        GetCurrentSideShowHighlighter(MyPositionID).gameObject.SetActive(true);
      
        GetCurrentSideShowHighlighter(MyPositionID).gameObject.GetComponent<Image>().enabled = false;

        GetCurrentSideShowHighlighterInner1(MyPositionID).gameObject.GetComponent<Image>().enabled = false;
        GetCurrentSideShowHighlighterInner2(MyPositionID).gameObject.GetComponent<Image>().enabled = false;
        GetCurrentSideShowHighlighterInner3(MyPositionID).gameObject.GetComponent<Image>().enabled = false;

        iTween.Stop(GetCurrentSideShowHighlighter(MyPositionID).gameObject);
        //Debug.Log("startPosition " + startPosition + "MyPositionID "+ MyPositionID + "GetCurrentSideShowHighlighter(MyPositionID).anchoredPosition "+ GetCurrentSideShowHighlighter(MyPositionID).gameObject.transform.position);
        iTween.ValueTo(GetCurrentSideShowHighlighter(MyPositionID).gameObject, iTween.Hash(
        "from", startPosition,
        "to", GetCurrentSideShowHighlighter(MyPositionID).gameObject.transform.position,
        "time", 1f,
        "delay", 1f,
        "looptype", iTween.LoopType.loop,
        "easeType", iTween.EaseType.linear,
        "onupdatetarget", this.gameObject,
        "onupdate", "MoveSideShowTitle",
         "onstart", "StartSideShowTitle",
        "onstarttarget", this.gameObject,
        "onstartparams", newValues,
        "oncompletetarget", this.gameObject, "oncomplete", "GoBackToOriginal", "oncompleteparams", newValues)) ;//add id for reference

        
    }
    public void SideShowAnimation(Vector2 targetPosition)
    {
        GetCurrentSideShowHighlighter(MyPositionID).gameObject.SetActive(true);
        iTween.ValueTo(GetCurrentSideShowHighlighter(MyPositionID).gameObject, iTween.Hash(
        "from", GetCurrentSideShowHighlighter(MyPositionID).anchoredPosition,
        "to", targetPosition,
        "speed", 100f,
        "delay", 1f,
        "onupdatetarget", this.gameObject,
        "onupdate", "MoveSideShowTitle"));
    }
    public void MoveSideShowTitle(Vector3 Tposition)
    {
       
        GetCurrentSideShowHighlighter(MyPositionID).gameObject.SetActive(true);
        GetCurrentSideShowHighlighter(MyPositionID).gameObject.transform.position = Tposition;
    }
    public void StartSideShowTitle(float[] newValues)
    {
        //Debug.Log("StartShow");
        GetCurrentSideShowHighlighter(MyPositionID).gameObject.SetActive(true);
        GetCurrentSideShowHighlighter(MyPositionID).gameObject.GetComponent<Image>().enabled = true;
        GetCurrentSideShowHighlighter(MyPositionID).gameObject.transform.position = new Vector3(newValues[3], newValues[4], newValues[5]);
        GetCurrentSideShowHighlighterInner1(MyPositionID).gameObject.GetComponent<Image>().enabled = true;
        GetCurrentSideShowHighlighterInner2(MyPositionID).gameObject.GetComponent<Image>().enabled = true;
        GetCurrentSideShowHighlighterInner3(MyPositionID).gameObject.GetComponent<Image>().enabled = true;
        GetCurrentSideShowHighlighterInner1(MyPositionID).gameObject.transform.localPosition = new Vector3(newValues[2] * -40f, newValues[1] * -40f, 0f);
        GetCurrentSideShowHighlighterInner1(MyPositionID).gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        GetCurrentSideShowHighlighterInner2(MyPositionID).gameObject.transform.localPosition = new Vector3(newValues[2] * -80f, newValues[1] * -80f, 0f);
        GetCurrentSideShowHighlighterInner2(MyPositionID).gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        GetCurrentSideShowHighlighterInner3(MyPositionID).gameObject.transform.localPosition = new Vector3(newValues[2] * -120f, newValues[1] * -120f, 0f);
        GetCurrentSideShowHighlighterInner3(MyPositionID).gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }
    private void GoBackToOriginal(float[] newValues)
    {

        //Debug.Log("GoBackToOriginal  ");//here we have to do work
        GetCurrentSideShowHighlighter(MyPositionID).gameObject.GetComponent<Image>().enabled = false;
      
        iTween.MoveTo(GetCurrentSideShowHighlighterInner1(MyPositionID).gameObject, iTween.Hash("position", Vector3.zero, "time", 0.20f, "delay", 0f, "looptype", iTween.LoopType.none, "easeType", iTween.EaseType.linear, "islocal", true, "movetopath", false, "onstart", "StartInner1", "onstarttarget", this.gameObject, "oncomplete", "MovingInnerComplete1", "oncompletetarget", gameObject, "oncompleteparams", newValues));
        iTween.MoveTo(GetCurrentSideShowHighlighterInner2(MyPositionID).gameObject, iTween.Hash("position", Vector3.zero, "time", 0.21f, "delay", 0f, "looptype", iTween.LoopType.none, "easeType", iTween.EaseType.linear, "islocal", true, "movetopath", false, "onstart", "StartInner2", "onstarttarget", this.gameObject, "oncomplete", "MovingInnerComplete2", "oncompletetarget", gameObject, "oncompleteparams", newValues));
        iTween.MoveTo(GetCurrentSideShowHighlighterInner3(MyPositionID).gameObject, iTween.Hash("position", Vector3.zero, "time", 0.22f, "delay", 0f, "looptype", iTween.LoopType.none, "easeType", iTween.EaseType.linear, "islocal", true, "movetopath", false, "onstart", "StartInner3", "onstarttarget", this.gameObject, "oncomplete", "MovingInnerComplete3", "oncompletetarget", gameObject, "oncompleteparams", newValues));

    }
    public void StartInner1()
    {
        GetCurrentSideShowHighlighterInner1(MyPositionID).gameObject.GetComponent<Image>().enabled = true;
    }
    public void StartInner2()
    {
        GetCurrentSideShowHighlighterInner2(MyPositionID).gameObject.GetComponent<Image>().enabled = true;
    }
    public void StartInner3()
    {
        GetCurrentSideShowHighlighterInner3(MyPositionID).gameObject.GetComponent<Image>().enabled = true;
    }
    public void MovingInnerComplete1(float[] newValues)
    {
        GetCurrentSideShowHighlighterInner1(MyPositionID).gameObject.transform.localPosition = new Vector3(newValues[2] * -40f, newValues[1] * - 40f, 0f);
       
        GetCurrentSideShowHighlighterInner1(MyPositionID).gameObject.GetComponent<Image>().enabled = false;
    }
    public void MovingInnerComplete2(float[] newValues)
    {
       GetCurrentSideShowHighlighterInner2(MyPositionID).gameObject.transform.localPosition = new Vector3(newValues[2] * -80f, newValues[1] * - 80f, 0f);
        
       GetCurrentSideShowHighlighterInner2(MyPositionID).gameObject.GetComponent<Image>().enabled = false;
    }
    public void MovingInnerComplete3(float[] newValues)
    {
       GetCurrentSideShowHighlighterInner3(MyPositionID).gameObject.transform.localPosition = new Vector3(newValues[2] * -120f, newValues[1] * - 120f, 0f);
        
       GetCurrentSideShowHighlighterInner3(MyPositionID).gameObject.GetComponent<Image>().enabled = false;
    }
    public void OnDisable()
    {
        CountdownTimer.OnCountdownTimerHasUpdated -= OnCountdownTimerHasUpdated;
       
    }
    private bool AnimationCounter = false;
    private bool SideShowCounter = false;
    float CurrentStepValueTime = 25f;
    float DoubleCurrentStepValueTime = 50f;
    float CurrentSideShowValueTime = 15f;
    float DoubleCurrentSideShowValueTime = 30f;

    float _tempCurrentValueTime = 25f;
    float _tempDoubleCurrentValueTime = 50f;
    float newSec = -1f;

    void OnCountdownTimerHasUpdated(double sec)
    {
        
        //Debug.Log(currentPlayer == null ? "currentPlayer is null " : "currentPlayer is not null" +this.gameObject.name );
        if (currentPlayer != null)
        {
            if (currentPlayer.playerData.IsLocalPlayer || (currentPlayer.playerData.isSideShow && !currentPlayer.playerData.isSentSideShow))
            {
                float someSec = (float)(sec);
                float newSec2 = someSec * 2f;
                float diff = _tempDoubleCurrentValueTime - newSec2;
                newSec = diff / _tempCurrentValueTime;
                if (newSec > 0f)
                {
                    inBackground = true;
                }
                //Debug.Log("OnCountdownTimerHasUpdatedeeeeee-------" + newSec + "-------------  getName------------------ " + sec + "this.gameObject.name "+ this.gameObject.name + "currentPlayer.playerData "+ currentPlayer.playerData.NamePlayer);

            }

        }

    }
    void OnCountdownTimerHasUpdated(double sec,PlayerManagerPun _playerManagerPun)//this is not working in this code
    {
        //Debug.Log("OnCountdownTimerHasUpdated--------------" + newSec + " ----------------------------  getName---------------------------- " + sec);
        if (currentPlayer != null)
        {
            if (_playerManagerPun != null)
            {
                if (currentPlayer == _playerManagerPun)
                {
                    float someSec = (float)(sec);
                    float newSec2 = someSec * 2f;
                    float diff = _tempDoubleCurrentValueTime - newSec2;
                    newSec = diff / _tempCurrentValueTime;
                    //if (currentPlayer.managerMain != null)
                    //{

                    //    newSec=currentPlayer.managerMain.managerInfo.IsCurrentTimer;
                    //}
                    inBackground = true;
                    //Debug.Log("OnCountdownTimerHasUpdated--------------" + newSec + " ----------------------------  getName---------------------------- " + sec + "_playerManagerPun "+ _playerManagerPun.playerData.NamePlayer);
                }
                else
                {
                    ForceStopStep();
                }
            }

        }

    }
    private bool inBackground = false;
#if UNITY_EDITOR
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            //Debug.Log("[Room]: ApplicationFocus Closed now");
            OnApplicationPause(hasFocus);
        }
        else
        {
            //Debug.Log("[Room]: ApplicationFocus Open now");
            OnApplicationPause(hasFocus);
        }
    }
#elif UNITY_ANDROID

#endif
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            inBackground = false;

            if (currentPlayer != null)
            {
                if (currentPlayer.managerMain != null)
                {
                    if (currentPlayer.localPlayer != null)
                    {
                        //currentPlayer.managerMain.PauseActivity(currentPlayer);
                        
                        if (currentPlayer.photonView != null)
                        {
                            currentPlayer.photonView.RPC("PauseActivity", RpcTarget.All);
                        }
                        PhotonNetwork.SendAllOutgoingCommands();
                        //Debug.Log("OnApplicationPause in pause state ");
                    }

                }

            }
        }
        else
        {
            inBackground = true;
            if (currentPlayer != null)
            {
                if (currentPlayer.managerMain != null)
                {
                    if (currentPlayer.localPlayer != null)
                    {
                        //currentPlayer.managerMain.UnPauseActivity(currentPlayer);
                       
                        if (currentPlayer.photonView != null)
                        {
                            currentPlayer.photonView.RPC("UnPauseActivity", RpcTarget.All);
                        }
                        PhotonNetwork.SendAllOutgoingCommands();
                        //Debug.Log("OnApplicationPause in unpause state ");
                    }

                }

            }
        }
    }
    void OnCountdownTimerHasExpired()
    {
        if (!CountdownTimer.isTimerRunning)
        {
            return;
        }

        CountdownTimer.isTimerRunning = false;
    }
}
