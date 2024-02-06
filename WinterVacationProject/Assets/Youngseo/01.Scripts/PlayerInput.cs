using UnityEngine;
using UnityEngine.Events;

namespace YS
{
    public class PlayerInput : MonoBehaviour
    {
        public UnityEvent<Vector3> OnMoveInput;
        public UnityEvent<Vector3> OnRotateInput;
        public UnityEvent<bool> OnSprintInput;

        private void Update()
        {
            GetRotateInput();
            GetSprintInput();
        }

        private void FixedUpdate()
        {
            GetMoveInput();
        }

        private void GetMoveInput()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            OnMoveInput?.Invoke(new Vector3(x, 0, z).normalized);
        }

        private void GetRotateInput()
        {
            float x = -Input.GetAxis("Mouse Y");
            float y = Input.GetAxis("Mouse X");
            OnRotateInput?.Invoke(new Vector3(x, y));
        }

        private void GetSprintInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift)) OnSprintInput?.Invoke(true);
            else if (Input.GetKeyUp(KeyCode.LeftShift)) OnSprintInput?.Invoke(false);
        }
    }
}