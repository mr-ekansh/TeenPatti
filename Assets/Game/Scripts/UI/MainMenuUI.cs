//using Facebook.Unity;
using SocialApp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Text monetPlayer;
    public Text chipsPlayer;
    public Text expPlayerText;
    public Slider explayerSlider;
    public Sprite avatarDefault;
    public Sprite girlAvatar;
    public Sprite BoyAvatar;
    
    public Text PlayerName;
    public Text addCashmoneyPlayer;

    public Text withdrawText;
    public Text depositText;
    public Text promoText;


    public GameObject Panel_MyAccount;
    public GameObject Panel_BonusCash;
    public GameObject Panel_Orientation;
    public GameObject Panel_Extra;
    public GameObject Panel_Withdraw;
    public GameObject Panel_ReferAFriend;
    public GameObject Panel_AddCashWebView;
    public GameObject Panel_BonusPopUp;
    public Image OfferPopUp;

    public GameObject SubPanel_PersonalDetails;
    public GameObject SubPanel_KycDocuments;
    public GameObject SubPanel_PanDocument;
    public GameObject SubPanel_AadharDocument;
    public GameObject SubPanel_DrivingLicense;
    public GameObject SubPanel_VoterCard;
    public GameObject SubPanel_PassportNumber;
    public GameObject SubPanel_BankDetails;
    public GameObject SubPanel_ChangeAvatar;
    public GameObject SubPanel_PrivacyPolicy;
    public GameObject SubPanel_TermsConditions;
    public GameObject SubPanel_AccountOverview;

    [Header("Personal Details Parameters")]
    public InputField PDEnterFirstName;
    public InputField PDEnterLastName;
    public InputField PDEnterMobileNo;
    public InputField PDEnterEmail;
    public InputField PDEnterDob;
    public InputField PDEnterStreetNo_1;
    public InputField PDEnterStreetNo_2;
    public InputField PDEnterCity;
    public InputField PDEnterState;
    public InputField PDEnterPinCode;
    public Button PDSaveButton;
    public Button PDEnterDOBBtn;
    public Dropdown PDGender;
    private string dropDownOutput;
    public GameObject[] AccountSelectedIcons;
    public GameObject[] AccountNonSelectedIcons;


    public Button myAccountBtn;

    [Header("KYC Parameters")]
    public Button PanBaseButton;
    public Button AddressBaseButton;

    public GameObject PanBaseSelected;
    public GameObject AddressBaseSelected;

    public GameObject PanNotVerified;
    public GameObject PanStatusPending;
    public GameObject PanVerified;

    public GameObject AddressNotVerified;
    public GameObject AddressStatusPending;
    public GameObject AddressVerified;

    public GameObject PanForwardArrow;
    public GameObject PanDownwardArrow;

    public GameObject AddressForwardArrow;
    public GameObject AddressDownwardArrow;

    public Button KycSubmitBtn;

    public GameObject KYCDropDownBase;
    public Button KYCAadharCard;
    public Button KYCLicense;
    public Button KYCPassport;
    public Button KYCVoterCard;

    public GameObject KYCAadharCardSelection;
    public GameObject KYCLicenseSelection;
    public GameObject KYCPassportSelection;
    public GameObject KYCVoterCardSelection;

    [Header("KYC Parameters - For PanCard")]
    public InputField PanEnterCardNo;
    public Button PanUploadButton;
    public Button PanSaveButton;
    public Text PanUploadedImage;

    [Header("KYC Parameters - For AadharCard")]
    public InputField AadharCardEnterCardNo;
    public Button AadharCardUploadButton;
    public Button AadharCardSaveButton;
    public Text AadharUploadedImage;

    [Header("KYC Parameters - For License")]
    public InputField LicenseEnterCardNo;
    public Button LicenseUploadButton;
    public Button LicenseSaveButton;
    public Text LicenseUploadedImage;

    [Header("KYC Parameters - For Passport")]
    public InputField PassportEnterCardNo;
    public Button PassportUploadButton;
    public Button PassportSaveButton;
    public Text PassportUploadedImage;

    [Header("KYC Parameters - For VoterCard")]
    public InputField VoterCardEnterCardNo;
    public Button VoterCardUploadButton;
    public Button VoterCardSaveButton;
    public Text VoterCardUploadedImage;

    [Header("Bank Details Parameters")]
    public InputField BankEnterAccountNo;
    public InputField BankReEnterAccountNo;
    public InputField BankEnterIFSCCode;
    public Button BankSubmitButton;
    public Button BankNewRequestButton;
    public GameObject EnterBankObject;
    public GameObject EnterUPIObject;

    [Header("New Request For Bank Details Parameters")]
    public GameObject BankNewRequestOptions;
    public InputField BankEnterAccountNo_NR;
    public InputField BankReEnterAccountNo_NR;
    public InputField BankEnterIFSCCode_NR;
    public Button BankSubmitButton_NR;
    public Text BankNewRequestStatus_NR;
    public Text BankRequestStatus;

    [Header("Bank Details UPI Parameters")]
    public InputField BankUPIEnterUPI;
    public Button BankUPISubmitButton;
    public Button BankUPINewRequestButton;
    public Toggle BankTranferToggle;
    public Toggle BankUPIToggle;

    [Header("New Request For Bank Details UPI Parameters")]
    public GameObject BankUPINewRequestOptions;
    public InputField BankUPIEnterUPI_NR;
    public Button BankUPISubmitButton_NR;
    public Text BankUPINewRequestStatus_NR;
    public Text BankUPINewRequestStatus;

    [Header("Change Avatar Parameters")]
    public Text AvatarName;
    public Image ChangeAvatarImage;
    public Button ChangeAvatarCameraButton;
    public Button ChangeAvatarGalleryButton;
    public Button ChangeAvatarSaveButton;
    public GameObject[] TickObjects;


    [Header("AddCash Parameters")]
    public GameObject[] AddCashSelectedIcons;
    public GameObject[] AddCashNonSelectedIcons;

    public GameObject Canvas_AddCash;
    public GameObject AddCashBg_0;
    public GameObject AddCashBg_1;
    public GameObject AddCashBg_2;
    public GameObject MiniStatement;

    [Header("AddCash SubParameters")]
    public InputField amountInputField;
    public InputField bonusInputField;
    public Button ApplyBonusCodeBtn;
    public Button PasteBonusCodeBtn;
    public GameObject AppliedBonusCode;
    public Text _amountText_1;
    public Text _amountText_2;
    public Text _amountText_3;
    public Text _amountText_4;
    public Text _amountText_1_S;
    public Text _amountText_2_S;
    public Text _amountText_3_S;
    public Text _amountText_4_S;

    public Text _BonusAmountText_1;
    public Text _BonusAmountText_2;
    public Text _BonusAmountText_3;
    public Text _BonusAmountText_4;
    public Text _BonusAmountText_1_S;
    public Text _BonusAmountText_2_S;
    public Text _BonusAmountText_3_S;
    public Text _BonusAmountText_4_S;
    public int MyBonusIndex = 0;
  
    public double LastDeposit = 0;
    public GameObject[] InputBoxSelection;


    [Header("Withdraw Parameters")]
    public InputField withdrawalAmount;
    public InputField withdrawalAccountHolderName;
    public InputField withdrawalAccountHolderIfscCode;
    public InputField withdrawalAccountHolderAccountNumber;
    public Text withdrawalTotalWinningAmount;
    public GameObject Panel_WithdrawStatement;
    public GameObject Panel_WithdrawNewRequest;
    public Toggle WithdrawTranferToggle;
    public Toggle WithdrawUPIToggle;
    public GameObject WithdrawBankObject;
    public GameObject WithdrawUPIObject;
    public InputField withdrawalUPIId;
    public GameObject ChangeBankButton;
    public GameObject ChangeUPIBankButton;

    [Header("Bonus Parameters")]
    public GameObject Panel_BonusBg;
    public GameObject Panel_BonusStatement;
    public GameObject[] BonusSelectedIcons;
    public GameObject[] BonusNonSelectedIcons;

    [Header("Refer Parameters")]
    public GameObject Panel_ReferBg;
    public GameObject Panel_ReferStatement;
    public GameObject[] ReferSelectedIcons;
    public GameObject[] ReferNonSelectedIcons;

    public delegate void onPayNowUI();
    public static event onPayNowUI OnPayNowUI;

    public delegate void onCloseWebViewNowUI();
    public static event onCloseWebViewNowUI OnCloseWebViewNowUI;

    

    public static MainMenuUI menuUI;
    public bool isDebug = false;
    // Use this for initialization
    void Start()
    {
        menuUI = this;
        monetPlayer.text = PlayerSave.singleton.GetCurrentMoney().ToString("F2");
        chipsPlayer.text = PlayerSave.singleton.GetCurrentChips().ToString("F2");
        if (addCashmoneyPlayer) addCashmoneyPlayer.text = PlayerSave.singleton.GetCurrentMoney().ToString();
        //double _exp = PlayerSave.singleton.GetExp();
        //double level = (int)_exp / 100;
        //explayerSlider.value = _exp - (level * 100);
        //expPlayerText.text = level.ToString();

        PlayerName.text = PlayerSave.FirstCharToUpper(PlayerSave.singleton.GetCurrentNamey());
        //if (PlayerSave.singleton.GetGender() == "Male")
        //{
        //    PlayerIcon.sprite = BoyAvatar;
        //}
        //else
        //{
        //    PlayerIcon.sprite = girlAvatar;
        //}

        PDEnterFirstName.onValueChanged.AddListener(OnValueChangedPDEnterFirstName);
        PDEnterFirstName.onEndEdit.AddListener(OnEndEditPDEnterFirstName);

        PDEnterLastName.onValueChanged.AddListener(OnValueChangedPDEnterLastName);
        PDEnterLastName.onEndEdit.AddListener(OnEndEditPDEnterLastName);

        PDEnterMobileNo.onValueChanged.AddListener(OnValueChangedPDEnterMobileNo);
        PDEnterMobileNo.onEndEdit.AddListener(OnEndEditPDEnterMobileNo);

        PDEnterEmail.onValueChanged.AddListener(OnValueChangedPDEnterEmail);
        PDEnterEmail.onEndEdit.AddListener(OnEndEditPDEnterEmail);

        PDEnterDob.onValueChanged.AddListener(OnValueChangedPDEnterDob);
        PDEnterDob.onEndEdit.AddListener(OnEndEditPDEnterDob);

        PDEnterStreetNo_1.onValueChanged.AddListener(OnValueChangedPDEnterStreetNo_1);
        PDEnterStreetNo_1.onEndEdit.AddListener(OnEndEditPDEnterStreetNo_1);

        PDEnterStreetNo_2.onValueChanged.AddListener(OnValueChangedPDEnterStreetNo_2);
        PDEnterStreetNo_2.onEndEdit.AddListener(OnEndEditPDEnterStreetNo_2);

        PDEnterCity.onValueChanged.AddListener(OnValueChangedPDEnterCity);
        PDEnterCity.onEndEdit.AddListener(OnEndEditPDEnterCity);

        PDEnterState.onValueChanged.AddListener(OnValueChangedPDEnterState);
        PDEnterState.onEndEdit.AddListener(OnEndEditPDEnterState);

        PDEnterPinCode.onValueChanged.AddListener(OnValueChangedPDEnterPinCode);
        PDEnterPinCode.onEndEdit.AddListener(OnEndEditPDEnterPinCode);

        PDEnterDOBBtn.onClick.AddListener(onClickPDEnterDOBBtn);

#if UNITY_ANDROID && !UNITY_EDITOR
         PDEnterDOBBtn.interactable = true;
        PDEnterDob.interactable = false;
         PDEnterDob.GetComponent<Image>().raycastTarget = false;
#else
        PDEnterDOBBtn.interactable = false;
        PDEnterDob.interactable = true;
        PDEnterDob.GetComponent<Image>().raycastTarget = true;
#endif
        //PDGender.GetComponent<Toggle>().onValueChanged.RemoveAllListeners();

        //PDGender.GetComponent<Toggle>().onValueChanged.AddListener((value) =>
        //{
        //    PlayerPrefs.SetInt(StaticStrings.SoundsKey, value ? 0 : 1);
        //    if (value)
        //    {
        //        AudioListener.volume = 1;
        //    }
        //    else
        //    {
        //        AudioListener.volume = 0;
        //    }
        //}
        //);

        PDGender.onValueChanged.AddListener(HandleInputData);

        dropDownOutput = "Male";

        PDSaveButton.onClick.AddListener(onClickPDSaveButton);

        AccountNonSelectedIcons[0].GetComponent<Button>().onClick.AddListener(onClickPersonalDetails);
        AccountNonSelectedIcons[1].GetComponent<Button>().onClick.AddListener(onClickKYCDetails);
        AccountNonSelectedIcons[2].GetComponent<Button>().onClick.AddListener(onClickBankDetails);
        AccountNonSelectedIcons[3].GetComponent<Button>().onClick.AddListener(onClickAvatarDetails);
        AccountNonSelectedIcons[4].GetComponent<Button>().onClick.AddListener(onClickPrivacyDetails);
        AccountNonSelectedIcons[5].GetComponent<Button>().onClick.AddListener(onClickTermsDetails);
        AccountNonSelectedIcons[6].GetComponent<Button>().onClick.AddListener(onClickAccountOverviewDetails);
        myAccountBtn.onClick.AddListener(onMyAccountBtn);

        KycSubmitBtn.onClick.AddListener(onKYCSubmitBtn);

        KYCAadharCard.onClick.AddListener(onKYCAadharCard);
        KYCLicense.onClick.AddListener(onKYCLicense);
        KYCPassport.onClick.AddListener(onKYCPassport);
        KYCVoterCard.onClick.AddListener(onKYCVoterCard);

        PanBaseButton.onClick.AddListener(onPanBaseButton);
        AddressBaseButton.onClick.AddListener(onAddressBaseButton);

        Panel_MyAccount.SetActive(false);
        Canvas_AddCash.SetActive(false);
        

        PanUploadButton.onClick.AddListener(onPanUploadButton);
        PanSaveButton.onClick.AddListener(onPanSaveButton);

        AadharCardUploadButton.onClick.AddListener(onAadharCardUploadButton);
        AadharCardSaveButton.onClick.AddListener(onAadharCardSaveButton);

        LicenseUploadButton.onClick.AddListener(onLicenseUploadButton);
        LicenseSaveButton.onClick.AddListener(onLicenseSaveButton);

        PassportUploadButton.onClick.AddListener(onPassportUploadButton);
        PassportSaveButton.onClick.AddListener(onPassportSaveButton);

        VoterCardUploadButton.onClick.AddListener(onVoterCardUploadButton);
        VoterCardSaveButton.onClick.AddListener(onVoterCardSaveButton);

        ChangeAvatarCameraButton.onClick.AddListener(TakePicture);
        ChangeAvatarGalleryButton.onClick.AddListener(PickPicture);
        ChangeAvatarSaveButton.onClick.AddListener(OnUploadImage);
        ChangeAvatarSaveButton.interactable = true;


        if (PlayerSave.singleton.GetAvatar()>= 0 && PlayerSave.singleton.GetAvatar()<27)
        {
            ChangeAvatarImage.sprite = PlayerSave.singleton._avatarImages[PlayerSave.singleton.GetAvatar()];
        }
        else
        {
            PlayerSave.singleton.SaveAvatarScreen(0);
            ChangeAvatarImage.sprite = PlayerSave.singleton._avatarImages[0];
        }
        
        if(PlayerSave.singleton!=null)
        {
            isDebug = PlayerSave.singleton.debug;
        }

        if (BankTranferToggle != null)
        {
            BankTranferToggle.onValueChanged.AddListener(delegate
            {
                BankTransferToggleValueChanged(BankTranferToggle);
            });
        }

        if (BankUPIToggle != null)
        {
            BankUPIToggle.onValueChanged.AddListener(delegate
            {
                BankUPIToggleValueChanged(BankUPIToggle);
            });
        }
        if (WithdrawTranferToggle != null)
        {
            WithdrawTranferToggle.onValueChanged.AddListener(delegate
            {
                WithdrawTransferToggleValueChanged(WithdrawTranferToggle);
            });
        }

        if (WithdrawUPIToggle != null)
        {
            WithdrawUPIToggle.onValueChanged.AddListener(delegate
            {
                WithdrawUPIToggleValueChanged(WithdrawUPIToggle);
            });
        }
        if(StaticValues.FirstTimeDepositPrompt)
        {
            StaticValues.FirstTimeDepositPrompt = false;
			StaticValues.OutOfLimitPopUp=false;
            onWithdrawButton();
        }

		if(StaticValues.OutOfLimitPopUp)
		{
			StaticValues.OutOfLimitPopUp=false;
			OnAddCashbutton();
		}
        
    }
    //Output the new state of the Toggle into Text
    void BankTransferToggleValueChanged(Toggle change)
    {
        // BankTranferToggle.isOn;
        if (Panel_MyAccount.activeSelf )
        {
            onClickBankDetails();
        }
    }
    void BankUPIToggleValueChanged(Toggle change)
    {
        // BankUPIToggle.isOn;
        if (Panel_MyAccount.activeSelf)
        {
            onClickBankDetails();
        }
    }
    void WithdrawTransferToggleValueChanged(Toggle change)
    {
        // BankTranferToggle.isOn;
        WithdrawBankObject.SetActive(true);
        WithdrawUPIObject.SetActive(false);


        BankTranferToggle.isOn = true;
        BankUPIToggle.isOn = false;
    }
    void WithdrawUPIToggleValueChanged(Toggle change)
    {
        // BankUPIToggle.isOn;
        WithdrawBankObject.SetActive(false);
        WithdrawUPIObject.SetActive(true);

        BankTranferToggle.isOn = false;
        BankUPIToggle.isOn = true;

    }
    public void RaiseOnConfirmButtonClick()
    {
        if (OnPayNowUI != null)
        {
            OnPayNowUI();
        }
        RefreshCoins();
    }
    public void CloseAddCash()
    {
       if(Canvas_AddCash) Canvas_AddCash.SetActive(false);
    }
    public void RaiseOnBackButtonClick()
    {
        if (OnCloseWebViewNowUI != null)
        {
            OnCloseWebViewNowUI();
        }
        RefreshCoins();

        if (PlayerSave.singleton != null)
        {
            PlayerSave.singleton.GetBannerDetails(PlayerSave.singleton.newID(), OnBannerListLoaded);
        }
    }
    public void OnBannerListLoaded(GetBannerResponse _callback)
    {
        if (_callback != null)
        {
            if (_callback.status == 200)
            {
                
                    Debug.Log("Canvas_AddCash.activeInHierarchy " + Canvas_AddCash.activeInHierarchy);
                    if (Canvas_AddCash.activeInHierarchy)
                    {
                        CallBeforeRefreshAddCashPage();
                        RefreshAddCashPage(MyBonusIndex);
                    }
                
            }
            else
            {

            }
        }
    }
    private void onClickPersonalDetails()
    {
        AccountSelectedIcons[0].SetActive(true);
        AccountSelectedIcons[1].SetActive(false);
        AccountSelectedIcons[2].SetActive(false);
        AccountSelectedIcons[3].SetActive(false);
        AccountSelectedIcons[4].SetActive(false);
        AccountSelectedIcons[5].SetActive(false);
        AccountSelectedIcons[6].SetActive(false);

        Panel_MyAccount.SetActive(true);
        SubPanel_PersonalDetails.SetActive(true);
        SubPanel_KycDocuments.SetActive(false);
        SubPanel_PanDocument.SetActive(false);
        SubPanel_AadharDocument.SetActive(false);
        SubPanel_DrivingLicense.SetActive(false);
        SubPanel_VoterCard.SetActive(false);
        SubPanel_PassportNumber.SetActive(false);
        SubPanel_BankDetails.SetActive(false);
        SubPanel_ChangeAvatar.SetActive(false);
        SubPanel_PrivacyPolicy.SetActive(false);
        SubPanel_TermsConditions.SetActive(false);
        SubPanel_AccountOverview.SetActive(false);
        FillPersonalDetails();
    }
    private void onClickKYCDetails()
    {
        AccountSelectedIcons[0].SetActive(false);
        AccountSelectedIcons[1].SetActive(true);
        AccountSelectedIcons[2].SetActive(false);
        AccountSelectedIcons[3].SetActive(false);
        AccountSelectedIcons[4].SetActive(false);
        AccountSelectedIcons[5].SetActive(false);
        AccountSelectedIcons[6].SetActive(false);

        Panel_MyAccount.SetActive(true);
        SubPanel_PersonalDetails.SetActive(false);
        SubPanel_KycDocuments.SetActive(true);
        SubPanel_PanDocument.SetActive(false);
        SubPanel_AadharDocument.SetActive(false);
        SubPanel_DrivingLicense.SetActive(false);
        SubPanel_VoterCard.SetActive(false);
        SubPanel_PassportNumber.SetActive(false);
        SubPanel_BankDetails.SetActive(false);
        SubPanel_ChangeAvatar.SetActive(false);
        SubPanel_PrivacyPolicy.SetActive(false);
        SubPanel_TermsConditions.SetActive(false);
        SubPanel_AccountOverview.SetActive(false);

        SetKYCDetailsPage();

        AadharUploadedImage.text = "";
        PassportUploadedImage.text = "";
        PanUploadedImage.text = "";
        LicenseUploadedImage.text = "";
        VoterCardUploadedImage.text = "";
    }
    private void onClickBankDetails()
    {

        //Debug.Log("Bank " + BankTranferToggle.isOn + " " + BankUPIToggle.isOn);
        AccountSelectedIcons[0].SetActive(false);
        AccountSelectedIcons[1].SetActive(false);
        AccountSelectedIcons[2].SetActive(true);
        AccountSelectedIcons[3].SetActive(false);
        AccountSelectedIcons[4].SetActive(false);
        AccountSelectedIcons[5].SetActive(false);
        AccountSelectedIcons[6].SetActive(false);

        Panel_MyAccount.SetActive(true);
        SubPanel_PersonalDetails.SetActive(false);
        SubPanel_KycDocuments.SetActive(false);
        SubPanel_PanDocument.SetActive(false);
        SubPanel_AadharDocument.SetActive(false);
        SubPanel_DrivingLicense.SetActive(false);
        SubPanel_VoterCard.SetActive(false);
        SubPanel_PassportNumber.SetActive(false);
        SubPanel_BankDetails.SetActive(true);
        SubPanel_ChangeAvatar.SetActive(false);
        SubPanel_PrivacyPolicy.SetActive(false);
        SubPanel_TermsConditions.SetActive(false);
        SubPanel_AccountOverview.SetActive(false);

        if (!string.IsNullOrEmpty(StaticValues.BankAccountNo))
        {
            if (StaticValues.BankAccountNo.Length > 0)
            {
                //string newString = new string('*', (StaticValues.BankAccountNo.Length - 4));
                BankEnterAccountNo.text = StaticValues.BankAccountNo;// newString + StaticValues.BankAccountNo.Substring(StaticValues.BankAccountNo.Length - 4, 4);
            }
        }
        else
        {
            BankEnterAccountNo.text = "";
        }
        if (!string.IsNullOrEmpty(StaticValues.BankAccountNo))
        {
            if (StaticValues.BankAccountNo.Length > 0)
            {
                //string newString = new string('*', (StaticValues.BankAccountNo.Length - 4));
                BankReEnterAccountNo.text = StaticValues.BankAccountNo;// newString + StaticValues.BankAccountNo.Substring(StaticValues.BankAccountNo.Length - 4, 4);
            }
        }
        else
        {
            BankReEnterAccountNo.text = "";
        }

        if (!string.IsNullOrEmpty(StaticValues.BankIFSCCode))
        {
            if (StaticValues.BankIFSCCode.Length > 0 )
            {
                //string newString = new string('*', (StaticValues.BankIFSCCode.Length - 4));
                BankEnterIFSCCode.text = StaticValues.BankIFSCCode; //newString + StaticValues.BankIFSCCode.Substring(StaticValues.BankIFSCCode.Length - 4, 4);
            }
        }
        else
        {
            BankEnterIFSCCode.text = "";
        }

        if (!string.IsNullOrEmpty(StaticValues.BankAccountNo_NR))
        {
            if (StaticValues.BankAccountNo_NR.Length > 0)
            {
                //string newString = new string('*', (StaticValues.BankAccountNo.Length - 4));
                BankEnterAccountNo_NR.text = StaticValues.BankAccountNo_NR;// newString + StaticValues.BankAccountNo.Substring(StaticValues.BankAccountNo.Length - 4, 4);
            }
        }
        else
        {
            BankEnterAccountNo_NR.text = "";
        }
        if (!string.IsNullOrEmpty(StaticValues.BankAccountNo_NR))
        {
            if (StaticValues.BankAccountNo_NR.Length > 0)
            {
                //string newString = new string('*', (StaticValues.BankAccountNo.Length - 4));
                BankReEnterAccountNo_NR.text = StaticValues.BankAccountNo_NR;// newString + StaticValues.BankAccountNo.Substring(StaticValues.BankAccountNo.Length - 4, 4);
            }
        }
        else
        {
            BankReEnterAccountNo_NR.text = "";
        }

        if (!string.IsNullOrEmpty(StaticValues.BankIFSCCode_NR))
        {
            if (StaticValues.BankIFSCCode_NR.Length > 0)
            {
                //string newString = new string('*', (StaticValues.BankIFSCCode.Length - 4));
                BankEnterIFSCCode_NR.text = StaticValues.BankIFSCCode_NR; //newString + StaticValues.BankIFSCCode.Substring(StaticValues.BankIFSCCode.Length - 4, 4);
            }
        }
        else
        {
            BankEnterIFSCCode_NR.text = "";
        }

        BankRequestStatus.text = "";
        if (StaticValues.isBankDetailsSubmitted)
        {
            BankSubmitButton.gameObject.SetActive(false);
            BankNewRequestButton.gameObject.SetActive(true);
            BankEnterAccountNo.interactable = false;
            BankReEnterAccountNo.interactable = false;
            BankEnterIFSCCode.interactable = false;
            BankRequestStatus.text = "";
            if (!string.IsNullOrEmpty(StaticValues.isBankStatusForNewRequest))
            {
                BankNewRequestOptions.SetActive(true);
                BankNewRequestStatus_NR.text = "<color=#FFFFFF>Status For New Bank Details Change is :</color> " + StaticValues.isBankStatusForNewRequest;
                BankNewRequestButton.interactable = false;
                BankSubmitButton_NR.interactable = false;
                BankEnterIFSCCode_NR.interactable = false;
                BankEnterAccountNo_NR.interactable = false;
                BankReEnterAccountNo_NR.interactable = false;
                BankEnterAccountNo.interactable = false;
                BankReEnterAccountNo.interactable = false;
                BankEnterIFSCCode.interactable = false;
                if(string.IsNullOrEmpty(BankEnterAccountNo_NR.text))
                {
                    BankNewRequestOptions.SetActive(false);
                    BankRequestStatus.text= "<color=#FFFFFF>Status For New Bank Details Change is :</color> " + StaticValues.isBankStatusForNewRequest;
                }
                else
                {
                    BankRequestStatus.text = "";
                    BankEnterIFSCCode_NR.interactable = true;
                    BankEnterAccountNo_NR.interactable = true;
                    BankReEnterAccountNo_NR.interactable = true;
                }
            }
            else
            {
                BankRequestStatus.text = "";
                BankNewRequestOptions.SetActive(false);
                BankNewRequestButton.interactable = true;
                BankSubmitButton_NR.interactable = true;
                BankEnterIFSCCode_NR.interactable = true;
                BankEnterAccountNo_NR.interactable = true;
                BankReEnterAccountNo_NR.interactable = true;
                BankEnterAccountNo.interactable = false;
                BankReEnterAccountNo.interactable = false;
                BankEnterIFSCCode.interactable = false;
            }
        }
        else
        {
            BankSubmitButton.gameObject.SetActive(true);
            BankEnterAccountNo.interactable = true;
            BankEnterIFSCCode.interactable = true;
            BankReEnterAccountNo.interactable = true;
            BankEnterAccountNo_NR.interactable = true;
            BankReEnterAccountNo_NR.interactable = true;
            BankEnterIFSCCode_NR.interactable = true;
            BankNewRequestButton.gameObject.SetActive(false);
            BankNewRequestOptions.SetActive(false);
            BankRequestStatus.text = "";
        }

        if (!string.IsNullOrEmpty(StaticValues.BankUPIId))
        {
            if (StaticValues.BankUPIId.Length > 0)
            {
                //string newString = new string('*', (StaticValues.BankAccountNo.Length - 4));
                BankUPIEnterUPI.text = StaticValues.BankUPIId;// newString + StaticValues.BankAccountNo.Substring(StaticValues.BankAccountNo.Length - 4, 4);
            }
        }
        else
        {
            BankUPIEnterUPI.text = "";
            BankUPIEnterUPI.interactable = true;
        }
        if (!string.IsNullOrEmpty(StaticValues.BankUPIId_NR))
        {
            if (StaticValues.BankUPIId_NR.Length > 0)
            {
                //string newString = new string('*', (StaticValues.BankAccountNo.Length - 4));
                BankUPIEnterUPI_NR.text = StaticValues.BankUPIId_NR;// newString + StaticValues.BankAccountNo.Substring(StaticValues.BankAccountNo.Length - 4, 4);
            }
        }
        else
        {
            BankUPIEnterUPI_NR.text = "";
        }
        BankUPINewRequestStatus.text = "";
        BankUPINewRequestStatus_NR.text = "";

        //Debug.Log("StaticValues.isBankUPIDetailsSubmitted " + StaticValues.isBankUPIDetailsSubmitted);
        if (StaticValues.isBankUPIDetailsSubmitted)
        {
            BankUPISubmitButton.gameObject.SetActive(false);
            BankUPINewRequestButton.gameObject.SetActive(true);
            //Debug.Log("StaticValues.isBankUPIStatusForNewRequest " + StaticValues.isBankUPIStatusForNewRequest);
            if (!string.IsNullOrEmpty(StaticValues.isBankUPIStatusForNewRequest))
            {
                BankUPINewRequestOptions.SetActive(true);
                BankUPINewRequestStatus_NR.text = "<color=#FFFFFF>Status For UPI Details Change is :</color> " + StaticValues.isBankUPIStatusForNewRequest;
                BankUPINewRequestButton.interactable = false;
                BankUPISubmitButton_NR.interactable = false;
                BankUPIEnterUPI.interactable = false;
                BankUPIEnterUPI_NR.interactable = false;
                if (string.IsNullOrEmpty(BankUPIEnterUPI_NR.text))
                {
                    BankUPINewRequestOptions.SetActive(false);
                    BankUPINewRequestStatus.text = "<color=#FFFFFF>Status For UPI Details Change is :</color> " + StaticValues.isBankUPIStatusForNewRequest;
                }
                else
                {
                    BankUPINewRequestStatus.text = "";
                    BankUPIEnterUPI_NR.interactable = true;
                }
            }
            else
            {
                BankUPINewRequestOptions.SetActive(false);
                BankUPINewRequestButton.interactable = true;
                BankUPISubmitButton_NR.interactable = true;
                BankUPIEnterUPI.interactable = false;
                BankUPIEnterUPI_NR.interactable = true;
            }
        }
        else
        {
            BankUPISubmitButton.gameObject.SetActive(true);
            BankUPIEnterUPI.interactable = true;
            BankUPIEnterUPI_NR.interactable = true;
            BankUPINewRequestButton.gameObject.SetActive(false);
            BankUPINewRequestOptions.SetActive(false);
        }
        //Debug.Log("Banknnnn " + BankTranferToggle.isOn + " " + BankUPIToggle.isOn);
        if (BankTranferToggle != null)
        {
            if (BankTranferToggle.isOn)
            {
                if (EnterBankObject) EnterBankObject.SetActive(true);
                if (EnterUPIObject) EnterUPIObject.SetActive(false);
            }
            
        }
        //Debug.Log("Bankfffff " + BankTranferToggle.isOn + " " + BankUPIToggle.isOn);
        if (BankUPIToggle != null)
        {
            if (BankUPIToggle.isOn)
            {
                if (EnterBankObject) EnterBankObject.SetActive(false);
                if (EnterUPIObject) EnterUPIObject.SetActive(true);
            }
            
        }
        //Debug.Log("Bankggg " + BankTranferToggle.isOn + " " + BankUPIToggle.isOn);
    }
    private void onClickAvatarDetails()
    {
        AccountSelectedIcons[0].SetActive(false);
        AccountSelectedIcons[1].SetActive(false);
        AccountSelectedIcons[2].SetActive(false);
        AccountSelectedIcons[3].SetActive(true);
        AccountSelectedIcons[4].SetActive(false);
        AccountSelectedIcons[5].SetActive(false);
        AccountSelectedIcons[6].SetActive(false);

        Panel_MyAccount.SetActive(true);
        SubPanel_PersonalDetails.SetActive(false);
        SubPanel_KycDocuments.SetActive(false);
        SubPanel_PanDocument.SetActive(false);
        SubPanel_AadharDocument.SetActive(false);
        SubPanel_DrivingLicense.SetActive(false);
        SubPanel_VoterCard.SetActive(false);
        SubPanel_PassportNumber.SetActive(false);
        SubPanel_BankDetails.SetActive(false);
        SubPanel_ChangeAvatar.SetActive(true);
        SubPanel_PrivacyPolicy.SetActive(false);
        SubPanel_TermsConditions.SetActive(false);
        SubPanel_AccountOverview.SetActive(false);

        AvatarName.text = StaticValues.displayNameinUC.ToUpperInvariant();

      
        SetSelection();
        OnceDownloadImage = false;
        if(!string.IsNullOrEmpty(StaticValues.avatarPicUrl))
        {
            CallOnceInFrame(StaticValues.avatarPicUrl);
        }
        else
        {
            if (!string.IsNullOrEmpty(StaticValues.customPicUrl))
            {
                CallOnceInFrame(StaticValues.customPicUrl);
            }
        }
    }
    private bool OnceDownloadImage = false;
    private Sprite ImageBody;
    private IEnumerator OnLoadGraphic(string imageUrl)
    {
        string _url = imageUrl;

        if (!string.IsNullOrEmpty(_url) && (_url.StartsWith("http") || _url.StartsWith("file")))
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(_url);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                if (isDebug)
                {
                    Debug.Log(www.error);
                }
                OnceDownloadImage = false;
            }
            else
            {
                Texture2D _texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                if (_texture != null)
                {
                    ImageBody = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);

                    ChangeAvatarImage.sprite = ImageBody;
                    OnceDownloadImage = true;
                }
                else
                {
                    OnceDownloadImage = false;
                }
            }
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
    private void CallOnceInFrame(string _imageUrl)
    {
        if (!OnceDownloadImage)
        {
            DownloadImage(_imageUrl);
        }
    }
    private void onClickPrivacyDetails()
    {
        AccountSelectedIcons[0].SetActive(false);
        AccountSelectedIcons[1].SetActive(false);
        AccountSelectedIcons[2].SetActive(false);
        AccountSelectedIcons[3].SetActive(false);
        AccountSelectedIcons[4].SetActive(true);
        AccountSelectedIcons[5].SetActive(false);
        AccountSelectedIcons[6].SetActive(false);

        Panel_MyAccount.SetActive(true);
        SubPanel_PersonalDetails.SetActive(false);
        SubPanel_KycDocuments.SetActive(false);
        SubPanel_PanDocument.SetActive(false);
        SubPanel_AadharDocument.SetActive(false);
        SubPanel_DrivingLicense.SetActive(false);
        SubPanel_VoterCard.SetActive(false);
        SubPanel_PassportNumber.SetActive(false);
        SubPanel_BankDetails.SetActive(false);
        SubPanel_ChangeAvatar.SetActive(false);
        SubPanel_PrivacyPolicy.SetActive(true);
        SubPanel_TermsConditions.SetActive(false);
        SubPanel_AccountOverview.SetActive(false);
    }
    private void onClickTermsDetails()
    {
        AccountSelectedIcons[0].SetActive(false);
        AccountSelectedIcons[1].SetActive(false);
        AccountSelectedIcons[2].SetActive(false);
        AccountSelectedIcons[3].SetActive(false);
        AccountSelectedIcons[4].SetActive(false);
        AccountSelectedIcons[5].SetActive(true);
        AccountSelectedIcons[6].SetActive(false);

        Panel_MyAccount.SetActive(true);
        SubPanel_PersonalDetails.SetActive(false);
        SubPanel_KycDocuments.SetActive(false);
        SubPanel_PanDocument.SetActive(false);
        SubPanel_AadharDocument.SetActive(false);
        SubPanel_DrivingLicense.SetActive(false);
        SubPanel_VoterCard.SetActive(false);
        SubPanel_PassportNumber.SetActive(false);
        SubPanel_BankDetails.SetActive(false);
        SubPanel_ChangeAvatar.SetActive(false);
        SubPanel_PrivacyPolicy.SetActive(false);
        SubPanel_TermsConditions.SetActive(true);
        SubPanel_AccountOverview.SetActive(false);
    }
    private void onClickAccountOverviewDetails()
    {
        AccountSelectedIcons[0].SetActive(false);
        AccountSelectedIcons[1].SetActive(false);
        AccountSelectedIcons[2].SetActive(false);
        AccountSelectedIcons[3].SetActive(false);
        AccountSelectedIcons[4].SetActive(false);
        AccountSelectedIcons[5].SetActive(false);
        AccountSelectedIcons[6].SetActive(true);

        Panel_MyAccount.SetActive(true);
        SubPanel_PersonalDetails.SetActive(false);
        SubPanel_KycDocuments.SetActive(false);
        SubPanel_PanDocument.SetActive(false);
        SubPanel_AadharDocument.SetActive(false);
        SubPanel_DrivingLicense.SetActive(false);
        SubPanel_VoterCard.SetActive(false);
        SubPanel_PassportNumber.SetActive(false);
        SubPanel_BankDetails.SetActive(false);
        SubPanel_ChangeAvatar.SetActive(false);
        SubPanel_PrivacyPolicy.SetActive(false);
        SubPanel_TermsConditions.SetActive(false);
        SubPanel_AccountOverview.SetActive(true);
    }
    private void onMyAccountBtn()
    {
        Panel_MyAccount.SetActive(true);
        SubPanel_PersonalDetails.SetActive(false);
        SubPanel_KycDocuments.SetActive(false);
        SubPanel_PanDocument.SetActive(false);
        SubPanel_AadharDocument.SetActive(false);
        SubPanel_DrivingLicense.SetActive(false);
        SubPanel_VoterCard.SetActive(false);
        SubPanel_PassportNumber.SetActive(false);
        SubPanel_BankDetails.SetActive(false);
        SubPanel_ChangeAvatar.SetActive(false);
        SubPanel_PrivacyPolicy.SetActive(false);
        SubPanel_TermsConditions.SetActive(false);
        SubPanel_AccountOverview.SetActive(true);

        AccountSelectedIcons[0].SetActive(false);
        AccountSelectedIcons[1].SetActive(false);
        AccountSelectedIcons[2].SetActive(false);
        AccountSelectedIcons[3].SetActive(false);
        AccountSelectedIcons[4].SetActive(false);
        AccountSelectedIcons[5].SetActive(false);
        AccountSelectedIcons[6].SetActive(true);

        
    }
    
    private void onKYCSubmitBtn()
    {
        //call api
        if (!string.IsNullOrEmpty(PanEnterCardNo.text))
        {
            if (PanEnterCardNo.text.Length >= 10)
            {
                if (!string.IsNullOrEmpty(PanUploadedImage.text))
                {
                    if (StaticValues.AddressType == "Aadhar")
                    {

                        if (!string.IsNullOrEmpty(AadharCardEnterCardNo.text))
                        {
                            if (!string.IsNullOrEmpty(AadharUploadedImage.text))
                            {
                                AppManager.VIEW_CONTROLLER.ShowLoading();
                                StaticValues.PanDocNo = PanEnterCardNo.text;
                                StaticValues.AddressNo = AadharCardEnterCardNo.text;
                                PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, AadharCardEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                            }
                            else
                            {
                                PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                            }
                        }
                        else
                        {
                            PlayerSave.singleton.ShowErrorMessage("Please enter your Aadhar card no.");
                        }
                    }
                    else if (StaticValues.AddressType == "License")
                    {
                        if (!string.IsNullOrEmpty(LicenseEnterCardNo.text))
                        {
                            if (!string.IsNullOrEmpty(LicenseUploadedImage.text))
                            {
                                AppManager.VIEW_CONTROLLER.ShowLoading();
                                StaticValues.PanDocNo = PanEnterCardNo.text;
                                StaticValues.AddressNo = LicenseEnterCardNo.text;
                                PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, LicenseEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                            }
                            else
                            {
                                PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                            }
                        }
                        else
                        {
                            PlayerSave.singleton.ShowErrorMessage("Please enter your License no.");
                        }
                    }
                    else if (StaticValues.AddressType == "Voter")
                    {
                        if (!string.IsNullOrEmpty(VoterCardEnterCardNo.text))
                        {
                            if (!string.IsNullOrEmpty(VoterCardUploadedImage.text))
                            {
                                AppManager.VIEW_CONTROLLER.ShowLoading();
                                StaticValues.PanDocNo = PanEnterCardNo.text;
                                StaticValues.AddressNo = VoterCardEnterCardNo.text;
                                PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, VoterCardEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                            }
                            else
                            {
                                PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                            }
                        }
                        else
                        {
                            PlayerSave.singleton.ShowErrorMessage("Please enter your Voter card no.");
                        }
                    }
                    else if (StaticValues.AddressType == "Passport")
                    {
                        if (!string.IsNullOrEmpty(PassportEnterCardNo.text))
                        {
                            if (!string.IsNullOrEmpty(PassportUploadedImage.text))
                            {
                                AppManager.VIEW_CONTROLLER.ShowLoading();
                                StaticValues.PanDocNo = PanEnterCardNo.text;
                                StaticValues.AddressNo = PassportEnterCardNo.text;
                                PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, PassportEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                            }
                            else
                            {
                                PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                            }
                        }
                        else
                        {
                            PlayerSave.singleton.ShowErrorMessage("Please enter your Passport no.");
                        }
                    }
                    else
                    {
                        PlayerSave.singleton.ShowErrorMessage("Please select your AddressProof");
                    }
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                }
            }
            else
            {
                PlayerSave.singleton.ShowErrorMessage("Please enter your correct Pan Card No");
            }
        }
        else
        {
            PlayerSave.singleton.ShowErrorMessage("Please enter your Pan Card No");
        }
    }
    private void OnKYCUpdateResponse(ServerKYCDetailsResponse serverKYCDetailsResponse)
    {
        if(serverKYCDetailsResponse!=null)
        {
            if(serverKYCDetailsResponse.status.Contains("200"))
            {
                AppManager.VIEW_CONTROLLER.HideLoading();
                //PlayerSave.singleton.ShowErrorMessage(serverKYCDetailsResponse.message);
                if(serverKYCDetailsResponse.data!=null)
                {
                    StaticValues.PanCardStatus = serverKYCDetailsResponse.data.pancardstatus;
                    StaticValues.AddressStatus = serverKYCDetailsResponse.data.addressp_status;
                    onClickKYCDetails();
                    AadharUploadedImage.text = "";
                    PassportUploadedImage.text = "";
                    PanUploadedImage.text = "";
                    LicenseUploadedImage.text = "";
                    VoterCardUploadedImage.text = "";
                    PanEnterCardNo.text = "";
                    AadharCardEnterCardNo.text = "";
                    PassportEnterCardNo.text = "";
                    LicenseEnterCardNo.text = "";
                    VoterCardEnterCardNo.text = "";
                    
                    
                }
                
            }
            else
            {
                AppManager.VIEW_CONTROLLER.HideLoading();
                PlayerSave.singleton.ShowErrorMessage(serverKYCDetailsResponse.message);
            }
        }
        else
        {
            AppManager.VIEW_CONTROLLER.HideLoading();
            PlayerSave.singleton.ShowErrorMessage(serverKYCDetailsResponse.message);
        }
    }
    private void onKYCAadharCard()
    {

        KYCAadharCardSelection.SetActive(true);
        KYCLicenseSelection.SetActive(false);
        KYCPassportSelection.SetActive(false);
        KYCVoterCardSelection.SetActive(false);

        CancelInvoke("onKYCAadharCardAfterOne");
        CancelInvoke("onKYCLicenseAfterOne");
        CancelInvoke("onKYCPassportAfterOne");
        CancelInvoke("onKYCVoterCardAfterOne");
        CancelInvoke("onKYCPanBaseAfterOne");
        Invoke("onKYCAadharCardAfterOne", 1f);
    }
    private void onKYCAadharCardAfterOne()
    {
        Panel_MyAccount.SetActive(true);
        SubPanel_PersonalDetails.SetActive(false);
        SubPanel_KycDocuments.SetActive(false);
        SubPanel_PanDocument.SetActive(false);
        SubPanel_AadharDocument.SetActive(true);
        SubPanel_DrivingLicense.SetActive(false);
        SubPanel_VoterCard.SetActive(false);
        SubPanel_PassportNumber.SetActive(false);
        SubPanel_BankDetails.SetActive(false);
        SubPanel_ChangeAvatar.SetActive(false);
        SubPanel_PrivacyPolicy.SetActive(false);
        SubPanel_TermsConditions.SetActive(false);
        SubPanel_AccountOverview.SetActive(false);
    }
    private void onKYCLicense()
    {

        KYCAadharCardSelection.SetActive(false);
        KYCLicenseSelection.SetActive(true);
        KYCPassportSelection.SetActive(false);
        KYCVoterCardSelection.SetActive(false);



        CancelInvoke("onKYCAadharCardAfterOne");
        CancelInvoke("onKYCLicenseAfterOne");
        CancelInvoke("onKYCPassportAfterOne");
        CancelInvoke("onKYCVoterCardAfterOne");
        CancelInvoke("onKYCPanBaseAfterOne");
        Invoke("onKYCLicenseAfterOne", 1f);
    }
    private void onKYCLicenseAfterOne()
    {
        Panel_MyAccount.SetActive(true);
        SubPanel_PersonalDetails.SetActive(false);
        SubPanel_KycDocuments.SetActive(false);
        SubPanel_PanDocument.SetActive(false);
        SubPanel_AadharDocument.SetActive(false);
        SubPanel_DrivingLicense.SetActive(true);
        SubPanel_VoterCard.SetActive(false);
        SubPanel_PassportNumber.SetActive(false);
        SubPanel_BankDetails.SetActive(false);
        SubPanel_ChangeAvatar.SetActive(false);
        SubPanel_PrivacyPolicy.SetActive(false);
        SubPanel_TermsConditions.SetActive(false);
        SubPanel_AccountOverview.SetActive(false);
    }
    private void onKYCPassport()
    {

        KYCAadharCardSelection.SetActive(false);
        KYCLicenseSelection.SetActive(false);
        KYCPassportSelection.SetActive(true);
        KYCVoterCardSelection.SetActive(false);



        CancelInvoke("onKYCAadharCardAfterOne");
        CancelInvoke("onKYCLicenseAfterOne");
        CancelInvoke("onKYCPassportAfterOne");
        CancelInvoke("onKYCVoterCardAfterOne");
        CancelInvoke("onKYCPanBaseAfterOne");
        Invoke("onKYCPassportAfterOne", 1f);
    }
    private void onKYCPassportAfterOne()
    {
        Panel_MyAccount.SetActive(true);
        SubPanel_PersonalDetails.SetActive(false);
        SubPanel_KycDocuments.SetActive(false);
        SubPanel_PanDocument.SetActive(false);
        SubPanel_AadharDocument.SetActive(false);
        SubPanel_DrivingLicense.SetActive(false);
        SubPanel_VoterCard.SetActive(false);
        SubPanel_PassportNumber.SetActive(true);
        SubPanel_BankDetails.SetActive(false);
        SubPanel_ChangeAvatar.SetActive(false);
        SubPanel_PrivacyPolicy.SetActive(false);
        SubPanel_TermsConditions.SetActive(false);
        SubPanel_AccountOverview.SetActive(false);
    }
    private void onKYCVoterCard()
    {
        KYCAadharCardSelection.SetActive(false);
        KYCLicenseSelection.SetActive(false);
        KYCPassportSelection.SetActive(false);
        KYCVoterCardSelection.SetActive(true);



        CancelInvoke("onKYCAadharCardAfterOne");
        CancelInvoke("onKYCLicenseAfterOne");
        CancelInvoke("onKYCPassportAfterOne");
        CancelInvoke("onKYCVoterCardAfterOne");
        CancelInvoke("onKYCPanBaseAfterOne");
        Invoke("onKYCVoterCardAfterOne", 1f);
    }
    private void onKYCVoterCardAfterOne()
    {
        Panel_MyAccount.SetActive(true);
        SubPanel_PersonalDetails.SetActive(false);
        SubPanel_KycDocuments.SetActive(false);
        SubPanel_PanDocument.SetActive(false);
        SubPanel_AadharDocument.SetActive(false);
        SubPanel_DrivingLicense.SetActive(false);
        SubPanel_VoterCard.SetActive(true);
        SubPanel_PassportNumber.SetActive(false);
        SubPanel_BankDetails.SetActive(false);
        SubPanel_ChangeAvatar.SetActive(false);
        SubPanel_PrivacyPolicy.SetActive(false);
        SubPanel_TermsConditions.SetActive(false);
        SubPanel_AccountOverview.SetActive(false);
    }
    private void SetKYCDetailsPage()
    {
        PanBaseSelected.SetActive(false);
        AddressBaseSelected.SetActive(false);

        PanForwardArrow.SetActive(true);
        PanDownwardArrow.SetActive(false);

        if (StaticValues.PanCardStatus == 0)
        {
            PanNotVerified.SetActive(true);
            PanStatusPending.SetActive(false);
            PanVerified.SetActive(false);
        }
        else if (StaticValues.PanCardStatus == 1 || StaticValues.PanCardStatus >2)
        { 
            PanNotVerified.SetActive(false);
            PanStatusPending.SetActive(true);
            PanVerified.SetActive(false);
        }
        else if (StaticValues.PanCardStatus == 2)
        {
            PanNotVerified.SetActive(false);
            PanStatusPending.SetActive(false);
            PanVerified.SetActive(true);
        }

        AddressForwardArrow.SetActive(true);
        AddressDownwardArrow.SetActive(false);

        if (StaticValues.AddressStatus == 0)
        {
            AddressNotVerified.SetActive(true);
            AddressStatusPending.SetActive(false);
            AddressVerified.SetActive(false);
        }
        else if (StaticValues.AddressStatus == 1 || StaticValues.AddressStatus > 2)
        {
            AddressNotVerified.SetActive(false);
            AddressStatusPending.SetActive(true);
            AddressVerified.SetActive(false);
        }
        else if (StaticValues.AddressStatus == 2)
        {
            AddressNotVerified.SetActive(false);
            AddressStatusPending.SetActive(false);
            AddressVerified.SetActive(true);
        }

        KYCDropDownBase.SetActive(false);

        CancelInvoke("onKYCAadharCardAfterOne");
        CancelInvoke("onKYCLicenseAfterOne");
        CancelInvoke("onKYCPassportAfterOne");
        CancelInvoke("onKYCVoterCardAfterOne");
        CancelInvoke("onKYCPanBaseAfterOne");
    }
    private void onPanBaseButton()
    {
        PanBaseSelected.SetActive(true);
        AddressBaseSelected.SetActive(false);

        PanForwardArrow.SetActive(true);
        PanDownwardArrow.SetActive(false);

        if (StaticValues.PanCardStatus == 0)
        {
            PanNotVerified.SetActive(true);
            PanStatusPending.SetActive(false);
            PanVerified.SetActive(false);
        }
        else if (StaticValues.PanCardStatus == 1 || StaticValues.PanCardStatus > 2)
        {
            PanNotVerified.SetActive(false);
            PanStatusPending.SetActive(true);
            PanVerified.SetActive(false);
        }
        else if (StaticValues.PanCardStatus == 2)
        {
            PanNotVerified.SetActive(false);
            PanStatusPending.SetActive(false);
            PanVerified.SetActive(true);
        }

        AddressForwardArrow.SetActive(true);
        AddressDownwardArrow.SetActive(false);

        if (StaticValues.AddressStatus == 0)
        {
            AddressNotVerified.SetActive(true);
            AddressStatusPending.SetActive(false);
            AddressVerified.SetActive(false);
        }
        else if (StaticValues.AddressStatus == 1 || StaticValues.AddressStatus > 2)
        {
            AddressNotVerified.SetActive(false);
            AddressStatusPending.SetActive(true);
            AddressVerified.SetActive(false);
        }
        else if (StaticValues.AddressStatus == 2)
        {
            AddressNotVerified.SetActive(false);
            AddressStatusPending.SetActive(false);
            AddressVerified.SetActive(true);
        }

        KYCDropDownBase.SetActive(false);

        CancelInvoke("onKYCAadharCardAfterOne");
        CancelInvoke("onKYCLicenseAfterOne");
        CancelInvoke("onKYCPassportAfterOne");
        CancelInvoke("onKYCVoterCardAfterOne");
        CancelInvoke("onKYCPanBaseAfterOne");
        Invoke("onKYCPanBaseAfterOne", 1f);

    }
    private void onKYCPanBaseAfterOne()
    {
        Panel_MyAccount.SetActive(true);
        SubPanel_PersonalDetails.SetActive(false);
        SubPanel_KycDocuments.SetActive(false);
        SubPanel_PanDocument.SetActive(true);
        SubPanel_AadharDocument.SetActive(false);
        SubPanel_DrivingLicense.SetActive(false);
        SubPanel_VoterCard.SetActive(false);
        SubPanel_PassportNumber.SetActive(false);
        SubPanel_BankDetails.SetActive(false);
        SubPanel_ChangeAvatar.SetActive(false);
        SubPanel_PrivacyPolicy.SetActive(false);
        SubPanel_TermsConditions.SetActive(false);
        SubPanel_AccountOverview.SetActive(false);

    }
    private void onAddressBaseButton()
    {
        PanBaseSelected.SetActive(false);
        AddressBaseSelected.SetActive(true);

        AddressForwardArrow.SetActive(false);
        AddressDownwardArrow.SetActive(true);
        if (StaticValues.PanCardStatus == 0)
        {
            PanNotVerified.SetActive(true);
            PanStatusPending.SetActive(false);
            PanVerified.SetActive(false);
        }
        else if (StaticValues.PanCardStatus == 1 || StaticValues.PanCardStatus > 2)
        {
            PanNotVerified.SetActive(false);
            PanStatusPending.SetActive(true);
            PanVerified.SetActive(false);
        }
        else if (StaticValues.PanCardStatus == 2)
        {
            PanNotVerified.SetActive(false);
            PanStatusPending.SetActive(false);
            PanVerified.SetActive(true);
        }

        if (StaticValues.AddressStatus == 0)
        {
            AddressNotVerified.SetActive(true);
            AddressStatusPending.SetActive(false);
            AddressVerified.SetActive(false);
        }
        else if (StaticValues.AddressStatus == 1 || StaticValues.AddressStatus > 2)
        {
            AddressNotVerified.SetActive(false);
            AddressStatusPending.SetActive(true);
            AddressVerified.SetActive(false);
        }
        else if (StaticValues.AddressStatus == 2)
        {
            AddressNotVerified.SetActive(false);
            AddressStatusPending.SetActive(false);
            AddressVerified.SetActive(true);
        }

        KYCDropDownBase.SetActive(true);

        KYCAadharCardSelection.SetActive(false);
        KYCLicenseSelection.SetActive(false);
        KYCPassportSelection.SetActive(false);
        KYCVoterCardSelection.SetActive(false);
    }
    public void FillPersonalDetails()
    {
        if (string.IsNullOrEmpty(StaticValues.MobileValue))
        {
            StaticValues.MobileValue = StaticValues.phoneNumberWithoutPrefix;
        }
        PDEnterFirstName.text = StaticValues.FirstNameValue;
        PDEnterLastName.text = StaticValues.LastNameValue;
        PDEnterMobileNo.text = StaticValues.phoneNumberWithoutPrefix;
        PDEnterEmail.text = StaticValues.Email;
        PDEnterPinCode.text = StaticValues.PinCodeValue;
        PDEnterState.text = StaticValues.StateValue;
        PDEnterStreetNo_1.text = StaticValues.StreetValue_1;
        PDEnterStreetNo_2.text = StaticValues.StreetValue_2;
        if (StaticValues.GenderValue == "Male" || StaticValues.GenderValue == "MALE" || StaticValues.GenderValue == "male")
        {
            PDGender.value = 0;
        }
        else if (StaticValues.GenderValue == "Female" || StaticValues.GenderValue == "female" || StaticValues.GenderValue == "FEMALE")
        {
            PDGender.value = 1;
        }
        else if (StaticValues.GenderValue == "Other" || StaticValues.GenderValue == "OTHER" || StaticValues.GenderValue == "other")
        {
            PDGender.value = 2;
        }
        PDEnterDob.text = StaticValues.DOBValue;
        PDEnterCity.text = StaticValues.CityValue;
        PDEnterPinCode.text = StaticValues.PinCodeValue;

        if (string.IsNullOrEmpty(PDEnterFirstName.text))
        {
            if (!string.IsNullOrEmpty(StaticValues.displayNameinUC))
            {
                string[] subtokens = StaticValues.displayNameinUC.Split(new char[] { ' ' });

                if (subtokens.Length > 0)
                {
                    StaticValues.FirstNameValue = subtokens[0];
                }

                if (subtokens.Length > 1)
                {
                    StaticValues.LastNameValue = subtokens[1];
                }
                PDEnterFirstName.text = StaticValues.FirstNameValue;
                PDEnterLastName.text = StaticValues.LastNameValue;
            }
        }
        if (!string.IsNullOrEmpty(PDEnterMobileNo.text))
        {
            if (StaticValues.isMobileVerify)
            {
                PDEnterMobileNo.interactable = false;
            }
            else
            {
                PDEnterMobileNo.interactable = true;
            }
        }
        else
        {
            PDEnterMobileNo.interactable = true;
        }
        if (!string.IsNullOrEmpty(PDEnterEmail.text))
        {
            if (StaticValues.isEmailVerify)
            {
                PDEnterEmail.interactable = false;
            }
            else
            {
                PDEnterEmail.interactable = true;
            }
        }
        else
        {
            PDEnterEmail.interactable = true;
        }
        if (!string.IsNullOrEmpty(PDEnterFirstName.text) && !string.IsNullOrEmpty(PDEnterLastName.text) && !string.IsNullOrEmpty(PDEnterMobileNo.text) && !string.IsNullOrEmpty(PDEnterEmail.text) && !string.IsNullOrEmpty(PDEnterPinCode.text) && !string.IsNullOrEmpty(PDEnterState.text) && !string.IsNullOrEmpty(PDEnterStreetNo_1.text) && !string.IsNullOrEmpty(PDEnterStreetNo_2.text) && !string.IsNullOrEmpty(PDEnterCity.text) && !string.IsNullOrEmpty(PDEnterPinCode.text) && !string.IsNullOrEmpty(PDEnterState.text) && !string.IsNullOrEmpty(PDEnterDob.text))
        {
            PDEnterFirstName.interactable = false;
            PDEnterLastName.interactable = false;
            PDEnterMobileNo.interactable = false;
            PDEnterEmail.interactable = false;
            PDEnterPinCode.interactable = false;
            PDEnterState.interactable = false;
            PDEnterStreetNo_1.interactable = false;
            PDEnterStreetNo_2.interactable = false;
            PDSaveButton.interactable = false;
            PDEnterCity.interactable = false;
            PDGender.interactable = false;
            PDEnterDob.interactable = false;
            PDEnterDOBBtn.interactable = false;
        }
    }
    public void FillKYCDetails()
    {

    }
    private void OnEnable()
    {
        if (PlayerSave.singleton != null)
        {
            PlayerSave.singleton.GetUserWalletDetails(PlayerSave.singleton.newID());
        }
    }
    private void Update()
    {
        if (PlayerSave.singleton != null)
        {
            RefreshMoneyUI();
        }
        if (DateValueUpdated)
        {
            DateValueUpdated = false;
            if (PDEnterDob) PDEnterDob.text = selectedDate.ToString("yyyy-MM-dd");
        }


        if (string.IsNullOrEmpty(StaticValues.UserNameValue))
        {
            return;
        }
        if (PlayerSave.singleton != null)
        {
            if (!Canvas_AddCash.activeSelf && !Panel_MyAccount.activeSelf && !Panel_BonusCash.activeSelf && !Panel_Orientation.activeSelf && !Panel_ReferAFriend.activeSelf && !Panel_Withdraw.activeSelf && !Panel_AddCashWebView.activeSelf && !Panel_BonusPopUp.activeSelf)
            {
                if (StaticValues.getBannerImageDetails != null)
                {
                    if (StaticValues.getBannerImageDetails.Count > 0)
                    {
                        for (int i = 0; i < StaticValues.getBannerImageDetails.Count; i++)
                        {
                            if (StaticValues.getBannerImageDetails[i].extraBannerCheck)
                            {
                                if (!string.IsNullOrEmpty(StaticValues.getBannerImageDetails[i].extralbanner_url) && PlayerSave.singleton.ReadTimestamp(StaticValues.getBannerImageDetails[i].couponcode))
                                {
                                    if (PlayerSave.singleton.TimerisOnOrOff() && string.IsNullOrEmpty(StaticValues.CurrentBonusCode))
                                    {
                                        if (!string.IsNullOrEmpty(StaticValues.getBannerImageDetails[i].couponcode))
                                        {
                                            if (StaticValues.getBannerImageDetails[i].isDay)
                                            {
                                                StaticValues.CurrentBonusCode = StaticValues.getBannerImageDetails[i].couponcode;
                                                try
                                                {
                                                    MyBonusIndex = StaticValues.getBannerImageDetails.FindIndex(x => x.couponcode.Contains(StaticValues.CurrentBonusCode));
                                                }
                                                catch
                                                {

                                                }
                                                if (StaticValues.getBannerImageDetails[i].sprite1 == null)
                                                {
                                                    StopCoroutine(OnOfferImageLoadGraphic(StaticValues.getBannerImageDetails[i].extralbanner_url));
                                                    StartCoroutine(OnOfferImageLoadGraphic(StaticValues.getBannerImageDetails[i].extralbanner_url));
                                                }
                                                else
                                                {
                                                    Panel_BonusPopUp.SetActive(true);
                                                    OfferPopUp.sprite = StaticValues.getBannerImageDetails[i].sprite1;
                                                    OfferPopUp.rectTransform.sizeDelta = new Vector2(StaticValues.getBannerImageDetails[i].sprite1.rect.width, StaticValues.getBannerImageDetails[i].sprite1.rect.height);
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
        }


        if(amountInputField!=null)
        {
            if(amountInputField.isFocused)
            {
                //if(AddCashBg_1)AddCashBg_1.GetComponent<RectTransform>().anchoredPosition = new Vector2(202.71f, 220f);
                //StaticValues.CurrentAddCashIndex = 3;
                //if(InputBoxSelection[0])InputBoxSelection[0].SetActive(false);
                //if (InputBoxSelection[1]) InputBoxSelection[1].SetActive(false);
                //if (InputBoxSelection[2]) InputBoxSelection[2].SetActive(false);
                //if (InputBoxSelection[3]) InputBoxSelection[3].SetActive(true);
            }
            else
            {
                if (AddCashBg_1) AddCashBg_1.GetComponent<RectTransform>().anchoredPosition = new Vector2(202.71f, -78.6f);
            }
        }
    }
    private IEnumerator OnOfferImageLoadGraphic(string _url2)
    {
        string _url = PlayerSave.singleton.BaseAPI + "" + _url2;
        if (!string.IsNullOrEmpty(_url) && (_url.StartsWith("http") || _url.StartsWith("file")))
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(_url);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Panel_BonusPopUp.SetActive(false);
                Debug.Log(www.error);
            }
            else
            {
                Texture2D _texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                if (_texture != null)
                {
                    Panel_BonusPopUp.SetActive(true);
                    OfferPopUp.sprite = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                    OfferPopUp.rectTransform.sizeDelta = new Vector2(_texture.width, _texture.height);
                }
            }
        }
    }

    public void SetBonusPopupOff()
    {
        Panel_BonusPopUp.SetActive(false);
        if(PlayerSave.singleton!=null)
        {
            PlayerSave.singleton.ScheduleTimer(StaticValues.CurrentBonusCode);
        }
        StaticValues.CurrentBonusCode = string.Empty;
    }
    public void RefreshCoins()
    {
        if (PlayerSave.singleton != null)
        {
            PlayerSave.singleton.GetUserWalletDetails(PlayerSave.singleton.newID());
        }
        if (PlayerSave.singleton != null)
        {
            isDebug = PlayerSave.singleton.debug;
        }
    }
    public void RefreshMoneyUI()
    {
        if (monetPlayer)
        {
            monetPlayer.text = PlayerSave.singleton.GetCurrentMoney().ToString("F2");
        }
        if (withdrawText)
        {
            withdrawText.text = StaticValues.WithdrawEarningCount.ToString();
        }
        if (depositText)
        {
            depositText.text = StaticValues.DepositEarningCount.ToString();
        }
        if (promoText)
        {
            promoText.text = StaticValues.PromoEarningCount.ToString();
        }
        if (addCashmoneyPlayer)
        {
            addCashmoneyPlayer.text = PlayerSave.singleton.GetCurrentMoney().ToString("F2");
        }
        if (chipsPlayer)
        {
            chipsPlayer.text = PlayerSave.singleton.GetCurrentChips().ToString("F2") + " chips";
        }
    }

    public void OnLogOut()
    {
        PlayerPrefs.DeleteAll();
        if (PlayerSave.singleton != null)
        {
            PlayerSave.singleton.ResetData();
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void OnSettingsContactUsButton()
    {
        SendEmail();
    }
    void SendEmail()
    {
        string email = "tamashakhel@gmail.com";
        string subject = MyEscapeURL("KhelTamasha Feedback " + StaticValues.phoneNumberWithoutPrefix.ToString());
        string body = MyEscapeURL("Please\r\nprovide your feedback here...");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }
    string MyEscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }
    public void TakePicture()
    {
        if (NativeGallery.CheckPermission(NativeGallery.PermissionType.Read,NativeGallery.MediaType.Image) != NativeGallery.Permission.Granted)
        {
            NativeGallery.RequestPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);
        }
        TakePicture(256);
    }
    public void PickPicture()
    {
        if (NativeGallery.CheckPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image) != NativeGallery.Permission.Granted)
        {
            NativeGallery.RequestPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);
        }
        PickImage(256);
    }
    private void OnUploadImage()
    {
        ChangeAvatarSaveButton.interactable = true;

        Texture2D texture = ConvertSpriteToTexture(ChangeAvatarImage.sprite);
        if (texture != null)
        {
            byte[] uploadBytes = ImageConversion.EncodeToJPG(texture, 128);
            string savePic = System.Convert.ToBase64String(uploadBytes);
            if (PlayerSave.singleton != null)
            {
                StaticValues.avatarPicUrl = savePic;
                PlayerSave.singleton.SavePic(savePic);
                ChangeAvatarSaveButton.interactable = true;
            }
        }
        if (!string.IsNullOrEmpty(StaticValues.avatarPicUrl))
        {
            AppManager.VIEW_CONTROLLER.ShowLoading();
            PlayerSave.singleton.UpdateAvatarAPICall(StaticValues.FirebaseUserId, StaticValues.avatarPicUrl, OnUploadImageResponse);
        }
        else
        {
            //PlayerSave.singleton.ShowErrorMessage("Please upload image before proceed!");

            
        }
    }
    private void OnUploadImageResponse(ServerUserDetailsResponse serverUserDetailsResponse)
    {
        AppManager.VIEW_CONTROLLER.HideLoading();
        if (serverUserDetailsResponse!=null)
        {
            if(serverUserDetailsResponse.status.Contains("200"))
            {
                OnceDownloadImage = false;
                //PlayerSave.singleton.ShowErrorMessage("Avatar Pic Updated!");
                //onClickAvatarDetails();
            }
            else
            {
                PlayerSave.singleton.ShowErrorMessage(serverUserDetailsResponse.message);
            }
        }
        else
        {
            PlayerSave.singleton.ShowErrorMessage("Error : Avatar Pic");
        }
        
    }
    public void TakePicture(int maxSize)
    {

        try
        {
            if (NativeCamera.IsCameraBusy())
            {
                PlayerSave.singleton.ShowErrorMessage("Please Allow sometime to proceed!!!");
                return;
            }

            NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
            {
                if (isDebug)
                {
                    Debug.Log("Image path: " + path);
                }
                if (path != null)
                {
                // Create a Texture2D from the captured image
                Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize, false, false);
                    if (texture == null)
                    {
                        if (isDebug)
                        {
                            Debug.Log("Couldn't load texture from " + path);
                        }
                        PlayerSave.singleton.ShowErrorMessage("Couldn't load texture from " + path);
                        return;
                    }
                    if (PlayerSave.singleton != null)
                    {
                        PlayerSave.singleton.playerTexture = texture;
                        ChangeAvatarImage.sprite = Sprite.Create(PlayerSave.singleton.playerTexture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                    }


                //byte[] uploadBytes = ImageConversion.EncodeToJPG(texture, 128);
                //string savePic = System.Convert.ToBase64String(uploadBytes);
                //if (PlayerSave.singleton != null)
                //{
                //    StaticValues.avatarPicUrl = savePic;
                //    PlayerSave.singleton.SavePic(savePic);
                //    ChangeAvatarSaveButton.interactable = true;
                //}

            }
            }, maxSize);
        }
        catch
        {

        }

        //Debug.Log("Permission result: " + permission);
    }
    private void PickImage(int maxSize)
    {

        // Don't attempt to pick media from Gallery/Photos if
        // another media pick operation is already in progress
        if (NativeGallery.IsMediaPickerBusy())
        {
            PlayerSave.singleton.ShowErrorMessage("Please Allow sometime to proceed!!!");
            return;
        }

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (isDebug)
            {
                Debug.Log("Image path: " + path);
            }
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize, false, false);
                if (texture == null)
                {
                    if (isDebug)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                    }
                    PlayerSave.singleton.ShowErrorMessage("Couldn't load texture from " + path);
                    return;
                }
                if (PlayerSave.singleton != null)
                {
                    PlayerSave.singleton.playerTexture = texture;
                    ChangeAvatarImage.sprite = Sprite.Create(PlayerSave.singleton.playerTexture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                }

                //byte[] uploadBytes = ImageConversion.EncodeToJPG(texture, 128);
                //string savePic = System.Convert.ToBase64String(uploadBytes);
                //if (PlayerSave.singleton != null)
                //{
                //    StaticValues.avatarPicUrl = savePic;
                //    PlayerSave.singleton.SavePic(savePic);
                //    ChangeAvatarSaveButton.interactable = true;
                //}
                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
                //Destroy(texture, 5f);
            }
        }, "Select a PNG image", "image/png");

        if (isDebug)
        {
            Debug.Log("Permission result: " + permission);
        }
    }
    private void UploadAadharDocumentsImage(int maxSize)
    {

        // Don't attempt to pick media from Gallery/Photos if
        // another media pick operation is already in progress
        if (NativeGallery.IsMediaPickerBusy())
        {
            PlayerSave.singleton.ShowErrorMessage("Please Allow sometime to proceed!!!");
            return;
        }

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (isDebug)
            {
                Debug.Log("Image path: " + path);
            }
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize, false, false);
                if (texture == null)
                {
                    if (isDebug)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                    }
                    return;
                }

                if (texture != null)
                {
                    byte[] uploadBytes = ImageConversion.EncodeToJPG(texture, 256);
                    StaticValues.SaveAddressDocumentPic = System.Convert.ToBase64String(uploadBytes);
                    StaticValues.AddressType = "Aadhar";
                    AadharUploadedImage.text = Path.GetFileName(path).ToUpperInvariant();
                }

                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
                //Destroy(texture, 5f);
            }
        }, "Select a PNG image", "image/png");

        if (isDebug)
        {
            Debug.Log("Permission result: " + permission);
        }
    }
    private void UploadVoterCardDocumentsImage(int maxSize)
    {

        // Don't attempt to pick media from Gallery/Photos if
        // another media pick operation is already in progress
        if (NativeGallery.IsMediaPickerBusy())
        {
            PlayerSave.singleton.ShowErrorMessage("Please Allow sometime to proceed!!!");
            return;
        }

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (isDebug)
            {
                Debug.Log("Image path: " + path);
            }
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize, false, false);
                if (texture == null)
                {
                    if (isDebug)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                    }
                    return;
                }

                if (texture != null)
                {
                    byte[] uploadBytes = ImageConversion.EncodeToJPG(texture, 256);
                    StaticValues.SaveAddressDocumentPic = System.Convert.ToBase64String(uploadBytes);
                    StaticValues.AddressType = "Voter";
                    VoterCardUploadedImage.text = Path.GetFileName(path).ToUpperInvariant();
                }

                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
                //Destroy(texture, 5f);
            }
        }, "Select a PNG image", "image/png");

        if (isDebug)
        {
            Debug.Log("Permission result: " + permission);
        }
    }
    private void UploadLicenseDocumentsImage(int maxSize)
    {

        // Don't attempt to pick media from Gallery/Photos if
        // another media pick operation is already in progress
        if (NativeGallery.IsMediaPickerBusy())
        {
            PlayerSave.singleton.ShowErrorMessage("Please Allow sometime to proceed!!!");
            return;
        }

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (isDebug)
            {
                Debug.Log("Image path: " + path);
            }
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize, false, false);
                if (texture == null)
                {
                    if (isDebug)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                    }
                    return;
                }

                if (texture != null)
                {
                    byte[] uploadBytes = ImageConversion.EncodeToJPG(texture, 256);
                    StaticValues.SaveAddressDocumentPic = System.Convert.ToBase64String(uploadBytes);
                    StaticValues.AddressType = "License";
                    LicenseUploadedImage.text = Path.GetFileName(path).ToUpperInvariant();
                }

                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
                //Destroy(texture, 5f);
            }
        }, "Select a PNG image", "image/png");

        if (isDebug)
        {
            Debug.Log("Permission result: " + permission);
        }
    }
    private void UploadPassportDocumentsImage(int maxSize)
    {

        // Don't attempt to pick media from Gallery/Photos if
        // another media pick operation is already in progress
        if (NativeGallery.IsMediaPickerBusy())
        {
            PlayerSave.singleton.ShowErrorMessage("Please Allow sometime to proceed!!!");
            return;
        }

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (isDebug)
            {
                Debug.Log("Image path: " + path);
            }
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize,false,false);
                if (texture == null)
                {
                    if (isDebug)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                    }
                    return;
                }

                if (texture != null)
                {
                    byte[] uploadBytes = ImageConversion.EncodeToJPG(texture, 256);
                    StaticValues.SaveAddressDocumentPic = System.Convert.ToBase64String(uploadBytes);
                    StaticValues.AddressType = "Passport";
                    PassportUploadedImage.text = Path.GetFileName(path).ToUpperInvariant();
                }

                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
                //Destroy(texture, 5f);
            }
        }, "Select a PNG image", "image/png");

        if (isDebug)
        {
            Debug.Log("Permission result: " + permission);
        }
    }
    private void UploadPanDocumentsImage(int maxSize)
    {

        // Don't attempt to pick media from Gallery/Photos if
        // another media pick operation is already in progress
        if (NativeGallery.IsMediaPickerBusy())
        {
            PlayerSave.singleton.ShowErrorMessage("Please Allow sometime to proceed!!!");
            return;
        }

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (isDebug)
            {
                Debug.Log("Image path: " + path);
            }
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize, false, false);
                if (texture == null)
                {
                    if (isDebug)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                    }
                    return;
                }

                if (texture != null)
                {
                    byte[] uploadBytes = ImageConversion.EncodeToJPG(texture, 256);
                    StaticValues.SavePancardDocumentPic = System.Convert.ToBase64String(uploadBytes);
                    PanUploadedImage.text = Path.GetFileName(path).ToUpperInvariant();
                }

                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
                //Destroy(texture, 5f);
            }
        }, "Select a PNG image", "image/png");

        if (isDebug)
        {
            Debug.Log("Permission result: " + permission);
        }
    }
    private string pdfFileType;

    public void PdfChecker()
    {
        pdfFileType = NativeFilePicker.ConvertExtensionToFileType("pdf"); // Returns "application/pdf" on Android and "com.adobe.pdf" on iOS
        if (isDebug)
        {
            Debug.Log("pdf's MIME/UTI is: " + pdfFileType);
        }
    }
    public void TakePdfFile()
    {

        if (NativeFilePicker.CheckPermission() != NativeFilePicker.Permission.Granted)
        {
            NativeFilePicker.RequestPermission();
        }

        // Don't attempt to import/export files if the file picker is already open
        if (NativeFilePicker.IsFilePickerBusy())
        {
            PlayerSave.singleton.ShowErrorMessage("Please Allow sometime to proceed!!!");
            return;
        }


        // Pick a PDF file
        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
                Debug.Log("Operation cancelled");
            else
                Debug.Log("Picked file: " + path);
        }, new string[] { pdfFileType });

        if (isDebug)
        {
            Debug.Log("Permission result: " + permission);
        }



    }
    public void TakeMultipleFiles()
    {
        if (NativeFilePicker.CheckPermission() != NativeFilePicker.Permission.Granted)
        {
            NativeFilePicker.RequestPermission();
        }

        if (NativeFilePicker.IsFilePickerBusy())
        {
            PlayerSave.singleton.ShowErrorMessage("Please Allow sometime to proceed!!!");
            return;
        }


#if UNITY_ANDROID
        // Use MIMEs on Android
        string[] fileTypes = new string[] { "image/*", pdfFileType };// "video/*" };
#else
			// Use UTIs on iOS
			string[] fileTypes = new string[] { "public.image", "public.movie" };
#endif

        // Pick image(s) and/or pdf
        NativeFilePicker.Permission permission = NativeFilePicker.PickMultipleFiles((paths) =>
        {
            if (paths == null)
                Debug.Log("Operation cancelled");
            else
            {
                for (int i = 0; i < paths.Length; i++)
                    Debug.Log("Picked file: " + paths[i]);
            }
        }, fileTypes);

        if (isDebug)
        {
            Debug.Log("Permission result: " + permission);
        }
    }
    
    private void OnValueChangedPDEnterFirstName(string _value)
    {
        PDEnterFirstName.text = _value.Replace(" ", string.Empty);
    }
    private void OnEndEditPDEnterFirstName(string _value)
    {
        PDEnterFirstName.text = _value.Replace(" ", string.Empty);
    }
    private void OnEndEditPDEnterLastName(string _value)
    {
        PDEnterLastName.text = _value.Replace(" ", string.Empty);
    }
    private void OnValueChangedPDEnterLastName(string _value)
    {
        PDEnterLastName.text = _value.Replace(" ", string.Empty);
    }
    private void OnValueChangedPDEnterMobileNo(string _value)
    {
        PDEnterMobileNo.text = _value.Replace(" ", string.Empty);
    }
    private void OnEndEditPDEnterMobileNo(string _value)
    {
        PDEnterMobileNo.text = _value.Replace(" ", string.Empty);
    }
    private void OnValueChangedPDEnterEmail(string _value)
    {
        PDEnterEmail.text = _value.Replace(" ", string.Empty);
    }
    private void OnEndEditPDEnterEmail(string _value)
    {
        PDEnterEmail.text = _value.Replace(" ", string.Empty);
    }
    private void OnValueChangedPDEnterDob(string _value)
    {
        PDEnterDob.text = _value.Replace(" ", string.Empty);
    }
    private void OnEndEditPDEnterDob(string _value)
    {
        PDEnterDob.text = _value.Replace(" ", string.Empty);
    }
    private void OnValueChangedPDEnterStreetNo_1(string _value)
    {
        //PDEnterStreetNo_1.text = _value.Replace(" ", string.Empty);
    }
    private void OnEndEditPDEnterStreetNo_1(string _value)
    {
        //PDEnterStreetNo_1.text = _value.Replace(" ", string.Empty);
    }
    private void OnValueChangedPDEnterStreetNo_2(string _value)
    {
        //PDEnterStreetNo_2.text = _value.Replace(" ", string.Empty);
    }
    private void OnEndEditPDEnterStreetNo_2(string _value)
    {
        //PDEnterStreetNo_2.text = _value.Replace(" ", string.Empty);
    }
    private void OnValueChangedPDEnterCity(string _value)
    {
        //PDEnterCity.text = _value.Replace(" ", string.Empty);
    }
    private void OnEndEditPDEnterCity(string _value)
    {
        //PDEnterCity.text = _value.Replace(" ", string.Empty);
    }
    private void OnValueChangedPDEnterState(string _value)
    {
        //PDEnterState.text = _value.Replace(" ", string.Empty);
    }
    private void OnEndEditPDEnterState(string _value)
    {
        //PDEnterState.text = _value.Replace(" ", string.Empty);
    }
    private void OnValueChangedPDEnterPinCode(string _value)
    {
        PDEnterPinCode.text = _value.Replace(" ", string.Empty);
    }
    private void OnEndEditPDEnterPinCode(string _value)
    {
        PDEnterPinCode.text = _value.Replace(" ", string.Empty);
    }
    public void HandleInputData(int val)
    {
        if (isDebug)
        {
            Debug.Log("val " + val);
        }
        if (val == 0)
        {
            dropDownOutput = "Male";
            StaticValues.GenderValue = dropDownOutput;
        }
        else if (val == 1)
        {
            dropDownOutput = "Female";
            StaticValues.GenderValue = dropDownOutput;
        }
        else if (val == 2)
        {
            dropDownOutput = "Other";
            StaticValues.GenderValue = dropDownOutput;
        }
        if (isDebug)
        {
            Debug.Log("StaticValues.GenderValue " + StaticValues.GenderValue);
        }
    }
    private void onClickPDEnterDOBBtn()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        long maxDate = 0;
         long minDate = 0;
 
         // set min date
         DateTime dateMin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
         minDate = TimeUtils.totalMilliseconds(dateMin); // convert date to mills long
         // set max date
         DateTime dateMax = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, 0, 0, 0, DateTimeKind.Utc);
         maxDate = TimeUtils.totalMilliseconds(dateMax); // convert date to mills long
 
        var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            AndroidJavaObject datePickerDialog = new AndroidJavaObject("android.app.DatePickerDialog", activity, new DateCallback(), selectedDate.Year,  selectedDate.Month-1, selectedDate.Day);

                 AndroidJavaObject datePicker = datePickerDialog.Call<AndroidJavaObject>("getDatePicker");
                 datePicker.Call("setMaxDate", maxDate);
                 datePicker.Call("setMinDate", minDate);
                 datePickerDialog.Call("show");
        }));
