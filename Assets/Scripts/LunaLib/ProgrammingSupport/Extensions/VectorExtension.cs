using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {

    public static partial class Vector2Extension {

        public static Vector2 Rounded(this Vector2 v) => new(Mathf.Round(v.x), Mathf.Round(v.y));
        public static Vector2 Floored(this Vector2 v) => new(Mathf.Floor(v.x), Mathf.Floor(v.y));
        public static Vector2 Ceiled(this Vector2 v) => new(Mathf.Ceil(v.x), Mathf.Ceil(v.y));
        public static Vector2 Rounded(this Vector2 v, RoundMethod method) => method switch {
            RoundMethod.Round => v.Rounded(),
            RoundMethod.Floor => v.Floored(),
            RoundMethod.Ceil => v.Ceiled(),
            _ => v,
        };
        public static Vector2Int RoundedToInt(this Vector2 v) => Vector2Int.RoundToInt(v);
        public static Vector2Int FlooredToInt(this Vector2 v) => Vector2Int.FloorToInt(v);
        public static Vector2Int CeiledToInt(this Vector2 v) => Vector2Int.CeilToInt(v);
        public static Vector2Int RoundedToInt(this Vector2 v, RoundMethod method) => method switch {
            RoundMethod.Round => v.RoundedToInt(),
            RoundMethod.Floor => v.FlooredToInt(),
            RoundMethod.Ceil => v.CeiledToInt(),
            _ => default,
        };

        public static Vector2 Clamped(this Vector2 v, Vector2 min, Vector2 max) => Vector2Utils.ClampInside(v, min, max);
        public static Vector2 Clamped(this Vector2 v, Rect limits) => Vector2Utils.ClampInside(v, limits);

        public static Vector2 Abs(this Vector2 v) => v.WithParts(Mathf.Abs);

        public static Vector2 RotatedBy(this Vector2 v, float angle) => Quaternion.AngleAxis(angle, Vector3.forward) * v;
        public static Vector2 RotatedBy(this Vector2 v, float angle, Vector2 point) => (v - point).RotatedBy(angle) + point;
        public static Vector2 ScaledBy(this Vector2 v, Vector2 other) => Vector2Utils.OperatedByParts(v, other, (v0, v1) => v0 * v1);
        public static Vector2 DividedBy(this Vector2 v, Vector2 other) => Vector2Utils.OperatedByParts(v, other, (v0, v1) => v0 / v1);
        public static Vector2 FilteredBy(this Vector2 v, Vector2Bool other) => new(other.x? v.x : 0f, other.y ? v.y : 0f);

        public static Vector2 MultipliedBy(this Vector2 v, float[,] matrix) => new(
            matrix[0, 0] * v.x + matrix[0, 1] * v.y, 
            matrix[1, 0] * v.x + matrix[1, 1] * v.y
        );

        public static float DistanceTo(this Vector2 v, Vector2 other) => Vector2.Distance(v, other);
        public static float SqrDistanceTo(this Vector2 v, Vector2 other) => Vector2Utils.SqrDistance(v, other);
        public static float ManhatanDistanceTo(this Vector2 v, Vector2 other) => Vector2Utils.ManhatanDistance(v, other);
        public static float ManhatanMagnitude(this Vector2 v) => v.Abs().AddedParts();

        public static Vector2 WithX(this Vector2 v, float x) => new(x, v.y);
        public static Vector2 WithY(this Vector2 v, float y) => new(v.x, y);
        public static Vector2 WithX(this Vector2 v, Func<float, float> operation) => new(operation(v.x), v.y);
        public static Vector2 WithY(this Vector2 v, Func<float, float> operation) => new(v.x, operation(v.y));
        public static Vector2 WithInvertedX(this Vector2 v) => v.WithX(-v.x);
        public static Vector2 WithInvertedY(this Vector2 v) => v.WithY(-v.y);
        public static Vector2 WithMagnitude(this Vector2 v, float newMagnitude) => v.normalized * newMagnitude;
        public static Vector2 WithMagnitude(this Vector2 v, Func<float, float> operation) => v.normalized * operation(v.magnitude);

        public static Vector2 WithParts(this Vector2 v, Func<float, float> operation) => new(operation(v.x), operation(v.y));
        public static bool AllParts(this Vector2 v, Predicate<float> match) => v.WithBoolParts(v => match(v)).AndParts();
        public static bool AnyPart(this Vector2 v, Predicate<float> match) => v.WithBoolParts(v => match(v)).OrParts();
        public static float AddedParts(this Vector2 v) => v.x + v.y;
        public static float MultipliedParts(this Vector2 v) => v.x * v.y;
        public static Vector2Int WithIntParts(this Vector2 v, Func<float, int> toInt) => new(toInt(v.x), toInt(v.y));
        public static Vector2Bool WithBoolParts(this Vector2 v, Func<float, bool> toBool) => new(toBool(v.x), toBool(v.y));

        public static Vector2 ProjectedToX(this Vector2 v) => v.WithY(0f);
        public static Vector2 ProjectedToY(this Vector2 v) => v.WithX(0f);
        public static Vector2 MidpointTo(this Vector2 v, Vector2 other) => Geometry.Midpoint(v, other);

        public static Vector2 YX(this Vector2 v) => new(v.y, v.x);
        
        public static Vector3 ToV3(this Vector2 v, AxisPlane plane) => plane switch { 
            AxisPlane.XY => new(v.x, v.y, 0f), 
            AxisPlane.XZ => new(v.x, 0f, v.y), 
            AxisPlane.YZ => new(0f, v.x, v.y), 
            _ => default 
        };

    }

    public static partial class Vector2IntExtension {

        public static Vector2Int Clamped(this Vector2Int v, Vector2Int min, Vector2Int max) => Vector2IntUtils.ClampInside(v, min, max);
        public static Vector2Int Clamped(this Vector2Int v, RectInt limits) => Vector2IntUtils.ClampInside(v, limits.min, limits.max);

        public static Vector2Int Abs(this Vector2Int v) => v.WithParts(v => v.Abs());

        public static Vector2Int RotatedBy90Clockwise(this Vector2Int v) => new(v.y, -v.x);
        public static Vector2Int RotatedBy180Clockwise(this Vector2Int v) => -v;
        public static Vector2Int RotatedBy270Clockwise(this Vector2Int v) => new(-v.y, v.x);
        public static Vector2Int RotatedBy90Clockwise(this Vector2Int v, int times) => (times % 4) switch {
            0 => v,
            1 => v.RotatedBy90Clockwise(),
            2 => v.RotatedBy180Clockwise(),
            3 => v.RotatedBy270Clockwise(),
            _ => Vector2Int.zero,
        };
        public static Vector2Int RotatedBy90Counterclockwise(this Vector2Int v, int times) => (times % 4) switch {
            0 => v,
            1 => v.RotatedBy270Clockwise(),
            2 => v.RotatedBy180Clockwise(),
            3 => v.RotatedBy90Clockwise(),
            _ => Vector2Int.zero,
        };
        public static Vector2Int ScaledBy(this Vector2Int v, Vector2Int other) => Vector2IntUtils.OperatedByParts(v, other, (v0, v1) => v0 * v1);
        public static Vector2Int DividedBy(this Vector2Int v, Vector2Int other) => Vector2IntUtils.OperatedByParts(v, other, (v0, v1) => v0 / v1);
        public static Vector2Int FilteredBy(this Vector2Int v, Vector2Bool other) => new(other.x ? v.x : 0, other.y ? v.y : 0);

        public static Vector2Int MultipliedBy(this Vector2Int v, int[,] matrix) => new(
            matrix[0, 0] * v.x + matrix[0, 1] * v.y, 
            matrix[1, 0] * v.x + matrix[1, 1] * v.y
        );

        public static float DistanceTo(this Vector2Int v, Vector2Int other) => Vector2Int.Distance(v, other);
        public static float SqrDistanceTo(this Vector2Int v, Vector2Int other) => Vector2IntUtils.SqrDistance(v, other);
        public static int ManhatanDistanceTo(this Vector2Int v, Vector2Int other) => Vector2IntUtils.ManhatanDistance(v, other);
        public static int ManhatanMagnitude(this Vector2Int v) => v.Abs().AddedParts();

        public static Vector2Int WithX(this Vector2Int v, int x) => new(x, v.y);
        public static Vector2Int WithY(this Vector2Int v, int y) => new(v.x, y);
        public static Vector2Int WithX(this Vector2Int v, Func<int, int> operation) => new(operation(v.x), v.y);
        public static Vector2Int WithY(this Vector2Int v, Func<int, int> operation) => new(v.x, operation(v.y));
        public static Vector2Int WithInvertedX(this Vector2Int v) => v.WithX(-v.x);
        public static Vector2Int WithInvertedY(this Vector2Int v) => v.WithY(-v.y);

        public static Vector2Int WithParts(this Vector2Int v, Func<int, int> operation) => new(operation(v.x), operation(v.y));
        public static bool AllParts(this Vector2Int v, Predicate<int> match) => v.WithBoolParts(v => match(v)).AndParts();
        public static bool AnyPart(this Vector2Int v, Predicate<int> match) => v.WithBoolParts(v => match(v)).OrParts();
        public static int AddedParts(this Vector2Int v) => v.x + v.y;
        public static int MultipliedParts(this Vector2Int v) => v.x * v.y;
        public static Vector2 WithfloatParts(this Vector2Int v, Func<int, float> toFloat) => new(toFloat(v.x), toFloat(v.y));
        public static Vector2Bool WithBoolParts(this Vector2Int v, Func<int, bool> toBool) => new(toBool(v.x), toBool(v.y));

        public static Vector2Int ProjectedToX(this Vector2Int v) => v.WithY(0);
        public static Vector2Int ProjectedToY(this Vector2Int v) => v.WithX(0);

        public static Vector2Int YX(this Vector2Int v) => new(v.y, v.x);

        public static Vector3 ToVec3(this Vector2Int v) => new(v.x, v.y, 0f);

        #region Grid2D
        public static bool IsNeighborsWith(this Vector2Int self, Vector2Int other, bool includeDiagonals = false) => Vector2IntUtils.AreNeighbors(self, other, includeDiagonals);
        public static List<Vector2Int> Neighbors(this Vector2Int v, bool includeDiagonals = false) => Vector2IntUtils.Neighbors(includeDiagonals).ThruFuncElement(n => n + v);
        #endregion

    }

    public static partial class Vector3Extension {

        public static Vector3 Rounded(this Vector3 v) => new(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
        public static Vector3 Floored(this Vector3 v) => new(Mathf.Floor(v.x), Mathf.Floor(v.y), Mathf.Floor(v.z));
        public static Vector3 Ceiled(this Vector3 v) => new(Mathf.Ceil(v.x), Mathf.Ceil(v.y), Mathf.Ceil(v.z));
        public static Vector3 Rounded(this Vector3 v, RoundMethod method) => method switch {
            RoundMethod.Round => v.Rounded(),
            RoundMethod.Floor => v.Floored(),
            RoundMethod.Ceil => v.Ceiled(),
            _ => v,
        };
        public static Vector3Int RoundedToInt(this Vector3 v) => Vector3Int.RoundToInt(v);
        public static Vector3Int FlooredToInt(this Vector3 v) => Vector3Int.FloorToInt(v);
        public static Vector3Int CeiledToInt(this Vector3 v) => Vector3Int.CeilToInt(v);
        public static Vector3Int RoundedToInt(this Vector3 v, RoundMethod method) => method switch {
            RoundMethod.Round => v.RoundedToInt(),
            RoundMethod.Floor => v.FlooredToInt(),
            RoundMethod.Ceil => v.CeiledToInt(),
            _ => default,
        };

        public static Vector3 Clamped(this Vector3 v, Vector3 min, Vector3 max) => Vector3Utils.ClampInside(v, min, max);
        public static Vector3 Clamped(this Vector3 v, Rect3 limits) => Vector3Utils.ClampInside(v, limits);

        public static Vector3 Abs(this Vector3 v) => v.WithParts(Mathf.Abs);

        public static Vector3 ScaledBy(this Vector3 v, Vector3 other) => Vector3Utils.OperatedByParts(v, other, (v0, v1) => v0 * v1);
        public static Vector3 DividedBy(this Vector3 v, Vector3 other) => Vector3Utils.OperatedByParts(v, other, (v0, v1) => v0 / v1);
        public static Vector3 FilteredBy(this Vector3 v, Vector3Bool other) => new(other.x ? v.x : 0f, other.y ? v.y : 0f, other.z ? v.z : 0f);

        public static Vector3 MultipliedBy(this Vector3 v, float[,] matrix) => new(
            matrix[0, 0] * v.x + matrix[0, 1] * v.y + matrix[0, 2] * v.z, 
            matrix[1, 0] * v.x + matrix[1, 1] * v.y + matrix[1, 2] * v.z,
            matrix[2, 0] * v.x + matrix[2, 1] * v.y + matrix[2, 2] * v.z
        );

        public static float DistanceTo(this Vector3 v, Vector3 other) => Vector3.Distance(v, other);
        public static float SqrDistanceTo(this Vector3 v, Vector3 other) => Vector3Utils.SqrDistance(v, other);
        public static float ManhatanDistanceTo(this Vector3 v, Vector3 other) => Vector3Utils.ManhatanDistance(v, other);
        public static float ManhatanMagnitude(this Vector3 v) => v.Abs().AddedParts();

        public static Vector3 WithX(this Vector3 v, float x) => new(x, v.y, v.z);
        public static Vector3 WithY(this Vector3 v, float y) => new(v.x, y, v.z);
        public static Vector3 WithZ(this Vector3 v, float z) => new(v.x, v.y, z);
        public static Vector3 WithX(this Vector3 v, Func<float, float> operation) => new(operation(v.x), v.y, v.z);
        public static Vector3 WithY(this Vector3 v, Func<float, float> operation) => new(v.x, operation(v.y), v.z);
        public static Vector3 WithZ(this Vector3 v, Func<float, float> operation) => new(v.x, v.y, operation(v.z));
        public static Vector3 WithInvertedX(this Vector3 v) => v.WithX(-v.x);
        public static Vector3 WithInvertedY(this Vector3 v) => v.WithY(-v.y);
        public static Vector3 WithInvertedZ(this Vector3 v) => v.WithZ(-v.z);
        public static Vector3 WithMagnitude(this Vector3 v, float newMagnitude) => v.normalized * newMagnitude;
        public static Vector3 WithMagnitude(this Vector3 v, Func<float, float> operation) => v.normalized * operation(v.magnitude);

        public static Vector3 WithParts(this Vector3 v, Func<float, float> operation) => new(operation(v.x), operation(v.y), operation(v.z));
        public static bool AllParts(this Vector3 v, Predicate<float> match) => v.WithBoolParts(v => match(v)).AndParts();
        public static bool AnyPart(this Vector3 v, Predicate<float> match) => v.WithBoolParts(v => match(v)).OrParts();
        public static float AddedParts(this Vector3 v) => v.x + v.y + v.z;
        public static float MultipliedParts(this Vector3 v) => v.x * v.y * v.z;
        public static Vector3Int WithIntParts(this Vector3 v, Func<float, int> toInt) => new(toInt(v.x), toInt(v.y), toInt(v.z));
        public static Vector3Bool WithBoolParts(this Vector3 v, Func<float, bool> toBool) => new(toBool(v.x), toBool(v.y), toBool(v.z));

        public static Vector3 LerpedTo(this Vector3 v, Vector3 other, float t) => Vector3.Lerp(v, other, t);

        public static Vector3 ProjectedToX(this Vector3 v) => new(v.x, 0f, 0f);
        public static Vector3 ProjectedToY(this Vector3 v) => new(0f, v.y, 0f);
        public static Vector3 ProjectedToZ(this Vector3 v) => new(0f, 0f, v.z);
        public static Vector3 ProjectedToXY(this Vector3 v) => v.WithZ(0f);
        public static Vector3 ProjectedToXZ(this Vector3 v) => v.WithY(0f);
        public static Vector3 ProjectedToYZ(this Vector3 v) => v.WithX(0f);
        public static Vector3 ProjectedToPlane(this Vector3 v, AxisPlane plane) => plane switch { 
            AxisPlane.XY => v.ProjectedToXY(), 
            AxisPlane.XZ => v.ProjectedToXZ(), 
            AxisPlane.YZ => v.ProjectedToYZ(), 
            _ => default 
        };
        public static Vector3 MidpointTo(this Vector3 v, Vector3 other) => (v + other) / 2f;

        public static Vector2 XY(this Vector3 v) => new(v.x, v.y);
        public static Vector2 XZ(this Vector3 v) => new(v.x, v.z);
        public static Vector2 YX(this Vector3 v) => new(v.y, v.x);
        public static Vector2 YZ(this Vector3 v) => new(v.y, v.z);
        public static Vector2 ZX(this Vector3 v) => new(v.z, v.x);
        public static Vector2 ZY(this Vector3 v) => new(v.z, v.y);
        public static Vector3 XZY(this Vector3 v) => new(v.x, v.z, v.y);
        public static Vector3 YXZ(this Vector3 v) => new(v.y, v.x, v.z);
        public static Vector3 YZX(this Vector3 v) => new(v.y, v.z, v.x);
        public static Vector3 ZXY(this Vector3 v) => new(v.z, v.x, v.y);
        public static Vector3 ZYX(this Vector3 v) => new(v.z, v.y, v.x);

    }

    public static partial class Vector3IntExtension {

        public static Vector3Int Clamped(this Vector3Int v, Vector3Int min, Vector3Int max) => Vector3IntUtils.ClampInside(v, min, max);
        public static Vector3Int Clamped(this Vector3Int v, Rect3Int limits) => Vector3IntUtils.ClampInside(v, limits.Min, limits.Max);

        public static Vector3Int Abs(this Vector3Int v) => v.WithParts(v => v.Abs());

        public static Vector3Int ScaledBy(this Vector3Int v, Vector3Int other) => Vector3IntUtils.OperatedByParts(v, other, (v0, v1) => v0 * v1);
        public static Vector3Int DividedBy(this Vector3Int v, Vector3Int other) => Vector3IntUtils.OperatedByParts(v, other, (v0, v1) => v0 / v1);
        public static Vector3Int FilteredBy(this Vector3Int v, Vector3Bool other) => new(other.x ? v.x : 0, other.y ? v.y : 0, other.z ? v.z : 0);

        public static Vector3Int MultipliedBy(this Vector3Int v, int[,] matrix) => new(
            matrix[0, 0] * v.x + matrix[0, 1] * v.y + matrix[0, 2] * v.z,
            matrix[1, 0] * v.x + matrix[1, 1] * v.y + matrix[1, 2] * v.z,
            matrix[2, 0] * v.x + matrix[2, 1] * v.y + matrix[2, 2] * v.z
        );

        public static float DistanceTo(this Vector3Int v, Vector3Int other) => (other - v).magnitude;
        public static int SqrDistanceTo(this Vector3Int v, Vector3Int other) => (other - v).sqrMagnitude;
        public static int ManhatanDistanceTo(this Vector3Int v, Vector3Int other) => Vector3IntUtils.ManhatanDistance(v, other);
        public static int ManhatanMagnitude(this Vector3Int v) => v.Abs().AddedParts();

        public static Vector3Int WithX(this Vector3Int v, int x) => new(x, v.y, v.z);
        public static Vector3Int WithY(this Vector3Int v, int y) => new(v.x, y, v.z);
        public static Vector3Int WithZ(this Vector3Int v, int z) => new(v.x, v.y, z);
        public static Vector3Int WithX(this Vector3Int v, Func<int, int> operation) => new(operation(v.x), v.y, v.z);
        public static Vector3Int WithY(this Vector3Int v, Func<int, int> operation) => new(v.x, operation(v.y), v.z);
        public static Vector3Int WithZ(this Vector3Int v, Func<int, int> operation) => new(v.x, v.y, operation(v.z));
        public static Vector3Int WithInvertedX(this Vector3Int v) => v.WithX(-v.x);
        public static Vector3Int WithInvertedY(this Vector3Int v) => v.WithY(-v.y);
        public static Vector3Int WithInvertedZ(this Vector3Int v) => v.WithZ(-v.z);

        public static Vector3Int WithParts(this Vector3Int v, Func<int, int> operation) => new(operation(v.x), operation(v.y), operation(v.z));
        public static bool AllParts(this Vector3Int v, Predicate<int> match) => v.WithBoolParts(v => match(v)).AndParts();
        public static bool AnyPart(this Vector3Int v, Predicate<int> match) => v.WithBoolParts(v => match(v)).OrParts();
        public static int AddedParts(this Vector3Int v) => v.x + v.y + v.z;
        public static int MultipliedParts(this Vector3Int v) => v.x * v.y * v.z;
        public static Vector3 WithFloatParts(this Vector3Int v, Func<int, float> toFloat) => new(toFloat(v.x), toFloat(v.y), toFloat(v.z));
        public static Vector3Bool WithBoolParts(this Vector3Int v, Func<int, bool> toBool) => new(toBool(v.x), toBool(v.y), toBool(v.z));

        public static Vector3Int ProjectedToX(this Vector3Int v) => new(v.x, 0, 0);
        public static Vector3Int ProjectedToY(this Vector3Int v) => new(0, v.y, 0);
        public static Vector3Int ProjectedToZ(this Vector3Int v) => new(0, 0, v.z);
        public static Vector3Int ProjectedToXY(this Vector3Int v) => v.WithZ(0);
        public static Vector3Int ProjectedToXZ(this Vector3Int v) => v.WithY(0);
        public static Vector3Int ProjectedToYZ(this Vector3Int v) => v.WithX(0);

        public static Vector2Int XY(this Vector3Int v) => new(v.x, v.y);
        public static Vector2Int XZ(this Vector3Int v) => new(v.x, v.z);
        public static Vector2Int YX(this Vector3Int v) => new(v.y, v.x);
        public static Vector2Int YZ(this Vector3Int v) => new(v.y, v.z);
        public static Vector2Int ZX(this Vector3Int v) => new(v.z, v.x);
        public static Vector2Int ZY(this Vector3Int v) => new(v.z, v.y);
        public static Vector3Int XZY(this Vector3Int v) => new(v.x, v.z, v.y);
        public static Vector3Int YXZ(this Vector3Int v) => new(v.y, v.x, v.z);
        public static Vector3Int YZX(this Vector3Int v) => new(v.y, v.z, v.x);
        public static Vector3Int ZXY(this Vector3Int v) => new(v.z, v.x, v.y);
        public static Vector3Int ZYX(this Vector3Int v) => new(v.z, v.y, v.x);

        #region Grid3D
        public static bool IsNeighborsWith(this Vector3Int self, Vector3Int other, Neighboring3Type neighboringRule = Neighboring3Type.SharesFace) => Vector3IntUtils.AreNeighbors(self, other, neighboringRule);
        public static List<Vector3Int> Neighbors(this Vector3Int v, Neighboring3Type neighboringRule = Neighboring3Type.SharesFace) => Vector3IntUtils.Neighbors(neighboringRule).ThruFuncElement(n => n + v);
        #endregion

    }

    public static partial class Vector4Extension {

        public static Vector4 Abs(this Vector4 v) => v.WithParts(Mathf.Abs);

        public static Vector4 ScaledBy(this Vector4 v, Vector4 other) => Vector4Utils.OperatedByParts(v, other, (v0, v1) => v0 * v1);
        public static Vector4 DividedBy(this Vector4 v, Vector4 other) => Vector4Utils.OperatedByParts(v, other, (v0, v1) => v0 / v1);

        public static Vector4 MultipliedBy(this Vector4 v, float[,] matrix) => new(
            matrix[0, 0] * v.x + matrix[0, 1] * v.y + matrix[0, 2] * v.z + matrix[0, 3] * v.w,
            matrix[1, 0] * v.x + matrix[1, 1] * v.y + matrix[1, 2] * v.z + matrix[1, 3] * v.w,
            matrix[2, 0] * v.x + matrix[2, 1] * v.y + matrix[2, 2] * v.z + matrix[2, 3] * v.w,
            matrix[3, 0] * v.x + matrix[3, 1] * v.y + matrix[3, 2] * v.z + matrix[3, 3] * v.w
        );

        public static float DistanceTo(this Vector4 v, Vector4 other) => (other - v).magnitude;
        public static float SqrDistanceTo(this Vector4 v, Vector4 other) => (other - v).sqrMagnitude;
        public static float ManhatanDistanceTo(this Vector4 v, Vector4 other) => Vector4Utils.ManhatanDistance(v, other);
        public static float ManhatanMagnitude(this Vector4 v) => v.Abs().AddedParts();

        public static Vector4 WithX(this Vector4 v, float x) => new(x, v.y, v.z, v.w);
        public static Vector4 WithY(this Vector4 v, float y) => new(v.x, y, v.z, v.w);
        public static Vector4 WithZ(this Vector4 v, float z) => new(v.x, v.y, z, v.w);
        public static Vector4 WithW(this Vector4 v, float w) => new(v.x, v.y, v.z, w);
        public static Vector4 WithX(this Vector4 v, Func<float, float> operation) => new(operation(v.x), v.y, v.z, v.w);
        public static Vector4 WithY(this Vector4 v, Func<float, float> operation) => new(v.x, operation(v.y), v.z, v.w);
        public static Vector4 WithZ(this Vector4 v, Func<float, float> operation) => new(v.x, v.y, operation(v.z), v.w);
        public static Vector4 WithW(this Vector4 v, Func<float, float> operation) => new(v.x, v.y, v.z, operation(v.w));
        public static Vector4 WithInvertedX(this Vector4 v) => v.WithX(-v.x);
        public static Vector4 WithInvertedY(this Vector4 v) => v.WithY(-v.y);
        public static Vector4 WithInvertedZ(this Vector4 v) => v.WithZ(-v.z);
        public static Vector4 WithInvertedW(this Vector4 v) => v.WithW(-v.w);

        public static Vector4 WithParts(this Vector4 v, Func<float, float> operation) => new(operation(v.x), operation(v.y), operation(v.z), operation(v.w));
        public static bool AllParts(this Vector4 v, Predicate<float> match) => match(v.x) && match(v.y) && match(v.z) && match(v.w);
        public static bool AnyPart(this Vector4 v, Predicate<float> match) => match(v.x) || match(v.y) || match(v.z) || match(v.w);
        public static float AddedParts(this Vector4 v) => v.x + v.y + v.z + v.w;
        public static float MultipliedParts(this Vector4 v) => v.x * v.y * v.z * v.w;

        public static Vector4 ProjectedToX(this Vector4 v) => new(v.x, 0f, 0f, 0f);
        public static Vector4 ProjectedToY(this Vector4 v) => new(0f, v.y, 0f, 0f);
        public static Vector4 ProjectedToZ(this Vector4 v) => new(0f, 0f, v.z, 0f);
        public static Vector4 ProjectedToW(this Vector4 v) => new(0f, 0f, 0f, v.w);

    }

    public static partial class Vector2ListExtension {

        public static void ScalePath(this List<Vector2> path, float scale) {
            Vector2 midPoint = (path.First() + path.Last()) / 2f;
            path.SetEachElement(e => (e-midPoint) * scale + midPoint);
        }
        public static float PathLength(this List<Vector2> path) => path.SumPairs(Vector2.Distance);
        public static List<float> PathSegmentLengths(this List<Vector2> path) => path.MergedInPairs(Vector2.Distance);
        public static Vector2 LerpPath(this List<Vector2> path, float t) {
            if (path.FindAccumulatedElementPair(t, Vector2.Distance, out float localT, out Vector2 p1, out Vector2 p2)) {
                return Vector2.Lerp(p1, p2, localT);
            }
            if (t < 0f) {
                return path.First();
            }
            else {
                return path.Last();
            }
        }

    }

    public static partial class Vector3ListExtension {

        public static void ScalePath(this List<Vector2> path, float scale) {
            Vector2 midPoint = (path.First() + path.Last()) / 2f;
            path.SetEachElement(e => (e - midPoint) * scale + midPoint);
        }
        public static float PathLength(this List<Vector3> path) => path.SumPairs(Vector3.Distance);
        public static List<float> PathSegmentLengths(this List<Vector3> path) => path.MergedInPairs(Vector3.Distance);
        public static Vector3 LerpPath(this List<Vector3> path, float t) {
            if (path.FindAccumulatedElementPair(t, Vector3.Distance, out float localT, out Vector3 p1, out Vector3 p2)) {
                return p1.LerpedTo(p2, localT);
            }
            if (t < 0f) {
                return path.First();
            }
            else {
                return path.Last();
            }
        }

    }

    public enum Neighboring2Type {
        SharesEdge = 1,
        SharesVertex = 2,
        SharesBoth = 3
    }

    public enum Neighboring3Type {
        SharesFace = 1,
        SharesEdge = 2,
        SharesVertex = 4,
        All = 7
    }

}
