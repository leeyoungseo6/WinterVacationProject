using UnityEngine;

namespace YS
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        [SerializeField] private PoolingListSO _poolingListSO;

        private void Awake()
        {
            Instance ??= this;
            CreatePoolManager();
        }

        private void CreatePoolManager()
        {
            PoolManager.Instance = new PoolManager(transform);
            foreach (PoolingPair pair in _poolingListSO.Pairs)
            {
                PoolManager.Instance.CreatePool(pair.Prefab, pair.Count);
            }
        }
    }
}