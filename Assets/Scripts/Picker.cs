using UnityEngine;

public class Picker : MonoBehaviour
{

    public float speedForZ_axis; 
    public Rigidbody rb; 
    private Vector3 direction;
    private Touch touch;
    private float speedModifierForX_axis = 1;
 
    void Start()
    {
       
        rb = this.GetComponent<Rigidbody>();

        direction = new Vector3(1f, 0, 1f);
    }

    void Update()
    {
        Debug.Log(TouchInputForX_axis());
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
                return touch.deltaPosition.x;
            }
        }
    }
 
    void FixedUpdate()
    {
        Move(direction); 
    }
 
 
    void Move(Vector3 direction)
    {
        rb.velocity = direction * speedForZ_axis;
    }
 
}