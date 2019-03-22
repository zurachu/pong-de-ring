using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Wall : MonoBehaviour
{
    [SerializeField] SpriteRenderer boundEffectPrefab;

    InGame parent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(InGame parent, Vector3 position1, Vector3 position2, Color color)
    {
        this.parent = parent;

        transform.localPosition = (position1 + position2) / 2;

        var diff = position2 - position1;
        var rad = Mathf.Atan2(diff.y, diff.x);
        transform.localRotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * rad);

        var scale = transform.localScale;
        scale.x = diff.magnitude + 0.75f;
        transform.localScale = scale;

        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            parent.OnBallHitWall(ball);
            CreateBoundEffect();
        }
    }

    void CreateBoundEffect()
    {
        var boundEffect = Instantiate(boundEffectPrefab).GetComponent<SpriteRenderer>();
        boundEffect.transform.localPosition = transform.localPosition;
        boundEffect.transform.localRotation = transform.localRotation;
        boundEffect.transform.localScale = transform.localScale;
        boundEffect.color = GetComponent<SpriteRenderer>().color;

        var seq = DOTween.Sequence();
        seq.Append(boundEffect.DOFade(1f, 0f));
        seq.Append(boundEffect.transform.DOScaleY(boundEffect.transform.localScale.y * 4, 1f));
        seq.Join(boundEffect.DOFade(0f, 1f));
        seq.onComplete = () => {
            Destroy(boundEffect.gameObject);
        };
        seq.Play();
    }
}
