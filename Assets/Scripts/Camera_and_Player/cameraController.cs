using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] float sensitivity;
    [SerializeField] float verticalMin;
    [SerializeField] float verticalMax;

    private float verticalRotation;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        //get input
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        //not adding invert
        verticalRotation -= mouseY;

        //clamp x rotation of cam
        verticalRotation = Mathf.Clamp(verticalRotation, verticalMin, verticalMax);
        //rotate cam on x axis
        transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        //rotate player on y-axis
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
