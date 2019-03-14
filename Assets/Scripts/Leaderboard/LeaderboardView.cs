using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField] List<LeaderboardEntryView> entryViews;
    [SerializeField] DisplayNameRequester displayNameRequester;
    [SerializeField] ChangeDisplayNameView changeDisplayNameViewPrefab;

    List<PlayerLeaderboardEntry> leaderboardEntries;

    public void Initialize(List<PlayerLeaderboardEntry> entries)
    {
        leaderboardEntries = entries;

        var index = 0;
        foreach (var view in entryViews)
        {
            if (index < leaderboardEntries.Count)
            {
                view.gameObject.SetActive(true);
                view.Initialize(leaderboardEntries[index]);
            }
            else
            {
                view.gameObject.SetActive(false);
            }
            index++;
        }

        var myEntry = FindMyEntry(leaderboardEntries);
        if (myEntry != null && string.IsNullOrEmpty(myEntry.DisplayName))
        {
            ShowChangeDisplayNameView(null);
        }
    }

    PlayerLeaderboardEntry FindMyEntry(List<PlayerLeaderboardEntry> entries)
    {
        return entries.Find(_entry => _entry.PlayFabId == PlayFabLoginManagerSingleton.Instance.PlayFabId);
    }

    public void OnClickChangeDisplayName()
    {
        displayNameRequester.Request(ShowChangeDisplayNameView);
    }

    void ShowChangeDisplayNameView(string currentName)
    {
        var view = Instantiate(changeDisplayNameViewPrefab);
        view.Initialize(currentName, _newName => {
            var myEntry = FindMyEntry(leaderboardEntries);
            if (myEntry != null)
            {
                myEntry.DisplayName = _newName;
                Initialize(leaderboardEntries);
            }
        });
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
