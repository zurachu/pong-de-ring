using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreUpDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI score;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(int score, Vector2 localPosition)
    {
        this.score.text = score.ToString();
        transform.localPosition = localPosition;

        var seq = DOTween.Sequence();
        seq.Append(this.score.transform.DOMoveY(1f, 1f).SetEase(Ease.OutQuart).SetRelative());
        seq.Join(this.score.DOFade(0f, 1f));
        seq.onComplete = () => Destroy(gameObject);
        seq.Play();
    }
}
