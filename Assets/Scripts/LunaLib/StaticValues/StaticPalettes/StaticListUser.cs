using System.Collections.Generic;
using UnityEngine.Events;

namespace LunaLib {
    public abstract class StaticListUser<S, EL, EV, V> : StaticValueUser<S, EL, List<V>> 
        where S : StaticValue<List<V>> 
        where EL : UnityEvent<List<V>>
        where EV : UnityEvent<V> {

        public int index;
        public EV onSetElement;

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
            if (setElementOnEnable) {
                SetElement();
            }
        }

        public V Element => GetElement(index);
        public V GetElement(int index) => Value[index];

        public virtual void SetElement() => onSetElement?.Invoke(Element);

    }
}