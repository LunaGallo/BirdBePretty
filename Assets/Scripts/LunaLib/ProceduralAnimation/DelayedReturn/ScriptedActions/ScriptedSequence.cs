using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace LunaLib {
    public class ScriptedSequence : ScriptedAction {

        public List<ScriptedAction> steps;
        public IDelayedReturn AsSequence {
            get {
                if (sequence == null) {
                    sequence = new DelayedReturnSequence(steps.ToArray());
                }
                return sequence;
            }
        }
        public IDelayedReturn sequence;

        private void Start() => AsSequence.OnReturn = () => OnReturn.Invoke();
        public override void StartAction() => AsSequence.StartAction();
        public override void InterruptAction() => AsSequence.InterruptAction();
        public override void SkipAction() => AsSequence.SkipAction();
        public override void PauseAction() => AsSequence.PauseAction();
        public override void ResumeAction() => AsSequence.ResumeAction();

    }
}
