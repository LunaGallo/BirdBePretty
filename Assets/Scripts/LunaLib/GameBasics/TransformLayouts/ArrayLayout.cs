using Sirenix.OdinInspector;
using UnityEngine;

namespace LunaLib {
    [ExecuteAlways]
    public class ArrayLayout : TransformLayout {
        public float pivot;
        public Vector3 step;

        protected override void Apply() {
            foreach ((int index, Transform transform) child in Children) {
                child.transform.localPosition = step * (child.index - pivot * (ChildCount-1));
            }
        }

    }
}
