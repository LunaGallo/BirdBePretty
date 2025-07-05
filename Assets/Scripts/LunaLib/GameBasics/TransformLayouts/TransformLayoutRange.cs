using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    [ExecuteAlways]
    public abstract class TransformLayoutRange : TransformLayout {
        [SerializeField] private bool useEnds = true;

        protected IEnumerable<(float t, Transform transform)> ChildrenInRange {
            get {
                foreach((int index, Transform transform) child in Children) {
                    yield return (useEnds ? ((float) child.index / (ChildCount - 1)) : ((child.index + 0.5f) / ChildCount), child.transform);
                }
            }
        }

    }
}
