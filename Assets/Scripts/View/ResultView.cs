using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Advertisements;

public class ResultView : MonoBehaviour
{
    public static ResultView Show(Transform parent, TitleConstData titleConstData, int score, Action onClickReturn)
    {
        var view = Create(parent);
        view.Initialize(titleConstData, score, true, onClickReturn);
        return view;
    }

    public static ResultView Show(Transform parent, Action onClickReturn)
    {
        var view = Create(parent);
        view.Initialize(null, 0, false, onClickReturn);
        return view;
    }

    static ResultView Create(Transform parent)
    {
        return Instantiate(Prefab, parent).GetComponent<ResultView>();
    }

    static GameObject Prefab
    {
        get
        {
            return Resources.Load<GameObject>("Prefabs/View/ResultView");
        }
    }

    [SerializeField] LeaderboardRequester leaderboardRequester;
    [SerializeField] LeaderboardView leaderboardView;
    [SerializeField] GameObject tweetButton;
    [SerializeField] GameObject returnButton;

    string tweetScoreFormat;
    string tweetMessage;
    int score;
    bool gameEnded;
    Action onClickReturn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Initialize(TitleConstData titleConstData, int score, bool gameEnded, Action onClickReturn)
    {
        if (titleConstData != null)
        {
            this.tweetScoreFormat = titleConstData.TweetScoreFormat;
            this.tweetMessage = titleConstData.TweetMessage;
        }
        this.score = score;
        this.gameEnded = gameEnded;
        this.onClickReturn = onClickReturn;

        leaderboardView.gameObject.SetActive(false);
        tweetButton.SetActive(false);
        returnButton.SetActive(false);

        GetLeaderboard();

        if (gameEnded && Advertisement.IsReady())
        {
            var options = new ShowOptions();
            Advertisement.Show(options);
        }
    }

    void GetLeaderboard()
    {
        leaderboardRequester.Request(SetupLeaderboard);
    }

    void SetupLeaderboard(List<PlayerLeaderboardEntry> leaderboardEntries)
    {
        if (gameEnded)
        {
            if (LeaderboardIsNotUpdatedYet(leaderboardEntries))
            {
                Debug.Log("Leaderboard does not updated yet.");
                Invoke("GetLeaderboard", 0.5f);
                return;
            }
        }

        leaderboardView.Initialize(leaderboardEntries);
        leaderboardView.gameObject.SetActive(true);
        tweetButton.SetActive(gameEnded);
        returnButton.SetActive(true);
    }

    bool LeaderboardIsNotUpdatedYet(List<PlayerLeaderboardEntry> leaderboardEntries)
    {
        var myEntry = leaderboardEntries.Find(_entry => _entry.PlayFabId == PlayFabLoginManagerSingleton.Instance.PlayFabId);
        if (myEntry != null)
        {
            return myEntry.StatValue < score;
        }
        else
        {
            if (leaderboardEntries.Count < LeaderboardRequester.MaxEntriesCount)
            {
                return true;
            }
            return leaderboardEntries[leaderboardEntries.Count - 1].StatValue < score;
        }
    }

    public void OnClickTweet()
    {
        if (!gameEnded)
        {
            return;
        }

        AudioManagerSingleton.Instance.PlaySe(AudioManagerSingleton.Audio.OneUp);

        var message = string.Format(tweetScoreFormat, score);
#if UNITY_WEBGL
        naichilab.UnityRoomTweet.Tweet("pong-de-ring", message, "unityroom", "unity1week");
#else
        message += "\n";
        message += tweetMessage;
        Application.OpenURL("http://twitter.com/intent/tweet?text=" + WWW.EscapeURL(message));
#endif
    }

    public void OnClickReturn()
    {
        AudioManagerSingleton.Instance.PlaySe(AudioManagerSingleton.Audio.Cancel);

        onClickReturn?.Invoke();
    }

    public void OnClickReturnIfGameNotStarted()
    {
        if (!gameEnded)
        {
            OnClickReturn();
        }
    }
}
