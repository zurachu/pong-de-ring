using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
            text.text = score.ToString();
        }
    }
    int score;
}
