using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLoginManagerSingleton
{
    static PlayFabLoginManagerSingleton instance;

    static readonly string GuidKey = "Guid";

    LoginResult result;

    public static PlayFabLoginManagerSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayFabLoginManagerSingleton();
            }
            return instance;
        }
    }

    public string PlayFabId
    {
        get
        {
            return result.PlayFabId;
        }
    }

    public bool LoggedIn
    {
        get
        {
            return result != null;
        }
    }

    public void TryLogin(Action onSuccess, Action<string> onFailure)
    {
        // Inspector で設定
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            if (onFailure != null)
            {
                onFailure("PlayFabSettings.TitleId is not set");
            }
            return;
        }

        Action<LoginResult> resultCallback = _result =>
        {
            Debug.Log(_result.PlayFabId);
            result = _result;
            if (onSuccess != null)
            {
                onSuccess();
            }
        };
        Action<PlayFabError> errorCallback = _error =>
        {
            var report = _error.GenerateErrorReport();
            Debug.LogError(report);
            if (onFailure != null)
            {
                onFailure(report);
            }
        };

#if !UNITY_EDITOR && UNITY_ANDROID
        TryLoginAndroid(resultCallback, errorCallback);
#else
        TryLoginDefault(resultCallback, errorCallback);
#endif
    }

    void TryLoginAndroid(Action<LoginResult> resultCallback, Action<PlayFabError> errorCallback)
    {
        var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        var contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
        var secure = new AndroidJavaClass("android.provider.Settings$Secure");
        var androidId = secure.CallStatic<string>("getString", contentResolver, "android_id");
        var request = new LoginWithAndroidDeviceIDRequest {
            AndroidDeviceId = androidId,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithAndroidDeviceID(request, resultCallback, errorCallback);
    }

    void TryLoginDefault(Action<LoginResult> resultCallback, Action<PlayFabError> errorCallback)
    {
        // WebGL では端末 ID 的なものがなく、スコアランキング程度で Facebook 等連携してもらうのもユーザに手間をかけるので、
        // 簡易な端末 ID もどきとして。
        var guid = PlayerPrefs.GetString(GuidKey);
        if (string.IsNullOrEmpty(guid))
        {
            guid = Guid.NewGuid().ToString("D");
            PlayerPrefs.SetString(GuidKey, guid);
            PlayerPrefs.Save();
        }
        Debug.Log(guid);

        var request = new LoginWithCustomIDRequest {
            CustomId = guid,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, resultCallback, errorCallback);
    }
}
