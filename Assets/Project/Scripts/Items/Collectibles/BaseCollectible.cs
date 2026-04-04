using UnityEngine;

namespace LS.Items.Collectibles
{
    public abstract class BaseCollectible : MonoBehaviour
    {
        private int _uniqueId = -1;
        
        public int UniqueId => _uniqueId;
        public abstract int ResourceID { get; }
        public abstract int Value { get; }
 
        protected virtual void Awake()
        {
            _uniqueId = transform.GetSiblingIndex();
        }

        public virtual void OnCollected()
        {
            gameObject.SetActive(false);
        }
    }
}
