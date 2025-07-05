using System;
using System.Collections.Generic;

namespace LunaLib {
    public class DelayedReturnGroup : IDelayedReturn {
        public List<IDelayedReturn> delayedReturns;
        public Action OnReturn { get; set; }
        public List<int> ReturnIndices => new List<int>(returnIndices);

        private bool active = false;
        private readonly List<int> returnIndices = new List<int>();

        public DelayedReturnGroup(params IDelayedReturn[] delayedReturns) {
            this.delayedReturns = new List<IDelayedReturn>(delayedReturns);
        }

        public IDelayedReturn this[int index] {
            get => delayedReturns[index];
            set => delayedReturns[index] = value;
        }

        public void StartAction() {
            if (!active) {
                active = true;
                returnIndices.Clear();
                for (int i = 0; i < delayedReturns.Count; i++) {
                    delayedReturns[i].OnReturn = () => Returned(i);
                    delayedReturns[i].StartAction();
                }
            }
        }
        public void InterruptAction() {
            if (active) {
                for (int i = 0; i < delayedReturns.Count; i++) {
                    if (!returnIndices.Contains(i)) {
                        delayedReturns[i].InterruptAction();
                    }
                }
                active = false;
                returnIndices.Clear();
            }
        }
        public void SkipAction() {
            for (int i = 0; i < delayedReturns.Count; i++) {
                if (!returnIndices.Contains(i)) {
                    delayedReturns[i].SkipAction();
                }
            }
            active = false;
            returnIndices.Clear();
        }
        public void PauseAction() {
            if (active) {
                for (int i = 0; i < delayedReturns.Count; i++) {
                    if (!returnIndices.Contains(i)) {
                        delayedReturns[i].PauseAction();
                    }
                }
            }
        }
        public void ResumeAction() {
            if (active) {
                for (int i = 0; i < delayedReturns.Count; i++) {
                    if (!returnIndices.Contains(i)) {
                        delayedReturns[i].ResumeAction();
                    }
                }
            }
        }
        private void Returned(int index) {
            if (active) {
                returnIndices.Add(index);
                if (returnIndices.Count == delayedReturns.Count) {
                    active = false;
                    OnReturn?.Invoke();
                }
            }
        }
    }
}
