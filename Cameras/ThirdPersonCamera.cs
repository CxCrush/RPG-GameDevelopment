using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {

    public float x_Sensity=1.2f;//x方向灵敏度
    public float y_Sensity=1f;//y方向灵敏度
    public float wheel_Sensity = 20; //滚轮灵敏度
    public float dragRotate_Sensity = 1;  //拖动灵敏度

    public float yMin = -60;
    public float yMax = 60;

    public float minFieldView = 10;
    public float maxFieldView = 120;

    float originalXEulerAngle;
    float orginalYEulerAngle;

    public Transform target; //人物目标

    Vector3 offset;

    Camera camera3;
    bool canRotatePerson = false;
    //bool lookAtTarget = false;
	// Use this for initialization

    void Awake()
    {
        camera3 = GetComponent<Camera>();
    }
	void Start () {

        originalXEulerAngle = transform.eulerAngles.x;
        orginalYEulerAngle = transform.eulerAngles.y;

        if (target!=null)
        {
            offset = transform.position - target.position;
        }
       
        MyEventSystem.AddListener(EventsNames.cannotRotatePlayer, CannotRotatePlayer);
        MyEventSystem.AddListener(EventsNames.canRotatePlayer, CanRotatePlayer);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        Quaternion angle = new Quaternion();

        //转动人物和相机水平和垂直方向视角
        if (Input.GetMouseButton(1))
        {

            float mouseX = Input.GetAxis("Mouse X");

            if (canRotatePerson)
            {
                target.eulerAngles += Vector3.up * mouseX * x_Sensity;
            }

            angle.eulerAngles = new UnityEngine.Vector3(0, mouseX * x_Sensity);

            float mouseY = Input.GetAxis("Mouse Y");
            originalXEulerAngle -= mouseY * y_Sensity;
            originalXEulerAngle = Mathf.Clamp(originalXEulerAngle, yMin, yMax);

        }
        
        //转动相机的位置
        //if (Input.GetMouseButton(2))
        //{
            
        //    float mouseX = Input.GetAxis("Mouse X");

        //    transform.position += new UnityEngine.Vector3( mouseX * dragRotate_Sensity,0);

        //    float mouseY = Input.GetAxis("Mouse Y");

        //    transform.position += new UnityEngine.Vector3(0,mouseX * dragRotate_Sensity);

        //    offset = transform.position - target.position;
        //}

        if (target!=null)
        {
            transform.position = target.position + angle * offset;
            offset = angle * offset;

            transform.LookAt(target);
        }
       
        transform.eulerAngles = new Vector3(originalXEulerAngle, transform.eulerAngles.y, transform.eulerAngles.z);
      
        //改变相机能看到的距离
        float wheel = Input.GetAxis("Mouse ScrollWheel");
        camera3.fieldOfView -= wheel * wheel_Sensity;
        camera3.fieldOfView = Mathf.Clamp(camera3.fieldOfView, minFieldView, maxFieldView);
    }

    void CameraLookAtTarget()
    {
        //lookAtTarget = true;
    }

    void CannotRotatePlayer()
    {
        canRotatePerson = false;
    }

    void CanRotatePlayer()
    {
        canRotatePerson = true;
    }
}
