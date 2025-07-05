using System;
using UnityEngine;

namespace LunaLib {
    [Serializable]
    public struct Rect3 : IEquatable<Rect3> {

        public Vector3 position;
        public Vector3 size;

        public static Rect3 Zero => new(Vector3.zero, Vector3.zero);

        public Vector3 Center {
            get => position + (size / 2f);
            set => position = value - (size / 2f);
        }
        public Vector3 Min {
            get => position;
            set {
                size -= value - position;
                position = value;
            }
        }
        public Vector3 Max {
            get => position + size;
            set => size = value - position;
        }
        public float Width {
            get => size.x;
            set => size.x = value;
        }
        public float Height {
            get => size.y;
            set => size.y = value;
        }
        public float Depth {
            get => size.z;
            set => size.z = value;
        }

        public Rect3(float x, float y, float z, float width, float height, float depth) {
            position = new Vector3(x, y, z);
            size = new Vector3(width, height, depth);
        }
        public Rect3(Vector3 position, Vector3 size) {
            this.position = position;
            this.size = size;
        }
        public Rect3(Rect3 source) {
            position = source.position;
            size = source.size;
        }

        public bool Contains(Vector3 point) {
            return (point.x >= Min.x && point.x <= Max.x)
                && (point.y >= Min.y && point.y <= Max.y)
                && (point.z >= Min.z && point.z <= Max.z);
        }
        public bool Overlaps(Rect3 other) {
            return (other.Max.x >= Min.x && other.Min.x <= Max.x)
                && (other.Max.y >= Min.y && other.Min.y <= Max.y)
                && (other.Max.z >= Min.z && other.Min.z <= Max.z);
        }

        public Vector3 RandomPointInside() {
            return new Vector3(UnityEngine.Random.Range(Min.x, Max.x), UnityEngine.Random.Range(Min.y, Max.y), UnityEngine.Random.Range(Min.z, Max.z));
        }

        public Vector3 LerpPoint(Vector3 t) => Vector3Utils.Iterpolate(Min, Max, t);
        public Vector3 UnlerpPoint(Vector3 v) => Vector3Utils.InverseIterpolate(Min, Max, v);

        public static bool operator ==(Rect3 lhs, Rect3 rhs) => (lhs.position == rhs.position) && (lhs.size == rhs.size);
        public static bool operator !=(Rect3 lhs, Rect3 rhs) => (lhs.position != rhs.position) || (lhs.size != rhs.size);
        public bool Equals(Rect3 other) => position.Equals(other.position) && size.Equals(other.size);
        public override bool Equals(object obj) => obj is Rect3 rect && Equals(rect);
        public override int GetHashCode() => HashCode.Combine(position, size);
        public override string ToString() => position.ToString() + ", " + size.ToString();

    }
}