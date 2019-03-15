using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
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

        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
            parent.OnBallHitWall();
            ball.Bound();
        }
    }
}
