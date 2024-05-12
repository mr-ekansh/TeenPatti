using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryListViewController : MonoBehaviour
{

    [SerializeField]
    private Text AmountLabel;
    [SerializeField]
    private Text PayIDLabel;
    [SerializeField]
    private Text PayDateLabel;
    [SerializeField]
    private Text PayByLabel;
    [SerializeField]
    private Text StatusLabel;
    [SerializeField]
    private string PayStatus;



    [SerializeField]
    private Color GreenColor;
    [SerializeField]
    private Color YellowColor;
    [SerializeField]
    private Color RedColor;
    private void ClearData()
    {

        AmountLabel.text = string.Empty;
        PayIDLabel.text = string.Empty;
        PayDateLabel.text = string.Empty;
        PayByLabel.text = string.Empty;
        PayStatus = string.Empty;
        StatusLabel.text = string.Empty;
        //RupeesImage.sprite = RupeesPlus;
        //PayIDLabel.color = RupeesPlusColor;
    }
    public void DisplayInfo(DepositList depositList)
    {
        
        ClearData();
        if (depositList != null)
        {
            AmountLabel.text = depositList.DepositAmount.ToString();
            
            PayDateLabel.text = depositList.DepositDate.ToString();
            PayStatus = depositList.DepositStatus.ToUpperInvariant().ToString();
           
            StatusLabel.text = PayStatus.ToString();

            if (PayStatus.Contains("PENDING"))
            {
                StatusLabel.color = YellowColor;
            }
            else if (PayStatus.Contains("REJECTED") || PayStatus.Contains("CANCELLED"))
            {
                StatusLabel.color = RedColor;
            }
            else if (depositList.DepositStatus.Equals("success"))
            {
                StatusLabel.color = GreenColor;
                
            }
            else if (depositList.DepositStatus.Equals("Success"))
            {
                StatusLabel.color = GreenColor;
            }
            else
            {
                StatusLabel.color = Color.white;
            }
        }
       
        
    }
}
