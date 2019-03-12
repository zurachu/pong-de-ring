using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : MonoBehaviour
{
    [SerializeField] InGame inGame;

    // Start is called before the first frame update
    void Start()
    {
        inGame.StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
