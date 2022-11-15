using UnityEngine;

public class Picker : Singleton<Picker>
{
    public float speedForZ_axis;
    private Rigidbody rb;
    private Vector3 velocityVector;
    private Vector3 positionVector;
    private float speedForX_axis = 20;
    private float speedCoeffForX = 1;
    private bool reachedEndPoint = false;
    private bool objectsPushed = false;
    private bool moveToNextLevel = false;
    private int currentLevel;
    private Transform startingPoint;
    public Transform backWall;
    private Vector3 backWallInitialPosition;
 
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
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            velocityVector.x =  -speedForX_axis / 2;
        }
        else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            velocityVector.x =  speedForX_axis / 2;
        }
    }

    void FixedUpdate()
    {   

        // when this level is finished go to the next level.
        if(moveToNextLevel)
        {
            MoveToNextLevel();

            // when picker going to the next level user can't control x velocity.
            velocityVector.x = 0;
        }

        // If the end of the level is not reached,
        // The picker is constantly moving in the Z axis.
        if(reachedEndPoint)
        {
            velocityVector.z = 0;

            // push objects out.
            PushObjects();
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
        // prevent the collector from leaving the platform.
        positionVector = transform.position;
        positionVector.x = Mathf.Clamp(positionVector.x, -5.5f, 5.5f);
        positionVector.y = 0.03f;
        transform.position = positionVector;
    }

    void PlaceAtStartingPoint()
    {
        // Place the picker at current level starting point.
        currentLevel = GameManager.Instance.currentLevel;
        startingPoint = GameManager.Instance.GetLevelGO(currentLevel).GetComponent<LevelData>().startingPoint;
        transform.position = startingPoint.position;
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

    public void Stop()
    {
        reachedEndPoint = true;
        objectsPushed = false;
        backWallInitialPosition = backWall.localPosition;
    }

    void PushObjects()
    {
        // push the objects and then return the wall to its starting position.
        if(!objectsPushed)
        {
            backWall.localPosition = new Vector3(backWall.localPosition.x, backWall.localPosition.y, backWall.localPosition.z + 0.1f);

            if(backWall.localPosition.z >= 5)
            {
                objectsPushed = true;
                backWall.localPosition = backWallInitialPosition;
            }
        }
        
    }

    public void GoToNextStage()
    {
        reachedEndPoint = false;
    }

    public void GoToNextLevel()
    {
        currentLevel = GameManager.Instance.currentLevel;
        startingPoint = GameManager.Instance.GetLevelGO(currentLevel).GetComponent<LevelData>().startingPoint;
        moveToNextLevel = true;
        // güzel bir görüntü oluşması için toplayıcı bir sonraki levele giderken 
        // kameranın onu takip etmesini istemiyorum.
        CameraController.Instance.follow = false;
    }

    public void MoveToNextLevel()
    {
        if(moveToNextLevel)
        {
            Vector3 dir = startingPoint.position - transform.position;
            float distanceThisFrame = 40f * Time.fixedDeltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                transform.position = startingPoint.position;
                moveToNextLevel = false;
                CameraController.Instance.follow = true;
            }
            else
            {
                transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            }
        }
    } 
}
