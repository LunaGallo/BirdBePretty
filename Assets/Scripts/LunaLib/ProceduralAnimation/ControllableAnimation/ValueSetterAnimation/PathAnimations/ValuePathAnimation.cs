using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LunaLib {
    [ExecuteAlways] 
    public abstract class ValuePathAnimation<T, E> : ValueSetterAnimation<T, E> where E : UnityEvent<T> {
        public List<T> valuePath;
        public AnimationCurve animationCurve;
        public int curIndex = 0;

        protected abstract Func<T, T, float, T> LerpFunc { get; }

        public void RotateIndex() => curIndex = NextIndex;
        public int NextIndex => (curIndex + 1) % valuePath.Count;
        protected override T CurrentValue(float animationProgress) => LerpFunc(valuePath[curIndex], valuePath[NextIndex], animationCurve.Evaluate(animationProgress));
        protected override bool CanSetValue() => valuePath.IsValidIndex(curIndex) && valuePath.IsValidIndex(NextIndex);
    }


}
