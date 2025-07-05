using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LunaLib {
    public abstract class StaticValueUser<S, E, V> where S : StaticValue<V> where E : UnityEvent<V> {

        public S staticValueReference;
        public E onSetValue;

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

        public V Value => staticValueReference.value;

        public virtual void SetValue() => onSetValue?.Invoke(Value);

    }

}