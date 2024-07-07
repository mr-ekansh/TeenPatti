using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SocialApp;
using System.Text.RegularExpressions;
using System;
using Google.MiniJSON;
using System.Security.Policy;

public class AddCashOffice : MonoBehaviour
{
   
    private bool debug = false;

   
    public static string _m_url;


    private UniWebView _uniwebView;
    string toastString;
    AndroidJavaObject currentActivity;


    public RectTransform myUITransform;

    public Button _GoBack;
    public Button _GoForward;

    [SerializeField, Multiline(6)]
    private string m_HTMLString;

    [SerializeField]
    private string m_javaScript;

    public const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    public InputField amountText;
    public InputField addCashCouponCode;
    public GameObject ForWebViewCanvas;
    void OnEnable()
    {
        Debug.Log("run whole");
        if (!PlayerPrefs.HasKey("MyLastDeposit"))
        {
            Debug.Log("run this");
            amountText.text = "200";   
        }
        else
        {
            Debug.Log("run that");
            amountText.text = PlayerPrefs.GetString("MyLastDeposit");
        }
        debug = true;

        UniWebViewLogger.Instance.LogLevel = UniWebViewLogger.Level.Off;
        
        if(_uniwebView==null)
        {
            UniWebView.SetAllowJavaScriptOpenWindow(true);
            UniWebView.SetAllowUniversalAccessFromFileURLs(true);
            UniWebView.SetJavaScriptEnabled(true);
            UniWebView.SetWebContentsDebuggingEnabled(true);

            var webViewGameObject = new GameObject("AddCashUniWebView");
            _uniwebView = webViewGameObject.AddComponent<UniWebView>();
            
            _uniwebView.Frame = new Rect(0, 0, Screen.width, Screen.height);

            _uniwebView.SetBackButtonEnabled(true);
            _uniwebView.SetShowSpinnerWhileLoading(true);
            _uniwebView.SetSpinnerText("Loading...");
            _uniwebView.SetZoomEnabled(false);
            _uniwebView.ReferenceRectTransform = myUITransform;
            //_uniwebView.AddUrlScheme("https");

        }
        
        if(_uniwebView!=null)
        {
            
            _uniwebView.OnPageErrorReceived += (view, statuscode, errorMessage) =>
             {
                 Debug.Log("OnPageErrorReceived is: ");
                 CloseWebView();


             };
            _uniwebView.OnPageProgressChanged += (view, progress) =>
            {
                Debug.Log("OnPageProgressChanged is: ");
            };

        }
        if (_uniwebView != null)
        {
           

            _uniwebView.OnMessageReceived += (view, message) => {
                Debug.Log("OnMessageReceived is: " + message.RawMessage);
                Debug.Log("OnMessageReceived is: " + message.Path);
                Debug.Log("OnMessageReceived is: " + message.Scheme);
                if (message.Path.Equals("game-over"))
                {
                    var score = message.Args["score"];
                    Debug.Log("Your final score is: " + score);

                    // Restart the game
                }
                if (message.Path.Contains("kheltamasha.site"))
                {
                    if (message.Path.Equals("cancel") || message.Path.ToLower().Equals("cancel") || message.Path.ToLower().Contains("cancel"))
                    {
                        Debug.Log("Cancel");
                        PopupMessage msg = new PopupMessage();
                        msg.Title = "Error";
                        msg.Message = "Your Deposit of Rs " + amountText.text.ToString() + " was cancelled.Please retry from below options.";
                        AppManager.VIEW_CONTROLLER.ShowPopupMessage(msg,0);
                        if (MainMenuUI.menuUI != null)
                        {

                            MainMenuUI.menuUI.RaiseOnBackButtonClick();
                        }
                        if (ForWebViewCanvas) ForWebViewCanvas.SetActive(false);
                        //_uniwebView.CleanCache();
                        //_uniwebView.Hide();
                    }
                    else if (message.Path == ("sucess") || message.Path.ToLower().Equals("sucess") || message.Path == ("success") || message.Path.ToLower().Equals("success") || message.Path.ToLower().Contains("success") || message.Path.ToLower().Contains("sucess"))
                    {
                        Debug.Log("sucess");
                        PopupMessage msg = new PopupMessage();
                        msg.Title = "Success";
                        msg.Message = "Your Deposit of Rs " + amountText.text.ToString() + " is Successful!!!";
                        AppManager.VIEW_CONTROLLER.ShowPopupMessage(msg,0);
                        if (MainMenuUI.menuUI != null)
                        {
                            MainMenuUI.menuUI.CloseAddCash();
                            MainMenuUI.menuUI.RaiseOnBackButtonClick();
                        }
                        if (ForWebViewCanvas) ForWebViewCanvas.SetActive(false);
                        //_uniwebView.CleanCache();
                        //_uniwebView.Hide();
                    }
                    else if (message.Path == ("failed") || message.Path.ToLower().Equals("failed") || message.Path.ToLower().Contains("failed") || message.Path.ToLower().Contains("fail"))
                    {
                        Debug.Log("failed");
                        PopupMessage msg = new PopupMessage();
                        msg.Title = "Error";
                        msg.Message = "Your Deposit of Rs " + amountText.text.ToString() + " was failed.Please retry from below options.";
                        AppManager.VIEW_CONTROLLER.ShowPopupMessage(msg,0);
                        if (MainMenuUI.menuUI != null)
                        {

                            MainMenuUI.menuUI.RaiseOnBackButtonClick();
                        }
                        if (ForWebViewCanvas) ForWebViewCanvas.SetActive(false);
                        //_uniwebView.CleanCache();
                        //_uniwebView.Hide();

                    }
                }
            };

            // Add a method which will be invoked when the orientation changes:
            _uniwebView.OnOrientationChanged += (view, orientation) => {
                // For example it is from portrait to landscape, now it is 640x320 (width x height)
                // By setting again, we could keep the web view full screen.
                _uniwebView.Frame = new Rect(0, 0, Screen.width, Screen.height);
                _uniwebView.SetBackButtonEnabled(true);
                _uniwebView.SetShowSpinnerWhileLoading(true);
                _uniwebView.SetSpinnerText("Loading...");
                _uniwebView.SetZoomEnabled(true);
                _uniwebView.ReferenceRectTransform = myUITransform;
            };
        }
        if (_uniwebView != null)
        {
            _uniwebView.OnPageFinished += (view, statusCode, url) =>
            {
                // Page load finished
                Debug.Log("OnPageFinished is: " + view);
                Debug.Log("OnPageFinished is: statusCode" + statusCode);
                Debug.Log("OnPageFinished is: url " + url);
                AppManager.VIEW_CONTROLLER.HideLoading();
                ClearCache();
                if (url.Contains("kheltamasha.site"))
                {
                    if (url.Equals("cancel") || url.ToLower().Equals("cancel") || url.ToLower().Contains("cancel"))
                    {
                        Debug.Log("Cancel");
                        PopupMessage msg = new PopupMessage();
                        msg.Title = "Error";
                        msg.Message = "Your Deposit of Rs " + amountText.text.ToString() + " was cancelled.Please retry from below options.";
                        AppManager.VIEW_CONTROLLER.ShowPopupMessage(msg,0);
                        if (MainMenuUI.menuUI != null)
                        {

                            MainMenuUI.menuUI.RaiseOnBackButtonClick();
                        }
                        if (ForWebViewCanvas) ForWebViewCanvas.SetActive(false);
                        //_uniwebView.CleanCache();
                        //_uniwebView.Hide();
                    }
                    else if (url == ("sucess") || url.ToLower().Equals("sucess") || url == ("success") || url.ToLower().Equals("success") || url.ToLower().Contains("success") || url.ToLower().Contains("sucess"))
                    {
                        Debug.Log("sucess");
                        PopupMessage msg = new PopupMessage();
                        msg.Title = "Success";
                        msg.Message = "Your Deposit of Rs " + amountText.text.ToString() + " is Successful!!!";
                        AppManager.VIEW_CONTROLLER.ShowPopupMessage(msg,0);
                        if (MainMenuUI.menuUI != null)
                        {
                            MainMenuUI.menuUI.CloseAddCash();
                            MainMenuUI.menuUI.RaiseOnBackButtonClick();
                        }
                        if (ForWebViewCanvas) ForWebViewCanvas.SetActive(false);
                        //_uniwebView.CleanCache();
                        //_uniwebView.Hide();
                    }
                    else if (url == ("failed") || url.ToLower().Equals("failed") || url.ToLower().Contains("failed") || url.ToLower().Contains("fail"))
                    {
                        Debug.Log("failed");
                        PopupMessage msg = new PopupMessage();
                        msg.Title = "Error";
                        msg.Message = "Your Deposit of Rs " + amountText.text.ToString() + " was failed.Please retry from below options.";
                        AppManager.VIEW_CONTROLLER.ShowPopupMessage(msg,0);
                        if (MainMenuUI.menuUI != null)
                        {

                            MainMenuUI.menuUI.RaiseOnBackButtonClick();
                        }
                        if (ForWebViewCanvas) ForWebViewCanvas.SetActive(false);
                        //_uniwebView.CleanCache();
                        //_uniwebView.Hide();

                    }
                }

            };
            
            
        }

        if (_uniwebView != null)
        {
            _uniwebView.OnShouldClose += (view) => {
                _uniwebView = null;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.orientation = ScreenOrientation.Landscape;
                if (ForWebViewCanvas) ForWebViewCanvas.SetActive(false);
                //_uniwebView.Hide();
                return true;
            };

        }
        MainMenuUI.OnPayNowUI += PayNow;
        MainMenuUI.OnCloseWebViewNowUI += CloseWebView;

        if (ForWebViewCanvas) ForWebViewCanvas.SetActive(false);
        
    }
    void CloseWebView()
    {
        if (_uniwebView != null)
        {
            Destroy(_uniwebView.gameObject);
            _uniwebView = null;
        }
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.Landscape;
        if (ForWebViewCanvas) ForWebViewCanvas.SetActive(false);
    }
    private void FixedUpdate2()
    {
        if (_uniwebView != null)
        {
            if (_GoBack)
            {
                if (!_uniwebView.CanGoBack)
                {
                    _GoBack.interactable = false;
                }
                else
                {
                    _GoBack.interactable = true;
                }
            }
            if (_GoForward)
            {
                if (!_uniwebView.CanGoForward)
                {
                    _GoForward.interactable = false;
                }
                else
                {
                    _GoForward.interactable = true;
                }
            }
        }
        else
        {
            if (_GoBack)
            {
                _GoBack.interactable = false;
            }
            if (_GoForward)
            {
                _GoForward.interactable = false;
            }
        }
       
    }
    private void Update2()
    {
        if (_uniwebView != null)
        {
            if (myUITransform)
            {
                _uniwebView.ReferenceRectTransform = myUITransform;
            }
        }
    }
    public void GoBack()
    {
        if (_uniwebView != null)
        {
            if (_uniwebView.CanGoBack)
            {
                _uniwebView.GoBack();
            }
        }
        else
        {
            MyShowToastMethod("Wait Page is loading..");
        }
    }
    public void GoForward()
    {
        if (_uniwebView != null)
        {
            if (_uniwebView.CanGoForward)
            {
                _uniwebView.GoForward();
            }
        }
        else
        {
            MyShowToastMethod("Wait Page is loading..");
        }
    }
    void OnDestroy()
    {
        CloseWebView();
    }
    void OnRectTransformDimensionsChange()
    {
        if (_uniwebView != null)
        {
            // This will update web view's frame to match the reference rect transform if set.
            _uniwebView.UpdateFrame();
        }
    }
    public void Restart()
    {
        if (_uniwebView != null)
        {
            _uniwebView.Reload();
        }
        else
        {
            MyShowToastMethod("Wait Page is loading..");
        }
    }
    void ClearCache()
    {
        if (_uniwebView != null)
        {
            //_uniwebView.CleanCache();
        }
    }
    void OnDisable()
    {

        MainMenuUI.OnPayNowUI -= PayNow;
        MainMenuUI.OnCloseWebViewNowUI -= CloseWebView;
        CloseWebView();
    }
   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            CloseWebView();
            
            
        }
       Update2();
    }
    public void Clear()
    {

        CloseWebView();
        
        
    }
    public void PayNow()
    {

        ClearCache();

        AddPaymentDetails _AddPaymentDetails = new AddPaymentDetails();
        _AddPaymentDetails.firstname = StaticValues.UserNameValue.ToString();
        _AddPaymentDetails.email = "support@kheltamasha.site";// StaticValues.Email;
        _AddPaymentDetails.amount = amountText.text.ToString();
        _AddPaymentDetails.mobile = StaticValues.FirebaseUserId;
        _AddPaymentDetails.coupon_code = addCashCouponCode.text.ToString();
        _AddPaymentDetails.phone = "9876622096";// StaticValues.phoneNumberWithoutPrefix;


        try
        {
            if (!string.IsNullOrEmpty(amountText.text))
            {
                if (System.Convert.ToInt64(amountText.text) > 0)
                {
                    if (!string.IsNullOrEmpty(StaticValues.UserNameValue))
                    {
                        if (!string.IsNullOrEmpty(_AddPaymentDetails.email))
                        {
                            if (validateEmail(_AddPaymentDetails.email.ToString()))
                            {
                                if (!string.IsNullOrEmpty(_AddPaymentDetails.phone))
                                {
                                    if (_AddPaymentDetails.phone.Length >= 10)
                                    {
                                        PlayerPrefs.SetString("MyLastDeposit", amountText.text);
                                        AppManager.VIEW_CONTROLLER.ShowLoading();
                                        
                                        PostAddMoneyRequest(_AddPaymentDetails);
                                    }
                                    else
                                    {
                                        MyShowToastMethod("Please enter your correct details before proceed!!!");
                                    }
                                }
                                else
                                {
                                    MyShowToastMethod("Please complete your personal details before proceed!!!");
                                }
                            }
                            else
                            {
                                MyShowToastMethod("Please enter your correct details before proceed!!!");
                            }
                        }
                        else
                        {
                            MyShowToastMethod("Please complete your personal details before proceed!!!");
                        }
                    }
                    else
                    {
                        MyShowToastMethod("Please complete your personal details before proceed!!!");
                    }
                }
                else
                {
                    MyShowToastMethod("Please add money greater than 0");
                }
            }
            else
            {
                MyShowToastMethod("Please enter amount before proceed!!!");
            }
        }
        catch(Exception e)
        {
            MyShowToastMethod("Error : "+e.Message);
        }
       
    }
    public bool validateEmail(string email)
    {
        if (email != null)
            return Regex.IsMatch(email, MatchEmailPattern);
        else
            return false;
    }
    public void PostAddMoneyRequest(AddPaymentDetails _paytmAddPaymentDetails)
    {

        var jsonString = JsonUtility.ToJson(_paytmAddPaymentDetails) ?? "";
        StartCoroutine(PostAddMoneyRequest("https://kheltamasha.site/easypay/pay", jsonString.ToString()));
    }


    IEnumerator PostAddMoneyRequest(string url, string json)
    {
        if (debug)
        {
            Debug.Log("json in PostAddMoneyRequest" + json);
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
            NetworkError();
        }
        else
        {
            if (debug)
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
            PostAddMoneyRequest(uwr.downloadHandler.text.ToString());
        }
    }

    private string txnid = null;
    private bool ispayment = false;
    private int pollCount = 0;
    private Coroutine pollRoutine = null;
    private Coroutine checkpollRoutine = null;

    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            if(ispayment)
            {
                if(txnid != null)
                {
                    pollCount = 0;
                    if (checkpollRoutine != null)
                    {
                        StopCoroutine(checkpollRoutine);
                        checkpollRoutine = null;
                    }
                    if (pollRoutine != null)
                    {
                        StopCoroutine(pollRoutine);
                        pollRoutine = null;
                    }
                    pollRoutine = StartCoroutine(StartPolling(PlayerSave.singleton.BaseAPI + "/easypay/status/" + txnid));
                    ispayment = false;
                }
            }
        }
    }

    private IEnumerator StartPolling(string url)
    {
        if (debug)
        {
            Debug.Log("json in Checkpolling " + url);
        }
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            if (debug)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
            NetworkError();
        }
        else
        {
            if (debug)
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
            if (checkpollRoutine != null)
            {
                StopCoroutine(checkpollRoutine);
                checkpollRoutine = null;
            }
            checkpollRoutine = StartCoroutine(CheckPollResponse(uwr.downloadHandler.text));
        }
    }

    private IEnumerator CheckPollResponse(string result)
    {
        Debug.Log(result);
        PaymentData pollresponse = JsonUtility.FromJson<PaymentData>(result);
        if(pollresponse.status.ToUpper() == "SUCCESS")
        {
            AppManager.VIEW_CONTROLLER.HideLoading();
            Debug.Log("sucess");
            PopupMessage msg = new PopupMessage();
            msg.Title = "Success";
            msg.Message = "Your Deposit of Rs " + amountText.text.ToString() + " is Successful!!!";
            AppManager.VIEW_CONTROLLER.ShowPopupMessage(msg, 0);
            if (MainMenuUI.menuUI != null)
            {
                MainMenuUI.menuUI.CloseAddCash();
                MainMenuUI.menuUI.RaiseOnBackButtonClick();
            }
        }
        else if(pollresponse.status.ToUpper() == "FAILED")
        {
            AppManager.VIEW_CONTROLLER.HideLoading();
            Debug.Log("failed");
            PopupMessage msg = new PopupMessage();
            msg.Title = "Error";
            msg.Message = "Your Deposit of Rs " + amountText.text.ToString() + " was failed.Please retry from below options.";
            AppManager.VIEW_CONTROLLER.ShowPopupMessage(msg, 0);
            if (MainMenuUI.menuUI != null)
            {
                MainMenuUI.menuUI.RaiseOnBackButtonClick();
            }
        }
        else
        {
            Debug.Log("pending");
            if (pollCount < 4)
            {
                yield return new WaitForSeconds(3);
                if (pollRoutine != null)
                {
                    StopCoroutine(pollRoutine);
                    pollRoutine = null;
                }
                pollRoutine = StartCoroutine(StartPolling(PlayerSave.singleton.BaseAPI + "/easypay/status/" + txnid));
                pollCount++;
            }
            else
            {
                AppManager.VIEW_CONTROLLER.HideLoading();
                PopupMessage msg = new PopupMessage();
                msg.Title = "Pending";
                msg.Message = "Your Deposit of Rs " + amountText.text.ToString() + " is in pending state, please wait for sometime while we process your payment.";
                AppManager.VIEW_CONTROLLER.ShowPopupMessage(msg, 0);
                if (MainMenuUI.menuUI != null)
                {
                    MainMenuUI.menuUI.RaiseOnBackButtonClick();
                }
            }
        }

    }
    public void PostAddMoneyRequest(string _serverPostAddMoneyResponse)
    {
        try
        {
            string result = _serverPostAddMoneyResponse;
            Debug.Log("my result is " + result);
            PaymentStart createpayresponse = JsonUtility.FromJson<PaymentStart>(result);
            ispayment = true;
            Application.OpenURL(createpayresponse.upiUrl);
            txnid = createpayresponse.txnId;
            
            //m_HTMLString = "";
            //m_HTMLString = result;

            //LoadUrlString(m_HTMLString);  //older payment gateway
            if (debug)
            {
                Debug.Log("result  " + result);
            }



        }
        catch
        {
            NetworkError();
            if (debug)
            {
                Debug.LogWarning("File was not in correct format");

            }
        }
    }
    public void NetworkError()
    {
        MyShowToastMethod("Network Error!!!");
        AppManager.VIEW_CONTROLLER.HideLoading();

        ClearCache();

        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.Landscape;
        if (ForWebViewCanvas) ForWebViewCanvas.SetActive(false);
    }
    
    private void LoadUrlString(string m_url)
    {
        AppManager.VIEW_CONTROLLER.HideLoading();
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.orientation = ScreenOrientation.Portrait;
        if (ForWebViewCanvas) ForWebViewCanvas.SetActive(true);
        if (!string.IsNullOrEmpty(m_url))
        {
            if (_uniwebView != null)
            {
                
                _uniwebView.LoadHTMLString(m_url,PlayerSave.singleton.BaseAPI);
                _uniwebView.Show();
                var message = new UniWebViewMessage("https://" + m_url + "?key=success&key=successurl&key=retrycancelled&key=failedurl&key=Errormessage&key=pay_status");
                Debug.Log("message ......." + message.Path + "raw "+message.RawMessage + "mess " + message.Scheme);
            }
            else
            {
                if (_uniwebView == null)
                {
                    var webViewGameObject = new GameObject("AddCashUniWebView");
                    _uniwebView = webViewGameObject.AddComponent<UniWebView>();

                    _uniwebView.Frame = new Rect(0, 0, Screen.width, Screen.height);
                    _uniwebView.ReferenceRectTransform = myUITransform;
                    //_uniwebView.AddUrlScheme("https");
                    _uniwebView.LoadHTMLString(m_url, PlayerSave.singleton.BaseAPI);
                    _uniwebView.Show();

                    var message = new UniWebViewMessage("https://" + m_url+ "?key=success&key=successurl&key=retrycancelled&key=failedurl&key=Errormessage&key=pay_status");
                    Debug.Log("message ......." + message.Path + "raw " + message.RawMessage + "mess " + message.Scheme);
                }
                if (_uniwebView != null)
                {
                    _uniwebView.OnPageFinished += (view, statusCode, url) =>
                    {
                        // Page load finished
                        Debug.Log("OnPageFinished is:...... " + url + " statuscode " + statusCode);
                        AppManager.VIEW_CONTROLLER.HideLoading();
                        ClearCache();
                        if (url.Contains("kheltamasha.site"))
                        {
                            if (url.Equals("cancel") || url.ToLower().Equals("cancel") || url.ToLower().Contains("cancel"))
                            {
                                Debug.Log("Cancel");
                                PopupMessage msg = new PopupMessage();
                                msg.Title = "Error";
                                msg.Message = "Your Deposit of Rs " + amountText.text.ToString() + " was cancelled.Please retry from below options.";
                                AppManager.VIEW_CONTROLLER.ShowPopupMessage(msg,0);
                                if (MainMenuUI.menuUI != null)
                                {

                                    MainMenuUI.menuUI.RaiseOnBackButtonClick();
                                }
                                if (ForWebViewCanvas) ForWebViewCanvas.SetActive(false);
                                //_uniwebView.CleanCache();
                                //_uniwebView.Hide();
                            }
                            else if (url == ("sucess") || url.ToLower().Equals("sucess") || url == ("success") || url.ToLower().Equals("success") || url.ToLower().Contains("success") || url.ToLower().Contains("sucess"))
                            {
                                Debug.Log("sucess");
                                PopupMessage msg = new PopupMessage();
                                msg.Title = "Success";
                                msg.Message = "Your Deposit of Rs " + amountText.text.ToString() + " is Successful!!!";
                                AppManager.VIEW_CONTROLLER.ShowPopupMessage(msg,0);
                                if (MainMenuUI.menuUI != null)
                                {
                                    MainMenuUI.menuUI.CloseAddCash();
                                    MainMenuUI.menuUI.RaiseOnBackButtonClick();
                                }
                                if (ForWebViewCanvas) ForWebViewCanvas.SetActive(false);
                                //_uniwebView.CleanCache();
                                //_uniwebView.Hide();
                            }
                            else if (url == ("failed") || url.ToLower().Equals("failed") || url.ToLower().Contains("failed") || url.ToLower().Contains("fail"))
                            {
                                Debug.Log("failed");
                                PopupMessage msg = new PopupMessage();
                                msg.Title = "Error";
                                msg.Message = "Your Deposit of Rs " + amountText.text.ToString() + " was failed.Please retry from below options.";
                                AppManager.VIEW_CONTROLLER.ShowPopupMessage(msg,0);
                                if (MainMenuUI.menuUI != null)
                                {

                                    MainMenuUI.menuUI.RaiseOnBackButtonClick();
                                }
                                if (ForWebViewCanvas) ForWebViewCanvas.SetActive(false);
                                //_uniwebView.CleanCache();
                                //_uniwebView.Hide();

                            }
                        }

                    };
                    _uniwebView.OnPageErrorReceived += (view, statuscode, errorMessage) =>
                    {
                        Debug.Log("OnPageErrorReceived is:..... " + errorMessage + " statuscode " + statuscode);
                        CloseWebView();
                       
                    };
                    _uniwebView.OnMessageReceived += (view, message) => {

                        Debug.Log("OnMessageReceived is:..... " + message.RawMessage);
                        Debug.Log("OnMessageReceived is: ......" + message.Path);
                        Debug.Log("OnMessageReceived is: " + message.Scheme);
                        if (message.Path.Equals("game-over"))
                        {
                            var score = message.Args["score"];
                            Debug.Log("Your final score is: " + score);

                            // Restart the game
                        }
                        if (message.Path.Contains("kheltamasha.site"))
                        {
                            if (message.Path.Equals("cancel") || message.Path.ToLower().Equals("cancel") || message.Path.ToLower().Contains("cancel"))
                            {
                                Debug.Log("Cancel");
                                PopupMessage msg = new PopupMessage();
                                msg.Title = "Error";
                                msg.Message = "Your Deposit of Rs " + amountText.text.ToString() + " was cancelled.Please retry from below options.";
                                AppManager.VIEW_CONTROLLER.ShowPopupMessage(msg,0);
                                if (MainMenuUI.menuUI != null)
                                {

                                    MainMenuUI.menuUI.RaiseOnBackButtonClick();
                                }
                                if (ForWebViewCanvas) ForWebViewCanvas.SetActive(false);
                                //_uniwebView.CleanCache();
                                //_uniwebView.Hide();
                            }
                            else if (message.Path == ("sucess") || message.Path.ToLower().Equals("sucess") || message.Path == ("success") || message.Path.ToLower().Equals("success") || message.Path.ToLower().Contains("success") || message.Path.ToLower().Contains("sucess"))
                            {
                                Debug.Log("sucess");
                                PopupMessage msg = new PopupMessage();
                                msg.Title = "Success";
                                msg.Message = "Your Deposit of Rs " + amountText.text.ToString() + " is Successful!!!";
                                AppManager.VIEW_CONTROLLER.ShowPopupMessage(msg,0);
                                if (MainMenuUI.menuUI != null)
                                {
                                    MainMenuUI.menuUI.CloseAddCash();
                                    MainMenuUI.menuUI.RaiseOnBackButtonClick();
                                }
                                if (ForWebViewCanvas) ForWebViewCanvas.SetActive(false);
                                //_uniwebView.CleanCache();
                                //_uniwebView.Hide();
                            }
                            else if (message.Path == ("failed") || message.Path.ToLower().Equals("failed") || message.Path.ToLower().Contains("failed") || message.Path.ToLower().Contains("fail"))
                            {
                                Debug.Log("failed");
                                PopupMessage msg = new PopupMessage();
                                msg.Title = "Error";
                                msg.Message = "Your Deposit of Rs " + amountText.text.ToString() + " was failed.Please retry from below options.";
                                AppManager.VIEW_CONTROLLER.ShowPopupMessage(msg,0);
                                if (MainMenuUI.menuUI != null)
                                {

                                    MainMenuUI.menuUI.RaiseOnBackButtonClick();
                                }
                                if (ForWebViewCanvas) ForWebViewCanvas.SetActive(false);
                                //_uniwebView.CleanCache();
                                //_uniwebView.Hide();

                            }
                        }
                    };

                    // Add a method which will be invoked when the orientation changes:
                    _uniwebView.OnOrientationChanged += (view, orientation) => {
                        // For example it is from portrait to landscape, now it is 640x320 (width x height)
                        // By setting again, we could keep the web view full screen.
                        _uniwebView.Frame = new Rect(0, 0, Screen.width, Screen.height);
                        _uniwebView.SetBackButtonEnabled(true);
                        _uniwebView.SetShowSpinnerWhileLoading(true);
                        //_uniwebView.SetSpinnerText("Loading...");
                        _uniwebView.SetZoomEnabled(false);
                        _uniwebView.SetAllowFileAccessFromFileURLs(true);
                        _uniwebView.ReferenceRectTransform = myUITransform; 
                    };
                }

                if (_uniwebView != null)
                {
                    _uniwebView.OnShouldClose += (view) => {
                        _uniwebView = null;
                        Screen.autorotateToLandscapeLeft = true;
                        Screen.autorotateToLandscapeRight = true;
                        Screen.autorotateToPortrait = false;
                        Screen.autorotateToPortraitUpsideDown = false;
                        Screen.orientation = ScreenOrientation.Landscape;
                        if (ForWebViewCanvas) ForWebViewCanvas.SetActive(false);
                        //_uniwebView.Hide();
                        return true;
                    };

                }
            }
        }
        else
        {
            NetworkError();
        }
        
        
    }


    
   
    public void MyShowToastMethod(string value)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            showToastOnUiThread(value);
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


}
[Serializable]
public class AddPaymentDetails
{
    public string firstname;
    public string email;
    public string mobile;
    public string amount;
    public string coupon_code;
    public string phone;
}

[Serializable]
public class PaymentStart
{
    public string upiUrl;
    public string txnId;
}

[Serializable]
public class PaymentData
{
    public string status;
}
