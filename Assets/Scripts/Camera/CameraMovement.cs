using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform t;
    private Vector3 forward, back, left, right, zoomIn, zoomOut;
    private float speed;
    private int planeAngle; // in degrees

    void Start()
    {
        planeAngle = 20;
        speed = 0.25f;

        t = GetComponent<Transform>();
        forward = Quaternion.AngleAxis(planeAngle, new Vector3(-1,0,0)) * Vector3.forward * speed;
        back = Quaternion.AngleAxis(planeAngle, new Vector3(-1, 0, 0)) * Vector3.back * speed;
        left = Vector3.left * speed;
        right = Vector3.right * speed;

        zoomIn = Quaternion.AngleAxis(planeAngle, new Vector3(-1, 0, 0)) * Vector3.up * speed;
        zoomOut = Quaternion.AngleAxis(planeAngle, new Vector3(-1, 0, 0)) * Vector3.down * speed;
    }

    void Update()
    {
        //Basic movements ( WASD + zoomIn, ZoomOut)
        if (Input.GetKey(KeyCode.W))
            transform.position += forward;
        if (Input.GetKey(KeyCode.A))
            transform.position += left;
        if (Input.GetKey(KeyCode.S))
            transform.position += back;
        if (Input.GetKey(KeyCode.D))
            transform.position += right;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            transform.position += zoomIn;
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            transform.position += zoomOut;

    }
}
