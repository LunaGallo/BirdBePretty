using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    [ExecuteAlways, RequireComponent(typeof(SphericalTransform))]
    public class SphericalSpaceCircleLayout : TransformLayoutRange {

        private SphericalTransform sphericalTransform;
        public SphericalTransform SphericalTransform {
            get {
                if (sphericalTransform == null) {
                    sphericalTransform = GetComponent<SphericalTransform>();
                }
                return sphericalTransform;
            }
        }

        public ArcParameter circleRadiusArc = 1f;
        public Range circleAngleRange = new(0f,360f);
        public bool setChildrenRotation = true;

        protected override void Apply() {
            float angleRadius = circleRadiusArc.Angle(SphericalTransform.Radius);
            foreach((float t, Transform transform) child in ChildrenInRange) {
                Quaternion childRotation = Quaternion.AngleAxis(angleRadius, Quaternion.AngleAxis(child.t * 360f, SphericalTransform.Outward) * SphericalTransform.North);
                child.transform.position = SphericalTransform.Space.Origin + childRotation * SphericalTransform.LongLatRotation * Vector3.forward * SphericalTransform.Radius;
            }
        }
    }

}