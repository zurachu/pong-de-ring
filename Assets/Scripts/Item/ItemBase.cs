using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemBase : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;

    protected InGame parent;
    bool alive;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(InGame parent, Vector3 position)
    {
        this.parent = parent;
        transform.localPosition = position;
        this.alive = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Ball>() != null)
        {
            if (alive)
            {
                OnHitBall();
                alive = false;

                var seq = DOTween.Sequence();
                seq.Append(sprite.transform.DOScale(1.5f, 0.5f));
                seq.Join(sprite.DOFade(0f, 0.5f));
                seq.onComplete = () => Destroy(gameObject);
                seq.Play();
            }
        }
    }

    protected virtual void OnHitBall()
    {
    }
}
