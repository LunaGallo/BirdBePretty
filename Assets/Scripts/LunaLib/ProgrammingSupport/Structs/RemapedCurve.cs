using System;
using UnityEngine;

namespace LunaLib {

    [Serializable]
    public struct RemapedCurve {
        public Rect remap;
        public AnimationCurve curve;

        public float Extension => remap.size.y;
        public float Duration => remap.size.x;
        public float Evaluate(float f) => remap.YRange().Lerp(curve.Evaluate(remap.XRange().InverseLerp(f)));

        public RemapedCurve(Rect remap, AnimationCurve curve) {
            this.remap = remap;
            this.curve = curve;
        }
    }

}