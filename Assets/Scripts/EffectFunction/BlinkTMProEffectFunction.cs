using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BlinkTMProEffectFunction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var tmPro = GetComponent<TextMeshProUGUI>();
        tmPro.DOFade(0f, 0.25f).SetEase(Ease.InCubic).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
