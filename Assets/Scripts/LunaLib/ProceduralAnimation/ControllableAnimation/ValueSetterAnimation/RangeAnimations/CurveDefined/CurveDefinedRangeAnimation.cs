using UnityEngine;
using UnityEngine.Events;

namespace LunaLib {
    [ExecuteAlways]
    public abstract class CurveDefinedRangeAnimation<T, E> : RangeAnimation<T, E> where E : UnityEvent<T> {
        public AnimationCurve beginAnimationCurve;
        public AnimationCurve endAnimationCurve;
        protected override RangeInt CurrentRange(float animationProgress) {
            int beginIndex = Mathf.FloorToInt(beginAnimationCurve.Evaluate(animationProgress) * RangeLength());
            int endIndex = Mathf.FloorToInt(endAnimationCurve.Evaluate(animationProgress) * RangeLength());
            return new RangeInt(beginIndex, endIndex - beginIndex);
        }
        protected abstract int RangeLength();
    }


}
