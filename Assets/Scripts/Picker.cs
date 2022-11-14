using UnityEngine;

public class Picker : Singleton<Picker>
{
    public float speedForZ_axis; 
    public Rigidbody rb; 
    private Vector3 velocityVector;
    private Vector3 positionVector;
    private float speedForX_axis = 20;
    private float speedCoeffForX = 1;
    private bool reachedEndPoint = false;
    private int currentLevel;
    private Transform startingPoint;
 
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        velocityVector = new Vector3(0, 0, 0);

        PlaceAtStartingPoint();
    }

    void Update()
    {
        // movement in the x-axis depending on the user's drag input.
        velocityVector.x =  speedCoeffForX * TouchInputForX_axis();

        velocityVector.x = Mathf.Clamp(velocityVector.x, -speedForX_axis, speedForX_axis);

        // for editor debug
        if(Input.GetKey(KeyCode.A))
        {
            velocityVector.x =  -speedForX_axis / 2;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            velocityVector.x =  speedForX_axis / 2;
        }
    }

    void PlaceAtStartingPoint()
    {
        // Place the picker at current level starting point.
        currentLevel = GameManager.Instance.currentLevel;
        startingPoint = GameManager.Instance.GetLevelGO(currentLevel).GetComponent<LevelData>().startingPoint;

    }

    float TouchInputForX_axis()
    {
        if(Input.touchCount != 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            return (Input.GetTouch(0).deltaPosition.x / Screen.width) * 1000;
        }
        else
        {
            return 0;
        }
    }
 
    void FixedUpdate()
    {   
        // If the end of the level is not reached,
        // The picker is constantly moving in the Z axis.
        if(reachedEndPoint)
        {
            velocityVector.z = 0;
        }
        else
        {
            velocityVector.z = speedForZ_axis;
        }

        // Apply the calculated velocity vector to the picker.
        rb.velocity = velocityVector;
    }

    void LateUpdate()
    {
        positionVector = transform.position;
        positionVector.x = Mathf.Clamp(positionVector.x, -5.5f, 5.5f);
        transform.position = positionVector;
    }

    public void Stop()
    {
        reachedEndPoint = true;
    }

    public void MoveToNextStage()
    {
        reachedEndPoint = false;
    }
 
}