using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    [Serializable]
    public struct Vector2Bool : IEquatable<Vector2Bool> {

        public bool x, y;

        public Vector2Bool(bool x, bool y) { this.x = x; this.y = y; }

        public bool AndParts() => x && y;
        public bool OrParts() => x || y;

        public static Vector2Bool False => new(false, false);
        public static Vector2Bool True => new(true, true);

        public bool Equals(Vector2Bool other) => x == other.x && y == other.y;
        public override bool Equals(object obj) => obj is Vector2Bool other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(x, y);
        public override string ToString() => x + ", " + y;

        public static bool operator ==(Vector2Bool lhs, Vector2Bool rhs) => (lhs.x == rhs.x) && (lhs.y == rhs.y);
        public static bool operator !=(Vector2Bool lhs, Vector2Bool rhs) => (lhs.x != rhs.x) || (lhs.y != rhs.y);
        public static Vector2Bool operator !(Vector2Bool v) => new(!v.x, !v.y);
        public static Vector2Bool operator +(Vector2Bool lhs, Vector2Bool rhs) => new(lhs.x || rhs.x, lhs.y || rhs.y);
        public static Vector2Bool operator *(Vector2Bool lhs, Vector2Bool rhs) => new(lhs.x && rhs.x, lhs.y && rhs.y);

    }
}
