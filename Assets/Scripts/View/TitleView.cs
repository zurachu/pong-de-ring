using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleView : MonoBehaviour
{
    public static TitleView Show(Action onClickStart, Action onClickResult)
    {
        var view = Create();
        view.Initialize(onClickStart, onClickResult);
        return view;
    }

    static TitleView Create()
    {
        return Instantiate(Prefab).GetComponent<TitleView>();
    }

    static GameObject Prefab
    {
        get
        {
            return Resources.Load<GameObject>("Prefabs/View/TitleView");
        }
    }

    Action onClickStart;
    Action onClickResult;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Initialize(Action onClickStart, Action onClickResult)
    {
        this.onClickStart = onClickStart;
        this.onClickResult = onClickResult;
    }

    public void OnClickStart()
    {
        onClickStart?.Invoke();
    }

    public void OnClickResult()
    {
        onClickResult?.Invoke();
    }
}
