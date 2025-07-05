using System;
using UnityEngine;
using UnityEngine.Events;

namespace LunaLib {
    public class ScriptedEventCallback : ScriptedAction {
        public UnityEvent onStartAction;
        public UnityEvent onInterruptAction;
        public UnityEvent onSkipAction;
        public UnityEvent onPauseAction;
        public UnityEvent onResumeAction;
        private bool active = false;
        public override void StartAction() {
            if (!active) {
                active = true;
                onStartAction.Invoke();
            }
        }
        public override void InterruptAction() {
            if (active) {
                active = false;
                onInterruptAction?.Invoke();
            }
        }
        public override void SkipAction() {
            if (active) {
                active = false;
                onSkipAction?.Invoke();
                OnReturn?.Invoke();
            }
        }
        public override void PauseAction() {
            if (active) {
                onPauseAction?.Invoke();
            }
        }
        public override void ResumeAction() {
            if (active) {
                onResumeAction?.Invoke();
            }
        }
        public void Return() {
            if (active) {
                active = false;
                OnReturn?.Invoke();
            }
        }
    }
}
