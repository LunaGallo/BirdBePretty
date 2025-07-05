using System;
using System.Collections.Generic;

namespace LunaLib {
    public class DelayedReturnSequence : IDelayedReturn {
        public List<IDelayedReturn> delayedReturns;
        public Action<int> onEachReturned;

        private int curIndex = 0;
        private bool active = false;

        public Action OnReturn { get; set; }

        public DelayedReturnSequence(params IDelayedReturn[] delayedReturns) {
            this.delayedReturns = new List<IDelayedReturn>(delayedReturns);
        }

        public IDelayedReturn this[int index] {
            get => delayedReturns[index];
            set => delayedReturns[index] = value;
        }

        public void StartAction() {
            if (!active) {
                active = true;
                curIndex = 0;
                TryStart();
            }
        }
        public void InterruptAction() {
            if (active) {
                active = false;
                delayedReturns[curIndex].InterruptAction();
            }
        }
        public void SkipAction() {
            for (int i = curIndex; i < delayedReturns.Count; i++) {
                delayedReturns[curIndex].SkipAction();
            }
        }
        public void PauseAction() {
            if (active) {
                delayedReturns[curIndex].PauseAction();
            }
        }
        public void ResumeAction() {
            if (active) {
                delayedReturns[curIndex].ResumeAction();
            }
        }
        private void Returned() {
            if (active) {
                onEachReturned?.Invoke(curIndex);
                curIndex++;
                TryStart();
            }
        }
        private void TryStart() {
            if (delayedReturns.IsValidIndex(curIndex)) {
                delayedReturns[curIndex].OnReturn = Returned;
                delayedReturns[curIndex].StartAction();
            }
            else {
                active = false;
                OnReturn.Invoke();
            }
        }

    }
}
