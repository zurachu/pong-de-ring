using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] LeaderboardEntryView entryViewPrefab;
    [SerializeField] DisplayNameRequester displayNameRequester;
    [SerializeField] ChangeDisplayNameView changeDisplayNameViewPrefab;

    List<PlayerLeaderboardEntry> leaderboardEntries;
    List<LeaderboardEntryView> leaderboardEntryViews;

    public void Initialize(List<PlayerLeaderboardEntry> entries)
    {
        leaderboardEntries = entries;
        leaderboardEntryViews = new List<LeaderboardEntryView>();

        var entryViewHeight = entryViewPrefab.GetComponent<RectTransform>().sizeDelta.y;
        var position = new Vector3();
        foreach (var entry in leaderboardEntries)
        {
            var entryView = Instantiate(entryViewPrefab, scrollRect.content);
            entryView.Initialize(entry);
            entryView.transform.localPosition = position;
            leaderboardEntryViews.Add(entryView);

            position.y -= entryViewHeight;
        }
        var contentRectTransform = scrollRect.content.GetComponent<RectTransform>();
        var contentSize = contentRectTransform.sizeDelta;
        contentSize.y = -position.y;
        contentRectTransform.sizeDelta = contentSize;

        var myEntryIndex = FindMyEntryIndex();
        if (myEntryIndex >= 0)
        {
            var y = scrollRect.GetComponent<RectTransform>().sizeDelta.y - entryViewHeight * (myEntryIndex + 1);
            scrollRect.verticalNormalizedPosition = 1f + ((float)y / contentSize.y);
            if (scrollRect.verticalNormalizedPosition > 1f)
            {
                scrollRect.verticalNormalizedPosition = 1f;
            }

            if (string.IsNullOrEmpty(leaderboardEntries[myEntryIndex].DisplayName))
            {
                ShowChangeDisplayNameView(null);
            }
        }
    }

    int FindMyEntryIndex()
    {
        return leaderboardEntries.FindIndex(_entry => _entry.PlayFabId == PlayFabLoginManagerSingleton.Instance.PlayFabId);
    }

    public void OnClickChangeDisplayName()
    {
        displayNameRequester.Request(ShowChangeDisplayNameView);
    }

    void ShowChangeDisplayNameView(string currentName)
    {
        var view = Instantiate(changeDisplayNameViewPrefab);
        view.Initialize(currentName, _newName => {
            var myEntryIndex = FindMyEntryIndex();
            if (myEntryIndex >= 0)
            {
                leaderboardEntries[myEntryIndex].DisplayName = _newName;
                leaderboardEntryViews[myEntryIndex].Initialize(leaderboardEntries[myEntryIndex]);
            }
        });
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
