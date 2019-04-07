using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleView : MonoBehaviour
{
    [SerializeField] GameObject clickToStart;
    [SerializeField] GameObject selectLevel;
    [SerializeField] GameObject privacyPolicyButton;

    public static TitleView Show(Transform parent, Action<InGame.Level> onClickStart, Action onClickResult)
    {
        var view = Create(parent);
        view.Initialize(onClickStart, onClickResult);
        return view;
    }

    static TitleView Create(Transform parent)
    {
        return Instantiate(Prefab, parent).GetComponent<TitleView>();
    }

    static GameObject Prefab
    {
        get
        {
            return Resources.Load<GameObject>("Prefabs/View/TitleView");
        }
    }

    Action<InGame.Level> onClickStart;
    Action onClickResult;

    // Start is called before the first frame update
    void Start()
    {
        clickToStart.SetActive(true);
        selectLevel.SetActive(false);
#if UNITY_WEBGL
        privacyPolicyButton.SetActive(false);
#else
        privacyPolicyButton.SetActive(true);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Initialize(Action<InGame.Level> onClickStart, Action onClickResult)
    {
        this.onClickStart = onClickStart;
        this.onClickResult = onClickResult;
    }

    public void OnClickStart()
    {
        AudioManagerSingleton.Instance.PlaySe(AudioManagerSingleton.Audio.Select);

        clickToStart.SetActive(false);
        selectLevel.SetActive(true);
    }

    public void OnClickNormal()
    {
        OnClickLevel(InGame.Level.Normal);
    }

    public void OnClickExpert()
    {
        OnClickLevel(InGame.Level.Expert);
    }

    void OnClickLevel(InGame.Level level)
    {
        AudioManagerSingleton.Instance.PlaySe(AudioManagerSingleton.Audio.OneUp);

        onClickStart?.Invoke(level);
    }

    public void OnClickResult()
    {
        AudioManagerSingleton.Instance.PlaySe(AudioManagerSingleton.Audio.Coin);

        onClickResult?.Invoke();
    }

    public void OnClickPrivacyPolicy()
    {
        Application.OpenURL("https://zurachu.github.io/pong-de-ring/privacy-policy.html");
    }
}
