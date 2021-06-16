using System;
using System.Linq;
using Movement.Crouch;
using UnityEngine;
using UnityEngine.PlayerLoop;
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
        public float jumpHeight = .7f;
        public float gravityScale = 1f;
        public LayerMask groundMask;
        
        static readonly string[] MovementKeys = {"w", "a", "s", "d"};
        static readonly int IsWalking = Animator.StringToHash("isWalking");

        CharacterController characterController;
        Transform cameraTransform;
        ICrouchable crouchable;
        bool isGrounded = false;
        bool lookAroundActive = true;
        Vector3 velocity;
        float gravity = -9.81f;
        float radiusSphereCheck = .2f;

        public float XRotation { get; set; }


        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, radiusSphereCheck);
        }

        void Awake()
        {
            cameraTransform = playerCamera.transform;
            weaponSelection = weaponSelection ? weaponSelection : GetComponentInChildren<WeaponSelection>();
            crouchable = GetComponent<ICrouchable>();
            characterController = GetComponent<CharacterController>();
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

        void FixedUpdate()
        {
            Gravity();
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
            var z = transform.forward * Input.GetAxis("Vertical");
            var x = transform.right * Input.GetAxis("Horizontal");
            var movementVector = (z + x) * sensitivity;
            
            characterController.Move(movementVector);
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

        void Gravity()
        {
            velocity.y += gravity * gravityScale * Time.deltaTime;
            isGrounded = Physics.CheckSphere(transform.position, radiusSphereCheck, groundMask);
            characterController.Move(velocity * Time.deltaTime);
            
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = 0f;
            }
        }

        void Jump()
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity * gravityScale);
            }
        }
    }
}