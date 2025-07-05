using System;
using UnityEngine;

namespace LunaLib {
    [Serializable]
    public struct Trigger {

        [SerializeField] private bool value;

        public Trigger(bool value) {
            this.value = value;
        }

        public bool Check {
            get {
                bool result = value;
                value = false;
                return result;
            }
        }
        public void Set() => value = true;

        public static implicit operator bool(Trigger t) => t.Check;
        public static implicit operator Trigger(bool b) => new(b);

    }
}
