using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    [Serializable]
    public struct Range : IEquatable<Range> {

        public float position;
        [SerializeField] private float size;

        public float Size {
            get => size;
            set {
                size = ValidateSize(value);
            }
        }
        public float Center {
            get => position + (size / 2f);
            set => position = value - (size / 2f);
        }
        public float Min {
            get => position;
            set {
                Size -= (value - position);
                position = value;
            }
        }
        public float Max {
            get => position + size;
            set => Size = value - position;
        }

        public Range(float position, float size) {
            this.position = position;
            this.size = ValidateSize(size);
        }
        public static Range MinMax(float min, float max) => new(min, max - min);
        public static float ValidateSize(float value) => Mathf.Max(value, 0f);

        public void Inset(float thickness) {
            thickness = Mathf.Max(thickness, Size / 2f);
            position += thickness;
            Size -= thickness * 2f;
        }
        public void Outset(float thickness) {
            thickness = Mathf.Max(thickness, -Size / 2f);
            position -= thickness;
            Size += thickness * 2f;
        }
        public void ScaleFromPivot(float factor, float pivot) {
            float newSize = ValidateSize(Size * factor);
            position -= Mathf.Lerp(0f, newSize - Size, pivot);
            Size = newSize;
        }

        public bool Contains(float point) => point >= Min && point <= Max;
        public bool Overlaps(Range other) => other.Max >= Min && other.Min <= Max;
        public float Clamp(float value) => Mathf.Clamp(value, Min, Max);

        public float Lerp(float t) => Mathf.Lerp(Min, Max, t);
        public float LerpUnclamped(float t) => Mathf.LerpUnclamped(Min, Max, t);
        public float InverseLerp(float value) => Mathf.InverseLerp(Min, Max, value);
        public float InverseLerpUnclamped(float value) => FloatUtils.InverseLerpUnclamped(Min, Max, value);
        public float RemapTo(Range target, float value) => target.Lerp(InverseLerp(value));

        public float RandomValue() => UnityEngine.Random.Range(Min, Max);

        public RangeInt RoundToInt() => new(Mathf.RoundToInt(position), Mathf.RoundToInt(size));
        public RangeInt RoundToIntMinMax() => RangeInt.MinMax(Mathf.RoundToInt(Min), Mathf.RoundToInt(Max));

        public static Range Zero => new(0f, 0f);
        public static Range ZeroOne => new(0f, 1f);

        public bool Equals(Range other) => position == other.position && size == other.size;
        public override bool Equals(object obj) => obj is Range range && Equals(range);
        public override int GetHashCode() => HashCode.Combine(position, size);
        public override string ToString() => position + ", " + size;

        public static bool operator ==(Range lhs, Range rhs) => (lhs.position == rhs.position) && (lhs.size == rhs.size);
        public static bool operator !=(Range lhs, Range rhs) => (lhs.position != rhs.position) || (lhs.size != rhs.size);
        public static Range operator +(Range lhs, Range rhs) => MinMax(Mathf.Min(lhs.Min, rhs.Min), Mathf.Max(lhs.Max, rhs.Max));
        public static Range operator *(Range lhs, Range rhs) => MinMax(Mathf.Max(lhs.Min, rhs.Min), Mathf.Min(lhs.Max, rhs.Max));

    }
}