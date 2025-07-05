using UnityEngine;
using Sirenix.OdinInspector;

namespace LunaLib {

    [ExecuteAlways]
    public class CircleLayout : TransformLayoutRange {
        public float radius = 1f;
        public Range angleRange = new Range(0f,360f);
        public bool setRotation = true;

        protected override void Apply() {
            foreach((float t, Transform transform) child in ChildrenInRange) {
                Vector3 forward = Quaternion.AngleAxis(angleRange.Lerp(child.t), transform.up) * transform.forward;
                child.transform.position = transform.position + forward * radius;
                if (setRotation) {
                    child.transform.rotation = Quaternion.LookRotation(forward, transform.up);
                }
            }
        }

    }
}
