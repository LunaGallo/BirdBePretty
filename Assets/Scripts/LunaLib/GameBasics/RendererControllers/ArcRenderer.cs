using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LunaLib {
    public class ArcRenderer : LineRendererController {

        public float radius = 1f;
        public int segmentCount = 6;
        public float angleStart = 0f;
        public float angleFinish = 60f;

        protected override List<Vector3> Positions => Discretize();

        public List<Vector3> Discretize() {
            List<Vector3> result = new();
            for (int i = 0; i < segmentCount + 1; i++) {
                float curPoint = Mathf.Lerp(angleStart, angleFinish, (float)i / segmentCount);
                result.Add(Lerp(curPoint));
            }
            return result;
        }
        public Vector3 Lerp(float value) => Quaternion.AngleAxis(value, Vector3.forward) * Vector3.right * radius;

    }
}