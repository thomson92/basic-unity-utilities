using UnityEngine;

namespace UnityUtilities.PoolManager {
    public abstract class BasePoolable : MonoBehaviour {
        private IBasePool PoolRef { get; set; }

        public void SetPoolRef(IBasePool poolRef) {
            PoolRef = poolRef;
        }

        protected virtual void ReturnToPool() {
            PoolRef.ReturnObjectToPool(this);
        }
    }
}
