using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace BelousovGameDev.Spawn
{
    public class SiblingsSpawner : MonoBehaviour
    {
        private readonly List<PooledComponent> _activeObjects = new ();
        
        [SerializeField] private PooledComponent _prefab;
        [SerializeField] private Transform _parent;
        [SerializeField][Min(1)] private int _poolSize = 20;

        private IObjectPool<PooledComponent> _pool;

        protected virtual void Awake()
        {
            _pool = new ObjectPool<PooledComponent>(
                createFunc: CreatePooledComponent,
                actionOnGet: GetPooledComponent,
                actionOnRelease: ReleasePooledComponent,
                actionOnDestroy: DestroyPooledComponent,
                defaultCapacity: _poolSize);
            _parent ??= transform;
        }

        public void Initialize(PooledComponent prefab, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent ?? transform;
        }

        public PooledComponent Spawn()
            => Spawn(true);

        protected PooledComponent Spawn(bool activate)
        {
            PooledComponent pooledComponent = _pool.Get();
            pooledComponent.gameObject.SetActive(activate);

            return pooledComponent;
        }

        public void Clear()
        {
            while (_activeObjects.Count > 0)
            {
                _activeObjects[^1].Release();
            }
            
            _pool.Clear();
        }

        private void GetPooledComponent(PooledComponent pooledComponent)
        {
            _activeObjects.Add(pooledComponent);
            pooledComponent.ReleaseRequested += _pool.Release;
            pooledComponent.transform.SetAsLastSibling();
        }

        private void ReleasePooledComponent(PooledComponent pooledComponent)
        {
            pooledComponent.gameObject.SetActive(false);
            pooledComponent.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            pooledComponent.ReleaseRequested -= _pool.Release;
            _activeObjects.Remove(pooledComponent);
        }

        protected virtual PooledComponent CreatePooledComponent()
        {
            PooledComponent pooledComponent = Instantiate(_prefab, _parent);

            return pooledComponent;
        }

        private void DestroyPooledComponent(PooledComponent pooledComponent) =>
            Destroy(pooledComponent.gameObject);
    }
}
