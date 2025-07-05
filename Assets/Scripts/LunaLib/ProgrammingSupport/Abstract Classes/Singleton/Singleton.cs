using UnityEngine;

namespace LunaLib {
    public abstract class Singleton<Self> : MonoBehaviour where Self : MonoBehaviour {

        private static Singleton<Self> instance;
        public static Self Instance {
            get {
                return instance as Self;
            }
        }

        protected virtual void Awake() {
            instance = this;
        }
        protected virtual void OnDestroy() {
            instance = null;
        }

    }
}
