using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallBox : MonoBehaviour
{
    public TextMeshProUGUI ballCountText;
    public float requiredBalls;
    private float collectedBalls;
    

    void Start()
    {
        ballCountText.text = collectedBalls.ToString() + " / " + requiredBalls.ToString();
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Ball"))
        {
            // top temas edince sayacı güncelle.
            collectedBalls += 1;
            ballCountText.text = collectedBalls.ToString() + " / " + requiredBalls.ToString();

            // Temas edilen topun collider komponentini disable ediyoruz.
            // Böylece bir topu birden fazla kez saymayacak.
            collision.gameObject.GetComponent<SphereCollider>().enabled = false;

            // ardından topu yok ediyoruz. şimdilik sadece yok edilecek. ilerde parçalanma animasyonu
            // yapılacak.
            Destroy(collision.gameObject);   
        }
    }
}
