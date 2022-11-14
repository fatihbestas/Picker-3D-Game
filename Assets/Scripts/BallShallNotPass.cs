using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShallNotPass : MonoBehaviour
{
    // Top kutusunun gerisindeki topların levelin bir sonraki kısmına geçme ihtimalini
    // ortadan kaldıran script.

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
        }
    }
}
