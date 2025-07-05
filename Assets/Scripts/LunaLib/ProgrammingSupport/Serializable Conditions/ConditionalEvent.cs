using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LunaLib {
    [Serializable]
    public class ConditionalEvent<C, E, T> where E : UnityEvent<T> where C : ISerializableCondition<T> {
        public C condition;
        public E unityEvent;

        public void Invoke(T value) {
            if (condition.Evaluate(value)) {
                unityEvent.Invoke(value);
            }
        }
    }

    [Serializable] public class BoolConditionalEvent : ConditionalEvent<BoolCondition, BooleanEvent, bool> { }
    [Serializable] public class IntConditionalEvent : ConditionalEvent<IntCondition, IntegerEvent, int> { }
    [Serializable] public class FloatConditionalEvent : ConditionalEvent<FloatCondition, FloatEvent, float> { }
    [Serializable] public class CharConditionalEvent : ConditionalEvent<CharCondition, CharEvent, char> { }
    [Serializable] public class StringConditionalEvent : ConditionalEvent<StringCondition, StringEvent, string> { }

}