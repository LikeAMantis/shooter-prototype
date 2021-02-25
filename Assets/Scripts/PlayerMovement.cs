using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float movementSpeed = 10;
    public float boostMultiplier = 2;
    public float mouseSensitivity = 200;
    public bool invertYAxis = false;
    
    Transform cameraTransform;
    Animator cameraAnimator;
    static readonly int IsWalking = Animator.StringToHash("isWalking");

    void Awake()
    {
        cameraTransform = playerCamera.transform;
        cameraAnimator = playerCamera.GetComponent<Animator>();
    }

    void Update() 
    {
        LookAround();
        Movement();
    }

    void LookAround() 
    {
        var verticalDelta = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * (invertYAxis ? 1 : -1);
        var horizontalDelta = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        
        var eulerAngles = cameraTransform.eulerAngles;
        eulerAngles += new Vector3(verticalDelta, 0, 0);
        cameraTransform.eulerAngles = eulerAngles;
        
        transform.eulerAngles += new Vector3(0, horizontalDelta, 0);
    }

    void Movement() 
    {
        var sensitivity = movementSpeed * Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? boostMultiplier : 1);
        var forward = Input.GetAxis("Vertical") * sensitivity;
        var side = Input.GetAxis("Horizontal") * sensitivity;
        var up = sensitivity;
        
        if (Input.GetKey("e")) {
            up *= 1;
        } else if (Input.GetKey("q")) {
            up *= -1;
        } else {
            up *= 0;
        }

        transform.Translate(side, up, forward, Space.Self);
        
        MovementAnimation();
    }

    void MovementAnimation()
    {
        if (Input.GetKeyDown("w") || Input.GetKeyDown("a") || Input.GetKeyDown("s") || Input.GetKeyDown("d"))
        {
            cameraAnimator.SetBool(IsWalking, true);

        }
        if (Input.GetKeyUp("w") || Input.GetKeyUp("a") || Input.GetKeyUp("s") || Input.GetKeyUp("d"))
        {
            cameraAnimator.SetBool(IsWalking, false);
        }
    }
}
