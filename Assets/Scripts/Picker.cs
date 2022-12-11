using UnityEngine;

public class Picker : Singleton<Picker>
{
    public float speedForZ_axis;
    private Rigidbody rb;
    private Vector3 forceVector;
    private Vector3 positionVector;
    private float SpeedForX_axis;
    private float speedCoeffForX = 2;
    private float maxSpeedForX_axis = 17f;
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

        forceVector = new Vector3(0, 0, 0);

        PlaceAtStartingPoint();
    }

    void Update()
    {
        // movement in the x-axis depending on the user's drag input.
        SpeedForX_axis =  speedCoeffForX * TouchInputForX_axis();

        SpeedForX_axis = Mathf.Clamp(SpeedForX_axis, -maxSpeedForX_axis, maxSpeedForX_axis);

        // for editor debug
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            SpeedForX_axis =  -maxSpeedForX_axis;
        }
        else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            SpeedForX_axis =  maxSpeedForX_axis;
        }

        forceVector.x = SpeedController(SpeedForX_axis, rb.velocity.x);

    }

    void FixedUpdate()
    {   

        // when this level is finished go to the next level.
        if(moveToNextLevel)
        {
            MoveToNextLevel();

            // when picker going to the next level user can't control x velocity.
            forceVector.x = 0;
        }

        // If the end of the stage is not reached,
        // The picker is constantly moving in the Z axis.
        if(reachedEndPoint)
        {
            // hedef noktada beklerken z ekseninde hareket etmesine izin verme.
            if(rb.velocity.z != 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            }

            forceVector.z = 0;

            // push objects out.
            PushObjects();
        }
        else
        {
            forceVector.z = SpeedController(speedForZ_axis, rb.velocity.z);
        }

        rb.AddForce(forceVector, ForceMode.Acceleration);
    }

    float SpeedController(float desiredSpeed, float currentSpeed)
    {
        float force;
        float maxForce = 100f;
        float error = desiredSpeed - currentSpeed;
        float kp = 10;

        // Oransal kontrol şu an güzel çalışıyor. 
        // O yüzden integral ve türev kontrollerine şimdilik gerek yok.
        force = error * kp;

        force = Mathf.Clamp(force, -maxForce, maxForce);

        return force;
    }

    void LateUpdate()
    {
        // prevent the picker from leaving the platform.
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

        // hedef noktaya ulaşınca z eksenindeki hareketi hemen durdur.
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
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

    public void Move()
    {
        reachedEndPoint = false;
    }

    public void GoToNextLevel()
    {
        currentLevel = GameManager.Instance.currentLevel;
        startingPoint = GameManager.Instance.GetLevelGO(currentLevel).GetComponent<LevelData>().startingPoint;
        moveToNextLevel = true;
        // Güzel bir görüntü oluşması için toplayıcı bir sonraki levele giderken 
        // kameranın onu takip etmesini istemiyorum. Eskiden ataride oynadığım bir
        // yarış oyununda öyleydi.
        CameraController.Instance.follow = false;

        // bir sonraki levele gitmeden önce x eksenindeki hızı sıfırla.
        rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
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
                GameManager.Instance.OpenLevelCompleteScreen();
            }
            else
            {
                transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            }
        }
    } 
}
