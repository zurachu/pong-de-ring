using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Vertex : MonoBehaviour, IPointerClickHandler
{
    static readonly float bgmBpm = 130f;
    static readonly float bgmBps = bgmBpm / 60;

    [SerializeField] SpriteRenderer body;
    [SerializeField] SpriteRenderer effect;

    InGame parent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(InGame parent, Vector3 localPosition)
    {
        this.parent = parent;
        transform.localPosition = localPosition;
        ResetColor();

        body.transform.DOScale(1.1f, 0.5f / bgmBps).SetLoops(-1, LoopType.Yoyo);
    
        var seqEffect = DOTween.Sequence();
        seqEffect.Append(effect.transform.DOScale(1f, 0f));
        seqEffect.Join(effect.DOFade(1f, 0f));
        seqEffect.Append(effect.transform.DOScale(2f, 1f / bgmBps));
        seqEffect.Join(effect.DOFade(0f, 1f / bgmBps));
        seqEffect.SetLoops(-1);
        seqEffect.Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (parent != null)
        {
            parent.OnVertexClicked(this);
        }
    }

    public void SetColor(Color color)
    {
        body.color = color;
        effect.color = color;
    }

    public void ResetColor()
    {
        SetColor(Color.gray);
    }
}
