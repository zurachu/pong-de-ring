using System;
using System.Collections.Generic;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class LeaderboardRequester : MonoBehaviour
{
    public static readonly int MaxEntriesCount = 30;

    static readonly string StatisticName = "score";

    public void UpdatePlayerStatistic(int value, Action onSuccess, Action onCancel = null)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>() {
                new StatisticUpdate {
                    StatisticName = StatisticName,
                    Value = value
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(
            request,
            _result => {
                DebugLogUpdateStatistics(_result);

                onSuccess?.Invoke();
            },
            _error => {
                var report = _error.GenerateErrorReport();
                Debug.LogError(report);

                ErrorDialogView.Show("UpdatePlayerStatistics failed", report, () => {
                    UpdatePlayerStatistic(value, onSuccess, onCancel);
                }, onCancel);
            });
    }

    void DebugLogUpdateStatistics(UpdatePlayerStatisticsResult result)
    {
        var request = result.Request as UpdatePlayerStatisticsRequest;
        var stringBuilder = new StringBuilder();
        foreach (var statistic in request.Statistics)
        {
            stringBuilder.AppendFormat("{0}:{1}:{2}", statistic.StatisticName, statistic.Version, statistic.Value);
        }
        Debug.Log(stringBuilder);
    }

    public void Request(Action<List<PlayerLeaderboardEntry>> onReceiveLeaderboard, Action onCancel = null)
    {
        var connectingView = ConnectingView.Show();

        var request = new GetLeaderboardRequest {
            MaxResultsCount = MaxEntriesCount,
            StatisticName = StatisticName,
        };
        PlayFabClientAPI.GetLeaderboard(
            request,
            _result => {
                DebugLogLeaderboard(_result);

                connectingView.Close();
                onReceiveLeaderboard?.Invoke(_result.Leaderboard);
            },
            _error => {
                var report = _error.GenerateErrorReport();
                Debug.LogError(report);

                connectingView.Close();
                ErrorDialogView.Show("GetLeaderboard failed", report, () => {
                    Request(onReceiveLeaderboard, onCancel);
                }, onCancel);
            });
    }

    void DebugLogLeaderboard(GetLeaderboardResult result)
    {
        var stringBuilder = new StringBuilder();
        foreach (var entry in result.Leaderboard)
        {
            stringBuilder.AppendFormat("{0}:{1}:{2}:{3}\n", entry.Position, entry.StatValue, entry.PlayFabId, entry.DisplayName);
        }
        Debug.Log(stringBuilder);
    }
}
