using UnityEngine;

namespace LunaLib {
    public static partial class QuaternionExtension {

        public static Vector3 Forward(this Quaternion q) => q * Vector3.forward;
        public static Vector3 Right(this Quaternion q) => q * Vector3.right;
        public static Vector3 Up(this Quaternion q) => q * Vector3.up;

        public static float AngleTo(this Quaternion q, Quaternion other) {
            return Quaternion.Angle(q, other);
        }

        public static Quaternion ScaledBy(this Quaternion q, float factor) {
            return Quaternion.LerpUnclamped(Quaternion.identity, q, factor);
        }

        public static Quaternion Less(this Quaternion q1, Quaternion q2) {
            return QuaternionUtils.Difference(q1, q2);
        }

        public static Quaternion Inversed(this Quaternion q) {
            return Quaternion.Inverse(q);
        }

        public static Quaternion LerpedTo(this Quaternion q, Quaternion other, float f) {
            return Quaternion.Lerp(q, other, f);
        }

    }

}
