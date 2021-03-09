using UnityEngine;

namespace Movement
{
    public class BakedCrouch : MonoBehaviour, ICrouchable
    {
        [SerializeField] Animator playerCameraAnimator; 
        
        static readonly int CrouchUp = Animator.StringToHash("crouchUp");
        static readonly int CrouchDown = Animator.StringToHash("crouchDown");

        void Awake()
        {
            playerCameraAnimator = playerCameraAnimator ? playerCameraAnimator : Camera.main.GetComponent<Animator>();
        }

        public void Crouch()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                playerCameraAnimator.SetTrigger(CrouchDown);
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                playerCameraAnimator.SetTrigger(CrouchUp);
            }
        }
    }
}