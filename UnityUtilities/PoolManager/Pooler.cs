using System.Collections.Generic;
using UnityEngine;

namespace UnityUtilities.PoolManager {

    public class ObjectPooler : MonoBehaviour, IBasePool {

        protected BasePoolable ObjectPrefab;
        private static Dictionary<string, Stack<BasePoolable>> _gameObjectPool;
        private static ObjectPooler _instance;

        public static ObjectPooler Instance {
            get {
                if(_instance == null) {
                    var gameObject = new GameObject();
                    _instance = gameObject.AddComponent<ObjectPooler>();
                    _gameObjectPool = new Dictionary<string, Stack<BasePoolable>>();
                }

                return _instance;
            }
        }

        public virtual BasePoolable GetPooledObject(string objectKey) {
            if(!_gameObjectPool.TryGetValue(objectKey, out var indicatedPool))
                Debug.LogError("Object pool not found for given key.");

            var value = indicatedPool.Count > 0 ? indicatedPool.Pop() : InstantiateObject();

            return value;
        }

        public void PopulatePool(int amount, string pathToObjectPrefab) {
            if(ObjectPrefab == null)
                ObjectPrefab = TryLoadGameObject(pathToObjectPrefab);

            var key = ObjectPrefab.GetPoolKey();

            if(!_gameObjectPool.TryGetValue(key, out var indicatedPool))
                indicatedPool = new Stack<BasePoolable>();

            for(var i = 0; i < amount; i++) {
                var el = InstantiateObject();
                indicatedPool.Push(el);
            }

            _gameObjectPool.Add(key, indicatedPool);
        }

        public void PopulatePool(int amount, BasePoolable prefab) {
            ObjectPrefab = prefab;

            if(prefab == null)
                Debug.LogError("No prefab passed");

            var key = ObjectPrefab.GetPoolKey();

            if(!_gameObjectPool.TryGetValue(key, out var indicatedPool))
                indicatedPool = new Stack<BasePoolable>();

            for(var i = 0; i < amount; i++) {
                var el = InstantiateObject();
                indicatedPool.Push(el);
            }

            _gameObjectPool.Add(key, indicatedPool);
        }

        public void ReturnObjectToPool(string objectKey, BasePoolable obj) {
            if(!_gameObjectPool.TryGetValue(objectKey, out var indicatedPool))
                Debug.LogError("Object pool not found for given key.");

            obj.OnAddToPool();
            indicatedPool.Push(obj);
        }

        protected virtual BasePoolable InstantiateObject() {
            var obj = Instantiate(ObjectPrefab);

            // Get reference of poolable component
            var component = obj.GetComponent<BasePoolable>();

            if(component == null) {
                Debug.LogWarning("No poolable component attached to the object");
            } else {
                component.OnAddToPool();
            }

            return component;
        }

        private static BasePoolable TryLoadGameObject(string path) {
            var gameObj = Resources.Load<BasePoolable>(path);

            if(gameObj == null)
                Debug.LogError("Game object can not be loaded - could be wrong prefab path");

            return gameObj;
        }
    }

}
