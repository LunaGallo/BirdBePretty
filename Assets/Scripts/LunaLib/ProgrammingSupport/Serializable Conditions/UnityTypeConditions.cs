using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LunaLib {

    [Serializable]
    public class LayerCondition : CustomSerializableCondition<int, LayerCondition.Type> {
        public enum Type {
            Equals,
            InMask
        }
        public bool IsEquals => type == Type.Equals;
        public bool IsInMask => type == Type.InMask;
        [ShowIf("IsEquals"), ValueDropdown("EDITOR_GetNamedLayers")] public int requiredValue;
        [ShowIf("IsInMask")] public LayerMask requiredFlags;
        protected override bool EvaluateValue(int value) => type switch {
            Type.Equals => value == requiredValue,
            Type.InMask => IsLayerInMask(value, requiredFlags),
            _ => false,
        };
        public static bool IsLayerInMask(int layer, LayerMask mask) => (mask.value & (1 << layer)) != 0;
        public static IEnumerable<ValueDropdownItem<int>> EDITOR_GetNamedLayers() => Enumerable.Range(0, 32).Select(i => new ValueDropdownItem<int>(LayerMask.LayerToName(i), i)).Where(l => !string.IsNullOrEmpty(l.Text));
    }



    [Serializable]
    public class Vector2Condition : CustomEquatableCondition<Vector2, Vector2Condition.Type> {
        public enum Type {
            Equals,
            AxisCondition,
            MagnitudeCondition,
            DistanceCondition,
            AngleCondition,
            DotCondition
        }
        public override bool IsEquals => type == Type.Equals;
        public bool IsAxisCondition => type == Type.AxisCondition;
        public bool IsMagnitudeCondition => type == Type.MagnitudeCondition;
        public bool IsDistanceCondition => type == Type.DistanceCondition;
        public bool IsAngleCondition => type == Type.AngleCondition;
        public bool IsDotCondition => type == Type.DotCondition;
        public bool UsesFloatCondition => IsAxisCondition || IsMagnitudeCondition || IsDistanceCondition || IsAngleCondition || IsDotCondition;
        public bool UsesReferenceValue => IsDistanceCondition || IsAngleCondition || IsDotCondition;
        public bool CalculatesDistance => IsDistanceCondition || IsMagnitudeCondition;
        public enum Axis2D { X, Y }
        [ShowIf("IsAxisCondition")] public Axis2D axis;
        [ShowIf("UsesFloatCondition"), SerializeReference] public ISerializableCondition<float> floatCondition;
        [ShowIf("CalculatesDistance")] public bool useSqrDistance = false;
        [ShowIf("IsAngleCondition")] public bool signed = false;
        [ShowIf("UsesReferenceValue")] public Vector2 referenceValue;
        protected override bool EvaluateValue(Vector2 value) {
            return type switch {
                Type.Equals => base.EvaluateValue(value),
                Type.AxisCondition => axis switch {
                    Axis2D.X => floatCondition.Evaluate(value.x),
                    Axis2D.Y => floatCondition.Evaluate(value.y),
                    _ => false,
                },
                Type.MagnitudeCondition => floatCondition.Evaluate(useSqrDistance ? value.sqrMagnitude : value.magnitude),
                Type.DistanceCondition => floatCondition.Evaluate(useSqrDistance ? Vector2.Distance(value, referenceValue) : Vector2.SqrMagnitude(referenceValue - value)),
                Type.AngleCondition => floatCondition.Evaluate(signed ? Vector2.SignedAngle(referenceValue, value) : Vector2.Angle(referenceValue, value)),
                Type.DotCondition => floatCondition.Evaluate(Vector2.Dot(referenceValue, value)),
                _ => false,
            };
        }
    }

    [Serializable]
    public class Vector2IntCondition : CustomEquatableCondition<Vector2Int, Vector2IntCondition.Type> {
        public enum Type {
            Equals,
            AxisCondition,
            MagnitudeCondition,
            DistanceCondition
        }
        public override bool IsEquals => type == Type.Equals;
        public bool IsAxisCondition => type == Type.AxisCondition;
        public bool IsMagnitudeCondition => type == Type.MagnitudeCondition;
        public bool IsDistanceCondition => type == Type.DistanceCondition;
        public bool UsesFloatCondition => IsMagnitudeCondition || IsDistanceCondition;
        public enum Axis2D { X, Y }
        [ShowIf("IsAxisCondition")] public Axis2D axis;
        [ShowIf("IsAxisCondition"), SerializeReference] public ISerializableCondition<int> intCondition;
        [ShowIf("UsesFloatCondition"), SerializeReference] public ISerializableCondition<float> floatCondition;
        [ShowIf("UsesFloatCondition")] public bool useSqrDistance = false;
        [ShowIf("IsDistanceCondition")] public Vector2Int referenceValue;
        protected override bool EvaluateValue(Vector2Int value) {
            return type switch {
                Type.Equals => base.EvaluateValue(value),
                Type.AxisCondition => axis switch {
                    Axis2D.X => intCondition.Evaluate(value.x),
                    Axis2D.Y => intCondition.Evaluate(value.y),
                    _ => false,
                },
                Type.MagnitudeCondition => floatCondition.Evaluate(useSqrDistance ? value.sqrMagnitude : value.magnitude),
                Type.DistanceCondition => floatCondition.Evaluate(useSqrDistance ? Vector2Int.Distance(value, referenceValue) : Vector2.SqrMagnitude(referenceValue - value)),
                _ => false,
            };
        }
    }
    
    [Serializable]
    public class Vector3Condition : CustomEquatableCondition<Vector3, Vector3Condition.Type> {
        public enum Type {
            Equals,
            AxisCondition,
            MagnitudeCondition,
            DistanceCondition,
            AngleCondition,
            DotCondition
        }
        public override bool IsEquals => type == Type.Equals;
        public bool IsAxisCondition => type == Type.AxisCondition;
        public bool IsMagnitudeCondition => type == Type.MagnitudeCondition;
        public bool IsDistanceCondition => type == Type.DistanceCondition;
        public bool IsAngleCondition => type == Type.AngleCondition;
        public bool IsDotCondition => type == Type.DotCondition;
        public bool UsesFloatCondition => IsAxisCondition || IsMagnitudeCondition;
        public bool UsesReferenceValue => IsDistanceCondition || IsAngleCondition || IsDotCondition;
        public bool CalculatesDistance => IsDistanceCondition || IsMagnitudeCondition;
        public enum Axis3D { X, Y, Z }
        [ShowIf("IsAxisCondition")] public Axis3D axis;
        [ShowIf("UsesFloatCondition"), SerializeReference] public ISerializableCondition<float> floatCondition;
        [ShowIf("CalculatesDistance")] public bool useSqrDistance = false;
        [ShowIf("UsesReferenceValue")] public Vector3 referenceValue;
        protected override bool EvaluateValue(Vector3 value) {
            return type switch {
                Type.Equals => base.EvaluateValue(value),
                Type.AxisCondition => axis switch {
                    Axis3D.X => floatCondition.Evaluate(value.x),
                    Axis3D.Y => floatCondition.Evaluate(value.y),
                    Axis3D.Z => floatCondition.Evaluate(value.z),
                    _ => false,
                },
                Type.MagnitudeCondition => floatCondition.Evaluate(useSqrDistance ? value.sqrMagnitude : value.magnitude),
                Type.DistanceCondition => floatCondition.Evaluate(useSqrDistance ? Vector3.Distance(value, referenceValue) : Vector3.SqrMagnitude(referenceValue - value)),
                Type.AngleCondition => floatCondition.Evaluate(Vector3.Angle(referenceValue, value)),
                Type.DotCondition => floatCondition.Evaluate(Vector3.Dot(referenceValue, value)),
                _ => false,
            };
        }
    }
    
    [Serializable]
    public class Vector3IntCondition : CustomEquatableCondition<Vector3Int, Vector3IntCondition.Type> {
        public enum Type {
            Equals,
            AxisCondition,
            MagnitudeCondition,
            DistanceCondition
        }
        public override bool IsEquals => type == Type.Equals;
        public bool IsAxisCondition => type == Type.AxisCondition;
        public bool IsMagnitudeCondition => type == Type.MagnitudeCondition;
        public bool IsDistanceCondition => type == Type.DistanceCondition;
        public bool CalculatesDistance => IsDistanceCondition || IsMagnitudeCondition;
        public enum Axis3D { X, Y, Z }
        [ShowIf("IsAxisCondition")] public Axis3D axis;
        [ShowIf("IsAxisCondition"), SerializeReference] public ISerializableCondition<int> intCondition;
        [ShowIf("IsMagnitudeCondition"), SerializeReference] public ISerializableCondition<float> floatCondition;
        [ShowIf("CalculatesDistance")] public bool useSqrDistance = false;
        [ShowIf("IsDistanceCondition")] public Vector3Int referenceValue;
        protected override bool EvaluateValue(Vector3Int value) {
            return type switch {
                Type.Equals => base.EvaluateValue(value),
                Type.AxisCondition => axis switch {
                    Axis3D.X => intCondition.Evaluate(value.x),
                    Axis3D.Y => intCondition.Evaluate(value.y),
                    Axis3D.Z => intCondition.Evaluate(value.z),
                    _ => false,
                },
                Type.MagnitudeCondition => floatCondition.Evaluate(useSqrDistance ? value.sqrMagnitude : value.magnitude),
                Type.DistanceCondition => floatCondition.Evaluate(useSqrDistance ? Vector3Int.Distance(value, referenceValue) : Vector3.SqrMagnitude(referenceValue - value)),
                _ => false,
            };
        }
    }

    [Serializable]
    public class Vector4Condition : CustomEquatableCondition<Vector4, Vector4Condition.Type> {
        public enum Type {
            Equals,
            AxisCondition
        }
        public override bool IsEquals => type == Type.Equals;
        public bool IsAxisCondition => type == Type.AxisCondition;
        public enum Axis4D { X, Y, Z, W }
        [ShowIf("IsAxisCondition")] public Axis4D axis;
        [ShowIf("IsAxisCondition"), SerializeReference] public ISerializableCondition<float> floatCondition;
        protected override bool EvaluateValue(Vector4 value) {
            return type switch {
                Type.Equals => base.EvaluateValue(value),
                Type.AxisCondition => axis switch {
                    Axis4D.X => floatCondition.Evaluate(value.x),
                    Axis4D.Y => floatCondition.Evaluate(value.y),
                    Axis4D.Z => floatCondition.Evaluate(value.z),
                    Axis4D.W => floatCondition.Evaluate(value.w),
                    _ => false,
                },
                _ => false,
            };
        }
    }


    [Serializable]
    public class RectCondition : CustomEquatableCondition<Rect, RectCondition.Type> {
        public enum Type {
            Equals,
            MinPosCondition,
            MaxPosCondition,
            CenterCondition,
            SizeCondition,
            Contains,
            Overlaps
        }
        public override bool IsEquals => type == Type.Equals;
        public bool IsMinPosCondition => type == Type.MinPosCondition;
        public bool IsMaxPosCondition => type == Type.MaxPosCondition;
        public bool IsCenterCondition => type == Type.CenterCondition;
        public bool IsSizeCondition => type == Type.SizeCondition;
        public bool IsContains => type == Type.Contains;
        public bool IsOverlaps => type == Type.Overlaps;
        public bool UsesVector2Condition => IsMinPosCondition || IsMaxPosCondition || IsCenterCondition || IsSizeCondition;
        [ShowIf("UsesVector2Condition"), SerializeReference] public ISerializableCondition<Vector2> vector2Condition;
        [ShowIf("IsContains")] public Vector2 point;
        [ShowIf("IsOverlaps")] public Rect referenceValue;
        protected override bool EvaluateValue(Rect value) {
            return type switch {
                Type.Equals => base.EvaluateValue(value),
                Type.MinPosCondition => vector2Condition.Evaluate(value.min),
                Type.MaxPosCondition => vector2Condition.Evaluate(value.max),
                Type.CenterCondition => vector2Condition.Evaluate(value.center),
                Type.SizeCondition => vector2Condition.Evaluate(value.size),
                Type.Contains => value.Contains(point),
                Type.Overlaps => value.Overlaps(referenceValue),
                _ => false,
            };
        }
    }

    [Serializable]
    public class RectIntCondition : CustomEquatableCondition<RectInt, RectIntCondition.Type> {
        public enum Type {
            Equals,
            MinPosCondition,
            MaxPosCondition,
            CenterCondition,
            SizeCondition,
            Contains,
            Overlaps
        }
        public override bool IsEquals => type == Type.Equals;
        public bool IsMinPosCondition => type == Type.MinPosCondition;
        public bool IsMaxPosCondition => type == Type.MaxPosCondition;
        public bool IsCenterCondition => type == Type.CenterCondition;
        public bool IsSizeCondition => type == Type.SizeCondition;
        public bool IsContains => type == Type.Contains;
        public bool IsOverlaps => type == Type.Overlaps;
        public bool UsesVector2IntCondition => IsMinPosCondition || IsMaxPosCondition || IsSizeCondition;
        [ShowIf("IsCenterCondition"), SerializeReference] public ISerializableCondition<Vector2> vector2Condition;
        [ShowIf("UsesVector2IntCondition"), SerializeReference] public ISerializableCondition<Vector2Int> vector2IntCondition;
        [ShowIf("IsContains")] public Vector2Int point;
        [ShowIf("IsOverlaps")] public RectInt referenceValue;
        protected override bool EvaluateValue(RectInt value) {
            return type switch {
                Type.Equals => base.EvaluateValue(value),
                Type.MinPosCondition => vector2IntCondition.Evaluate(value.min),
                Type.MaxPosCondition => vector2IntCondition.Evaluate(value.max),
                Type.CenterCondition => vector2Condition.Evaluate(value.center),
                Type.SizeCondition => vector2IntCondition.Evaluate(value.size),
                Type.Contains => value.Contains(point),
                Type.Overlaps => value.Overlaps(referenceValue),
                _ => false,
            };
        }
    }



    [Serializable]
    public class QuaternionCondition : CustomEquatableCondition<Quaternion, QuaternionCondition.Type> {
        public enum Type {
            Equals,
            EulerAnglesCondition,
            AngleToCondition
        }
        public override bool IsEquals => type == Type.Equals;
        public bool IsEulerAnglesCondition => type == Type.EulerAnglesCondition;
        public bool IsAngleToCondition => type == Type.AngleToCondition;
        [ShowIf("IsEulerAnglesCondition"), SerializeReference] public ISerializableCondition<Vector3> vector3Condition;
        [ShowIf("IsAngleToCondition"), SerializeReference] public ISerializableCondition<float> floatCondition;
        [ShowIf("IsAngleToCondition")] public Quaternion referenceValue;
        protected override bool EvaluateValue(Quaternion value) {
            return type switch {
                Type.Equals => base.EvaluateValue(value),
                Type.EulerAnglesCondition => vector3Condition.Evaluate(value.eulerAngles),
                Type.AngleToCondition => floatCondition.Evaluate(value.AngleTo(referenceValue)),
                _ => false,
            };
        }
    }


    [Serializable]
    public class ColorCondition : CustomEquatableCondition<Color, ColorCondition.Type> {
        public enum Type {
            Equals,
            ChannelCondition
        }
        public override bool IsEquals => type == Type.Equals;
        public bool IsChannelCondition => type == Type.ChannelCondition;
        public enum ColorChannel { R, G, B, A }
        [ShowIf("IsChannelCondition")] public ColorChannel channel;
        [ShowIf("IsChannelCondition"), SerializeReference] public ISerializableCondition<float> floatCondition;
        protected override bool EvaluateValue(Color value) {
            return type switch {
                Type.Equals => base.EvaluateValue(value),
                Type.ChannelCondition => channel switch {
                    ColorChannel.R => floatCondition.Evaluate(value.r),
                    ColorChannel.G => floatCondition.Evaluate(value.g),
                    ColorChannel.B => floatCondition.Evaluate(value.b),
                    ColorChannel.A => floatCondition.Evaluate(value.a),
                    _ => false,
                },
                _ => false,
            };
        }
    }


    public class ObjectCondition<T> : CustomEquatableCondition<T, ObjectCondition<T>.Type> where T : UnityEngine.Object {
        public enum Type {
            Equals,
            NameCondition
        }
        public override bool IsEquals => type == Type.Equals;
        public bool IsNameCondition => type == Type.NameCondition;
        [ShowIf("IsNameCondition"), SerializeReference] public ISerializableCondition<string> nameCondition;
        protected override bool EvaluateValue(T value) => type switch {
            Type.Equals => base.EvaluateValue(value),
            Type.NameCondition => nameCondition.Evaluate(value.name),
            _ => false,
        };
    }
    public abstract class CustomObjectCondition<Tvalue, Ttype> : CustomEquatableCondition<Tvalue, Ttype> where Ttype : Enum where Tvalue : UnityEngine.Object {
        public abstract bool IsNameCondition { get; }
        [ShowIf("IsNameCondition"), SerializeReference] public ISerializableCondition<string> nameCondition;
        protected override bool EvaluateValue(Tvalue value) {
            if (IsEquals) base.EvaluateValue(value); 
            if (IsNameCondition) nameCondition.Evaluate(value.name); 
            return false;
        }
    }

    [Serializable]
    public class GameObjectCondition : CustomObjectCondition<GameObject, GameObjectCondition.Type> {
        public enum Type {
            Equals,
            NameCondition,
            TransformCondition,
            LayerCondition,
            Active,
            Static,
            TagCondition
        }
        public override bool IsEquals => type == Type.Equals;
        public override bool IsNameCondition => type == Type.NameCondition;
        public bool IsTransformCondition => type == Type.TransformCondition;
        public bool IsLayerCondition => type == Type.LayerCondition;
        public bool IsActive => type == Type.Active;
        public bool IsStatic => type == Type.Static;
        public bool IsTagCondition => type == Type.TagCondition;
        [ShowIf("IsTransformCondition"), SerializeReference] public ISerializableCondition<Transform> transformCondition;
        [ShowIf("IsLayerCondition"), SerializeReference] public ISerializableCondition<int> intCondition;
        public enum ActiveCheck { Self, InHierarchy }
        [ShowIf("IsActive")] public ActiveCheck activeCheck;
        [ShowIf("IsTagCondition"), SerializeReference] public ISerializableCondition<string> stringCondition;
        protected override bool EvaluateValue(GameObject value) {
            return type switch {
                Type.Equals => base.EvaluateValue(value),
                Type.TransformCondition => transformCondition.Evaluate(value.transform),
                Type.LayerCondition => intCondition.Evaluate(value.layer),
                Type.Active => activeCheck switch { 
                    ActiveCheck.Self => value.activeSelf, 
                    ActiveCheck.InHierarchy => value.activeInHierarchy, 
                    _ => false, 
                },
                Type.Static => value.isStatic,
                Type.TagCondition => stringCondition.Evaluate(value.tag),
                _ => false,
            };
        }
    }

}