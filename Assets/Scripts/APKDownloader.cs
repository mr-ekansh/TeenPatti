using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
#if PLATFORM_ANDROID
using UnityEngine.Android;
using UnityEngine.Networking;
#endif
using UnityEngine.UI;

public class APKDownloader : MonoBehaviour
{
    // Start is called before the first frame update
    public Image VersionColor;
    public Text VersionInfoText;
    public Text VersionTextDebug;
    void Start()
    {
        //StartCoroutine(downLoadFromServerOther());
        //StartCoroutine(NewDownload());
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
#if PLATFORM_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
                
            }
            else
            {
                if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
                {
                    Permission.RequestUserPermission(Permission.ExternalStorageRead);

                }
            }
#endif
        }

    }
    IEnumerator DownloadFile()
    {
        yield return new WaitForSeconds(5f);
        string url = StaticValues.VersionUrl; //"https://kheltamasha.site/apk/kheltmasha.apk";
        var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        string savePath = Path.Combine(Application.persistentDataPath, "data");
        savePath = Path.Combine(savePath, "kheltamasha.apk");
        uwr.downloadHandler = new DownloadHandlerFile(savePath);
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError || uwr.isHttpError)
            Debug.LogError(uwr.error);
        else
            Debug.Log("File successfully downloaded and saved to " + savePath + " "+uwr.downloadedBytes);

        while (!uwr.isDone)
        {
            //Must yield below/wait for a frame
            //GameObject.Find("TextDebug").GetComponent<Text>().text = "Stat: " + uwr.downloadedBytes.ToString();
            //Debug.LogWarning($"Download Progress with {uwr.responseCode}, reason: {uwr.downloadProgress}", this);
            float downloadDataProgress = uwr.downloadProgress * 100;

            /*
             * use a float division here 
             * I don't know what type downloadDataProgress is
             * but if it is an int than you will always get 
             * an int division <somethingSmallerThan100>/100 = 0
             */
            if (VersionTextDebug) VersionTextDebug.text = "" + downloadDataProgress / 100.0f;

            print("Download: " + downloadDataProgress);
            yield return null;
        }
    }
    string url = "https://kheltamasha.site/apk/kheltmasha.apk";
    public void StartUpdate()
    {
        if (VersionColor) VersionColor.fillAmount = 0f;
        url = StaticValues.VersionUrl; // "https://kheltamasha.site/apk/kheltmasha.apk";
        try
        {
            if (VersionInfoText) VersionInfoText.text = "Downloading Progress";
            if (VersionTextDebug) VersionTextDebug.text = "0 %";
            if (VersionColor) VersionColor.fillAmount = 0f;
        }
        catch
        {

        }
        //android.permission.READ_EXTERNAL_STORAGE
        AndroidRuntimePermissions.Permission ExternalResult = AndroidRuntimePermissions.CheckPermission("android.permission.WRITE_EXTERNAL_STORAGE");

        if (ExternalResult != AndroidRuntimePermissions.Permission.Granted)
        {
            AndroidRuntimePermissions.Permission result = AndroidRuntimePermissions.RequestPermission("android.permission.WRITE_EXTERNAL_STORAGE");
            if (result == AndroidRuntimePermissions.Permission.Granted)
                Debug.Log("We have permission to access external storage!");
            else
                Debug.Log("Permission state: " + result);
        }
       
    

        // Requesting WRITE_EXTERNAL_STORAGE and CAMERA permissions simultaneously
        //AndroidRuntimePermissions.Permission[] result = AndroidRuntimePermissions.RequestPermissions( "android.permission.WRITE_EXTERNAL_STORAGE", "android.permission.CAMERA" );
        //if( result[0] == AndroidRuntimePermissions.Permission.Granted && result[1] == AndroidRuntimePermissions.Permission.Granted )
        //	Debug.Log( "We have all the permissions!" );
        //else
        //	Debug.Log( "Some permission(s) are not granted..." );
        //if (Application.isMobilePlatform)
        //{

        //    StopCoroutine(NewDownload());
        //    StartCoroutine(NewDownload());
        //}
        //else
        //{
        //    StopCoroutine(downLoadFromServer());
        //    StartCoroutine(downLoadFromServer());
          
        //}
        StopCoroutine(downLoadFromServer());
        StartCoroutine(downLoadFromServer());


    }
    public void InstallAPKPrompt()
    {
        if (VersionColor)
        { 
            if (VersionColor.fillAmount >= 0.99f)
            {
                string vidSavePath = Path.Combine(Application.persistentDataPath, "data");
                vidSavePath = Path.Combine(vidSavePath, "kheltamasha.apk");

                if (VersionInfoText)
                {
                    if (VersionInfoText.text.Contains("Error"))
                    {
                        Application.OpenURL(url);
                    }
                    else
                    {
                        if (getSDKInt() < 24)
                        {
                            //Install APK
                            installApp(vidSavePath);
                        }
                        else
                        {
                            installAppforAbove24(vidSavePath);
                        }
                    }
                }
                else
                {
                    MyShowToastMethod("Wait to finish download!!!");
                }
                
            }
            else
            {
                
                MyShowToastMethod("Wait to finish download!!!");
            }
        }
        else
        {
            MyShowToastMethod("Error!!!");
        }
        
    }
    public void MyShowToastMethod(string value)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            showToastOnUiThread(value);
        }
    }
    string toastString;
    AndroidJavaObject currentActivity;

    void showToastOnUiThread(string toastString)
    {
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        this.toastString = toastString;

        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(showToast));
    }

    void showToast()
    {
        Debug.Log("Running on UI thread");
        AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", toastString);
        AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));
        toast.Call("show");
    }

    IEnumerator NewDownload()
    {


        try
        {
            if (VersionInfoText) VersionInfoText.text = "Downloading Progress";
            if (VersionTextDebug) VersionTextDebug.text = "0 %";
            if (VersionColor) VersionColor.fillAmount = 0f;
        }
        catch
        {

        }

        string vidSavePath = Path.Combine(Application.persistentDataPath, "data");
        vidSavePath = Path.Combine(vidSavePath, "kheltamasha.apk");

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(vidSavePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(vidSavePath));
        }

        var uwr = new UnityWebRequest(url);
        uwr.method = UnityWebRequest.kHttpVerbGET;
        var dh = new DownloadHandlerFile(vidSavePath);
        dh.removeFileOnAbort = true;
        uwr.downloadHandler = dh;
        uwr.SendWebRequest();
        Debug.Log("uwr.isDone " + uwr.isDone);
        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log(uwr.error);
            Debug.LogWarning($"Download Failed with {uwr.responseCode}, reason: {uwr.error}", this);

            if (VersionInfoText) VersionInfoText.text = $"Download Failed Error: {uwr.error}";
            if (VersionTextDebug) VersionTextDebug.text = "0 %";
            if (VersionColor) VersionColor.fillAmount = 0f;

            // Cancel this Coroutine
            yield break;
        }
        else
        {
            Debug.Log("Download saved to: " + vidSavePath.Replace("/", "\\") + "\r\n" + uwr.error);
            while (!uwr.isDone)
            {
                //Must yield below/wait for a frame
                //GameObject.Find("TextDebug").GetComponent<Text>().text = "Stat: " + uwr.downloadedBytes.ToString();
                //Debug.LogWarning($"Download Progress with {uwr.responseCode}, reason: {uwr.downloadProgress}", this);
                float downloadDataProgress = uwr.downloadProgress * 100;

                /*
                 * use a float division here 
                 * I don't know what type downloadDataProgress is
                 * but if it is an int than you will always get 
                 * an int division <somethingSmallerThan100>/100 = 0
                 */
                if (VersionInfoText) VersionInfoText.text = "Downloading Progress";
                if (VersionTextDebug) VersionTextDebug.text  = (downloadDataProgress/100f).ToString("P", CultureInfo.InvariantCulture);
                if (VersionColor) VersionColor.fillAmount = downloadDataProgress / 100.0f;

                print("Download: " + downloadDataProgress);
                yield return null;
            }
            print("Download: " + uwr.downloadHandler.isDone);


            try
            {
                // Or retrieve results as binary data
                //byte[] yourBytes = uwr.downloadHandler.data;
                byte[] yourBytes = BitConverter.GetBytes(uwr.downloadedBytes);

                if (VersionInfoText) VersionInfoText.text = "Done downloading. Size: " + uwr.downloadedBytes;
            }
            catch
            {
                if (VersionInfoText) VersionInfoText.text = "Download Error ";
            }
            
        }

        if (!Directory.Exists(Path.GetDirectoryName(vidSavePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(vidSavePath));
            //if (VersionTextDebug) VersionTextDebug.text = "Created Dir";
        }

        try
        {
            //Now Save it
            System.IO.File.WriteAllBytes(vidSavePath, uwr.downloadHandler.data);
            Debug.Log("Saved Data to: " + vidSavePath.Replace("/", "\\"));
            if (VersionInfoText) VersionInfoText.text = "Download Completed!";
        }
        catch (Exception e)
        {
            Debug.Log("Failed To Save Data to: " + vidSavePath.Replace("/", "\\"));
            Debug.Log("Error: " + e.Message);
            if (VersionInfoText) VersionInfoText.text = "Download Saving Error!";
        }
        InstallAPKPrompt();



    }

   
  
    IEnumerator downLoadFromServer()
    {

        try
        {
            if (VersionInfoText) VersionInfoText.text = "Downloading Progress";
            if (VersionTextDebug) VersionTextDebug.text = "0 %";
            if (VersionColor) VersionColor.fillAmount = 0f;
        }
        catch
        {

        }
        yield return new WaitForSeconds(1f);
        string url = StaticValues.VersionUrl;// "https://kheltamasha.site/apk/kheltmasha.apk";


        string savePath = Path.Combine(Application.persistentDataPath, "data");
        savePath = Path.Combine(savePath, "kheltamasha.apk");

     


       


        UnityWebRequest www = new UnityWebRequest(url);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            if (VersionInfoText) VersionInfoText.text = $"Download Failed Error: {www.error}";
            if (VersionTextDebug) VersionTextDebug.text = "0 %";
            if (VersionColor) VersionColor.fillAmount = 0f;

        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);
            while (!www.isDone)
            {
                //Must yield below/wait for a frame
               
                float downloadDataProgress = www.downloadProgress * 100;

                /*
                 * use a float division here 
                 * I don't know what type downloadDataProgress is
                 * but if it is an int than you will always get 
                 * an int division <somethingSmallerThan100>/100 = 0
                 */
                if (VersionInfoText) VersionInfoText.text = "Downloading Progress";
                if (VersionTextDebug) VersionTextDebug.text = (downloadDataProgress / 100f).ToString("P", CultureInfo.InvariantCulture);
                if (VersionColor) VersionColor.fillAmount = downloadDataProgress / 100.0f;
                yield return null;
            }
            // Or retrieve results as binary data
            byte[] yourBytes = www.downloadHandler.data;
            if (VersionTextDebug) VersionTextDebug.text  = "Done downloading. Size: " + yourBytes.Length;
            if (VersionColor) VersionColor.fillAmount = 1f;
        }




       


        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(savePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            //if (VersionTextDebug) VersionTextDebug.text  = "Created Dir";
        }

        try
        {
            //Now Save it
            System.IO.File.WriteAllBytes(savePath, www.downloadHandler.data);
            Debug.Log("Saved Data to: " + savePath.Replace("/", "\\"));
            if (VersionTextDebug) VersionTextDebug.text  = "100 %";
        }
        catch (Exception e)
        {
            Debug.Log("Failed To Save Data to: " + savePath.Replace("/", "\\"));
            Debug.Log("Error: " + e.Message);
            if (VersionTextDebug) VersionTextDebug.text = "100 %";
             if (VersionColor) VersionColor.fillAmount = 0f;
            if (VersionInfoText) VersionInfoText.text = "Error Saving Data";
            Application.OpenURL(StaticValues.VersionUrl);
        }
        InstallAPKPrompt();
    }

    public bool installApp(string apkPath)
    {
#if UNITY_EDITOR
        return false;
#elif UNITY_ANDROID

         if (VersionInfoText) VersionInfoText.text = "Downloading....";
        try
        {
            AndroidJavaClass intentObj = new AndroidJavaClass("android.content.Intent");
            string ACTION_VIEW = intentObj.GetStatic<string>("ACTION_VIEW");
            int FLAG_ACTIVITY_NEW_TASK = intentObj.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");
            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", ACTION_VIEW);

            AndroidJavaObject fileObj = new AndroidJavaObject("java.io.File", apkPath);
            AndroidJavaClass uriObj = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uri = uriObj.CallStatic<AndroidJavaObject>("fromFile", fileObj);

            intent.Call<AndroidJavaObject>("setDataAndType", uri, "application/vnd.android.package-archive");
            intent.Call<AndroidJavaObject>("addFlags", FLAG_ACTIVITY_NEW_TASK);
            intent.Call<AndroidJavaObject>("setClassName", "com.android.packageinstaller", "com.android.packageinstaller.PackageInstallerActivity");

            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("startActivity", intent);

            if (VersionInfoText) VersionInfoText.text = "Download Completed";
            return true;
        }
        catch (System.Exception e)
        {
            if (VersionInfoText) VersionInfoText.text = "Download Error: " + e.Message;
            return false;
        }


         
#endif
    }
    static int getSDKInt()
    {
#if  UNITY_EDITOR
        return 0;
#elif  UNITY_ANDROID
       
        using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            return version.GetStatic<int>("SDK_INT");
        }
