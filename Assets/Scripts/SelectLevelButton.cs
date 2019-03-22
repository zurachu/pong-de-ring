using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectLevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject background;

    // Start is called before the first frame update
    void Start()
    {
        background.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        background.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        background.SetActive(false);
    }
}
