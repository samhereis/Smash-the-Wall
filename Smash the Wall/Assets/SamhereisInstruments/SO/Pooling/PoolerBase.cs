using Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Pooling
{
    public abstract class PoolerBase<T> : ScriptableObject where T : Component
    {
        [field: SerializeField] public T poolable { get; private set; }
        [SerializeField] protected Queue<T> _poolablesQueue = new Queue<T>();
        [SerializeField] protected List<T> _poolablesDequeued = new List<T>();

#if UNITY_EDITOR
        [SerializeField] protected List<T> _poolablesQueued = new List<T>();
#endif

        [Header("Settings")]
        [SerializeField] protected bool _setParent = true;
        [SerializeField] private int _defaultSpawnQuantity = 2;

        protected virtual void Init()
        {
            Clear();
        }

        public virtual async Task SpawnAsync(int quantity = 5, Transform parent = null)
        {
            for (int i = 0; i < quantity; i++)
            {
                await AsyncHelper.Delay();
                PutIn(Instantiate(poolable, parent));
            }
        }

        public virtual void Spawn(int quantity = 5, Transform parent = null)
        {
            for (int i = 0; i < quantity; i++)
            {
                PutIn(Instantiate(poolable, parent));
            }
        }

        [ContextMenu(nameof(Clear))]
        public virtual void Clear()
        {
            _poolablesQueue?.Clear();
            _poolablesQueue = new Queue<T>();

            _poolablesDequeued?.Clear();
            _poolablesDequeued = new List<T>();

#if UNITY_EDITOR
            _poolablesQueued?.Clear();
            _poolablesQueued = new List<T>();
#endif
        }

        public virtual Task<T> PutOffAsync(Transform position, Quaternion rotation, Transform parent = null)
        {
            return PutOffAsync(position.position, rotation, parent);
        }

        public virtual Task<T> PutOffAsync(Transform position, Transform parent = null)
        {
            return PutOffAsync(position.position, Quaternion.identity, parent);
        }

        public virtual Task<T> PutOffAsync(Vector3 position, Transform parent = null)
        {
            return PutOffAsync(position, Quaternion.identity, parent);
        }

        public virtual async Task<T> PutOffAsync(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            T poolable;

            if (_poolablesQueue.Count < 1) await SpawnAsync(_defaultSpawnQuantity, parent);

            poolable = _poolablesQueue.Dequeue();

            _poolablesDequeued.SafeAdd(poolable);

#if UNITY_EDITOR
            _poolablesQueued.SafeRemove(poolable);
#endif
            if (parent != null) poolable.transform.parent = parent;

            poolable.transform.position = position;
            poolable.transform.rotation = rotation;

            poolable.gameObject.SetActive(true);

            return poolable;
        }

        public virtual void PutIn(T poolable)
        {
            if (poolable)
            {
                try
                {
                    _poolablesQueue?.Enqueue(poolable);
                    _poolablesDequeued?.SafeRemove(poolable);

#if UNITY_EDITOR
                    _poolablesQueued?.SafeAdd(poolable);
#endif

                    poolable.gameObject.SetActive(false);

                    if (_setParent) poolable.transform.parent = null;
                }
                finally
                {

                }
            }
        }

        public virtual async void PutInAll()
        {
            var copy = new List<T>();
            copy.AddRange(_poolablesDequeued);

            foreach (var poolable in copy)
            {
                PutIn(poolable);
                await AsyncHelper.Delay();
            }

            Clear();
        }
    }
}