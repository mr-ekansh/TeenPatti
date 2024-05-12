using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WithdrawListViewController : MonoBehaviour
{

    [SerializeField]
    private Text AmountLabel;
    [SerializeField]
    private Text PayIDLabel;
    [SerializeField]
    private Text PayACHolderLabel;
    [SerializeField]
    private Text PayACNumberLabel;
    [SerializeField]
    private Text StatusLabel;
    [SerializeField]
    private Text DateLabel;
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
        PayACHolderLabel.text = string.Empty;
        PayACNumberLabel.text = string.Empty;
        PayStatus = string.Empty;
        StatusLabel.text = string.Empty;
        DateLabel.text = string.Empty;
        StatusLabel.color = Color.white;
    }
    public void DisplayInfo(WithdrawList withdrawList)
    {
        
        ClearData();
        if (withdrawList != null)
        {
            //PayIDLabel.text = withdrawList.Id.ToString();
            AmountLabel.text = withdrawList.WithdrawAmount.ToString();
          
            PayStatus = withdrawList.WithdrawStatus.ToUpperInvariant().ToString();
            StatusLabel.text = PayStatus;
            DateLabel.text = withdrawList.WithdrawDate.ToString();

            if(PayStatus.Contains("PENDING"))
            {
                StatusLabel.color = YellowColor;
            }
            else if (PayStatus.Contains("REJECTED"))
            {
                StatusLabel.color = RedColor;
            }
            else if (withdrawList.WithdrawStatus.Equals("success"))
            {
                StatusLabel.color = GreenColor;
                StatusLabel.text = "PAID";
            }
            else if (PayStatus.Contains("PAID"))
            {
                StatusLabel.color = GreenColor;
                StatusLabel.text = "PAID";
            }
            else if (withdrawList.WithdrawStatus.Equals("Success"))
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
