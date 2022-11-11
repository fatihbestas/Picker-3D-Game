using UnityEngine;

public class Picker : MonoBehaviour
{

    public float speedForZ_axis; 
    public Rigidbody rb; 
    private Vector3 velocityVector;
    private Touch touch;
    private float speedForX_axis = 10;
 
    void Start()
    {
       
        rb = this.GetComponent<Rigidbody>();

        velocityVector = new Vector3(0, 0, 0);
    }

    void Update()
    {
        velocityVector.x =  speedForX_axis * TouchInputForX_axis();
    }

    float TouchInputForX_axis()
    {
        if(Input.touchCount == 0)
        {
            return 0;
        }
        else
        {
            touch = Input.GetTouch(0);

            if(touch.phase != TouchPhase.Moved)
            {
                return 0;
            }
            else
            {
                return Mathf.Sign(touch.deltaPosition.x);
            }
        }
    }
 
    void FixedUpdate()
    {
        velocityVector.z = speedForZ_axis;

        rb.velocity = velocityVector;
    }
 
}