using UnityEngine;

namespace LunaLib {
    public class DontDestroyGameObjectOnLoad : MonoBehaviour {

        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }

    }

}
