using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BonusNewListViewController : MonoBehaviour
{

    [SerializeField]
    private Text BonusLabel;
    [SerializeField]
    private Sprite ImageBody;
    [SerializeField]
    private Image BonusName;

    private string PayStatus;
    private string BonusCode;
   
    private void ClearData()
    {

        BonusLabel.text = "Deposit Now";
       
        PayStatus = string.Empty;
        BonusCode = string.Empty;


    }
    public void CopyBonusCode()
    {
        UniClipboard.SetText(BonusCode);



        UniClipboard.GetText();

        //PlayerSave.singleton.ShowErrorMessage("Bonus Code is : " + UniClipboard.GetText());

        if(MainMenuUI.menuUI!=null)
        {
            MainMenuUI.menuUI.PasteCode();
            MainMenuUI.menuUI.ApplyBonusCode();
           
        }

        if(MainMenuUI.menuUI!=null)
        {
            if (MainMenuUI.menuUI.Panel_BonusCash) MainMenuUI.menuUI.Panel_BonusCash.SetActive(false);
        }
    }
    private IEnumerator OnLoadGraphic(GetBannerImageDetail getBannerImageDetail)
    {
        string _url = PlayerSave.singleton.BaseAPI + "" + getBannerImageDetail.benner_source;
        Debug.Log(_url);
        if (!string.IsNullOrEmpty(_url) && (_url.StartsWith("http") || _url.StartsWith("file")))
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(_url);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D _texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                if (_texture != null)
                {
                    ImageBody = Sprite.Create(_texture, new Rect(0.0f, 0.0f, _texture.width, _texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                    BonusCode = getBannerImageDetail.couponcode.ToString();
                    BonusName.sprite = ImageBody;
                }
            }
        }
    }
    public void DisplayInfo(GetBannerImageDetail bonusList)
    {
        
        ClearData();
        if (bonusList != null)
        {

            BonusCode = bonusList.couponcode.ToString();
           
            PayStatus = bonusList.benner_source.ToString();

            Debug.Log("gameObject.activeInHierarchy " + gameObject.activeInHierarchy);
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(OnLoadGraphic(bonusList));
            }
        }
       
        
    }
    public void OnDisable()
    {
        StopAllCoroutines();
    }
}
