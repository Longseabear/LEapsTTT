using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace TTT.System
{
    public class UltimatePrefabManager : MonoBehaviour
    {
        [Header("Instance")]
        public static UltimatePrefabManager Instance;

        private Dictionary<Type, ObjectPool<PrefabPoolable>> _pool;
        private Dictionary<Type, List<PrefabPoolable>> _activeInstance { get; set; }

        [Serializable]
        public class PrefabInfo
        {
            public PrefabPoolable gameObject;
            public int PreCreatedSize = 10;
            public int MaximumInstanceSize = 1000;
        }
        public List<PrefabInfo> PrefabInfos;

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }   

        [Button("Initialize")]
        public void Initialize()
        {
            _pool = new Dictionary<Type, ObjectPool<PrefabPoolable>>();
            _activeInstance = new Dictionary<Type, List<PrefabPoolable>>();

            foreach (var prefab in PrefabInfos)
            {
                var group = new GameObject(prefab.gameObject.name);
                group.name = prefab.gameObject.name; 
                group.transform.parent = transform; 

                _pool[prefab.gameObject.GetType()] = new ObjectPool<PrefabPoolable>(
                        OnCreateInstance(prefab, group.transform),
                        OnInitialized,
                        OnReleased,
                        OnDestroyed, false, prefab.PreCreatedSize, prefab.MaximumInstanceSize
                    );
                _activeInstance[prefab.gameObject.GetType()] = new List<PrefabPoolable>();

                for (int i = 0; i < 3; i++)
                {
                    _pool[prefab.gameObject.GetType()].Release(OnCreateInstance(prefab, group.transform)());
                } 
            }
        }

        private Func<PrefabPoolable> OnCreateInstance(PrefabInfo prefabInfo, Transform parent)
        {
            return () => {
                var obj = Instantiate<PrefabPoolable>(prefabInfo.gameObject, parent);
                obj.OnCreated();
                return obj;
            };
        }
        private void OnInitialized(PrefabPoolable instance)
        {
            instance.gameObject.SetActive(true);
            instance.OnInitialized();
            _activeInstance[instance.GetType()].Add(instance);
        }
        private void OnReleased(PrefabPoolable instance)
        {
            instance.OnReleased();
            instance.gameObject.SetActive(false);
            _activeInstance[instance.GetType()].Remove(instance);
        }
        private void OnDestroyed(PrefabPoolable instance)
        {
            instance.OnDestroyed();
            Destroy(instance);
        }

        public T Instantiate<T>() where T : PrefabPoolable
        {
            return _pool.GetValueOrDefault(typeof(T))?.Get() as T ?? null;
        }
        public T Instantiate<T>(Vector3 position, Quaternion rotation) where T : PrefabPoolable
        {
            var instance = Instantiate<T>();
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            return instance;
        }
        public void Release<T>(T instance) where T : PrefabPoolable
        {
            Debug.Log(typeof(T));
            _pool.GetValueOrDefault(typeof(T))?.Release(instance);
        }


        public List<T> GetActiveInstances<T>() where T : PrefabPoolable
        {
            Debug.Log(typeof(T));
            return _activeInstance.GetValueOrDefault(typeof(T)).Cast<T>().ToList();
        }
    }
}
