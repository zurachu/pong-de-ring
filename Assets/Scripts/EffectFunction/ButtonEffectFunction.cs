using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class ButtonEffectFunction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var rectTransform = GetComponent<RectTransform>();
        rectTransform.DOScale(Vector3.one * 1.05f, 0.25f).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
