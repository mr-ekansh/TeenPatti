using SocialApp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitySMSReceiver : MonoBehaviour
{
    public InputField newText;
    public static UnitySMSReceiver sMSReceiver;
    public delegate void FetchOtpNumber(string otp_no);
    public static event FetchOtpNumber fetchOtpNumber;
    private void OnEnable()
    {
        sMSReceiver = this;
    }
   
    public void CallFromSignOut()
    {
        int savedCred = PlayerPrefs.GetInt(Utils.LOGGED, Utils.NONE);
        if (savedCred == 0)
        {
#if !UNITY_EDITOR && UNITY_ANDROID
               onMobileHint();
#endif
        }
    }
    public void onMobileHint()
    {
        AndroidJavaClass ajo = new AndroidJavaClass("com.abk.cashfeeplugin.CashFeeHelper");

        //Debug.Log("className "+ className );
        string className = "startMobileNo";
        ajo.CallStatic(className, paymemt_Data().ToString());
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        int savedCred = PlayerPrefs.GetInt(Utils.LOGGED, Utils.NONE);
        if (savedCred == 0)
        {
#if !UNITY_EDITOR && UNITY_ANDROID
                onMobileHint();
#endif
        }



    }
    //data for plugin
    JSONObject paymemt_Data()
    {
        JSONObject j = new JSONObject();


        j.AddField("stage", "PROD");

        return j;
    }

    public void OnSMSReceive(string message)
    {
        // do something with your SMS message here.
        newText.text = message;
        if (!string.IsNullOrWhiteSpace(newText.text))
        {
            if (newText.text.Length > 10)
            {

                if (newText.text.StartsWith("+91"))
                {
                    newText.text = newText.text.Remove(0, 3);

                }
                else if (newText.text.StartsWith("091"))
                {
                    newText.text = newText.text.Remove(0, 3);

                }
                else if (newText.text.StartsWith("0"))
                {
                    newText.text = newText.text.Remove(0, 1);

                }
                else if (newText.text.StartsWith("91"))
                {
                    newText.text = newText.text.Remove(0, 2);

                }
                else if (newText.text.StartsWith("+091"))
                {
                    newText.text = newText.text.Remove(0, 4);

                }
                else if (newText.text.Length == 11)
                {
                    newText.text = newText.text.Remove(0, 1);
                }
                else if (newText.text.Length == 12)
                {
                    newText.text = newText.text.Remove(0, 2);
                }
                else if (newText.text.Length == 13)
                {
                    newText.text = newText.text.Remove(0, 3);
                }
                else if (newText.text.Length == 14)
                {
                    newText.text = newText.text.Remove(0, 4);
                }

            }
            else if (newText.text.Length == 10)
            {

            }
            else
            {
                newText.text = "";
            }
        }
        else
        {
            newText.text = "";
        }
       

    }
    internal string otpText;
    public void OnOTPReceive(string message)
    {
        // do something with your SMS message here.
        Debug.Log("OnOTPReceive " + message);
        if (!string.IsNullOrEmpty(message))
        {
            if (message.Contains("Khel Tamasha") || message.Contains("KHELTAMASHA"))
            {
                string[] pin = message.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (pin != null)
                {
                    if (pin.Length > 0)
                    {
                        for (int i = 0; i < pin.Length; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(pin[i]))
                            {
                                if (pin[i].Any(char.IsDigit))
                                {
                                    // NOTE : Here I'm passing received OTP to Portable Project using MessagingCenter. So I can display the OTP in the relevant entry field.
                                    otpText = pin[i];
                                    if (otpText.Length == 6)
                                    {
                                        fetchOtpNumber?.Invoke(otpText);
                                        break;
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
