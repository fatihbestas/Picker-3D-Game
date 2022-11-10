using UnityEngine;

public class Picker : MonoBehaviour
{

    public float speed; 
    public Rigidbody rb; 
    private Vector3 direction;
 
    void Start()
    {
       
        rb = this.GetComponent<Rigidbody>();
        // picker only moves in z axis for now
        direction = new Vector3(0, 0, 1f);
    }
 
 
    void FixedUpdate()
    {
        Move(direction); 
    }
 
 
    void Move(Vector3 direction)
    {
        rb.velocity = direction * speed * Time.fixedDeltaTime;
    }
 
}