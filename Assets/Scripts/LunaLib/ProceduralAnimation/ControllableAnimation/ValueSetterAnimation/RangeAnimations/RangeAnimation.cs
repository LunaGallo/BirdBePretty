using UnityEngine;
using UnityEngine.Events;

namespace LunaLib {
    [ExecuteAlways]
    public abstract class RangeAnimation<T, E> : ValueSetterAnimation<T, E> where E : UnityEvent<T> {
        protected override T CurrentValue(float animationProgress) => ValueFromRange(CurrentRange(animationProgress));
        protected abstract RangeInt CurrentRange(float animationProgress);
        protected abstract T ValueFromRange(RangeInt range);
    }


}
