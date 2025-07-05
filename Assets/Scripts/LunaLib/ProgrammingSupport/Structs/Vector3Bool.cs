using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    [Serializable]
    public struct Vector3Bool : IEquatable<Vector3Bool> {

        public bool x, y, z;

        public Vector3Bool(bool x, bool y, bool z) { this.x = x; this.y = y; this.z = z; }

        public bool AndParts() => x && y && z;
        public bool OrParts() => x || y || z;

        public static Vector3Bool False => new(false, false, false);
        public static Vector3Bool True => new(true, true, true);

        public bool Equals(Vector3Bool other) => x == other.x && y == other.y && z == other.z;
        public override bool Equals(object obj) => obj is Vector3Bool other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(x, y, z);
        public override string ToString() => x + ", " + y + ", " + z;

        public static bool operator ==(Vector3Bool lhs, Vector3Bool rhs) => (lhs.x == rhs.x) && (lhs.y == rhs.y) && (lhs.z == rhs.z);
        public static bool operator !=(Vector3Bool lhs, Vector3Bool rhs) => (lhs.x != rhs.x) || (lhs.y != rhs.y) || (lhs.z != rhs.z);
        public static Vector3Bool operator !(Vector3Bool v) => new(!v.x, !v.y, !v.z);
        public static Vector3Bool operator +(Vector3Bool lhs, Vector3Bool rhs) => new(lhs.x || rhs.x, lhs.y || rhs.y, lhs.z || rhs.z);
        public static Vector3Bool operator *(Vector3Bool lhs, Vector3Bool rhs) => new(lhs.x && rhs.x, lhs.y && rhs.y, lhs.z && rhs.z);

    }
}
