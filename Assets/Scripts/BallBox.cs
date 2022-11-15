using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallBox : MonoBehaviour
{
    [SerializeField]private GameObject endpoint;
    public TextMeshProUGUI objectCountText;
    public int requiredObjects;
    private int collectedObjects;
    private int lastCollectedObjects;
    public Animator animator;
    public bool stageEnded;
    private bool decisionMade;
    private float timeCounter;
    private float timeDelay = 3f;

    void Start()
    {
        collectedObjects = 0;
        lastCollectedObjects = 0;
        objectCountText.text = collectedObjects.ToString() + " / " + requiredObjects.ToString();
        stageEnded = false;
        decisionMade = false;
        timeCounter = timeDelay;
    }

    void Update()
    {   
        if(stageEnded && !decisionMade)
        {
            // Eğer yeterli nesne toplandıysa belirli bir süre bekledikten sonra animasyonu başlat ve
            // bu bölümün geçildiğini GameManager'a söyle.
            if(collectedObjects >= requiredObjects)
            {
                decisionMade = true;
                Invoke("StagePassed", 1f);
            }
            // eğer yeterli nesne toplanmadıysa timedelay kadar bekle ve ondan sonra stage failed durumuna geç.
            // toplanan nesne sayısı her değiştiğinde sayacı resetle. İlerde çok fazla nesne olduğu
            // zaman önemli bir durum olacak bu. Yani timedelay süresi boyunca toplanan nesne sayısı değişmezse fail olacak.
            else
            {
                timeCounter -= Time.deltaTime;

                if(lastCollectedObjects != collectedObjects)
                {
                    lastCollectedObjects = collectedObjects;
                    timeCounter = timeDelay;
                }

                if(timeCounter <= 0)
                {
                    decisionMade = true;
                    StageFailed();
                }
            }
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
        GameManager.Instance.stagePassed = true;
        animator.SetTrigger("slideGround");
        // OnTriggerEnter tekrar çalşıp da picker'ı durdurmasın diye endpoint nesnesini kapat.
        endpoint.SetActive(false);
    }

    void StageFailed()
    {
        GameManager.Instance.LevelFailed();
    }
}
