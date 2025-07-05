using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class UnityEventExtension {

    public static void EnsureListener(this UnityEvent unityEvent, UnityAction call) {
        unityEvent.RemoveListener(call);
        unityEvent.AddListener(call);
    }
    public static void EnsureListener<T>(this UnityEvent<T> unityEvent, UnityAction<T> call) {
        unityEvent.RemoveListener(call);
        unityEvent.AddListener(call);
    }
    public static void EnsureListener<T0, T1>(this UnityEvent<T0, T1> unityEvent, UnityAction<T0, T1> call) {
        unityEvent.RemoveListener(call);
        unityEvent.AddListener(call);
    }

}
