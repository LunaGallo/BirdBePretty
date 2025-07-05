using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class ComponentPool<T> : Pool<T> where T : Component {
        public ComponentPool(T prefab, Transform container = null, int initialSize = 0) : base(null, initialSize: initialSize) {
            if (container != null) {
                create = () => UnityEngine.Object.Instantiate(prefab, container);
            }
            else {
                create = () => UnityEngine.Object.Instantiate(prefab);
            }
            destroy = a => UnityEngine.Object.Destroy(a.gameObject);
            activate = a => a.gameObject.SetActive(true);
            deactivate = a => a.gameObject.SetActive(false);
        }
        public ComponentPool(T prefab, Transform container = null, Vector3 anchor = default, int initialSize = 0) : base(null, initialSize: initialSize) {
            if (container != null) {
                create = () => UnityEngine.Object.Instantiate(prefab, container);
            }
            else {
                create = () => UnityEngine.Object.Instantiate(prefab);
            }
            destroy = a => UnityEngine.Object.Destroy(a.gameObject);
            activate = a => a.gameObject.SetActive(true);
            deactivate = a => {
                a.gameObject.SetActive(false);
                a.transform.localPosition = anchor;
            };
        }
    }
}