using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace LunaLib {
    public class ScriptedGroup : ScriptedAction {

        public List<ScriptedAction> parts;
        public IDelayedReturn AsGroup {
            get {
                if (group == null) {
                    group = new DelayedReturnGroup(parts.ToArray());
                }
                return group;
            }
        }
        public IDelayedReturn group;

        private void Start() => AsGroup.OnReturn = () => OnReturn.Invoke();
        public override void StartAction() => AsGroup.StartAction();
        public override void InterruptAction() => AsGroup.InterruptAction();
        public override void SkipAction() => AsGroup.SkipAction();
        public override void PauseAction() => AsGroup.PauseAction();
        public override void ResumeAction() => AsGroup.ResumeAction();

    }
}
