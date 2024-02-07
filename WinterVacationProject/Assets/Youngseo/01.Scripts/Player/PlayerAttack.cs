using UnityEngine;

namespace YS
{
    public class PlayerAttack : MonoBehaviour
    {
        [Header("Skill1")]
        [SerializeField] private GameObject _scanner;
        private float _lastPlayedTime1 = -9999f;
        private float _coolDown1 = 3f;
        
        public void Skill1(bool value)
        {
            if (Time.time - _lastPlayedTime1 < _coolDown1) return;
            if (value)
            {
                _lastPlayedTime1 = Time.time;
                TerrainScanner scanner = PoolManager.Instance.Pop("TerrainScanner") as TerrainScanner;
                scanner.transform.position = transform.position;
            }
            else
            {
                
            }
        }
    }
}