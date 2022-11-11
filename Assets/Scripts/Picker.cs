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
        // movement in the x-axis depending on the user's drag input.
        velocityVector.x =  speedForX_axis * TouchInputForX_axis();

        // for editor debug
        if(Input.GetKey(KeyCode.A))
        {
            velocityVector.x =  -speedForX_axis;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            velocityVector.x =  speedForX_axis;
        }
    }

    float TouchInputForX_axis()
    {
        // if there is a drag input, return its direction.
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
        // The picker is constantly moving in the Z axis.
        velocityVector.z = speedForZ_axis;

        // Apply the calculated velocity vector to the picker.
        rb.velocity = velocityVector;
    }
 
}