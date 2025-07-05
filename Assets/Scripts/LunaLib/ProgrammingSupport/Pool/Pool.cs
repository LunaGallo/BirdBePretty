using System;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public class Pool<T> : IPool<T> {
        public Func<T> create;
        public Action<T> destroy;
        public Action<T> activate;
        public Action<T> deactivate;
        public Pool(Func<T> create, Action<T> destroy = null, Action<T> activate = null, Action<T> deactivate = null, int initialSize = 0) {
            this.create = create;
            this.destroy = destroy;
            this.activate = activate;
            this.deactivate = deactivate;
            for (int i = 0; i < initialSize; i++) {
                NewInactive();
            }
        }

        public List<T> ActiveElements => new(activeElements);
        private readonly List<T> activeElements = new();
        private readonly List<T> inactiveElements = new();

        public T GetInstance() {
            EnsureInstance();
            T chosenElement = inactiveElements.First();
            inactiveElements.RemoveFirst();
            activeElements.Add(chosenElement);
            activate?.Invoke(chosenElement);
            return chosenElement;
        }
        public void ReturnInstance(T element) {
            if (activeElements.Contains(element)) {
                activeElements.Remove(element);
                inactiveElements.Add(element);
                deactivate?.Invoke(element);
            }
        }
        public void ReturnAll() {
            activeElements.ForEach(e => {
                inactiveElements.Add(e);
                deactivate?.Invoke(e);
            });
            activeElements.Clear();
        }
        public void Clear() {
            if (destroy != null) {
                inactiveElements.ForEach(destroy);
                activeElements.ForEach(destroy);
            }
            inactiveElements.Clear();
            activeElements.Clear();
        }
        public void SetActiveCount(int newCount) {
            if (activeElements.Count > newCount) {
                for (int i = activeElements.Count - 1; i >= newCount; i--) {
                    ReturnInstance(activeElements[i]);
                }
            }
            else if (activeElements.Count < newCount) {
                for (int i = activeElements.Count; i < newCount; i++) {
                    GetInstance();
                }
            }
        }

        private void EnsureInstance() {
            if (inactiveElements.Count == 0) {
                NewInactive();
            }
        }
        private void NewInactive() {
            T newElement = create.Invoke();
            inactiveElements.Add(newElement);
            deactivate?.Invoke(newElement);
        }

    }
}
