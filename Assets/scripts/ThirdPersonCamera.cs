using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonCamera : MonoBehaviour
{


    public Transform camTransform;
    public Transform lookAt;
    private Camera cam;
    private float distance = 2500.00f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float sensitivityX = 40.0f;
    private float sensitivityY = 40.0f;
    private Vector3 temp;
    public Vector2 startpos;
    public Vector2 direction;
    public bool directionchosen;
    public float rotationrate = 10.0f;


    // Use this for initialization
    private void Start()
    {
        cam = Camera.main;
        camTransform = transform;

    }

    // Update is called once per frame

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            //Vector2 touchPrevPos = touch.position - touch.deltaPosition;
            currentX += touch.deltaPosition.x / 2;
            currentY -= touch.deltaPosition.y / 2;

        }
        else if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            distance += deltaMagnitudeDiff * 10;

        }
    }


    private void LateUpdate()
    {

        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);

    }



}
