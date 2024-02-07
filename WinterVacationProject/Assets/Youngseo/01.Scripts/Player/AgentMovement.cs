using System.Collections;
using UnityEngine;

namespace YS
{
    [RequireComponent(typeof(Rigidbody))]
    public class AgentMovement : MonoBehaviour
    {
        [Header("Move")] 
        [SerializeField] private float _maxSpeed = 5;
        [SerializeField] private float _accel = 25;
        [SerializeField] private float _deAccel = 25;
        private float _currentSpeed;
        private Vector3 _moveDir;
        
        [Header("Rotate")] 
        [SerializeField] private float _rotateSpeed = 15;
        [SerializeField] private float _upRotateLimit = 70;
        [SerializeField] private float _downRotateLimit = -70;
        private Transform _headTrm;
        private float _rotX;

        [Header("Jump")]
        [SerializeField] private float _jumpPower = 10;
        [SerializeField] private LayerMask _groundLayer;
        private bool _isJump;
        
        private Rigidbody _rigid;

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody>();
            _headTrm = transform.Find("Head");
            Cursor.lockState = CursorLockMode.Locked;
        }

        private readonly float _c1 = Mathf.Sqrt(3) / 2;

        public void OnMove(Vector3 dir)
        {
            if (_isJump) return;
            
            dir = Quaternion.Euler(0, _headTrm.eulerAngles.y, 0) * dir;
            
            if (dir.sqrMagnitude > 0)
            {
                if (Vector3.Dot(_moveDir, dir) < 0)
                {
                    _currentSpeed /= 2;
                }
                else if (Vector3.Dot(_moveDir, dir) < _c1)
                {
                    _currentSpeed /= 1.5f;
                }
                _moveDir = dir;
            }
            
            _currentSpeed = CalculateSpeed(dir);
            if (_currentSpeed <= 0) return;
            if (Physics.Raycast(transform.position, Vector3.down, out var hit, 1.1f, _groundLayer))
            {
                if (dir.sqrMagnitude > 0 && Vector3.Angle(Vector3.up, hit.normal) is > 20 or < 90)
                {
                    _moveDir = Vector3.ProjectOnPlane(_moveDir, hit.normal) * 2;
                }
            }
            _rigid.AddForce(_moveDir * 30, ForceMode.Force);
            _rigid.velocity = Vector3.ClampMagnitude(_rigid.velocity, _currentSpeed);
        }
        
        private float CalculateSpeed(Vector3 dir)
        {
            if (dir.sqrMagnitude > 0)
            {
                _currentSpeed += _accel * Time.deltaTime;
            }
            else
            {
                _currentSpeed -= _deAccel * Time.deltaTime;
            }

            return Mathf.Clamp(_currentSpeed, 0, _maxSpeed);
        }

        public void OnSprint(bool value)
        {
            _maxSpeed = value ? 5 : 2.5f;
        }

        public void OnRotate(Vector3 rot)
        {
            rot *= _rotateSpeed;
            float rotY = _headTrm.eulerAngles.y + rot.y;
            _rotX = Mathf.Clamp(_rotX + rot.x, -_upRotateLimit, -_downRotateLimit);
            _headTrm.eulerAngles = new Vector3(_rotX, rotY);
        }

        public void OnJump()
        {
            if (_isJump == false && RaycastDown(0.15f, _groundLayer))
            {
                StartCoroutine(Jump());
            }
        }

        private IEnumerator Jump()
        {
            _isJump = true;
            _rigid.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => RaycastDown(0.15f, _groundLayer));
            _isJump = false;
        }

        private bool RaycastDown(float maxDistance, int layer)
        {
            return Physics.Raycast(transform.position + new Vector3(0, 0.01f, 0), Vector3.down, maxDistance, layer);
        }
    }
}