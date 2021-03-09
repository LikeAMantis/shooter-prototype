using UnityEngine;

namespace Movement
{
    public class ProceduralCrouch : MonoBehaviour, ICrouchable
    {
        public void Crouch()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Debug.Log("proc crouch down");
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                Debug.Log("proc crouch up");
            }
            
        }
    }
}