#endif
    }
    //For API 24 and above
    private bool installAppforAbove24(string apkPath)
    {
        bool success = true;
#if UNITY_EDITOR

        success = false;
#elif UNITY_ANDROID

         if (VersionInfoText) VersionInfoText.text = "Downloading....";

        try
        {
            //Get Activity then Context
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject unityContext = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

            //Get the package Name
            string packageName = unityContext.Call<string>("getPackageName");
            string authority = packageName + ".fileprovider";

            AndroidJavaClass intentObj = new AndroidJavaClass("android.content.Intent");
            string ACTION_VIEW = intentObj.GetStatic<string>("ACTION_VIEW");
            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", ACTION_VIEW);


            int FLAG_ACTIVITY_NEW_TASK = intentObj.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");
            int FLAG_GRANT_READ_URI_PERMISSION = intentObj.GetStatic<int>("FLAG_GRANT_READ_URI_PERMISSION");

            //File fileObj = new File(String pathname);
            AndroidJavaObject fileObj = new AndroidJavaObject("java.io.File", apkPath);
            //FileProvider object that will be used to call it static function
            AndroidJavaClass fileProvider = new AndroidJavaClass("androidx.core.content.FileProvider");
            //getUriForFile(Context context, String authority, File file)
            AndroidJavaObject uri = fileProvider.CallStatic<AndroidJavaObject>("getUriForFile", unityContext, authority, fileObj);

            intent.Call<AndroidJavaObject>("setDataAndType", uri, "application/vnd.android.package-archive");
            intent.Call<AndroidJavaObject>("addFlags", FLAG_ACTIVITY_NEW_TASK);
            intent.Call<AndroidJavaObject>("addFlags", FLAG_GRANT_READ_URI_PERMISSION);
            currentActivity.Call("startActivity", intent);

             if (VersionInfoText) VersionInfoText.text = "Download Completed";
        }
        catch (System.Exception e)
        {
             if (VersionInfoText) VersionInfoText.text = "Download Error: " + e.Message;
            success = false;
        }


        
#endif

        return success;
    }
}
