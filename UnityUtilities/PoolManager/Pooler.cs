using System.Collections.Generic;
using UnityEngine;

namespace UnityUtilities.PoolManager {

    public interface IBasePool {
        void PopulatePool(int amount, string pathToObjectPrefab);
        void PopulatePool(int amount, GameObject prefab);
        Component GetPooledObject();
        void ReturnObjectToPool(BasePoolable obj);
    }

    public class ObjectPooler<T> : MonoBehaviour, IBasePool where T : BasePoolable {

        protected GameObject ObjectPrefab;
        private readonly Stack<T> _gameObjectPool = new Stack<T>();

        public virtual Component GetPooledObject() {
            var value = _gameObjectPool.Count > 0 ? _gameObjectPool.Pop() : InstantiateObject();

            return value;
        }

        public void PopulatePool(int amount, string pathToObjectPrefab) {

            if(ObjectPrefab == null)
                ObjectPrefab = TryLoadGameObject(out ObjectPrefab, pathToObjectPrefab);

            for(var i = 0; i < amount; i++) {
                var el = InstantiateObject();
                _gameObjectPool.Push(el);
            }
        }

        public void PopulatePool(int amount, GameObject prefab) {
            if(prefab == null)
                Debug.LogError("No prefab passed");

            if(ObjectPrefab == null)
                ObjectPrefab = prefab;

            for(var i = 0; i < amount; i++) {
                var el = InstantiateObject();
                _gameObjectPool.Push(el);
            }
        }

        public void ReturnObjectToPool(BasePoolable obj) {
            obj.gameObject.SetActive(false);
            _gameObjectPool.Push((T)obj);
        }

        protected virtual T InstantiateObject() {
            var obj = Instantiate(ObjectPrefab);
            obj.SetActive(false);

            // Get reference of poolable component
            var component = obj.GetComponent<T>();

            if(component == null) {
                Debug.LogWarning("No poolable component attached to the object");
            } else {
                component.SetPoolRef(this);
            }

            return component;
        }

        private static GameObject TryLoadGameObject(out GameObject gameObj, string path) {
            gameObj = Resources.Load<GameObject>(path);

            if(gameObj == null)
                Debug.LogError("Game object can not be loaded - could be wrong prefab path");

            return gameObj;
        }
    }

}
