using System;
using System.Collections;
using UnityEngine;

namespace SocialApp
{

    public class DeviceController : MonoBehaviour
    {

        public string GetSystemDate()
        {
            return System.DateTime.Now.ToString(AppManager.APP_SETTINGS.SystemDateFormat);
        }
        public DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(unixTimeStamp).DateTime.ToLocalTime();
            // return new DateTime(1970, 1, 1).Add(TimeSpan.FromSeconds(unixTimeStamp));
            //// Unix timestamp is seconds past epoch
            //System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            //dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            //return dtDateTime;
        }
        private DateTime FromUnixTime(long unixDateTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixDateTime).DateTime.ToLocalTime();
        }
        public static DateTime ToDateTime(long unixTime)
        {
            return new DateTime(1970, 1, 1).Add(TimeSpan.FromSeconds(unixTime));
        }
        public void UnloadAssets()
        {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        public void StartOnlineChecker()
        {
            //StartCoroutine(OnStartOnlineChecker());
        }

        private IEnumerator OnStartOnlineChecker()
        {
            while (true)
            {
                AppManager.FIREBASE_CONTROLLER.UpdateUserActivity();
                yield return new WaitForSeconds(AppManager.APP_SETTINGS.UpdateActivityInterval);
            }
        }

        public void StopOnlineChecker()
        {
            StopAllCoroutines();
        }
    }
}