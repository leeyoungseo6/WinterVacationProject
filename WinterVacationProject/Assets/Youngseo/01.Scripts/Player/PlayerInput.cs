using UnityEngine;
using UnityEngine.Events;

namespace YS
{
    public class PlayerInput : MonoBehaviour
    {
        [Header("Movement")]
        public UnityEvent<Vector3> OnMoveInput;
        public UnityEvent<Vector3> OnRotateInput;
        public UnityEvent<bool> OnSprintInput;
        public UnityEvent OnJumpInput;

        [Header("Skill")]
        public UnityEvent<bool> OnSkillInput1;

        private void Update()
        {
            GetRotateInput();
            GetSprintInput();
            GetJumpInput();
            GetSkillInput1();
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

        private void GetJumpInput()
        {
            if (Input.GetButtonDown("Jump")) OnJumpInput?.Invoke();
        }

        private void GetSkillInput1()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl)) OnSkillInput1?.Invoke(true);
            else if (Input.GetKeyUp(KeyCode.LeftControl)) OnSkillInput1?.Invoke(false);
        }
    }
}