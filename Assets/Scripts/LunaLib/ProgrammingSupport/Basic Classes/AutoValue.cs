using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LunaLib {
    public class AutoValue<T> {

        internal Predicate<T> invalidValue;
        internal Func<T> getValidValue;

        public AutoValue(Predicate<T> invalidValue, Func<T> getValidValue) {
            this.invalidValue = invalidValue;
            this.getValidValue = getValidValue;
        }

        internal T Value {
            get {
                if (invalidValue.Invoke(cachedValue)) {
                    SetValidValue();
                }
                return cachedValue;
            }
        }
        private T cachedValue = default;

        public static implicit operator T(AutoValue<T> getter) => getter.Value;

        public void SetValidValue() => cachedValue = getValidValue.Invoke();
    }

    public class AutoClass<T> : AutoValue<T> where T : class {
        public AutoClass(Func<T> getValidValue) : base(v => v == null, getValidValue) { }
    }

    public class AutoComponent<T> : AutoClass<T> where T : Component {
        public AutoComponent(MonoBehaviour source, AutoComponentRefMode componentSearchType = AutoComponentRefMode.Self) : base(() => componentSearchType switch {
            AutoComponentRefMode.Self => source.GetComponent<T>(),
            AutoComponentRefMode.Parent => source.GetComponentInParent<T>(),
            AutoComponentRefMode.Children => source.GetComponentInChildren<T>(),
            AutoComponentRefMode.Manual => throw new NotImplementedException(),
            _ => throw new NotImplementedException()
        }) {}
    }

    [Serializable]
    public class SerializableAutoComponent<T> where T : Component {
        public AutoComponentRefMode mode;
        public bool IsManual => mode == AutoComponentRefMode.Manual;
        public bool IsOthersComponent => mode == AutoComponentRefMode.Parent || mode == AutoComponentRefMode.Children;
        [ShowIf("IsManual")] public T reference;
        [ShowIf("IsOthersComponent")] public bool includeInactive = true;

        private MonoBehaviour source;
        private T Value {
            get {
                if (Application.isPlaying) {
                    switch (mode) {
                        case AutoComponentRefMode.Self:
                            if (reference == null) {
                                reference = source.GetComponent<T>();
                            }
                            break;
                        case AutoComponentRefMode.Parent:
                            if (reference == null) {
                                reference = source.GetComponentInParent<T>(includeInactive);
                            }
                            break;
                        case AutoComponentRefMode.Children:
                            if (reference == null) {
                                reference = source.GetComponentInChildren<T>(includeInactive);
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    return reference;
                }
                else {
                    return mode switch {
                        AutoComponentRefMode.Manual => reference,
                        AutoComponentRefMode.Self => source.GetComponent<T>(),
                        AutoComponentRefMode.Parent => source.GetComponentInParent<T>(includeInactive),
                        AutoComponentRefMode.Children => source.GetComponentInChildren<T>(includeInactive),
                        _ => throw new NotImplementedException()
                    };
                }
            }
        }
        public T GetValue(MonoBehaviour source) {
            this.source = source;
            return Value;
        }
    }

    public enum AutoComponentRefMode {
        Manual,
        Self,
        Parent,
        Children
    }
}
