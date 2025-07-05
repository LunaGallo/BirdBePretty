using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


namespace LunaLib {
    [Serializable]
    public class SerializableConfigurableComponentPool<T, C> : IPool<T> where T : Configurator<C> {
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
        public T GetInstanceConfigured(C configuration) {
            T result = GetInstance();
            result.Configure(configuration);
            return result;
        }
        public T GetInstanceAtConfigured(Vector3 position, C configuration) {
            T result = GetInstance();
            result.transform.position = position;
            result.Configure(configuration);
            return result;
        }

    }
}