using UnityEngine;

namespace SocialApp
{
    public class SettingsController : MonoBehaviour
    {

        public void Logout()
        {
            //AppManager.NAVIGATION.RemoveListeners();
            PlayerPrefs.DeleteAll();
            AppManager.VIEW_CONTROLLER.HideAllScreen();
            AppManager.VIEW_CONTROLLER.ShowLogin();
            AppManager.DEVICE_CONTROLLER.StopOnlineChecker();
            AppManager.FIREBASE_CONTROLLER.RemoveDeviceTokens();
            AppManager.FIREBASE_CONTROLLER.RemovePushNotificationEvents();
            AppManager.LOGIN_CONTROLLER.OnSignOut();
            AppManager.FIREBASE_CONTROLLER.LogOut();
            AppManager.USER_PROFILE.ClearUser();
            AppManager.VIEW_CONTROLLER.ShowLogin();
            PlayerPrefs.DeleteAll();
            if (PlayerSave.singleton != null)
            {
                PlayerSave.singleton.ResetData();
            }
            
        }
    }
}
