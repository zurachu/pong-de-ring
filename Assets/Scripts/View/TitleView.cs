﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleView : MonoBehaviour
{
    public static TitleView Show(Transform parent, Action onClickStart, Action onClickResult)
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