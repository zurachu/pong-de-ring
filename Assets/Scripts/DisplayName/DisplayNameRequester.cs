using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class DisplayNameRequester : MonoBehaviour
{
    public void Request(Action<string> onReceiveDisplayName)
    {
        var connectingView = ConnectingView.Show();

        PlayFabClientAPI.GetPlayerProfile(
            new GetPlayerProfileRequest(),
            _result => {
                var playerProfile = _result.PlayerProfile;
                Debug.Log(string.Format("{0}:{1}", playerProfile.PlayerId, playerProfile.DisplayName));

                connectingView.Close();
                if (onReceiveDisplayName != null)
                {
                    onReceiveDisplayName(playerProfile.DisplayName);
                }
            },
            _error => {
                var report = _error.GenerateErrorReport();
                Debug.LogError(report);

                connectingView.Close();
                ErrorDialogView.Show("GetPlayerProfile failed", report, () => {
                    Request(onReceiveDisplayName);
                }, true);
            });
    }
}
