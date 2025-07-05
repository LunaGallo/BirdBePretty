using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LunaLib {

    [Serializable]
    public class SerializableComponentPool<T> : IPool<T> where T : Component {
        public T prefab;
        public Transform container;
        public int initialSize;
        public bool useAnchor;
        [ShowIf("useAnchor")] public Vector3 anchor;

        private ComponentPool<T> pool;
        public ComponentPool<T> ActualPool {
            get {
                if (pool == null) {
                    if (useAnchor) {
                        pool = new ComponentPool<T>(prefab, container, anchor, initialSize);
                    }
                    else {
                        pool = new ComponentPool<T>(prefab, container, initialSize);
                    }
                }
                return pool;
            }
        }
        public static implicit operator ComponentPool<T>(SerializableComponentPool<T> s) => s.ActualPool;

        public List<T> ActiveElements => ActualPool.ActiveElements;
        public void Clear() => ActualPool.Clear();
        public T GetInstance() => ActualPool.GetInstance();
        public void ReturnAll() => ActualPool.ReturnAll();
        public void ReturnInstance(T element) => ActualPool.ReturnInstance(element);
        public void SetActiveCount(int newCount) => ActualPool.SetActiveCount(newCount);

        public T GetInstanceAt(Vector3 position) {
            T result = GetInstance();
            result.transform.position = position;
            return result;
        }

    }
}