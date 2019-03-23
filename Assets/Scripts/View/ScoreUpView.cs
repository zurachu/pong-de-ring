using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreUpView : MonoBehaviour
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

    public void Initialize(int scoreBase, int scoreRate, Vector2 localPosition)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(scoreBase.ToString());
        if (scoreRate > 1)
        {
            stringBuilder.AppendFormat(string.Format("X{0}", scoreRate));
        }
        score.text = stringBuilder.ToString();
        transform.localPosition = localPosition;

        var seq = DOTween.Sequence();
        seq.Append(score.transform.DOMoveY(1f, 1f).SetEase(Ease.OutQuart).SetRelative());
        seq.Join(score.DOFade(0f, 1f));
        seq.onComplete = () => Destroy(gameObject);
        seq.Play();
    }
}
