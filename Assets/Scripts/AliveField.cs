using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveField : MonoBehaviour
{
    [SerializeField] InGame parent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Ball") // 仮
        {
            parent.OnBallOutOfBounds(collider.gameObject);
        }
    }
}
