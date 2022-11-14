using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject[] levels;
    int currentLevel;
    public bool stagePassed;

    void Start()
    {
        stagePassed = false;
        
    }

    
    void Update()
    {
        
    }
}
