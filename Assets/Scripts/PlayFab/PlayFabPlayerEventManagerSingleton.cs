using System.Collections.Generic;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabPlayerEventManagerSingleton
{
    static PlayFabPlayerEventManagerSingleton instance;

    public static readonly string GameStartEventName = "game_start";
    public static readonly string GameOverEventName = "game_over";

    public static PlayFabPlayerEventManagerSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayFabPlayerEventManagerSingleton();
            }
            return instance;
        }
    }

    public void Write(string eventName)
    {
        var request = new WriteClientPlayerEventRequest {
            EventName = eventName
        };
        PlayFabClientAPI.WritePlayerEvent(request, OnWriteSuccess, OnWriteFailure);
    }

    void OnWriteSuccess(WriteEventResponse response)
    {
        var request = response.Request as WriteClientPlayerEventRequest;
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat(string.Format("{0}:{1}\n", request.EventName, response.EventId));
        if (request.Body != null)
        {
            foreach (var item in request.Body)
            {
                stringBuilder.AppendFormat(string.Format("{0}:{1}\n", item.Key, item.Value));
            }
        }
        Debug.Log(stringBuilder);
    }

    void OnWriteFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
