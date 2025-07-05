using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public abstract class RegionChildActivator : ChildActivator {
        public bool activeWhenInside = true;
        protected override bool GetActive(int index, Transform child) => activeWhenInside ^ !IsPointInsideRegion(transform.InverseTransformPoint(child.position));
        protected abstract bool IsPointInsideRegion(Vector3 point);
    }
}
