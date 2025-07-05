using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LunaLib {
    [ExecuteAlways] 
    public class ValueListAnimation<T, E> : ValueSetterAnimation<T, E> where E : UnityEvent<T> {
        public List<T> valueList;
        public RoundMethod roundMethod;
        protected override T CurrentValue(float animationProgress) => valueList[Mathf.Min((animationProgress * valueList.Count).RoundToInt(roundMethod), valueList.Count - 1)];
        protected override bool CanSetValue() => true;
    }


}
