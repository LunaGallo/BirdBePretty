using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {

    public static class RangeUtils {

        public static Range Lerp(Range r1, Range r2, float t) {
            return new Range(r1.position.LerpedTo(r2.position, t), r1.Size.LerpedTo(r2.Size, t));
        }
        public static float LerpValue(Range r, float t) {
            return Mathf.Lerp(r.Min, r.Max, t);
        }
        public static float InverseLerpValue(Range r, float value) {
            return Mathf.InverseLerp(r.Min, r.Max, value);
        }
        public static float RemapValue(Range from, Range target, float value) {
            return LerpValue(target, InverseLerpValue(from, value));
        }

        public static float Distance(Range r1, Range r2) {
            return Mathf.Max(r1.position.DistanceTo(r2.position), r2.Size.DistanceTo(r1.Size));
        }

    }
    public static class RectUtils {

        public static Rect Lerp(Rect r1, Rect r2, float t) {
            return new Rect(Vector2.Lerp(r1.position, r2.position, t), Vector2.Lerp(r1.size, r2.size, t));
        }
        public static Rect LerpUnclamped(Rect r1, Rect r2, float t) {
            return new Rect(Vector2.LerpUnclamped(r1.position, r2.position, t), Vector2.LerpUnclamped(r1.size, r2.size, t));
        }
        public static Vector2 InterpolateValue(Rect r, Vector2 t) {
            return Vector2Utils.Iterpolate(r.min, r.max, t);
        }
        public static Vector2 InterpolateUnclampedValue(Rect r, Vector2 t) {
            return Vector2Utils.IterpolateUnclamped(r.min, r.max, t);
        }
        public static Vector2 InverseInterpolateValue(Rect r, Vector2 value) {
            return Vector2Utils.InverseIterpolate(r.min, r.max, value);
        }
        public static Vector2 InverseInterpolateUnclampedValue(Rect r, Vector2 value) {
            return Vector2Utils.InverseIterpolateUnclamped(r.min, r.max, value);
        }
        public static Vector2 RemapValue(Rect from, Rect target, Vector2 value) {
            return InterpolateValue(target, InverseInterpolateValue(from, value));
        }

        public static float Distance(Rect r1, Rect r2) {
            return Mathf.Max(r1.position.DistanceTo(r2.position), r2.size.DistanceTo(r1.size));
        }

        public static Rect FromMinMax(Vector2 min, Vector2 max) => new(min, max - min);
        public static Rect FromBounds(List<Vector2> points) {
            Vector2 min = new(float.MaxValue, float.MaxValue);
            Vector2 max = new(float.MinValue, float.MinValue);
            foreach (Vector2 point in points) {
                if (min.x > point.x) min.x = point.x;
                if (min.y > point.y) min.y = point.y;
                if (max.x < point.x) max.x = point.x;
                if (max.y < point.y) max.y = point.y;
            }
            return FromMinMax(min, max);
        }

    }
    public static class Rect3Utils {

        public static Rect3 Lerp(Rect3 r1, Rect3 r2, float t) {
            return new Rect3(r1.position.LerpedTo(r2.position, t), r1.size.LerpedTo(r2.size, t));
        }
        public static Vector3 InterpolateValue(Rect3 r, Vector3 t) {
            return Vector3Utils.Iterpolate(r.Min, r.Max, t);
        }
        public static Vector3 InverseInterpolateValue(Rect3 r, Vector3 value) {
            return Vector3Utils.InverseIterpolate(r.Min, r.Max, value);
        }
        public static Vector3 RemapValue(Rect3 from, Rect3 target, Vector3 value) {
            return InterpolateValue(target, InverseInterpolateValue(from, value));
        }

        public static float Distance(Rect3 r1, Rect3 r2) {
            return Mathf.Max(r1.position.DistanceTo(r2.position), r2.size.DistanceTo(r1.size));
        }

        public static Rect3 FromMinMax(Vector3 min, Vector3 max) => new(min, max - min);
        public static Rect3 FromBounds(List<Vector3> points) {
            Vector3 min = new(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new(float.MinValue, float.MinValue, float.MinValue);
            foreach (Vector3 point in points) {
                if (min.x > point.x) min.x = point.x;
                if (min.y > point.y) min.y = point.y;
                if (min.z > point.z) min.z = point.z;
                if (max.x < point.x) max.x = point.x;
                if (max.y < point.y) max.y = point.y;
                if (max.z < point.z) max.z = point.z;
            }
            return FromMinMax(min, max);
        }
    }

    public static class RectIntUtils {

        public static RectInt.PositionEnumerator Enumerate(Vector2Int lengths) {
            return new RectInt(0, 0, lengths.x, lengths.y).allPositionsWithin;
        }
        public static RectInt.PositionEnumerator Enumerate(int lengthX, int lengthY) {
            return new RectInt(0, 0, lengthX, lengthY).allPositionsWithin;
        }
        public static RectInt.PositionEnumerator Enumerate(Vector2Int starts, Vector2Int lengths) {
            return new RectInt(starts.x, starts.y, lengths.x, lengths.y).allPositionsWithin;
        }
        public static RectInt.PositionEnumerator Enumerate(int startX, int startY, int lengthX, int lengthY) {
            return new RectInt(startX, startY, lengthX, lengthY).allPositionsWithin;
        }

        public static float Distance(RectInt r1, RectInt r2) {
            return Mathf.Max(r1.position.DistanceTo(r2.position), r2.size.DistanceTo(r1.size));
        }

        public static RectInt BoundsIncluding(RectInt r1, RectInt r2) {
            return CreateMinMax(Mathf.Min(r1.xMin, r2.xMin), Mathf.Min(r1.yMin, r2.yMin), Mathf.Max(r1.xMax, r2.xMax), Mathf.Max(r1.yMax, r2.yMax));
        }
        public static RectInt BoundsIncluding(List<RectInt> rects) {
            return rects.Reduce((e,acc) => BoundsIncluding(e, acc));
        }
        public static RectInt CreateMinMax(Vector2Int min, Vector2Int max) => new(min.x, min.y, max.x - min.x, max.y - min.y);
        public static RectInt CreateMinMax(int minX, int minY, int maxX, int maxY) => new(minX,minY, maxX - minX, maxY - minY);

    }
    public static class Rect3IntUtils {

        public static Rect3Int Enumerate(Vector3Int lengths) {
            return new Rect3Int(0, 0, 0, lengths.x - 1, lengths.y - 1, lengths.z - 1);
        }
        public static Rect3Int Enumerate(int lengthX, int lengthY, int lengthZ) {
            return new Rect3Int(0, 0, 0, lengthX - 1, lengthY - 1, lengthZ - 1);
        }
        public static Rect3Int Enumerate(Vector3Int starts, Vector3Int lengths) {
            return new Rect3Int(starts.x, starts.y, starts.z, lengths.x - 1, lengths.y - 1, lengths.z - 1);
        }
        public static Rect3Int Enumerate(int startX, int startY, int startZ, int lengthX, int lengthY, int lengthZ) {
            return new Rect3Int(startX, startY, startZ, lengthX - 1, lengthY - 1, lengthZ - 1);
        }

        public static float Distance(Rect3Int r1, Rect3Int r2) {
            return Mathf.Max(r1.position.DistanceTo(r2.position), r2.size.DistanceTo(r1.size));
        }

    }

}
