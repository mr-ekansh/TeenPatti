using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusListViewController : MonoBehaviour
{

    [SerializeField]
    private Text RedeemDateLabel;
    [SerializeField]
    private Text RedeemBonusName;
    [SerializeField]
    private Text RedeemBonusReleased;
    [SerializeField]
    private Text PendingBonus;
    [SerializeField]
    private Text StatusLabel;
   
    private string PayStatus;

   
    private void ClearData()
    {

        RedeemDateLabel.text = string.Empty;
        RedeemBonusName.text = string.Empty;
        RedeemBonusReleased.text = string.Empty;
        PendingBonus.text = string.Empty;
        PayStatus = string.Empty;
        StatusLabel.text = string.Empty;
        
    }
    public void DisplayInfo(BonusList bonusList)
    {
        
        ClearData();
        if (bonusList != null)
        {
            
            RedeemDateLabel.text = bonusList.RedeemDateLabel.ToString();
            RedeemBonusName.text = bonusList.RedeemBonusName.ToUpperInvariant().ToString();
            RedeemBonusReleased.text = bonusList.RedeemBonusReleased.ToString();
            PayStatus = bonusList.StatusLabel.ToString();
            StatusLabel.text = PayStatus;
            PendingBonus.text = bonusList.PendingBonus.ToString();

        }
       
        
    }
}
