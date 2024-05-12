using System;
using UnityEngine;
//using UnityEngine.Advertisements;
//using UnityEngine.Purchasing;

public class ShopCoin : MonoBehaviour//, IStoreListener
{
    //private static IStoreController m_StoreController;          // The Unity Purchasing system.
    //private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    public static string kProduct5k = "5kcoin";
    public static string kProduct15k = "15kcoins";
    public static string kProduct30k = "30kcoins";
    public static string kProduct90k = "90kcoins";     

    void Start()
    {
        //if (m_StoreController == null)        
        //    InitializePurchasing();        
    }

    public void InitializePurchasing()
    {
        //if (IsInitialized())        
        //    return;   
      
        //var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        //builder.AddProduct(kProduct5k, ProductType.Consumable);
        //builder.AddProduct(kProduct15k, ProductType.Consumable);
        //builder.AddProduct(kProduct30k, ProductType.Consumable);
        //builder.AddProduct(kProduct90k, ProductType.Consumable);    
        //UnityPurchasing.Initialize(this, builder);
    }   

    void BuyProductID(string productId)
    {
        //if (IsInitialized())
        //{            
        //    Product product = m_StoreController.products.WithID(productId);          
        //    if (product != null && product.availableToPurchase)
        //    {
        //        Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));             
        //        m_StoreController.InitiatePurchase(product);
        //    }           
        //}       
    }

    public void RestorePurchases()
    {
        //if (!IsInitialized())        
        //    return;        
     
        //if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        //{
        //    var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();            
        //    apple.RestoreTransactions((result) => {                
        //        Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
        //    });
        //}
    }   

	public void OnInitialized()//IStoreController controller, IExtensionProvider extensions)
    {
        //Debug.Log("OnInitialized: PASS");        
        //m_StoreController = controller;      
        //m_StoreExtensionProvider = extensions;
    }

	public void OnInitializeFailed()//InitializationFailureReason error)
    {        
       // Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

	public void ProcessPurchase()//PurchaseEventArgs args)
    {
        //if (String.Equals(args.purchasedProduct.definition.id, kProduct5k, StringComparison.Ordinal))
        //{           
        //    GiveReward(5000);
        //}
        //else if (String.Equals(args.purchasedProduct.definition.id, kProduct15k, StringComparison.Ordinal))
        //{
        //    GiveReward(15000);
        //}       
        //else if (String.Equals(args.purchasedProduct.definition.id, kProduct30k, StringComparison.Ordinal))
        //{
        //    GiveReward(30000);
        //}
        //else if (String.Equals(args.purchasedProduct.definition.id, kProduct90k, StringComparison.Ordinal))
        //{
        //    GiveReward(90000);
        //}
        //else
        //{
        //    Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        //}
        //return PurchaseProcessingResult.Complete;
    }


	public void OnPurchaseFailed()//Product product, PurchaseFailureReason failureReason)
    {        
        //Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }


    private bool IsInitialized()
    {
		return false;// m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void Buy5Coins()
    {   
       // BuyProductID(kProduct5k);
    }
    public void Buy15Coins()
    {
       // BuyProductID(kProduct15k);
    }
    public void Buy30Coins()
    {
       // BuyProductID(kProduct30k);
    }
    public void Buy90Coins()
    {
        //BuyProductID(kProduct90k);
    }

    public void FreeCoins()
	{
        //ShowRewardedVideo();     
    }

    void ShowRewardedVideo()
    {
        //ShowOptions options = new ShowOptions();
        //options.resultCallback = HandleShowResult;
        //Advertisement.Show("rewardedVideo", options);
    }

	private void HandleShowResult()//ShowResult result)
    {
        //if (result == ShowResult.Finished)      
        //    GiveReward(250);           
    }

    private void GiveReward(int _addMoney)
    {
        //PlayerSave.singleton.SaveNewMoney(PlayerSave.singleton.GetCurrentMoney() + _addMoney);
        //MainMenuUI menu = FindObjectOfType<MainMenuUI>();       
        //if (menu != null)
        //{
        //    menu.RefreshMoneyUI();
        //}
        //else
        //{
        //    FindObjectOfType<LocalPlayerPun>().RefreshMoneyTopBar();
        //}
    }   
		
}
