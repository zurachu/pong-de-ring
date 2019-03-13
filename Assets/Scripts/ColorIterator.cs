using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorIterator : MonoBehaviour
{
    [SerializeField] List<Color> colors;

    int index;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Color Current => colors[index];

    public Color Next()
    {
        index++;
        if (index >= colors.Count)
        {
            index = 0;
        }
        return Current;
    }
}
