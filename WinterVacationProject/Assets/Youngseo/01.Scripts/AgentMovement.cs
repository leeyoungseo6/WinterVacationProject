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
            _rigid.AddForce(_moveDir * 30);
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
            if (value) _maxSpeed *= 2;
            else _maxSpeed /= 2;
        }

        public void OnRotate(Vector3 rot)
        {
            rot *= _rotateSpeed;
            float rotY = _headTrm.eulerAngles.y + rot.y;
            _rotX = Mathf.Clamp(_rotX + rot.x, -_upRotateLimit, -_downRotateLimit);
            _headTrm.eulerAngles = new Vector3(_rotX, rotY);
        }
    }
}