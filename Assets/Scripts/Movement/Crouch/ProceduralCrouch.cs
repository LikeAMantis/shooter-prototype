using System;
using System.Collections;
using UnityEngine;

namespace Movement.Crouch
{
    public class ProceduralCrouch : MonoBehaviour, ICrouchable
    {
        public Transform playerCamera;
        public float animationTime = 1f;
        public float crouchPos = .3f;

        float t = 0f;
        float cameraHeight;

        void Awake()
        {
            cameraHeight = playerCamera.localPosition.y;
            playerCamera.gameObject.GetComponent<Animator>().enabled = false;
        }


        public void Crouch()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                t += Time.deltaTime * (1 / animationTime);
                t = Mathf.Clamp01(t);
                var pos = Mathf.SmoothStep(cameraHeight, crouchPos, t);
                playerCamera.localPosition = new Vector3(0, pos, 0);
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                StartCoroutine(StandUp());
            }
        }

        IEnumerator StandUp()
        {
            while (true)
            {
                if (t <= 0) yield break;
                
                t -= Time.deltaTime * (1 / animationTime);
                var pos = Mathf.SmoothStep(cameraHeight, crouchPos, t);
                playerCamera.localPosition = new Vector3(0, pos, 0);
                
                yield return null;
            }
        }
    }
}