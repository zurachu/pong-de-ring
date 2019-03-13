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

    public void Initialize(InGame parent)
    {
        this.parent = parent;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ball") // 仮
        {
            parent.OnBallHitWall();
        }
    }

    public void SetColor(Color color)
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
    }
}
