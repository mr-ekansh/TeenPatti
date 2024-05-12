using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReferListViewController : MonoBehaviour
{

    [SerializeField]
    private Text RegDateLabel;
    [SerializeField]
    private Text FriendName;
    [SerializeField]
    private Text FriendUserName;
    [SerializeField]
    private Text BonusReleased;
    [SerializeField]
    private Text DaysLeft;
   
    private string PayStatus;

   
    private void ClearData()
    {

        RegDateLabel.text = string.Empty;
        FriendName.text = string.Empty;
        FriendUserName.text = string.Empty;
        BonusReleased.text = string.Empty;
        PayStatus = string.Empty;
        DaysLeft.text = string.Empty;
        
    }
    public void DisplayInfo(ReferList referList)
    {
        
        ClearData();
        if (referList != null)
        {

            RegDateLabel.text = referList.RegDateLabel.ToString();
            FriendName.text = referList.FriendName.ToUpperInvariant().ToString();
            FriendUserName.text = referList.FriendUserName.ToString();
            BonusReleased.text = referList.BonusReleased.ToString();
            PayStatus = referList.DaysLeft.ToString();
            DaysLeft.text = referList.DaysLeft.ToString();

        }
       
        
    }
}
