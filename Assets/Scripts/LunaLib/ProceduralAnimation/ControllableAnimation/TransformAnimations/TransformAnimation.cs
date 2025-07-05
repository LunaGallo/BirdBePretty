using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LunaLib {
    public class TransformAnimation : ControllableAnimation {

        [Serializable]
        public class Animation {

            public TransformProperty controlledProperties = TransformProperty.Position;

            public bool IsPos => controlledProperties.HasFlag(TransformProperty.Position);
            [ShowIf("IsPos")] public Axis3Flags controlledPositionAxis = Axis3Flags.None;
            public bool IsPosX => IsPos && controlledPositionAxis.HasFlag(Axis3Flags.X);
            [ShowIf("IsPosX")] public RemapedCurve positionXCurve;
            public bool IsPosY => IsPos && controlledPositionAxis.HasFlag(Axis3Flags.Y);
            [ShowIf("IsPosY")] public RemapedCurve positionYCurve;
            public bool IsPosZ => IsPos && controlledPositionAxis.HasFlag(Axis3Flags.Z);
            [ShowIf("IsPosZ")] public RemapedCurve positionZCurve;

            public bool IsRot => controlledProperties.HasFlag(TransformProperty.Rotation);
            [ShowIf("IsRot")] public Axis3Flags controlledRotationAxis = Axis3Flags.None;
            public bool IsRotX => IsRot && controlledRotationAxis.HasFlag(Axis3Flags.X);
            [ShowIf("IsRotX")] public RemapedCurve rotationXCurve;
            public bool IsRotY => IsRot && controlledRotationAxis.HasFlag(Axis3Flags.Y);
            [ShowIf("IsRotY")] public RemapedCurve rotationYCurve;
            public bool IsRotZ => IsRot && controlledRotationAxis.HasFlag(Axis3Flags.Z);
            [ShowIf("IsRotZ")] public RemapedCurve rotationZCurve;

            public bool IsScl => controlledProperties.HasFlag(TransformProperty.Scale);
            [ShowIf("IsScl")] public Axis3Flags controlledScaleAxis = Axis3Flags.None;
            public bool IsSclX => IsScl && controlledScaleAxis.HasFlag(Axis3Flags.X);
            [ShowIf("IsSclX")] public RemapedCurve scaleXCurve;
            public bool IsSclY => IsScl && controlledScaleAxis.HasFlag(Axis3Flags.Y);
            [ShowIf("IsSclY")] public RemapedCurve scaleYCurve;
            public bool IsSclZ => IsScl && controlledScaleAxis.HasFlag(Axis3Flags.Z);
            [ShowIf("IsSclZ")] public RemapedCurve scaleZCurve;

            public float Duration {
                get {
                    float result = 0f;
                    if (IsPosX) result = Mathf.Max(result, positionXCurve.Duration);
                    if (IsPosY) result = Mathf.Max(result, positionYCurve.Duration);
                    if (IsPosZ) result = Mathf.Max(result, positionZCurve.Duration);

                    if (IsRotX) result = Mathf.Max(result, rotationXCurve.Duration);
                    if (IsRotY) result = Mathf.Max(result, rotationYCurve.Duration);
                    if (IsRotZ) result = Mathf.Max(result, rotationZCurve.Duration);

                    if (IsSclX) result = Mathf.Max(result, scaleXCurve.Duration);
                    if (IsSclY) result = Mathf.Max(result, scaleYCurve.Duration);
                    if (IsSclZ) result = Mathf.Max(result, scaleZCurve.Duration);
                    return result;
                }
            }

            public Vector3 AlterPositionAt(Vector3 original, float time) {
                time /= Duration;
                if (IsPosX) original.x = positionXCurve.Evaluate(time);
                if (IsPosY) original.y = positionYCurve.Evaluate(time);
                if (IsPosZ) original.z = positionZCurve.Evaluate(time);
                return original;
            }
            public Vector3 AlterRotationAt(Vector3 original, float time) {
                time /= Duration;
                if (IsRotX) original.x = rotationXCurve.Evaluate(time);
                if (IsRotY) original.y = rotationYCurve.Evaluate(time);
                if (IsRotZ) original.z = rotationZCurve.Evaluate(time);
                return original;
            }
            public Vector3 AlterScaleAt(Vector3 original, float time) {
                time /= Duration;
                if (IsSclX) original.x = scaleXCurve.Evaluate(time);
                if (IsSclY) original.y = scaleYCurve.Evaluate(time);
                if (IsSclZ) original.z = scaleZCurve.Evaluate(time);
                return original;
            }

            public static Animation TranslationLinear(Vector3 delta, float duration) => new() {
                controlledProperties = TransformProperty.Position,
                controlledPositionAxis = Axis3Flags.X | Axis3Flags.Y | Axis3Flags.Z,
                positionXCurve = new(new(0f, 0f, duration, delta.x), AnimationCurve.Linear(0f, 0f, 1f, 1f)),
                positionYCurve = new(new(0f, 0f, duration, delta.y), AnimationCurve.Linear(0f, 0f, 1f, 1f)),
                positionZCurve = new(new(0f, 0f, duration, delta.z), AnimationCurve.Linear(0f, 0f, 1f, 1f))
            };
            public static Animation Empty() => new() { controlledProperties = TransformProperty.None };
            public bool IsEmpty() => Duration <= 0f;

        }
        public Animation currentAnimation;

        public float ProgressAsValue(float percentage) => percentage * currentAnimation.Duration;
        public override void UpdateProgress(float animationProgress) {
            if (!currentAnimation.IsEmpty()) {
                transform.localPosition = currentAnimation.AlterPositionAt(transform.localPosition, ProgressAsValue(animationProgress));
                transform.localRotation = Quaternion.Euler(currentAnimation.AlterRotationAt(transform.localRotation.eulerAngles, ProgressAsValue(animationProgress)));
                transform.localScale = currentAnimation.AlterScaleAt(transform.localScale, ProgressAsValue(animationProgress));
            }
        }

    }
}