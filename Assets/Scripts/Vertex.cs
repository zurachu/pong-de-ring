using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Vertex : MonoBehaviour, IPointerClickHandler
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

    public void Initialize(InGame parent, Vector3 localPosition)
    {
        this.parent = parent;
        transform.localPosition = localPosition;
        ResetColor();
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
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
    }

    public void ResetColor()
    {
        SetColor(Color.gray);
    }
}
