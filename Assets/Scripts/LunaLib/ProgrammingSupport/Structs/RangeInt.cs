using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    [Serializable]
    public struct RangeInt : IEnumerable<int>, IEquatable<RangeInt> {

        public int position;
        [SerializeField] private int size;

        public int Size {
            get => size;
            set {
                size = ValidateSize(value);
            }
        }
        public int Center {
            get => position + (size / 2);
            set => position = Mathf.RoundToInt(value - (size / 2));
        }
        public int Min {
            get => position;
            set {
                Size -= value - position;
                position = value;
            }
        }
        public int Max {
            get => position + size;
            set => Size = value - position;
        }

        public RangeInt(int position, int size) {
            this.position = position;
            this.size = ValidateSize(size);
        }
        public static RangeInt MinMax(int min, int max) => new(min, max-min);
        public static int ValidateSize(int value) => Mathf.Max(value, 0);

        public void Inset(int thickness) {
            thickness = Mathf.Max(thickness, Size / 2);
            position += thickness;
            Size -= thickness * 2;
        }
        public void Outset(int thickness) {
            thickness = Mathf.Max(thickness, -Size / 2);
            position -= thickness;
            Size += thickness * 2;
        }

        public bool Contains(int point) => point >= Min && point <= Max;
        public bool Overlaps(RangeInt other) => other.Max >= Min && other.Min <= Max;
        public int Clamp(int value) => Mathf.Clamp(value, Min, Max);

        public float InverseLerp(int value) => Mathf.InverseLerp(Min, Max, value);

        public int RandomValue() => UnityEngine.Random.Range(Min, Max);

        public static RangeInt Zero => new(0, 0);
        public static RangeInt ZeroOne => new(0, 1);

        public bool Equals(RangeInt other) => position == other.position && size == other.size;
        public override bool Equals(object obj) => obj is RangeInt range && Equals(range);
        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => position + ", " + size;

        public static bool operator ==(RangeInt lhs, RangeInt rhs) => (lhs.position == rhs.position) && (lhs.size == rhs.size);
        public static bool operator !=(RangeInt lhs, RangeInt rhs) => (lhs.position != rhs.position) || (lhs.size != rhs.size);
        public static RangeInt operator +(RangeInt lhs, RangeInt rhs) => MinMax(Mathf.Min(lhs.Min, rhs.Min), Mathf.Max(lhs.Max, rhs.Max));
        public static RangeInt operator *(RangeInt lhs, RangeInt rhs) => MinMax(Mathf.Max(lhs.Min, rhs.Min), Mathf.Min(lhs.Max, rhs.Max));
        public static implicit operator Range(RangeInt t) => new(t.position, t.size);

        public IEnumerator<int> GetEnumerator() {
            for (int i = Min; i <= Max; i++) {
                yield return i;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
