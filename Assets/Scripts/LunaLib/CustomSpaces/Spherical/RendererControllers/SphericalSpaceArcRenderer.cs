using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using LunaLib;

namespace LunaLib {
    [ExecuteAlways]
    public class SphericalSpaceArcRenderer : LineRendererController {

        public enum InputMode {
            CoordinatesToCoordinates,
            CoordinatesToTransform,
            TransformToCoordinates,
            TransformToTransform
        }
        public InputMode inputMode;
        public bool IsStartCoordinates => inputMode == InputMode.CoordinatesToCoordinates && inputMode == InputMode.CoordinatesToTransform;
        public bool IsStartTransform => inputMode == InputMode.TransformToCoordinates && inputMode == InputMode.TransformToTransform;
        public bool IsEndCoordinates => inputMode == InputMode.CoordinatesToCoordinates && inputMode == InputMode.TransformToCoordinates;
        public bool IsEndTransform => inputMode == InputMode.CoordinatesToTransform && inputMode == InputMode.TransformToTransform;
        public bool IsAllCoordinates => inputMode == InputMode.CoordinatesToCoordinates;
        [ShowIf("IsStartCoordinates")] public Vector3 startCoordinates;
        [ShowIf("IsStartTransform")] public SphericalTransform startTransform;
        [ShowIf("IsEndCoordinates")] public Vector3 endCoordinates;
        [ShowIf("IsEndTransform")] public SphericalTransform endTransform;
        [ShowIf("IsAllCoordinates")] public SphericalSpace spaceRef;
        public ArcParameter segmentMaxArc = 2f;

        public Vector3 StartPoint => IsStartCoordinates? startCoordinates : startTransform.customPosition;
        public Quaternion StartLongLatRot => SphericalSpace.CustomPointToLongLatRotation(StartPoint);
        public Vector3 EndPoint => IsEndCoordinates? endCoordinates : endTransform.customPosition;
        public Quaternion EndLongLatRot => SphericalSpace.CustomPointToLongLatRotation(EndPoint);
        public SphericalSpace Space => IsStartTransform? startTransform.Space : IsEndTransform? endTransform.Space : spaceRef;

        protected override List<Vector3> Positions => DiscretizedWorldPoints();
        protected override bool CanApply => base.CanApply && (IsStartCoordinates || startTransform != null) && (IsEndCoordinates || endTransform != null);
        public List<Vector3> DiscretizedWorldPoints() {
            List<Vector3> result = new();
            if (StartPoint != EndPoint) {
                float angle = Quaternion.Angle(StartLongLatRot, EndLongLatRot);
                int segmentCount = Mathf.CeilToInt(angle / segmentMaxArc.Angle((StartPoint.z + EndPoint.z) / 2f));
                for (int i = 0; i <= segmentCount; i++) {
                    float t = (float)i / segmentCount;
                    result.Add(Space.CustomToWorldPoint(Vector3.Lerp(StartPoint, EndPoint, t)));
                }
            }
            return result;
        }

    }
}