using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LunaLib {
    [ExecuteAlways] 
    public class ValueKeyframeAnimation<K, T, E> : ValueSetterAnimation<T, E> where K : ValueKeyframe<T> where E : UnityEvent<T> {
        public List<K> keyframeList;
        protected override T CurrentValue(float animationProgress) {
            float timer = ProgressToTimer(animationProgress);
            return keyframeList.FindLastConsecutive(t => t.timestamp <= timer).value;
        }
        protected override bool CanSetValue() => true;
        private void OnValidate() {
            keyframeList.SortAsFloat(t => t.timestamp);
        }
    }

    [Serializable]
    public class ValueKeyframe<T> {
        public float timestamp;
        public T value;
    }
}
