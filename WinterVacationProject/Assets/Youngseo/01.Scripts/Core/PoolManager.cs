using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class PoolManager
    {
        public static PoolManager Instance;

        private Dictionary<string, Pool<PoolableMono>> _pools = new();
        private Transform _trmParent;

        public PoolManager(Transform trmParent)
        {
            _trmParent = trmParent;
        }

        public void CreatePool(PoolableMono prefab, int count = 10)
        {
            Pool<PoolableMono> pool = new Pool<PoolableMono>(prefab, _trmParent, count);
            _pools.Add(prefab.gameObject.name, pool);
        }

        public PoolableMono Pop(string name)
        {
            if (!_pools.ContainsKey(name))
            {
                Debug.LogError($"Prefab does not exist on pool : {name}");
                return null;
            }

            PoolableMono item = _pools[name].Pop();
            item.Init();
            return item;
        }

        public void Push(PoolableMono obj)
        {
            _pools[obj.name].Push(obj);
        }
    }
}