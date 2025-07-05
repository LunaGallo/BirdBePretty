using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LunaLib {
    public abstract class StaticValueDefiner<S, V> : MonoBehaviour where S : StaticValue<V> {

        public S staticValueReference;
        public V value;
        public bool setValueOnStart = true;
        public bool setValueOnEnable = true;

        public virtual void Start() {
            if (setValueOnStart) {
                SetValue();
            }
        }
        public virtual void OnEnable() {
            if (setValueOnEnable) {
                SetValue();
            }
        }

        public void SetValue() => SetValue(value);
        [Button] public void SetValue(V newValue) => staticValueReference.value = newValue;

    }
}
