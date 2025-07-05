using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    [Serializable]
    public abstract class LinearPath<T> : List<T> {

        protected abstract Func<T, T, float, T> LerpFunc { get; }
        protected abstract Func<T, T, float> DistFunc { get; }

        public int SegmentCount => Count - 1;
        public bool IsValidSegmentIndex(int i) => i >= 0 && i < SegmentCount;
        public (T p0, T p1) GetSegment(int i) => (this[i], this[i + 1]);
        public T LerpSegment((T p0, T p1) segment, float progress) => LerpFunc(segment.p0, segment.p1, progress);
        public T LerpSegment(int segmentIndex, float progress) => LerpSegment(GetSegment(segmentIndex), progress);
        public float SegmentLength((T p0, T p1) segment) => DistFunc(segment.p0, segment.p1);
        public float SegmentLength(int segmentIndex) => SegmentLength(GetSegment(segmentIndex));

        public T LerpGlobalValue(float v) => LerpSegmentProgress(GlobalValueToSegmentProgress(v));
        public T LerpGlobalProgress(float t) {
            int i = Mathf.Clamp(Mathf.FloorToInt(t), 0, SegmentCount - 1);
            return LerpSegmentProgress((i, t - i));
        }
        public T LerpSegmentProgress((int i, float t) segmentProgress) => LerpSegment(GetSegment(segmentProgress.i), segmentProgress.t);

        public (int, float) GlobalValueToSegmentProgress(float v) {
            float accumulatedValue = 0;
            for (int j = 0; j < SegmentCount; j++) {
                float curValue = SegmentLength(j);
                if (v >= accumulatedValue && v < accumulatedValue + curValue) {
                    return (j, (v - accumulatedValue) / curValue);
                }
                accumulatedValue += curValue;
            }
            return (SegmentCount - 1, 1f);
        }

    }

    [Serializable]
    public class FloatPath : LinearPath<float> {
        protected override Func<float, float, float> DistFunc => FloatUtils.Distance;
        protected override Func<float, float, float, float> LerpFunc => Mathf.Lerp;
    }
    [Serializable]
    public class Vector2Path : LinearPath<Vector2> {
        protected override Func<Vector2, Vector2, float> DistFunc => Vector2.Distance;
        protected override Func<Vector2, Vector2, float, Vector2> LerpFunc => Vector2.Lerp;
    }
    [Serializable]
    public class Vector3Path : LinearPath<Vector3> {
        protected override Func<Vector3, Vector3, float> DistFunc => Vector3.Distance;
        protected override Func<Vector3, Vector3, float, Vector3> LerpFunc => Vector3.Lerp;
    }
    [Serializable]
    public class Vector4Path : LinearPath<Vector4> {
        protected override Func<Vector4, Vector4, float> DistFunc => Vector4.Distance;
        protected override Func<Vector4, Vector4, float, Vector4> LerpFunc => Vector4.Lerp;
    }
    [Serializable]
    public class ColorPath : LinearPath<Color> {
        protected override Func<Color, Color, float> DistFunc => ColorUtils.Distance;
        protected override Func<Color, Color, float, Color> LerpFunc => Color.Lerp;
    }
}