using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    [SerializeField] InGame inGame;
    [SerializeField] LeaderboardRequester leaderboardRequester;
    [SerializeField] Transform uiParent;
    [SerializeField] GameObject gameOverDisplay;


    // Start is called before the first frame update
    void Start()
    {
        inGame.OnGameOver = OnGameOver;
        gameOverDisplay.SetActive(false);

        PlayFabLoginManagerSingleton.Instance.TryLogin(OnLoginSuccess, OnLoginFailure);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLoginSuccess()
    {
        ShowTitle();

        PlayFabPlayerEventManagerSingleton.Instance.Write(PlayFabPlayerEventManagerSingleton.GameStartEventName);
    }

    void OnLoginFailure(string report)
    {
        ErrorDialogView.Show("Login failed", report, () => {
            SceneManager.LoadScene(this.GetType().Name);
        });
    }

    void ShowTitle()
    {
        TitleView view = null;
        view = TitleView.Show(uiParent,
            (_level) => {
                GetTitleData((_data) => {
                inGame.StartGame(_level, new TitleConstData(_data));
                    gameOverDisplay.SetActive(false);
                    Destroy(view.gameObject);
                });
            },
            () => {
                ResultView resultView = null;
                resultView = ResultView.Show(uiParent, () => {
                    ShowTitle();
                    Destroy(resultView.gameObject);
                });
                Destroy(view.gameObject);
            });
    }

    void GetTitleData(Action<Dictionary<string, string>> onSuccess)
    {
        var connectingView = ConnectingView.Show();

        PlayFabClientAPI.GetTitleData(
            new GetTitleDataRequest(),
            _result => {
                DebugLogTitleData(_result);

                connectingView.Close();
                onSuccess?.Invoke(_result.Data);
            },
            _error => {
                var report = _error.GenerateErrorReport();
                Debug.LogError(report);

                connectingView.Close();
                ErrorDialogView.Show("GetTitleData failed", report, () => {
                GetTitleData(onSuccess);
                }, true);
            });
    }

    void DebugLogTitleData(GetTitleDataResult result)
    {
        if (result.Data != null)
        {
            var stringBuilder = new StringBuilder();
            foreach (var item in result.Data)
            {
                stringBuilder.AppendFormat(string.Format("{0}:{1}\n", item.Key, item.Value));
            }
            Debug.Log(stringBuilder);
        }
    }

    void OnGameOver()
    {
        PlayFabPlayerEventManagerSingleton.Instance.Write(PlayFabPlayerEventManagerSingleton.GameOverEventName);
        leaderboardRequester.UpdatePlayerStatistic(inGame.Score, () => {
            Invoke("StartResult", 2f);
        });

        gameOverDisplay.SetActive(true);
    }

    void StartResult()
    {
        gameOverDisplay.SetActive(false);

        ResultView view = null;
        view = ResultView.Show(uiParent, inGame.Score, () => {
            ShowTitle();
            Destroy(view.gameObject);
        });
    }
}
