using System;
using UnityEngine;

namespace LunaLib {
    public interface IDelayedReturn {
        void StartAction();
        void InterruptAction();
        void SkipAction();
        void PauseAction();
        void ResumeAction();
        Action OnReturn { get; set; }
    }
}
