using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public abstract class SerializableConditionList<T> : ISerializableCondition<T> {
        [SerializeReference] public List<ISerializableCondition<T>> conditions;
        public enum Type {
            Any,
            All
        }
        public Type type;
        public bool Evaluate(T value) => conditions.Count == 0 || type switch {
            Type.Any => conditions.Any(c => c.Evaluate(value)),
            Type.All => conditions.All(c => c.Evaluate(value)),
            _ => false,
        };
    }


    [Serializable] public class IntConditionList : SerializableConditionList<int> { }
    [Serializable] public class FloatConditionList : SerializableConditionList<float> { }
    [Serializable] public class CharConditionList : SerializableConditionList<char> { }
    [Serializable] public class StringConditionList : SerializableConditionList<string> { }

}