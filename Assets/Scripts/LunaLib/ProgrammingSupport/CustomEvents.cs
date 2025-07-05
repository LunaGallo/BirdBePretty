using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LunaLib {

    #region DoubleTypedEvents
    [Serializable] public class DoubleBooleanEvent : UnityEvent<bool, bool> { }
    [Serializable] public class IntegerBooleanEvent : UnityEvent<int, bool> { }
    #endregion

    #region LabeledEvent
    [Serializable]
    public class LabeledEvent<L> {
        public L label;
        public UnityEvent unityEvent;

        public void TryInvoke(L matcingLabel) {
            if (label.Equals(matcingLabel)) {
                unityEvent.Invoke();
            }
        }
    }
    [Serializable]
    public class LabeledEvent<L, E, T> where E : UnityEvent<T> {
        public L label;
        public E unityEvent;

        public void TryInvoke(L matcingLabel, T value) {
            if (label.Equals(matcingLabel)) {
                unityEvent.Invoke(value);
            }
        }
    }

    #region IdentifierEvents
    [Serializable] public class IdentifierEvent : LabeledEvent<string> { }
    [Serializable] public class BooleanIdentifierEvent : LabeledEvent<string, BooleanEvent, bool> { }
    [Serializable] public class IntegerIdentifierEvent : LabeledEvent<string, IntegerEvent, int> { }
    [Serializable] public class FloatIdentifierEvent : LabeledEvent<string, FloatEvent, float> { }
    [Serializable] public class CharIdentifierEvent : LabeledEvent<string, CharEvent, char> { }
    [Serializable] public class StringIdentifierEvent : LabeledEvent<string, StringEvent, string> { }
    #endregion

    #region MessageEvents
    [Serializable] public class MessageEvent : LabeledEvent<MessageType> { }
    [Serializable] public class BooleanMessageEvent : LabeledEvent<MessageType, BooleanEvent, bool> { }
    [Serializable] public class IntegerMessageEvent : LabeledEvent<MessageType, IntegerEvent, int> { }
    [Serializable] public class FloatMessageEvent : LabeledEvent<MessageType, FloatEvent, float> { }
    [Serializable] public class CharMessageEvent : LabeledEvent<MessageType, CharEvent, char> { }
    [Serializable] public class StringMessageEvent : LabeledEvent<MessageType, StringEvent, string> { }
    #endregion
    #endregion

    #region ModifiableEvents
    [Serializable] 
    public class ModifiableBooleanEvent {
        public bool invert = false;
        public BooleanEvent normalEvent;
        public UnityEvent onTrue;
        public UnityEvent onFalse;

        public void Invoke(bool value) {
            if (invert) {
                value = !value;
            }
            normalEvent.Invoke(value);
            if (value) {
                onTrue.Invoke();
            }
            if (!value) {
                onFalse.Invoke();
            }
        }
    }

    [Serializable]
    public class ModifiableIntegerEvent {
        public IntegerEvent normalEvent;
        public List<UnityEvent> indexedEvents;

        public void Invoke(int value) {
            normalEvent.Invoke(value);
            if (indexedEvents.IsValidIndex(value)) {
                indexedEvents[value].Invoke();
            }
        }
    }
    #endregion

}