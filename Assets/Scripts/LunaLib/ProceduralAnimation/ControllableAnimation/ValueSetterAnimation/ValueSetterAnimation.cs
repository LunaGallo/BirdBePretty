using UnityEngine;
using UnityEngine.Events;

namespace LunaLib {
    [ExecuteAlways]
    public abstract class ValueSetterAnimation<T, E> : ControllableAnimation where E : UnityEvent<T> {
        public E setValue;
        protected abstract T CurrentValue(float animationProgress);
        protected abstract bool CanSetValue();
        public override void UpdateProgress(float animationProgress) {
            if (CanSetValue()) {
                setValue?.Invoke(CurrentValue(animationProgress));
            }
        }
    }


}
