using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEndPoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // When the picker reaches the end of the level, it stops and releases the balls.
        // BallBox starts counting collected objects and decides stage passed or failed.
        if(other.CompareTag("Picker"))
        {
            Picker.Instance.Stop();
            transform.parent.gameObject.GetComponent<BallBox>().stageEnded = true;
        }
    }
}
