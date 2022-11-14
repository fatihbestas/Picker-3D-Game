using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int currentLevel;

    // eğer yüklenecek veri yoksa kullanılacak olan default değerler.
    public PlayerData()
    {
        currentLevel = 1;
    }
}
