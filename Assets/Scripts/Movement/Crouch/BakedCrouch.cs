using System;
using Movement.Crouch;
using UnityEditor.Animations;
using UnityEngine;

namespace Movement.Crouch
{
    public class BakedCrouch : MonoBehaviour, ICrouchable
    {
        public Animator playerCameraAnimator;
        public AnimationClip crouchDownClip;
        public AnimationClip crouchUpClip;
        public float animationTime = .15f;
        public float crouchPos = .3f;
        
        static readonly int CrouchUp = Animator.StringToHash("crouchUp");
        static readonly int CrouchDown = Animator.StringToHash("crouchDown");
        float characterSize;

        void Awake()
        {
            playerCameraAnimator = playerCameraAnimator ? playerCameraAnimator : Camera.main.GetComponent<Animator>();
            SetCurveCrouchDown();
            SetCurveCrouchUp();
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
        
        void SetCurveCrouchDown()
        {
            var curve = AnimationCurve.EaseInOut(0f, playerCameraAnimator.transform.localPosition.y, animationTime, crouchPos);
            crouchDownClip.ClearCurves();
            crouchDownClip.SetCurve("", typeof(Transform), "localPosition.y", curve);
        }
        
        void SetCurveCrouchUp()
        {
            var curve = AnimationCurve.EaseInOut(0f, crouchPos, animationTime, playerCameraAnimator.transform.localPosition.y);
            crouchUpClip.ClearCurves();
            crouchUpClip.SetCurve("", typeof(Transform), "localPosition.y", curve);
        }

        [ContextMenu("SetCurves")]
        void SetAnimationCurves()
        {
           SetCurveCrouchDown();
           SetCurveCrouchUp(); 
        }
    }
}