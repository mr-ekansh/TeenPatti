using UnityEngine;
using UnityEngine.UI;
//using Facebook.Unity;
//using UnityEngine.Advertisements;

public class PanelAfterGame : MonoBehaviour
{

    public Text textMoney;
    
    // Use this for initialization
    void Start()
    {
        if (PlayerSave.singleton.currentTable == eTable.Standard || PlayerSave.singleton.currentTable == eTable.Private)
        {
            double _currentMoney = PlayerSave.singleton.GetCurrentMoney();
            double _beforeMoney = PlayerSave.singleton.GetMoneyBeforeGame();
            double difference = _currentMoney - _beforeMoney;

            if (difference > 0)
            {
                textMoney.text = "Win money " + difference.ToString("F2");
            }
            else
            {
                textMoney.text = "Lose money " + difference.ToString("F2");
            }
            //if (PlayerSave.singleton != null)
            //{
            //    PlayerSave.singleton.CallGameExit(difference);
            //}
        }
        else
        {

            double _currentChips = PlayerSave.singleton.GetCurrentChips();
            double _beforeChips = PlayerSave.singleton.GetChipsBeforeGame();
            double difference2 = _currentChips - _beforeChips;

            if (difference2 > 0)
            {
                textMoney.text = "Win chips " + difference2.ToString("F2");
            }
            else
            {
                textMoney.text = "Lose chips " + difference2.ToString("F2");
            }

            //if (PlayerSave.singleton != null)
            //{
            //    PlayerSave.singleton.CallGameExit(difference2);
            //}
        }
        //shareFB.onClick.AddListener(ShareFacebook);
        //ShowOptions options = new ShowOptions();
        //options.resultCallback = HandleShowResult;
        //Advertisement.Show("rewardedVideo", options);
    }

	private void HandleShowResult()//ShowResult result)
    {       
    }

    public void ShareFacebook()
    {
        double _currentMoney = PlayerSave.singleton.GetCurrentMoney();
        double _beforeMoney = PlayerSave.singleton.GetMoneyBeforeGame();
        double difference = _currentMoney - _beforeMoney;
        //FB.ShareLink(new System.Uri(Url.facebook), "Teen Patti War", "I played in Teen Patti War and won "+ difference.ToString());
    }


}
