using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace LunaLib {
    public abstract class StaticListDefiner<S, V> : StaticValueDefiner<S, List<V>> 
        where S : StaticValue<List<V>> {

        public int index;
        public V element;
        public bool setElementOnStart = true;
        public bool setElementOnEnable = true;

        public override void Start() {
            base.Start();
            if (setElementOnStart) {
                SetElement();
            }
        }
        public override void OnEnable() {
            base.OnEnable();
            if (setElementOnStart) {
                SetElement();
            }
        }

        public void SetElement() => SetElement(index, element);
        [Button] public void SetElement(int index, V newElement) => staticValueReference.value[index] = newElement;

    }
}
