using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallBox : MonoBehaviour
{
    public TextMeshProUGUI objectCountText;
    public int requiredObjects;
    private int collectedObjects;
    public Animator animator;
    private bool stagePassed;

    void Start()
    {
        objectCountText.text = collectedObjects.ToString() + " / " + requiredObjects.ToString();
        stagePassed = false;
    }

    void Update()
    {   
        // Eğer yeterli nesne toplandıysa belirli bir süre bekledikten sonra animasyonu başlat ve
        // bu bölümün geçildiğini GameManager'a söyle.
        if(collectedObjects >= requiredObjects && !stagePassed)
        {
            stagePassed = true;
            Invoke("StagePassed", 1f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Collectible"))
        {
            // toplanabilir bir nesne temas edince sayacı güncelle.
            collectedObjects += 1;
            objectCountText.text = collectedObjects.ToString() + " / " + requiredObjects.ToString();

            // Temas edilen nesnenin Y ekseninde hareket etme özelliğini kapatalım. Böylece nesne zıplamaz
            // ve aynı nesne onCollisionEnter fonksiyonunu tekrar çalıştırmaz.
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        }
    }

    void StagePassed()
    {
        animator.SetTrigger("slideGround");
        GameManager.Instance.stagePassed = true;
    }
}
