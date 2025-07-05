using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class GameObjectPool : Pool<GameObject> {
        public GameObjectPool(GameObject prefab, Transform container = null, int initialSize = 0) : base(null, initialSize: initialSize) {
            if (container != null) {
                create = () => UnityEngine.Object.Instantiate(prefab, container);
            }
            else {
                create = () => UnityEngine.Object.Instantiate(prefab);
            }
            destroy = a => UnityEngine.Object.Destroy(a);
            activate = a => a.SetActive(true);
            deactivate = a => a.SetActive(false);
        }
        public GameObjectPool(GameObject prefab, Transform container = null, Vector3 anchor = default, int initialSize = 0) : base(null, initialSize: initialSize) {
            if (container != null) {
                create = () => UnityEngine.Object.Instantiate(prefab, container);
            }
            else {
                create = () => UnityEngine.Object.Instantiate(prefab);
            }
            destroy = a => UnityEngine.Object.Destroy(a);
            activate = a => a.SetActive(true);
            deactivate = a => {
                a.SetActive(false);
                a.transform.localPosition = anchor;
            };
        }
    }
}