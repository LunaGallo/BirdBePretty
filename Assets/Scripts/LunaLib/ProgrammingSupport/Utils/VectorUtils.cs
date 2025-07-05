using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {

    public static partial class Vector2Utils {

        public static float SqrDistance(Vector2 v0, Vector2 v1) => Vector2.SqrMagnitude(v1 - v0);
        public static float ManhatanDistance(Vector2 v0, Vector2 v1) => OperatedByParts(v0, v1, (v0, v1) => v1 - v0).ManhatanMagnitude();

        public static Vector2 ClampInside(Vector2 value, Vector2 min, Vector2 max) => Vector2.Max(Vector2.Min(value, max), min);
        public static Vector2 ClampInside(Vector2 value, Rect limits) => ClampInside(value, limits.min, limits.max);

        public static Vector2 ClampOutside(Vector2 value, Vector2 min, Vector2 max) {
            Vector2 result = value;
            Vector2 minDelta = value - min;
            Vector2 maxDelta = value - max;
            float selectedDelta = Mathf.Min(minDelta.x, minDelta.y, maxDelta.x, maxDelta.y);
            if (selectedDelta == minDelta.x) {
                result.x = minDelta.x;
            }
            else if (selectedDelta == maxDelta.x) {
                result.x = maxDelta.x;
            }
            else if (selectedDelta == minDelta.y) {
                result.y = minDelta.y;
            }
            else {
                result.y = maxDelta.y;
            }
            return result;
        }
        public static Vector2 ClampOutside(Vector2 value, Rect limits) => ClampOutside(value, limits.min, limits.max);

        public static Vector2 Iterpolate(Vector2 min, Vector2 max, Vector2 t) => new(Mathf.Lerp(min.x, max.x, t.x), Mathf.Lerp(min.y, max.y, t.y));
        public static Vector2 InverseIterpolate(Vector2 min, Vector2 max, Vector2 value) => new(Mathf.InverseLerp(min.x, max.x, value.x), Mathf.InverseLerp(min.y, max.y, value.y));
        public static Vector2 Remap(Vector2 iMin, Vector2 iMax, Vector2 oMin, Vector2 oMax, Vector2 value) => Iterpolate(oMin, oMax, InverseIterpolate(iMin, iMax, value));
        public static Vector2 Remap(float iMin, float iMax, Vector2 oMin, Vector2 oMax, float value) => Vector2.Lerp(oMin, oMax, Mathf.InverseLerp(iMin, iMax, value));

        public static Vector2 IterpolateUnclamped(Vector2 min, Vector2 max, Vector2 t) => new(Mathf.LerpUnclamped(min.x, max.x, t.x), Mathf.LerpUnclamped(min.y, max.y, t.y));
        public static Vector2 InverseIterpolateUnclamped(Vector2 min, Vector2 max, Vector2 value) => new(FloatUtils.InverseLerpUnclamped(min.x, max.x, value.x), FloatUtils.InverseLerpUnclamped(min.y, max.y, value.y));
        public static Vector2 RemapUnclamped(Vector2 iMin, Vector2 iMax, Vector2 oMin, Vector2 oMax, Vector2 value) => IterpolateUnclamped(oMin, oMax, InverseIterpolateUnclamped(iMin, iMax, value));
        public static Vector2 RemapUnclamped(float iMin, float iMax, Vector2 oMin, Vector2 oMax, float value) => Vector2.LerpUnclamped(oMin, oMax, FloatUtils.InverseLerpUnclamped(iMin, iMax, value));

        public static Vector2 OperatedByParts(Vector2 v0, Vector2 v1, Func<float, float, float> operation) => new(operation(v0.x, v1.x), operation(v0.y, v1.y));

    }

    public static partial class Vector2IntUtils {

        public static float SqrDistance(Vector2Int v0, Vector2Int v1) => Vector2.SqrMagnitude(v1 - v0);
        public static int ManhatanDistance(Vector2Int v0, Vector2Int v1) => OperatedByParts(v0, v1, (v0, v1) => v1 - v0).ManhatanMagnitude();

        public static Vector2Int ClampInside(Vector2Int value, Vector2Int min, Vector2Int max) => Vector2Int.Max(Vector2Int.Min(value, max), min);
        public static Vector2Int ClampInside(Vector2Int value, RectInt limits) => ClampInside(value, limits.min, limits.max);

        public static Vector2Int ClampOutside(Vector2Int value, Vector2Int min, Vector2Int max) {
            Vector2Int result = value;
            Vector2Int minDelta = value - min;
            Vector2Int maxDelta = value - max;
            float selectedDelta = Mathf.Min(minDelta.x, minDelta.y, maxDelta.x, maxDelta.y);
            if (selectedDelta == minDelta.x) {
                result.x = minDelta.x;
            }
            else if (selectedDelta == maxDelta.x) {
                result.x = maxDelta.x;
            }
            else if (selectedDelta == minDelta.y) {
                result.y = minDelta.y;
            }
            else {
                result.y = maxDelta.y;
            }
            return result;
        }
        public static Vector2Int ClampOutside(Vector2Int value, RectInt limits) => ClampOutside(value, limits.min, limits.max);

        public static Vector2Int OperatedByParts(Vector2Int v0, Vector2Int v1, Func<int, int, int> operation) => new(operation(v0.x, v1.x), operation(v0.y, v1.y));

        #region Grid2D
        public static bool AreNeighbors(Vector2Int v0, Vector2Int v1, bool includeDiagonals = false) => v0.Neighbors(includeDiagonals).Contains(v1);
        public static List<Vector2Int> Neighbors(bool includeDiagonals = false) => includeDiagonals ? 
            new() { new(0, 1), new(1, 1), new(1, 0), new(1, -1), new(0, -1), new(-1, -1), new(-1, 0), new(-1, 1) } : 
            new() { new(0, 1), new(1, 0), new(0, -1), new(-1, 0) };
        #endregion

    }

    public static partial class Vector3Utils {

        public static float SqrDistance(Vector3 v0, Vector3 v1) => Vector3.SqrMagnitude(v1 - v0);
        public static float ManhatanDistance(Vector3 v0, Vector3 v1) => OperatedByParts(v0, v1, (v0, v1) => v1 - v0).ManhatanMagnitude();

        public static Vector3 ClampInside(Vector3 value, Vector3 min, Vector3 max) => Vector3.Max(Vector3.Min(value, max), min);
        public static Vector3 ClampInside(Vector3 value, Rect3 limits) => ClampInside(value, limits.Min, limits.Max);

        public static Vector3 ClampOutside(Vector3 value, Vector3 min, Vector3 max) {
            Vector3 result = value;
            Vector3 minDelta = value - min;
            Vector3 maxDelta = value - max;
            float selectedDelta = Mathf.Min(minDelta.x, minDelta.y, maxDelta.x, maxDelta.y);
            if (selectedDelta == minDelta.x) {
                result.x = minDelta.x;
            }
            else if (selectedDelta == maxDelta.x) {
                result.x = maxDelta.x;
            }
            else if (selectedDelta == minDelta.y) {
                result.y = minDelta.y;
            }
            else {
                result.y = maxDelta.y;
            }
            return result;
        }
        public static Vector3 ClampOutside(Vector3 value, Rect3 limits) => ClampOutside(value, limits.Min, limits.Max);

        public static Vector3 Iterpolate(Vector3 min, Vector3 max, Vector3 t) => new(Mathf.Lerp(min.x, max.x, t.x), Mathf.Lerp(min.y, max.y, t.y), Mathf.Lerp(min.z, max.z, t.z));
        public static Vector3 InverseIterpolate(Vector3 min, Vector3 max, Vector3 value) => new(Mathf.InverseLerp(min.x, max.x, value.x), Mathf.InverseLerp(min.y, max.y, value.y), Mathf.InverseLerp(min.z, max.z, value.z));
        public static Vector3 Remap(Vector3 iMin, Vector3 iMax, Vector3 oMin, Vector3 oMax, Vector3 value) => Iterpolate(oMin, oMax, InverseIterpolate(iMin, iMax, value));
        public static Vector3 Remap(float iMin, float iMax, Vector3 oMin, Vector3 oMax, float value) => Vector3.Lerp(oMin, oMax, Mathf.InverseLerp(iMin, iMax, value));

        public static Vector3 IterpolateUnclamped(Vector3 min, Vector3 max, Vector3 t) => new(Mathf.LerpUnclamped(min.x, max.x, t.x), Mathf.LerpUnclamped(min.y, max.y, t.y), Mathf.LerpUnclamped(min.z, max.z, t.z));
        public static Vector3 InverseIterpolateUnclamped(Vector3 min, Vector3 max, Vector3 value) => new(FloatUtils.InverseLerpUnclamped(min.x, max.x, value.x), FloatUtils.InverseLerpUnclamped(min.y, max.y, value.y), FloatUtils.InverseLerpUnclamped(min.z, max.z, value.z));
        public static Vector3 RemapUnclamped(Vector3 iMin, Vector3 iMax, Vector3 oMin, Vector3 oMax, Vector3 value) => IterpolateUnclamped(oMin, oMax, InverseIterpolateUnclamped(iMin, iMax, value));
        public static Vector3 RemapUnclamped(float iMin, float iMax, Vector3 oMin, Vector3 oMax, float value) => Vector3.LerpUnclamped(oMin, oMax, FloatUtils.InverseLerpUnclamped(iMin, iMax, value));

        public static Vector3 OperatedByParts(Vector3 v0, Vector3 v1, Func<float, float, float> operation) => new(operation(v0.x, v1.x), operation(v0.y, v1.y), operation(v0.z, v1.z));

    }

    public static partial class Vector3IntUtils {

        public static float SqrDistance(Vector3Int v0, Vector3Int v1) => Vector3.SqrMagnitude(v1 - v0);
        public static int ManhatanDistance(Vector3Int v0, Vector3Int v1) => OperatedByParts(v0, v1, (v0, v1) => v1 - v0).ManhatanMagnitude();

        public static Vector3Int ClampInside(Vector3Int value, Vector3Int min, Vector3Int max) => Vector3Int.Max(Vector3Int.Min(value, max), min);
        public static Vector3Int ClampInside(Vector3Int value, Rect3Int limits) => ClampInside(value, limits.Min, limits.Max);

        public static Vector3Int ClampOutside(Vector3Int value, Vector3Int min, Vector3Int max) {
            Vector3Int result = value;
            Vector3Int minDelta = value - min;
            Vector3Int maxDelta = value - max;
            float selectedDelta = Mathf.Min(minDelta.x, minDelta.y, maxDelta.x, maxDelta.y);
            if (selectedDelta == minDelta.x) {
                result.x = minDelta.x;
            }
            else if (selectedDelta == maxDelta.x) {
                result.x = maxDelta.x;
            }
            else if (selectedDelta == minDelta.y) {
                result.y = minDelta.y;
            }
            else {
                result.y = maxDelta.y;
            }
            return result;
        }
        public static Vector3Int ClampOutside(Vector3Int value, Rect3Int limits) => ClampOutside(value, limits.Min, limits.Max);

        public static Vector3Int OperatedByParts(Vector3Int v0, Vector3Int v1, Func<int, int, int> operation) => new(operation(v0.x, v1.x), operation(v0.y, v1.y), operation(v0.z, v1.z));

        #region Grid3D
        public static bool AreNeighbors(Vector3Int first, Vector3Int second, Neighboring3Type neighboringRule = Neighboring3Type.SharesFace) => first.Neighbors(neighboringRule).Contains(second);
        public static List<Vector3Int> Neighbors(Neighboring3Type neighboringRule = Neighboring3Type.SharesFace) {
            List<Vector3Int> result = new();
            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    for (int k = -1; k <= 1; k++) {
                        Vector3Int v = new(i, j, k);
                        int m = v.ManhatanMagnitude();
                        if ((m == 1 && neighboringRule.HasFlag(Neighboring3Type.SharesFace))
                            || (m == 2 && neighboringRule.HasFlag(Neighboring3Type.SharesEdge))
                            || (m == 3 && neighboringRule.HasFlag(Neighboring3Type.SharesVertex))) {
                            result.Add(v);
                        }
                    }
                }
            }
            return result;
        }
        #endregion

    }

    public static partial class Vector4Utils {

        public static float SqrDistance(Vector4 v0, Vector4 v1) => Vector4.SqrMagnitude(v1 - v0);
        public static float ManhatanDistance(Vector4 v0, Vector4 v1) => OperatedByParts(v0, v1, (v0, v1) => v1 - v0).ManhatanMagnitude();

        public static Vector4 OperatedByParts(Vector4 v0, Vector4 v1, Func<float, float, float> operation) => new(operation(v0.x, v1.x), operation(v0.y, v1.y), operation(v0.z, v1.z), operation(v0.w, v1.w));

    }

}
