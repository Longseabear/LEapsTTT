using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TTT.Rhythms;
using UnityEngine;
using UnityEngine.Pool;

namespace TTT.System
{
    public class PrefabSnapshot
    {
        private Dictionary<Type, List<object>> _serializedDatas;

        public PrefabSnapshot(Dictionary<Type, List<PrefabPoolable>> _activeInstance)
        {
            _serializedDatas = new Dictionary<Type, List<object>>();

            foreach (var type in _activeInstance.Keys)
            {
                var items = _activeInstance[type];
                _serializedDatas[type] = new List<object>();
                foreach (var item in items)
                {
                    _serializedDatas[type].Add(item.Save());
                }
            }
        }
        public void Restore(bool AllInstanceClear = false)
        {
            if (AllInstanceClear)
            {
                UltimatePrefabManager.Instance.ReleaseAll();
            }

            foreach (var kv in _serializedDatas)
            {
                foreach(var parameter in kv.Value)
                {
                    var instance = UltimatePrefabManager.Instance.Instantiate(kv.Key);
                    instance.Restore(parameter);
                }
            }
        }
    }

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

                for (int i = 0; i < prefab.PreCreatedSize; i++)
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
        public PrefabPoolable Instantiate(Type type)
        {
            return _pool.GetValueOrDefault(type)?.Get();
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
            _pool.GetValueOrDefault(typeof(T))?.Release(instance);
        }

        public void ReleaseAll()
        {
            foreach(var kv in _activeInstance)
            {
                foreach(var item in kv.Value)
                {
                    _pool[kv.Key].Release(item);
                }
            }
        }

        public List<T> GetActiveInstances<T>() where T : PrefabPoolable
        { 
            return _activeInstance.GetValueOrDefault(typeof(T)).Cast<T>().ToList();
        }
    }
}
