using System.Collections;
using System.Collections.Generic;
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
            () => {
                inGame.StartGame();
                gameOverDisplay.SetActive(false);
                Destroy(view.gameObject);
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
