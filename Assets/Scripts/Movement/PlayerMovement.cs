using System;
using System.Linq;
using Movement.Crouch;
using UnityEngine;
using UnityEngine.Serialization;
using Weapon;

namespace Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        public WeaponSelection weaponSelection;
    
        [Header("Movement")]
        public Camera playerCamera;
        public float movementSpeed = 10;
        public float boostMultiplier = 2;
        public float mouseSensitivity = 200;
        public bool invertYAxis = false;
    
        [Header("Jump")]
        public float jumpVelocity;
        public float gravityScale = 1f;
        
        
        static readonly string[] MovementKeys = {"w", "a", "s", "d"};
        static readonly int IsWalking = Animator.StringToHash("isWalking");

        CharacterController characterController;
        Transform cameraTransform;
        ICrouchable crouchable;
        Rigidbody rb;
        Gravity gravity;
        bool isGrounded = false;
        bool lookAroundActive = true;
        
        public float XRotation { get; set; }


        void Awake()
        {
            cameraTransform = playerCamera.transform;
            rb = GetComponent<Rigidbody>();
            gravity = new Gravity(rb);
            weaponSelection = weaponSelection ? weaponSelection : GetComponentInChildren<WeaponSelection>();
            crouchable = GetComponent<ICrouchable>();
            characterController = GetComponent<CharacterController>();
        }

        void FixedUpdate()
        {
            gravity.UseGravity(gravityScale);
        }
    
        void Update() 
        {
            LookAround();
            Movement();
            crouchable.Crouch();
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                lookAroundActive = !lookAroundActive;
            }
        }

        void OnCollisionEnter(Collision other)
        {
            isGrounded = true;
        }

        void OnCollisionExit(Collision other)
        {
            isGrounded = false;
        }
    
        void LookAround() 
        {
            if (!lookAroundActive) return;
        
            var verticalDelta = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * (invertYAxis ? 1 : -1);
            var horizontalDelta = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

            XRotation += verticalDelta;
            XRotation = Mathf.Clamp(XRotation, -90f, 90f);
            
            cameraTransform.localRotation = Quaternion.Euler(XRotation, 0, 0f);
            transform.eulerAngles += new Vector3(0, horizontalDelta, 0);
        }

        void Movement() 
        {
            var sensitivity = movementSpeed * Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? boostMultiplier : 1);
            var forward = Input.GetAxis("Vertical") * sensitivity;
            var side = Input.GetAxis("Horizontal") * sensitivity;
            var up = sensitivity;
        
            transform.Translate(side, 0, forward, Space.Self);
        
            MovementAnimation();
        }

        void MovementAnimation()
        {
            var currentSelectionAnimator = weaponSelection.CurrentSelectionAnimator;
            if (MovementKeys.Any(Input.GetKeyDown))
            {
                currentSelectionAnimator.SetBool(IsWalking, true);
            }

            if (MovementKeys.Any(Input.GetKeyUp))
            {
                currentSelectionAnimator.SetBool(IsWalking, false);
            }
        }

        void Jump()
        {
            if (isGrounded)
            {
                rb.velocity = Vector3.up * jumpVelocity;
            }
        }
    }
}
