using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class TeenPatiHUD : MonoBehaviour
{
    public Slider playerExp;
   
    public Text playerLevel; 
    public Text totalBet;
    public Text bootAmount;
    public Text blindLimit;
    public Text chalLimit;
    public Text potLimit;
    public GameObject textGlobalInfoBlackbg;
    [SerializeField]
    private Text textGlobalInfo;
    public AudioSource[] audioSource;
    public AudioClip click;
    public AudioClip clickClose;
    public AudioClip packSound;
    public AudioClip ChaalSound;
    public AudioClip SeenSound;
    public AudioClip TipSound;
    public AudioClip PlayerClickSound;
    public AudioClip ChatSound;
    public AudioClip PlusSound;
    public AudioClip MinusSound;
    public AudioClip YourTurnSound;
    public AudioClip TimerSound;
    public AudioClip CardSound;
    public AudioClip GiveCoinsSound;

  

    public AudioClip StartTimeSound;
    public AudioClip RunTimeSound;
    public AudioClip StopTimeSound;

    public AudioClip CardSeenClip;
    public AudioClip SideShowClip;
    public AudioClip SideShowWinClip;
    public AudioClip SideShowRefuseClip;
    public AudioClip BlindClip;
    public GameObject ShareButton;
    public Text ShareCodeText;
    public Image ShareIcon;

    private void Start()
    {
       
        
        if (PhotonNetwork.InRoom)
        {
            PlayerSave.FullRoomName = PhotonNetwork.CurrentRoom.Name;
        }

        if (PlayerSave.singleton.currentTable == eTable.Private)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                string roomCode = "" + PlayerSave.FullRoomName;
                if (ShareCodeText) ShareCodeText.text = roomCode;
                //if (ShareButton) ShareButton.GetComponent<Button>().onClick.AddListener(OnRoomShareClicked);
                if (ShareButton) ShareButton.gameObject.SetActive(true);
                if (ShareButton) ShareButton.GetComponent<Image>().raycastTarget = true;
                if (ShareIcon)ShareIcon.gameObject.SetActive(true);
            }
            else
            {
                string roomCode = "" + PlayerSave.FullRoomName;
                if (ShareCodeText) ShareCodeText.text = roomCode;
                if (ShareButton) ShareButton.gameObject.SetActive(true);
                if (ShareButton) ShareButton.GetComponent<Image>().raycastTarget = false;
                if (ShareIcon) ShareIcon.gameObject.SetActive(false);
            }
        }
        else
        {
            string roomCode = "" + PlayerSave.FullRoomName;
            if (ShareCodeText) ShareCodeText.text = roomCode;
            if (ShareButton) ShareButton.gameObject.SetActive(true);
            if (ShareButton) ShareButton.GetComponent<Image>().raycastTarget = false;
            if (ShareIcon) ShareIcon.gameObject.SetActive(false);
        }

    }

    public void UpdateShareIconForMasterOnlyInPrivate()
    {
        if (PhotonNetwork.InRoom)
        {
            PlayerSave.FullRoomName = PhotonNetwork.CurrentRoom.Name;
        }

        if (PlayerSave.singleton.currentTable == eTable.Private)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                string roomCode = "" + PlayerSave.FullRoomName;
                if (ShareCodeText) ShareCodeText.text = roomCode;
                //if (ShareButton) ShareButton.GetComponent<Button>().onClick.AddListener(OnRoomShareClicked);
                if (ShareButton) ShareButton.gameObject.SetActive(true);
                if (ShareButton) ShareButton.GetComponent<Image>().raycastTarget = true;
                if (ShareIcon) ShareIcon.gameObject.SetActive(true);
            }
            else
            {
                string roomCode = "" + PlayerSave.FullRoomName;
                if (ShareCodeText) ShareCodeText.text = roomCode;
                if (ShareButton) ShareButton.gameObject.SetActive(true);
                if (ShareButton) ShareButton.GetComponent<Image>().raycastTarget = false;
                if (ShareIcon) ShareIcon.gameObject.SetActive(false);
            }
        }
        
    }
    private void Update()
    {
        if (PlayerSave.singleton.currentTable != eTable.Joker)
        {
            string roomCode = "" + PlayerSave.NewRoomName;
            if (ShareCodeText) ShareCodeText.text = roomCode;
        }
    }
    public void OnSoundButton()
    {
        PlayerPrefs.SetInt("SoundOn", (PlayerPrefs.GetInt("SoundOn", 0) == 0) ? 1 : 0);
        ClickSound();
        

    }
    public void OnVibrateButton()
    {
        PlayerPrefs.SetInt("VibrateOn", (PlayerPrefs.GetInt("VibrateOn", 0) == 0) ? 1 : 0);
        ClickSound();
       
    }
    public void SetGameRunningGlobalInfo(string _text)
    {
        textGlobalInfo.enabled = true;
        textGlobalInfo.text = _text;

        textGlobalInfoBlackbg.GetComponent<Image>().enabled = true;


    }
    public void ClearTextGlobalInfo()
    {
        textGlobalInfo.text = string.Empty;
        textGlobalInfoBlackbg.GetComponent<Image>().enabled = false;
    }
    public void SetTextGlobalInfo(string _text)
    {
        textGlobalInfo.enabled = true;
        textGlobalInfo.text = _text;

        textGlobalInfoBlackbg.GetComponent<Image>().enabled = true;
        CancelInvoke("DeactivateTextInfo");
        Invoke("DeactivateTextInfo",1.2f);
    }
   
    public void TotalBoot(string _text)
    {
        totalBet.text = _text;
    }  

    private void DeactivateTextInfo()
    {
        textGlobalInfo.enabled = false;
        textGlobalInfoBlackbg.GetComponent<Image>().enabled = false;
    }

    public void ClickSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource[0] != null)
            {
                audioSource[0].clip = click;
                audioSource[0].Play();
            }
        }
    }
    public void ClickCloseSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource[0] != null)
            {
                audioSource[0].clip = clickClose;
                audioSource[0].Play();
            }
        }
    }
    public void OnGiveCoinsSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource[2] != null)
            {
                audioSource[2].clip = GiveCoinsSound;
                audioSource[2].Play();
            }
        }
    }
    
    public void OnChaalSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource[2] != null)
            {
                audioSource[2].clip = ChaalSound;
                audioSource[2].Play();
            }
        }
    }
    public void OnPlusSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource[2] != null)
            {
                audioSource[2].clip = PlusSound;
                audioSource[2].Play();
            }
        }
    }
    public void OnMinusSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource[2] != null)
            {
                audioSource[2].clip = MinusSound;
                audioSource[2].Play();
            }
        }
    }
    public void OnPackSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource[2] != null)
            {
                audioSource[2].clip = packSound;
                audioSource[2].Play();
            }
        }
    }
    public void OnSeenSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource[2] != null)
            {
                audioSource[2].clip = SeenSound;
                audioSource[2].Play();
            }
        }
    }
    public void OnChatSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource[2] != null)
            {
                audioSource[2].clip = ChatSound;
                audioSource[2].Play();
            }
        }
    }
    public void OnYourTurnSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource[0] != null)
            {
                audioSource[0].clip = YourTurnSound;
                audioSource[0].Play();
            }
        }
        if (PlayerPrefs.GetInt("VibrateOn", 0) == 0)
        {
            Handheld.Vibrate();
        }
    }
    public void OnHomeSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource[0] != null)
            {
                audioSource[0].clip = click;
                audioSource[0].Play();
            }
        }
    }
    public void OnTipSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource[0] != null)
            {
                audioSource[0].clip = TipSound;
                audioSource[0].Play();
            }
        }
    }
    public void OnPlayerSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource[0] != null)
            {
                audioSource[0].clip = PlayerClickSound;
                audioSource[0].Play();
            }
        }
    }
    public void OnCardSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource[0] != null)
            {
                audioSource[0].clip = CardSound;
                audioSource[0].Play();
            }
        }
    }
    public void OnStartTimerSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource[1] != null)
            {
                audioSource[1].clip = StartTimeSound;
                audioSource[1].Play();
            }
        }
    }
    public void OnRunTimerSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource[1] != null)
            {
                audioSource[1].loop = true;
                audioSource[1].clip = RunTimeSound;
                audioSource[1].Play();
            }
        }
    }
    public void OnForceStopSound()
    {
        if (audioSource[1] != null)
        {
            audioSource[1].loop = false;
            audioSource[1].Stop();
        }
    }
    public void OnStopTimerSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource[1] != null)
            {
                audioSource[1].loop = false;
                audioSource[1].clip = StopTimeSound;
                audioSource[1].Play();
            }
        }
        else
        {
            if(audioSource[1]!=null)
            {
                audioSource[1].loop = false;
                audioSource[1].Stop();
            }
        }
    }
    public void OnCardSeenSound()
    {
        if (audioSource[0] != null)
        {
            audioSource[0].clip = CardSeenClip;
            audioSource[0].Play();
        }
    }
    public void OnSideShowSound()
    {
        if (audioSource[0] != null)
        {
            audioSource[0].clip = SideShowClip;
            audioSource[0].Play();
        }
    }
    public void OnSideShowWinSound()
    {
        if (audioSource[0] != null)
        {
            audioSource[0].clip = SideShowWinClip;
            audioSource[0].Play();
        }
    }
    
    public void OnSideShowRefuseSound()
    {
        if (audioSource[0] != null)
        {
            audioSource[0].clip = SideShowRefuseClip;
            audioSource[0].Play();
        }
    }
    public void OnBlindSound()
    {
        if (PlayerPrefs.GetInt("SoundOn", 0) == 0)
        {
            if (audioSource[2] != null)
            {
                audioSource[2].clip = BlindClip;
                audioSource[2].Play();
            }
        }
    }
    public void OnRoomShareClicked()
    {
       
        string share = " Join me in KhelTamasha .My private room code is " + PlayerSave.FullRoomName;
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
    public void OnHomeButton()
    {
        LocalPlayerPun localPlayerPun = FindObjectOfType<LocalPlayerPun>();
        if(localPlayerPun == null)
        {
            if(PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            }
        }
    }
}
