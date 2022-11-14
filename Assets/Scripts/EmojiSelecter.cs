using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EmojiSelecter : MonoBehaviour
{
    // pick random emoji for display
    GameObject[] emojis;
    int index;

    void Awake ()
    {
        emojis = new GameObject[transform.childCount];

        for (int i = 0; i < emojis.Length; i++)
        {   
            emojis[i] = transform.GetChild(i).gameObject;
        }
    }

    void OnEnable()
    {
        index = Random.Range(0, emojis.Length);
        emojis[index].SetActive(true);
    }

    void OnDisable()
    {
        emojis[index].SetActive(false);
    }
}
