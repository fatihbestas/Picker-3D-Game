using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    public Transform target;
    private int view;
    public Vector3 offsetForView1; // 0 32 -30
    private Vector3 rotationForView1 = new Vector3(25f, 0, 0);
    public Vector3 offsetForView2; // 0 10 -16
    private Vector3 rotationForView2 = new Vector3(10f, 0, 0);
    private Vector3 tempPosition;
    public bool follow = true;

    void Start()
    {
        SetCameratoView1();
    }

    void LateUpdate()
    {
        if(follow)
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
    }

    void SetCameratoView1()
    {
        transform.rotation = Quaternion.Euler(rotationForView1);
        transform.position = new Vector3(offsetForView1.x, offsetForView1.y, transform.position.z);
        view = 1;
    }

    void SetCameratoView2()
    {
        transform.rotation = Quaternion.Euler(rotationForView2);
        transform.position = new Vector3(offsetForView2.x, offsetForView2.y, transform.position.z);
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
