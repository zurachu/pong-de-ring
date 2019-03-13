using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    InGame parent;
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
                parent.OnGetCoin();
                alive = false;
                Destroy(gameObject); // TODO: GETアニメーションしてから消す
            }
        }
    }
}
