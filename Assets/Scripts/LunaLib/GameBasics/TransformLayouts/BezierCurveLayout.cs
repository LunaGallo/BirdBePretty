using UnityEngine;
using Sirenix.OdinInspector;

namespace LunaLib {
    [ExecuteAlways]
    public class BezierCurveLayout :TransformLayoutRange {
        public Transform knob0;
        public Transform knob1;
        public bool setRotation = true;

        private static readonly int subdivisions = 20;

        private Vector3 Point0 => knob0.position;
        private Vector3 Point1 => knob1.position;
        private Vector3 Tang0 => knob0.forward * knob0.localScale.z;
        private Vector3 Tang1 => knob1.forward * knob1.localScale.z;

        protected override bool CanApply => knob0 != null && knob1 != null;
        protected override void Apply() {
            float curveLength = BezierCurveSolver.ApproximateCubicBezierLength(Point0, Point0 - Tang0, Point1 - Tang1, Point1, subdivisions);
            foreach((float t, Transform transform) child in ChildrenInRange) {
                child.transform.position = BezierCurveSolver.InterpolateCubicBezier(Point0, Point0 - Tang0, Point1 - Tang1, Point1, child.t);
                if(setRotation) {
                    Vector3 nextPoint = BezierCurveSolver.InterpolateCubicBezier(Point0, Point0 - Tang0, Point1 - Tang1, Point1, child.t + (curveLength / 100f));
                    Vector3 previousPoint = BezierCurveSolver.InterpolateCubicBezier(Point0, Point0 - Tang0, Point1 - Tang1, Point1, child.t - (curveLength / 100f));
                    child.transform.rotation = Quaternion.LookRotation(nextPoint - previousPoint, Vector3.Slerp(knob0.up, knob1.up, child.t));
                }
            }
        }

    }
}
