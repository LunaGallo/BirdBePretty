using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class GameObjectDestroyer : MonoBehaviour {

        public GameObject target;

        public void DestroyTarget() {
            if (target != null) {
                Destroy(target);
            }
        }
        public void DestroyTarget(GameObject target) {
            this.target = target;
            DestroyTarget();
        }

    }

}