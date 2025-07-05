using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LunaLib {
    public interface ISerializableCondition<T> {
        public bool Evaluate(T value);
    }

    public abstract class SerializableCondition<T> : ISerializableCondition<T> {
        public bool inverted = false;
        public bool Evaluate(T value) => inverted ? !EvaluateValue(value) : EvaluateValue(value);
        protected abstract bool EvaluateValue(T value);
    }
    public abstract class CustomSerializableCondition<Tvalue, Ttype> : SerializableCondition<Tvalue> where Ttype : Enum {
        public Ttype type;
    }

    public class EquatableCondition<T> : SerializableCondition<T> where T : IEquatable<T> {
        public T requiredValue;
        protected override bool EvaluateValue(T value) => value.Equals(requiredValue);
    }
    public abstract class CustomEquatableCondition<Tvalue, Ttype> : CustomSerializableCondition<Tvalue, Ttype> where Ttype : Enum {
        public abstract bool IsEquals { get; }
        [ShowIf("IsEquals")] public Tvalue requiredValue;
        protected override bool EvaluateValue(Tvalue value) => value.Equals(requiredValue);
    }

    public class EnumCondition<T> : CustomEquatableCondition<T, EnumCondition<T>.Type> where T : Enum {
        public enum Type {
            Equals,
            HasFlag
        }
        public override bool IsEquals => type == Type.Equals;
        public bool IsHasFlag => type == Type.HasFlag;
        [ShowIf("IsHasFlag")] public T requiredFlags;
        [ShowIf("IsHasFlag")] public T prohibitedFlags;
        protected override bool EvaluateValue(T value) => type switch {
            Type.Equals => base.EvaluateValue(value),
            Type.HasFlag => (requiredFlags.Equals(0) || value.HasFlag(requiredFlags)) && (prohibitedFlags.Equals(0) || !value.HasFlag(prohibitedFlags)),
            _ => false,
        };
    }

    public class ComparableCondition<T> : CustomEquatableCondition<T, ComparableCondition<T>.Type> where T : IComparable<T>, IEquatable<T> {
        public T comparisonValue;
        [Flags]
        public enum Type {
            NotEqual = 0,
            EqualTo = 1,
            GreaterThan = 2,
            GreaterThanOrEqualTo = 3,
            LessThan = 4,
            LessThanOrEqualTo = 5
        }
        public override bool IsEquals => type == Type.EqualTo;
        protected override bool EvaluateValue(T value) => type switch {
            Type.NotEqual => value.CompareTo(comparisonValue) != 0,
            Type.EqualTo => value.CompareTo(comparisonValue) == 0,
            Type.GreaterThan => value.CompareTo(comparisonValue) > 0,
            Type.GreaterThanOrEqualTo => value.CompareTo(comparisonValue) >= 0,
            Type.LessThan => value.CompareTo(comparisonValue) < 0,
            Type.LessThanOrEqualTo => value.CompareTo(comparisonValue) <= 0,
            _ => false,
        };
    }
    public abstract class ComparableCondition<Tvalue, Ttype> : CustomEquatableCondition<Tvalue, Ttype> where Ttype : Enum where Tvalue : IComparable<Tvalue>, IEquatable<Tvalue> {
        public abstract bool IsComparison { get; }
        [Flags]
        public enum ComparisonType {
            NotEqual = 0,
            EqualTo = 1,
            GreaterThan = 2,
            GreaterThanOrEqualTo = 3,
            LessThan = 4,
            LessThanOrEqualTo = 5
        }
        [ShowIf("IsComparison")] public ComparisonType comparisonType;
        [ShowIf("IsComparison")] public Tvalue comparisonValue;
        public override bool IsEquals => IsComparison && comparisonType == ComparisonType.EqualTo;
        protected override bool EvaluateValue(Tvalue value) => comparisonType switch {
            ComparisonType.NotEqual => value.CompareTo(comparisonValue) != 0,
            ComparisonType.EqualTo => value.CompareTo(comparisonValue) == 0,
            ComparisonType.GreaterThan => value.CompareTo(comparisonValue) > 0,
            ComparisonType.GreaterThanOrEqualTo => value.CompareTo(comparisonValue) >= 0,
            ComparisonType.LessThan => value.CompareTo(comparisonValue) < 0,
            ComparisonType.LessThanOrEqualTo => value.CompareTo(comparisonValue) <= 0,
            _ => false,
        };
    }

    public class CollectionCondition<C, T> : SerializableCondition<C> where C : ICollection<T> {
        public enum Type {
            CountCondition,
            Contains
        }
        public Type type;
        public bool IsCountCondition => type == Type.CountCondition;
        public bool IsContains => type == Type.Contains;
        [ShowIf("IsCountCondition"), SerializeReference] public ISerializableCondition<int> countCondition;
        [ShowIf("IsContains")] public T requiredElement;
        protected override bool EvaluateValue(C value) => type switch {
            Type.CountCondition => countCondition.Evaluate(value.Count),
            Type.Contains => value.Contains(requiredElement),
            _ => false,
        };
    }
    public abstract class CustomCollectionCondition<C, Tvalue, Ttype> : CustomSerializableCondition<C, Ttype> where Ttype : Enum where C : ICollection<Tvalue> {
        public abstract bool IsCountCondition { get; }
        public abstract bool IsContains { get; }
        [ShowIf("IsCountCondition"), SerializeReference] public ISerializableCondition<int> countCondition;
        [ShowIf("IsContains")] public Tvalue requiredElement;
        protected override bool EvaluateValue(C value) {
            if (IsCountCondition) return countCondition.Evaluate(value.Count);
            if (IsContains) return value.Contains(requiredElement);
            return false;
        }
    }

    public class ListCondition<T> : CustomCollectionCondition<List<T>, T, ListCondition<T>.Type> {
        public enum Type {
            CountCondition,
            Contains,
            ElementAt,
            IndexOfCondition,
            StartsWith,
            EndsWith,
            EqualsAllElements
        }
        public override bool IsCountCondition => type == Type.CountCondition;
        public override bool IsContains => type == Type.Contains;
        public bool IsElementAt => type == Type.ElementAt;
        public bool IsIndexOfCondition => type == Type.IndexOfCondition;
        public bool IsStartsWith => type == Type.StartsWith;
        public bool IsEndsWith => type == Type.EndsWith;
        public bool IsEqualsAllElements => type == Type.EqualsAllElements;
        public bool RequiresMultipleElements => IsStartsWith || IsEndsWith || IsEqualsAllElements;
        [ShowIf("IsElementAt")] public int elementIndex;
        [ShowIf("IsElementAt")] public T requiredElementAtIndex;
        [ShowIf("IsIndexOfCondition")] public T indexOfElement;
        [ShowIf("IsIndexOfCondition"), SerializeReference] public ISerializableCondition<int> indexOfCondition;
        [ShowIf("RequiresMultipleElements")] public List<T> requiredElements;
        protected override bool EvaluateValue(List<T> value) => type switch {
            Type.CountCondition => base.EvaluateValue(value),
            Type.Contains => base.EvaluateValue(value),
            Type.ElementAt => value[elementIndex].Equals(requiredElementAtIndex),
            Type.IndexOfCondition => indexOfCondition.Evaluate(value.IndexOf(indexOfElement)),
            Type.StartsWith => value.GetRange(0, requiredElements.Count).SequenceEqual(requiredElements),
            Type.EndsWith => value.GetRange(value.Count - requiredElements.Count, requiredElements.Count).SequenceEqual(requiredElements),
            Type.EqualsAllElements => value.SequenceEqual(requiredElements),
            _ => false,
        };
    }
    public abstract class CustomListCondition<Tvalue, Ttype> : CustomCollectionCondition<List<Tvalue>, Tvalue, Ttype> where Ttype : Enum {
        public abstract bool IsElementAt { get; }
        public abstract bool IsIndexOfCondition { get; }
        public abstract bool IsStartsWith { get; }
        public abstract bool IsEndsWith { get; }
        public abstract bool IsEqualsAllElements { get; }
        public bool RequiresMultipleElements => IsStartsWith || IsEndsWith || IsEqualsAllElements;
        [ShowIf("IsElementAt")] public int elementIndex;
        [ShowIf("IsElementAt")] public Tvalue requiredElementAtIndex;
        [ShowIf("IsIndexOfCondition")] public Tvalue indexOfElement;
        [ShowIf("IsIndexOfCondition"), SerializeReference] public ISerializableCondition<int> indexOfCondition;
        [ShowIf("RequiresMultipleElements")] public List<Tvalue> requiredElements;
        protected override bool EvaluateValue(List<Tvalue> value) {
            if (IsCountCondition || IsContains) return base.EvaluateValue(value);
            if (IsElementAt) return value[elementIndex].Equals(requiredElementAtIndex);
            if (IsIndexOfCondition) return indexOfCondition.Evaluate(value.IndexOf(indexOfElement));
            if (IsStartsWith) return requiredElements.Count == 0 || value.GetRange(0, requiredElements.Count).SequenceEqual(requiredElements);
            if (IsEndsWith) return requiredElements.Count == 0 || value.StartingAt(value.Count - requiredElements.Count).SequenceEqual(requiredElements);
            if (IsEqualsAllElements) return requiredElements.Count == 0 || value.SequenceEqual(requiredElements);
            return false;
        }
    }


    [Serializable]
    public class BoolCondition : ISerializableCondition<bool> {
        public bool requiredValue;
        public bool Evaluate(bool value) => value == requiredValue;
    }
    [Serializable] public class IntCondition : ComparableCondition<int> { }
    [Serializable] public class FloatCondition : ComparableCondition<float> { }
    [Serializable] public class CharCondition : EquatableCondition<char> { }
    [Serializable] public class StringCondition : CustomSerializableCondition<string, StringCondition.Type> {
        public enum Type {
            LengthCondition,
            Equals,
            Contains,
            StartsWith,
            EndsWith
        }
        public bool IsLengthCondition => type == Type.LengthCondition;
        public bool IsEquals => type == Type.Equals;
        public bool IsContains => type == Type.Contains;
        public bool IsStartsWith => type == Type.StartsWith;
        public bool IsEndsWith => type == Type.EndsWith;
        public bool RequiresString => IsContains || IsEndsWith || (IsEquals && !requiresNull);
        [ShowIf("IsLengthCondition"), SerializeReference] public ISerializableCondition<int> lengthCondition;
        [ShowIf("IsEquals")] public bool requiresNull = false;
        [ShowIf("RequiresString")] public string requiredString;
        protected override bool EvaluateValue(string value) => type switch {
            Type.LengthCondition => lengthCondition.Evaluate(value.Length),
            Type.Equals => requiresNull? value.Equals(null) : value.Equals(requiredString),
            Type.Contains => value.Contains(requiredString),
            Type.StartsWith => value.StartsWith(requiredString),
            Type.EndsWith => value.EndsWith(requiredString),
            _ => false,
        };
    }

}