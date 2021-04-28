using System.Collections.Generic;
using UnityEngine;

namespace UnityUtilities.PoolManager {

    public class ObjectPooler : MonoBehaviour, IBasePool {

        private static Dictionary<string, BasePoolable> _prefabCache;
        private static Dictionary<string, Stack<BasePoolable>> _gameObjectPool;
        private static ObjectPooler _instance;

        public static ObjectPooler Instance {
            get {
                if(_instance == null) {
                    var gameObject = new GameObject();
                    _instance = gameObject.AddComponent<ObjectPooler>();
                    _gameObjectPool = new Dictionary<string, Stack<BasePoolable>>();
                    _prefabCache = new Dictionary<string, BasePoolable>();
                }

                return _instance;
            }
        }

        public virtual BasePoolable GetPooledObject(string objectKey) {
            if(!_gameObjectPool.TryGetValue(objectKey, out var indicatedPool))
                Debug.LogError("Object pool not found for given key.");

            var value = indicatedPool.Count > 0 ? indicatedPool.Pop() : InstantiateObject(objectKey);

            return value;
        }

        /// <summary>
        /// Add unity game object to pool.
        /// </summary>
        /// <param name="amount">Amount of game object clones to add to pool.</param>
        /// <param name="pathToObjectPrefab">Path in Resource folder where prefab is located eg. "FloatingEvents/FloatingText".</param>
        /// <returns>Pooled object key. It's needed to fetch pooled game object from pool.</returns>
        public string PopulatePool(int amount, string pathToObjectPrefab) {
            var prefab = TryLoadGameObject(pathToObjectPrefab);

            var key = prefab.GetPoolKey();

            if(!_prefabCache.ContainsKey(key))
                _prefabCache.Add(key, prefab);

            if(!_gameObjectPool.TryGetValue(key, out var indicatedPool))
                indicatedPool = new Stack<BasePoolable>();

            for(var i = 0; i < amount; i++) {
                var el = InstantiateObject(prefab);
                indicatedPool.Push(el);
            }

            _gameObjectPool.Add(key, indicatedPool);

            return key;
        }

        /// <summary>
        /// Add unity game object to pool.
        /// </summary>
        /// <param name="amount">Amount of game object clones to add to pool.</param>
        /// <param name="prefab">Prefab refernce which will be added to pool.</param>
        /// <returns>Pooled object key. It's needed to fetch pooled game object from pool.</returns>
        public string PopulatePool(int amount, BasePoolable prefab) {
            if(prefab == null)
                Debug.LogError("No prefab passed");

            var key = prefab.GetPoolKey();

            if(!_prefabCache.ContainsKey(key))
                _prefabCache.Add(key, prefab);

            if(!_gameObjectPool.TryGetValue(key, out var indicatedPool))
                indicatedPool = new Stack<BasePoolable>();

            for(var i = 0; i < amount; i++) {
                var el = InstantiateObject(prefab);
                indicatedPool.Push(el);
            }

            _gameObjectPool.Add(key, indicatedPool);

            return key;
        }

        public void ReturnObjectToPool(string objectKey, BasePoolable obj) {
            if(!_gameObjectPool.TryGetValue(objectKey, out var indicatedPool))
                Debug.LogError("Object pool not found for given key.");

            obj.OnAddToPool();
            indicatedPool.Push(obj);
        }

        protected virtual BasePoolable InstantiateObject(BasePoolable prefab) {
            var obj = Instantiate(prefab);

            if(obj == null)
                Debug.LogError($"Can not instantiate {prefab.name} for pool.");

            // Get reference of poolable component
            //var component = obj.GetComponent<BasePoolable>();

            //if(component == null) {
            //    Debug.LogWarning("No poolable component attached to the object");
            //} else {
            //    component.OnAddToPool();
            //}
            obj.OnAddToPool();

            return obj;
        }

        protected virtual BasePoolable InstantiateObject(string key)
        {
            BasePoolable obj;

            if(_prefabCache.TryGetValue(key, out var cachedPrefab))
            {
                obj = Instantiate(cachedPrefab);
            } else
            {
                obj = null;
                Debug.LogError("Object prefab not found in cache");
            }

            //var component = obj.GetComponent<BasePoolable>();

            //if(component == null)
            //{
            //    Debug.LogWarning("No poolable component attached to the object");
            //}
            //else
            //{
            //    component.OnAddToPool();
            //}

            obj.OnAddToPool();

            return obj;
        }

        private static BasePoolable TryLoadGameObject(string path) {
            var gameObj = Resources.Load<BasePoolable>(path);

            if(gameObj == null)
                Debug.LogError("Game object can not be loaded - could be wrong prefab path");

            return gameObj;
        }
    }

}
