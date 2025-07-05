using System;
using UnityEngine;

namespace LunaLib {
    public abstract class ScriptedAction : MonoBehaviour, IDelayedReturn {
        public Action OnReturn { get; set; }

        public abstract void StartAction();
        public abstract void InterruptAction();
        public abstract void SkipAction();
        public abstract void PauseAction();
        public abstract void ResumeAction();
    }
}
