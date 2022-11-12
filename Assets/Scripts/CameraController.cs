using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    private int view;
    public Vector3 offsetForView1; // 0 32 -35
    private Vector3 rotationForView1 = new Vector3(20f, 0, 0);
    public Vector3 offsetForView2; // 0 10 -16
    private Vector3 rotationForView2 = new Vector3(10f, 0, 0);

    private Vector3 tempPosition;

    void Start()
    {
        SetCameratoView1();
    }

    void Update()
    {
        if(view == 1)
        {
            // The x-axis of the camera will not change in this view.
            tempPosition = target.position + offsetForView1;
            tempPosition.x = 0;
            transform.position = tempPosition;
        }
        else
        {
            transform.position = target.position + offsetForView2;
        }
        
    }

    void SetCameratoView1()
    {
        transform.rotation = Quaternion.Euler(rotationForView1);
        view = 1;

    }

    void SetCameratoView2()
    {
        transform.rotation = Quaternion.Euler(rotationForView2);
        view = 2;
    }

    public void CameraButton()
    {
        if(view == 1)
        {
            SetCameratoView2();
        }
        else
        {
            SetCameratoView1();
        }
    }
}
