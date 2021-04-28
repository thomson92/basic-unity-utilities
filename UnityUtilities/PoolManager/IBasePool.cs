using UnityEngine;

namespace UnityUtilities.PoolManager {
    public interface IBasePool {
        string PopulatePool(int amount, string pathToObjectPrefab);
        string PopulatePool(int amount, BasePoolable prefab);
        BasePoolable GetPooledObject(string objectKey);
        void ReturnObjectToPool(string objectKey, BasePoolable obj);
    }

}
