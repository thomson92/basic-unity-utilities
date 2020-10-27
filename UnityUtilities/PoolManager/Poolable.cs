using UnityEngine;

namespace UnityUtilities.PoolManager {

    public interface IPoolable {
        public void OnAddToPool();
        public string GetPoolKey();
    }

    public abstract class BasePoolable : MonoBehaviour, IPoolable {

        [SerializeField] protected string PoolableKey;

        public string GetPoolKey() {
            return PoolableKey;
        }

        public virtual void OnAddToPool() {
            gameObject.SetActive(false);
        }

        protected virtual void ReturnToPool() {
            ObjectPooler.Instance.ReturnObjectToPool(PoolableKey, this);
        }
    }
}
