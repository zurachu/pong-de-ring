using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;

public class ResultView : MonoBehaviour
{
    public static ResultView Show(Transform parent, int score, Action onClickReturn)
    {
        var view = Create(parent);
        view.Initialize(score, true, onClickReturn);
        return view;
    }

    public static ResultView Show(Transform parent, Action onClickReturn)
    {
        var view = Create(parent);
        view.Initialize(0, false, onClickReturn);
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

    void Initialize(int score, bool gameEnded, Action onClickReturn)
    {
        this.score = score;
        this.gameEnded = gameEnded;
        this.onClickReturn = onClickReturn;

        leaderboardView.gameObject.SetActive(false);
        tweetButton.SetActive(false);
        returnButton.SetActive(false);

        GetLeaderboard();
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
#if UNITY_WEBGL
        tweetButton.SetActive(gameEnded);
#endif
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

        var message = string.Format("PONG DE RING あなたのスコアは{0}点でした", score);
#if UNITY_WEBGL
        naichilab.UnityRoomTweet.Tweet("pong-de-ring", message, "unityroom", "unity1week");
#endif
    }

    public void OnClickReturn()
    {
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