#else
        selectedDate = DateTime.Now;
      
        DateValueUpdated = true;
#endif
    }
    public const string MatchEmailPattern =
       @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
       + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
       + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
       + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
    public bool validateEmail(string email)
    {
        if (email != null)
            return Regex.IsMatch(email, MatchEmailPattern);
        else
            return false;
    }
    private void OnUpdateProfileResponse(ServerUserDetailsResponse serverUserDetailsResponse)
    {
        if(serverUserDetailsResponse!=null)
        {
            if(serverUserDetailsResponse.status.Contains("200"))
            {
                AppManager.VIEW_CONTROLLER.HideLoading();
                //PlayerSave.singleton.ShowErrorMessage(serverUserDetailsResponse.message);
                FillPersonalDetails();

                //if (!string.IsNullOrEmpty(StaticValues.phoneNumberWithoutPrefix))
                //{
                //    AppManager.FIREBASE_CONTROLLER.LinkWithProvider("phone");
                    
                //}
            }
            else
            {
                AppManager.VIEW_CONTROLLER.HideLoading();
                PlayerSave.singleton.ShowErrorMessage(serverUserDetailsResponse.message);
            }
        }
        else
        {
            AppManager.VIEW_CONTROLLER.HideLoading();
            PlayerSave.singleton.ShowErrorMessage("Error");
        }
    }
    private void onClickPDSaveButton()
    {
        if (!string.IsNullOrEmpty(PDEnterFirstName.text))
        {
            if (!string.IsNullOrEmpty(PDEnterLastName.text))
            {
                if (!string.IsNullOrEmpty(PDEnterMobileNo.text))
                {
                    if (PDEnterMobileNo.text.Length >= 10)
                    {
                        if (!string.IsNullOrEmpty(PDEnterEmail.text))
                        {
                            if (validateEmail(PDEnterEmail.text))
                            {
                                if (!string.IsNullOrEmpty(PDEnterDob.text))
                                {
                                    if (!string.IsNullOrEmpty(PDEnterStreetNo_1.text) && !string.IsNullOrEmpty(PDEnterStreetNo_2.text))
                                    {
                                        if (!string.IsNullOrEmpty(PDEnterCity.text))
                                        {
                                            if (!string.IsNullOrEmpty(PDEnterState.text))
                                            {
                                                if (!string.IsNullOrEmpty(PDEnterPinCode.text))
                                                {
                                                    if(PDEnterPinCode.text.Length>=6)
                                                    {
                                                        //call API
                                                        AppManager.VIEW_CONTROLLER.ShowLoading();

                                                        PlayerSave.singleton.UpdateProfileAPICall(StaticValues.FirebaseUserId, PDEnterFirstName.text, PDEnterLastName.text, StaticValues.GenderValue, PDEnterMobileNo.text, PDEnterEmail.text, PDEnterDob.text, PDEnterStreetNo_1.text, PDEnterStreetNo_2.text, PDEnterCity.text, PDEnterState.text, PDEnterPinCode.text, OnUpdateProfileResponse);
                                                    }
                                                    else
                                                    {
                                                        PlayerSave.singleton.ShowErrorMessage("Please enter your pin code with min 6 characters");
                                                    }
                                                   
                                                }
                                                else
                                                {
                                                    PlayerSave.singleton.ShowErrorMessage("Please enter your pin code");
                                                }
                                            }
                                            else
                                            {
                                                PlayerSave.singleton.ShowErrorMessage("Please enter your state");
                                            }
                                        }
                                        else
                                        {
                                            PlayerSave.singleton.ShowErrorMessage("Please enter your city");
                                        }
                                    }
                                    else
                                    {
                                        PlayerSave.singleton.ShowErrorMessage("Please enter your street no.");
                                    }
                                }
                                else
                                {
                                    PlayerSave.singleton.ShowErrorMessage("Please enter your dob");
                                }
                            }
                            else
                            {
                                PlayerSave.singleton.ShowErrorMessage("Please enter your valid email Id");
                            }
                        }
                        else
                        {
                            PlayerSave.singleton.ShowErrorMessage("Please enter your email Id");
                        }
                    }
                    else
                    {
                        PlayerSave.singleton.ShowErrorMessage("Please enter your valid mobile number");
                    }
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter your mobile number");
                }
            }
            else
            {
                PlayerSave.singleton.ShowErrorMessage("Please enter your last name");
            }
        }
        else
        {
            PlayerSave.singleton.ShowErrorMessage("Please enter your first name");
        }
    }


    private static DateTime selectedDate = DateTime.Now;
    private static bool DateValueUpdated = false;

    class DateCallback : AndroidJavaProxy
    {
        public DateCallback() : base("android.app.DatePickerDialog$OnDateSetListener") { }
        void onDateSet(AndroidJavaObject view, int year, int monthOfYear, int dayOfMonth)
        {
            selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            DateValueUpdated = true;
        }
    }
    private void onPanUploadButton()
    {
        UploadPanDocumentsImage(512);
    }
    private void onPanSaveButton()
    {
        if (!string.IsNullOrEmpty(PanUploadedImage.text))
        {
            if (!string.IsNullOrEmpty(PanEnterCardNo.text))
            {
                if (PanEnterCardNo.text.Length >= 10)
                {
                    if (!string.IsNullOrEmpty(PanUploadedImage.text))
                    {

                        AppManager.VIEW_CONTROLLER.ShowLoading();
                        StaticValues.PanDocNo = PanEnterCardNo.text;
                        StaticValues.AddressNo = AadharCardEnterCardNo.text;
                        PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, AadharCardEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                    }
                    else
                    {
                        PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                    }
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter your correct Pan Card No!!!");
                }
            }
            else
            {
                PlayerSave.singleton.ShowErrorMessage("Please enter your Pan Card No");
            }
        }
        else
        {
            PlayerSave.singleton.ShowErrorMessage("Please upload file to proceed!");
        }
    }
    private void onAadharCardUploadButton()
    {
        UploadAadharDocumentsImage(512);
    }
    private void onAadharCardSaveButton()
    {
        if (!string.IsNullOrEmpty(AadharUploadedImage.text))
        {
           
                        if (StaticValues.AddressType == "Aadhar")
                        {

                            if (!string.IsNullOrEmpty(AadharCardEnterCardNo.text))
                            {
                                if (!string.IsNullOrEmpty(AadharUploadedImage.text))
                                {
                                    AppManager.VIEW_CONTROLLER.ShowLoading();
                                    StaticValues.PanDocNo = PanEnterCardNo.text;
                                    StaticValues.AddressNo = AadharCardEnterCardNo.text;
                                    PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, AadharCardEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                                }
                                else
                                {
                                    PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                                }
                            }
                            else
                            {
                                PlayerSave.singleton.ShowErrorMessage("Please enter your Aadhar card no.");
                            }
                        }
                        else if (StaticValues.AddressType == "License")
                        {
                            if (!string.IsNullOrEmpty(LicenseEnterCardNo.text))
                            {
                                if (!string.IsNullOrEmpty(LicenseUploadedImage.text))
                                {
                                    AppManager.VIEW_CONTROLLER.ShowLoading();
                                    StaticValues.PanDocNo = PanEnterCardNo.text;
                                    StaticValues.AddressNo = LicenseEnterCardNo.text;
                                    PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, LicenseEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                                }
                                else
                                {
                                    PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                                }
                            }
                            else
                            {
                                PlayerSave.singleton.ShowErrorMessage("Please enter your License no.");
                            }
                        }
                        else if (StaticValues.AddressType == "Voter")
                        {
                            if (!string.IsNullOrEmpty(VoterCardEnterCardNo.text))
                            {
                                if (!string.IsNullOrEmpty(VoterCardUploadedImage.text))
                                {
                                    AppManager.VIEW_CONTROLLER.ShowLoading();
                                    StaticValues.PanDocNo = PanEnterCardNo.text;
                                    StaticValues.AddressNo = VoterCardEnterCardNo.text;
                                    PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, VoterCardEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                                }
                                else
                                {
                                    PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                                }
                            }
                            else
                            {
                                PlayerSave.singleton.ShowErrorMessage("Please enter your Voter card no.");
                            }
                        }
                        else if (StaticValues.AddressType == "Passport")
                        {
                            if (!string.IsNullOrEmpty(PassportEnterCardNo.text))
                            {
                                if (!string.IsNullOrEmpty(PassportUploadedImage.text))
                                {
                                    AppManager.VIEW_CONTROLLER.ShowLoading();
                                    StaticValues.PanDocNo = PanEnterCardNo.text;
                                    StaticValues.AddressNo = PassportEnterCardNo.text;
                                    PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, PassportEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                                }
                                else
                                {
                                    PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                                }
                            }
                            else
                            {
                                PlayerSave.singleton.ShowErrorMessage("Please enter your Passport no.");
                            }
                        }
                        else
                        {
                            PlayerSave.singleton.ShowErrorMessage("Please select your AddressProof");
                        }
        }
        else
        {
            PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
        }
                
                
    }
    private void onLicenseUploadButton()
    {
        UploadLicenseDocumentsImage(512);
    }
    private void onLicenseSaveButton()
    {
        if (!string.IsNullOrEmpty(LicenseUploadedImage.text))
        {
            if (StaticValues.AddressType == "Aadhar")
            {

                if (!string.IsNullOrEmpty(AadharCardEnterCardNo.text))
                {
                    if (!string.IsNullOrEmpty(AadharUploadedImage.text))
                    {
                        AppManager.VIEW_CONTROLLER.ShowLoading();
                        StaticValues.PanDocNo = PanEnterCardNo.text;
                        StaticValues.AddressNo = AadharCardEnterCardNo.text;
                        PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, AadharCardEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                    }
                    else
                    {
                        PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                    }
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter your Aadhar card no.");
                }
            }
            else if (StaticValues.AddressType == "License")
            {
                if (!string.IsNullOrEmpty(LicenseEnterCardNo.text))
                {
                    if (!string.IsNullOrEmpty(LicenseUploadedImage.text))
                    {
                        AppManager.VIEW_CONTROLLER.ShowLoading();
                        StaticValues.PanDocNo = PanEnterCardNo.text;
                        StaticValues.AddressNo = LicenseEnterCardNo.text;
                        PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, LicenseEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                    }
                    else
                    {
                        PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                    }
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter your License no.");
                }
            }
            else if (StaticValues.AddressType == "Voter")
            {
                if (!string.IsNullOrEmpty(VoterCardEnterCardNo.text))
                {
                    if (!string.IsNullOrEmpty(VoterCardUploadedImage.text))
                    {
                        AppManager.VIEW_CONTROLLER.ShowLoading();
                        StaticValues.PanDocNo = PanEnterCardNo.text;
                        StaticValues.AddressNo = VoterCardEnterCardNo.text;
                        PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, VoterCardEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                    }
                    else
                    {
                        PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                    }
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter your Voter card no.");
                }
            }
            else if (StaticValues.AddressType == "Passport")
            {
                if (!string.IsNullOrEmpty(PassportEnterCardNo.text))
                {
                    if (!string.IsNullOrEmpty(PassportUploadedImage.text))
                    {
                        AppManager.VIEW_CONTROLLER.ShowLoading();
                        StaticValues.PanDocNo = PanEnterCardNo.text;
                        StaticValues.AddressNo = PassportEnterCardNo.text;
                        PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, PassportEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                    }
                    else
                    {
                        PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                    }
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter your Passport no.");
                }
            }
            else
            {
                PlayerSave.singleton.ShowErrorMessage("Please select your AddressProof");
            }
        }
        else
        {
            PlayerSave.singleton.ShowErrorMessage("Please upload file to proceed!");
        }
    }
    private void onPassportUploadButton()
    {
        UploadPassportDocumentsImage(512);
    }
    private void onPassportSaveButton()
    {
        if (!string.IsNullOrEmpty(PassportUploadedImage.text))
        {
            if (StaticValues.AddressType == "Aadhar")
            {

                if (!string.IsNullOrEmpty(AadharCardEnterCardNo.text))
                {
                    if (!string.IsNullOrEmpty(AadharUploadedImage.text))
                    {
                        AppManager.VIEW_CONTROLLER.ShowLoading();
                        StaticValues.PanDocNo = PanEnterCardNo.text;
                        StaticValues.AddressNo = AadharCardEnterCardNo.text;
                        PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, AadharCardEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                    }
                    else
                    {
                        PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                    }
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter your Aadhar card no.");
                }
            }
            else if (StaticValues.AddressType == "License")
            {
                if (!string.IsNullOrEmpty(LicenseEnterCardNo.text))
                {
                    if (!string.IsNullOrEmpty(LicenseUploadedImage.text))
                    {
                        AppManager.VIEW_CONTROLLER.ShowLoading();
                        StaticValues.PanDocNo = PanEnterCardNo.text;
                        StaticValues.AddressNo = LicenseEnterCardNo.text;
                        PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, LicenseEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                    }
                    else
                    {
                        PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                    }
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter your License no.");
                }
            }
            else if (StaticValues.AddressType == "Voter")
            {
                if (!string.IsNullOrEmpty(VoterCardEnterCardNo.text))
                {
                    if (!string.IsNullOrEmpty(VoterCardUploadedImage.text))
                    {
                        AppManager.VIEW_CONTROLLER.ShowLoading();
                        StaticValues.PanDocNo = PanEnterCardNo.text;
                        StaticValues.AddressNo = VoterCardEnterCardNo.text;
                        PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, VoterCardEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                    }
                    else
                    {
                        PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                    }
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter your Voter card no.");
                }
            }
            else if (StaticValues.AddressType == "Passport")
            {
                if (!string.IsNullOrEmpty(PassportEnterCardNo.text))
                {
                    if (!string.IsNullOrEmpty(PassportUploadedImage.text))
                    {
                        AppManager.VIEW_CONTROLLER.ShowLoading();
                        StaticValues.PanDocNo = PanEnterCardNo.text;
                        StaticValues.AddressNo = PassportEnterCardNo.text;
                        PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, PassportEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                    }
                    else
                    {
                        PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                    }
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter your Passport no.");
                }
            }
            else
            {
                PlayerSave.singleton.ShowErrorMessage("Please select your AddressProof");
            }
        }
        else
        {
            PlayerSave.singleton.ShowErrorMessage("Please upload file to proceed!");
        }
    }
    private void onVoterCardUploadButton()
    {
        UploadVoterCardDocumentsImage(512);
    }
    private void onVoterCardSaveButton()
    {
        if (!string.IsNullOrEmpty(VoterCardUploadedImage.text))
        {
            if (StaticValues.AddressType == "Aadhar")
            {

                if (!string.IsNullOrEmpty(AadharCardEnterCardNo.text))
                {
                    if (!string.IsNullOrEmpty(AadharUploadedImage.text))
                    {
                        AppManager.VIEW_CONTROLLER.ShowLoading();
                        StaticValues.PanDocNo = PanEnterCardNo.text;
                        StaticValues.AddressNo = AadharCardEnterCardNo.text;
                        PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, AadharCardEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                    }
                    else
                    {
                        PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                    }
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter your Aadhar card no.");
                }
            }
            else if (StaticValues.AddressType == "License")
            {
                if (!string.IsNullOrEmpty(LicenseEnterCardNo.text))
                {
                    if (!string.IsNullOrEmpty(LicenseUploadedImage.text))
                    {
                        AppManager.VIEW_CONTROLLER.ShowLoading();
                        StaticValues.PanDocNo = PanEnterCardNo.text;
                        StaticValues.AddressNo = LicenseEnterCardNo.text;
                        PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, LicenseEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                    }
                    else
                    {
                        PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                    }
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter your License no.");
                }
            }
            else if (StaticValues.AddressType == "Voter")
            {
                if (!string.IsNullOrEmpty(VoterCardEnterCardNo.text))
                {
                    if (!string.IsNullOrEmpty(VoterCardUploadedImage.text))
                    {
                        AppManager.VIEW_CONTROLLER.ShowLoading();
                        StaticValues.PanDocNo = PanEnterCardNo.text;
                        StaticValues.AddressNo = VoterCardEnterCardNo.text;
                        PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, VoterCardEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                    }
                    else
                    {
                        PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                    }
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter your Voter card no.");
                }
            }
            else if (StaticValues.AddressType == "Passport")
            {
                if (!string.IsNullOrEmpty(PassportEnterCardNo.text))
                {
                    if (!string.IsNullOrEmpty(PassportUploadedImage.text))
                    {
                        AppManager.VIEW_CONTROLLER.ShowLoading();
                        StaticValues.PanDocNo = PanEnterCardNo.text;
                        StaticValues.AddressNo = PassportEnterCardNo.text;
                        PlayerSave.singleton.UpdateKYCInfoCall(StaticValues.FirebaseUserId, PanEnterCardNo.text, StaticValues.AddressType, PassportEnterCardNo.text, StaticValues.SavePancardDocumentPic, StaticValues.SaveAddressDocumentPic, OnKYCUpdateResponse);
                    }
                    else
                    {
                        PlayerSave.singleton.ShowErrorMessage("Please upload your file to proceed!!!");
                    }
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter your Passport no.");
                }
            }
            else
            {
                PlayerSave.singleton.ShowErrorMessage("Please select your AddressProof");
            }
        }
        else
        {
            PlayerSave.singleton.ShowErrorMessage("Please upload file to proceed!");
        }
    }
    public string censor(string text,string word)
    {

        // Break down sentence by ' ' spaces 
        // and store each individual word in 
        // a different list 
        string[] word_list = text.Split(' ');

        // A new string to store the result 
        string result = "";

        // Creating the censor which is an asterisks 
        // "*" text of the length of censor word 
        string stars = "";
        for (int i = 0; i < word.Length; i++)
            stars += '*';

        // Iterating through our list 
        // of extracted words 
        int index = 0;
        foreach (string i in word_list)
        {
            if (i.CompareTo(word) == 0)

                // changing the censored word to 
                // created asterisks censor 
                word_list[index] = stars;
            index++;
        }

        // join the words 
        foreach (string i in word_list)
            result += i + " ";

        return result;
    }
    private void OnUpdateBankDetailsResponse(ServerBankDetailsResponse serverBankDetailsResponse)
    {
        if (serverBankDetailsResponse != null)
        {
            if (serverBankDetailsResponse.status.Contains("200"))
            {
                AppManager.VIEW_CONTROLLER.HideLoading();
                //PlayerSave.singleton.ShowErrorMessage(serverBankDetailsResponse.message);
                if (!string.IsNullOrEmpty(StaticValues.BankAccountNo))
                {
                    if (StaticValues.BankAccountNo.Length >0)
                    {
                        //string newString = new string('*', (StaticValues.BankAccountNo.Length - 4));
                        BankEnterAccountNo.text = StaticValues.BankAccountNo;// newString + StaticValues.BankAccountNo.Substring(StaticValues.BankAccountNo.Length-4, 4 );
                    }
                }
                else
                {
                    BankEnterAccountNo.text = "";
                }
                if (!string.IsNullOrEmpty(StaticValues.BankIFSCCode))
                {
                    if (StaticValues.BankIFSCCode.Length >0)
                    {
                        //string newString = new string('*', (StaticValues.BankIFSCCode.Length - 4));
                        BankEnterIFSCCode.text = StaticValues.BankIFSCCode;// newString + StaticValues.BankIFSCCode.Substring(StaticValues.BankIFSCCode.Length - 4, 4);
                    }
                }
                else
                {
                    BankEnterIFSCCode.text = "";
                }
                if (!string.IsNullOrEmpty(StaticValues.BankAccountNo))
                {
                    if (StaticValues.BankAccountNo.Length >0)
                    {
                        //string newString = new string('*', (StaticValues.BankAccountNo.Length - 4));
                        BankReEnterAccountNo.text = StaticValues.BankAccountNo;// newString + StaticValues.BankAccountNo.Substring(StaticValues.BankAccountNo.Length - 4, 4);
                    }
                }
                else
                {
                    BankReEnterAccountNo.text = "";
                }
               
                if(BankNewRequestButton) BankNewRequestButton.gameObject.SetActive(true);
                if (BankSubmitButton) BankSubmitButton.gameObject.SetActive(false);
                if (BankNewRequestStatus_NR) BankNewRequestStatus_NR.text = "";
                BankNewRequestOptions.SetActive(false);
                BankEnterAccountNo.interactable = false;
                BankReEnterAccountNo.interactable = false;
                BankEnterIFSCCode.interactable = false;
                BankEnterAccountNo_NR.text = "";
                BankReEnterAccountNo_NR.text = "";
                BankEnterIFSCCode_NR.text = "";
              
                if (serverBankDetailsResponse.data != null)
                {
                    StaticValues.isBankDetailsSubmitted = serverBankDetailsResponse.data.isBankDetailsSubmitted;
                    StaticValues.isBankStatusForNewRequest = serverBankDetailsResponse.data.isBankStatusForNewRequest;
                    StaticValues.BankAccountNo = serverBankDetailsResponse.data.Account_no;
                    StaticValues.BankAccountNo_NR = serverBankDetailsResponse.data.NewAccount_no;
                    StaticValues.BankIFSCCode = serverBankDetailsResponse.data.back_ifsc_code;
                    StaticValues.BankIFSCCode_NR = serverBankDetailsResponse.data.Newback_ifsc_code;
                    
                }
                onClickBankDetails();
				PopupMessage msg = new PopupMessage();
				msg.Title = "";
				msg.Message = serverBankDetailsResponse.message;
				if(!string.IsNullOrEmpty(StaticValues.isBankStatusForNewRequest))
				{
					AppManager.VIEW_CONTROLLER.ShowBankPopupMSG(msg,null,1);
				}
				else
				{
					AppManager.VIEW_CONTROLLER.ShowBankPopupMSG(msg,RedirectToWithdrawNow,0);
				}
            }
            else
            {
                AppManager.VIEW_CONTROLLER.HideLoading();
                //PlayerSave.singleton.ShowErrorMessage(serverBankDetailsResponse.message);
                BankEnterAccountNo.text = "";
                BankEnterIFSCCode.text = "";
                StaticValues.BankAccountNo = "";
                StaticValues.BankIFSCCode = "";
				PopupMessage msg = new PopupMessage();
				msg.Title = "";
				msg.Message = serverBankDetailsResponse.message;
				AppManager.VIEW_CONTROLLER.ShowBankPopupMSG(msg,null,1);

            }
        }
        else
        {
            AppManager.VIEW_CONTROLLER.HideLoading();
            //PlayerSave.singleton.ShowErrorMessage("Error");
            BankEnterAccountNo.text = "";
            BankEnterIFSCCode.text = "";
            StaticValues.BankAccountNo = "";
            StaticValues.BankIFSCCode = "";
			PopupMessage msg = new PopupMessage();
			msg.Title = "";
			msg.Message = "Some error occured.Please try after some time!";
			AppManager.VIEW_CONTROLLER.ShowBankPopupMSG(msg,null,1);
        }

        if (BankTranferToggle != null)
        {
            if (BankTranferToggle.isOn)
            {
                if (EnterBankObject) EnterBankObject.SetActive(true);
                if (EnterUPIObject) EnterUPIObject.SetActive(false);
            }
        }
        if (BankUPIToggle != null)
        {
            if (BankUPIToggle.isOn)
            {
                if (EnterBankObject) EnterBankObject.SetActive(false);
                if (EnterUPIObject) EnterUPIObject.SetActive(true);
            }
        }
    }
    public string RemoveIntegers(string input)
    {
        return Regex.Replace(input, @"[\d-]", string.Empty);
    }
    public void OnBankDetailsSubmitButton()
    {
        BankEnterAccountNo.text = BankEnterAccountNo.text.Replace(" ", string.Empty);
        BankReEnterAccountNo.text = BankReEnterAccountNo.text.Replace(" ", string.Empty);
        BankEnterIFSCCode.text = BankEnterIFSCCode.text.Replace(" ", string.Empty);
        if (!string.IsNullOrEmpty(BankEnterAccountNo.text))
        {
            if (BankEnterAccountNo.text.Length >= 10)
            {
                if (!string.IsNullOrEmpty(BankEnterAccountNo.text))
                {
                    if (!string.IsNullOrEmpty(BankReEnterAccountNo.text))
                    {
                        if (BankReEnterAccountNo.text.Length >= 10)
                        {
                            if (!string.IsNullOrEmpty(BankEnterIFSCCode.text))
                            {
                                if (BankEnterIFSCCode.text.Length >= 8)
                                {
                                    if (BankEnterAccountNo.text.Equals(BankReEnterAccountNo.text))
                                    {
                                        //call api
                                        AppManager.VIEW_CONTROLLER.ShowLoading();
                                        StaticValues.BankAccountNo = BankEnterAccountNo.text;
                                        StaticValues.BankIFSCCode = BankEnterIFSCCode.text;
                                        PlayerSave.singleton.UpdateBankInfoCall(StaticValues.FirebaseUserId, BankEnterAccountNo.text, BankEnterIFSCCode.text, RemoveIntegers(BankEnterIFSCCode.text),"1",OnUpdateBankDetailsResponse);
                                    }
                                    else
                                    {
                                        PlayerSave.singleton.ShowErrorMessage("Your account number mismatched.");
                                    }
                                }
                                else
                                {
                                    PlayerSave.singleton.ShowErrorMessage("Please enter your valid IFSC Code.");
                                }
                            }
                            else
                            {
								PlayerSave.singleton.ShowErrorMessage("Please enter your IFSC code.");
                            }
                        }
                        else
                        {
                            PlayerSave.singleton.ShowErrorMessage("Please re-enter your valid account number.");
                        }
                    }
                    else
                    {
                        PlayerSave.singleton.ShowErrorMessage("Please re-enter your account number.");
                    }
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter your valid account number.");
                }
            }
            else
            {
                PlayerSave.singleton.ShowErrorMessage("Please enter your account number.");
            }
        }

    }

    public void OnNewBankDetailsSubmitButton()
    {
        BankEnterAccountNo_NR.text = BankEnterAccountNo_NR.text.Replace(" ", string.Empty);
        BankReEnterAccountNo_NR.text = BankReEnterAccountNo_NR.text.Replace(" ", string.Empty);
        BankEnterIFSCCode_NR.text = BankEnterIFSCCode_NR.text.Replace(" ", string.Empty);
        if (!string.IsNullOrEmpty(BankEnterAccountNo_NR.text))
        {
            if (BankEnterAccountNo_NR.text.Length >= 10)
            {
                if (!string.IsNullOrEmpty(BankEnterAccountNo_NR.text))
                {
                    if (!string.IsNullOrEmpty(BankReEnterAccountNo_NR.text))
                    {
                        if (BankReEnterAccountNo_NR.text.Length >= 10)
                        {
                            if (!string.IsNullOrEmpty(BankEnterIFSCCode_NR.text))
                            {
                                if (BankEnterIFSCCode_NR.text.Length >= 8)
                                {
                                    if (BankEnterAccountNo_NR.text.Equals(BankReEnterAccountNo_NR.text))
                                    {
                                        //call api
                                        AppManager.VIEW_CONTROLLER.ShowLoading();
                                        StaticValues.BankAccountNo_NR = BankEnterAccountNo_NR.text;
                                        StaticValues.BankIFSCCode_NR = BankEnterIFSCCode_NR.text;
                                        PlayerSave.singleton.UpdateBankInfoCall(StaticValues.FirebaseUserId, BankEnterAccountNo_NR.text, BankEnterIFSCCode_NR.text, RemoveIntegers(BankEnterIFSCCode_NR.text),"2", OnUpdateNewBankDetailsResponse);
                                    }
                                    else
                                    {
										PlayerSave.singleton.ShowErrorMessage("Your account number mismatched.");
                                    }
                                }
                                else
                                {
                                    PlayerSave.singleton.ShowErrorMessage("Please enter your valid IFSC Code.");
                                }
                            }
                            else
                            {
								PlayerSave.singleton.ShowErrorMessage("Please enter your IFSC Code.");
                            }
                        }
                        else
                        {
                            PlayerSave.singleton.ShowErrorMessage("Please re-enter your valid account number.");
                        }
                    }
                    else
                    {
                        PlayerSave.singleton.ShowErrorMessage("Please re-enter your account number.");
                    }
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter your valid account number.");
                }
            }
            else
            {
                PlayerSave.singleton.ShowErrorMessage("Please enter your account number.");
            }
        }

    }
    public void OnBankDetailsNewRequestButton()
    {
        if (BankNewRequestOptions) BankNewRequestOptions.SetActive(true);
        if (BankSubmitButton) BankSubmitButton.gameObject.SetActive(false);
        if (BankNewRequestStatus_NR) BankNewRequestStatus_NR.text = "";
        BankRequestStatus.text = "";
        BankEnterAccountNo.interactable = false;
        BankReEnterAccountNo.interactable = false;
        BankEnterIFSCCode.interactable = false;
        BankEnterAccountNo_NR.text = "";
        BankReEnterAccountNo_NR.text = "";
        BankEnterIFSCCode_NR.text = "";
    }
    private void OnUpdateNewBankDetailsResponse(ServerBankDetailsResponse serverBankDetailsResponse)
    {
        if (serverBankDetailsResponse != null)
        {
            if (serverBankDetailsResponse.status.Contains("200"))
            {
                AppManager.VIEW_CONTROLLER.HideLoading();
                //PlayerSave.singleton.ShowErrorMessage(serverBankDetailsResponse.message);
                BankNewRequestStatus_NR.text = "<color=#FFFFFF>Status For New Bank Details Change is :</color> "+serverBankDetailsResponse.message;
                BankRequestStatus.text = "";
                if (!string.IsNullOrEmpty(StaticValues.BankAccountNo_NR))
                {
                    if (StaticValues.BankAccountNo_NR.Length > 0)
                    {
                        //string newString = new string('*', (StaticValues.BankAccountNo.Length - 4));
                        BankEnterAccountNo_NR.text = StaticValues.BankAccountNo_NR;// newString + StaticValues.BankAccountNo.Substring(StaticValues.BankAccountNo.Length-4, 4 );
                    }
                }
                else
                {
                    BankEnterAccountNo.text = "";
                }
                if (!string.IsNullOrEmpty(StaticValues.BankIFSCCode_NR))
                {
                    if (StaticValues.BankIFSCCode_NR.Length > 0)
                    {
                        //string newString = new string('*', (StaticValues.BankIFSCCode.Length - 4));
                        BankEnterIFSCCode_NR.text = StaticValues.BankIFSCCode_NR;// newString + StaticValues.BankIFSCCode.Substring(StaticValues.BankIFSCCode.Length - 4, 4);
                    }
                }
                else
                {
                    BankEnterIFSCCode_NR.text = "";
                }
                if (!string.IsNullOrEmpty(StaticValues.BankAccountNo_NR))
                {
                    if (StaticValues.BankAccountNo_NR.Length > 0)
                    {
                        //string newString = new string('*', (StaticValues.BankAccountNo.Length - 4));
                        BankReEnterAccountNo_NR.text = StaticValues.BankAccountNo_NR;// newString + StaticValues.BankAccountNo.Substring(StaticValues.BankAccountNo.Length - 4, 4);
                    }
                }
                else
                {
                    BankReEnterAccountNo_NR.text = "";
                }
                if (serverBankDetailsResponse.data != null)
                {
                    StaticValues.isBankDetailsSubmitted = serverBankDetailsResponse.data.isBankDetailsSubmitted;
                    StaticValues.isBankStatusForNewRequest = serverBankDetailsResponse.data.isBankStatusForNewRequest;
                    StaticValues.BankAccountNo = serverBankDetailsResponse.data.Account_no;
                    StaticValues.BankAccountNo_NR = serverBankDetailsResponse.data.NewAccount_no;
                    StaticValues.BankIFSCCode = serverBankDetailsResponse.data.back_ifsc_code;
                    StaticValues.BankIFSCCode_NR = serverBankDetailsResponse.data.Newback_ifsc_code;
                }
                onClickBankDetails();
				PopupMessage msg = new PopupMessage();
				msg.Title = "";
				msg.Message = serverBankDetailsResponse.message;
				if(!string.IsNullOrEmpty(StaticValues.isBankStatusForNewRequest))
				{
					AppManager.VIEW_CONTROLLER.ShowBankPopupMSG(msg,null,1);
				}
				else
				{
					AppManager.VIEW_CONTROLLER.ShowBankPopupMSG(msg,RedirectToWithdrawNow,0);
				}
            }
            else
            {
                AppManager.VIEW_CONTROLLER.HideLoading();
                //PlayerSave.singleton.ShowErrorMessage(serverBankDetailsResponse.message);
                BankEnterAccountNo_NR.text = "";
                BankEnterIFSCCode_NR.text = "";
                StaticValues.BankAccountNo_NR = "";
                StaticValues.BankIFSCCode_NR = "";
				PopupMessage msg = new PopupMessage();
				msg.Title = "";
				msg.Message = serverBankDetailsResponse.message;
				AppManager.VIEW_CONTROLLER.ShowBankPopupMSG(msg,null,1);

            }
        }
        else
        {
            AppManager.VIEW_CONTROLLER.HideLoading();
            //PlayerSave.singleton.ShowErrorMessage("Error");
            BankEnterAccountNo_NR.text = "";
            BankEnterIFSCCode_NR.text = "";
            StaticValues.BankAccountNo_NR = "";
            StaticValues.BankIFSCCode_NR = "";
			PopupMessage msg = new PopupMessage();
			msg.Title = "";
			msg.Message = "Some error occurred.Please try after some time!";
			AppManager.VIEW_CONTROLLER.ShowBankPopupMSG(msg,null,1);
        }
    }

    public void OnBankDetailsUPISubmitButton()
    {
        BankUPIEnterUPI.text = BankUPIEnterUPI.text.Replace(" ", string.Empty);
        if (!string.IsNullOrEmpty(BankUPIEnterUPI.text))
        {
            if (BankUPIEnterUPI.text.Length >= 6)
            {
                if (BankUPIEnterUPI.text.Contains("@"))
                {
                   
                                        //call api
                                        AppManager.VIEW_CONTROLLER.ShowLoading();
                                        StaticValues.BankUPIId = BankUPIEnterUPI.text;
                                        PlayerSave.singleton.UpdateBankInfoCall(StaticValues.FirebaseUserId, BankUPIEnterUPI.text, "", "", "11", OnUpdateUPIBankDetailsResponse);
                                    
                   
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter your valid UPI ID.");
                }
            }
            else
            {
                PlayerSave.singleton.ShowErrorMessage("Please enter your UPI ID.");
            }
        }
        else
        {
            PlayerSave.singleton.ShowErrorMessage("Please enter your UPI ID.");
        }

    }
    public void OnBankDetailsUPINewRequestButton()
    {
        if (BankUPINewRequestOptions) BankUPINewRequestOptions.SetActive(true);
        if (BankUPISubmitButton) BankUPISubmitButton.gameObject.SetActive(false);
        if (BankUPINewRequestStatus_NR) BankUPINewRequestStatus_NR.text = "";
        BankUPIEnterUPI.interactable = false;
        
        BankUPIEnterUPI_NR.text = "";
       
    }
    public void OnNewBankUPIDetailsSubmitButton()
    {
        BankUPIEnterUPI_NR.text = BankUPIEnterUPI_NR.text.Replace(" ", string.Empty);
       
        if (!string.IsNullOrEmpty(BankUPIEnterUPI_NR.text))
        {
            if (BankUPIEnterUPI_NR.text.Length >= 6)
            {
                if (BankUPIEnterUPI.text.Contains("@"))
                {
                    //call api
                    AppManager.VIEW_CONTROLLER.ShowLoading();
                    StaticValues.BankUPIId_NR = BankUPIEnterUPI_NR.text;

                    PlayerSave.singleton.UpdateBankInfoCall(StaticValues.FirebaseUserId, BankUPIEnterUPI_NR.text, "", "", "12", OnUpdateUPINewBankDetailsResponse);
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter your valid UPI ID.");
                }
                                    
                    
            }
            else
            { 
                    PlayerSave.singleton.ShowErrorMessage("Please enter your UPI ID.");
            }
           
        }
        else
        {
            PlayerSave.singleton.ShowErrorMessage("Please enter your UPI ID.");
        }

    }

    private void OnUpdateUPIBankDetailsResponse(ServerBankDetailsResponse serverBankDetailsResponse)
    {
        if (serverBankDetailsResponse != null)
        {
            if (serverBankDetailsResponse.status.Contains("200"))
            {
                AppManager.VIEW_CONTROLLER.HideLoading();
                //PlayerSave.singleton.ShowErrorMessage(serverBankDetailsResponse.message);
                if (!string.IsNullOrEmpty(StaticValues.BankUPIId))
                {
                    if (StaticValues.BankUPIId.Length > 0)
                    {
                        //string newString = new string('*', (StaticValues.BankAccountNo.Length - 4));
                        BankUPIEnterUPI.text = StaticValues.BankUPIId;// newString + StaticValues.BankAccountNo.Substring(StaticValues.BankAccountNo.Length-4, 4 );
                    }
                }
                else
                {
                    BankUPIEnterUPI.text = "";
                }
                if (BankUPINewRequestButton) BankUPINewRequestButton.gameObject.SetActive(true);
                if (BankUPISubmitButton) BankUPISubmitButton.gameObject.SetActive(false);
                if (BankUPINewRequestStatus_NR) BankUPINewRequestStatus_NR.text = "";
                BankUPINewRequestOptions.SetActive(false);
                BankUPIEnterUPI.interactable = false;
                
                BankUPIEnterUPI_NR.text = "";
                

                if (serverBankDetailsResponse.data != null)
                {
                    StaticValues.isBankUPIDetailsSubmitted = serverBankDetailsResponse.data.isUPIBankDetailsSubmitted;
                    StaticValues.isBankUPIStatusForNewRequest = serverBankDetailsResponse.data.BankUPIId_status;
                    StaticValues.BankUPIId = serverBankDetailsResponse.data.BankUPIId;
                    StaticValues.BankUPIId_NR = serverBankDetailsResponse.data.BankUPIId_NR;

                }
                onClickBankDetails();
				PopupMessage msg = new PopupMessage();
				msg.Title = "";
				msg.Message = serverBankDetailsResponse.message;
				if(!string.IsNullOrEmpty(StaticValues.isBankUPIStatusForNewRequest))
				{
					AppManager.VIEW_CONTROLLER.ShowBankPopupMSG(msg,null,1);
				}
				else
				{
					AppManager.VIEW_CONTROLLER.ShowBankPopupMSG(msg,RedirectToWithdrawNow,0);
				}
            }
            else
            {
                AppManager.VIEW_CONTROLLER.HideLoading();
                PlayerSave.singleton.ShowErrorMessage(serverBankDetailsResponse.message);
                BankUPIEnterUPI.text = "";
               
                StaticValues.BankUPIId = "";
				PopupMessage msg = new PopupMessage();
				msg.Title = "";
				msg.Message = serverBankDetailsResponse.message;
				AppManager.VIEW_CONTROLLER.ShowBankPopupMSG(msg,null,1);

            }
        }
        else
        {
            AppManager.VIEW_CONTROLLER.HideLoading();
            PlayerSave.singleton.ShowErrorMessage("Error");
            BankUPIEnterUPI.text = "";
           
            StaticValues.BankUPIId = "";
			PopupMessage msg = new PopupMessage();
			msg.Title = "";
			msg.Message = "Some error occurred.Please try after some time!";
			AppManager.VIEW_CONTROLLER.ShowBankPopupMSG(msg,null,1);
        }
    }
    private void OnUpdateUPINewBankDetailsResponse(ServerBankDetailsResponse serverBankDetailsResponse)
    {
        if (serverBankDetailsResponse != null)
        {
            if (serverBankDetailsResponse.status.Contains("200"))
            {
                AppManager.VIEW_CONTROLLER.HideLoading();
                //PlayerSave.singleton.ShowErrorMessage(serverBankDetailsResponse.message);
                BankUPINewRequestStatus_NR.text = "<color=#FFFFFF>Status For UPI Details Change is :</color> " + serverBankDetailsResponse.message;
                if (!string.IsNullOrEmpty(StaticValues.BankUPIId_NR))
                {
                    if (StaticValues.BankUPIId_NR.Length > 0)
                    {
                        //string newString = new string('*', (StaticValues.BankAccountNo.Length - 4));
                        BankUPIEnterUPI_NR.text = StaticValues.BankUPIId_NR;// newString + StaticValues.BankAccountNo.Substring(StaticValues.BankAccountNo.Length-4, 4 );
                    }
                }
                else
                {
                    BankUPIEnterUPI_NR.text = "";
                }
                
                
                if (serverBankDetailsResponse.data != null)
                {
                    StaticValues.isBankUPIDetailsSubmitted = serverBankDetailsResponse.data.isUPIBankDetailsSubmitted;
                    StaticValues.isBankUPIStatusForNewRequest = serverBankDetailsResponse.data.BankUPIId_status;
                    StaticValues.BankUPIId = serverBankDetailsResponse.data.BankUPIId;
                    StaticValues.BankUPIId_NR = serverBankDetailsResponse.data.BankUPIId_NR;
                }
                onClickBankDetails();
				PopupMessage msg = new PopupMessage();
				msg.Title = "";
				msg.Message = serverBankDetailsResponse.message;
				if(!string.IsNullOrEmpty(StaticValues.isBankUPIStatusForNewRequest))
				{
					AppManager.VIEW_CONTROLLER.ShowBankPopupMSG(msg,null,1);
				}
				else
				{
					AppManager.VIEW_CONTROLLER.ShowBankPopupMSG(msg,RedirectToWithdrawNow,0);
				}
            }
            else
            {
                AppManager.VIEW_CONTROLLER.HideLoading();
                //PlayerSave.singleton.ShowErrorMessage(serverBankDetailsResponse.message);
                BankUPIEnterUPI_NR.text = "";
               
                StaticValues.BankUPIId_NR = "";
                
				PopupMessage msg = new PopupMessage();
				msg.Title = "";
				msg.Message = serverBankDetailsResponse.message;
				AppManager.VIEW_CONTROLLER.ShowBankPopupMSG(msg,null,1);
            }
        }
        else
        {
            AppManager.VIEW_CONTROLLER.HideLoading();
            //PlayerSave.singleton.ShowErrorMessage("Error");
            BankUPIEnterUPI_NR.text = "";
            
            StaticValues.BankUPIId_NR = "";
			PopupMessage msg = new PopupMessage();
			msg.Title = "";
			msg.Message = "Some error occurred.Please try after some time!";
			AppManager.VIEW_CONTROLLER.ShowBankPopupMSG(msg,null,1);
        }
    }
	public void RedirectToWithdrawNow()
	{
		OnLobbyPage();
		onWithdrawButton();
	}
    public void OnAddCashbutton()
    {
        Canvas_AddCash.SetActive(true);

        AddCashSelectedIcons[0].SetActive(true);
        AddCashSelectedIcons[1].SetActive(false);
        AddCashSelectedIcons[2].SetActive(true);

        AddCashNonSelectedIcons[0].SetActive(true);
        AddCashNonSelectedIcons[1].SetActive(false);
        AddCashNonSelectedIcons[2].SetActive(false);

        AddCashBg_0.SetActive(false);
        AddCashBg_1.SetActive(true);
        AddCashBg_2.SetActive(false);
        MiniStatement.SetActive(false);

        //amountInputField.text = "200";
        bonusInputField.text = "";
        StaticValues.CurrentAddCashIndex = 2;
        ApplyBonusCodeBtn.interactable = true;
        AppliedBonusCode.SetActive(false);
        PasteBonusCodeBtn.interactable = true;

        if (InputBoxSelection[0]) InputBoxSelection[0].SetActive(false);
        if (InputBoxSelection[1]) InputBoxSelection[1].SetActive(false);
        if (InputBoxSelection[2]) InputBoxSelection[2].SetActive(true);
        if (InputBoxSelection[3]) InputBoxSelection[3].SetActive(false);
        LastDeposit = StaticValues.LastDeposit;
        if (LastDeposit > 0)
        {
            StaticValues.CurrentAddCashIndex = 3;
            if (InputBoxSelection[0]) InputBoxSelection[0].SetActive(false);
            if (InputBoxSelection[1]) InputBoxSelection[1].SetActive(false);
            if (InputBoxSelection[2]) InputBoxSelection[2].SetActive(false);
            if (InputBoxSelection[3]) InputBoxSelection[3].SetActive(true);
        }
        CallBeforeRefreshAddCashPage();

        RefreshAddCashPage(MyBonusIndex);

    }
    public void OnAddCashbuttonBonusPopUp()
    {
        Canvas_AddCash.SetActive(true);

        AddCashSelectedIcons[0].SetActive(true);
        AddCashSelectedIcons[1].SetActive(false);
        AddCashSelectedIcons[2].SetActive(true);

        AddCashNonSelectedIcons[0].SetActive(true);
        AddCashNonSelectedIcons[1].SetActive(false);
        AddCashNonSelectedIcons[2].SetActive(false);

        AddCashBg_0.SetActive(false);
        AddCashBg_1.SetActive(true);
        AddCashBg_2.SetActive(false);
        MiniStatement.SetActive(false);

        //amountInputField.text = "200";
        bonusInputField.text = "";
        StaticValues.CurrentAddCashIndex = 2;
        ApplyBonusCodeBtn.interactable = true;
        AppliedBonusCode.SetActive(false);
        PasteBonusCodeBtn.interactable = true;

        if (InputBoxSelection[0]) InputBoxSelection[0].SetActive(false);
        if (InputBoxSelection[1]) InputBoxSelection[1].SetActive(false);
        if (InputBoxSelection[2]) InputBoxSelection[2].SetActive(true);
        if (InputBoxSelection[3]) InputBoxSelection[3].SetActive(false);
        LastDeposit = StaticValues.LastDeposit;
        if(LastDeposit>0)
        {
            StaticValues.CurrentAddCashIndex = 3;
            if (InputBoxSelection[0]) InputBoxSelection[0].SetActive(false);
            if (InputBoxSelection[1]) InputBoxSelection[1].SetActive(false);
            if (InputBoxSelection[2]) InputBoxSelection[2].SetActive(false);
            if (InputBoxSelection[3]) InputBoxSelection[3].SetActive(true);
        }
        

        RefreshAddCashPage(MyBonusIndex);

    }
    public void OnAddCashbuttonBonusClose()
    {
        Canvas_AddCash.SetActive(true);

        AddCashSelectedIcons[0].SetActive(true);
        AddCashSelectedIcons[1].SetActive(false);
        AddCashSelectedIcons[2].SetActive(true);

        AddCashNonSelectedIcons[0].SetActive(true);
        AddCashNonSelectedIcons[1].SetActive(false);
        AddCashNonSelectedIcons[2].SetActive(false);

        AddCashBg_0.SetActive(false);
        AddCashBg_1.SetActive(true);
        AddCashBg_2.SetActive(false);
        MiniStatement.SetActive(false);



       
      

        switch(StaticValues.CurrentAddCashIndex)
        {
            case 0:
                {
                    if (InputBoxSelection[0]) InputBoxSelection[0].SetActive(true);
                    if (InputBoxSelection[1]) InputBoxSelection[1].SetActive(false);
                    if (InputBoxSelection[2]) InputBoxSelection[2].SetActive(false);
                    if (InputBoxSelection[3]) InputBoxSelection[3].SetActive(false);
                }
                break;

            case 1:
                {
                    if (InputBoxSelection[0]) InputBoxSelection[0].SetActive(false);
                    if (InputBoxSelection[1]) InputBoxSelection[1].SetActive(true);
                    if (InputBoxSelection[2]) InputBoxSelection[2].SetActive(false);
                    if (InputBoxSelection[3]) InputBoxSelection[3].SetActive(false);
                }
                break;

            case 2:
                {
                    if (InputBoxSelection[0]) InputBoxSelection[0].SetActive(false);
                    if (InputBoxSelection[1]) InputBoxSelection[1].SetActive(false);
                    if (InputBoxSelection[2]) InputBoxSelection[2].SetActive(true);
                    if (InputBoxSelection[3]) InputBoxSelection[3].SetActive(false);
                }
                break;

            case 3:
                {
                    if (InputBoxSelection[0]) InputBoxSelection[0].SetActive(false);
                    if (InputBoxSelection[1]) InputBoxSelection[1].SetActive(false);
                    if (InputBoxSelection[2]) InputBoxSelection[2].SetActive(false);
                    if (InputBoxSelection[3]) InputBoxSelection[3].SetActive(true);
                }
                break;

        
        }

    }
    public void CallBeforeRefreshAddCashPage()
    {
        Debug.Log("refresh page etto");
        try
        {
            double StaticAmount = 20000;
            double thirdBoxAmount = 1;
            int StaticbonusPercent = 0;
            double highestBonus = 0;

            if (StaticValues.getBannerImageDetails != null)
            {
                if (StaticValues.getBannerImageDetails.Count >= 1)
                {
                    for (int i = 0; i < StaticValues.getBannerImageDetails.Count; i++)
                    {
                       
                        StaticbonusPercent = StaticValues.getBannerImageDetails[i].bonus_persent;

                        
                        StaticAmount = StaticValues.getBannerImageDetails[i].amount;

                       
                        thirdBoxAmount = 1;
                        if (StaticbonusPercent > 0)
                        {
                            thirdBoxAmount = 100 / StaticbonusPercent;
                        }
                        double newCalAmount = thirdBoxAmount * StaticAmount;

                        double calAmount = (StaticbonusPercent * newCalAmount) / 100;
                        if (calAmount > 0)
                        {
                            if (calAmount >= highestBonus)
                            {
                                highestBonus = calAmount;
                                MyBonusIndex = i;
                                if (isDebug)
                                {
                                    Debug.Log("highestBonus " + highestBonus + " MyBonusIndex " + MyBonusIndex);
                                }
                            }

                        }
                    }
                }
            }
        }
        catch
        {
            Debug.Log("caught the statement");
            MyBonusIndex = 0;
        }
    }
    public void RefreshAddCashPage(int _MyBonusIndex)
    {

        StaticValues.CurrentAddCashIndex = 3;
        try
        {
            string lastDeposit = LastDeposit > 0 ? LastDeposit.ToString() : "______";
            if (!lastDeposit.Contains("______"))
            {
                _amountText_4.text = "₹ " + LastDeposit.ToString("#,##0");
                _amountText_4_S.text = _amountText_4.text;
                if (StaticValues.CurrentAddCashIndex == 3)
                {

                    amountInputField.text = LastDeposit.ToString();
                }
            }
            else
            {
                _amountText_4.text = "₹ " + lastDeposit;
                _amountText_4_S.text = _amountText_4.text;
                if (StaticValues.CurrentAddCashIndex == 3)
                {

                    //amountInputField.text = "0";
                }
            }
        }
        catch
        {

        }
    }
    public void OnAddCash_0_AddCashButton()
    {
        AddCashSelectedIcons[0].SetActive(true);
        AddCashSelectedIcons[1].SetActive(false);
        AddCashSelectedIcons[2].SetActive(true);

        AddCashNonSelectedIcons[0].SetActive(true);
        AddCashNonSelectedIcons[1].SetActive(false);
        AddCashNonSelectedIcons[2].SetActive(false);

        AddCashBg_0.SetActive(false);
        AddCashBg_1.SetActive(true);
        AddCashBg_2.SetActive(false);
        MiniStatement.SetActive(false);

        amountInputField.text = "200";
        bonusInputField.text = "";

        ApplyBonusCodeBtn.interactable = true;
        AppliedBonusCode.SetActive(false);
        PasteBonusCodeBtn.interactable = true;
        if (InputBoxSelection[0]) InputBoxSelection[0].SetActive(false);
        if (InputBoxSelection[1]) InputBoxSelection[1].SetActive(false);
        if (InputBoxSelection[2]) InputBoxSelection[2].SetActive(true);
        if (InputBoxSelection[3]) InputBoxSelection[3].SetActive(false);
        LastDeposit = StaticValues.LastDeposit;
        if (LastDeposit > 0)
        {
            StaticValues.CurrentAddCashIndex = 3;
            if (InputBoxSelection[0]) InputBoxSelection[0].SetActive(false);
            if (InputBoxSelection[1]) InputBoxSelection[1].SetActive(false);
            if (InputBoxSelection[2]) InputBoxSelection[2].SetActive(false);
            if (InputBoxSelection[3]) InputBoxSelection[3].SetActive(true);
        }
        CallBeforeRefreshAddCashPage();

        RefreshAddCashPage(MyBonusIndex);
    }
    
    public void OnAddCash_1_ConfirmButton()
    {
        AddCashSelectedIcons[0].SetActive(true);
        AddCashSelectedIcons[1].SetActive(false);
        AddCashSelectedIcons[2].SetActive(true);

        AddCashNonSelectedIcons[0].SetActive(true);
        AddCashNonSelectedIcons[1].SetActive(false);
        AddCashNonSelectedIcons[2].SetActive(false);

        AddCashBg_0.SetActive(false);
        AddCashBg_1.SetActive(true);//change to true
        AddCashBg_2.SetActive(false);//change to false
        MiniStatement.SetActive(false);

        if(!string.IsNullOrEmpty(amountInputField.text))
        {
            try
            {
                if (Convert.ToInt32(amountInputField.text) >= 10 && Convert.ToInt32(amountInputField.text) <= 1000000)
                {
                    //call api for add cash
                    RaiseOnConfirmButtonClick();
                    try
                    {

#if USEFBLOGAPPEVENT
                        FB.LogAppEvent(AppEventName.InitiatedCheckout, null,
                         new Dictionary<string, object>()
                         {
                            { AppEventParameterName.Description, "Deposit Request Amount "+amountInputField.text.ToString() +"'Log AppEvent' "+StaticValues.UserNameValue }
                         });
#endif
                    }
                    catch
                    {

                    }
                }
                else
                {
                    PlayerSave.singleton.ShowErrorMessage("Please enter amount in the range(50-1000000)!!!");
                }
            }
            catch(Exception e)
            {
                PlayerSave.singleton.ShowErrorMessage("Error : "+e.Message);
            }
        }
        else
        {
            PlayerSave.singleton.ShowErrorMessage("Please enter amount before proceed!!!");
        }


    }
    public void OnAddCash_1_BackButton()
    {
        AddCashSelectedIcons[0].SetActive(true);
        AddCashSelectedIcons[1].SetActive(false);
        AddCashSelectedIcons[2].SetActive(true);

        AddCashNonSelectedIcons[0].SetActive(true);
        AddCashNonSelectedIcons[1].SetActive(false);
        AddCashNonSelectedIcons[2].SetActive(false);

        AddCashBg_0.SetActive(true);
        AddCashBg_1.SetActive(false);
        AddCashBg_2.SetActive(false);
        MiniStatement.SetActive(false);

        RaiseOnBackButtonClick();
    }
    public void OnAddCash_2_AddCashButton()
    {
        AddCashSelectedIcons[0].SetActive(true);
        AddCashSelectedIcons[1].SetActive(false);
        AddCashSelectedIcons[2].SetActive(true);

        AddCashNonSelectedIcons[0].SetActive(true);
        AddCashNonSelectedIcons[1].SetActive(false);
        AddCashNonSelectedIcons[2].SetActive(false);

        AddCashBg_0.SetActive(false);
        AddCashBg_1.SetActive(false);
        AddCashBg_2.SetActive(true);
        MiniStatement.SetActive(false);
        
    }
    public void OnAddCash_2_BackButton()
    {
        AddCashSelectedIcons[0].SetActive(true);
        AddCashSelectedIcons[1].SetActive(false);
        AddCashSelectedIcons[2].SetActive(true);

        AddCashNonSelectedIcons[0].SetActive(true);
        AddCashNonSelectedIcons[1].SetActive(false);
        AddCashNonSelectedIcons[2].SetActive(false);

        AddCashBg_0.SetActive(false);
        AddCashBg_1.SetActive(true);
        AddCashBg_2.SetActive(false);
        MiniStatement.SetActive(false);

        RaiseOnBackButtonClick();
    }
    public void OnAddCashInnerbutton()
    {

        Canvas_AddCash.SetActive(true);


        AddCashSelectedIcons[0].SetActive(true);
        AddCashSelectedIcons[1].SetActive(false);
        AddCashSelectedIcons[2].SetActive(true);

        AddCashNonSelectedIcons[0].SetActive(true);
        AddCashNonSelectedIcons[1].SetActive(false);
        AddCashNonSelectedIcons[2].SetActive(false);

        AddCashBg_0.SetActive(false);
        AddCashBg_1.SetActive(true);
        AddCashBg_2.SetActive(false);
        MiniStatement.SetActive(false);

        amountInputField.text = "200";
        bonusInputField.text = "";
        StaticValues.CurrentAddCashIndex = 2;

        ApplyBonusCodeBtn.interactable = true;
        AppliedBonusCode.SetActive(false);
        PasteBonusCodeBtn.interactable = true;


        if (InputBoxSelection[0]) InputBoxSelection[0].SetActive(false);
        if (InputBoxSelection[1]) InputBoxSelection[1].SetActive(false);
        if (InputBoxSelection[2]) InputBoxSelection[2].SetActive(true);
        if (InputBoxSelection[3]) InputBoxSelection[3].SetActive(false);
        LastDeposit = StaticValues.LastDeposit;
        if (LastDeposit > 0)
        {
            StaticValues.CurrentAddCashIndex = 3;
            if (InputBoxSelection[0]) InputBoxSelection[0].SetActive(false);
            if (InputBoxSelection[1]) InputBoxSelection[1].SetActive(false);
            if (InputBoxSelection[2]) InputBoxSelection[2].SetActive(false);
            if (InputBoxSelection[3]) InputBoxSelection[3].SetActive(true);
        }

        CallBeforeRefreshAddCashPage();

        RefreshAddCashPage(MyBonusIndex);
    }
    public void OnBalancebutton()
    {
        AddCashSelectedIcons[0].SetActive(true);
        AddCashSelectedIcons[1].SetActive(false);
        AddCashSelectedIcons[2].SetActive(true);

        AddCashNonSelectedIcons[0].SetActive(false);
        AddCashNonSelectedIcons[1].SetActive(true);
        AddCashNonSelectedIcons[2].SetActive(false);

        AddCashBg_0.SetActive(true);
        AddCashBg_1.SetActive(false);
        AddCashBg_2.SetActive(false);
        MiniStatement.SetActive(false);

        RaiseOnBackButtonClick();
    }
    public void OnMiniStatementbutton()
    {
        AddCashSelectedIcons[0].SetActive(true);
        AddCashSelectedIcons[1].SetActive(false);
        AddCashSelectedIcons[2].SetActive(true);

        AddCashNonSelectedIcons[0].SetActive(false);
        AddCashNonSelectedIcons[1].SetActive(false);
        AddCashNonSelectedIcons[2].SetActive(true);

        AddCashBg_0.SetActive(false);
        AddCashBg_1.SetActive(false);
        AddCashBg_2.SetActive(false);
        MiniStatement.SetActive(true);

        RaiseOnBackButtonClick();
    }
    public void OnKYCPanBackbutton()
    {
        AccountSelectedIcons[0].SetActive(false);
        AccountSelectedIcons[1].SetActive(true);
        AccountSelectedIcons[2].SetActive(false);
        AccountSelectedIcons[3].SetActive(false);
        AccountSelectedIcons[4].SetActive(false);
        AccountSelectedIcons[5].SetActive(false);
        AccountSelectedIcons[6].SetActive(false);

        Panel_MyAccount.SetActive(true);
        SubPanel_PersonalDetails.SetActive(false);
        SubPanel_KycDocuments.SetActive(true);
        SubPanel_PanDocument.SetActive(false);
        SubPanel_AadharDocument.SetActive(false);
        SubPanel_DrivingLicense.SetActive(false);
        SubPanel_VoterCard.SetActive(false);
        SubPanel_PassportNumber.SetActive(false);
        SubPanel_BankDetails.SetActive(false);
        SubPanel_ChangeAvatar.SetActive(false);
        SubPanel_PrivacyPolicy.SetActive(false);
        SubPanel_TermsConditions.SetActive(false);

        SetKYCDetailsPage();
        PanBaseSelected.SetActive(true);
        AadharUploadedImage.text = "";
        PassportUploadedImage.text = "";
        PanUploadedImage.text = "";
        LicenseUploadedImage.text = "";
        VoterCardUploadedImage.text = "";
    }
    public void OnKYCAadharBackbutton()
    {
        AccountSelectedIcons[0].SetActive(false);
        AccountSelectedIcons[1].SetActive(true);
        AccountSelectedIcons[2].SetActive(false);
        AccountSelectedIcons[3].SetActive(false);
        AccountSelectedIcons[4].SetActive(false);
        AccountSelectedIcons[5].SetActive(false);
        AccountSelectedIcons[6].SetActive(false);

        Panel_MyAccount.SetActive(true);
        SubPanel_PersonalDetails.SetActive(false);
        SubPanel_KycDocuments.SetActive(true);
        SubPanel_PanDocument.SetActive(false);
        SubPanel_AadharDocument.SetActive(false);
        SubPanel_DrivingLicense.SetActive(false);
        SubPanel_VoterCard.SetActive(false);
        SubPanel_PassportNumber.SetActive(false);
        SubPanel_BankDetails.SetActive(false);
        SubPanel_ChangeAvatar.SetActive(false);
        SubPanel_PrivacyPolicy.SetActive(false);
        SubPanel_TermsConditions.SetActive(false);
        SubPanel_AccountOverview.SetActive(false);

        SetKYCDetailsPage();
        AddressBaseSelected.SetActive(true);
        KYCDropDownBase.SetActive(true);

        KYCAadharCardSelection.SetActive(true);
        KYCLicenseSelection.SetActive(false);
        KYCPassportSelection.SetActive(false);
        KYCVoterCardSelection.SetActive(false);
        AadharUploadedImage.text = "";
        PassportUploadedImage.text = "";
        PanUploadedImage.text = "";
        LicenseUploadedImage.text = "";
        VoterCardUploadedImage.text = "";
    }
    public void OnKYCLicenseBackbutton()
    {
        AccountSelectedIcons[0].SetActive(false);
        AccountSelectedIcons[1].SetActive(true);
        AccountSelectedIcons[2].SetActive(false);
        AccountSelectedIcons[3].SetActive(false);
        AccountSelectedIcons[4].SetActive(false);
        AccountSelectedIcons[5].SetActive(false);
        AccountSelectedIcons[6].SetActive(false);

        Panel_MyAccount.SetActive(true);
        SubPanel_PersonalDetails.SetActive(false);
        SubPanel_KycDocuments.SetActive(true);
        SubPanel_PanDocument.SetActive(false);
        SubPanel_AadharDocument.SetActive(false);
        SubPanel_DrivingLicense.SetActive(false);
        SubPanel_VoterCard.SetActive(false);
        SubPanel_PassportNumber.SetActive(false);
        SubPanel_BankDetails.SetActive(false);
        SubPanel_ChangeAvatar.SetActive(false);
        SubPanel_PrivacyPolicy.SetActive(false);
        SubPanel_TermsConditions.SetActive(false);
        SubPanel_AccountOverview.SetActive(false);

        SetKYCDetailsPage();
        AddressBaseSelected.SetActive(true);
        KYCDropDownBase.SetActive(true);

        KYCAadharCardSelection.SetActive(false);
        KYCLicenseSelection.SetActive(true);
        KYCPassportSelection.SetActive(false);
        KYCVoterCardSelection.SetActive(false);
        AadharUploadedImage.text = "";
        PassportUploadedImage.text = "";
        PanUploadedImage.text = "";
        LicenseUploadedImage.text = "";
        VoterCardUploadedImage.text = "";
    }
    public void OnKYCVoterBackbutton()
    {
        AccountSelectedIcons[0].SetActive(false);
        AccountSelectedIcons[1].SetActive(true);
        AccountSelectedIcons[2].SetActive(false);
        AccountSelectedIcons[3].SetActive(false);
        AccountSelectedIcons[4].SetActive(false);
        AccountSelectedIcons[5].SetActive(false);
        AccountSelectedIcons[6].SetActive(false);

        Panel_MyAccount.SetActive(true);
        SubPanel_PersonalDetails.SetActive(false);
        SubPanel_KycDocuments.SetActive(true);
        SubPanel_PanDocument.SetActive(false);
        SubPanel_AadharDocument.SetActive(false);
        SubPanel_DrivingLicense.SetActive(false);
        SubPanel_VoterCard.SetActive(false);
        SubPanel_PassportNumber.SetActive(false);
        SubPanel_BankDetails.SetActive(false);
        SubPanel_ChangeAvatar.SetActive(false);
        SubPanel_PrivacyPolicy.SetActive(false);
        SubPanel_TermsConditions.SetActive(false);
        SubPanel_AccountOverview.SetActive(false);

        SetKYCDetailsPage();
        AddressBaseSelected.SetActive(true);
        KYCDropDownBase.SetActive(true);

        KYCAadharCardSelection.SetActive(false);
        KYCLicenseSelection.SetActive(false);
        KYCPassportSelection.SetActive(false);
        KYCVoterCardSelection.SetActive(true);
        AadharUploadedImage.text = "";
        PassportUploadedImage.text = "";
        PanUploadedImage.text = "";
        LicenseUploadedImage.text = "";
        VoterCardUploadedImage.text = "";
    }
    public void OnKYCPassportBackbutton()
    {
        AccountSelectedIcons[0].SetActive(false);
        AccountSelectedIcons[1].SetActive(true);
        AccountSelectedIcons[2].SetActive(false);
        AccountSelectedIcons[3].SetActive(false);
        AccountSelectedIcons[4].SetActive(false);
        AccountSelectedIcons[5].SetActive(false);
        AccountSelectedIcons[6].SetActive(false);

        Panel_MyAccount.SetActive(true);
        SubPanel_PersonalDetails.SetActive(false);
        SubPanel_KycDocuments.SetActive(true);
        SubPanel_PanDocument.SetActive(false);
        SubPanel_AadharDocument.SetActive(false);
        SubPanel_DrivingLicense.SetActive(false);
        SubPanel_VoterCard.SetActive(false);
        SubPanel_PassportNumber.SetActive(false);
        SubPanel_BankDetails.SetActive(false);
        SubPanel_ChangeAvatar.SetActive(false);
        SubPanel_PrivacyPolicy.SetActive(false);
        SubPanel_TermsConditions.SetActive(false);
        SubPanel_AccountOverview.SetActive(false);

        SetKYCDetailsPage();
        AddressBaseSelected.SetActive(true);
        KYCDropDownBase.SetActive(true);

        KYCAadharCardSelection.SetActive(false);
        KYCLicenseSelection.SetActive(false);
        KYCPassportSelection.SetActive(true);
        KYCVoterCardSelection.SetActive(false);
        AadharUploadedImage.text = "";
        PassportUploadedImage.text = "";
        PanUploadedImage.text = "";
        LicenseUploadedImage.text = "";
        VoterCardUploadedImage.text = "";
    }
    public void onWithdrawButton()
    {
        Panel_Withdraw.SetActive(true);
        Panel_WithdrawNewRequest.SetActive(true);
        Panel_WithdrawStatement.SetActive(false);

        withdrawalTotalWinningAmount.text = StaticValues.WithdrawEarningCount;
        withdrawalAmount.text = "";
        withdrawalAccountHolderName.text = StaticValues.UserNameValue;
        withdrawalAccountHolderAccountNumber.text = StaticValues.BankAccountNo;
        withdrawalAccountHolderIfscCode.text = StaticValues.BankIFSCCode;
        withdrawalUPIId.text = StaticValues.BankUPIId;

        if (WithdrawTranferToggle.isOn)
        {
            WithdrawBankObject.SetActive(true);
            WithdrawUPIObject.SetActive(false);
        }
        if (WithdrawUPIToggle.isOn)
        {
            WithdrawBankObject.SetActive(false);
            WithdrawUPIObject.SetActive(true);
        }
        if (StaticValues.isBankDetailsSubmitted)
        {
            withdrawalAccountHolderAccountNumber.interactable = false;
            withdrawalAccountHolderIfscCode.interactable = false;
            if (ChangeBankButton) ChangeBankButton.SetActive(true);
        }
        else
        {
            withdrawalAccountHolderAccountNumber.interactable = true;
            withdrawalAccountHolderIfscCode.interactable = true;
            if (ChangeBankButton) ChangeBankButton.SetActive(false);
        }
        if (StaticValues.isBankUPIDetailsSubmitted)
        {
            withdrawalUPIId.interactable = false;
            if (ChangeUPIBankButton) ChangeUPIBankButton.SetActive(true);
        }
        else
        {
            withdrawalUPIId.interactable = true;
            if (ChangeUPIBankButton) ChangeUPIBankButton.SetActive(false);
        }
        
    }

    public void FillWithdrawDetailsOnBack()
    {
        withdrawalTotalWinningAmount.text = StaticValues.WithdrawEarningCount;
        withdrawalAmount.text = "";
        withdrawalAccountHolderName.text = StaticValues.UserNameValue;
        withdrawalAccountHolderAccountNumber.text = StaticValues.BankAccountNo;
        withdrawalAccountHolderIfscCode.text = StaticValues.BankIFSCCode;
        withdrawalUPIId.text = StaticValues.BankUPIId;
    }
    public void SetAvatarScreen(int index)
    {
        if (PlayerSave.singleton != null)
        {
            PlayerSave.singleton.SaveAvatarScreen(index);
            PlayerSave.singleton.SavePic("");
        }
        if (MatchMakingPhoton.makingPhoton != null)
        {
            MatchMakingPhoton.makingPhoton.ClickSound();
        }
        for (int i = 0; i < TickObjects.Length; i++)
        {
            TickObjects[i].SetActive(true);
        }
        
        if (index >= 0 && index < 27)
        {
            TickObjects[index].SetActive(false);
            ChangeAvatarImage.sprite = PlayerSave.singleton._avatarImages[index];

            
        }
        
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
                if (isDebug)
                {
                    Debug.Log(colors.Length + "_" + newColors.Length);
                }
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

    public void SetSelection()
    {
        if (MatchMakingPhoton.makingPhoton != null)
        {
            MatchMakingPhoton.makingPhoton.ClickSound();
        }
       
       if (PlayerSave.singleton != null)
       {
           
           
           for (int i = 0; i < TickObjects.Length; i++)
           {
               
               TickObjects[i].SetActive(true);
              
           }
            if (PlayerSave.singleton.GetAvatar() >= 0 && PlayerSave.singleton.GetAvatar() < 27)
            {
                TickObjects[PlayerSave.singleton.GetAvatar()].SetActive(false);
            }
           
       }
        
    }
    
    public void SetAmountForAddCash(int _amount)
    {
        switch(_amount)
        {
            case 0:
                {
                    StaticValues.CurrentAddCashIndex = 0;
                   
                    if (!string.IsNullOrEmpty(_amountText_1.text))
                    {
                        if (!_amountText_1.text.Contains("_"))
                        {
                            string[] words = _amountText_1.text.Split(' ');
                            if (words.Length >= 2)
                            {
                                amountInputField.text = words[1];
                                
                            }
                            else
                            {
                                amountInputField.text = "0";
                            }
                        }
                        else
                        {
                            amountInputField.text = "0";
                        }
                    }
                    else
                    {
                        amountInputField.text = "0";
                    }
                }
                break;

            case 1:
                {
                    StaticValues.CurrentAddCashIndex = 1;
                    
                    if (!string.IsNullOrEmpty(_amountText_2.text))
                    {
                        if (!_amountText_2.text.Contains("_"))
                        {
                            string[] words = _amountText_2.text.Split(' ');
                            if (words.Length >= 2)
                            {
                                amountInputField.text = words[1];
                            }
                            else
                            {
                                amountInputField.text = "0";
                            }
                        }
                        else
                        {
                            amountInputField.text = "0";
                        }
                    }
                    else
                    {
                        amountInputField.text = "0";
                    }
                }
                break;

            case 2:
                {
                    StaticValues.CurrentAddCashIndex = 2;
                   
                    if (!string.IsNullOrEmpty(_amountText_3.text))
                    {
                        if (!_amountText_3.text.Contains("_"))
                        {
                            string[] words = _amountText_3.text.Split(' ');
                            if (words.Length >= 2)
                            {
                                amountInputField.text = words[1];
                            }
                            else
                            {
                                amountInputField.text = "0";
                            }
                        }
                        else
                        {
                            amountInputField.text = "0";
                        }
                    }
                    else
                    {
                        amountInputField.text = "0";
                    }
                }
                break;

            case 3:
                {
                    StaticValues.CurrentAddCashIndex = 3;
                   
                   
              
                    if (!string.IsNullOrEmpty(_amountText_4.text))
                    {
                        if (!_amountText_4.text.Contains("_"))
                        {
                            string[] words = _amountText_4.text.Split(' ');
                            if (words.Length >= 2)
                            {
                                amountInputField.text = words[1];
                            }
                            else
                            {
                                amountInputField.text = "0";
                            }
                        }
                        else
                        {
                            amountInputField.text = "0";
                        }
                    }
                    else
                    {
                        amountInputField.text = "0";
                    }
                }
                break;

            default:
                {
                   
                    if (!string.IsNullOrEmpty(_amountText_1.text))
                    {
                        if (!_amountText_1.text.Contains("_"))
                        {
                            string[] words = _amountText_1.text.Split(' ');
                            if (words.Length >= 2)
                            {
                                amountInputField.text = words[1];
                            }
                            else
                            {
                                amountInputField.text = "0";
                            }
                        }
                        else
                        {
                            amountInputField.text = "0";
                        }
                    }
                    else
                    {
                        amountInputField.text = "0";
                    }
                }
                break;
        }
       
        //bonusInputField.text = "";

        //ApplyBonusCodeBtn.interactable = true;
        //AppliedBonusCode.SetActive(false);
        //PasteBonusCodeBtn.interactable = true;

    }
    
    public void EnterAmountEditDone(string amount)
    {

        if (StaticValues.CurrentAddCashIndex == 3)
        {
            if (!string.IsNullOrEmpty(amount))
            {
                if (!amount.Contains("_"))
                {
                    try
                    {
                        if (Convert.ToInt32(amount) >= 0)
                        {
                            amountInputField.text = amount;
                            LastDeposit = Convert.ToDouble(amount);

                            RefreshAddCashPage(MyBonusIndex);
                        }
                    }
                    catch
                    {
                        amountInputField.text = "0";
                        LastDeposit = 0;

                        RefreshAddCashPage(MyBonusIndex);
                    }
                }
                else
                {
                    amountInputField.text = "0";
                    LastDeposit = 0;

                    RefreshAddCashPage(MyBonusIndex);
                }
            }
            else
            {
                amountInputField.text = "0";
                LastDeposit = 0;

                RefreshAddCashPage(MyBonusIndex);
            }
        }
    }
    public void EnterInFourthBox(string amount)
    {
        if (!string.IsNullOrEmpty(amount))
        {
            if (!amount.Contains("_"))
            {
                try
                {
                    if (Convert.ToInt32(amount) >= 0)
                    {
                        amountInputField.text = amount;
                        LastDeposit = Convert.ToDouble(amount);

                        RefreshAddCashPage(MyBonusIndex);
                    }
                }
                catch
                {
                    amountInputField.text = "0";
                    LastDeposit = 0;

                    RefreshAddCashPage(MyBonusIndex);
                }
            }
            else
            {
                amountInputField.text = "0";
                LastDeposit = 0;

                RefreshAddCashPage(MyBonusIndex);
            }
        }
        else
        {
            amountInputField.text = "0";
            LastDeposit = 0;

            RefreshAddCashPage(MyBonusIndex);
        }
       
    }

    public void EnterInFourthBoxDone(string amount)
    {
        if (!string.IsNullOrEmpty(amount))
        {
            if (!amount.Contains("_"))
            {
                try
                {
                    if (Convert.ToInt32(amount) >= 0)
                    {
                        amountInputField.text = amount;
                        LastDeposit = Convert.ToDouble(amount);

                        RefreshAddCashPage(MyBonusIndex);
                    }
                }
                catch
                {
                    amountInputField.text = "0";
                    LastDeposit = 0;

                    RefreshAddCashPage(MyBonusIndex);
                }
            }
            else
            {
                amountInputField.text = "0";
                LastDeposit = 0;

                RefreshAddCashPage(MyBonusIndex);
            }
        }
        else
        {
            amountInputField.text = "0";
            LastDeposit = 0;

            RefreshAddCashPage(MyBonusIndex);
        }

    }
    public void ApplyBonusCode()
    {
        //if (!string.IsNullOrEmpty(bonusInputField.text))
        //{
        //    if (!string.IsNullOrEmpty(amountInputField.text))
        //    {
        //        AppManager.VIEW_CONTROLLER.ShowLoading();
        //        BonusCodeRequest bonusCodeRequest = new BonusCodeRequest();
        //        try
        //        {
        //            bonusCodeRequest.amount = Convert.ToDouble(amountInputField.text);
        //            bonusCodeRequest.couponcode = bonusInputField.text;
        //            bonusCodeRequest.mobile = PlayerSave.singleton.newID();
        //            PlayerSave.singleton.CallBonusCode(bonusCodeRequest, OnBonusCodeResponse);
        //        }
        //        catch
        //        {
        //            PlayerSave.singleton.ShowErrorMessage("Enter amount is invalid!!!");
        //        }
        //    }
        //    else
        //    {
        //        PlayerSave.singleton.ShowErrorMessage("Please enter amount before proceed!!!");
        //    }
        //}
        //else
        //{
        //    PlayerSave.singleton.ShowErrorMessage("Please enter code before proceed!!!");
        //}

        if (!string.IsNullOrEmpty(bonusInputField.text))
        {
            BonusCodeResult bonusCodeResult = new BonusCodeResult();
            bonusCodeResult.status = "200";
            bonusCodeResult.message = "";
            OnBonusCodeResponse(bonusCodeResult);
        }
        else
        {
            PlayerSave.singleton.ShowErrorMessage("Please applied code before proceed!!!");
        }
    }
    public void OnBonusCodeResponse(BonusCodeResult bonusCodeResult)
    {
        AppManager.VIEW_CONTROLLER.HideLoading();
        if (bonusCodeResult!=null)
        {
            if(bonusCodeResult.status.Contains("200"))
            {
                ApplyBonusCodeBtn.interactable = false;
                AppliedBonusCode.SetActive(true);
                PasteBonusCodeBtn.interactable = false;
                //PlayerSave.singleton.ShowErrorMessage(bonusCodeResult.message);

                if(StaticValues.getBannerImageDetails!=null)
                {
                    if(StaticValues.getBannerImageDetails.Count>1)
                    {
                        MyBonusIndex = StaticValues.getBannerImageDetails.FindIndex(x => x.couponcode.Contains(bonusInputField.text));

                        if(!Canvas_AddCash.activeSelf)
                        {
                            LastDeposit = StaticValues.LastDeposit;
                            StaticValues.CurrentAddCashIndex = 2;
                            if (InputBoxSelection[0]) InputBoxSelection[0].SetActive(false);
                            if (InputBoxSelection[1]) InputBoxSelection[1].SetActive(false);
                            if (InputBoxSelection[2]) InputBoxSelection[2].SetActive(true);
                            if (InputBoxSelection[3]) InputBoxSelection[3].SetActive(false);
                            
                            if (LastDeposit > 0)
                            {
                                StaticValues.CurrentAddCashIndex = 3;
                                if (InputBoxSelection[0]) InputBoxSelection[0].SetActive(false);
                                if (InputBoxSelection[1]) InputBoxSelection[1].SetActive(false);
                                if (InputBoxSelection[2]) InputBoxSelection[2].SetActive(false);
                                if (InputBoxSelection[3]) InputBoxSelection[3].SetActive(true);
                            }
                        }
                        if (isDebug)
                        {
                            Debug.Log("MyBonusIndex ....... " + MyBonusIndex);
                        }
                        RefreshAddCashPage(MyBonusIndex);
                        OnAddCashbuttonBonusClose();
                    }
                    else
                    {
                        if (!Canvas_AddCash.activeSelf)
                        {
                            LastDeposit = StaticValues.LastDeposit;
                            StaticValues.CurrentAddCashIndex = 2;
                            if (InputBoxSelection[0]) InputBoxSelection[0].SetActive(false);
                            if (InputBoxSelection[1]) InputBoxSelection[1].SetActive(false);
                            if (InputBoxSelection[2]) InputBoxSelection[2].SetActive(true);
                            if (InputBoxSelection[3]) InputBoxSelection[3].SetActive(false);

                            if (LastDeposit > 0)
                            {
                                StaticValues.CurrentAddCashIndex = 3;
                                if (InputBoxSelection[0]) InputBoxSelection[0].SetActive(false);
                                if (InputBoxSelection[1]) InputBoxSelection[1].SetActive(false);
                                if (InputBoxSelection[2]) InputBoxSelection[2].SetActive(false);
                                if (InputBoxSelection[3]) InputBoxSelection[3].SetActive(true);
                            }
                        }
                        MyBonusIndex = 0;
                        RefreshAddCashPage(MyBonusIndex);
                        OnAddCashbuttonBonusClose();
                    }
                }
                else
                {
                    if (!Canvas_AddCash.activeSelf)
                    {
                        LastDeposit = StaticValues.LastDeposit;
                    }
                    CallBeforeRefreshAddCashPage();
                    RefreshAddCashPage(MyBonusIndex);
                }
            }
            else
            {
                if (!Canvas_AddCash.activeSelf)
                {
                    LastDeposit = StaticValues.LastDeposit;
                }
                ApplyBonusCodeBtn.interactable = true;
                AppliedBonusCode.SetActive(false);
                PasteBonusCodeBtn.interactable = true;
                PlayerSave.singleton.ShowErrorMessage(bonusCodeResult.message);
                CallBeforeRefreshAddCashPage();
                RefreshAddCashPage(MyBonusIndex);
            }
        }
        else
        {
            ApplyBonusCodeBtn.interactable = true;
            AppliedBonusCode.SetActive(false);
            PasteBonusCodeBtn.interactable = true;
            PlayerSave.singleton.ShowErrorMessage(bonusCodeResult.message);
            CallBeforeRefreshAddCashPage();
            RefreshAddCashPage(MyBonusIndex);
        }

    }
    public void OnSubPanelWithdrawButton()
    {
		StaticValues.StaticTrans_Continue=0;
        if(string.IsNullOrEmpty(StaticValues.BankAccountNo) || string.IsNullOrEmpty(StaticValues.BankIFSCCode))
        {
            if(string.IsNullOrEmpty(withdrawalAccountHolderAccountNumber.text) || string.IsNullOrEmpty(withdrawalAccountHolderIfscCode.text))
            {
                //PlayerSave.singleton.ShowErrorMessage("Please complete your bank details before proceed!!!");
				PopupMessage msg = new PopupMessage();
				msg.Title = "";
				msg.Message = "Please complete your bank details before proceed.";
				AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);
                return;
            }
            else
            {
                StaticValues.BankAccountNo = withdrawalAccountHolderAccountNumber.text;
                StaticValues.BankIFSCCode = withdrawalAccountHolderIfscCode.text;
            }
        }
        
        
        if (string.IsNullOrEmpty(StaticValues.UserNameValue))
        {
            PlayerSave.singleton.ShowErrorMessage("Please set your user name first!!!");
            return;

        }
        if (string.IsNullOrEmpty(withdrawalAmount.text))
        {
			PopupMessage msg = new PopupMessage();
			msg.Title = "";
			msg.Message = "Minimum withdraw amount is Rs "+ StaticValues.MinimumAmount.ToString();
			AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);
            return;

        }
        try
        {
			double MaxAmount = 100000;
            double WEC = Convert.ToDouble(StaticValues.WithdrawEarningCount);
            double WA = Convert.ToDouble(withdrawalAmount.text);
            if (WEC > 0 && WA <= WEC)
            {
                
                if (WEC >= StaticValues.MinimumAmount)
                {
                    
                        if (StaticValues.BankIFSCCode.Length >= 5)
                        {
                            AppManager.VIEW_CONTROLLER.ShowLoading();
                            //call api for withdraw
						PlayerSave.singleton.GameWithdrawDetailsAPICall(StaticValues.FirebaseUserId, "9876622096", "support@kheltamasha.site", StaticValues.BankAccountNo, WA.ToString(), StaticValues.BankIFSCCode.Substring(0, 4), StaticValues.BankIFSCCode, StaticValues.UserNameValue, OnWithdrwaResponse, "Bankaccount",StaticValues.StaticTrans_Continue);
                        }
                        else
                        {
                            //PlayerSave.singleton.ShowErrorMessage("Please enter your correct details before proceed!!!");
							PopupMessage msg = new PopupMessage();
							msg.Title = "";
							msg.Message = "Please enter your correct details before proceed!!!";
							AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);
                        }
                    
                }
                else
                {
                    //PlayerSave.singleton.ShowErrorMessage("Minimum withdraw amount is Rs "+ StaticValues.MinimumAmount.ToString());
					PopupMessage msg = new PopupMessage();
					msg.Title = "";
					msg.Message = "Minimum withdraw amount is Rs "+ StaticValues.MinimumAmount.ToString();
					AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);
                }
            }
            else
            {
                //PlayerSave.singleton.ShowErrorMessage("Withdrawal Amount can't be less or greater than winning amount.");
				PopupMessage msg = new PopupMessage();
				msg.Title = "";
				//msg.Message = "Withdrawal Amount can't be less or greater than winning amount.";
				if (WA <= MaxAmount)
				{
					if(WA <= WEC)
					{
						msg.Message = "Minimum withdraw amount is Rs "+StaticValues.MinimumAmount.ToString();
					}
					else
					{
						msg.Message = "Withdraw amount can't be greater than Rs "+WEC.ToString();
					}
				}
				else{
					msg.Message = "Maximum withdraw amount is Rs "+MaxAmount.ToString();
				}
				AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);
            }
            try
            {

#if USEFBLOGAPPEVENT
                FB.LogAppEvent(AppEventName.AddedPaymentInfo, null,
                           new Dictionary<string, object>()
                           {
                            { AppEventParameterName.Description, "Withdrawal Request Amount "+WA.ToString() +"'Log AppEvent' "+StaticValues.UserNameValue }
                           });
#endif
            }
            catch
            {

            }
        }
        catch(Exception e)
        {
            PlayerSave.singleton.ShowErrorMessage("Error : "+e.Message);
        }
    }
	public void OnSubPanelWithdrawButton2()//same as above but only difference is StaticValues.StaticTrans_Continue is 1 in this method and in above 0  and withdrawal_Amount.text value
	{
		StaticValues.StaticTrans_Continue=1;
		if(string.IsNullOrEmpty(StaticValues.BankAccountNo) || string.IsNullOrEmpty(StaticValues.BankIFSCCode))
		{
			if(string.IsNullOrEmpty(withdrawalAccountHolderAccountNumber.text) || string.IsNullOrEmpty(withdrawalAccountHolderIfscCode.text))
			{
				//PlayerSave.singleton.ShowErrorMessage("Please complete your bank details before proceed!!!");
				PopupMessage msg = new PopupMessage();
				msg.Title = "";
				msg.Message = "Please complete your bank details before proceed.";
					AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);
				return;
			}
			else
			{
				StaticValues.BankAccountNo = withdrawalAccountHolderAccountNumber.text;
				StaticValues.BankIFSCCode = withdrawalAccountHolderIfscCode.text;
			}
		}


		if (string.IsNullOrEmpty(StaticValues.UserNameValue))
		{
			PlayerSave.singleton.ShowErrorMessage("Please set your user name first!!!");
			return;

		}
		if (string.IsNullOrEmpty(withdrawalAmount.text))
		{
			withdrawalAmount.text="0";
	

		}
		try
		{
			double MaxAmount = 100000;
			double WEC = Convert.ToDouble(StaticValues.WithdrawEarningCount);
			double WA = Convert.ToDouble(withdrawalAmount.text);
			if (WEC > 0 && WA <= WEC)
			{

				if (WEC >= StaticValues.MinimumAmount)
				{

					if (StaticValues.BankIFSCCode.Length >= 5)
					{
						AppManager.VIEW_CONTROLLER.ShowLoading();
						//call api for withdraw
						PlayerSave.singleton.GameWithdrawDetailsAPICall(StaticValues.FirebaseUserId, "9876622096", "support@kheltamasha.site", StaticValues.BankAccountNo, WA.ToString(), StaticValues.BankIFSCCode.Substring(0, 4), StaticValues.BankIFSCCode, StaticValues.UserNameValue, OnWithdrwaResponse, "Bankaccount",StaticValues.StaticTrans_Continue);
					}
					else
					{
						//PlayerSave.singleton.ShowErrorMessage("Please enter your correct details before proceed!!!");
						PopupMessage msg = new PopupMessage();
						msg.Title = "";
						msg.Message = "Please enter your correct details before proceed!!!";
						AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);
					}

				}
				else
				{
					//PlayerSave.singleton.ShowErrorMessage("Minimum withdraw amount is Rs "+ StaticValues.MinimumAmount.ToString());
					PopupMessage msg = new PopupMessage();
					msg.Title = "";
					msg.Message = "Minimum withdraw amount is Rs "+ StaticValues.MinimumAmount.ToString();
					AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);
				}
			}
			else
			{
				//PlayerSave.singleton.ShowErrorMessage("Withdrawal Amount can't be less or greater than winning amount.");
				PopupMessage msg = new PopupMessage();
				msg.Title = "";
				//msg.Message = "Withdrawal Amount can't be less or greater than winning amount.";
				if (WA <= MaxAmount)
				{
					if(WA <= WEC)
					{
						msg.Message = "Minimum withdraw amount is Rs "+StaticValues.MinimumAmount.ToString();
					}
					else
					{
						msg.Message = "Withdraw amount can't be greater than Rs "+WEC.ToString();
					}
				}
				else{
					msg.Message = "Maximum withdraw amount is Rs "+MaxAmount.ToString();
				}
				AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);
			}
			try
			{

				#if USEFBLOGAPPEVENT
				FB.LogAppEvent(AppEventName.AddedPaymentInfo, null,
				new Dictionary<string, object>()
				{
				{ AppEventParameterName.Description, "Withdrawal Request Amount "+WA.ToString() +"'Log AppEvent' "+StaticValues.UserNameValue }
				});
				#endif
			}
			catch
			{

			}
		}
		catch(Exception e)
		{
			PlayerSave.singleton.ShowErrorMessage("Error : "+e.Message);
		}
	}
    public void OnSubPanelWithdrawUPIButton()
    {
		StaticValues.StaticTrans_Continue=0;
        if (string.IsNullOrEmpty(StaticValues.BankUPIId))
        {
            if (string.IsNullOrEmpty(withdrawalUPIId.text))
            {
                //PlayerSave.singleton.ShowErrorMessage("Please complete your bank details before proceed!!!");
				PopupMessage msg = new PopupMessage();
				msg.Title = "";
				msg.Message = "Please complete your bank details before proceed.";
					AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);
                return;
            }
            else
            {
                StaticValues.BankUPIId = withdrawalUPIId.text;
            }
        }


        if (string.IsNullOrEmpty(StaticValues.UserNameValue))
        {
            PlayerSave.singleton.ShowErrorMessage("Please set your user name first!!!");
            return;

        }
        if (string.IsNullOrEmpty(withdrawalAmount.text))
        {
			PopupMessage msg = new PopupMessage();
			msg.Title = "";
			msg.Message = "Minimum withdraw amount is Rs "+ StaticValues.MinimumAmount.ToString();
			AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);
            return;

        }
        try
        {
			double MaxAmount = 100000;
            double WEC = Convert.ToDouble(StaticValues.WithdrawEarningCount);
            double WA = Convert.ToDouble(withdrawalAmount.text);
            if (WEC > 0 && WA <= WEC)
            {

                if (WEC >= StaticValues.MinimumAmount)
                {
                    
                        if (StaticValues.BankUPIId.Length >= 5)
                        {
                            AppManager.VIEW_CONTROLLER.ShowLoading();
                            //call api for withdraw
						PlayerSave.singleton.GameWithdrawDetailsAPICall(StaticValues.FirebaseUserId, "9876622096", "support@kheltamasha.site", StaticValues.BankUPIId, WA.ToString(), "", "", StaticValues.UserNameValue, OnWithdrwaUPIResponse, "Bankupi",StaticValues.StaticTrans_Continue);
                        }
                        else
                        {
                            //PlayerSave.singleton.ShowErrorMessage("Please enter your correct details before proceed!!!");
							PopupMessage msg = new PopupMessage();
							msg.Title = "";
							msg.Message = "Please enter your correct details before proceed!!!";
							AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);
                        }
                    
                }
                else
                {
                    //PlayerSave.singleton.ShowErrorMessage("Minimum withdraw amount is Rs " + StaticValues.MinimumAmount.ToString());
					PopupMessage msg = new PopupMessage();
					msg.Title = "";
					msg.Message = "Minimum withdraw amount is Rs "+ StaticValues.MinimumAmount.ToString();
					AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);
                }
            }
            else
            {
                //PlayerSave.singleton.ShowErrorMessage("You don't have sufficient funds to request!!!");
				PopupMessage msg = new PopupMessage();
				msg.Title = "";
				//msg.Message = "Withdrawal Amount can't be less or greater than winning amount.";
				if (WA <= MaxAmount)
				{
					if(WA <= WEC)
					{
						msg.Message = "Minimum withdraw amount is Rs "+StaticValues.MinimumAmount.ToString();
					}
					else
					{
						msg.Message = "Withdraw amount can't be greater than Rs "+WEC.ToString();
					}
				}
				else{
					msg.Message = "Maximum withdraw amount is Rs "+MaxAmount.ToString();
				}
				AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);
            }
            try
            {

#if USEFBLOGAPPEVENT
                FB.LogAppEvent(AppEventName.AddedPaymentInfo, null,
                           new Dictionary<string, object>()
                           {
                            { AppEventParameterName.Description, "Withdrawal Request Amount "+WA.ToString() +"'Log AppEvent' "+StaticValues.UserNameValue }
                           });
#endif
            }
            catch
            {

            }
        }
        catch (Exception e)
        {
            PlayerSave.singleton.ShowErrorMessage("Error : " + e.Message);
        }
    }
	public void OnSubPanelWithdrawUPIButton2()//same as above but only difference is StaticValues.StaticTrans_Continue is 1 in this method and in above 0  and withdrawal_Amount.text value
	{
		StaticValues.StaticTrans_Continue=1;
		if (string.IsNullOrEmpty(StaticValues.BankUPIId))
		{
			if (string.IsNullOrEmpty(withdrawalUPIId.text))
			{
				//PlayerSave.singleton.ShowErrorMessage("Please complete your bank details before proceed!!!");
				PopupMessage msg = new PopupMessage();
				msg.Title = "";
				msg.Message = "Please complete your bank details before proceed.";
					AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);
				return;
			}
			else
			{
				StaticValues.BankUPIId = withdrawalUPIId.text;
			}
		}


		if (string.IsNullOrEmpty(StaticValues.UserNameValue))
		{
			PlayerSave.singleton.ShowErrorMessage("Please set your user name first!!!");
			return;

		}
		if (string.IsNullOrEmpty(withdrawalAmount.text))
		{
			withdrawalAmount.text="0";
		
		}
		try
		{
			double MaxAmount = 100000;
			double WEC = Convert.ToDouble(StaticValues.WithdrawEarningCount);
			double WA = Convert.ToDouble(withdrawalAmount.text);
			if (WEC > 0 && WA <= WEC)
			{

				if (WEC >= StaticValues.MinimumAmount)
				{

					if (StaticValues.BankUPIId.Length >= 5)
					{
						AppManager.VIEW_CONTROLLER.ShowLoading();
						//call api for withdraw
						PlayerSave.singleton.GameWithdrawDetailsAPICall(StaticValues.FirebaseUserId, "9876622096", "support@kheltamasha.site", StaticValues.BankUPIId, WA.ToString(), "", "", StaticValues.UserNameValue, OnWithdrwaUPIResponse, "Bankupi",StaticValues.StaticTrans_Continue);
					}
					else
					{
						//PlayerSave.singleton.ShowErrorMessage("Please enter your correct details before proceed!!!");
						PopupMessage msg = new PopupMessage();
						msg.Title = "";
						msg.Message = "Please enter your correct details before proceed!!!";
						AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);
					}

				}
				else
				{
					//PlayerSave.singleton.ShowErrorMessage("Minimum withdraw amount is Rs " + StaticValues.MinimumAmount.ToString());
					PopupMessage msg = new PopupMessage();
					msg.Title = "";
					msg.Message = "Minimum withdraw amount is Rs "+ StaticValues.MinimumAmount.ToString();
					AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);
				}
			}
			else
			{
				//PlayerSave.singleton.ShowErrorMessage("You don't have sufficient funds to request!!!");
				PopupMessage msg = new PopupMessage();
				msg.Title = "";
				//msg.Message = "Withdrawal Amount can't be less or greater than winning amount.";
				if (WA <= MaxAmount)
				{
					if(WA <= WEC)
					{
						msg.Message = "Minimum withdraw amount is Rs "+StaticValues.MinimumAmount.ToString();
					}
					else
					{
						msg.Message = "Withdraw amount can't be greater than Rs "+WEC.ToString();
					}
				}
				else{
					msg.Message = "Maximum withdraw amount is Rs "+MaxAmount.ToString();
				}
				AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);
			}
			try
			{

				#if USEFBLOGAPPEVENT
				FB.LogAppEvent(AppEventName.AddedPaymentInfo, null,
				new Dictionary<string, object>()
				{
				{ AppEventParameterName.Description, "Withdrawal Request Amount "+WA.ToString() +"'Log AppEvent' "+StaticValues.UserNameValue }
				});
				#endif
			}
			catch
			{

			}
		}
		catch (Exception e)
		{
			PlayerSave.singleton.ShowErrorMessage("Error : " + e.Message);
		}
	}
    public void OnNewRequestButton()
    {
        Panel_WithdrawNewRequest.SetActive(true);
        Panel_WithdrawStatement.SetActive(false);

        withdrawalTotalWinningAmount.text = StaticValues.WithdrawEarningCount;
        withdrawalAmount.text = "";
        withdrawalAccountHolderName.text = StaticValues.UserNameValue;
        withdrawalAccountHolderAccountNumber.text = StaticValues.BankAccountNo;
        withdrawalAccountHolderIfscCode.text = StaticValues.BankIFSCCode;
        withdrawalUPIId.text = StaticValues.BankUPIId;
    }
    private void OnWithdrwaResponse(WithdrawRefundDetailsResponse withdrawRefundDetailsResponse)
    {
        AppManager.VIEW_CONTROLLER.HideLoading();
		int newMsg=0;
        if(withdrawRefundDetailsResponse!=null)
        {
            //PlayerSave.singleton.ShowErrorMessage(withdrawRefundDetailsResponse.message);
            try
            {
                if (withdrawRefundDetailsResponse.status.Contains("200"))
                {
					newMsg=0;
                    if (withdrawRefundDetailsResponse.data2 != null)
                    {
                        StaticValues.isBankDetailsSubmitted = withdrawRefundDetailsResponse.data2.isBankDetailsSubmitted;
                        StaticValues.BankAccountNo = withdrawRefundDetailsResponse.data2.Account_no;
                      
                        StaticValues.BankAccountNo_NR = withdrawRefundDetailsResponse.data2.NewAccount_no;
                        StaticValues.BankIFSCCode = withdrawRefundDetailsResponse.data2.back_ifsc_code;
                        StaticValues.BankIFSCCode_NR = withdrawRefundDetailsResponse.data2.Newback_ifsc_code;
                        StaticValues.isBankStatusForNewRequest = withdrawRefundDetailsResponse.data2.isBankStatusForNewRequest;
                      
                    }
                }
				else if(withdrawRefundDetailsResponse.status.Contains("201"))
				{
					newMsg=2;
				}
				else
				{
					newMsg=1;
				}
            }
            catch
            {

            }
			if(newMsg !=2)
			{
            	onWithdrawButton();
			}
			if(!string.IsNullOrEmpty(withdrawRefundDetailsResponse.message))
			{
				PopupMessage msg = new PopupMessage();
				msg.Title = "";
				msg.Message = withdrawRefundDetailsResponse.message;

				if(newMsg ==0)
				{
					AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,OnLobbyPage,0);//continue playing send to lobby oage
				}
				else if(newMsg ==1)
				{
					AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);//close only prompt with ok button
				}
				else if(newMsg ==2)
				{
					AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,OnWithdrawBankContinueButton,2);//continue button and cancel button
				}
			}
        }
    }
    private void OnWithdrwaUPIResponse(WithdrawRefundDetailsResponse withdrawRefundDetailsResponse)
    {
        AppManager.VIEW_CONTROLLER.HideLoading();
		int newMsg=0;
        if (withdrawRefundDetailsResponse != null)
        {
            //PlayerSave.singleton.ShowErrorMessage(withdrawRefundDetailsResponse.message);
            try
            {
                if (withdrawRefundDetailsResponse.status.Contains("200"))
                {
					newMsg=0;
                    if (withdrawRefundDetailsResponse.data2 != null)
                    {
                       
                       
                        StaticValues.BankUPIId = withdrawRefundDetailsResponse.data2.BankUPIId;
                        StaticValues.isBankUPIDetailsSubmitted = withdrawRefundDetailsResponse.data2.isUPIBankDetailsSubmitted;
                        StaticValues.isBankUPIStatusForNewRequest = withdrawRefundDetailsResponse.data2.BankUPIId_status;
                        StaticValues.BankUPIId_NR = withdrawRefundDetailsResponse.data2.BankUPIId_NR;
                    }
                }
				else if(withdrawRefundDetailsResponse.status.Contains("201"))
				{
					newMsg=2;
				}
				else
				{
					newMsg=1;
				}
            }
            catch
            {

            }
			if(newMsg !=2)
			{
           		 onWithdrawButton();
			}
			if(!string.IsNullOrEmpty(withdrawRefundDetailsResponse.message))
			{
				PopupMessage msg = new PopupMessage();
				msg.Title = "";
				msg.Message = withdrawRefundDetailsResponse.message;
				if(newMsg ==0)
				{
					AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,OnLobbyPage,0);//continue playing send to lobby oage
				}
				else if(newMsg ==1)
				{
					AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,null,1);//close only prompt with ok button
				}
				else if(newMsg ==2)
				{
					AppManager.VIEW_CONTROLLER.ShowWithdrawPopupMSG(msg,OnWithdrawUPIContinueButton,2);//continue button and cancel button
				}
			}
        }
    }
	public void OnWithdrawBankContinueButton()
	{
		StaticValues.StaticTrans_Continue=1;
		OnSubPanelWithdrawButton2();
	}
	public void OnWithdrawUPIContinueButton()
	{
		StaticValues.StaticTrans_Continue=1;
		OnSubPanelWithdrawUPIButton2();
	}
	public void OnLobbyPage()
	{
		Panel_Withdraw.SetActive(false);
		Canvas_AddCash.SetActive(false);
		Panel_MyAccount.SetActive(false);
		Panel_BonusCash.SetActive(false);
		Panel_Orientation.SetActive(false);
		Panel_ReferAFriend.SetActive(false);
		Panel_Withdraw.SetActive(false);
		Panel_AddCashWebView.SetActive(false); 
		Panel_BonusPopUp.SetActive(false);
	}
    public void OnBonusButton()
    {
        BonusSelectedIcons[0].SetActive(true);
        BonusSelectedIcons[1].SetActive(false);
       
        Panel_BonusBg.SetActive(true);
        Panel_BonusStatement.SetActive(false);

        
    }
    public void OnBonusLatestPromotionClick()
    {
        BonusSelectedIcons[0].SetActive(true);
        BonusSelectedIcons[1].SetActive(false);

        Panel_BonusBg.SetActive(true);
        Panel_BonusStatement.SetActive(false);

       
    }
    public void OnBonusTransactionsClick()
    {
        BonusSelectedIcons[0].SetActive(false);
        BonusSelectedIcons[1].SetActive(true);

        Panel_BonusBg.SetActive(false);
        Panel_BonusStatement.SetActive(true);

        
    }
    public void OnReferAFriendButton()
    {
        ReferSelectedIcons[0].SetActive(true);
        ReferSelectedIcons[1].SetActive(false);

        Panel_ReferBg.SetActive(true);
        Panel_ReferStatement.SetActive(false);
    }
    public void OnReferAFriendSubButtonClick()
    {
        ReferSelectedIcons[0].SetActive(true);
        ReferSelectedIcons[1].SetActive(false);

        Panel_ReferBg.SetActive(true);
        Panel_ReferStatement.SetActive(false);
    }
    public void OnReferAFriendTransactionsClick()
    {
        ReferSelectedIcons[0].SetActive(false);
        ReferSelectedIcons[1].SetActive(true);

        Panel_ReferBg.SetActive(false);
        Panel_ReferStatement.SetActive(true);
    }
    public void OnReferAFriendBySMS()
    {
        string mobile_num = "";
        string message = "Tap here https://kheltamasha.site to download the game and get refer bonus using my referral code\n\n" +
                           StaticValues.MyReferralCode.ToString();

        StaticValues.MyReferralCode = StaticValues.UserNameValue;
        if (!string.IsNullOrEmpty(StaticValues.RAF_SMS))
        {
            message = StaticValues.RAF_SMS + "\n" + StaticValues.MyReferralCode.ToString();
        }

#if UNITY_ANDROID
        //Android SMS URL - doesn't require encoding for sms call to work
        string URL = string.Format("sms:{0}?body={1}", mobile_num, System.Uri.EscapeDataString(message));
#endif

#if UNITY_IOS
            //ios SMS URL - ios requires encoding for sms call to work
            //string URL = string.Format("sms:{0}?&body={1}",mobile_num,WWW.EscapeURL(message)); //Method1 - Works but puts "+" for spaces
            //string URL ="sms:"+mobile_num+"?&body="+WWW.EscapeURL(message); //Method2 - Works but puts "+" for spaces
            //string URL = string.Format("sms:{0}?&body={1}",mobile_num,System.Uri.EscapeDataString(message)); //Method3 - Works perfect
            string URL ="sms:"+mobile_num+"?&body="+ System.Uri.EscapeDataString(message); //Method4 - Works perfectly
#endif

        //Execute Text Message
        Application.OpenURL(URL);
    }
    public void OnReferAFriendByWhatsapp()
    {
        string message = "Tap here https://kheltamasha.site to download the game and get refer bonus using my referral code\n\n" +
                        StaticValues.MyReferralCode.ToString();
        if (NativeShare.TargetExists("com.whatsapp"))
        {
            new NativeShare().AddTarget("com.whatsapp").SetTitle("Khel Tamasha").SetText(message).SetSubject(" ").Share();
        }
        
    }
    
    public void PasteCode()
    {
        bonusInputField.text = UniClipboard.GetText();
    }

    public void OnEditWithdrawBankDetails()
    {
        BankTranferToggle.isOn = true;
        BankUPIToggle.isOn = false;
        if (EnterBankObject) EnterBankObject.SetActive(true);
        if (EnterUPIObject) EnterUPIObject.SetActive(false);
        onMyAccountBtn();
        onClickBankDetails();
    }
    public void OnEditWithdrawUPIBankDetails()
    {
       
        BankUPIToggle.isOn = true;
        BankTranferToggle.isOn = false;
        if (EnterBankObject) EnterBankObject.SetActive(false);
        if (EnterUPIObject) EnterUPIObject.SetActive(true);
        onMyAccountBtn();
        onClickBankDetails();
        
    }

}
public class TimeUtils
{
    private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    //
    public static long totalMilliseconds(DateTime _date)
    {
        return (long)(_date - Jan1st1970).TotalMilliseconds;
    }
}
