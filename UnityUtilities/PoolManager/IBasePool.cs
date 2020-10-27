using UnityEngine;

namespace UnityUtilities.PoolManager {
    public interface IBasePool {
        void PopulatePool(int amount, string pathToObjectPrefab);
        void PopulatePool(int amount, BasePoolable prefab);
        BasePoolable GetPooledObject(string objectKey);
        void ReturnObjectToPool(string objectKey, BasePoolable obj);
    }

}
